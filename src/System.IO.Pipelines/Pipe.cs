// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Default <see cref="IPipeWriter"/> and <see cref="IPipeReader"/> implementation.
    /// </summary>
    internal class Pipe : IPipe, IPipeReader, IPipeWriter, IReadableBufferAwaiter, IWritableBufferAwaiter
    {
        private readonly IBufferPool _pool;

        private readonly long _maximumSizeHigh;
        private readonly long _maximumSizeLow;

        private Awaitable _readerAwaitable;
        private Awaitable _writerAwaitable;

        // The read head which is the extent of the IPipelineReader's consumed bytes
        private BufferSegment _readHead;

        // The commit head which is the extent of the bytes available to the IPipelineReader to consume
        private BufferSegment _commitHead;
        private int _commitHeadIndex;

        // The write head which is the extent of the IPipelineWriter's written bytes
        private BufferSegment _writingHead;

        private int _consumingState;
        private int _producingState;
        private object _sync = new object();
        private int _cancelledState;

        private Completion _writerCompletion;
        private Completion _readerCompletion;
        private bool _disposed;

#if CONSUMING_LOCATION_TRACKING
        private string _consumingLocation;
#endif
#if PRODUCING_LOCATION_TRACKING
        private string _producingLocation;
#endif
#if COMPLETION_LOCATION_TRACKING
        private string _completeWriterLocation;
        private string _completeReaderLocation;
#endif
        private long _length;
        private long _currentWriteLength;

        internal long Length => _length;

        /// <summary>
        /// Initializes the <see cref="Pipe"/> with the specifed <see cref="IBufferPool"/>.
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="options"></param>
        public Pipe(IBufferPool pool, PipeOptions options = null)
        {
            if (pool == null)
            {
                throw new ArgumentNullException(nameof(pool));
            }

            options = options ?? new PipeOptions();

            if (options.MaximumSizeLow < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(options.MaximumSizeLow));
            }

            if (options.MaximumSizeHigh < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(options.MaximumSizeHigh));
            }

            if (options.MaximumSizeLow > options.MaximumSizeHigh)
            {
                throw new ArgumentException(nameof(options.MaximumSizeHigh) + " should be greater or equal to " + nameof(options.MaximumSizeLow), nameof(options.MaximumSizeHigh));
            }

            _pool = pool;
            _maximumSizeHigh = options.MaximumSizeHigh;
            _maximumSizeLow = options.MaximumSizeLow;

            _readerAwaitable = new Awaitable(options.ReaderScheduler ?? InlineScheduler.Default, completed: false);
            _writerAwaitable = new Awaitable(options.WriterScheduler ?? InlineScheduler.Default, completed: true);
        }

        internal Memory<byte> Memory => _writingHead?.Memory.Slice(_writingHead.End, _writingHead.WritableBytes) ?? Memory<byte>.Empty;

        /// <summary>
        /// Allocates memory from the pipeline to write into.
        /// </summary>
        /// <param name="minimumSize">The minimum size buffer to allocate</param>
        /// <returns>A <see cref="WritableBuffer"/> that can be written to.</returns>
        WritableBuffer IPipeWriter.Alloc(int minimumSize)
        {
#if PRODUCING_LOCATION_TRACKING
            _producingLocation = Environment.StackTrace;
#endif
            if (_writerCompletion.IsCompleted)
            {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.NoWritingAllowed
#if COMPLETION_LOCATION_TRACKING
                    , _completeWriterLocation
#endif
                    );
            }

            if (minimumSize < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.minimumSize);
            }

            // CompareExchange not required as its setting to current value if test fails
            if (Interlocked.Exchange(ref _producingState, State.Active) != State.NotActive)
            {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.AlreadyProducing
#if PRODUCING_LOCATION_TRACKING
                    , _producingLocation
#endif
                    );
            }

            if (minimumSize > 0)
            {
                try
                {
                    AllocateWriteHead(minimumSize);
                }
                catch (Exception)
                {
                    // Reset producing state if allocation failed
                    Interlocked.Exchange(ref _producingState, State.NotActive);
                    throw;
                }
            }
            _currentWriteLength = 0;
            return new WritableBuffer(this);
        }

        internal void Ensure(int count = 1)
        {
            EnsureAlloc();

            var segment = _writingHead;
            if (segment == null)
            {
                segment = AllocateWriteHead(count);
            }

            var bytesLeftInBuffer = segment.WritableBytes;

            // If inadequate bytes left or if the segment is readonly
            if (bytesLeftInBuffer == 0 || bytesLeftInBuffer < count || segment.ReadOnly)
            {
                var nextBuffer = _pool.Lease(count);
                var nextSegment = new BufferSegment(nextBuffer);

                segment.Next = nextSegment;

                _writingHead = nextSegment;
            }
        }

        private BufferSegment AllocateWriteHead(int count)
        {
            BufferSegment segment = null;

            if (_commitHead != null && !_commitHead.ReadOnly)
            {
                // Try to return the tail so the calling code can append to it
                int remaining = _commitHead.WritableBytes;

                if (count <= remaining)
                {
                    // Free tail space of the right amount, use that
                    segment = _commitHead;
                }
            }

            if (segment == null)
            {
                // No free tail space, allocate a new segment
                segment = new BufferSegment(_pool.Lease(count));
            }

            // Changing commit head shared with Reader
            lock (_sync)
            {
                if (_commitHead == null)
                {
                    // No previous writes have occurred
                    _commitHead = segment;
                }
                else if (segment != _commitHead && _commitHead.Next == null)
                {
                    // Append the segment to the commit head if writes have been committed
                    // and it isn't the same segment (unused tail space)
                    _commitHead.Next = segment;
                }
            }

            // Set write head to assigned segment
            _writingHead = segment;

            return segment;
        }

        internal void Append(ReadableBuffer buffer)
        {
            if (buffer.IsEmpty)
            {
                return; // nothing to do
            }

            EnsureAlloc();

            BufferSegment clonedEnd;
            var clonedBegin = BufferSegment.Clone(buffer.Start, buffer.End, out clonedEnd);

            if (_writingHead == null)
            {
                // No active write

                if (_commitHead == null)
                {
                    // No allocated buffers yet, not locking as _readHead will be null
                    _commitHead = clonedBegin;
                }
                else
                {
                    Debug.Assert(_commitHead.Next == null);
                    // Allocated buffer, append as next segment
                    _commitHead.Next = clonedBegin;
                }
            }
            else
            {
                Debug.Assert(_writingHead.Next == null);
                // Active write, append as next segment
                _writingHead.Next = clonedBegin;
            }

            // Move write head to end of buffer
            _writingHead = clonedEnd;
            _currentWriteLength += buffer.Length;
        }

        private void EnsureAlloc()
        {
            if (_producingState == State.NotActive)
            {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.NotProducingNoAlloc);
            }
        }

        internal void Commit()
        {
            // CompareExchange not required as its setting to current value if test fails
            if (Interlocked.Exchange(ref _producingState, State.NotActive) != State.Active)
            {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.NotProducingToComplete);
            }

            if (_writingHead == null)
            {
                // Nothing written to commit
                return;
            }

            // Changing commit head shared with Reader
            lock (_sync)
            {
                if (_readHead == null)
                {
                    // Update the head to point to the head of the buffer.
                    // This happens if we called alloc(0) then write
                    _readHead = _commitHead;
                }

                // Always move the commit head to the write head
                _commitHead = _writingHead;
                _commitHeadIndex = _writingHead.End;

                _length += _currentWriteLength;
                // Do not reset if reader is complete
                if (_maximumSizeHigh > 0 &&
                    _length >= _maximumSizeHigh &&
                    !_readerCompletion.IsCompleted)
                {
                    _writerAwaitable.Reset();
                }
            }

            // Clear the writing state
            _writingHead = null;
