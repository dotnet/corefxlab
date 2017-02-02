// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Default <see cref="IPipelineWriter"/> and <see cref="IPipelineReader"/> implementation.
    /// </summary>
    public class Pipe : IPipelineReader, IPipelineWriter, IReadableBufferAwaiter, IWritableBufferAwaiter
    {
        private static readonly Action _awaitableIsCompleted = () => { };
        private static readonly Action _awaitableIsNotCompleted = () => { };

        private readonly IBufferPool _pool;
        private readonly IScheduler _readerScheduler;
        private readonly IScheduler _writerScheduler;

        private readonly long _maximumSizeHigh;
        private readonly long _maximumSizeLow;

        private Action _readerCallback;
        private Action _writerCallback;

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

        // REVIEW: This object might be getting a little big :)
        private readonly TaskCompletionSource<object> _readingTcs = new TaskCompletionSource<object>();
        private readonly TaskCompletionSource<object> _writingTcs = new TaskCompletionSource<object>();
        private readonly TaskCompletionSource<object> _startingReadingTcs = new TaskCompletionSource<object>();
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
            _readerScheduler = options.ReaderScheduler ?? InlineScheduler.Default;
            _writerScheduler = options.WriterScheduler ?? InlineScheduler.Default;
            _maximumSizeHigh = options.MaximumSizeHigh;
            _maximumSizeLow = options.MaximumSizeLow;
            _readerCallback = _awaitableIsNotCompleted;
            _writerCallback = _awaitableIsCompleted;
        }

        /// <summary>
        /// A <see cref="Task"/> that completes when the consumer starts consuming the <see cref="IPipelineReader"/>.
        /// </summary>
        public Task ReadingStarted => _startingReadingTcs.Task;

        /// <summary>
        /// Gets a task that completes when no more data will be added to the pipeline.
        /// </summary>
        /// <remarks>This task indicates the producer has completed and will not write anymore data.</remarks>
        private Task Reading => _readingTcs.Task;

        /// <summary>
        /// Gets a task that completes when no more data will be read from the pipeline.
        /// </summary>
        /// <remarks>
        /// This task indicates the consumer has completed and will not read anymore data.
        /// When this task is triggered, the producer should stop producing data.
        /// </remarks>
        public Task Writing => _writingTcs.Task;

        internal Memory<byte> Memory => _writingHead?.Memory.Slice(_writingHead.End, _writingHead.WritableBytes) ?? Memory<byte>.Empty;

        /// <summary>
        /// Allocates memory from the pipeline to write into.
        /// </summary>
        /// <param name="minimumSize">The minimum size buffer to allocate</param>
        /// <returns>A <see cref="WritableBuffer"/> that can be written to.</returns>
        public WritableBuffer Alloc(int minimumSize = 0)
        {
            // CompareExchange not required as its setting to current value if test fails
            if (Interlocked.Exchange(ref _producingState, State.Active) != State.NotActive)
            {

                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.AlreadyProducing
#if PRODUCING_LOCATION_TRACKING
                    , _producingLocation
#endif
                    );
            }

#if PRODUCING_LOCATION_TRACKING
            _producingLocation = Environment.StackTrace;
#endif
            if (Reading.IsCompleted)
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
            else if (minimumSize > 0)
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
                // Do not reset when writing is complete
                if (_maximumSizeHigh > 0 &&
                    _length >= _maximumSizeHigh &&
                    !Writing.IsCompleted)
                {
                    Reset(ref _writerCallback);
                }
            }

            // Clear the writing state
            _writingHead = null;
#if PRODUCING_LOCATION_TRACKING
            _producingLocation = null;
