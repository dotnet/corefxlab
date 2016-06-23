// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;
using System.Text.Utf8;

namespace System.Text
{
    /// <summary>
    /// Pseudo-implementations of IBufferFormattable interface for primitive types
    /// </summary>
    /// <remarks>
    /// Holds extension methods for formatting types that cannot implement IBufferFormattable for layering reasons.
    /// </remarks>
    public static partial class PrimitiveFormatters
    {
        #region Integers
        public static bool TryFormat(this byte value, Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int bytesWritten)
        {
            return IntegerFormatter.TryFormatUInt64(value, 1, buffer, format, formattingData, out bytesWritten);
        }

        public static bool TryFormat(this sbyte value, Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int bytesWritten)
        {
            return IntegerFormatter.TryFormatInt64(value, 1, buffer, format, formattingData, out bytesWritten);
        }

        public static bool TryFormat(this ushort value, Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int bytesWritten)
        {
            return IntegerFormatter.TryFormatUInt64(value, 2, buffer, format, formattingData, out bytesWritten);
        }
        public static bool TryFormat(this short value, Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int bytesWritten)
        {
            return IntegerFormatter.TryFormatInt64(value, 2, buffer, format, formattingData, out bytesWritten);
        }

        public static bool TryFormat(this uint value, Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int bytesWritten)
        {
            return IntegerFormatter.TryFormatUInt64(value, 4, buffer, format, formattingData, out bytesWritten);
        }
        public static bool TryFormat(this int value, Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int bytesWritten)
        {
            return IntegerFormatter.TryFormatInt64(value, 4, buffer, format, formattingData, out bytesWritten);
        }

        public static bool TryFormat(this ulong value, Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int bytesWritten)
        {
            return IntegerFormatter.TryFormatUInt64(value, 8, buffer, format, formattingData, out bytesWritten);
        }
        public static bool TryFormat(this long value, Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int bytesWritten)
        {
            return IntegerFormatter.TryFormatInt64(value, 8, buffer, format, formattingData, out bytesWritten);
        }
        #endregion

        public static bool TryFormat(this char value, Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int bytesWritten)
        {
            if (formattingData.IsUtf16)
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
            if(bytesWritten > 1)
            {
                buffer[1] = encoded.Byte1;
            }
            if(bytesWritten > 2)
            {
                buffer[2] = encoded.Byte2;
            }
            if(bytesWritten > 3)
            {
                buffer[3] = encoded.Byte3;
            }
            return true;
        }

        public static bool TryFormat(this string value, Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int bytesWritten)
        {

            if (formattingData.IsUtf16)
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
                        buffer.Set(pBytes, valueBytes);
                    }
                }

                bytesWritten = valueBytes;
                return true;
            }

                
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

        public static bool TryFormat(this Utf8String value, Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int bytesWritten)
        {
            if (formattingData.IsUtf16) {
                throw new NotImplementedException();
            }

            if(buffer.Length < value.Length) {
                bytesWritten = 0;
                return false;
            }

            buffer.Set(value.Bytes);
            bytesWritten = value.Length;
            return true;
        }
    }
}
