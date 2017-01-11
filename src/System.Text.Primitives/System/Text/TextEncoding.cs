// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Runtime.CompilerServices;
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

    class Utf8TextEncoding : TextEncoder
    {
        private const byte b0000_0111U = 0x07; //7
        private const byte b0000_1111U = 0x0F; //15
        private const byte b0001_1111U = 0x1F; //31
        private const byte b0011_1111U = 0x3F; //63
        private const byte b0111_1111U = 0x7F; //127
        private const byte b1000_0000U = 0x80; //128
        private const byte b1100_0000U = 0xC0; //192
        private const byte b1110_0000U = 0xE0; //224
        private const byte b1111_0000U = 0xF0; //240
        private const byte b1111_1000U = 0xF8; //248

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
            int availableBytes = buffer.Length;
            int bytesWrittenForCodePoint = 0;
            bytesWritten = 0;

            for (int i = 0; i < codePoints.Length; i++)
            {
                UnicodeCodePoint codePoint = codePoints[i];
                bytesWrittenForCodePoint = GetNumberOfEncodedBytes(codePoint);
                if (!UnicodeCodePoint.IsSupportedCodePoint(codePoint) || bytesWritten + bytesWrittenForCodePoint > availableBytes)
                {
                    bytesWritten = 0;
                    return false;
                }

                switch (bytesWrittenForCodePoint)
                {
                    case 1:
                        buffer[bytesWritten] = (byte)(b0111_1111U & codePoint.Value);
                        break;
                    case 2:
                        buffer[bytesWritten] = (byte)(((codePoint.Value >> 6) & b0001_1111U) | b1100_0000U);
                        buffer[bytesWritten + 1] = (byte)(((codePoint.Value >> 0) & b0011_1111U) | b1000_0000U);
                        break;
                    case 3:
                        buffer[bytesWritten] = (byte)(((codePoint.Value >> 12) & b0000_1111U) | b1110_0000U);
                        buffer[bytesWritten + 1] = (byte)(((codePoint.Value >> 6) & b0011_1111U) | b1000_0000U);
                        buffer[bytesWritten + 2] = (byte)(((codePoint.Value >> 0) & b0011_1111U) | b1000_0000U);
                        break;
                    case 4:
                        buffer[bytesWritten] = (byte)(((codePoint.Value >> 18) & b0000_0111U) | b1111_0000U);
                        buffer[bytesWritten + 1] = (byte)(((codePoint.Value >> 12) & b0011_1111U) | b1000_0000U);
                        buffer[bytesWritten + 2] = (byte)(((codePoint.Value >> 6) & b0011_1111U) | b1000_0000U);
                        buffer[bytesWritten + 3] = (byte)(((codePoint.Value >> 0) & b0011_1111U) | b1000_0000U);
                        break;
                    default:
                        bytesWritten = 0;
                        return false;
                }

                bytesWritten += bytesWrittenForCodePoint;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetNumberOfEncodedBytes(UnicodeCodePoint codePoint)
        {
            if (codePoint.Value <= 0x7F)
            {
                return 1;
            }

            if (codePoint.Value <= 0x7FF)
            {
                return 2;
            }

            if (codePoint.Value <= 0xFFFF)
            {
                return 3;
            }

            if (codePoint.Value <= 0x1FFFFF)
            {
                return 4;
            }

            return 0;
        }

        public override bool TryDecodeToUnicode(Span<byte> encoded, Span<UnicodeCodePoint> decoded, out int bytesWritten)
        {
            var availableBytes = encoded.Length;
            int bytesWrittenForCodePoint = 0;
            bytesWritten = 0;

            for (int i = 0; i < decoded.Length; i++)
            {
                UnicodeCodePoint decodedCodePoint = decoded[i];

                if (availableBytes <= bytesWritten)
                {
                    decodedCodePoint = new UnicodeCodePoint();
                    bytesWritten = 0;
                    return false;
                }

                byte firstByte = encoded[bytesWritten];

                bytesWrittenForCodePoint = CountConsecutiveStartingOnes(firstByte);

                uint answer = firstByte;

                if (bytesWrittenForCodePoint == 0)
                {
                    bytesWrittenForCodePoint++;
                }
                else if (availableBytes < bytesWritten + bytesWrittenForCodePoint)
                {
                    decodedCodePoint = new UnicodeCodePoint();
                    bytesWritten = 0;
                    return false;
                }
                else if (bytesWrittenForCodePoint == 2)
                {
                    byte byte0 = (byte)(firstByte & b0001_1111U);
                    byte byte1 = (byte)(encoded[1 + bytesWritten] & b0011_1111U);
                    answer = (ushort)(((byte0 >> 2) << 8) | ((byte0 << 6) | byte1));
                }
                else if (bytesWrittenForCodePoint == 3)
                {
                    byte byte0 = (byte)(firstByte & b0000_1111U);
                    byte byte1 = (byte)(encoded[1 + bytesWritten] & b0011_1111U);
                    byte byte2 = (byte)(encoded[2 + bytesWritten] & b0011_1111U);
                    answer = (uint)((((byte0 << 4) | (byte1 >> 2)) << 8) | ((byte1 << 6) | byte2));
                }
                else if (bytesWrittenForCodePoint == 4)
                {
                    byte byte0 = (byte)(firstByte & b0000_0111U);
                    byte byte1 = (byte)(encoded[1 + bytesWritten] & b0011_1111U);
                    byte byte2 = (byte)(encoded[2 + bytesWritten] & b0011_1111U);
                    byte byte3 = (byte)(encoded[3 + bytesWritten] & b0011_1111U);
                    answer = (uint)((((byte0 << 2) | (byte1 >> 4)) << 16) | (((byte1 << 4) | (byte2 >> 2)) << 8) | ((byte2 << 6) | byte3));
                }

                decodedCodePoint = new UnicodeCodePoint(answer);
                decoded[i] = decodedCodePoint;
                bytesWritten += bytesWrittenForCodePoint;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int CountConsecutiveStartingOnes(byte input)
        {
            int count = 0;
            while ((input & b1000_0000U) != 0)
            {
                input = (byte)(input << 1);
                count++;
            }
            return count;
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
            var availableBytes = buffer.Length;
            int bytesWrittenForCodePoint = 0;
            bytesWritten = 0;

            for (int i = 0; i < codePoints.Length; i++)
            {
                if (availableBytes <= bytesWritten || !Text.Utf16.Utf16LittleEndianEncoder.TryEncodeCodePoint(codePoints[i], buffer.Slice(bytesWritten), out bytesWrittenForCodePoint))
                {
                    bytesWritten = 0;
                    return false;
                }
                bytesWritten += bytesWrittenForCodePoint;
            }

            return true;
        }

        public override bool TryDecodeToUnicode(Span<byte> encoded, Span<UnicodeCodePoint> decoded, out int bytesWritten)
        {
            var avaliableBytes = encoded.Length;
            int bytesWrittenForCodePoint = 0;
            bytesWritten = 0;

            for (int i = 0; i < decoded.Length; i++)
            {
                if (i == 0xE000)
                {
                    bytesWrittenForCodePoint = 0;
                }
                UnicodeCodePoint decodedCodePoint = decoded[i];

                if (avaliableBytes - bytesWritten < 2)
                {
                    decodedCodePoint = new UnicodeCodePoint();
                    bytesWritten = 0;
                    return false;
                }

                uint answer = (uint)(encoded[1 + bytesWritten] << 8 | encoded[bytesWritten]);
                decodedCodePoint = new UnicodeCodePoint(answer);
                bytesWrittenForCodePoint = 2;

                if (avaliableBytes - bytesWritten >= 4)
                {
                    uint highBytes = answer;
                    uint lowBytes = (uint)(encoded[3 + bytesWritten] << 8 | encoded[2 + bytesWritten]);

                    if (highBytes >= UnicodeConstants.Utf16HighSurrogateFirstCodePoint 
                            && highBytes <= UnicodeConstants.Utf16HighSurrogateLastCodePoint
                            && lowBytes >= UnicodeConstants.Utf16LowSurrogateFirstCodePoint
                            && lowBytes <= UnicodeConstants.Utf16LowSurrogateLastCodePoint)
                    {
                        answer = (((highBytes - UnicodeConstants.Utf16HighSurrogateFirstCodePoint) << 10)
                            | (lowBytes - UnicodeConstants.Utf16LowSurrogateFirstCodePoint)) + 0x10000;

                        decodedCodePoint = new UnicodeCodePoint(answer);
                        bytesWrittenForCodePoint = 4;
                    }
                }

                decoded[i] = decodedCodePoint;
                bytesWritten += bytesWrittenForCodePoint;
            }

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