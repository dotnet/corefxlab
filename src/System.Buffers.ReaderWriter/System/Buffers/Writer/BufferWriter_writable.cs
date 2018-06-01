// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using System.Buffers.Text;

namespace System.Buffers.Writer
{
    public ref partial struct BufferWriter
    {
        #region Byte
        public bool TryWriteBytes(byte[] bytes)
            => TryWriteBytes(bytes.AsSpan());

        public void WriteBytes(byte[] bytes)
            => WriteBytes(bytes.AsSpan());

        public bool TryWriteBytes(ReadOnlySpan<byte> bytes)
        {
            if (bytes.TryCopyTo(Free))
            {
                _written += bytes.Length;
                return true;
            }
            return false;
        }

        public void WriteBytes(ReadOnlySpan<byte> bytes)
        {
            while (!TryWriteBytes(bytes)) Resize();
        }

        public bool TryWriteBytes(ReadOnlyMemory<byte> bytes)
            => TryWriteBytes(bytes.Span);

        public void WriteBytes(ReadOnlyMemory<byte> bytes)
            => WriteBytes(bytes.Span);

        public bool TryWriteBytes(ReadOnlyMemory<byte> bytes, TransformationFormat format)
        {
            var span = bytes.Span;
            if (!span.TryCopyTo(Free))
            {
                return false;
            }
            int written = span.Length;

            if (format.TryTransform(Free, ref written))
            {
                _written += written;
                return true;
            }

            return false;
        }

        public void WriteBytes(ReadOnlyMemory<byte> bytes, TransformationFormat format)
        {
            while (!TryWriteBytes(bytes, format)) Resize();
        }

        #endregion

        #region IWritable
        public bool TryWriteBytes<T>(T value, StandardFormat format = default) where T : IWritable
        {
            if (!value.TryWrite(Free, out int written, format))
            {
                return false;
            }
            _written += written;
            return true;
        }

        public void WriteBytes<T>(T value, StandardFormat format = default) where T : IWritable
        {
            while (!TryWriteBytes(value, format)) Resize();
        }

        public bool TryWriteBytes<T>(T value, TransformationFormat format) where T : IWritable
        {
            if (!value.TryWrite(Free, out int written, format.Format))
            {
                return false;
            }

            if (format.TryTransform(Free, ref written))
            {
                _written += written;
                return true;
            }

            return false;
        }

        public void WriteBytes<T>(T value, TransformationFormat format) where T : IWritable
        {
            while (!TryWriteBytes(value, format)) Resize();
        }
        #endregion

        #region IBufferFormattable
        public bool TryWrite<T>(T value, StandardFormat format = default) where T : IBufferFormattable
        {
            if (value.TryFormat(Free, out int written, format, SymbolTable.InvariantUtf8))
            {
                _written += written;
                return true;
            }
            return false;
        }

        public void Write<T>(T value, StandardFormat format = default) where T : IBufferFormattable
        {
            while (!TryWrite(value, format)) Resize();
        }

        public bool TryWrite<T>(T value, TransformationFormat format) where T : IBufferFormattable
        {
            if (!value.TryFormat(Free, out int written, format.Format, SymbolTable.InvariantUtf8))
            {
                return false;
            }

            if (format.TryTransform(Free, ref written))
            {
                _written += written;
                return true;
            }
            return false;
        }

        public void Write<T>(T value, TransformationFormat format) where T : IBufferFormattable
        {
            while (!TryWrite(value, format)) Resize();
        }
        #endregion     
    }
}
