// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;

namespace System.Threading.Tasks.Channels
{
    public static partial class Channel
    {
        /// <summary>Provides a channel with a bounded capacity.</summary>
        [DebuggerDisplay("Items={ItemsCountForDebugger}, Capacity={_bufferedCapacity}")]
        [DebuggerTypeProxy(typeof(DebugEnumeratorDebugView<>))]
        private sealed class BoundedChannel<T> : IChannel<T>, IDebugEnumerable<T>
        {
            /// <summary>Task signaled when the channel has completed.</summary>
            private readonly TaskCompletionSource<VoidResult> _completion = new TaskCompletionSource<VoidResult>(TaskCreationOptions.RunContinuationsAsynchronously);
            /// <summary>The maximum capacity of the channel.</summary>
            private readonly int _bufferedCapacity;
            /// <summary>Items currently stored in the channel waiting to be read.</summary>
            private readonly SimpleQueue<T> _items = new SimpleQueue<T>();
            /// <summary>Readers waiting to read from the channel.</summary>
            private readonly SimpleQueue<Reader<T>> _blockedReaders = new SimpleQueue<Reader<T>>();
            /// <summary>Writers waiting to write to the channel.</summary>
            private readonly SimpleQueue<Writer<T>> _blockedWriters = new SimpleQueue<Writer<T>>();
            /// <summary>Task signaled when any WaitToReadAsync waiters should be woken up.</summary>
            private readonly SimpleQueue<Reader<bool>> _waitingReaders = new SimpleQueue<Reader<bool>>();
            /// <summary>Task signaled when any WaitToWriteAsync waiters should be woken up.</summary>
            private readonly SimpleQueue<Reader<bool>> _waitingWriters = new SimpleQueue<Reader<bool>>();
            /// <summary>Set to non-null once Complete has been called.</summary>
            private Exception _doneWriting;
            /// <summary>Gets an object used to synchronize all state on the instance.</summary>
            private object SyncObj => _items;

            /// <summary>Initializes the <see cref="BoundedChannel{T}"/>.</summary>
            /// <param name="bufferedCapacity">The positive bounded capacity for the channel.</param>
            internal BoundedChannel(int bufferedCapacity)
            {
                Debug.Assert(bufferedCapacity > 0);
                _bufferedCapacity = bufferedCapacity;
            }

            public Task Completion => _completion.Task;
            
            [Conditional("DEBUG")]
            private void AssertInvariants()
            {
                Debug.Assert(SyncObj != null, "The sync obj must not be null.");
                Debug.Assert(Monitor.IsEntered(SyncObj), "Invariants can only be validated while holding the lock.");

                if (_items.Count > 0)
                {
                    Debug.Assert(_blockedReaders.Count == 0, "There are items available, so there shouldn't be any blocked readers.");
                    Debug.Assert(_waitingReaders.Count == 0, "There are items available, so there shouldn't be any waiting readers.");
                }
                if (_items.Count < _bufferedCapacity)
                {
                    Debug.Assert(_blockedWriters.Count == 0, "There's space available, so there shouldn't be any blocked writers.");
                    Debug.Assert(_waitingWriters.Count == 0, "There's space available, so there shouldn't be any waiting writers.");
                }
                if (_blockedReaders.Count > 0)
                {
                    Debug.Assert(_items.Count == 0, "There shouldn't be queued items if there's a blocked reader.");
                    Debug.Assert(_blockedWriters.Count == 0, "There shouldn't be any blocked writer if there's a blocked reader.");
                }
                if (_blockedWriters.Count > 0)
                {
                    Debug.Assert(_items.Count == _bufferedCapacity, "We should have a full buffer if there's a blocked writer.");
                    Debug.Assert(_blockedReaders.Count == 0, "There shouldn't be any blocked readers if there's a blocked writer.");
                }
                if (_doneWriting != null || _completion.Task.IsCompleted)
                {
                    Debug.Assert(_blockedWriters.Count == 0, "We're done writing, so there shouldn't be any blocked writers.");
                    Debug.Assert(_blockedReaders.Count == 0, "We're done writing, so there shouldn't be any blocked readers.");
                    Debug.Assert(_waitingReaders.Count == 0, "We're done writing, so any reader should have woken up.");
                    Debug.Assert(_waitingWriters.Count == 0, "We're done writing, so any writer should have woken up.");
                }
                if (_completion.Task.IsCompleted)
                {
                    Debug.Assert(_doneWriting != null, "We can only complete if we're done writing.");
                }
            }

