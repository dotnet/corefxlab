// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks.Channels
{
    /// <summary>Provides a buffered channel of unbounded capacity.</summary>
    [DebuggerDisplay("Items={ItemsCountForDebugger}")]
    [DebuggerTypeProxy(typeof(DebugEnumeratorDebugView<>))]
    internal sealed class UnboundedChannel<T> : IChannel<T>, IDebugEnumerable<T>
    {
        /// <summary>Task that indicates the channel has completed.</summary>
        private readonly TaskCompletionSource<VoidResult> _completion = new TaskCompletionSource<VoidResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        /// <summary>The items in the channel.</summary>
        private readonly ConcurrentQueue<T> _items = new ConcurrentQueue<T>();
        /// <summary>Readers blocked reading from the channel.</summary>
        private readonly Dequeue<ReaderInteractor<T>> _blockedReaders = new Dequeue<ReaderInteractor<T>>();
        /// <summary>Readers waiting for a notification that data is available.</summary>
        private readonly Dequeue<ReaderInteractor<bool>> _waitingReaders = new Dequeue<ReaderInteractor<bool>>();
        /// <summary>Whether to force continuations to be executed asynchronously from producer writes.</summary>
        private readonly bool _runContinuationsAsynchronously;
        /// <summary>Set to non-null once Complete has been called.</summary>
        private Exception _doneWriting;

        /// <summary>Initialize the channel.</summary>
        internal UnboundedChannel(bool runContinuationsAsynchronously)
        {
            _runContinuationsAsynchronously = runContinuationsAsynchronously;
        }

        /// <summary>Gets the object used to synchronize access to all state on this instance.</summary>
        private object SyncObj => _items;

        public Task Completion => _completion.Task;

        [Conditional("DEBUG")]
        private void AssertInvariants()
        {
            Debug.Assert(SyncObj != null, "The sync obj must not be null.");
            Debug.Assert(Monitor.IsEntered(SyncObj), "Invariants can only be validated while holding the lock.");

            if (!_items.IsEmpty)
            {
                if (_runContinuationsAsynchronously)
                {
                    Debug.Assert(_blockedReaders.Count == 0, "There's data available, so there shouldn't be any blocked readers.");
                    Debug.Assert(_waitingReaders.Count == 0, "There's data available, so there shouldn't be any waiting readers.");
                }
                Debug.Assert(!_completion.Task.IsCompleted, "We still have data available, so shouldn't be completed.");
            }
            if ((_blockedReaders.Count > 0 || _waitingReaders.Count > 0) && _runContinuationsAsynchronously)
            {
                Debug.Assert(_items.IsEmpty, "There are blocked/waiting readers, so there shouldn't be any data available.");
            }
            if (_completion.Task.IsCompleted)
            {
                Debug.Assert(_doneWriting != null, "We're completed, so we must be done writing.");
            }
        }

        public bool TryComplete(Exception error = null)
        {
            bool anyRemainingBlockedReaders = false;
            bool anyRemainingWaitingReaders = false;

            lock (SyncObj)
            {
                AssertInvariants();

                // Mark that we're done writing
                if (_doneWriting != null)
                {
                    return false;
                }
                _doneWriting = error ?? ChannelUtilities.DoneWritingSentinel;

                // If there are no items in the queue, complete the channel's task,
                // as no more data can possibly arrive at this point.
                if (_items.IsEmpty)
                {
                    ChannelUtilities.Complete(_completion, error);
                }

                // If there are any waiting readers, then since we won't be getting any more
                // data and there can't be any currently in the queue (or else they wouldn't
                // be waiting), wake them all.  We can only do that while holding the lock
                // if they were created as running continuations asynchronously; otherwise,
                // we need to do it outside of the lock.
                if (_waitingReaders.Count > 0)
                {
                    Debug.Assert(_items.IsEmpty);
                    if (_runContinuationsAsynchronously)
                    {
                        ChannelUtilities.WakeUpWaiters(_waitingReaders, result: false);
                    }
                    else
                    {
                        anyRemainingWaitingReaders = true;
                    }
                }

                // Similarly, if there are any blocked readers, then since we won't be getting
                // any more data and there can't be any currently in the queue (or else they
                // wouldn't be waiting), fail them all.  We can only do that while holding the lock
                // if they were created as running continuations asynchronously; otherwise,
                // we need to do it outside of the lock.
                if (_blockedReaders.Count > 0)
                {
                    Debug.Assert(_items.IsEmpty);
                    if (_runContinuationsAsynchronously)
                    {
                        ChannelUtilities.FailInteractors(_blockedReaders, error);
                    }
                    else
                    {
                        anyRemainingBlockedReaders = true;
                    }
                }
            }

            // Now that we've exited the lock, see if we have any work left to do around waking
            // up or failing readers.  All of these completions may trigger synchronous continuations.

            if (anyRemainingBlockedReaders)
            {
                // Fail the remaining blocked readers
                Debug.Assert(!_runContinuationsAsynchronously, "If we're running continuations asynchronously, should have been handled above");
                ChannelUtilities.FailInteractors(SyncObj, _blockedReaders, error);
            }

            if (anyRemainingWaitingReaders)
            {
                // Wake up the remaining waiting readers
                Debug.Assert(!_runContinuationsAsynchronously, "If we're running continuations asynchronously, should have been handled above");
                ChannelUtilities.WakeUpWaiters(SyncObj, _waitingReaders, result: false);
            }

            // Successfully transitioned to completed
            return true;
        }

        public ValueAwaiter<T> GetAwaiter() => new ValueAwaiter<T>(ReadAsync());

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
                    return new ValueTask<T>(Task.FromException<T>(_doneWriting != ChannelUtilities.DoneWritingSentinel ? _doneWriting : ChannelUtilities.CreateInvalidCompletionException()));
                }

                // Otherwise, queue the reader.
                ReaderInteractor<T> reader = ReaderInteractor<T>.Create(_runContinuationsAsynchronously, cancellationToken);
                _blockedReaders.EnqueueTail(reader);
                return new ValueTask<T>(reader.Task);
            }
        }

        public Task<bool> WaitToReadAsync(CancellationToken cancellationToken = default(CancellationToken))
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
                    return ChannelUtilities.FalseTask;
                }

                // Queue the waiter
                ReaderInteractor<bool> r = ReaderInteractor<bool>.Create(_runContinuationsAsynchronously, cancellationToken);
                _waitingReaders.EnqueueTail(r);
                return r.Task;
            }
        }

        public bool TryRead(out T item)
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

            item = default(T);
            return false;
        }

        public bool TryWrite(T item)
        {
            ReaderInteractor<T> blockedReader = null;
            while (true)
            {
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
                    if (_blockedReaders.Count == 0)
                    {
                        _items.Enqueue(item);
                        if (_waitingReaders.Count == 0)
                        {
                            return true;
                        }
                        else if (_runContinuationsAsynchronously)
                        {
                            ChannelUtilities.WakeUpWaiters(_waitingReaders, result: true);
                            return true;
                        }
                        // else we have readers and are running continuations asynchronously
                        // so we need to exit the lock before we can wake them all up
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
                    Debug.Assert(!_runContinuationsAsynchronously, "Should only be here if we're allowing synchronous continuations");
                    ChannelUtilities.WakeUpWaiters(SyncObj, _waitingReaders, result: true);
                    return true;
                }
            }
        }

        public Task<bool> WaitToWriteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<bool>(cancellationToken);
            }

            // Other than for cancellation, writing can always be done if we haven't completed, as we're unbounded.
            lock (SyncObj)
            {
                return _doneWriting == null ? ChannelUtilities.TrueTask : ChannelUtilities.FalseTask;
            }
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
