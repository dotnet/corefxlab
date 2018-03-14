// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.Runtime.CompilerServices;

namespace System.Buffers.Writer
{
    public ref partial struct BufferWriter<T> where T : IBufferWriter<byte>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(int value, StandardFormat format = default)
        {
            int written;
            while (!Utf8Formatter.TryFormat(value, Buffer, out written, format))
            {
                Enlarge();
            }
            Advance(written);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(ulong value, StandardFormat format = default)
        {
            int written;
            while (!Utf8Formatter.TryFormat(value, Buffer, out written, format))
            {
                Enlarge();
            }
            Advance(written);
        }

        public void Write(string value)
        {
            var utf16Bytes = value.AsSpan().AsBytes();
            int totalConsumed = 0;
            while (true)
            {
                var status = Encodings.Utf16.ToUtf8(utf16Bytes.Slice(totalConsumed), Buffer, out int consumed, out int written);
                switch (status)
                {
                    case OperationStatus.Done:
                        Advance(written);
                        return;
                    case OperationStatus.DestinationTooSmall:
                        Advance(written);
                        Enlarge();
                        break;
                    case OperationStatus.NeedMoreData:
                    case OperationStatus.InvalidData:
                        throw new ArgumentOutOfRangeException(nameof(value));
                }
                totalConsumed += consumed;
            }
        }

        public void Write<TWritable>(TWritable value, TransformationFormat format) where TWritable : IWritable
        {
            int written;
            while (true)
            {
                while (!value.TryWrite(Buffer, out written, format.Format))
                {
                    Enlarge();
                }
                if (format.TryTransform(Buffer, ref written)) break;
                Enlarge();
            }
            Advance(written);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<TWritable>(TWritable value, StandardFormat format) where TWritable : IWritable
        {
            int written;
            while (!value.TryWrite(Buffer, out written, format))
            {
                Enlarge();
            }
            Advance(written);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(ReadOnlySpan<byte> source)
        {
            if (_span.Length >= source.Length)
            {
                source.CopyTo(_span);
                Advance(source.Length);
            }
            else
            {
                WriteMultiBuffer(source);
            }
        }

        private void WriteMultiBuffer(ReadOnlySpan<byte> source)
        {
            while (source.Length > 0)
            {
                if (_span.Length == 0)
                {
                    EnsureMore();
                }

                var writable = Math.Min(source.Length, _span.Length);
                source.Slice(0, writable).CopyTo(_span);
                source = source.Slice(writable);
                Advance(writable);
            }
        }
    }
}
