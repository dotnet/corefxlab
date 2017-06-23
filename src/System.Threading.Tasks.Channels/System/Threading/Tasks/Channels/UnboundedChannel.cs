// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Threading.Tasks.Channels
{
    /// <summary>Provides a buffered channel of unbounded capacity.</summary>
    [DebuggerDisplay("Items={ItemsCountForDebugger}")]
    [DebuggerTypeProxy(typeof(DebugEnumeratorDebugView<>))]
    internal sealed class UnboundedChannel<T> : Channel<T>, IDebugEnumerable<T>
    {
        /// <summary>Task that indicates the channel has completed.</summary>
        private readonly TaskCompletionSource<VoidResult> _completion;
        /// <summary>The items in the channel.</summary>
        private readonly ConcurrentQueue<T> _items = new ConcurrentQueue<T>();
        /// <summary>Readers blocked reading from the channel.</summary>
        private readonly Dequeue<ReaderInteractor<T>> _blockedReaders = new Dequeue<ReaderInteractor<T>>();
        /// <summary>Readers waiting for a notification that data is available.</summary>
        private ReaderInteractor<bool> _waitingReaders;
        /// <summary>Whether to force continuations to be executed asynchronously from producer writes.</summary>
        private readonly bool _runContinuationsAsynchronously;
        /// <summary>Set to non-null once Complete has been called.</summary>
        private Exception _doneWriting;

        /// <summary>Initialize the channel.</summary>
        internal UnboundedChannel(bool runContinuationsAsynchronously)
        {
            _runContinuationsAsynchronously = runContinuationsAsynchronously;
            _completion = new TaskCompletionSource<VoidResult>(runContinuationsAsynchronously ? TaskCreationOptions.RunContinuationsAsynchronously : TaskCreationOptions.None);
            In = new Readable(this);
            Out = new Writable(this);
        }

        private sealed class Readable : ReadableChannel<T>
        {
            internal readonly UnboundedChannel<T> _parent;
            internal Readable(UnboundedChannel<T> parent) { _parent = parent; }

            public override Task Completion => _parent.Completion;
            public override ValueTask<T> ReadAsync(CancellationToken cancellationToken) => _parent.ReadAsync(cancellationToken);
            public override bool TryRead(out T item) => _parent.TryRead(out item);
            public override Task<bool> WaitToReadAsync(CancellationToken cancellationToken) => _parent.WaitToReadAsync(cancellationToken);
        }

        private sealed class Writable : WritableChannel<T>
        {
            internal readonly UnboundedChannel<T> _parent;
            internal Writable(UnboundedChannel<T> parent) { _parent = parent; }

            public override bool TryComplete(Exception error) => _parent.TryComplete(error);
            public override bool TryWrite(T item) => _parent.TryWrite(item);
            public override Task<bool> WaitToWriteAsync(CancellationToken cancellationToken) => _parent.WaitToWriteAsync(cancellationToken);
            public override Task WriteAsync(T item, CancellationToken cancellationToken) => _parent.WriteAsync(item, cancellationToken);
        }

        public override ReadableChannel<T> In { get; }
        public override WritableChannel<T> Out { get; }

        /// <summary>Gets the object used to synchronize access to all state on this instance.</summary>
        private object SyncObj => _items;

        private Task Completion => _completion.Task;

        [Conditional("DEBUG")]
        private void AssertInvariants()
        {
            Debug.Assert(SyncObj != null, "The sync obj must not be null.");
            Debug.Assert(Monitor.IsEntered(SyncObj), "Invariants can only be validated while holding the lock.");

            if (!_items.IsEmpty)
            {
                if (_runContinuationsAsynchronously)
                {
                    Debug.Assert(_blockedReaders.IsEmpty, "There's data available, so there shouldn't be any blocked readers.");
                    Debug.Assert(_waitingReaders == null, "There's data available, so there shouldn't be any waiting readers.");
                }
                Debug.Assert(!_completion.Task.IsCompleted, "We still have data available, so shouldn't be completed.");
            }
            if ((!_blockedReaders.IsEmpty || _waitingReaders != null) && _runContinuationsAsynchronously)
            {
                Debug.Assert(_items.IsEmpty, "There are blocked/waiting readers, so there shouldn't be any data available.");
            }
            if (_completion.Task.IsCompleted)
            {
                Debug.Assert(_doneWriting != null, "We're completed, so we must be done writing.");
            }
        }

        private bool TryComplete(Exception error = null)
        {
            bool completeTask;
            lock (SyncObj)
            {
                AssertInvariants();

                // If we've already marked the channel as completed, bail.
                if (_doneWriting != null)
                {
                    return false;
                }

                // Mark that we're done writing.
                _doneWriting = error ?? ChannelUtilities.DoneWritingSentinel;
                completeTask = _items.IsEmpty;
            }

            // If there are no items in the queue, complete the channel's task,
            // as no more data can possibly arrive at this point.  We do this outside
            // of the lock in case we'll be running synchronous completions, and we
            // do it before completing blocked/waiting readers, so that when they
            // wake up they'll see the task as being completed.
            if (completeTask)
            {
                ChannelUtilities.Complete(_completion, error);
            }

            // At this point, _blockedReaders and _waitingReaders will not be mutated:
            // they're only mutated by readers while holding the lock, and only if _doneWriting is null.
            // We also know that only one thread (this one) will ever get here, as only that thread
            // will be the one to transition from _doneWriting false to true.  As such, we can
            // freely manipulate _blockedReaders and _waitingReaders without any concurrency concerns.
            ChannelUtilities.FailInteractors<ReaderInteractor<T>,T>(_blockedReaders, ChannelUtilities.CreateInvalidCompletionException(error));
            ChannelUtilities.WakeUpWaiters(ref _waitingReaders, result: false, error: error);

            // Successfully transitioned to completed.
            return true;
        }

        private ValueTask<T> ReadAsync(CancellationToken cancellationToken = default)
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
                AssertInvariants();

                // If there are any items, return one.
                T item;
                if (_items.TryDequeue(out item))
                {
                    // Dequeue an item
                    if (_doneWriting != null && _items.IsEmpty)
                    {
                        // If we've now emptied the items queue and we're not getting any more, complete.
                        ChannelUtilities.Complete(_completion, _doneWriting);
                    }

                    return new ValueTask<T>(item);
                }

                // There are no items, so if we're done writing, fail.
                if (_doneWriting != null)
                {
                    return ChannelUtilities.GetInvalidCompletionValueTask<T>(_doneWriting);
                }

                // Otherwise, queue the reader.
                ReaderInteractor<T> reader = ReaderInteractor<T>.Create(_runContinuationsAsynchronously, cancellationToken);
                _blockedReaders.EnqueueTail(reader);
                return new ValueTask<T>(reader.Task);
            }
        }

        private Task<bool> WaitToReadAsync(CancellationToken cancellationToken = default)
        {
            // If there are any items, readers can try to get them.
            if (!_items.IsEmpty)
            {
                return ChannelUtilities.TrueTask;
            }

            lock (SyncObj)
            {
                AssertInvariants();

                // Try again to read now that we're synchronized with writers.
                if (!_items.IsEmpty)
                {
                    return ChannelUtilities.TrueTask;
                }

                // There are no items, so if we're done writing, there's never going to be data available.
                if (_doneWriting != null)
                {
                    return _doneWriting != ChannelUtilities.DoneWritingSentinel ?
                        Task.FromException<bool>(_doneWriting) :
                        ChannelUtilities.FalseTask;
                }

                // Queue the waiter
                return ChannelUtilities.GetOrCreateWaiter(ref _waitingReaders, _runContinuationsAsynchronously, cancellationToken);
            }
        }

        private bool TryRead(out T item)
        {
            // Dequeue an item if we can
            if (_items.TryDequeue(out item))
            {
                if (_doneWriting != null && _items.IsEmpty)
                {
                    // If we've now emptied the items queue and we're not getting any more, complete.
                    ChannelUtilities.Complete(_completion, _doneWriting);
                }
                return true;
            }

            item = default;
            return false;
        }

        private bool TryWrite(T item)
        {
            while (true)
            {
                ReaderInteractor<T> blockedReader = null;
                ReaderInteractor<bool> waitingReaders = null;
                lock (SyncObj)
                {
                    // If writing has already been marked as done, fail the write.
                    AssertInvariants();
                    if (_doneWriting != null)
                    {
                        return false;
                    }

                    // If there aren't any blocked readers, just add the data to the queue,
                    // and let any waiting readers know that they should try to read it.
                    // We can only complete such waiters here under the lock if they run
                    // continuations asynchronously (otherwise the synchronous continuations
                    // could be invoked under the lock).  If we don't complete them here, we
                    // need to do so outside of the lock.
                    if (_blockedReaders.IsEmpty)
                    {
                        _items.Enqueue(item);
                        waitingReaders = _waitingReaders;
                        if (waitingReaders == null)
                        {
                            return true;
                        }
                        _waitingReaders = null;
                    }
                    else
                    {
                        // There were blocked readers.  Grab one, and then complete it outside of the lock.
                        blockedReader = _blockedReaders.DequeueHead();
                    }
                }

                if (blockedReader != null)
                {
                    // Complete the reader.  It's possible the reader was canceled, in which
                    // case we loop around to try everything again.
                    if (blockedReader.Success(item))
                    {
                        return true;
                    }
                }
                else
                {
                    // Wake up all of the waiters.  Since we've released the lock, it's possible
                    // we could cause some spurious wake-ups here, if we tell a waiter there's
                    // something available but all data has already been removed.  It's a benign
                    // race condition, though, as consumers already need to account for such things.
                    waitingReaders.Success(item: true);
                    return true;
                }
            }
        }

        private Task<bool> WaitToWriteAsync(CancellationToken cancellationToken = default) =>
            cancellationToken.IsCancellationRequested ? Task.FromCanceled<bool>(cancellationToken) :
            _doneWriting == null ? ChannelUtilities.TrueTask : // unbounded writing can always be done if we haven't completed
            _doneWriting != ChannelUtilities.DoneWritingSentinel ? Task.FromException<bool>(_doneWriting) :
            ChannelUtilities.FalseTask;

        private Task WriteAsync(T item, CancellationToken cancellationToken = default) =>
            cancellationToken.IsCancellationRequested ? Task.FromCanceled(cancellationToken) :
            TryWrite(item) ? Task.CompletedTask :
            Task.FromException(ChannelUtilities.CreateInvalidCompletionException(_doneWriting));

        /// <summary>Gets the number of items in the channel.  This should only be used by the debugger.</summary>
        private int ItemsCountForDebugger => _items.Count;

        /// <summary>Gets an enumerator the debugger can use to show the contents of the channel.</summary>
        IEnumerator<T> IDebugEnumerable<T>.GetEnumerator() => _items.GetEnumerator();
    }
}
