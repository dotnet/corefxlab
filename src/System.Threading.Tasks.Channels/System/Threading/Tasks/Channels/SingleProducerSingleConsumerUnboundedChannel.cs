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
    /// Provides a buffered channel of unbounded capacity for use by only
    /// a single producer and a single consumer at a time.
    /// </summary>
    [DebuggerDisplay("Items={ItemsCountForDebugger}")]
    [DebuggerTypeProxy(typeof(DebugEnumeratorDebugView<>))]
    internal sealed class SingleProducerSingleConsumerUnboundedChannel<T> : IChannel<T>, IDebugEnumerable<T>
    {
        /// <summary>Task that indicates the channel has completed.</summary>
        private readonly TaskCompletionSource<VoidResult> _completion = new TaskCompletionSource<VoidResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        private readonly SingleProducerSingleConsumerQueue<T> _items = new SingleProducerSingleConsumerQueue<T>();

        private AutoResetAwaiter<T> _awaiter;
        private volatile Exception _doneWriting;
        private object _blockedReader; // ReaderInteractor<T> or AutoResetAwaiter<T>
        private ReaderInteractor<bool> _waitingReader;

        /// <summary>Initialize the channel.</summary>
        internal SingleProducerSingleConsumerUnboundedChannel() { }

        private object SyncObj => _items;

        public Task Completion => _completion.Task;

        public bool TryComplete(Exception error = null)
        {
            lock (SyncObj)
            {
                if (_doneWriting != null)
                {
                    return false;
                }
                _doneWriting = error ?? ChannelUtilities.DoneWritingSentinel;

                if (_items.IsEmpty)
                {
                    ChannelUtilities.CompleteWithOptionalError(_completion, error);
                    if (_blockedReader != null)
                    {
                        if (error == null)
                        {
                            error = ChannelUtilities.CreateInvalidCompletionException();
                        }
                        ReaderInteractor<T> interactor = _blockedReader as ReaderInteractor<T>;
                        if (interactor != null)
                        {
                            interactor.Fail(error);
                        }
                        else
                        {
                            ((AutoResetAwaiter<T>)_blockedReader).SetException(error);
                        }
                        _blockedReader = null;
                    }
                    if (_waitingReader != null)
                    {
                        _waitingReader.Success(false);
                        _waitingReader = null;
                    }
                }
            }

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
                    _awaiter = new AutoResetAwaiter<T>();
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
            lock (SyncObj)
            {
                T item;
                if (TryRead(out item))
                {
                    return new ValueTask<T>(item);
                }

                if (_doneWriting != null || _blockedReader != null)
                {
                    Exception e = _doneWriting != null ?
                        _doneWriting != ChannelUtilities.DoneWritingSentinel ? _doneWriting : ChannelUtilities.CreateInvalidCompletionException() :
                        ChannelUtilities.CreateSingleReaderWriterMisuseException();
                    return new ValueTask<T>(Task.FromException<T>(e));
                }

                ReaderInteractor<T> reader = ReaderInteractor<T>.Create(cancellationToken);
                _blockedReader = reader;
                return new ValueTask<T>(reader.Task);
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
            lock (SyncObj)
            {
                object b = _blockedReader;
                if (b != null)
                {
                    _blockedReader = null;

                    ReaderInteractor<T> interactor = b as ReaderInteractor<T>;
                    if (interactor != null)
                    {
                        bool success = interactor.Success(item);
                        Debug.Assert(success, "Reader should not have been already completed");
                    }
                    else
                    {
                        ((AutoResetAwaiter<T>)b).SetResult(item);
                    }
                    return true;
                }

                if (_waitingReader != null)
                {
                    _waitingReader.Success(true);
                    _waitingReader = null;
                }

                if (_doneWriting != null)
                {
                    return false;
                }

                _items.Enqueue(item);
                return true;
            }
        }

        public Task<bool> WaitToReadAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!_items.IsEmpty)
            {
                return ChannelUtilities.TrueTask;
            }

            lock (SyncObj)
            {
                if (!_items.IsEmpty)
                    return ChannelUtilities.TrueTask;

                if (_waitingReader != null)
                {
                    _waitingReader.Fail(ChannelUtilities.CreateSingleReaderWriterMisuseException());
                    _waitingReader = null;
                    return Task.FromException<bool>(ChannelUtilities.CreateSingleReaderWriterMisuseException());
                }

                if (_doneWriting != null)
                    return ChannelUtilities.FalseTask;

                ReaderInteractor<bool> reader = ReaderInteractor<bool>.Create(cancellationToken);
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
