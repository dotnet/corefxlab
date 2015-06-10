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
        [CLSCompliant(false)]
        public static bool TryFormat(this sbyte value, Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int bytesWritten)
        {
            return IntegerFormatter.TryFormatInt64(value, 1, buffer, format, formattingData, out bytesWritten);
        }

        [CLSCompliant(false)]
        public static bool TryFormat(this ushort value, Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int bytesWritten)
        {
            return IntegerFormatter.TryFormatUInt64(value, 2, buffer, format, formattingData, out bytesWritten);
        }
        public static bool TryFormat(this short value, Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int bytesWritten)
        {
            return IntegerFormatter.TryFormatInt64(value, 2, buffer, format, formattingData, out bytesWritten);
        }

        [CLSCompliant(false)]
        public static bool TryFormat(this uint value, Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int bytesWritten)
        {
            return IntegerFormatter.TryFormatUInt64(value, 4, buffer, format, formattingData, out bytesWritten);
        }
        public static bool TryFormat(this int value, Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int bytesWritten)
        {
            return IntegerFormatter.TryFormatInt64(value, 4, buffer, format, formattingData, out bytesWritten);
        }

        [CLSCompliant(false)]
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

            var encoded = new Utf8Helpers.FourBytes();
            bytesWritten = Utf8Helpers.CharToUtf8(value, ref encoded);
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
            var byteSpan = PinnedByteArraySpan.BorrowDisposableByteSpan(buffer);
            try
            {
                var avaliableBytes = byteSpan.Length;

                if (formattingData.IsUtf16)
                {
                    var neededBytes = value.Length << 1;
                    if (neededBytes > avaliableBytes)
                    {
                        bytesWritten = 0;
                        return false;
                    }

                    unsafe
                    {
                        fixed (char* pCharacters = value)
                        {
                            byte* pBytes = (byte*)pCharacters;
                            byteSpan.Set(pBytes, neededBytes);
                        }
                    }

                    bytesWritten = neededBytes;
                    return true;
                }

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
                        byteSpan[bytesWritten++] = (byte)codepoint;
                    }
                    else
                    {
                        var encoded = new Utf8Helpers.FourBytes();
                        var bytes = Utf8Helpers.CharToUtf8(c, ref encoded);

                        if (bytesWritten + bytes > avaliableBytes)
                        {
                            bytesWritten = 0;
                            return false;
                        }

                        byteSpan[bytesWritten] = encoded.B0;
                        if (bytes > 1)
                        {
                            byteSpan[bytesWritten + 1] = encoded.B1;

                            if (bytes > 2)
                            {
                                byteSpan[bytesWritten + 2] = encoded.B2;

                                if (bytes > 3)
                                {
                                    byteSpan[bytesWritten + 3] = encoded.B3;
                                }
                            }
                        }

                        bytesWritten += bytes;
                    }
                }
                return true;
            }
            finally
            {
                byteSpan.Free();
            }
        }
    }
}