            public bool TryComplete(Exception error = null)
            {
                lock (SyncObj)
                {
                    AssertInvariants();

                    // Mark the channel as done for writing.  This may only be done once.
                    if (_doneWriting != null)
                    {
                        return false;
                    }
                    _doneWriting = error ?? s_doneWritingSentinel;

                    // If there are no items in the channel, then there's no more work to be done,
                    // so we complete the completion task.
                    if (_items.Count == 0)
                    {
                        CompleteWithOptionalError(_completion, error);
                    }

                    // If there are any waiting readers, fail them all, as they'll now never be satisfied.
                    while (_blockedReaders.Count > 0)
                    {
                        var reader = _blockedReaders.Dequeue();
                        reader.Fail(error ?? CreateInvalidCompletionException());
                    }

                    // If there are any waiting writers, fail them all, as they shouldn't be writing
                    // now that we're complete for writing.
                    while (_blockedWriters.Count > 0)
                    {
                        var writer = _blockedWriters.Dequeue();
                        writer.Fail(CreateInvalidCompletionException());
                    }

                    // If there are any pending WaitToRead/WriteAsync calls, wake them up.
                    WakeUpWaiters(_waitingReaders, false);
                    WakeUpWaiters(_waitingWriters, false);
                }

                return true;
            }

            public ValueTask<T> ReadAsync(CancellationToken cancellationToken)
            {
                // Fast-path cancellation check
                if (cancellationToken.IsCancellationRequested)
                    return new ValueTask<T>(Task.FromCanceled<T>(cancellationToken));

                lock (SyncObj)
                {
                    AssertInvariants();

                    // If there are any items, hand one back.
                    if (_items.Count > 0)
                    {
                        return new ValueTask<T>(DequeueItemAndPostProcess());
                    }

                    // There weren't any items.  If we're done writing so that there
                    // will never be more items, fail.
                    if (_doneWriting != null)
                    {
                        return new ValueTask<T>(Task.FromException<T>(_doneWriting != s_doneWritingSentinel ? _doneWriting : CreateInvalidCompletionException()));
                    }

                    // Otherwise, queue the reader.
                    var reader = Reader<T>.Create(cancellationToken);
                    _blockedReaders.Enqueue(reader);
                    return new ValueTask<T>(reader.Task);
                }
            }

            public Task<bool> WaitToReadAsync(CancellationToken cancellationToken)
            {
                if (cancellationToken.IsCancellationRequested)
                    return Task.FromCanceled<bool>(cancellationToken);

                lock (SyncObj)
                {
                    AssertInvariants();

                    // If there are any items available, a read is possible.
                    if (_items.Count > 0)
                    {
                        return s_trueTask;
                    }

                    // There were no items available, so if we're done writing, a read will never be possible.
                    if (_doneWriting != null)
                    {
                        return s_falseTask;
                    }

                    // There were no items available, but there could be in the future, so ensure
                    // there's a blocked reader task and return it.
                    var r = Reader<bool>.Create(cancellationToken);
                    _waitingReaders.Enqueue(r);
                    return r.Task;
                }
            }

            public bool TryRead(out T item)
            {
                lock (SyncObj)
                {
                    AssertInvariants();

                    // Get an item if there is one.
                    if (_items.Count > 0)
                    {
                        item = DequeueItemAndPostProcess();
                        return true;
                    }
                }
                item = default(T);
                return false;
            }

