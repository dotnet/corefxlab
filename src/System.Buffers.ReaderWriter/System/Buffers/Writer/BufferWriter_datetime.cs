// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;

namespace System.Buffers.Writer
{
    public ref partial struct BufferWriter
    {
        public bool TryWrite(DateTime date, StandardFormat format)
        {
            if (!Utf8Formatter.TryFormat(date, Free, out int written, format)) return false;
            _written += written;
            return true;
        }

        public void Write(DateTime date, StandardFormat format)
        {
            while (!TryWrite(date, format)) Resize();
        }

        public bool TryWriteLine(DateTime date, StandardFormat format)
        {
            if (!Utf8Formatter.TryFormat(date, Free, out int written, format)) return false;
            if (!NewLine.TryCopyTo(Free.Slice(written))) return false;
            _written += written + NewLine.Length;
            return true;
        }

        public void WriteLine(DateTime date, StandardFormat format)
        {
            while (!TryWriteLine(date, format)) Resize();
        }
    }
}
