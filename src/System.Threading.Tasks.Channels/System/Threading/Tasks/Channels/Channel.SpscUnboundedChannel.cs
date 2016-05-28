// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Threading.Tasks.Channels
{
    public static partial class Channel
    {
        /// <summary>Provides a buffered channel of unbounded capacity.</summary>
        [DebuggerDisplay("Items={ItemsCountForDebugger}")]
        [DebuggerTypeProxy(typeof(DebugEnumeratorDebugView<>))]
        private sealed class SpscUnboundedChannel<T> : IChannel<T>, IDebugEnumerable<T>
        {
            /// <summary>Task that indicates the channel has completed.</summary>
            private readonly TaskCompletionSource<VoidResult> _completion = new TaskCompletionSource<VoidResult>(TaskCreationOptions.RunContinuationsAsynchronously);
            private readonly SingleProducerSingleConsumerQueue<T> _items = new SingleProducerSingleConsumerQueue<T>();

            private volatile Exception _doneWriting;
            private Reader<T> _blockedReader;
            private Reader<bool> _waitingReader;

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
                    _doneWriting = error ?? s_doneWritingSentinel;

                    if (_items.IsEmpty)
                    {
                        CompleteWithOptionalError(_completion, error);
                        if (_blockedReader != null)
                        {
                            _blockedReader.Fail(error ?? CreateInvalidCompletionException());
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

            public ValueTask<T> ReadAsync(CancellationToken cancellationToken)
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

                    if (TryReadCore(out item))
                        return new ValueTask<T>(item);

                    if (_doneWriting != null)
                        return new ValueTask<T>(Task.FromException<T>(_doneWriting != s_doneWritingSentinel ? _doneWriting : CreateInvalidCompletionException()));

                    if (_blockedReader != null)
                        return new ValueTask<T>(Task.FromException<T>(CreateSingleReaderWriterMisuseException()));

                    Reader<T> reader = Reader<T>.Create(cancellationToken);
                    _blockedReader = reader;
                    return new ValueTask<T>(reader.Task);
                }
            }

            public bool TryRead(out T item)
            {
                SpinWait spinner = default(SpinWait);
                do
                {
                    if (TryReadCore(out item))
                    {
                        return true;
                    }
                    spinner.SpinOnce();
                }
                while (!spinner.NextSpinWillYield && _doneWriting == null);
                return false;
            }

            private bool TryReadCore(out T item)
            {
                if (_items.TryDequeue(out item))
                {
                    if (_doneWriting != null && _items.IsEmpty)
                    {
                        CompleteWithOptionalError(_completion, _doneWriting);
                    }
                    return true;
                }
                return false;
            }

            public bool TryWrite(T item)
            {
                lock (SyncObj)
                {
                    var b = _blockedReader;
                    if (b != null)
                    {
                        _blockedReader = null;
                        if (b.Success(item))
                        {
                            return true;
                        }
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

            public Task<bool> WaitToReadAsync(CancellationToken cancellationToken)
            {
                var spinner = default(SpinWait);
                do
                {
                    if (!_items.IsEmpty)
                    {
                        return s_trueTask;
                    }
                    spinner.SpinOnce();
                }
                while (!spinner.NextSpinWillYield);

                lock (SyncObj)
                {
                    if (!_items.IsEmpty)
                        return s_trueTask;

                    if (_waitingReader != null)
                    {
                        _waitingReader.Fail(CreateSingleReaderWriterMisuseException());
                        _waitingReader = null;
                        return Task.FromException<bool>(CreateSingleReaderWriterMisuseException());
                    }

                    if (_doneWriting != null)
                        return s_falseTask;

                    Reader<bool> reader = Reader<bool>.Create(cancellationToken);
                    _waitingReader = reader;
                    return reader.Task;
                }
            }

            public Task<bool> WaitToWriteAsync(CancellationToken cancellationToken)
            {
                return
                    cancellationToken.IsCancellationRequested ? Task.FromCanceled<bool>(cancellationToken) :
                    _doneWriting == null ? s_trueTask :
                    s_falseTask;
            }

            public Task WriteAsync(T item, CancellationToken cancellationToken)
            {
                // Writing always succeeds (unless we've already completed writing or cancellation has been requested),
                // so just TryWrite and return a completed task.
                return
                    cancellationToken.IsCancellationRequested ? Task.FromCanceled(cancellationToken) :
                    TryWrite(item) ? Task.CompletedTask :
                    Task.FromException(CreateInvalidCompletionException());
            }

            /// <summary>Gets the number of items in the channel.  This should only be used by the debugger.</summary>
            private int ItemsCountForDebugger => _items.Count;

            /// <summary>Gets an enumerator the debugger can use to show the contents of the channel.</summary>
            IEnumerator<T> IDebugEnumerable<T>.GetEnumerator() => _items.GetEnumerator();
        }
    }
}