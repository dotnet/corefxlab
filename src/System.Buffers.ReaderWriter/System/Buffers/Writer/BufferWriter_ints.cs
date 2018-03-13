// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;

namespace System.Buffers.Writer
{
    public ref partial struct BufferWriter
    {
        public bool TryWrite(int value, StandardFormat format = default)
        {
            if (!Utf8Formatter.TryFormat(value, Free, out int written, format)) return false;
            _written += written;
            return true;
        }

        public void Write(int value, StandardFormat format = default)
        {
            while (!TryWrite(value, format)) Resize();
        }

        public bool TryWriteLine(long value, StandardFormat format = default)
        {
            if (!Utf8Formatter.TryFormat(value, Free, out int written, format)) return false;
            if (!NewLine.TryCopyTo(Free.Slice(written))) return false;
            _written += written + NewLine.Length;
            return true;
        }

        public void WriteLine(long value, StandardFormat format = default)
        {
            while (!TryWriteLine(value, format)) Resize();
        }

        public void Write(long value, StandardFormat format = default)
        {
            while (!TryWrite(value, format)) Resize();
        }

        public bool TryWrite(long value, StandardFormat format = default)
        {
            if (!Utf8Formatter.TryFormat(value, Free, out int written, format)) return false;
            _written += written;
            return true;
        }
    }
}
