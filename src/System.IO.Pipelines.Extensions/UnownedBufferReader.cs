// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Works in buffers which it does not own, as opposed to using a <see cref="IBufferPool"/>. Designed
    /// to allow Streams to be easily adapted to <see cref="IPipeReader"/> via <see cref="System.IO.Stream.CopyToAsync(System.IO.Stream)"/>
    /// </summary>
    public class UnownedBufferReader : IPipeReader, IReadableBufferAwaiter
    {
        private static readonly Action _awaitableIsCompleted = () => { };
        private static readonly Action _awaitableIsNotCompleted = () => { };

        private Action _awaitableState;

        private BufferSegment _head;
        private BufferSegment _tail;

        private bool _consuming;
        private int _cancelledState;

        // REVIEW: This object might be getting a little big :)
        private readonly TaskCompletionSource<object> _readingTcs = new TaskCompletionSource<object>();
        private readonly TaskCompletionSource<object> _writingTcs = new TaskCompletionSource<object>();
        private readonly TaskCompletionSource<object> _startingReadingTcs = new TaskCompletionSource<object>();

        private Gate _readWaiting = new Gate();

        /// <summary>
        /// Constructs a new instance of <see cref="UnownedBufferReader" />
        /// </summary>
        public UnownedBufferReader()
        {
            _awaitableState = _awaitableIsNotCompleted;
        }

        /// <summary>
        /// A <see cref="Task"/> that completes when the consumer starts consuming the <see cref="IPipeReader"/>.
        /// </summary>
        public Task ReadingStarted => _startingReadingTcs.Task;

        /// <summary>
        /// Gets a task that completes when no more data will be added to the pipeline.
        /// </summary>
        /// <remarks>This task indicates the producer has completed and will not write anymore data.</remarks>
        public Task Reading => _readingTcs.Task;

        /// <summary>
        /// Gets a task that completes when no more data will be read from the pipeline.
        /// </summary>
        /// <remarks>
        /// This task indicates the consumer has completed and will not read anymore data.
        /// When this task is triggered, the producer should stop producing data.
        /// </remarks>
        public Task Writing => _writingTcs.Task;

        bool IReadableBufferAwaiter.IsCompleted => IsCompleted;

        private bool IsCompleted => ReferenceEquals(_awaitableState, _awaitableIsCompleted);

        /// <summary>
        /// Writes a new buffer into the pipeline. The task returned by this operation only completes when the next
        /// Read has been queued, or the Reader has completed, since the buffer provided here needs to be kept alive
        /// until the matching Read finishes (because we don't have ownership tracking when working with unowned buffers)
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task WriteAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
        {
            using (var unowned = new UnownedBuffer(buffer))
            {
                await WriteAsync(unowned, cancellationToken);
            }
        }

        /// <summary>
        /// Writes a new buffer into the pipeline. The task returned by this operation only completes when the next
        /// Read has been queued, or the Reader has completed, since the buffer provided here needs to be kept alive
        /// until the matching Read finishes (because we don't have ownership tracking when working with unowned buffers)
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        // Called by the WRITER
        public async Task WriteAsync(OwnedBuffer<byte> buffer, CancellationToken cancellationToken)
        {
            // If Writing has stopped, why is the caller writing??
            if (Writing.Status != TaskStatus.WaitingForActivation)
            {
                throw new OperationCanceledException("Writing has ceased on this pipeline");
            }

            // If Reading has stopped, we cancel. We don't write unless there's a reader ready in this pipeline.
            if (Reading.Status != TaskStatus.WaitingForActivation)
            {
                throw new OperationCanceledException("Reading has ceased on this pipeline");
            }

            // Register for cancellation on this token for the duration of the write
            using (cancellationToken.Register(state => ((UnownedBufferReader)state).CancelWriter(), this))
            {
                // Wait for reading to start
                await ReadingStarted;

                // Cancel this task if this write is cancelled
                cancellationToken.ThrowIfCancellationRequested();

                // Allocate a new segment to hold the buffer being written.
                using (var segment = new BufferSegment(buffer))
                {
                    segment.End = buffer.Buffer.Length;

                    if (_head == null || _head.ReadableBytes == 0)
                    {
                        // Update the head to point to the head of the buffer.
                        _head = segment;
                    }
                    else if (_tail != null)
                    {
                        // Add this segment to the end of the chain
                        _tail.Next = segment;
                    }

                    // Always update tail to the buffer's tail
                    _tail = segment;

                    // Trigger the continuation
                    Complete();

                    // Wait for another read to come (or for the end of Reading, which will also trigger this gate to open) in before returning
                    await _readWaiting;

                    if (_head.ReadableBytes > 0)
                    {
                        // We need to preserve any buffers that haven't been consumed
                        _head = BufferSegment.Clone(new ReadCursor(_head), new ReadCursor(_tail, _tail?.End ?? 0), out _tail);
                    }
                }

                // Cancel this task if this write is cancelled
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        // Called by the WRITER via WriteAsync to run the READER
        private void Complete()
        {
            // Don't need to interlock here. We have one reader and one writer and the ReadingStarted/ReadWaiting gates
            // ensure that the read is blocked while we write and vice-versa.
            var awaitableState = _awaitableState;
            _awaitableState = _awaitableIsCompleted;

            if (!ReferenceEquals(awaitableState, _awaitableIsCompleted) &&
                !ReferenceEquals(awaitableState, _awaitableIsNotCompleted))
            {
                awaitableState();
            }
        }

        // Called by the READER
        private ReadableBuffer Read()
        {
            if (_consuming)
            {
                throw new InvalidOperationException("Cannot Read until the previous read has been acknowledged by calling Advance");
            }
            _consuming = true;

            return new ReadableBuffer(new ReadCursor(_head), new ReadCursor(_tail, _tail?.End ?? 0));
        }

        // Called by the READER
        void IPipeReader.Advance(ReadCursor consumed, ReadCursor examined)
        {
            BufferSegment returnStart = null;
            BufferSegment returnEnd = null;

            if (!consumed.IsDefault)
            {
                returnStart = _head;
                returnEnd = consumed.Segment;
                _head = consumed.Segment;
                _head.Start = consumed.Index;
            }

            // Again, we don't need an interlock here because Read and Write proceed serially.
            // REVIEW: examined.IsEnd (PipelineReaderWriter has changed this logic)
            var consumedEverything = examined.IsEnd &&
                                     Reading.Status == TaskStatus.WaitingForActivation &&
                                     _awaitableState == _awaitableIsCompleted;

            CompareExchange(ref _cancelledState, CancelledState.NotCancelled, CancelledState.CancellationObserved);

            if (consumedEverything && _cancelledState != CancelledState.CancellationRequested)
            {
                _awaitableState = _awaitableIsNotCompleted;
            }

            while (returnStart != returnEnd)
            {
                var returnSegment = returnStart;
                returnStart = returnStart.Next;
                returnSegment.Dispose();
            }

            if (!_consuming)
            {
                throw new InvalidOperationException("No ongoing consuming operation to complete.");
            }
            _consuming = false;
        }

        /// <summary>
        /// Signal to the producer that the consumer is done reading.
        /// </summary>
        /// <param name="exception">Optional Exception indicating a failure that's causing the pipeline to complete.</param>
        // Called by the READER
        void IPipeReader.Complete(Exception exception)
        {
            if (exception != null)
            {
                _writingTcs.TrySetException(exception);
            }
            else
            {
                _writingTcs.TrySetResult(null);
            }

            if (Reading.IsCompleted)
            {
                Dispose();
            }
        }

        /// <summary>
        /// Marks the pipeline as being complete, meaning no more items will be written to it.
        /// </summary>
        /// <param name="exception">Optional Exception indicating a failure that's causing the pipeline to complete.</param>
        // Called by the WRITER
        public void CompleteWriter(Exception exception = null)
        {
            if (exception != null)
            {
                _readingTcs.TrySetException(exception);
            }
            else
            {
                _readingTcs.TrySetResult(null);
            }

            // Fire the completion so the Reader knows the Writer has completed.
            Complete();

            if (Writing.IsCompleted)
            {
                Dispose();
            }
        }

        /// <summary>
        /// Cancel to currently pending call to <see cref="ReadAsync"/> without completing the <see cref="IPipeReader"/>.
        /// </summary>
        public void CancelPendingRead()
        {
            _cancelledState = CancelledState.CancellationRequested;

            Complete();
        }

        /// <summary>
        /// Asynchronously reads a sequence of bytes from the current <see cref="IPipeReader"/>.
        /// </summary>
        /// <returns>A <see cref="ReadableBufferAwaitable"/> representing the asynchronous read operation.</returns>
        // Called by the READER
        public ReadableBufferAwaitable ReadAsync()
        {
            return new ReadableBufferAwaitable(this);
        }

        // Called by the READER
        void IReadableBufferAwaiter.OnCompleted(Action continuation)
        {
            if (ReferenceEquals(_awaitableState, _awaitableIsNotCompleted))
            {
                // Register our continuation
                _awaitableState = continuation;

                // NOTE(anurse): NEVER open both gates at once, it can cause WriteAsync to unblock before a new reader has actually arrived.
                if (!_startingReadingTcs.TrySetResult(null))
                {
                    // We've already started reading, so open the ReadWaiting gate instead
                    _readWaiting.Open();
                }
            }
            else if (ReferenceEquals(_awaitableState, _awaitableIsCompleted))
            {
                // NOTE(anurse): This shouldn't happen because everything is serialized... IsCompleted will be true so the generated code will never call OnCompleted

                // Dispatch here to avoid stack diving
                continuation();

                // We don't open the ReadWaiting gate here because we are continuing to work with the previous buffer
                // (since _awaitableState was _awaitableIsCompleted)
            }
            else
            {
                _readingTcs.SetException(new InvalidOperationException("Concurrent reads are not supported."));

                Interlocked.Exchange(
                    ref _awaitableState,
                    _awaitableIsCompleted);

                Task.Run(continuation);
                Task.Run(_awaitableState);
            }

        }

        ReadResult IReadableBufferAwaiter.GetResult()
        {
            if (!IsCompleted)
            {
                throw new InvalidOperationException("can't GetResult unless completed");
            }

            var readingIsCancelled = CompareExchange(ref _cancelledState, CancelledState.CancellationObserved, CancelledState.CancellationRequested) == CancelledState.CancellationRequested;
            var readingIsCompleted = Reading.IsCompleted;
            if (readingIsCompleted)
            {
                // Observe any exceptions if the reading task is completed
                Reading.GetAwaiter().GetResult();
            }

            return new ReadResult(Read(), readingIsCancelled, readingIsCompleted);
        }

        private void Dispose()
        {
            Debug.Assert(Writing.IsCompleted, "Not completed writing");
            Debug.Assert(Reading.IsCompleted, "Not completed reading");

            // Return all segments
            var segment = _head;
            while (segment != null)
            {
                var returnSegment = segment;
                segment = segment.Next;

                returnSegment.Dispose();
            }

            _head = null;
            _tail = null;
        }

        // Called by the WRITER
        private void CancelWriter()
        {
            // Cancel the reader
            _readingTcs.TrySetCanceled();

            // Allow the reader to observe this
            Complete();

            // Allow the WriteAsync end to throw the OperationCanceledException
            _readWaiting.Open();

            if (Writing.IsCompleted)
            {
                Dispose();
            }
        }

        // A helper to keep the logic between this and PipelineReaderWriter as similar as possible
        private static int CompareExchange(ref int location, int value, int comparand)
        {
            var previous = location;
            if (location == comparand)
            {
                location = value;
            }

            return previous;
        }

        private static class CancelledState
        {
            public static int NotCancelled = 0;
            public static int CancellationRequested = 1;
            public static int CancellationObserved = 2;
        }
    }
}
