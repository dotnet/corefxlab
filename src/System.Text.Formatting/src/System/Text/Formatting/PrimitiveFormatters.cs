// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Encodings;

namespace System.Text.Formatting
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

            var encoded = new Utf8.FourBytes();
            bytesWritten = Utf8.CharToUtf8(value, ref encoded);
            if(buffer.Length < bytesWritten)
            {
                bytesWritten = 0;
                return false;
            }

            buffer[0] = encoded.B0;
            if(bytesWritten > 1)
            {
                buffer[1] = encoded.B1;
            }
            if(bytesWritten > 2)
            {
                buffer[2] = encoded.B2;
            }
            if(bytesWritten > 3)
            {
                buffer[3] = encoded.B3;
            }
            return true;
        }

        public static bool TryFormat(this string value, Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int bytesWritten)
        {
            var avaliableBytes = buffer.Length;

            if (formattingData.IsUtf16)
            {
                bytesWritten = 0;
                var avaliableChars = avaliableBytes >> 1;
                for (int i = 0; i < value.Length; i++)
                {
                    if(avaliableChars <= i)
                    {
                        bytesWritten = 0;
                        return false;
                    }
                    ushort c = (ushort)value[i];
                    buffer[bytesWritten++] = (byte)c;
                    buffer[bytesWritten++] = (byte)(c >> 8);
                }
                return true;
            }

            bytesWritten = 0;
            for (int i = 0; i < value.Length; i++)
            {
                var c = value[i];
                var encoded = new Utf8.FourBytes();
                var bytes = Utf8.CharToUtf8(c, ref encoded);

                if(bytesWritten + bytes > avaliableBytes)
                {
                    bytesWritten = 0;
                    return false;
                }

                buffer[bytesWritten] = encoded.B0;
                if (bytes > 1)
                {
                    buffer[+bytesWritten + 1] = encoded.B1;
                }
                if (bytes > 2)
                {
                    buffer[+bytesWritten + 2] = encoded.B2;
                }
                if (bytes > 3)
                {
                    buffer[+bytesWritten + 3] = encoded.B3;
                }

                bytesWritten += bytes;
            }
            return true;
        }
    }
}