            /// <summary>Dequeues an item, and then fixes up our state around writers and completion.</summary>
            /// <returns>The dequeued item.</returns>
            private T DequeueItemAndPostProcess()
            {
                Debug.Assert(Monitor.IsEntered(SyncObj));

                // Dequeue an item.
                T item = _items.Dequeue();

                // If we're now empty and we're done writing, complete the channel.
                if (_doneWriting != null && _items.Count == 0)
                {
                    CompleteWithOptionalError(_completion, _doneWriting);
                }

                // If there are any writers blocked, there's now room for at least one
                // to be promoted to have its item moved into the items queue.  We need
                // to loop while trying to complete the writer in order to find one that
                // hasn't yet been canceled (canceled writers transition to canceled but
                // remain in the physical queue).
                while (_blockedWriters.Count > 0)
                {
                    Writer<T> w = _blockedWriters.Dequeue();
                    if (w.Success(default(VoidResult)))
                    {
                        _items.Enqueue(w.Item);
                        return item;
                    }
                }

                // There was no blocked writer, so see if there's a WaitToWriteAsync
                // we should wake up.
                WakeUpWaiters(_waitingWriters, true);

                // Return the item
                return item;
            }

            public bool TryWrite(T item)
            {
                lock (SyncObj)
                {
                    AssertInvariants();

                    // If we're done writing, nothing more to do.
                    if (_doneWriting != null)
                    {
                        return false;
                    }

                    // If there are any blocked readers, find one that's not canceled
                    // and complete it with the item from this writer.
                    while (_blockedReaders.Count > 0)
                    {
                        Reader<T> r = _blockedReaders.Dequeue();
                        if (r.Success(item))
                        {
                            return true;
                        }
                    }

                    // If we're full, we can't write.
                    if (_items.Count == _bufferedCapacity)
                    {
                        return false;
                    }

                    // There weren't any blocked (and non-canceled) readers, and
                    // there's room in the queue.  Queue item, and let any waiting 
                    // readers know they could try to read.
                    _items.Enqueue(item);
                    WakeUpWaiters(_waitingReaders, true);
                    return true;
                }
            }

            public Task<bool> WaitToWriteAsync(CancellationToken cancellationToken)
            {
                if (cancellationToken.IsCancellationRequested)
                    return Task.FromCanceled<bool>(cancellationToken);

                lock (SyncObj)
                {
                    AssertInvariants();

                    // If we're done writing, no writes will ever succeed.
                    if (_doneWriting != null)
                    {
                        return s_falseTask;
                    }

                    // If there's space to write, a write is possible.
                    if (_items.Count < _bufferedCapacity)
                        return s_trueTask;

                    // We're still allowed to write, but there's no space,
                    // so ensure a waiter is queued and return it.
                    var w = Reader<bool>.Create(cancellationToken);
                    _waitingWriters.Enqueue(w);
                    return w.Task;
                }
            }

            public Task WriteAsync(T item, CancellationToken cancellationToken)
            {
                if (cancellationToken.IsCancellationRequested)
                    return Task.FromCanceled(cancellationToken);

                lock (SyncObj)
                {
                    AssertInvariants();

                    // If we're done writing, trying to write is an error.
                    if (_doneWriting != null)
                    {
                        throw CreateInvalidCompletionException();
                    }

                    // If there are any blocked readers, find a non-canceled
                    // one to transfer the item to.
                    while (_blockedReaders.Count > 0)
                    {
                        Reader<T> r = _blockedReaders.Dequeue();
                        if (r.Success(item))
                        {
                            return s_trueTask;
                        }
                    }

                    // There were no available readers.  If there's
                    // room, simply store the item.
                    if (_items.Count < _bufferedCapacity)
                    {
                        _items.Enqueue(item);
                        WakeUpWaiters(_waitingReaders, true);
                        return s_trueTask;
                    }

                    // There were no readers and there was no room.
                    // Queue the writer.
                    var writer = Writer<T>.Create(cancellationToken, item);
                    _blockedWriters.Enqueue(writer);
                    return writer.Task;
                }
            }

            /// <summary>Gets the number of items in the channel.  This should only be used by the debugger.</summary>
            private int ItemsCountForDebugger => _items.Count;

            /// <summary>Gets an enumerator the debugger can use to show the contents of the channel.</summary>
            IEnumerator<T> IDebugEnumerable<T>.GetEnumerator() => _items.GetEnumerator();
        }
    }
}
