// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Utf8;
using System.Text.Utf16;

namespace System.Text {

    public abstract class TextEncoder
    {
        public Id  Scheme { get; private set; }
        public enum Id  : byte
        {
            Utf16 = 0,
            Utf8 = 1,
            Ascii = 2,
            //Utf16BE = 3,
            //ISO8859_1 = 4,
        }

        public readonly static TextEncoder Utf8 = new Utf8TextEncoding() { Scheme = Id.Utf8 };
        public readonly static TextEncoder Utf16 = new Utf16TextEncodingLE() { Scheme = Id.Utf16 };

        public abstract bool TryEncodeFromUtf8(ReadOnlySpan<byte> utf8, Span<byte> buffer, out int bytesWritten);

        public abstract bool TryEncodeFromUtf16(ReadOnlySpan<char> utf16, Span<byte> buffer, out int bytesWritten);

        public abstract bool TryEncodeFromUnicode(ReadOnlySpan<UnicodeCodePoint> codePoints, Span<byte> buffer, out int bytesWritten);

        public abstract bool TryDecodeToUnicode(Span<byte> encoded, Span<UnicodeCodePoint> decoded, out int bytesWritten);
        

        public virtual bool TryEncodeChar(char value, Span<byte> buffer, out int bytesWritten)
        {
            unsafe
            {
                var stackSpan = new ReadOnlySpan<char>(&value, 1);
                return TryEncodeFromUtf16(stackSpan, buffer, out bytesWritten);
            }
        }

        public virtual bool TryEncodeString(string value, Span<byte> buffer, out int bytesWritten)
        {
            return TryEncodeFromUtf16(value.Slice(), buffer, out bytesWritten);
        }
    }

}