#if PRODUCING_LOCATION_TRACKING
            _producingLocation = null;
#endif
        }

        internal void AdvanceWriter(int bytesWritten)
        {
            EnsureAlloc();

            if (bytesWritten > 0)
            {
                Debug.Assert(_writingHead != null);
                Debug.Assert(!_writingHead.ReadOnly);
                Debug.Assert(_writingHead.Next == null);

                var buffer = _writingHead.Memory;
                var bufferIndex = _writingHead.End + bytesWritten;

                Debug.Assert(bufferIndex <= buffer.Length);

                _writingHead.End = bufferIndex;
                _currentWriteLength += bytesWritten;
            }
            else if (bytesWritten < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.bytesWritten);
            } // and if zero, just do nothing; don't need to validate tail etc
        }

        internal WritableBufferAwaitable FlushAsync()
        {
            if (_producingState == State.Active)
            {
                // Commit the data as not already committed
                Commit();
            }

            _readerAwaitable.Resume();

            return new WritableBufferAwaitable(this);
        }

        internal ReadableBuffer AsReadableBuffer()
        {
            if (_writingHead == null)
            {
                return new ReadableBuffer(); // Nothing written return empty
            }

            return new ReadableBuffer(new ReadCursor(_commitHead, _commitHeadIndex), new ReadCursor(_writingHead, _writingHead.End));
        }

        private ReadableBuffer Read()
        {
            // CompareExchange not required as its setting to current value if test fails
            if (Interlocked.Exchange(ref _consumingState, State.Active) != State.NotActive)
            {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.AlreadyConsuming
#if CONSUMING_LOCATION_TRACKING
                    , _consumingLocation
#endif
                    );
            }
