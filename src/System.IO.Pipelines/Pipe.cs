// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Default <see cref="IPipeWriter"/> and <see cref="IPipeReader"/> implementation.
    /// </summary>
    internal class Pipe : IPipe, IPipeReader, IPipeWriter, IReadableBufferAwaiter, IWritableBufferAwaiter
    {
        private readonly object _sync = new object();

        private readonly IBufferPool _pool;
        private readonly long _maximumSizeHigh;
        private readonly long _maximumSizeLow;

        private long _length;
        private long _currentWriteLength;

        private PipeAwaitable _readerAwaitable;
        private PipeAwaitable _writerAwaitable;

        private PipeCompletion _writerCompletion;
        private PipeCompletion _readerCompletion;

        // The read head which is the extent of the IPipelineReader's consumed bytes
        private BufferSegment _readHead;

        // The commit head which is the extent of the bytes available to the IPipelineReader to consume
        private BufferSegment _commitHead;
        private int _commitHeadIndex;

        // The write head which is the extent of the IPipelineWriter's written bytes
        private BufferSegment _writingHead;

        private PipeOperationState _consumingState;
        private PipeOperationState _producingState;

        private bool _disposed;

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

            _readerAwaitable = new PipeAwaitable(options.ReaderScheduler ?? InlineScheduler.Default, completed: false);
            _writerAwaitable = new PipeAwaitable(options.WriterScheduler ?? InlineScheduler.Default, completed: true);
        }

        internal Memory<byte> Memory => _writingHead?.Memory.Slice(_writingHead.End, _writingHead.WritableBytes) ?? Memory<byte>.Empty;

        /// <summary>
        /// Allocates memory from the pipeline to write into.
        /// </summary>
        /// <param name="minimumSize">The minimum size buffer to allocate</param>
        /// <returns>A <see cref="WritableBuffer"/> that can be written to.</returns>
        WritableBuffer IPipeWriter.Alloc(int minimumSize)
        {
            if (_writerCompletion.IsCompleted)
            {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.NoWritingAllowed, _writerCompletion.Location);
            }

            if (minimumSize < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.minimumSize);
            }

            // CompareExchange not required as its setting to current value if test fails
            _producingState.Begin(ExceptionResource.AlreadyProducing);

            if (minimumSize > 0)
            {
                try
                {
                    AllocateWriteHead(minimumSize);
                }
                catch (Exception)
                {
                    // Reset producing state if allocation failed
                    _producingState.End(ExceptionResource.NotProducingToComplete);
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
            if (!_producingState.IsActive)
            {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.NotProducingNoAlloc);
            }
        }

        internal void Commit()
        {
            // CompareExchange not required as its setting to current value if test fails
            _producingState.End(ExceptionResource.NotProducingToComplete);

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
            }

            var currentLenght = Interlocked.Add(ref _length, _currentWriteLength);

                // Do not reset if reader is complete
                if (_maximumSizeHigh > 0 &&
                currentLenght >= _maximumSizeHigh &&
                    !_readerCompletion.IsCompleted)
                {
                    _writerAwaitable.Reset();
                }

            // Clear the writing state
            _writingHead = null;
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
            if (_producingState.IsActive)
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
            _consumingState.Begin(ExceptionResource.AlreadyConsuming);

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
            if (_producingState.IsActive)
            {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.CompleteWriterActiveProducer, _producingState.Location);
            }

                _writerCompletion.TryComplete(exception);

                _readerAwaitable.Resume();

                if (_readerCompletion.IsCompleted)
                {
                    Dispose();
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

            var currentLength = Interlocked.Add(ref _length, -consumedBytes);
            // Change the state from observed -> not cancelled. We only want to reset the cancelled state if it was observed
            Interlocked.CompareExchange(ref _cancelledState, CancelledState.NotCancelled, CancelledState.CancellationObserved);

            // Reading commit head shared with writer
            bool consumedEverything;
            lock (_sync)
            {


                var consumedEverything = examined.Segment == _commitHead &&
                                         examined.Index == _commitHeadIndex &&
                                         !_writerCompletion.IsCompleted;

                // We reset the awaitable to not completed if
                // 1. We've consumed everything the producer produced so far
                // 2. Cancellation wasn't requested
                if (consumedEverything)
                {
                    _readerAwaitable.Reset();
                }

            while (returnStart != null && returnStart != returnEnd)
            {
                var returnSegment = returnStart;
                returnStart = returnStart.Next;
                returnSegment.Dispose();
            }

            // CompareExchange not required as its setting to current value if test fails
            _consumingState.End(ExceptionResource.NotConsumingToComplete);

            if (currentLength < _maximumSizeLow)
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
            if (_consumingState.IsActive)
            {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.CompleteReaderActiveConsumer, _consumingState.Location);
            }

                _readerCompletion.TryComplete(exception);

                _writerAwaitable.Resume();

                if (_writerCompletion.IsCompleted)
                {
                    Dispose();
                }
            }

        /// <summary>
        /// Cancel to currently pending call to <see cref="ReadAsync"/> without completing the <see cref="IPipeReader"/>.
        /// </summary>
        void IPipeReader.CancelPendingRead()
        {
                _readerAwaitable.Cancel();
            }

        /// <summary>
        /// Asynchronously reads a sequence of bytes from the current <see cref="IPipeReader"/>.
        /// </summary>
        /// <returns>A <see cref="PipeAwaitable"/> representing the asynchronous read operation.</returns>
        ReadableBufferAwaitable IPipeReader.ReadAsync()
        {
            if (_readerCompletion.IsCompleted)
            {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.NoReadingAllowed, _readerCompletion.Location);
            }

            return new ReadableBufferAwaitable(this);
        }

        private static void GetResult(ref PipeAwaitable awaitableState,
            ref PipeCompletion completion,
            out bool isCancelled,
            out bool isCompleted)
        {
            if (!awaitableState.IsCompleted)
            {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.GetResultNotCompleted);
            }

            // Change the state from to be cancelled -> observed
            isCancelled = awaitableState.ObserveCancelation();
            isCompleted = completion.IsCompleted;
            completion.ThrowIfFailed();
        }

        private void Dispose()
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

        // IReadableBufferAwaiter members

        bool IReadableBufferAwaiter.IsCompleted => _readerAwaitable.IsCompleted;

        void IReadableBufferAwaiter.OnCompleted(Action continuation)
        {
            _readerAwaitable.OnCompleted(continuation, ref _writerCompletion);
        }

        ReadResult IReadableBufferAwaiter.GetResult()
        {
            GetResult(ref _readerAwaitable,
                ref _writerCompletion,
                out bool isCancelled,
                out bool isCompleted);
            return new ReadResult(Read(), isCancelled, isCompleted);
        }

        // IWritableBufferAwaiter members

        bool IWritableBufferAwaiter.IsCompleted => _writerAwaitable.IsCompleted;

        bool IWritableBufferAwaiter.GetResult()
        {
            GetResult(ref _writerAwaitable,
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
    }
}
