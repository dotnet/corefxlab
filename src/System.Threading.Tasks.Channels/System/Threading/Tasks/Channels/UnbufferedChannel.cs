// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics;

namespace System.Threading.Tasks.Channels
{
    /// <summary>Provides an unbuffered channel, such that a reader and a writer must rendezvous to succeed.</summary>
    [DebuggerDisplay("Waiting Writers = {WaitingWritersCountForDebugger}, Waiting Readers = {WaitingReadersCountForDebugger}")]
    [DebuggerTypeProxy(typeof(UnbufferedChannel<>.DebugView))]
    internal sealed class UnbufferedChannel<T> : IChannel<T>
    {
        /// <summary>Task that represents the completion of the channel.</summary>
        private readonly TaskCompletionSource<VoidResult> _completion = new TaskCompletionSource<VoidResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        /// <summary>A queue of readers blocked waiting to be matched with a writer.</summary>
        private readonly Dequeue<ReaderInteractor<T>> _blockedReaders = new Dequeue<ReaderInteractor<T>>();
        /// <summary>A queue of writers blocked waiting to be matched with a reader.</summary>
        private readonly Dequeue<WriterInteractor<T>> _blockedWriters = new Dequeue<WriterInteractor<T>>();
        /// <summary>Tasks waiting for data to be available to read.</summary>
        private readonly Dequeue<ReaderInteractor<bool>> _waitingReaders = new Dequeue<ReaderInteractor<bool>>();
        /// <summary>Tasks waiting for data to be available to write.</summary>
        private readonly Dequeue<ReaderInteractor<bool>> _waitingWriters = new Dequeue<ReaderInteractor<bool>>();

        /// <summary>Initialize the channel.</summary>
        internal UnbufferedChannel() { }

        /// <summary>Gets an object used to synchronize all state on the instance.</summary>
        private object SyncObj => _completion;

        public Task Completion => _completion.Task;

        [Conditional("DEBUG")]
        private void AssertInvariants()
        {
            Debug.Assert(SyncObj != null, "The sync obj must not be null.");
            Debug.Assert(Monitor.IsEntered(SyncObj), "Invariants can only be validated while holding the lock.");

            if (_blockedReaders.Count > 0)
            {
                Debug.Assert(_blockedWriters.Count == 0, "If there are blocked readers, there can't be blocked writers.");
            }
            if (_blockedWriters.Count > 0)
            {
                Debug.Assert(_blockedReaders.Count == 0, "If there are blocked writers, there can't be blocked readers.");
            }
            if (_completion.Task.IsCompleted)
            {
                Debug.Assert(_blockedReaders.Count == 0, "No readers can be blocked after we've completed.");
                Debug.Assert(_blockedWriters.Count == 0, "No writers can be blocked after we've completed.");
            }
        }

