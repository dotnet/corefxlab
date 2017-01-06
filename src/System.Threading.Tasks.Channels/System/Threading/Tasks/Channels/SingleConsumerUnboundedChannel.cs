// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks.Channels
{
    /// <summary>
    /// Provides a buffered channel of unbounded capacity for use by any number
    /// of writers but at most a single reader at a time.
    /// </summary>
    [DebuggerDisplay("Items={ItemsCountForDebugger}")]
    [DebuggerTypeProxy(typeof(DebugEnumeratorDebugView<>))]
    internal sealed class SingleConsumerUnboundedChannel<T> : IChannel<T>, IDebugEnumerable<T>
    {
        /// <summary>Task that indicates the channel has completed.</summary>
        private readonly TaskCompletionSource<VoidResult> _completion;
        /// <summary>
        /// A concurrent queue to hold the items for this channel.  The queue itself supports at most
        /// one writer and one reader at a time; as a result, since this channel supports multiple writers,
        /// all write access to the queue must be synchronized by the channel.
        /// </summary>
        private readonly SingleProducerSingleConsumerQueue<T> _items = new SingleProducerSingleConsumerQueue<T>();
        /// <summary>Whether to force continuations to be executed asynchronously from producer writes.</summary>
        private readonly bool _runContinuationsAsynchronously;

        /// <summary>A cached awaiter used when awaiting this channel.</summary>
        private AutoResetAwaiter<T> _awaiter;
        /// <summary>non-null if the channel has been marked as complete for writing.</summary>
        private volatile Exception _doneWriting;
        /// <summary>A <see cref="ReaderInteractor{T}"/> or <see cref="AutoResetAwaiter{TResult}"/> if there's a blocked reader.</summary>
        private object _blockedReader;
        /// <summary>A waiting reader (e.g. WaitForReadAsync) if there is one.</summary>
        private ReaderInteractor<bool> _waitingReader;

        /// <summary>Initialize the channel.</summary>
        /// <param name="runContinuationsAsynchronously">Whether to force continuations to be executed asynchronously.</param>
        internal SingleConsumerUnboundedChannel(bool runContinuationsAsynchronously)
        {
            _runContinuationsAsynchronously = runContinuationsAsynchronously;
            _completion = new TaskCompletionSource<VoidResult>(runContinuationsAsynchronously ? TaskCreationOptions.RunContinuationsAsynchronously : TaskCreationOptions.None);
        }

        private object SyncObj => _items;

        public Task Completion => _completion.Task;

        public bool TryComplete(Exception error = null)
        {
            object blockedReader = null;
            ReaderInteractor<bool> waitingReader = null;
            bool completeTask = false;

            lock (SyncObj)
            {
                // If we're already marked as complete, there's nothing more to do.
                if (_doneWriting != null)
                {
                    return false;
                }

                // Mark as complete for writing.
                _doneWriting = error ?? ChannelUtilities.DoneWritingSentinel;

                // If we have no more items remaining, then the channel needs to be marked as completed
                // and readers need to be informed they'll never get another item.  All of that needs
                // to happen outside of the lock to avoid invoking continuations under the lock.
                if (_items.IsEmpty)
                {
                    completeTask = true;

                    if (_blockedReader != null)
                    {
                        blockedReader = _blockedReader;
                        _blockedReader = null;
                    }

                    if (_waitingReader != null)
                    {
                        waitingReader = _waitingReader;
                        _waitingReader = null;
                    }
                }
            }

            // Complete the channel task if necessary
            if (completeTask)
            {
                ChannelUtilities.CompleteWithOptionalError(_completion, error);
            }

            Debug.Assert(blockedReader == null || waitingReader == null, "There should only ever be at most one reader.");

            // Complete a blocked/waiting reader if necessary
            if (blockedReader != null)
            {
                if (error == null)
                {
                    error = ChannelUtilities.CreateInvalidCompletionException();
                }

                ReaderInteractor<T> interactor = blockedReader as ReaderInteractor<T>;
                if (interactor != null)
                {
                    interactor.Fail(error);
                }
                else
                {
                    ((AutoResetAwaiter<T>)blockedReader).SetException(error);
                }
            }
            else if (waitingReader != null)
            {
                waitingReader.Success(false);
            }

            // Successfully completed the channel
            return true;
        }

        public ValueAwaiter<T> GetAwaiter()
        {
            T item;
            return TryRead(out item) ?
                new ValueAwaiter<T>(item) :
                GetAwaiterCore();
        }

        private ValueAwaiter<T> GetAwaiterCore()
        {
            lock (SyncObj)
            {
                T item;
                if (TryRead(out item))
                {
                    return new ValueAwaiter<T>(item);
                }

                if (_doneWriting != null || _blockedReader != null)
                {
                    Exception e = _doneWriting != null ?
                        _doneWriting != ChannelUtilities.DoneWritingSentinel ? _doneWriting : ChannelUtilities.CreateInvalidCompletionException() :
                        ChannelUtilities.CreateSingleReaderWriterMisuseException();
                    return new ValueAwaiter<T>(Task.FromException<T>(e));
                }

                if (_awaiter == null)
                {
                    _awaiter = new AutoResetAwaiter<T>(_runContinuationsAsynchronously);
                }

                _blockedReader = _awaiter;
                return new ValueAwaiter<T>(_awaiter);
            }
        }

        public ValueTask<T> ReadAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            T item;
            return TryRead(out item) ?
                new ValueTask<T>(item) :
                ReadAsyncCore(cancellationToken);
        }

        private ValueTask<T> ReadAsyncCore(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return new ValueTask<T>(Task.FromCanceled<T>(cancellationToken));
            }

            lock (SyncObj)
            {
                T item;
                if (TryRead(out item))
                {
                    return new ValueTask<T>(item);
                }

                if (_doneWriting != null || HasBlockedReader || HasWaitingReader)
                {
                    Exception e = _doneWriting != null ?
                        _doneWriting != ChannelUtilities.DoneWritingSentinel ? _doneWriting : ChannelUtilities.CreateInvalidCompletionException() :
                        ChannelUtilities.CreateSingleReaderWriterMisuseException();
                    return new ValueTask<T>(Task.FromException<T>(e));
                }

                ReaderInteractor<T> reader = ReaderInteractor<T>.Create(_runContinuationsAsynchronously, cancellationToken);
                _blockedReader = reader;
                return new ValueTask<T>(reader.Task);
            }
        }

        private bool HasBlockedReader
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                Debug.Assert(Monitor.IsEntered(SyncObj));

                object br = _blockedReader;
                if (br == null) return false;

                ReaderInteractor<T> ri = br as ReaderInteractor<T>;
                return ri != null && !ri.Task.IsCanceled;
            }
        }

        private bool HasWaitingReader
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                Debug.Assert(Monitor.IsEntered(SyncObj));
                ReaderInteractor<bool> wr = _waitingReader;
                return wr != null && !wr.Task.IsCanceled;
            }
        }

        public bool TryRead(out T item)
        {
            if (_items.TryDequeue(out item))
            {
                if (_doneWriting != null && _items.IsEmpty)
                {
                    ChannelUtilities.CompleteWithOptionalError(_completion, _doneWriting);
                }
                return true;
            }
            return false;
        }

        public bool TryWrite(T item)
        {
            while (true) // in case a reader was canceled and we need to try again
            {
                object blockedReader = null;
                ReaderInteractor<bool> waitingReader = null;

                lock (SyncObj)
                {
                    // If writing is completed, exit out without writing.
                    if (_doneWriting != null)
                    {
                        return false;
                    }

                    // If there's a blocked reader, store it into a local for completion outside of the lock.
                    // If there isn't a blocked reader, queue the item being written; then if there's a waiting
                    // reader, store it for notification outside of the lock.
                    blockedReader = _blockedReader;
                    if (blockedReader != null)
                    {
                        _blockedReader = null;
                    }
                    else
                    {
                        _items.Enqueue(item);

                        if (_waitingReader == null)
                        {
                            return true;
                        }

                        waitingReader = _waitingReader;
                        _waitingReader = null;
                    }
                }

                // If we get here, we grabbed a blocked or a waiting reader.
                Debug.Assert((blockedReader != null) ^ (waitingReader != null), "Expected either a blocked or waiting reader, but not both");

                // If we have a waiting reader, notify it that an item was written and exit.
                if (waitingReader != null)
                {
                    waitingReader.Success(true);
                    return true;
                }

                // Otherwise we have a blocked reader: complete it with the item being written.
                // In the case of a ReadAsync(CancellationToken), it's possible the reader could
                // have been completed due to cancellation by the time we get here.  In that case,
                // we'll loop around to try again so as not to lose the item being written.
                Debug.Assert(blockedReader != null);
                ReaderInteractor<T> interactor = blockedReader as ReaderInteractor<T>;
                if (interactor != null)
                {
                    if (interactor.Success(item))
                    {
                        return true;
                    }
                }
                else
                {
                    ((AutoResetAwaiter<T>)blockedReader).SetResult(item);
                    return true;
                }
            }
        }

        public Task<bool> WaitToReadAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<bool>(cancellationToken);
            }

            if (!_items.IsEmpty)
            {
                return ChannelUtilities.TrueTask;
            }

            lock (SyncObj)
            {
                if (!_items.IsEmpty)
                {
                    return ChannelUtilities.TrueTask;
                }

                if (HasWaitingReader || HasBlockedReader)
                {
                    return Task.FromException<bool>(ChannelUtilities.CreateSingleReaderWriterMisuseException());
                }

                if (_doneWriting != null)
                {
                    return ChannelUtilities.FalseTask;
                }

                ReaderInteractor<bool> reader = ReaderInteractor<bool>.Create(_runContinuationsAsynchronously, cancellationToken);
                _waitingReader = reader;
                return reader.Task;
            }
        }

        public Task<bool> WaitToWriteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return
                cancellationToken.IsCancellationRequested ? Task.FromCanceled<bool>(cancellationToken) :
                _doneWriting == null ? ChannelUtilities.TrueTask :
                ChannelUtilities.FalseTask;
        }

        public Task WriteAsync(T item, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Writing always succeeds (unless we've already completed writing or cancellation has been requested),
            // so just TryWrite and return a completed task.
            return
                cancellationToken.IsCancellationRequested ? Task.FromCanceled(cancellationToken) :
                TryWrite(item) ? Task.CompletedTask :
                Task.FromException(ChannelUtilities.CreateInvalidCompletionException());
        }

        /// <summary>Gets the number of items in the channel.  This should only be used by the debugger.</summary>
        private int ItemsCountForDebugger => _items.Count;

        /// <summary>Gets an enumerator the debugger can use to show the contents of the channel.</summary>
        IEnumerator<T> IDebugEnumerable<T>.GetEnumerator() => _items.GetEnumerator();
    }
}
