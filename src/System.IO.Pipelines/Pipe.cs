// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using System.Threading;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Default <see cref="IPipeWriter"/> and <see cref="IPipeReader"/> implementation.
    /// </summary>
    internal class Pipe : IPipe, IPipeReader, IPipeWriter, IReadableBufferAwaiter, IWritableBufferAwaiter
    {
        private static readonly Action<object> _signalReaderAwaitable = state => ((Pipe)state).ReaderCancellationRequested();
        private static readonly Action<object> _signalWriterAwaitable = state => ((Pipe)state).WriterCancellationRequested();

        // This sync objects protects the following state:
        // 1. _commitHead & _commitHeadIndex
        // 2. _length
        // 3. _readerAwaitable & _writerAwaitable
        private readonly object _sync = new object();

        private readonly BufferPool _pool;
        private readonly long _maximumSizeHigh;
        private readonly long _maximumSizeLow;

        private readonly IScheduler _readerScheduler;
        private readonly IScheduler _writerScheduler;

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

        private PipeOperationState _readingState;
        private PipeOperationState _writingState;

        private bool _disposed;

        internal long Length => _length;

        /// <summary>
        /// Initializes the <see cref="Pipe"/> with the specifed <see cref="IBufferPool"/>.
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="options"></param>
        public Pipe(BufferPool pool, PipeOptions options = null)
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
            _readerScheduler = options.ReaderScheduler ?? InlineScheduler.Default;
            _writerScheduler = options.WriterScheduler ?? InlineScheduler.Default;
            ResetState();
        }

        private void ResetState()
        {
            _readerAwaitable = new PipeAwaitable(completed: false);
            _writerAwaitable = new PipeAwaitable(completed: true);
            _readerCompletion = default(PipeCompletion);
            _writerCompletion = default(PipeCompletion);
            _commitHeadIndex = 0;
            _currentWriteLength = 0;
            _length = 0;
        }

        internal Buffer<byte> Buffer => _writingHead?.Buffer.Slice(_writingHead.End, _writingHead.WritableBytes) ?? Buffer<byte>.Empty;

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

            lock (_sync)
            {
                // CompareExchange not required as its setting to current value if test fails
                _writingState.Begin(ExceptionResource.AlreadyWriting);

                if (minimumSize > 0)
                {
                    try
                    {
                        AllocateWriteHeadUnsynchronized(minimumSize);
                    }
                    catch (Exception)
                    {
                        // Reset producing state if allocation failed
                        _writingState.End(ExceptionResource.NoWriteToComplete);
                        throw;
                    }
                }
                _currentWriteLength = 0;
                return new WritableBuffer(this);
            }
        }

        internal void Ensure(int count = 1)
        {
            EnsureAlloc();

            var segment = _writingHead;
            if (segment == null)
            {
                // Changing commit head shared with Reader
                lock (_sync)
                {
                    segment = AllocateWriteHeadUnsynchronized(count);
                }
            }

            var bytesLeftInBuffer = segment.WritableBytes;

            // If inadequate bytes left or if the segment is readonly
            if (bytesLeftInBuffer == 0 || bytesLeftInBuffer < count || segment.ReadOnly)
            {
                var nextBuffer = _pool.Rent(count);
                var nextSegment = new BufferSegment(nextBuffer);

                segment.Next = nextSegment;

                _writingHead = nextSegment;
            }
        }

        private BufferSegment AllocateWriteHeadUnsynchronized(int count)
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
                segment = new BufferSegment(_pool.Rent(count));
            }

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
                lock (_sync)
                {
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
            if (!_writingState.IsActive)
            {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.NotWritingNoAlloc);
            }
        }

        internal void Commit()
        {
            // Changing commit head shared with Reader
            lock (_sync)
            {
                CommitUnsynchronized();
            }
        }

        internal void CommitUnsynchronized()
        {
            _writingState.End(ExceptionResource.NoWriteToComplete);

            if (_writingHead == null)
            {
                // Nothing written to commit
                return;
            }

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
            // Clear the writing state
            _writingHead = null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void AdvanceWriter(int bytesWritten)
        {
            EnsureAlloc();
            if (bytesWritten > 0)
            {
                if (_writingHead == null)
                {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.AdvancingWithNoBuffer);
                }

                Debug.Assert(!_writingHead.ReadOnly);
                Debug.Assert(_writingHead.Next == null);

                var buffer = _writingHead.Buffer;
                var bufferIndex = _writingHead.End + bytesWritten;

                if (bufferIndex > buffer.Length)
                {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.AdvancingPastBufferSize);
                }

                _writingHead.End = bufferIndex;
                _currentWriteLength += bytesWritten;
            }
            else if (bytesWritten < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.bytesWritten);
            } // and if zero, just do nothing; don't need to validate tail etc
        }

        internal WritableBufferAwaitable FlushAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            Action awaitable;
            CancellationTokenRegistration cancellationTokenRegistration;
            lock (_sync)
            {
                if (_writingState.IsActive)
                {
                    // Commit the data as not already committed
                    CommitUnsynchronized();
                }

                awaitable = _readerAwaitable.Complete();

                cancellationTokenRegistration = _writerAwaitable.AttachToken(cancellationToken, _signalWriterAwaitable, this);
            }

            cancellationTokenRegistration.Dispose();

            TrySchedule(_readerScheduler, awaitable);

            return new WritableBufferAwaitable(this);
        }

        internal ReadableBuffer AsReadableBuffer()
        {
            if (_writingHead == null)
            {
                return new ReadableBuffer(); // Nothing written return empty
            }

            ReadCursor readStart;
            lock (_sync)
            {
                readStart = new ReadCursor(_commitHead, _commitHeadIndex);
            }

            return new ReadableBuffer(readStart, new ReadCursor(_writingHead, _writingHead.End));
        }

        /// <summary>
        /// Marks the pipeline as being complete, meaning no more items will be written to it.
        /// </summary>
        /// <param name="exception">Optional Exception indicating a failure that's causing the pipeline to complete.</param>
        void IPipeWriter.Complete(Exception exception)
        {
            if (_writingState.IsActive)
            {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.CompleteWriterActiveWriter, _writingState.Location);
            }

            _writerCompletion.TryComplete(exception);

            Action awaitable;

            lock (_sync)
            {
                awaitable = _readerAwaitable.Complete();
            }

            TrySchedule(_readerScheduler, awaitable);

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
                returnStart = _readHead;
                consumedBytes = ReadCursor.GetLength(returnStart, returnStart.Start, consumed.Segment, consumed.Index);

                returnEnd = consumed.Segment;
                _readHead = consumed.Segment;
                _readHead.Start = consumed.Index;
            }

            // Reading commit head shared with writer
            Action continuation = null;
            lock (_sync)
            {
                var oldLength = _length;
                _length -= consumedBytes;

                if (oldLength >= _maximumSizeLow &&
                    _length < _maximumSizeLow)
                {
                    continuation = _writerAwaitable.Complete();
                }

                // We reset the awaitable to not completed if we've examined everything the producer produced so far
                if (examined.Segment == _commitHead &&
                    examined.Index == _commitHeadIndex &&
                    !_writerCompletion.IsCompleted)
                {
                    if (!_writerAwaitable.IsCompleted)
                    {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.BackpressureDeadlock);
                    }
                    _readerAwaitable.Reset();
                }

                _readingState.End(ExceptionResource.NoReadToComplete);
            }

            while (returnStart != null && returnStart != returnEnd)
            {
                var returnSegment = returnStart;
                returnStart = returnStart.Next;
                returnSegment.Dispose();
            }

            TrySchedule(_writerScheduler, continuation);
        }

        /// <summary>
        /// Signal to the producer that the consumer is done reading.
        /// </summary>
        /// <param name="exception">Optional Exception indicating a failure that's causing the pipeline to complete.</param>
        void IPipeReader.Complete(Exception exception)
        {
            if (_readingState.IsActive)
            {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.CompleteReaderActiveReader, _readingState.Location);
            }

            _readerCompletion.TryComplete(exception);

            Action awaitable;
            lock (_sync)
            {
                awaitable = _writerAwaitable.Complete();
            }
            TrySchedule(_writerScheduler, awaitable);

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
            Action awaitable;
            lock (_sync)
            {
                awaitable = _readerAwaitable.Cancel();
            }
            TrySchedule(_readerScheduler, awaitable);
        }

        /// <summary>
        /// Cancel to currently pending call to <see cref="WritableBuffer.FlushAsync"/> without completing the <see cref="IPipeWriter"/>.
        /// </summary>
        void IPipeWriter.CancelPendingFlush()
        {
            Action awaitable;
            lock (_sync)
            {
                awaitable = _writerAwaitable.Cancel();
            }
            TrySchedule(_writerScheduler, awaitable);
        }

        ReadableBufferAwaitable IPipeReader.ReadAsync(CancellationToken token)
        {
            CancellationTokenRegistration cancellationTokenRegistration;
            if (_readerCompletion.IsCompleted)
            {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.NoReadingAllowed, _readerCompletion.Location);
            }
            lock (_sync)
            {
                cancellationTokenRegistration= _readerAwaitable.AttachToken(token, _signalReaderAwaitable, this);
            }
            cancellationTokenRegistration.Dispose();
            return new ReadableBufferAwaitable(this);
        }

        bool IPipeReader.TryRead(out ReadResult result)
        {
            lock (_sync)
            {
                if (_readerCompletion.IsCompleted)
                {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.NoReadingAllowed, _readerCompletion.Location);
                }

                result = new ReadResult();
                if (_length > 0 || _readerAwaitable.IsCompleted)
                {
                    GetResult(ref result);
                    return true;
                }

                if(_readerAwaitable.HasContinuation)
                {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.AlreadyReading);
                }
                return false;
            }
        }

        private static void TrySchedule(IScheduler scheduler, Action action)
        {
            if (action != null)
            {
                scheduler.Schedule(action);
            }
        }

        private void Dispose()
        {
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
            Action awaitable;
            lock (_sync)
            {
                awaitable = _readerAwaitable.OnCompleted(continuation, ref _writerCompletion);
            }
            TrySchedule(_readerScheduler, awaitable);
        }

        ReadResult IReadableBufferAwaiter.GetResult()
        {
            if (!_readerAwaitable.IsCompleted)
            {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.GetResultNotCompleted);
            }

            var result = new ReadResult();
            lock (_sync)
            {
                GetResult(ref result);
            }
            return result;
        }

        private void GetResult(ref ReadResult result)
        {
            if (_writerCompletion.IsCompletedOrThrow())
            {
                result.ResultFlags |= ResultFlags.Completed;
            }

            var isCancelled = _readerAwaitable.ObserveCancelation();
            if (isCancelled)
            {
                result.ResultFlags |= ResultFlags.Cancelled;
            }

            // No need to read end if there is no head
            var head = _readHead;

            if (head != null)
            {
                // Reading commit head shared with writer
                result.ResultBuffer.BufferEnd.Segment = _commitHead;
                result.ResultBuffer.BufferEnd.Index = _commitHeadIndex;
                result.ResultBuffer.BufferLength = ReadCursor.GetLength(head, head.Start, _commitHead, _commitHeadIndex);

                result.ResultBuffer.BufferStart.Segment = head;
                result.ResultBuffer.BufferStart.Index = head.Start;
            }

            if (isCancelled)
            {
                _readingState.BeginTentative(ExceptionResource.AlreadyReading);
            }
            else
            {
                _readingState.Begin(ExceptionResource.AlreadyReading);
            }
        }

        // IWritableBufferAwaiter members

        bool IWritableBufferAwaiter.IsCompleted => _writerAwaitable.IsCompleted;

        FlushResult IWritableBufferAwaiter.GetResult()
        {
            var result = new FlushResult();
            lock (_sync)
            {
                if (!_writerAwaitable.IsCompleted)
                {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.GetResultNotCompleted);
                }

                // Change the state from to be cancelled -> observed
                if (_writerAwaitable.ObserveCancelation())
                {
                    result.ResultFlags |= ResultFlags.Cancelled;
                }
                if (_readerCompletion.IsCompletedOrThrow())
                {
                    result.ResultFlags |= ResultFlags.Completed;
                }
            }

            return result;
        }

        void IWritableBufferAwaiter.OnCompleted(Action continuation)
        {
            Action awaitable;
            lock (_sync)
            {
                awaitable = _writerAwaitable.OnCompleted(continuation, ref _readerCompletion);
            }
            TrySchedule(_writerScheduler, awaitable);
        }

        private void ReaderCancellationRequested()
        {
            Action action;
            lock (_sync)
            {
                action = _readerAwaitable.Cancel();
            }
            TrySchedule(_readerScheduler, action);
        }

        private void WriterCancellationRequested()
        {
            Action action;
            lock (_sync)
            {
                action = _writerAwaitable.Cancel();
            }
            TrySchedule(_writerScheduler, action);
        }

        public IPipeReader Reader => this;
        public IPipeWriter Writer => this;

        public void Reset()
        {
            if (!_disposed)
            {
                throw new InvalidOperationException("Both reader and writer need to be completed to be able to reset ");
            }

            _disposed = false;
            ResetState();
        }
    }
}
