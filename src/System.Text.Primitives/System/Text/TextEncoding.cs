// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Text.Utf8;

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

        // TODO: I think we also need to add the following to support transcoding
        public abstract bool TryEncodeFromUnicode(ReadOnlySpan<UnicodeCodePoint> codePoints, Span<byte> buffer, out int bytesWritten);

        public abstract bool TryEncodeFromUnicode(UnicodeCodePoint codePoint, Span<byte> buffer, out int bytesWritten);

        //public abstract bool TryDecodeToUnicode(Span<byte> encoded, ReadOnlySpan<UnicodeCodePoint> decoded, out int bytesWritten);

        //public abstract bool TryDecodeToUnicode(Span<byte> encoded, UnicodeCodePoint decoded, out int bytesWritten);

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

    class Utf8TextEncoding : TextEncoder
    {
        public override bool TryEncodeFromUtf8(ReadOnlySpan<byte> utf8, Span<byte> buffer, out int bytesWritten)
        {
            if (buffer.Length < utf8.Length)
            {
                bytesWritten = 0;
                return false;
            }

            utf8.CopyTo(buffer);
            bytesWritten = utf8.Length;
            return true;
        }

        public override bool TryEncodeFromUtf16(ReadOnlySpan<char> utf16, Span<byte> buffer, out int bytesWritten)
        {
            var avaliableBytes = buffer.Length;
            bytesWritten = 0;
            for (int i = 0; i < utf16.Length; i++)
            {
                var c = utf16[i];

                var codepoint = (ushort)c;
                if (codepoint <= 0x7f) // this if block just optimizes for ascii
                {
                    if (bytesWritten + 1 > avaliableBytes)
                    {
                        bytesWritten = 0;
                        return false;
                    }
                    buffer[bytesWritten++] = (byte)codepoint;
                }
                else
                {
                    Utf8EncodedCodePoint encoded;
                    if (!char.IsSurrogate(c))
                        encoded = new Utf8EncodedCodePoint(c);
                    else
                    {
                        if (++i >= utf16.Length)
                            throw new ArgumentException("Invalid surrogate pair.", nameof(utf16));
                        char lowSurrogate = utf16[i];
                        encoded = new Utf8EncodedCodePoint(c, lowSurrogate);
                    }


                    if (bytesWritten + encoded.Length > avaliableBytes)
                    {
                        bytesWritten = 0;
                        return false;
                    }

                    buffer[bytesWritten] = encoded.Byte0;
                    if (encoded.Length > 1)
                    {
                        buffer[bytesWritten + 1] = encoded.Byte1;

                        if (encoded.Length > 2)
                        {
                            buffer[bytesWritten + 2] = encoded.Byte2;

                            if (encoded.Length > 3)
                            {
                                buffer[bytesWritten + 3] = encoded.Byte3;
                            }
                        }
                    }

                    bytesWritten += encoded.Length;
                }
            }
            return true;
        }

        public override bool TryEncodeFromUnicode(ReadOnlySpan<UnicodeCodePoint> codePoints, Span<byte> buffer, out int bytesWritten)
        {
            var avaliableBytes = buffer.Length;
            bytesWritten = 0;
            for (int i = 0; i < codePoints.Length; i++)
            {
                var c = codePoints[i];

                var codepoint = (ushort)c;
                if (codepoint <= 0x7f) // this if block just optimizes for ascii
                {
                    if (bytesWritten + 1 > avaliableBytes)
                    {
                        bytesWritten = 0;
                        return false;
                    }
                    buffer[bytesWritten++] = (byte)codepoint;
                }
                else
                {
                    Utf8EncodedCodePoint encoded;
                    encoded = new Utf8EncodedCodePoint(c);

                    if (bytesWritten + encoded.Length > avaliableBytes)
                    {
                        bytesWritten = 0;
                        return false;
                    }

                    buffer[bytesWritten] = encoded.Byte0;
                    if (encoded.Length > 1)
                    {
                        buffer[bytesWritten + 1] = encoded.Byte1;

                        if (encoded.Length > 2)
                        {
                            buffer[bytesWritten + 2] = encoded.Byte2;

                            if (encoded.Length > 3)
                            {
                                buffer[bytesWritten + 3] = encoded.Byte3;
                            }
                        }
                    }

                    bytesWritten += encoded.Length;
                }
            }
            return true;
        }

        public override bool TryEncodeFromUnicode(UnicodeCodePoint codePoint, Span<byte> buffer, out int bytesWritten)
        {
            var avaliableBytes = buffer.Length;
            bytesWritten = 0;

            var codepoint = (ushort)codePoint;
            if (codepoint <= 0x7f) // this if block just optimizes for ascii
            {
                if (bytesWritten + 1 > avaliableBytes)
                {
                    bytesWritten = 0;
                    return false;
                }
                buffer[bytesWritten++] = (byte)codepoint;
            }
            else
            {
                Utf8EncodedCodePoint encoded;
                encoded = new Utf8EncodedCodePoint(codePoint);

                if (bytesWritten + encoded.Length > avaliableBytes)
                {
                    bytesWritten = 0;
                    return false;
                }

                buffer[bytesWritten] = encoded.Byte0;
                if (encoded.Length > 1)
                {
                    buffer[bytesWritten + 1] = encoded.Byte1;

                    if (encoded.Length > 2)
                    {
                        buffer[bytesWritten + 2] = encoded.Byte2;

                        if (encoded.Length > 3)
                        {
                            buffer[bytesWritten + 3] = encoded.Byte3;
                        }
                    }
                }

                bytesWritten += encoded.Length;
            }
            return true;
        }

        public override bool TryEncodeString(string value, Span<byte> buffer, out int bytesWritten)
        {
            var avaliableBytes = buffer.Length;
            bytesWritten = 0;
            for (int i = 0; i < value.Length; i++)
            {
                var c = value[i];

                var codepoint = (ushort)c;
                if (codepoint <= 0x7f) // this if block just optimizes for ascii
                {
                    if (bytesWritten + 1 > avaliableBytes)
                    {
                        bytesWritten = 0;
                        return false;
                    }
                    buffer[bytesWritten++] = (byte)codepoint;
                }
                else
                {
                    Utf8EncodedCodePoint encoded;
                    if (!char.IsSurrogate(c))
                        encoded = new Utf8EncodedCodePoint(c);
                    else
                    {
                        if (++i >= value.Length)
                            throw new ArgumentException("Invalid surrogate pair.", nameof(value));
                        char lowSurrogate = value[i];
                        encoded = new Utf8EncodedCodePoint(c, lowSurrogate);
                    }


                    if (bytesWritten + encoded.Length > avaliableBytes)
                    {
                        bytesWritten = 0;
                        return false;
                    }

                    buffer[bytesWritten] = encoded.Byte0;
                    if (encoded.Length > 1)
                    {
                        buffer[bytesWritten + 1] = encoded.Byte1;

                        if (encoded.Length > 2)
                        {
                            buffer[bytesWritten + 2] = encoded.Byte2;

                            if (encoded.Length > 3)
                            {
                                buffer[bytesWritten + 3] = encoded.Byte3;
                            }
                        }
                    }

                    bytesWritten += encoded.Length;
                }
            }
            return true;
        }

        public override bool TryEncodeChar(char value, Span<byte> buffer, out int bytesWritten)
        {
            if (buffer.Length < 1)
            {
                bytesWritten = 0;
                return false;
            }

            // fast path for ASCII
            if (value <= 127)
            {
                buffer[0] = (byte)value;
                bytesWritten = 1;
                return true;
            }

            // TODO: This can be directly encoded to SpanByte. There is no conversion between spans yet
            var encoded = new Utf8EncodedCodePoint(value);
            bytesWritten = encoded.Length;
            if (buffer.Length < bytesWritten)
            {
                bytesWritten = 0;
                return false;
            }

            buffer[0] = encoded.Byte0;
            if (bytesWritten > 1)
            {
                buffer[1] = encoded.Byte1;
            }
            if (bytesWritten > 2)
            {
                buffer[2] = encoded.Byte2;
            }
            if (bytesWritten > 3)
            {
                buffer[3] = encoded.Byte3;
            }
            return true;
        }
    }

    class Utf16TextEncodingLE : TextEncoder
    {
        public override bool TryEncodeFromUtf8(ReadOnlySpan<byte> utf8, Span<byte> buffer, out int bytesWritten)
        {
            bytesWritten = 0;
            int justWritten;
            foreach (var cp in new Utf8String(utf8).CodePoints)
            {
                if (!Text.Utf16.Utf16LittleEndianEncoder.TryEncodeCodePoint(cp, buffer.Slice(bytesWritten), out justWritten))
                {
                    bytesWritten = 0;
                    return false;
                }
                bytesWritten += justWritten;
            }
            return true;
        }

        public override bool TryEncodeFromUtf16(ReadOnlySpan<char> utf16, Span<byte> buffer, out int bytesWritten)
        {
            var valueBytes = utf16.Cast<char, byte>();
            if (buffer.Length < valueBytes.Length)
            {
                bytesWritten = 0;
                return false;
            }
            valueBytes.CopyTo(buffer);
            bytesWritten = valueBytes.Length;
            return true;
        }

        public override bool TryEncodeFromUnicode(ReadOnlySpan<UnicodeCodePoint> codePoints, Span<byte> buffer, out int bytesWritten)
        {
            var avaliableBytes = buffer.Length;
            bytesWritten = 0;
            int justWritten;

            for (int i = 0; i < codePoints.Length; i++)
            {
                if (!Text.Utf16.Utf16LittleEndianEncoder.TryEncodeCodePoint(codePoints[i], buffer.Slice(bytesWritten), out justWritten))
                {
                    bytesWritten = 0;
                    return false;
                }
                bytesWritten += justWritten;
            }
            return true;
        }

        public override bool TryEncodeFromUnicode(UnicodeCodePoint codePoint, Span<byte> buffer, out int bytesWritten)
        {
            var avaliableBytes = buffer.Length;
            bytesWritten = 0;
            int justWritten;

            if (!Text.Utf16.Utf16LittleEndianEncoder.TryEncodeCodePoint(codePoint, buffer.Slice(bytesWritten), out justWritten))
            {
                bytesWritten = 0;
                return false;
            }
            bytesWritten += justWritten;
            return true;
        }

        public override bool TryEncodeString(string value, Span<byte> buffer, out int bytesWritten)
        {
            var valueBytes = value.Length << 1;
            if (valueBytes > buffer.Length)
            {
                bytesWritten = 0;
                return false;
            }

            unsafe
            {
                fixed (char* pCharacters = value)
                {
                    byte* pBytes = (byte*)pCharacters;
                    new Span<byte>(pBytes, valueBytes).CopyTo(buffer);
                }
            }

            bytesWritten = valueBytes;
            return true;
        }

        public override bool TryEncodeChar(char value, Span<byte> buffer, out int bytesWritten)
        {
            if (buffer.Length < 2)
            {
                bytesWritten = 0;
                return false;
            }
            buffer[0] = (byte)value;
            buffer[1] = (byte)(value >> 8);
            bytesWritten = 2;
            return true;
        }
    }
}