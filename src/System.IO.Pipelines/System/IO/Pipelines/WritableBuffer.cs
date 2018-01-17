// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Threading;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Represents a buffer that can write a sequential series of bytes.
    /// </summary>
    public struct WritableBuffer: IOutput
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
        public ValueAwaiter<FlushResult> FlushAsync(CancellationToken cancellationToken = default)
        {
            return Pipe.FlushAsync(cancellationToken);
        }

        /// <summary>
        /// Writes the source <see cref="ReadOnlySpan{Byte}"/> to the <see cref="WritableBuffer"/>.
        /// </summary>
        /// <param name="buffer">The <see cref="WritableBuffer"/></param>
        /// <param name="source">The <see cref="ReadOnlySpan{Byte}"/> to write</param>
        public void Write(ReadOnlySpan<byte> source)
        {
            if (Buffer.IsEmpty)
            {
                Ensure();
            }

            // Fast path, try copying to the available memory directly
            if (source.Length <= Buffer.Length)
            {
                source.CopyTo(Buffer.Span);
                Advance(source.Length);
                return;
            }

            var remaining = source.Length;
            var offset = 0;

            while (remaining > 0)
            {
                var writable = Math.Min(remaining, Buffer.Length);

                Ensure(writable);

                if (writable == 0)
                {
                    continue;
                }

                source.Slice(offset, writable).CopyTo(Buffer.Span);

                remaining -= writable;
                offset += writable;

                Advance(writable);
            }
        }

        Memory<byte> IOutput.GetMemory(int minimumLength)
        {
            Pipe.Ensure(minimumLength);
            return Buffer;
        }

        Span<byte> IOutput.GetSpan(int minimumLength) => ((IOutput) this).GetMemory(minimumLength).Span;
    }
}
