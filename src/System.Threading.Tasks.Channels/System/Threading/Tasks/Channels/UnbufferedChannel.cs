// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics;

namespace System.Threading.Tasks.Channels
{
    /// <summary>Provides an unbuffered channel, such that a reader and a writer must rendezvous to succeed.</summary>
    [DebuggerDisplay("Writers Waiting/Blocked: {WaitingWritersCountForDebugger}/{BlockedWritersCountForDebugger}, Readers Waiting/Blocked: {WaitingReadersCountForDebugger}/{BlockedReadersCountForDebugger}")]
    [DebuggerTypeProxy(typeof(UnbufferedChannel<>.DebugView))]
    internal sealed class UnbufferedChannel<T> : Channel<T>
    {
        /// <summary>Task that represents the completion of the channel.</summary>
        private readonly TaskCompletionSource<VoidResult> _completion = new TaskCompletionSource<VoidResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        /// <summary>A queue of readers blocked waiting to be matched with a writer.</summary>
        private readonly Dequeue<ReaderInteractor<T>> _blockedReaders = new Dequeue<ReaderInteractor<T>>();
        /// <summary>A queue of writers blocked waiting to be matched with a reader.</summary>
        private readonly Dequeue<WriterInteractor<T>> _blockedWriters = new Dequeue<WriterInteractor<T>>();
        /// <summary>Task signaled when any WaitToReadAsync waiters should be woken up.</summary>
        private ReaderInteractor<bool> _waitingReaders;
        /// <summary>Task signaled when any WaitToReadAsync waiters should be woken up.</summary>
        private ReaderInteractor<bool> _waitingWriters;

        private sealed class Readable : ReadableChannel<T>
        {
            internal readonly UnbufferedChannel<T> _parent;
            internal Readable(UnbufferedChannel<T> parent) { _parent = parent; }

            public override Task Completion => _parent.Completion;
            public override ValueTask<T> ReadAsync(CancellationToken cancellationToken) => _parent.ReadAsync(cancellationToken);
            public override bool TryRead(out T item) => _parent.TryRead(out item);
            public override Task<bool> WaitToReadAsync(CancellationToken cancellationToken) => _parent.WaitToReadAsync(cancellationToken);
        }

        private sealed class Writable : WritableChannel<T>
        {
            internal readonly UnbufferedChannel<T> _parent;
            internal Writable(UnbufferedChannel<T> parent) { _parent = parent; }

            public override bool TryComplete(Exception error) => _parent.TryComplete(error);
            public override bool TryWrite(T item) => _parent.TryWrite(item);
            public override Task<bool> WaitToWriteAsync(CancellationToken cancellationToken) => _parent.WaitToWriteAsync(cancellationToken);
            public override Task WriteAsync(T item, CancellationToken cancellationToken) => _parent.WriteAsync(item, cancellationToken);
        }

        /// <summary>Initialize the channel.</summary>
        internal UnbufferedChannel()
        {
            In = new Readable(this);
            Out = new Writable(this);
        }

        public override ReadableChannel<T> In { get; }
        public override WritableChannel<T> Out { get; }

        /// <summary>Gets an object used to synchronize all state on the instance.</summary>
        private object SyncObj => _completion;

        private Task Completion => _completion.Task;

        [Conditional("DEBUG")]
        private void AssertInvariants()
        {
            Debug.Assert(SyncObj != null, "The sync obj must not be null.");
            Debug.Assert(Monitor.IsEntered(SyncObj), "Invariants can only be validated while holding the lock.");

            if (!_blockedReaders.IsEmpty)
            {
                Debug.Assert(_blockedWriters.IsEmpty, "If there are blocked readers, there can't be blocked writers.");
            }
            if (!_blockedWriters.IsEmpty)
            {
                Debug.Assert(_blockedReaders.IsEmpty, "If there are blocked writers, there can't be blocked readers.");
            }
            if (_completion.Task.IsCompleted)
            {
                Debug.Assert(_blockedReaders.IsEmpty, "No readers can be blocked after we've completed.");
                Debug.Assert(_blockedWriters.IsEmpty, "No writers can be blocked after we've completed.");
            }
        }

        private bool TryComplete(Exception error = null)
        {
            lock (SyncObj)
            {
                AssertInvariants();

                // Mark the channel as being done. Since there's no buffered data, we can complete immediately.
                if (_completion.Task.IsCompleted)
                {
                    return false;
                }
                ChannelUtilities.Complete(_completion, error);

                // Fail any blocked readers/writers, as there will be no writers/readers to pair them with.
                ChannelUtilities.FailInteractors<ReaderInteractor<T>, T>(_blockedReaders, ChannelUtilities.CreateInvalidCompletionException(error));
                ChannelUtilities.FailInteractors<WriterInteractor<T>, VoidResult>(_blockedWriters, ChannelUtilities.CreateInvalidCompletionException(error));

                // Let any waiting readers and writers know there won't be any more data
                ChannelUtilities.WakeUpWaiters(ref _waitingReaders, result: false, error: error);
                ChannelUtilities.WakeUpWaiters(ref _waitingWriters, result: false, error: error);
            }

            return true;
        }

        private ValueTask<T> ReadAsync(CancellationToken cancellationToken = default(CancellationToken))
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

