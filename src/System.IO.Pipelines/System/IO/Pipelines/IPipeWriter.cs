// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Defines a class that provides a pipeline to which data can be written.
    /// </summary>
    public interface IPipeWriter
    {
        /// <summary>
        /// Allocates memory from the pipeline to write into.
        /// </summary>
        /// <param name="minimumSize">The minimum size buffer to allocate</param>
        /// <returns>A <see cref="WritableBuffer"/> that can be written to.</returns>
        [Obsolete("Use "  + nameof(Allocate ) + " and  members of " + nameof(IPipeWriter) + " directly")]
        WritableBuffer Alloc(int minimumSize = 0);

        /// <summary>
        /// Allocates memory from the pipeline to write into.
        /// </summary>
        /// <param name="minimumSize">The minimum size buffer to allocate</param>
        void Allocate(int minimumSize = 0);

        /// <summary>
        /// Ensures the specified number of bytes are available.
        /// Will assign more memory to the <see cref="WritableBuffer"/> if requested amount not currently available.
        /// </summary>
        /// <param name="count">number of bytes</param>
        /// <remarks>
        /// Used when writing to <see cref="Buffer"/> directly.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        /// More requested than underlying <see cref="IBufferPool"/> can allocate in a contiguous block.
        /// </exception>
        void Ensure(int count = 1);

        /// <summary>
        /// Moves forward the underlying <see cref="IPipeWriter"/>'s write cursor but does not commit the data.
        /// </summary>
        /// <param name="bytesWritten">number of bytes to be marked as written.</param>
        /// <remarks>Forwards the start of available <see cref="Buffer"/> by <paramref name="bytesWritten"/>.</remarks>
        /// <exception cref="ArgumentException"><paramref name="bytesWritten"/> is larger than the current data available data.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bytesWritten"/> is negative.</exception>
        void Advance(int bytesWritten);

        /// <summary>
        /// Appends the <see cref="ReadableBuffer"/> to the <see cref="WritableBuffer"/> in-place without copies.
        /// </summary>
        /// <param name="buffer">The <see cref="ReadableBuffer"/> to append</param>
        void Append(ReadableBuffer buffer);

        /// <summary>
        /// Commits all outstanding written data to the underlying <see cref="IPipeWriter"/> so they can be read
        /// and seals the <see cref="WritableBuffer"/> so no more data can be committed.
        /// </summary>
        /// <remarks>
        /// While an on-going concurrent read may pick up the data, <see cref="FlushAsync"/> should be called to signal the reader.
        /// </remarks>
        void Commit();

        /// <summary>
        /// Signals the <see cref="IPipeReader"/> data is available.
        /// Will <see cref="Commit"/> if necessary.
        /// </summary>
        /// <returns>A task that completes when the data is fully flushed.</returns>
        WritableBufferAwaitable FlushAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Available memory.
        /// </summary>
        Buffer<byte> Buffer { get; }

        /// <summary>
        /// Marks the pipeline as being complete, meaning no more items will be written to it.
        /// </summary>
        /// <param name="exception">Optional Exception indicating a failure that's causing the pipeline to complete.</param>
        void Complete(Exception exception = null);

        /// <summary>
        /// Cancel to currently pending or next call to <see cref="ReadAsync"/> if none is pending, without completing the <see cref="IPipeReader"/>.
        /// </summary>
        void CancelPendingFlush();

        /// <summary>
        /// Registers callback that gets executed when reader side of pipe completes.
        /// </summary>
        void OnReaderCompleted(Action<Exception, object> callback, object state);
    }
}