#if CONSUMING_LOCATION_TRACKING
            _consumingLocation = Environment.StackTrace;
#endif
            ReadCursor readEnd;
            // No need to read end if there is no head
            var head = _readHead;
            if (head == null)
            {
                readEnd = new ReadCursor(null);
            }
            else
            {
                // Reading commit head shared with writer
                lock (_sync)
                {
                    readEnd = new ReadCursor(_commitHead, _commitHeadIndex);
                }
            }

            return new ReadableBuffer(new ReadCursor(head), readEnd);
        }

        /// <summary>
        /// Marks the pipeline as being complete, meaning no more items will be written to it.
        /// </summary>
        /// <param name="exception">Optional Exception indicating a failure that's causing the pipeline to complete.</param>
        void IPipeWriter.Complete(Exception exception)
        {
            if (_producingState != State.NotActive)
            {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.CompleteWriterActiveProducer
#if PRODUCING_LOCATION_TRACKING
                    , _producingLocation
#endif
                    );
            }
#if COMPLETION_LOCATION_TRACKING
            _completeWriterLocation += Environment.StackTrace + Environment.NewLine;
#endif
            // TODO: Review this lock?
            lock (_sync)
            {
                _writerCompletion.TryComplete(exception);

                _readerAwaitable.Resume();

                if (_readerCompletion.IsCompleted)
                {
                    Dispose();
                }
            }
        }

        // Reading

        void IPipeReader.Advance(ReadCursor consumed, ReadCursor examined)
        {
            BufferSegment returnStart = null;
            BufferSegment returnEnd = null;

            int consumedBytes = 0;
            if (!consumed.IsDefault)
            {
                consumedBytes = ReadCursor.GetLength(_readHead, _readHead.Start, consumed.Segment, consumed.Index);

                returnStart = _readHead;
                returnEnd = consumed.Segment;
                _readHead = consumed.Segment;
                _readHead.Start = consumed.Index;
            }

            bool resumeWriter;

            // Reading commit head shared with writer
            lock (_sync)
            {
                _length -= consumedBytes;
                resumeWriter = _length < _maximumSizeLow;

                // Change the state from observed -> not cancelled. We only want to reset the cancelled state if it was observed
                Interlocked.CompareExchange(ref _cancelledState, CancelledState.NotCancelled, CancelledState.CancellationObserved);

                var consumedEverything = examined.Segment == _commitHead &&
                                         examined.Index == _commitHeadIndex &&
                                         !_writerCompletion.IsCompleted;

                // We reset the awaitable to not completed if
                // 1. We've consumed everything the producer produced so far
                // 2. Cancellation wasn't requested
                if (consumedEverything && _cancelledState != CancelledState.CancellationRequested)
                {
                    _readerAwaitable.Reset();
                }
            }

            while (returnStart != null && returnStart != returnEnd)
            {
                var returnSegment = returnStart;
                returnStart = returnStart.Next;
                returnSegment.Dispose();
            }

#if CONSUMING_LOCATION_TRACKING
            _consumingLocation = null;
#endif
            // CompareExchange not required as its setting to current value if test fails
            if (Interlocked.Exchange(ref _consumingState, State.NotActive) != State.Active)
            {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.NotConsumingToComplete);
            }

            if (resumeWriter)
            {
                _writerAwaitable.Resume();
            }
        }

        /// <summary>
        /// Signal to the producer that the consumer is done reading.
        /// </summary>
        /// <param name="exception">Optional Exception indicating a failure that's causing the pipeline to complete.</param>
        void IPipeReader.Complete(Exception exception)
        {
            if (_consumingState != State.NotActive)
            {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.CompleteReaderActiveConsumer
#if CONSUMING_LOCATION_TRACKING
                    , _consumingLocation
#endif
                    );
            }
