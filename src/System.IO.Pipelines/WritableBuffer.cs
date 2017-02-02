// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Buffers;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Represents a buffer that can write a sequential series of bytes.
    /// </summary>
    public struct WritableBuffer : IOutput
    {
        private Pipe _pipe;

        internal WritableBuffer(Pipe pipe)
        {
            _pipe = pipe;
        }

        /// <summary>
        /// Available memory.
        /// </summary>
        public Memory<byte> Memory => _pipe.Memory;

        /// <summary>
        /// Returns the number of bytes currently written and uncommitted.
        /// </summary>
        public int BytesWritten => AsReadableBuffer().Length;

        Span<byte> IOutput.Buffer => Memory.Span;

        void IOutput.Enlarge(int desiredBufferLength) => Ensure(NewSize(desiredBufferLength));

        private int NewSize(int desiredBufferLength)
        {
            var currentSize = Memory.Length;
            if(desiredBufferLength == 0) {
                if (currentSize <= 0) return 256;
                else return currentSize * 2;
            }
            return desiredBufferLength < currentSize ? currentSize : desiredBufferLength;
        }

        /// <summary>
        /// Obtain a readable buffer over the data written but uncommitted to this buffer.
        /// </summary>
        public ReadableBuffer AsReadableBuffer()
        {
            return _pipe.AsReadableBuffer();
        }

        /// <summary>
        /// Ensures the specified number of bytes are available.
        /// Will assign more memory to the <see cref="WritableBuffer"/> if requested amount not currently available.
        /// </summary>
        /// <param name="count">number of bytes</param>
        /// <remarks>
        /// Used when writing to <see cref="Memory"/> directly. 
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        /// More requested than underlying <see cref="IBufferPool"/> can allocate in a contiguous block.
        /// </exception>
        public void Ensure(int count = 1)
        {
            _pipe.Ensure(count);
        }

        /// <summary>
        /// Appends the <see cref="ReadableBuffer"/> to the <see cref="WritableBuffer"/> in-place without copies.
        /// </summary>
        /// <param name="buffer">The <see cref="ReadableBuffer"/> to append</param>
        public void Append(ReadableBuffer buffer)
        {
            _pipe.Append(buffer);
        }

        /// <summary>
        /// Moves forward the underlying <see cref="IPipelineWriter"/>'s write cursor but does not commit the data.
        /// </summary>
        /// <param name="bytesWritten">number of bytes to be marked as written.</param>
        /// <remarks>Forwards the start of available <see cref="Memory"/> by <paramref name="bytesWritten"/>.</remarks>
        /// <exception cref="ArgumentException"><paramref name="bytesWritten"/> is larger than the current data available data.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bytesWritten"/> is negative.</exception>
        public void Advance(int bytesWritten)
        {
            _pipe.AdvanceWriter(bytesWritten);
        }

        /// <summary>
        /// Commits all outstanding written data to the underlying <see cref="IPipelineWriter"/> so they can be read
        /// and seals the <see cref="WritableBuffer"/> so no more data can be committed.
        /// </summary>
        /// <remarks>
        /// While an on-going conncurent read may pick up the data, <see cref="FlushAsync"/> should be called to signal the reader.
        /// </remarks>
        public void Commit()
        {
            _pipe.Commit();
        }

        /// <summary>
        /// Signals the <see cref="IPipelineReader"/> data is available.
        /// Will <see cref="Commit"/> if necessary.
        /// </summary>
        /// <returns>A task that completes when the data is fully flushed.</returns>
        public WritableBufferAwaitable FlushAsync()
        {
            return _pipe.FlushAsync();
        }
    }
}