#endif
        }

        public void AdvanceWriter(int bytesWritten)
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

            // TODO: Can factor out this lock
            lock (_sync)
            {
                Resume(_readerScheduler, ref _readerCallback);
            }

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

        void IPipelineWriter.Complete(Exception exception) => CompleteWriter(exception);

        /// <summary>
        /// Marks the pipeline as being complete, meaning no more items will be written to it.
        /// </summary>
        /// <param name="exception">Optional Exception indicating a failure that's causing the pipeline to complete.</param>
        public void CompleteWriter(Exception exception = null)
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
                SignalReader(exception);

                if (Writing.IsCompleted)
                {
                    Dispose();
                }
            }
        }

        public void AdvanceReader(ReadCursor consumed, ReadCursor examined)
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
                                         Reading.Status == TaskStatus.WaitingForActivation;

                // We reset the awaitable to not completed if
                // 1. We've consumed everything the producer produced so far
                // 2. Cancellation wasn't requested
                if (consumedEverything && _cancelledState != CancelledState.CancellationRequested)
                {
                    Reset(ref _readerCallback);
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
                Resume(_writerScheduler, ref _writerCallback);
            }
        }

        private void SignalWriter(Exception exception)
        {
            if (exception != null)
            {
                _writingTcs.TrySetException(exception);
            }
            else
            {
                _writingTcs.TrySetResult(null);
            }
        }

        // Reading

        void IPipelineReader.Complete(Exception exception) => CompleteReader(exception);

        void IPipelineReader.Advance(ReadCursor consumed, ReadCursor examined) => AdvanceReader(consumed, examined);

        /// <summary>
        /// Signal to the producer that the consumer is done reading.
        /// </summary>
        /// <param name="exception">Optional Exception indicating a failure that's causing the pipeline to complete.</param>
        public void CompleteReader(Exception exception = null)
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
                // Trigger this if it's never been triggered
                _startingReadingTcs.TrySetResult(null);

                SignalWriter(exception);

                Resume(_writerScheduler, ref _writerCallback);

                if (Reading.IsCompleted)
                {
                    Dispose();
                }
            }
        }

        /// <summary>
        /// Cancel to currently pending call to <see cref="ReadAsync"/> without completing the <see cref="IPipelineReader"/>.
        /// </summary>
        public void CancelPendingRead()
        {
            // TODO: Can factor out this lock
            lock (_sync)
            {
                // Mark reading is cancellable
                _cancelledState = CancelledState.CancellationRequested;

                Resume(_readerScheduler, ref _readerCallback);
            }
        }

        /// <summary>
        /// Asynchronously reads a sequence of bytes from the current <see cref="IPipelineReader"/>.
        /// </summary>
        /// <returns>A <see cref="PipeAwaitable"/> representing the asynchronous read operation.</returns>
        public ReadableBufferAwaitable ReadAsync()
        {
            if (Writing.IsCompleted)
            {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.NoReadingAllowed
#if COMPLETION_LOCATION_TRACKING
                    , _completeReaderLocation
#endif
                    );
            }
            _startingReadingTcs.TrySetResult(null);

            return new ReadableBufferAwaitable(this);
        }

        private void SignalReader(Exception exception)
        {
            if (exception != null)
            {
                _readingTcs.TrySetException(exception);
            }
            else
            {
                _readingTcs.TrySetResult(null);
            }
        }

        // Awaiter support members

        private static bool IsCompleted(Action awaitableState) => ReferenceEquals(awaitableState, _awaitableIsCompleted);

        private static void OnCompleted(Action continuation, IScheduler scheduler, ref Action action, TaskCompletionSource<object> taskCompletionSource)
        {
            var awaitableState = Interlocked.CompareExchange(
                ref action,
                continuation,
                _awaitableIsNotCompleted);

            if (ReferenceEquals(awaitableState, _awaitableIsNotCompleted))
            {
                return;
            }
            else if (ReferenceEquals(awaitableState, _awaitableIsCompleted))
            {
                scheduler.Schedule(continuation);
            }
            else
            {
                taskCompletionSource.SetException(ThrowHelper.GetInvalidOperationException(ExceptionResource.NoConcurrentOperation));

                Interlocked.Exchange(
                    ref action,
                    _awaitableIsCompleted);

                Task.Run(continuation);
                Task.Run(awaitableState);
            }
        }

        private static void GetResult(Action awaitableState,
            ref int cancelledState,
            Task task,
            out bool readingIsCancelled,
            out bool readingIsCompleted)
        {
            if (!IsCompleted(awaitableState))
            {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.GetResultNotCompleted);
            }

            // Change the state from to be cancelled -> observed
            readingIsCancelled = Interlocked.CompareExchange(
                ref cancelledState,
                CancelledState.CancellationObserved,
                CancelledState.CancellationRequested) == CancelledState.CancellationRequested;

            readingIsCompleted = task.IsCompleted;
            if (readingIsCompleted)
            {
                // Observe any exceptions if the reading task is completed
                task.GetAwaiter().GetResult();
            }
        }

        private static void Resume(IScheduler scheduler, ref Action state)
        {
            var awaitableState = Interlocked.Exchange(
                ref state,
                _awaitableIsCompleted);

            if (!ReferenceEquals(awaitableState, _awaitableIsCompleted) &&
                !ReferenceEquals(awaitableState, _awaitableIsNotCompleted))
            {
                scheduler.Schedule(awaitableState);
            }
        }

        private static void Reset(ref Action awaitableState)
        {
            Interlocked.CompareExchange(
                ref awaitableState,
                _awaitableIsNotCompleted,
                _awaitableIsCompleted);
        }

        private void Dispose()
        {
            Debug.Assert(Writing.IsCompleted, "Not completed writing");
            Debug.Assert(Reading.IsCompleted, "Not completed reading");

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

        bool IReadableBufferAwaiter.IsCompleted => IsCompleted(_readerCallback);

        void IReadableBufferAwaiter.OnCompleted(Action continuation)
        {
            OnCompleted(continuation, _readerScheduler, ref _readerCallback, _readingTcs);
        }

        ReadResult IReadableBufferAwaiter.GetResult()
        {
            bool readingIsCancelled;
            bool readingIsCompleted;
            GetResult(_readerCallback, ref _cancelledState, Reading, out readingIsCancelled, out readingIsCompleted);
            return new ReadResult(Read(), readingIsCancelled, readingIsCompleted);
        }

        // IFlushAwaiter members

        bool IWritableBufferAwaiter.IsCompleted => IsCompleted(_writerCallback);

        void IWritableBufferAwaiter.GetResult()
        {
            bool readingIsCancelled;
            bool readingIsCompleted;
            int cancelledState = CancelledState.NotCancelled;
            GetResult(_writerCallback, ref cancelledState, _writingTcs.Task, out readingIsCancelled, out readingIsCompleted);
        }

        void IWritableBufferAwaiter.OnCompleted(Action continuation)
        {
            OnCompleted(continuation, _writerScheduler, ref _writerCallback, _writingTcs);
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
