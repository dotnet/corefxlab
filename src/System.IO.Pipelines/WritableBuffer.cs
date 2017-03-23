// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Runtime.CompilerServices;

namespace System.IO.Pipelines
{
    /// <summary>
    /// StackOnly abstraction for fast forward only writes
    /// </summary>
    public struct WritableBufferWriter
    {
        private readonly Pipe _pipe;
        private Span<byte> _span;

        public WritableBufferWriter(WritableBuffer writableBuffer) : this(writableBuffer.Pipe)
        {
        }

        internal WritableBufferWriter(Pipe pipe)
        {
            _pipe = pipe;
            _span = pipe.Buffer.Span;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void WriteFast(byte[] source, int offset, int length)
        {
            var sourceLength = length;
            if (sourceLength <= _span.Length)
            {
                ref byte pSource = ref source[offset];
                ref byte pDest = ref _span.DangerousGetPinnableReference();
                Unsafe.CopyBlockUnaligned(ref pDest, ref pSource, (uint)sourceLength);
                _span = _span.Slice(sourceLength);
                _pipe.AdvanceWriter(sourceLength);
                return;
            }

            WriteMultiBuffer(source, offset, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void CheckDaSpan()
        {
            _pipe.Ensure();
            _span = _pipe.Buffer.Span;
        }

        private void WriteMultiBuffer(byte[] source, int offset, int length)
        {
            var remaining = length;

            while (remaining > 0)
            {
                var writable = Math.Min(remaining, _span.Length);

                ref byte pSource = ref source[offset];
                ref byte pDest = ref _span.DangerousGetPinnableReference();

                Unsafe.CopyBlockUnaligned(ref pDest, ref pSource, (uint)writable);

                remaining -= writable;
                offset += writable;

                _pipe.AdvanceWriter(writable);

                CheckDaSpan();
            }
        }
    }

    /// <summary>
    /// Represents a buffer that can write a sequential series of bytes.
    /// </summary>
    public struct WritableBuffer : IOutput
    {
        internal WritableBuffer(Pipe pipe)
        {
            Pipe = pipe;
        }

        /// <summary>
        /// Available memory.
        /// </summary>
        public Buffer<byte> Buffer => Pipe.Buffer;

        /// <summary>
        /// Returns the number of bytes currently written and uncommitted.
        /// </summary>
        public int BytesWritten => AsReadableBuffer().Length;

        Span<byte> IOutput.Buffer => Buffer.Span;
        internal Pipe Pipe { get; set; }

        void IOutput.Enlarge(int desiredBufferLength) => Ensure(NewSize(desiredBufferLength));

        private int NewSize(int desiredBufferLength)
        {
            var currentSize = Buffer.Length;
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
            return Pipe.AsReadableBuffer();
        }

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
            Pipe.AdvanceWriter(bytesWritten);
        }

        /// <summary>
        /// Commits all outstanding written data to the underlying <see cref="IPipeWriter"/> so they can be read
        /// and seals the <see cref="WritableBuffer"/> so no more data can be committed.
        /// </summary>
        /// <remarks>
        /// While an on-going conncurent read may pick up the data, <see cref="FlushAsync"/> should be called to signal the reader.
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
        public WritableBufferAwaitable FlushAsync()
        {
            return Pipe.FlushAsync();
        }
    }
}
