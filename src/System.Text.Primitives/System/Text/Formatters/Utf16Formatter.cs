// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text;
using System.Runtime.InteropServices;

namespace System.Buffers.Text
{
    public static partial class Utf16Formatter
    {
        #region Common constants

        private const char Colon = ':';
        private const char Comma = ',';
        private const char Minus = '-';
        private const char Period = '.';
        private const char Plus = '+';
        private const char Slash = '/';
        private const char Space = ' ';
        private static readonly byte[] s_True = Encoding.Unicode.GetBytes("True");
        private static readonly byte[] s_False = Encoding.Unicode.GetBytes("False");
        private static readonly byte[] s_true = Encoding.Unicode.GetBytes("true");
        private static readonly byte[] s_false = Encoding.Unicode.GetBytes("false");

        #endregion Common constants

        #region Date / Time APIs

        public static bool TryFormat(DateTimeOffset value, Span<byte> buffer, out int bytesWritten, StandardFormat format = default)
        {
            TimeSpan offset = NullOffset;
            char symbol = format.Symbol;
            if (format.IsDefault)
            {
                symbol = 'G';
                offset = value.Offset;
            }

            switch (symbol)
            {
                case 'R':
                    return TryFormatRfc1123(value.UtcDateTime, buffer, out bytesWritten);

                case 'l':
                    return TryFormatRfc1123Lowercase(value.UtcDateTime, buffer, out bytesWritten);

                case 'O':
                    return TryFormatO(value.DateTime, value.Offset, buffer, out bytesWritten);

                case 'G':
                    return TryFormatG(value.DateTime, offset, buffer, out bytesWritten);

                default:
                    throw new NotSupportedException();
            }
        }

        public static bool TryFormat(DateTime value, Span<byte> buffer, out int bytesWritten, StandardFormat format = default)
        {
            char symbol = format.IsDefault ? 'G' : format.Symbol;

            switch (symbol)
            {
                case 'R':
                    return TryFormatRfc1123(value, buffer, out bytesWritten);

                case 'l':
                    return TryFormatRfc1123Lowercase(value, buffer, out bytesWritten);

                case 'O':
                    return TryFormatO(value, NullOffset, buffer, out bytesWritten);

                case 'G':
                    return TryFormatG(value, NullOffset, buffer, out bytesWritten);

                default:
                    throw new NotSupportedException();
            }
        }

        public static bool TryFormat(TimeSpan value, Span<byte> buffer, out int bytesWritten, StandardFormat format = default)
        {
            char symbol = format.IsDefault ? 'c' : format.Symbol;

            switch (symbol)
            {
                case 'G':
                case 'g':
                case 'c':
                case 't':
                case 'T':
                    return TryFormatTimeSpan(value, symbol, buffer, out bytesWritten);

                default:
                    throw new NotSupportedException();
            }
        }

        #endregion Date / Time APIs

        #region Integer APIs

        public static bool TryFormat(byte value, Span<byte> buffer, out int bytesWritten, StandardFormat format = default)
            => TryFormatCore(value, buffer, out bytesWritten, format);

        public static bool TryFormat(sbyte value, Span<byte> buffer, out int bytesWritten, StandardFormat format = default)
            => TryFormatCore(value, 0xff, buffer, out bytesWritten, format);

        public static bool TryFormat(ushort value, Span<byte> buffer, out int bytesWritten, StandardFormat format = default)
            => TryFormatCore(value, buffer, out bytesWritten, format);

        public static bool TryFormat(short value, Span<byte> buffer, out int bytesWritten, StandardFormat format = default)
            => TryFormatCore(value, 0xffff, buffer, out bytesWritten, format);

        public static bool TryFormat(uint value, Span<byte> buffer, out int bytesWritten, StandardFormat format = default)
            => TryFormatCore(value, buffer, out bytesWritten, format);

        public static bool TryFormat(int value, Span<byte> buffer, out int bytesWritten, StandardFormat format = default)
            => TryFormatCore(value, 0xffffffff, buffer, out bytesWritten, format);

        public static bool TryFormat(ulong value, Span<byte> buffer, out int bytesWritten, StandardFormat format = default)
            => TryFormatCore(value, buffer, out bytesWritten, format);

        public static bool TryFormat(long value, Span<byte> buffer, out int bytesWritten, StandardFormat format = default)
            => TryFormatCore(value, 0xffffffffffffffff, buffer, out bytesWritten, format);