        public bool TryComplete(Exception error = null)
        {
            lock (SyncObj)
            {
                AssertInvariants();

                // Mark the channel as being done. Since there's no buffered data, we can complete immediately.
                if (_completion.Task.IsCompleted)
                {
                    return false;
                }
                ChannelUtilities.CompleteWithOptionalError(_completion, error);

                // Fail any blocked readers, as there will be no writers to pair them with.
                while (_blockedReaders.Count > 0)
                {
                    var reader = _blockedReaders.DequeueHead();
                    reader.Fail(error ?? ChannelUtilities.CreateInvalidCompletionException());
                }

                // Fail any blocked writers, as there will be no readers to pair them with.
                while (_blockedWriters.Count > 0)
                {
                    var writer = _blockedWriters.DequeueHead();
                    writer.Fail(ChannelUtilities.CreateInvalidCompletionException());
                }

                // Let any waiting readers and writers know there won't be any more data
                ChannelUtilities.WakeUpWaiters(_waitingReaders, false);
                ChannelUtilities.WakeUpWaiters(_waitingWriters, false);
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
            if (cancellationToken.IsCancellationRequested)
                return new ValueTask<T>(Task.FromCanceled<T>(cancellationToken));

            lock (SyncObj)
            {
                AssertInvariants();

                // If we're already completed, nothing to read.
                if (_completion.Task.IsCompleted)
                {
                    return new ValueTask<T>(Task.FromException<T>(_completion.Task.IsFaulted ? _completion.Task.Exception.InnerException : ChannelUtilities.CreateInvalidCompletionException()));
                }

                // If there are any blocked writers, find one to pair up with
                // and get its data.  Writers that got canceled will remain in the queue,
                // so we need to loop to skip past them.
                while (_blockedWriters.Count > 0)
                {
                    WriterInteractor<T> w = _blockedWriters.DequeueHead();
                    if (w.Success(default(VoidResult)))
                    {
                        return new ValueTask<T>(w.Item);
                    }
                }

                // No writer found to pair with.  Queue the reader.
                var r = ReaderInteractor<T>.Create(cancellationToken);
                _blockedReaders.EnqueueTail(r);

                // And let any waiting writers know it's their lucky day.
                ChannelUtilities.WakeUpWaiters(_waitingWriters, true);

                return new ValueTask<T>(r.Task);
            }
        }

        public bool TryRead(out T item)
        {
            lock (SyncObj)
            {
                AssertInvariants();

                // Try to find a writer to pair with
                while (_blockedWriters.Count > 0)
                {
                    WriterInteractor<T> w = _blockedWriters.DequeueHead();
                    if (w.Success(default(VoidResult)))
                    {
                        item = w.Item;
                        return true;
                    }
                }
            }

            // None found
            item = default(T);
            return false;
        }

        public bool TryWrite(T item)
        {
            lock (SyncObj)
            {
                AssertInvariants();

                // Try to find a reader to pair with
                while (_blockedReaders.Count > 0)
                {
                    ReaderInteractor<T> r = _blockedReaders.DequeueHead();
                    if (r.Success(item))
                    {
                        return true;
                    }
                }
            }

            // None found
            return false;
        }

        public Task WriteAsync(T item, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled(cancellationToken);

            lock (SyncObj)
            {
                // Fail if we've already completed
                if (_completion.Task.IsCompleted)
                {
                    return Task.FromException(ChannelUtilities.CreateInvalidCompletionException());
                }

                // Try to find a reader to pair with.  Canceled readers remain in the queue,
                // so we need to loop until we find one.
                while (_blockedReaders.Count > 0)
                {
                    ReaderInteractor<T> r = _blockedReaders.DequeueHead();
                    if (r.Success(item))
                    {
                        return Task.CompletedTask;
                    }
                }

                // No reader was available.  Queue the writer.
                var w = WriterInteractor<T>.Create(cancellationToken, item);
                _blockedWriters.EnqueueTail(w);

                // And let any waiting readers know it's their lucky day.
                ChannelUtilities.WakeUpWaiters(_waitingReaders, true);

                return w.Task;
            }
        }

        public Task<bool> WaitToReadAsync(CancellationToken cancellationToken)
        {
            lock (SyncObj)
            {
                // If we're done writing, fail.
                if (_completion.Task.IsCompleted)
                {
                    return ChannelUtilities.FalseTask;
                }

                // If there's a blocked writer, we can read.
                if (_blockedWriters.Count > 0)
                {
                    return ChannelUtilities.TrueTask;
                }

                // Otherwise, queue the waiter.
                var r = ReaderInteractor<bool>.Create(cancellationToken);
                _waitingReaders.EnqueueTail(r);
                return r.Task;
            }
        }

        public Task<bool> WaitToWriteAsync(CancellationToken cancellationToken)
        {
            lock (SyncObj)
            {
                // If we're done writing, fail.
                if (_completion.Task.IsCompleted)
                {
                    return ChannelUtilities.FalseTask;
                }

                // If there's a blocked reader, we can write
                if (_blockedReaders.Count > 0)
                {
                    return ChannelUtilities.TrueTask;
                }

                // Otherwise, queue the writer
                var w = ReaderInteractor<bool>.Create(cancellationToken);
                _waitingWriters.EnqueueTail(w);
                return w.Task;
            }
        }

        /// <summary>Gets the number of writers waiting on the channel.  This should only be used by the debugger.</summary>
        private int WaitingWritersCountForDebugger => _waitingWriters.Count;

        /// <summary>Gets the number of readers waiting on the channel.  This should only be used by the debugger.</summary>
        private int WaitingReadersCountForDebugger => _waitingReaders.Count;

        private sealed class DebugView
        {
            private readonly UnbufferedChannel<T> _channel;

            public DebugView(UnbufferedChannel<T> channel)
            {
                _channel = channel;
            }

            public int WaitingReaders => _channel._waitingReaders.Count;
            public int WaitingWriters => _channel._waitingWriters.Count;
            public int BlockedReaders => _channel._blockedReaders.Count;
            public T[] BlockedWriters
            {
                get
                {
                    var items = new List<T>();
                    foreach (WriterInteractor<T> blockedWriter in _channel._blockedWriters)
                    {
                        items.Add(blockedWriter.Item);
                    }
                    return items.ToArray();
                }
            }
        }
    }
}
