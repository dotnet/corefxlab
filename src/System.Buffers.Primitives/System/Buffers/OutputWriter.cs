// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Buffers
{
    public static class OutputWriter
    {
        public static OutputWriter<T> Create<T>(T output) where T:IOutput
        {
            return new OutputWriter<T>(output);
        }
    }

    public ref struct OutputWriter<T> where T: IOutput
    {
        private T _output;
        private Span<byte> _span;

        public OutputWriter(T output)
        {
            _output = output;
            _span = output.GetSpan();
        }

        public Span<byte> Span => _span;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Advance(int count)
        {
            _span = _span.Slice(count);
            _output.Advance(count);
        }

        public void Write(byte[] source)
        {
            if (source.Length > 0 && _span.Length >= source.Length)
            {
                ref byte pSource = ref source[0];
                ref byte pDest = ref MemoryMarshal.GetReference(_span);

                Unsafe.CopyBlockUnaligned(ref pDest, ref pSource, (uint)source.Length);

                Advance(source.Length);
            }
            else
            {
                WriteMultiBuffer(source, 0, source.Length);
            }
        }

        public void Write(byte[] source, int offset, int length)
        {
            // If offset or length is negative the cast to uint will make them larger than int.MaxValue
            // so each test both tests for negative values and greater than values. This pattern wil also
            // elide the second bounds check that would occur at source[offset]; as is pre-checked
            // https://github.com/dotnet/coreclr/pull/9773
            if ((uint)offset > (uint)source.Length || (uint)length > (uint)(source.Length - offset))
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.offset);
            }

            if (length > 0 && _span.Length >= length)
            {
                ref byte pSource = ref source[offset];
                ref byte pDest = ref MemoryMarshal.GetReference(_span);

                Unsafe.CopyBlockUnaligned(ref pDest, ref pSource, (uint)length);

                Advance(length);
            }
            else
            {
                WriteMultiBuffer(source, offset, length);
            }
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
                ref byte pDest = ref MemoryMarshal.GetReference(_span);

                Unsafe.CopyBlockUnaligned(ref pDest, ref pSource, (uint)writable);

                Advance(writable);

                remaining -= writable;
                offset += writable;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Ensure(int count = 1)
        {
            _output.Enlarge(count);
            _span = _output.GetSpan();
        }
    }
}