        static bool TryFormatCore(long value, ulong mask, Span<byte> buffer, out int bytesWritten, StandardFormat format)
        {
            if (format.IsDefault)
            {
                format = 'G';
            }

            switch (format.Symbol)
            {
                case 'd':
                case 'D':
                case 'G':
                case 'g':
                    return TryFormatDecimalInt64(value, format.Precision, buffer, out bytesWritten);

                case 'n':
                case 'N':
                    return TryFormatNumericInt64(value, format.Precision, buffer, out bytesWritten);

                case 'x':
                    return TryFormatHexUInt64((ulong)value & mask, format.Precision, true, buffer, out bytesWritten);

                case 'X':
                    return TryFormatHexUInt64((ulong)value & mask, format.Precision, false, buffer, out bytesWritten);

                default:
                    throw new NotSupportedException();
            }
        }

        static bool TryFormatCore(ulong value, Span<byte> buffer, out int bytesWritten, StandardFormat format)
        {
            if (format.IsDefault)
            {
                format = 'G';
            }

            switch (format.Symbol)
            {
                case 'd':
                case 'D':
                case 'G':
                case 'g':
                    return TryFormatDecimalUInt64(value, format.Precision, buffer, out bytesWritten);

                case 'n':
                case 'N':
                    return TryFormatNumericUInt64(value, format.Precision, buffer, out bytesWritten);

                case 'x':
                    return TryFormatHexUInt64(value, format.Precision, true, buffer, out bytesWritten);

                case 'X':
                    return TryFormatHexUInt64(value, format.Precision, false, buffer, out bytesWritten);

                default:
                    throw new NotSupportedException();
            }
        }

        #endregion Integer APIs

        #region Floating-point APIs

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="buffer"></param>
        /// <param name="bytesWritten"></param>
        /// <param name="format">only 'G' format is supported</param>
        /// <returns></returns>
        public static bool TryFormat(double value, Span<byte> buffer, out int bytesWritten, StandardFormat format = default)
            => CustomFormatter.TryFormat(value, buffer, out bytesWritten, format, SymbolTable.InvariantUtf8);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="buffer"></param>
        /// <param name="bytesWritten"></param>
        /// <param name="format">only 'G' format is supported</param>
        /// <returns></returns>
        public static bool TryFormat(float value, Span<byte> buffer, out int bytesWritten, StandardFormat format = default)
            => CustomFormatter.TryFormat(value, buffer, out bytesWritten, format, SymbolTable.InvariantUtf8);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="buffer"></param>
        /// <param name="bytesWritten"></param>
        /// <param name="format">only 'G' format is supported</param>
        /// <returns></returns>
        public static bool TryFormat(decimal value, Span<byte> buffer, out int bytesWritten, StandardFormat format = default)
        {
            if (format.IsDefault) format = 'G';
            else if (format.Symbol != 'G') throw new FormatException();

            var text = value.ToString("G");
            if (MemoryMarshal.AsBytes(text.AsSpan()).TryCopyTo(buffer))
            {
                bytesWritten = text.Length * 2;
                return true;
            }
            bytesWritten = 0;
            return false;
        }

        #endregion Floating-point APIs

        #region Other
        public static bool TryFormat(bool value, Span<byte> buffer, out int bytesWritten, StandardFormat format = default)
        {
            if (value)
            {
                if (buffer.Length < 8) // 4 chars for "true" = 8 bytes
                {
                    bytesWritten = 0;
                    return false;
                }

                if (format.IsDefault || format.Symbol == 'G')
                {
                    s_True.CopyTo(buffer);
                    bytesWritten = s_True.Length;
                    return true;
                }
                else if (format.Symbol == 'l')
                {
                    s_true.CopyTo(buffer);
                    bytesWritten = s_true.Length;
                    return true;
                }
                else
                {
                    throw new FormatException();
                }
            }
            else
            {
                if (buffer.Length < 10) // 5 chars for "false" = 10 bytes
                {
                    bytesWritten = 0;
                    return false;
                }

                if (format.IsDefault || format.Symbol == 'G')
                {
                    s_False.CopyTo(buffer);
                    bytesWritten = s_False.Length;
                    return true;
                }
                else if (format.Symbol == 'l')
                {
                    s_false.CopyTo(buffer);
                    bytesWritten = s_false.Length;
                    return true;
                }
                else
                {
                    throw new FormatException();
                }
            }
        }
        #endregion
    }
}