#if COMPLETION_LOCATION_TRACKING
            _completeReaderLocation += Environment.StackTrace + Environment.NewLine;
#endif
            // TODO: Review this lock?
            lock (_sync)
            {
                _readerCompletion.TryComplete(exception);

                _writerAwaitable.Resume();

                if (_writerCompletion.IsCompleted)
                {
                    Dispose();
                }
            }
        }

        /// <summary>
        /// Cancel to currently pending call to <see cref="ReadAsync"/> without completing the <see cref="IPipeReader"/>.
        /// </summary>
        void IPipeReader.CancelPendingRead()
        {
            // TODO: Can factor out this lock
            lock (_sync)
            {
                // Mark reading is cancellable
                _cancelledState = CancelledState.CancellationRequested;

                _readerAwaitable.Resume();
            }
        }

        /// <summary>
        /// Asynchronously reads a sequence of bytes from the current <see cref="IPipeReader"/>.
        /// </summary>
        /// <returns>A <see cref="PipeAwaitable"/> representing the asynchronous read operation.</returns>
        ReadableBufferAwaitable IPipeReader.ReadAsync()
        {
            if (_readerCompletion.IsCompleted)
            {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.NoReadingAllowed
#if COMPLETION_LOCATION_TRACKING
                    , _completeReaderLocation
#endif
                    );
            }

            return new ReadableBufferAwaitable(this);
        }

        private static void GetResult(ref Awaitable awaitableState,
            ref int cancelledState,
            ref Completion completion,
            out bool isCancelled,
            out bool isCompleted)
        {
            if (!awaitableState.IsCompleted)
            {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.GetResultNotCompleted);
            }

            // Change the state from to be cancelled -> observed
            isCancelled = Interlocked.CompareExchange(
                ref cancelledState,
                CancelledState.CancellationObserved,
                CancelledState.CancellationRequested) == CancelledState.CancellationRequested;

            isCompleted = completion.IsCompleted;
            completion.ThrowIfFailed();
        }

        private void Dispose()
        {
            // TODO: Review throw if not completed?

            lock (_sync)
            {
                if (_disposed)
                {
                    return;
                }

                _disposed = true;
                // Return all segments
                var segment = _readHead;
                while (segment != null)
                {
                    var returnSegment = segment;
                    segment = segment.Next;

                    returnSegment.Dispose();
                }

                _readHead = null;
                _commitHead = null;
            }
        }

        // IReadableBufferAwaiter members

        bool IReadableBufferAwaiter.IsCompleted => _readerAwaitable.IsCompleted;

        void IReadableBufferAwaiter.OnCompleted(Action continuation)
        {
            _readerAwaitable.OnCompleted(continuation, ref _writerCompletion);
        }

        ReadResult IReadableBufferAwaiter.GetResult()
        {
            GetResult(ref _readerAwaitable,
                ref _cancelledState,
                ref _writerCompletion,
                out bool isCancelled,
                out bool isCompleted);
            return new ReadResult(Read(), isCancelled, isCompleted);
        }

        // IWritableBufferAwaiter members

        bool IWritableBufferAwaiter.IsCompleted => _writerAwaitable.IsCompleted;

        bool IWritableBufferAwaiter.GetResult()
        {
            var cancelledState = CancelledState.NotCancelled;
            GetResult(ref _writerAwaitable,
                ref cancelledState,
                ref _readerCompletion,
                out bool isCancelled,
                out bool isCompleted);
            return !isCompleted;
        }

        void IWritableBufferAwaiter.OnCompleted(Action continuation)
        {
            _writerAwaitable.OnCompleted(continuation, ref _readerCompletion);
        }

        public IPipeReader Reader => this;
        public IPipeWriter Writer => this;

        private struct Awaitable
        {
            private static readonly Action _awaitableIsCompleted = () => { };
            private static readonly Action _awaitableIsNotCompleted = () => { };

            private Action _state;
            private readonly IScheduler _scheduler;

            public Awaitable(IScheduler scheduler, bool completed)
            {
                _state = completed ? _awaitableIsCompleted : _awaitableIsNotCompleted;
                _scheduler = scheduler;
            }

            public void Resume()
            {
                var awaitableState = Interlocked.Exchange(
                    ref _state,
                    _awaitableIsCompleted);

                if (!ReferenceEquals(awaitableState, _awaitableIsCompleted) &&
                    !ReferenceEquals(awaitableState, _awaitableIsNotCompleted))
                {
                    _scheduler.Schedule(awaitableState);
                }
            }

            public void Reset()
            {
                Interlocked.CompareExchange(
                    ref _state,
                    _awaitableIsNotCompleted,
                    _awaitableIsCompleted);
            }

            public bool IsCompleted => ReferenceEquals(_state, _awaitableIsCompleted);

            public void OnCompleted(Action continuation, ref Completion completion)
            {
                var awaitableState = Interlocked.CompareExchange(
                    ref _state,
                    continuation,
                    _awaitableIsNotCompleted);

                if (ReferenceEquals(awaitableState, _awaitableIsNotCompleted))
                {
                    return;
                }
                else if (ReferenceEquals(awaitableState, _awaitableIsCompleted))
                {
                    _scheduler.Schedule(continuation);
                }
                else
                {
                    completion.TryComplete(ThrowHelper.GetInvalidOperationException(ExceptionResource.NoConcurrentOperation));

                    Interlocked.Exchange(
                        ref _state,
                        _awaitableIsCompleted);

                    Task.Run(continuation);
                    Task.Run(awaitableState);
                }
            }
        }

        private struct Completion
        {
            private static readonly Exception _completedNoException = new Exception();

            private Exception _exception;

            public bool IsCompleted => _exception != null;

            public void TryComplete(Exception exception = null)
            {
                // Set the exception object to the exception passed in or a sentinel value
                Interlocked.CompareExchange(ref _exception, exception ?? _completedNoException, null);
            }

            public void ThrowIfFailed()
            {
                if (_exception != null && _exception != _completedNoException)
                {
                    throw _exception;
                }
            }
        }

        // Can't use enums with Interlocked
        private static class State
        {
            public static int NotActive = 0;
            public static int Active = 1;
        }

        private static class CancelledState
        {
            public static int NotCancelled = 0;
            public static int CancellationRequested = 1;
            public static int CancellationObserved = 2;
        }
    }
}
