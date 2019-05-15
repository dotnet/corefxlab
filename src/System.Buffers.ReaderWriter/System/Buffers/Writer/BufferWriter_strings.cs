// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.Text.Utf8;
using System.Runtime.InteropServices;

namespace System.Buffers.Writer
{
    public ref partial struct BufferWriter
    {
        #region String
        public bool TryWrite(string text)
        {
            if (!TryWrite(text, out int written)) return false;
            _written += written;
            return true;
        }

        public void Write(string text)
        {
            while (!TryWrite(text)) Resize();
        }

        public bool TryWrite(string text, TransformationFormat format)
        {
            if (!TryWrite(text, out int written)) return false;
            if (!format.TryTransform(Free, ref written)) return false;
            _written += written;
            return true;
        }

        public void Write(string text, TransformationFormat format)
        {
            while (!TryWrite(text, format)) Resize();
        }

        public bool TryWriteLine(string text)
        {
            if (!TryWriteLine(text, out int written)) return false;
            _written += written;
            return true;
        }

        public void WriteLine(string text)
        {
            while (!TryWriteLine(text)) Resize();
        }

        public bool TryWriteLine(string text, TransformationFormat format)
        {
            if (!TryWriteLine(text, out int written)) return false;
            if (!format.TryTransform(Free, ref written)) return false;
            _written += written;
            return true;
        }

        public void WriteLine(string text, TransformationFormat format)
        {
            while (!TryWriteLine(text, format)) Resize();
        }
        #endregion

        #region UTF8String
        public bool TryWrite(Utf8Span text)
        {
            ReadOnlySpan<byte> bytes = text.Bytes;
            if (!bytes.TryCopyTo(Free)) return false;
            _written += bytes.Length;
            return true;
        }

        public void Write(Utf8Span text)
        {
            while (!TryWrite(text)) Resize();
        }

        public bool TryWrite(Utf8Span text, TransformationFormat format)
        {
            int written = text.Bytes.Length;
            if (!text.Bytes.TryCopyTo(Free)) return false;
            if (!format.TryTransform(Free, ref written)) return false;
            _written += written;
            return true;
        }

        public void Write(Utf8Span text, TransformationFormat transformation)
        {
            while (!TryWrite(text, transformation)) Resize();
        }

        public bool TryWriteLine(Utf8Span text)
        {
            if (!TryWriteLine(text, out var written)) return false;
            _written += written;
            return true;
        }

        public void WriteLine(Utf8Span text)
        {
            while (!TryWriteLine(text)) Resize();
        }

        public bool TryWriteLine(Utf8Span text, TransformationFormat format)
        {
            if (!TryWriteLine(text, out var written)) return false;
            if (!format.TryTransform(Free, ref written)) return false;
            _written += written;
            return true;
        }

        public void WriteLine(Utf8Span text, TransformationFormat transformation)
        {
            while (!TryWriteLine(text, transformation)) Resize();
        }

        public bool TryWrite(Utf8String text)
            => TryWrite((Utf8Span)text);

        public void Write(Utf8String text)
            => Write((Utf8Span)text);

        public bool TryWrite(Utf8String text, TransformationFormat transformation)
            => TryWrite((Utf8Span)text, transformation);

        public void Write(Utf8String text, TransformationFormat transformation)
            => Write((Utf8Span)text, transformation);
        #endregion

        public void Write(char character)
        {
            if (character <= 127)
            {
                var free = Free;
                if (free.Length < 1)
                {
                    Resize();
                    free = Free;
                }
                free[0] = (byte)character;
                _written++;
            }
            else
            {
                // TODO: this needs to be optimized.
                Span<byte> utf8Span = stackalloc byte[4];
                Span<char> utf16Span = stackalloc char[1];
                utf16Span[0] = character;
                if(TextEncodings.Utf8.FromUtf16(MemoryMarshal.AsBytes(utf16Span), utf8Span, out int consumed, out int written) == OperationStatus.Done)
                {
                    var encoded = utf8Span.Slice(0, written);
                    while (!encoded.TryCopyTo(Free))
                    {
                        Resize();
                    }
                    _written += written;
                }
            }
        }

        private bool TryWrite(string text, out int written)
        {
            var status = TextEncodings.Utf16.ToUtf8(MemoryMarshal.AsBytes(text.AsSpan()), Free, out _, out written);

            switch (status)
            {
                case OperationStatus.Done:
                    return true;
                case OperationStatus.DestinationTooSmall:
                    written = 0;
                    return false;
                default:
                    throw new ArgumentOutOfRangeException(nameof(text));
            }
        }

        private bool TryWriteLine(string text, out int written)
        {
            if (!TryWrite(text, out written)) return false;
            if (!NewLine.TryCopyTo(Free.Slice(written))) return false;

            written += NewLine.Length;
            return true;
        }

        private bool TryWriteLine(Utf8Span text, out int written)
        {
            var free = Free;
            if (!text.Bytes.TryCopyTo(free))
            {
                written = 0;
                return false;
            }
            written = text.Bytes.Length;
            if (!NewLine.TryCopyTo(free.Slice(written)))
            {
                written = 0;
                return false;
            }
            written += NewLine.Length;
            return true;
        }
    }
}
