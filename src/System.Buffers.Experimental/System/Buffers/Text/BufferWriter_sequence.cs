// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Utf8;

namespace System.Buffers.Text
{
    public ref struct BufferWriter<TOutput> where TOutput : IOutput
    {
        TOutput _output;
        Span<byte> _buffer;
        int _written;

        public BufferWriter(TOutput output)
        {
            _output = output;
            _buffer = _output.GetSpan();
            _written = 0;
        }

        public void Flush()
        {
            _output.Advance(_written);
            _buffer = _output.GetSpan();
            _written = 0;
        }

        public void WriteBytes(byte[] bytes)
        {
            var free = Free;
            if (bytes.Length > 0 && free.Length >= bytes.Length)
            {
                ref byte pSource = ref bytes[0];
                ref byte pDest = ref free.DangerousGetPinnableReference();

                Unsafe.CopyBlockUnaligned(ref pDest, ref pSource, (uint)bytes.Length);

                Advance(bytes.Length);
            }
            else
            {
                WriteBytesChunked(bytes, 0, bytes.Length);
            }
        }

        public void WriteBytes(byte[] bytes, int index, int length)
        {
            var free = Free;

            // If offset or length is negative the cast to uint will make them larger than int.MaxValue
            // so each test both tests for negative values and greater than values. This pattern wil also
            // elide the second bounds check that would occur at source[offset]; as is pre-checked
            // https://github.com/dotnet/coreclr/pull/9773
            if ((uint)index > (uint)bytes.Length || (uint)length > (uint)(bytes.Length - index))
            {
                // Only need to pass in array length and offset for ThrowHelper to determine which test failed
                ThrowArgumentOutOfRangeException(bytes.Length, index);
            }

            if (length > 0 && free.Length >= length)
            {
                ref byte pSource = ref bytes[index];
                ref byte pDest = ref free.DangerousGetPinnableReference();

                Unsafe.CopyBlockUnaligned(ref pDest, ref pSource, (uint)length);

                Advance(length);
            }
            else
            {
                WriteBytesChunked(bytes, index, length);
            }
        }

        public void WriteBytes(ReadOnlySpan<byte> bytes)
        {
            var free = Free;
            if(bytes.TryCopyTo(free))
            {
                Advance(bytes.Length);
                return;
            }
            WriteBytesChunked(bytes);
        }

        public void WriteBytes(ReadOnlyMemory<byte> bytes)
            => WriteBytes(bytes.Span);

        public void WriteBytes<T>(T value, StandardFormat format = default) where T : IWritable
        {
            var free = Free;
            int written;
            while (!value.TryWrite(free, out written, format))
            {
                free = Enlarge();
            }
            Advance(written);
        }

        public void WriteBytes<T>(T value, TransformationFormat format) where T : IWritable
        {
            var free = Free;
            int written;
            while (true)
            {
                while (!value.TryWrite(free, out written, format.Format))
                {
                    free = Enlarge();
                }
                if (format.TryTransform(free, ref written)) break;
                free = Enlarge();
            }
            Advance(written);
        }

        public void Write(int value, StandardFormat format = default)
        {
            var free = Free;
            int written;
            while (!Utf8Formatter.TryFormat(value, free, out written, format))
            {
                free = Enlarge();
            }
            Advance(written);
        }

        public void Write(string value)
        {
            var utf16Bytes = value.AsReadOnlySpan().AsBytes();
            while (true)
            {
                var free = Free;
                // TODO: shouldn't it be easier if Free never returned an empty span?
                if(free.Length == 0)
                {
                    free = Enlarge();
                } 
                var status = Encodings.Utf16.ToUtf8(utf16Bytes, free, out var consumed, out int written);
                switch (status)
                {
                    case OperationStatus.Done:
                        Advance(written);
                        return;
                    case OperationStatus.DestinationTooSmall:
                        Advance(written);
                        utf16Bytes = utf16Bytes.Slice(consumed);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value));
                }
            }
        }

        private void WriteBytesChunked(ReadOnlySpan<byte> bytes)
        {
            var length = bytes.Length;
            var index = 0;
            while (length > 0)
            {
                var free = Free;
                if (free.Length == 0)
                {
                    free = Enlarge();
                }

                var chunkLength = Math.Min(length, free.Length);

                var chunk = bytes.Slice(index, chunkLength);
                chunk.CopyTo(free);
                Advance(chunkLength);

                length -= chunkLength;
                index += chunkLength;
            }
        }
        private void WriteBytesChunked(byte[] bytes, int index, int length)
        {
            while (length > 0)
            {
                var free = Free;
                if (free.Length == 0)
                {
                    free = Enlarge();
                }

                var chunkLength = Math.Min(length, free.Length);

                ref byte pSource = ref bytes[index];
                ref byte pDest = ref free.DangerousGetPinnableReference();

                Unsafe.CopyBlockUnaligned(ref pDest, ref pSource, (uint)chunkLength);

                Advance(chunkLength);

                length -= chunkLength;
                index += chunkLength;
            }
        }

        private void ThrowArgumentOutOfRangeException(int length, int index)
        {
            if ((uint)index > (uint)length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        private Span<byte> Free => _buffer.Slice(_written);

        private Span<byte> Enlarge(int desiredBufferSize = 0)
        {
            var before = _buffer.Length - _written;
            Flush(); // This sets _written to 0
            Debug.Assert(_written == 0);
            if (_buffer.Length > before) return _buffer;

            _output.Enlarge(desiredBufferSize);
            _buffer = _output.GetSpan();
            Debug.Assert(_written == 0); // ensure still 0
            return _buffer;
        }

        private void Advance(int count)
            => _written += count;
    }
}
