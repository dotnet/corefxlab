// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text.Formatters
{
    public static partial class Utf16
    {
        #region Common constants

        private const char Colon = ':';
        private const char Comma = ',';
        private const char Minus = '-';
        private const char Period = '.';
        private const char Plus = '+';
        private const char Slash = '/';
        private const char Space = ' ';

        #endregion Common constants

        #region Date / Time APIs

        public static bool TryFormat(DateTimeOffset value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default)
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

        public static bool TryFormat(DateTime value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default)
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

        public static bool TryFormat(TimeSpan value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default)
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

        public static bool TryFormat(byte value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default)
            => TryFormatCore(value, buffer, out bytesWritten, format);

        public static bool TryFormat(sbyte value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default)
            => TryFormatCore(value, 0xff, buffer, out bytesWritten, format);

        public static bool TryFormat(ushort value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default)
            => TryFormatCore(value, buffer, out bytesWritten, format);

        public static bool TryFormat(short value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default)
            => TryFormatCore(value, 0xffff, buffer, out bytesWritten, format);

        public static bool TryFormat(uint value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default)
            => TryFormatCore(value, buffer, out bytesWritten, format);

        public static bool TryFormat(int value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default)
            => TryFormatCore(value, 0xffffffff, buffer, out bytesWritten, format);

        public static bool TryFormat(ulong value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default)
            => TryFormatCore(value, buffer, out bytesWritten, format);

        public static bool TryFormat(long value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default)
            => TryFormatCore(value, 0xffffffffffffffff, buffer, out bytesWritten, format);

        static bool TryFormatCore(long value, ulong mask, Span<byte> buffer, out int bytesWritten, ParsedFormat format)
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

        static bool TryFormatCore(ulong value, Span<byte> buffer, out int bytesWritten, ParsedFormat format)
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

        public static bool TryFormat(this double value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default)
            => Custom.TryFormat(value, buffer, out bytesWritten, format, SymbolTable.InvariantUtf8);

        public static bool TryFormat(this float value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default)
            => Custom.TryFormat(value, buffer, out bytesWritten, format, SymbolTable.InvariantUtf8);

        #endregion Floating-point APIs
    }
}
