// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Represents a buffer that can write a sequential series of bytes.
    /// </summary>
    public struct WritableBuffer
    {
        internal Pipe Pipe { get; }

        internal WritableBuffer(Pipe pipe)
        {
            Pipe = pipe;
        }

        /// <summary>
        /// Available memory.
        /// </summary>
        public Memory<byte> Buffer => Pipe.Buffer;

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
        public void Ensure(int count = 1)
        {
            Pipe.Ensure(count);
        }

        /// <summary>
        /// Appends the <see cref="ReadableBuffer"/> to the <see cref="WritableBuffer"/> in-place without copies.
        /// </summary>
        /// <param name="buffer">The <see cref="ReadableBuffer"/> to append</param>
        public void Append(ReadableBuffer buffer)
        {
            Pipe.Append(buffer);
        }

        /// <summary>
        /// Moves forward the underlying <see cref="IPipeWriter"/>'s write cursor but does not commit the data.
        /// </summary>
        /// <param name="bytesWritten">number of bytes to be marked as written.</param>
        /// <remarks>Forwards the start of available <see cref="Buffer"/> by <paramref name="bytesWritten"/>.</remarks>
        /// <exception cref="ArgumentException"><paramref name="bytesWritten"/> is larger than the current data available data.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bytesWritten"/> is negative.</exception>
        public void Advance(int bytesWritten)
        {
            Pipe.Advance(bytesWritten);
        }

        /// <summary>
        /// Commits all outstanding written data to the underlying <see cref="IPipeWriter"/> so they can be read
        /// and seals the <see cref="WritableBuffer"/> so no more data can be committed.
        /// </summary>
        /// <remarks>
        /// While an on-going concurrent read may pick up the data, <see cref="FlushAsync"/> should be called to signal the reader.
        /// </remarks>
        public void Commit()
        {
            Pipe.Commit();
        }

        /// <summary>
        /// Signals the <see cref="IPipeReader"/> data is available.
        /// Will <see cref="Commit"/> if necessary.
        /// </summary>
        /// <returns>A task that completes when the data is fully flushed.</returns>
        public WritableBufferAwaitable FlushAsync(CancellationToken cancellationToken = default)
        {
            return Pipe.FlushAsync(cancellationToken);
        }
    }
}