                // If we're already completed, nothing to read.
                if (_completion.Task.IsCompleted)
                {
                    return new ValueTask<T>(
                        _completion.Task.IsCanceled ? Task.FromCanceled<T>(new CancellationToken(true)) :
                        Task.FromException<T>(
                            _completion.Task.IsFaulted ?
                            ChannelUtilities.CreateInvalidCompletionException(_completion.Task.Exception.InnerException) :
                            ChannelUtilities.CreateInvalidCompletionException()));
                }

                // If there are any blocked writers, find one to pair up with
                // and get its data.  Writers that got canceled will remain in the queue,
                // so we need to loop to skip past them.
                while (!_blockedWriters.IsEmpty)
                {
                    WriterInteractor<T> w = _blockedWriters.DequeueHead();
                    if (w.Success(default(VoidResult)))
                    {
                        return new ValueTask<T>(w.Item);
                    }
                }

                // No writer found to pair with.  Queue the reader.
                var r = ReaderInteractor<T>.Create(true, cancellationToken);
                _blockedReaders.EnqueueTail(r);

                // And let any waiting writers know it's their lucky day.
                ChannelUtilities.WakeUpWaiters(ref _waitingWriters, result: true);

                return new ValueTask<T>(r.Task);
            }
        }

        private bool TryRead(out T item)
        {
            lock (SyncObj)
            {
                AssertInvariants();

                // Try to find a writer to pair with
                while (!_blockedWriters.IsEmpty)
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

        private bool TryWrite(T item)
        {
            lock (SyncObj)
            {
                AssertInvariants();

                // Try to find a reader to pair with
                while (!_blockedReaders.IsEmpty)
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

        private Task WriteAsync(T item, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            lock (SyncObj)
            {
                // Fail if we've already completed
                if (_completion.Task.IsCompleted)
                {
                    return
                        _completion.Task.IsCanceled ? Task.FromCanceled<T>(new CancellationToken(true)) :
                        Task.FromException<T>(
                            _completion.Task.IsFaulted ?
                            ChannelUtilities.CreateInvalidCompletionException(_completion.Task.Exception.InnerException) :
                            ChannelUtilities.CreateInvalidCompletionException());
                }

                // Try to find a reader to pair with.  Canceled readers remain in the queue,
                // so we need to loop until we find one.
                while (!_blockedReaders.IsEmpty)
                {
                    ReaderInteractor<T> r = _blockedReaders.DequeueHead();
                    if (r.Success(item))
                    {
                        return Task.CompletedTask;
                    }
                }

                // No reader was available.  Queue the writer.
                var w = WriterInteractor<T>.Create(true, cancellationToken, item);
                _blockedWriters.EnqueueTail(w);

                // And let any waiting readers know it's their lucky day.
                ChannelUtilities.WakeUpWaiters(ref _waitingReaders, result: true);

                return w.Task;
            }
        }

        private Task<bool> WaitToReadAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            lock (SyncObj)
            {
                // If we're done writing, fail.
                if (_completion.Task.IsCompleted)
                {
                    return _completion.Task.IsFaulted ?
                        Task.FromException<bool>(_completion.Task.Exception.InnerException) :
                        ChannelUtilities.s_falseTask;
                }

                // If there's a blocked writer, we can read.
                if (!_blockedWriters.IsEmpty)
                {
                    return ChannelUtilities.s_trueTask;
                }

                // Otherwise, queue the waiter.
                return ChannelUtilities.GetOrCreateWaiter(ref _waitingReaders, true, cancellationToken);
            }
        }

        private Task<bool> WaitToWriteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            lock (SyncObj)
            {
                // If we're done writing, fail.
                if (_completion.Task.IsCompleted)
                {
                    return _completion.Task.IsFaulted ?
                        Task.FromException<bool>(_completion.Task.Exception.InnerException) :
                        ChannelUtilities.s_falseTask;
                }

                // If there's a blocked reader, we can write
                if (!_blockedReaders.IsEmpty)
                {
                    return ChannelUtilities.s_trueTask;
                }

                // Otherwise, queue the writer
                return ChannelUtilities.GetOrCreateWaiter(ref _waitingWriters, true, cancellationToken);
            }
        }

        /// <summary>Gets whether there are any waiting writers.  This should only be used by the debugger.</summary>
        private bool WaitingWritersCountForDebugger => _waitingWriters != null;
        /// <summary>Gets whether there are any waiting readers.  This should only be used by the debugger.</summary>
        private bool WaitingReadersCountForDebugger => _waitingReaders != null;
        /// <summary>Gets the number of blocked writers.  This should only be used by the debugger.</summary>
        private int BlockedWritersCountForDebugger => _blockedWriters.Count;
        /// <summary>Gets the number of blocked readers.  This should only be used by the debugger.</summary>
        private int BlockedReadersCountForDebugger => _blockedReaders.Count;

        private sealed class DebugView
        {
            private readonly UnbufferedChannel<T> _channel;

            public DebugView(UnbufferedChannel<T> channel)
            {
                _channel = channel;
            }

            public bool WaitingReaders => _channel._waitingReaders != null;
            public bool WaitingWriters => _channel._waitingWriters != null;
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
