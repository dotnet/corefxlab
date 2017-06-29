// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text
{
    public static partial class PrimitiveFormatter
    {
        internal static readonly TimeSpan NullOffset = TimeSpan.MinValue;

        public static bool TryFormat(this DateTimeOffset value, Span<byte> buffer, out int bytesWritten, TextFormat format = default, SymbolTable symbolTable = null)
        {
            TimeSpan offset = NullOffset;
            if (format.IsDefault)
            {
                format.Symbol = 'G';
                offset = value.Offset;
            }

            symbolTable = symbolTable ?? SymbolTable.InvariantUtf8;

            switch (format.Symbol)
            {
                case 'R':
                    return TryFormatDateTimeRfc1123(value.UtcDateTime, buffer, out bytesWritten, symbolTable);

                case 'l':
                    return TryFormatDateTimeRfc1123Lowercase(value.UtcDateTime, buffer, out bytesWritten, symbolTable);

                case 'O':
                    return TryFormatDateTimeFormatO(value.DateTime, value.Offset, buffer, out bytesWritten, symbolTable);

                case 'G':
                    return TryFormatDateTimeFormatG(value.DateTime, offset, buffer, out bytesWritten, symbolTable);

                default:
                    throw new NotImplementedException();
            }
        }

        public static bool TryFormat(this DateTime value, Span<byte> buffer, out int bytesWritten, TextFormat format = default, SymbolTable symbolTable = null)
        {
            if (format.IsDefault)
            {
                format.Symbol = 'G';
            }

            symbolTable = symbolTable ?? SymbolTable.InvariantUtf8;

            switch (format.Symbol)
            {
                case 'R':
                    return TryFormatDateTimeRfc1123(value, buffer, out bytesWritten, symbolTable);

                case 'l':
                    return TryFormatDateTimeRfc1123Lowercase(value, buffer, out bytesWritten, symbolTable);

                case 'O':
                    return TryFormatDateTimeFormatO(value, NullOffset, buffer, out bytesWritten, symbolTable);

                case 'G':
                    return TryFormatDateTimeFormatG(value, NullOffset, buffer, out bytesWritten, symbolTable);

                default:
                    throw new NotImplementedException();
            }
        }

        public static bool TryFormat(this TimeSpan value, Span<byte> buffer, out int bytesWritten, TextFormat format = default, SymbolTable symbolTable = null)
        {
            if (format.IsDefault)
            {
                format.Symbol = 'c';
            }

            Precondition.Require(format.Symbol == 'G' || format.Symbol == 'g' || format.Symbol == 'c' || format.Symbol == 't' || format.Symbol == 'T');

            symbolTable = symbolTable ?? SymbolTable.InvariantUtf8;

            return TryFormatTimeSpan(value, format.Symbol, buffer, out bytesWritten, symbolTable);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryFormatDateTimeFormatG(DateTime value, TimeSpan offset, Span<byte> buffer, out int bytesWritten, SymbolTable symbolTable)
        {
            // for now it only works for invariant culture
            if (symbolTable == SymbolTable.InvariantUtf8)
                return InvariantUtf8TimeFormatter.TryFormatG(value, offset, buffer, out bytesWritten);
            else if (symbolTable == SymbolTable.InvariantUtf16)
                return InvariantUtf16TimeFormatter.TryFormatG(value, offset, buffer, out bytesWritten);
            else
                throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryFormatDateTimeFormatO(DateTime value, TimeSpan offset, Span<byte> buffer, out int bytesWritten, SymbolTable symbolTable)
        {
            // for now it only works for invariant culture
            if (symbolTable == SymbolTable.InvariantUtf8)
                return InvariantUtf8TimeFormatter.TryFormatO(value, offset, buffer, out bytesWritten);
            else if (symbolTable == SymbolTable.InvariantUtf16)
                return InvariantUtf16TimeFormatter.TryFormatO(value, offset, buffer, out bytesWritten);
            else
                throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryFormatDateTimeRfc1123(DateTime value, Span<byte> buffer, out int bytesWritten, SymbolTable symbolTable)
        {
            // for now it only works for invariant culture
            if (symbolTable == SymbolTable.InvariantUtf8)
                return InvariantUtf8TimeFormatter.TryFormatRfc1123(value, buffer, out bytesWritten);
            else if (symbolTable == SymbolTable.InvariantUtf16)
                return InvariantUtf16TimeFormatter.TryFormatRfc1123(value, buffer, out bytesWritten);
            else
                throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryFormatDateTimeRfc1123Lowercase(DateTime value, Span<byte> buffer, out int bytesWritten, SymbolTable symbolTable)
        {
            // for now it only works for invariant culture
            if (symbolTable == SymbolTable.InvariantUtf8)
                return InvariantUtf8TimeFormatter.TryFormatRfc1123Lowercase(value, buffer, out bytesWritten);
            else if (symbolTable == SymbolTable.InvariantUtf16)
                return InvariantUtf16TimeFormatter.TryFormatRfc1123Lowercase(value, buffer, out bytesWritten);
            else
                throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryFormatTimeSpan(TimeSpan value, char format, Span<byte> buffer, out int bytesWritten, SymbolTable symbolTable)
        {
            // for now it only works for invariant culture
            if (symbolTable == SymbolTable.InvariantUtf8)
                return InvariantUtf8TimeFormatter.TryFormat(value, format, buffer, out bytesWritten);
            else if (symbolTable == SymbolTable.InvariantUtf16)
                return InvariantUtf16TimeFormatter.TryFormat(value, format, buffer, out bytesWritten);
            else
                throw new NotImplementedException();
        }
    }
}
