// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks.Channels
{
    /// <summary>Provides a channel with a bounded capacity.</summary>
    [DebuggerDisplay("Items={ItemsCountForDebugger}, Capacity={_bufferedCapacity}")]
    [DebuggerTypeProxy(typeof(DebugEnumeratorDebugView<>))]
    internal sealed class BoundedChannel<T> : IChannel<T>, IDebugEnumerable<T>
    {
        /// <summary>The mode used when the channel hits its bound.</summary>
        private readonly BoundedChannelFullMode _mode;
        /// <summary>Task signaled when the channel has completed.</summary>
        private readonly TaskCompletionSource<VoidResult> _completion = new TaskCompletionSource<VoidResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        /// <summary>The maximum capacity of the channel.</summary>
        private readonly int _bufferedCapacity;
        /// <summary>Items currently stored in the channel waiting to be read.</summary>
        private readonly Dequeue<T> _items = new Dequeue<T>();
        /// <summary>Readers waiting to read from the channel.</summary>
        private readonly Dequeue<ReaderInteractor<T>> _blockedReaders = new Dequeue<ReaderInteractor<T>>();
        /// <summary>Writers waiting to write to the channel.</summary>
        private readonly Dequeue<WriterInteractor<T>> _blockedWriters = new Dequeue<WriterInteractor<T>>();
        /// <summary>Task signaled when any WaitToReadAsync waiters should be woken up.</summary>
        private readonly Dequeue<ReaderInteractor<bool>> _waitingReaders = new Dequeue<ReaderInteractor<bool>>();
        /// <summary>Task signaled when any WaitToWriteAsync waiters should be woken up.</summary>
        private readonly Dequeue<ReaderInteractor<bool>> _waitingWriters = new Dequeue<ReaderInteractor<bool>>();
        /// <summary>Set to non-null once Complete has been called.</summary>
        private Exception _doneWriting;
        /// <summary>Gets an object used to synchronize all state on the instance.</summary>
        private object SyncObj => _items;

        /// <summary>Initializes the <see cref="BoundedChannel{T}"/>.</summary>
        /// <param name="bufferedCapacity">The positive bounded capacity for the channel.</param>
        /// <param name="mode">The mode used when writing to a full channel.</param>
        internal BoundedChannel(int bufferedCapacity, BoundedChannelFullMode mode)
        {
            Debug.Assert(bufferedCapacity > 0);
            _bufferedCapacity = bufferedCapacity;
            _mode = mode;
        }

        public Task Completion => _completion.Task;

        [Conditional("DEBUG")]
        private void AssertInvariants()
        {
            Debug.Assert(SyncObj != null, "The sync obj must not be null.");
            Debug.Assert(Monitor.IsEntered(SyncObj), "Invariants can only be validated while holding the lock.");

            if (!_items.IsEmpty)
            {
                Debug.Assert(_blockedReaders.IsEmpty, "There are items available, so there shouldn't be any blocked readers.");
                Debug.Assert(_waitingReaders.IsEmpty, "There are items available, so there shouldn't be any waiting readers.");
            }
            if (_items.Count < _bufferedCapacity)
            {
                Debug.Assert(_blockedWriters.IsEmpty, "There's space available, so there shouldn't be any blocked writers.");
                Debug.Assert(_waitingWriters.IsEmpty, "There's space available, so there shouldn't be any waiting writers.");
            }
            if (!_blockedReaders.IsEmpty)
            {
                Debug.Assert(_items.IsEmpty, "There shouldn't be queued items if there's a blocked reader.");
                Debug.Assert(_blockedWriters.IsEmpty, "There shouldn't be any blocked writer if there's a blocked reader.");
            }
            if (!_blockedWriters.IsEmpty)
            {
                Debug.Assert(_items.Count == _bufferedCapacity, "We should have a full buffer if there's a blocked writer.");
                Debug.Assert(_blockedReaders.IsEmpty, "There shouldn't be any blocked readers if there's a blocked writer.");
            }
            if (_doneWriting != null || _completion.Task.IsCompleted)
            {
                Debug.Assert(_blockedWriters.IsEmpty, "We're done writing, so there shouldn't be any blocked writers.");
                Debug.Assert(_blockedReaders.IsEmpty, "We're done writing, so there shouldn't be any blocked readers.");
                Debug.Assert(_waitingReaders.IsEmpty, "We're done writing, so any reader should have woken up.");
                Debug.Assert(_waitingWriters.IsEmpty, "We're done writing, so any writer should have woken up.");
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
                _doneWriting = error ?? ChannelUtilities.DoneWritingSentinel;

                // If there are no items in the channel, then there's no more work to be done,
                // so we complete the completion task.
                if (_items.IsEmpty)
                {
                    ChannelUtilities.Complete(_completion, error);
                }

                // If there are any waiting readers, fail them all, as they'll now never be satisfied.
                while (!_blockedReaders.IsEmpty)
                {
                    var reader = _blockedReaders.DequeueHead();
                    reader.Fail(error ?? ChannelUtilities.CreateInvalidCompletionException());
                }

                // If there are any waiting writers, fail them all, as they shouldn't be writing
                // now that we're complete for writing.
                while (!_blockedWriters.IsEmpty)
                {
                    var writer = _blockedWriters.DequeueHead();
                    writer.Fail(ChannelUtilities.CreateInvalidCompletionException());
                }

                // If there are any pending WaitToRead/WriteAsync calls, wake them up.
                ChannelUtilities.WakeUpWaiters(_waitingReaders, result: false);
                ChannelUtilities.WakeUpWaiters(_waitingWriters, result: false);
            }

            return true;
        }

        public ValueAwaiter<T> GetAwaiter() => new ValueAwaiter<T>(ReadAsync());

        public ValueTask<T> ReadAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Fast-path cancellation check
            if (cancellationToken.IsCancellationRequested)
            {
                return new ValueTask<T>(Task.FromCanceled<T>(cancellationToken));
            }

            lock (SyncObj)
            {
                AssertInvariants();

                // If there are any items, hand one back.
                if (!_items.IsEmpty)
                {
                    return new ValueTask<T>(DequeueItemAndPostProcess());
                }

                // There weren't any items.  If we're done writing so that there
                // will never be more items, fail.
                if (_doneWriting != null)
                {
                    return new ValueTask<T>(Task.FromException<T>(_doneWriting != ChannelUtilities.DoneWritingSentinel ? _doneWriting : ChannelUtilities.CreateInvalidCompletionException()));
                }

                // Otherwise, queue the reader.
                var reader = ReaderInteractor<T>.Create(true, cancellationToken);
                _blockedReaders.EnqueueTail(reader);
                return new ValueTask<T>(reader.Task);
            }
        }

        public Task<bool> WaitToReadAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<bool>(cancellationToken);
            }

            lock (SyncObj)
            {
                AssertInvariants();

                // If there are any items available, a read is possible.
                if (!_items.IsEmpty)
                {
                    return ChannelUtilities.TrueTask;
                }

                // There were no items available, so if we're done writing, a read will never be possible.
                if (_doneWriting != null)
                {
                    return ChannelUtilities.FalseTask;
                }

                // There were no items available, but there could be in the future, so ensure
                // there's a blocked reader task and return it.
                var r = ReaderInteractor<bool>.Create(true, cancellationToken);
                _waitingReaders.EnqueueTail(r);
                return r.Task;
            }
        }

        public bool TryRead(out T item)
        {
            lock (SyncObj)
            {
                AssertInvariants();

                // Get an item if there is one.
                if (!_items.IsEmpty)
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
            T item = _items.DequeueHead();

            // If we're now empty and we're done writing, complete the channel.
            if (_doneWriting != null && _items.IsEmpty)
            {
                ChannelUtilities.Complete(_completion, _doneWriting);
            }

            // If there are any writers blocked, there's now room for at least one
            // to be promoted to have its item moved into the items queue.  We need
            // to loop while trying to complete the writer in order to find one that
            // hasn't yet been canceled (canceled writers transition to canceled but
            // remain in the physical queue).
            while (!_blockedWriters.IsEmpty)
            {
                WriterInteractor<T> w = _blockedWriters.DequeueHead();
                if (w.Success(default(VoidResult)))
                {
                    _items.EnqueueTail(w.Item);
                    return item;
                }
            }

            // There was no blocked writer, so see if there's a WaitToWriteAsync
            // we should wake up.
            ChannelUtilities.WakeUpWaiters(_waitingWriters, result: true);

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
                while (!_blockedReaders.IsEmpty)
                {
                    ReaderInteractor<T> r = _blockedReaders.DequeueHead();
                    if (r.Success(item))
                    {
                        return true;
                    }
                }

                // If we're full, whether we can write depends on the mode.
                // If the mode is to drop an existing item, do it.
                if (_items.Count == _bufferedCapacity)
                {
                    if (_mode == BoundedChannelFullMode.Wait)
                    {
                        return false;
                    }
                    else
                    {
                        T droppedItem = _mode == BoundedChannelFullMode.DropNewest ?
                            _items.DequeueTail() :
                            _items.DequeueHead();
                    }
                }

                // There weren't any blocked (and non-canceled) readers, and
                // there's room in the queue.  Queue item, and let any waiting 
                // readers know they could try to read.
                _items.EnqueueTail(item);
                ChannelUtilities.WakeUpWaiters(_waitingReaders, result: true);
                return true;
            }
        }

        public Task<bool> WaitToWriteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<bool>(cancellationToken);
            }

            lock (SyncObj)
            {
                AssertInvariants();

                // If we're done writing, no writes will ever succeed.
                if (_doneWriting != null)
                {
                    return ChannelUtilities.FalseTask;
                }

                // If there's space to write, a write is possible.
                // And if the mode involves dropping, we can always write, as even if it's
                // full we'll just drop an element to make room.
                if (_items.Count < _bufferedCapacity || _mode != BoundedChannelFullMode.Wait)
                {
                    return ChannelUtilities.TrueTask;
                }

                // We're still allowed to write, but there's no space,
                // so ensure a waiter is queued and return it.
                var w = ReaderInteractor<bool>.Create(true, cancellationToken);
                _waitingWriters.EnqueueTail(w);
                return w.Task;
            }
        }

        public Task WriteAsync(T item, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            lock (SyncObj)
            {
                AssertInvariants();

                // If we're done writing, trying to write is an error.
                if (_doneWriting != null)
                {
                    throw ChannelUtilities.CreateInvalidCompletionException();
                }

                // If there are any blocked readers, find a non-canceled
                // one to transfer the item to.
                while (!_blockedReaders.IsEmpty)
                {
                    ReaderInteractor<T> r = _blockedReaders.DequeueHead();
                    if (r.Success(item))
                    {
                        return ChannelUtilities.TrueTask;
                    }
                }

                // There were no available readers.  If there's
                // room, simply store the item.
                if (_items.Count < _bufferedCapacity)
                {
                    _items.EnqueueTail(item);
                    ChannelUtilities.WakeUpWaiters(_waitingReaders, result: true);
                    return ChannelUtilities.TrueTask;
                }

                // There was no room, but if the mode involves dropping, we can
                // make room.
                if (_mode != BoundedChannelFullMode.Wait)
                {
                    T droppedItem = _mode == BoundedChannelFullMode.DropNewest ?
                        _items.DequeueTail() :
                        _items.DequeueHead();
                    Debug.Assert(_items.Count < _bufferedCapacity);

                    _items.EnqueueTail(item);
                    ChannelUtilities.WakeUpWaiters(_waitingReaders, result: true);
                    return ChannelUtilities.TrueTask;
                }

                // There were no readers and there was no room.
                // Queue the writer.
                var writer = WriterInteractor<T>.Create(true, cancellationToken, item);
                _blockedWriters.EnqueueTail(writer);
                return writer.Task;
            }
        }

        /// <summary>Gets the number of items in the channel.  This should only be used by the debugger.</summary>
        private int ItemsCountForDebugger => _items.Count;

        /// <summary>Gets an enumerator the debugger can use to show the contents of the channel.</summary>
        IEnumerator<T> IDebugEnumerable<T>.GetEnumerator() => _items.GetEnumerator();
    }
}
