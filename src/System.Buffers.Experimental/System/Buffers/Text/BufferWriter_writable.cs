// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Utf8;

namespace System.Buffers.Text
{
    public ref partial struct BufferWriter
    {
        #region Byte
        public bool TryWriteBytes(byte[] bytes)
            => TryWriteBytes(bytes.AsReadOnlySpan());

        public void WriteBytes(byte[] bytes)
            => WriteBytes(bytes.AsReadOnlySpan());

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

        #endregion

        #region IWritable
        public bool TryWriteBytes<T>(T value, StandardFormat format) where T : IWritable
        {
            int written;
            if (!value.TryWrite(Free, out written, format))
            {
                return false;
            }
            _written += written;
            return true;
        }

        public void WriteBytes<T>(T value, StandardFormat format) where T : IWritable
        {
            while (!TryWriteBytes(value, format)) Resize();
        }

        public bool TryWriteBytes<T>(T value, TransformationFormat format) where T : IWritable
        {
            int written;
            if(!value.TryWrite(Free, out written, format.Format))
            {
                return false;
            }

            if(format.TryTransform(Free, ref written))
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
        public bool TryWrite<T>(T value, StandardFormat format) where T : IBufferFormattable
        {
            if (value.TryFormat(Free, out int written, format, SymbolTable.InvariantUtf8))
            {
                _written += written;
                return true;
            }
            return false;
        }

        public void Write<T>(T value, StandardFormat format) where T : IBufferFormattable
        {
            while (!TryWrite(value, format)) Resize();
        }

        public bool TryWrite<T>(T value, TransformationFormat format) where T : IBufferFormattable
        {
            int written;
            if (!value.TryFormat(Free, out written, format.Format, SymbolTable.InvariantUtf8))
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
