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
    internal sealed class SingleConsumerUnboundedChannel<T> : Channel<T>, IDebugEnumerable<T>
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

            In = new Readable(this);
            Out = new Writable(this);
        }

        private sealed class Readable : ReadableChannel<T>
        {
            internal readonly SingleConsumerUnboundedChannel<T> _parent;
            internal Readable(SingleConsumerUnboundedChannel<T> parent) { _parent = parent; }

            public override Task Completion => _parent.Completion;
            public override ValueAwaiter<T> GetAwaiter() => _parent.GetAwaiter();
            public override ValueTask<T> ReadAsync(CancellationToken cancellationToken) => _parent.ReadAsync(cancellationToken);
            public override bool TryRead(out T item) => _parent.TryRead(out item);
            public override Task<bool> WaitToReadAsync(CancellationToken cancellationToken) => _parent.WaitToReadAsync(cancellationToken);
        }

        private sealed class Writable : WritableChannel<T>
        {
            internal readonly SingleConsumerUnboundedChannel<T> _parent;
            internal Writable(SingleConsumerUnboundedChannel<T> parent) { _parent = parent; }

            public override bool TryComplete(Exception error) => _parent.TryComplete(error);
            public override bool TryWrite(T item) => _parent.TryWrite(item);
            public override Task<bool> WaitToWriteAsync(CancellationToken cancellationToken) => _parent.WaitToReadAsync(cancellationToken);
            public override Task WriteAsync(T item, CancellationToken cancellationToken) => _parent.WriteAsync(item, cancellationToken);
        }

        public override ReadableChannel<T> In { get; }
        public override WritableChannel<T> Out { get; }

        private object SyncObj => _items;

        private new Task Completion => _completion.Task;

        private new bool TryComplete(Exception error = null)
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
                ChannelUtilities.Complete(_completion, error);
            }

            Debug.Assert(blockedReader == null || waitingReader == null, "There should only ever be at most one reader.");

            // Complete a blocked reader if necessary
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

            // Complete a waiting reader if necessary.  (We really shouldn't have both a blockedReader
            // and a waitingReader, but it's more expensive to prevent it than to just tolerate it.)
            waitingReader?.Success(false);

            // Successfully completed the channel
            return true;
        }

        private new ValueAwaiter<T> GetAwaiter()
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
                // Now that we hold the lock, try reading again.
                T item;
                if (TryRead(out item))
                {
                    return new ValueAwaiter<T>(item);
                }

                // If no more items will be written, fail the read.
                if (_doneWriting != null)
                {
                    Exception e = _doneWriting != ChannelUtilities.DoneWritingSentinel ? _doneWriting : ChannelUtilities.CreateInvalidCompletionException();
                    return new ValueAwaiter<T>(Task.FromException<T>(e));
                }

                Debug.Assert(_blockedReader == null || ((_blockedReader as ReaderInteractor<T>)?.Task.IsCanceled ?? false),
                    "Incorrect usage; multiple outstanding reads were issued against this single-consumer channel");

                // Store the reader to be completed by a writer.
                _blockedReader = _awaiter ?? (_awaiter = new AutoResetAwaiter<T>(_runContinuationsAsynchronously));
                return new ValueAwaiter<T>(_awaiter);
            }
        }

        private new ValueTask<T> ReadAsync(CancellationToken cancellationToken = default(CancellationToken))
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
                // Now that we hold the lock, try reading again.
                T item;
                if (TryRead(out item))
                {
                    return new ValueTask<T>(item);
                }

                // If no more items will be written, fail the read.
                if (_doneWriting != null)
                {
                    Exception e = _doneWriting != ChannelUtilities.DoneWritingSentinel ? _doneWriting : ChannelUtilities.CreateInvalidCompletionException();
                    return new ValueTask<T>(Task.FromException<T>(e));
                }

                Debug.Assert(_blockedReader == null || ((_blockedReader as ReaderInteractor<T>)?.Task.IsCanceled ?? false),
                    "Incorrect usage; multiple outstanding reads were issued against this single-consumer channel");

                // Store the reader to be completed by a writer.
                ReaderInteractor<T> reader = ReaderInteractor<T>.Create(_runContinuationsAsynchronously, cancellationToken);
                _blockedReader = reader;
                return new ValueTask<T>(reader.Task);
            }
        }

        private new bool TryRead(out T item)
        {
            if (_items.TryDequeue(out item))
            {
                if (_doneWriting != null && _items.IsEmpty)
                {
                    ChannelUtilities.Complete(_completion, _doneWriting);
                }
                return true;
            }
            return false;
        }

        private new bool TryWrite(T item)
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

        private new Task<bool> WaitToReadAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Outside of the lock, check if there are any items waiting to be read.  If there are, we're done.
            if (!_items.IsEmpty)
            {
                return ChannelUtilities.TrueTask;
            }

            // Now check for cancellation.
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<bool>(cancellationToken);
            }

            ReaderInteractor<bool> oldWaiter = null, newWaiter;
            lock (SyncObj)
            {
                // Again while holding the lock, check to see if there are any items available.
                if (!_items.IsEmpty)
                {
                    return ChannelUtilities.TrueTask;
                }

                // There aren't any items; if we're done writing, there never will be more items.
                if (_doneWriting != null)
                {
                    return ChannelUtilities.FalseTask;
                }

                // Create the new waiter.  We're a bit more tolerant of a stray waiting reader
                // than we are of a blocked reader, as with usage patterns it's easier to leave one
                // behind, so we just cancel any that may have been waiting around.
                oldWaiter = _waitingReader;
                _waitingReader = newWaiter = ReaderInteractor<bool>.Create(_runContinuationsAsynchronously, cancellationToken);
            }

            oldWaiter?.TrySetCanceled();
            return newWaiter.Task;
        }

        private new Task<bool> WaitToWriteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return
                cancellationToken.IsCancellationRequested ? Task.FromCanceled<bool>(cancellationToken) :
                _doneWriting == null ? ChannelUtilities.TrueTask :
                ChannelUtilities.FalseTask;
        }

        private new Task WriteAsync(T item, CancellationToken cancellationToken = default(CancellationToken))
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
