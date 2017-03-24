// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;

namespace System.IO.Pipelines
{
    public struct WritableBufferWriter
    {
        private WritableBuffer _writableBuffer;
        private Span<byte> _span;

        public WritableBufferWriter(WritableBuffer writableBuffer)
        {
            _writableBuffer = writableBuffer;
            _span = writableBuffer.Buffer.Span;
        }

        public Span<byte> Span => _span;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Advance(int count)
        {
            _span = _span.Slice(count);
            _writableBuffer.Advance(count);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Write(byte[] source)
        {
            Write(source, 0, source.Length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Write(byte[] source, int offset, int length)
        {
            if (length <= _span.Length && length > 0)
            {
                ref byte pSource = ref source[offset];
                ref byte pDest = ref _span.DangerousGetPinnableReference();

                Unsafe.CopyBlockUnaligned(ref pDest, ref pSource, (uint)length);

                Advance(length);
                return;
            }

            WriteMultiBuffer(source, offset, length);
        }

        private void WriteMultiBuffer(byte[] source, int offset, int length)
        {
            var remaining = length;

            while (remaining > 0)
            {
                if (_span.Length == 0)
                {
                    Ensure();
                }

                var writable = Math.Min(remaining, _span.Length);

                ref byte pSource = ref source[offset];
                ref byte pDest = ref _span.DangerousGetPinnableReference();

                Unsafe.CopyBlockUnaligned(ref pDest, ref pSource, (uint)writable);

                Advance(writable);

                remaining -= writable;
                offset += writable;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void Ensure()
        {
            _writableBuffer.Ensure();
            _span = _writableBuffer.Buffer.Span;
        }
    }
}