// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;

namespace System.Threading.Tasks.Channels
{
    public static partial class Channel
    {
        /// <summary>Provides a buffered channel of unbounded capacity.</summary>
        [DebuggerDisplay("Items={ItemsCountForDebugger}")]
        [DebuggerTypeProxy(typeof(DebugEnumeratorDebugView<>))]
        private sealed class UnboundedChannel<T> : IChannel<T>, IDebugEnumerable<T>
        {
            /// <summary>Task that indicates the channel has completed.</summary>
            private readonly TaskCompletionSource<VoidResult> _completion = new TaskCompletionSource<VoidResult>(TaskCreationOptions.RunContinuationsAsynchronously);
            /// <summary>The items in the channel.</summary>
            private readonly SimpleQueue<T> _items = new SimpleQueue<T>();
            /// <summary>Readers blocked reading from the channel.</summary>
            private readonly SimpleQueue<Reader<T>> _blockedReaders = new SimpleQueue<Reader<T>>();
            /// <summary>Readers waiting for a notification that data is available.</summary>
            private readonly SimpleQueue<Reader<bool>> _waitingReaders = new SimpleQueue<Reader<bool>>();
            /// <summary>Set to non-null once Complete has been called.</summary>
            private Exception _doneWriting;

            /// <summary>Gets the object used to synchronize access to all state on this instance.</summary>
            private object SyncObj => _items;

            public Task Completion => _completion.Task;

            [Conditional("DEBUG")]
            private void AssertInvariants()
            {
                Debug.Assert(SyncObj != null, "The sync obj must not be null.");
                Debug.Assert(Monitor.IsEntered(SyncObj), "Invariants can only be validated while holding the lock.");

                if (_items.Count > 0)
                {
                    Debug.Assert(_blockedReaders.Count == 0, "There's data available, so there shouldn't be any blocked readers.");
                    Debug.Assert(_waitingReaders.Count == 0, "There's data available, so there shouldn't be any waiting readers.");
                    Debug.Assert(!_completion.Task.IsCompleted, "We still have data available, so shouldn't be completed.");
                }
                if (_blockedReaders.Count > 0 || _waitingReaders.Count > 0)
                {
                    Debug.Assert(_items.Count == 0, "There are blocked/waiting readers, so there shouldn't be any data available.");
                }
                if (_completion.Task.IsCompleted)
                {
                    Debug.Assert(_doneWriting != null, "We're completed, so we must be done writing.");
                }
            }

            public bool TryComplete(Exception error = null)
            {
                lock (SyncObj)
                {
                    AssertInvariants();

                    // Mark that we're done writing
                    if (_doneWriting != null)
                    {
                        return false;
                    }
                    _doneWriting = error ?? s_doneWritingSentinel;

                    // If there are no items in the queue, then we're done (there won't be any more coming).
                    if (_items.Count == 0)
                    {
                        CompleteWithOptionalError(_completion, error);
                    }

                    // If there are any blocked readers, then since we won't be getting any more
                    // data and there can't be any currently in the queue (or else they wouldn't
                    // be blocked), fail them all.
                    while (_blockedReaders.Count > 0)
                    {
                        var reader = _blockedReaders.Dequeue();
                        reader.Fail(error ?? CreateInvalidCompletionException());
                    }

                    // Similarly, if there are any waiting readers, let them know 
                    // no more data will be coming.
                    WakeUpWaiters(_waitingReaders, false);
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

            public ValueTask<T> ReadAsyncCore(CancellationToken cancellationToken)
            {
                if (cancellationToken.IsCancellationRequested)
                    return new ValueTask<T>(Task.FromCanceled<T>(cancellationToken));

                lock (SyncObj)
                {
                    AssertInvariants();

                    // If there are any items, return one.
                    if (_items.Count > 0)
                    {
                        // Dequeue an item
                        T item = _items.Dequeue();
                        if (_doneWriting != null && _items.Count == 0)
                        {
                            // If we've now emptied the items queue and we're not getting any more, complete.
                            CompleteWithOptionalError(_completion, _doneWriting);
                        }

                        return new ValueTask<T>(item);
                    }

                    // There are no items, so if we're done writing, fail.
                    if (_doneWriting != null)
                        return new ValueTask<T>(Task.FromException<T>(_doneWriting != s_doneWritingSentinel ? _doneWriting : CreateInvalidCompletionException()));

                    // Otherwise, queue the reader.
                    var reader = Reader<T>.Create(cancellationToken);
                    _blockedReaders.Enqueue(reader);
                    return new ValueTask<T>(reader.Task);
                }
            }

            public Task<bool> WaitToReadAsync(CancellationToken cancellationToken)
            {
                lock (SyncObj)
                {
                    AssertInvariants();

                    // If there are any items, readers can try to get them.
                    if (_items.Count > 0)
                        return s_trueTask;
                    
                    // There are no items, so if we're done writing, there's never going to be data available.
                    if (_doneWriting != null)
                        return s_falseTask;

                    // Queue the waiter
                    var r = Reader<bool>.Create(cancellationToken);
                    _waitingReaders.Enqueue(r);
                    return r.Task;
                }
            }

            public bool TryRead(out T item)
            {
                SpinWait spinner = default(SpinWait);
                do
                {
                    lock (SyncObj)
                    {
                        AssertInvariants();

                        // Dequeue an item if we can
                        if (_items.Count > 0)
                        {
                            item = _items.Dequeue();
                            if (_doneWriting != null && _items.Count == 0)
                            {
                                // If we've now emptied the items queue and we're not getting any more, complete.
                                CompleteWithOptionalError(_completion, _doneWriting);
                            }
                            return true;
                        }
                    }
                    spinner.SpinOnce();
                }
                while (!spinner.NextSpinWillYield);

                item = default(T);
                return false;
            }

            public bool TryWrite(T item)
            {
                lock (SyncObj)
                {
                    AssertInvariants();

                    // If writing has already been marked as done, fail the write.
                    if (_doneWriting != null)
                        return false;

                    // If there are any blocked readers, wake one of them up to transfer
                    // the data to.  Note that readers may be canceled but still be in the 
                    // queue, so we need to loop.
                    while (_blockedReaders.Count > 0)
                    {
                        Reader<T> r = _blockedReaders.Dequeue();
                        if (r.Success(item))
                        {
                            return true;
                        }
                    }

                    // There were no blocked readers, so just add the data to the queue
                    _items.Enqueue(item);

                    // Then let any waiting readers know that they should try to read it.
                    WakeUpWaiters(_waitingReaders, true);

                    return true;
                }
            }

            public Task<bool> WaitToWriteAsync(CancellationToken cancellationToken)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return Task.FromCanceled<bool>(cancellationToken);
                }

                // Other than for cancellation, writing can always be done if we haven't completed, as we're unbounded.
                lock (SyncObj)
                {
                    return _doneWriting == null ? s_trueTask : s_falseTask;
                }
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
