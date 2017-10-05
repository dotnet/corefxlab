// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers.Text;

namespace System.Buffers
{
    public static partial class CustomFormatter
    {
        #region Date / Time APIs

        public static bool TryFormat(DateTimeOffset value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default, SymbolTable symbolTable = null)
        {
            if (symbolTable == null || symbolTable == SymbolTable.InvariantUtf8)
                return Utf8Formatter.TryFormat(value, buffer, out bytesWritten, format);
            else if (symbolTable == SymbolTable.InvariantUtf16)
                return Utf16Formatter.TryFormat(value, buffer, out bytesWritten, format);
            else
                throw new NotSupportedException();
        }

        public static bool TryFormat(DateTime value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default, SymbolTable symbolTable = null)
        {
            if (symbolTable == null || symbolTable == SymbolTable.InvariantUtf8)
                return Utf8Formatter.TryFormat(value, buffer, out bytesWritten, format);
            else if (symbolTable == SymbolTable.InvariantUtf16)
                return Utf16Formatter.TryFormat(value, buffer, out bytesWritten, format);
            else
                throw new NotSupportedException();
        }

        public static bool TryFormat(TimeSpan value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default, SymbolTable symbolTable = null)
        {
            if (symbolTable == null || symbolTable == SymbolTable.InvariantUtf8)
                return Utf8Formatter.TryFormat(value, buffer, out bytesWritten, format);
            else if (symbolTable == SymbolTable.InvariantUtf16)
                return Utf16Formatter.TryFormat(value, buffer, out bytesWritten, format);
            else
                throw new NotSupportedException();
        }

        #endregion Date / Time APIs

        #region GUID APIs

        public static bool TryFormat(Guid value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default, SymbolTable symbolTable = null)
        {
            if (symbolTable == null || symbolTable == SymbolTable.InvariantUtf8)
                return Utf8Formatter.TryFormat(value, buffer, out bytesWritten, format);
            else if (symbolTable == SymbolTable.InvariantUtf16)
                return Utf16Formatter.TryFormat(value, buffer, out bytesWritten, format);
            else
                throw new NotSupportedException();
        }

        #endregion GUID APIs

        #region Integer APIs

        public static bool TryFormat(byte value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default, SymbolTable symbolTable = null)
        {
            if (format.IsDefault)
            {
                format = 'G';
            }

            if (symbolTable == null || symbolTable == SymbolTable.InvariantUtf8)
                return Utf8Formatter.TryFormat(value, buffer, out bytesWritten, format);
            else if (symbolTable == SymbolTable.InvariantUtf16)
                return Utf16Formatter.TryFormat(value, buffer, out bytesWritten, format);
            else
                return TryFormatUInt64(value, buffer, out bytesWritten, format, symbolTable);
        }

        public static bool TryFormat(sbyte value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default, SymbolTable symbolTable = null)
        {
            if (format.IsDefault)
            {
                format = 'G';
            }

            if (symbolTable == null || symbolTable == SymbolTable.InvariantUtf8)
                return Utf8Formatter.TryFormat(value, buffer, out bytesWritten, format);
            else if (symbolTable == SymbolTable.InvariantUtf16)
                return Utf16Formatter.TryFormat(value, buffer, out bytesWritten, format);
            else
                return TryFormatInt64(value, 0xff, buffer, out bytesWritten, format, symbolTable);
        }

        public static bool TryFormat(ushort value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default, SymbolTable symbolTable = null)
        {
            if (format.IsDefault)
            {
                format = 'G';
            }

            if (symbolTable == null || symbolTable == SymbolTable.InvariantUtf8)
                return Utf8Formatter.TryFormat(value, buffer, out bytesWritten, format);
            else if (symbolTable == SymbolTable.InvariantUtf16)
                return Utf16Formatter.TryFormat(value, buffer, out bytesWritten, format);
            else
                return TryFormatUInt64(value, buffer, out bytesWritten, format, symbolTable);
        }

        public static bool TryFormat(short value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default, SymbolTable symbolTable = null)
        {
            if (format.IsDefault)
            {
                format = 'G';
            }

            if (symbolTable == null || symbolTable == SymbolTable.InvariantUtf8)
                return Utf8Formatter.TryFormat(value, buffer, out bytesWritten, format);
            else if (symbolTable == SymbolTable.InvariantUtf16)
                return Utf16Formatter.TryFormat(value, buffer, out bytesWritten, format);
            else
                return TryFormatInt64(value, 0xffff, buffer, out bytesWritten, format, symbolTable);
        }

        public static bool TryFormat(uint value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default, SymbolTable symbolTable = null)
        {
            if (format.IsDefault)
            {
                format = 'G';
            }

            if (symbolTable == null || symbolTable == SymbolTable.InvariantUtf8)
                return Utf8Formatter.TryFormat(value, buffer, out bytesWritten, format);
            else if (symbolTable == SymbolTable.InvariantUtf16)
                return Utf16Formatter.TryFormat(value, buffer, out bytesWritten, format);
            else
                return TryFormatUInt64(value, buffer, out bytesWritten, format, symbolTable);
        }

        public static bool TryFormat(int value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default, SymbolTable symbolTable = null)
        {
            if (format.IsDefault)
            {
                format = 'G';
            }

            if (symbolTable == null || symbolTable == SymbolTable.InvariantUtf8)
                return Utf8Formatter.TryFormat(value, buffer, out bytesWritten, format);
            else if (symbolTable == SymbolTable.InvariantUtf16)
                return Utf16Formatter.TryFormat(value, buffer, out bytesWritten, format);
            else
                return TryFormatInt64(value, 0xffffffff, buffer, out bytesWritten, format, symbolTable);
        }

        public static bool TryFormat(ulong value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default, SymbolTable symbolTable = null)
        {
            if (format.IsDefault)
            {
                format = 'G';
            }

            if (symbolTable == null || symbolTable == SymbolTable.InvariantUtf8)
                return Utf8Formatter.TryFormat(value, buffer, out bytesWritten, format);
            else if (symbolTable == SymbolTable.InvariantUtf16)
                return Utf16Formatter.TryFormat(value, buffer, out bytesWritten, format);
            else
                return TryFormatUInt64(value, buffer, out bytesWritten, format, symbolTable);
        }

        public static bool TryFormat(long value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default, SymbolTable symbolTable = null)
        {
            if (format.IsDefault)
            {
                format = 'G';
            }

            if (symbolTable == null || symbolTable == SymbolTable.InvariantUtf8)
                return Utf8Formatter.TryFormat(value, buffer, out bytesWritten, format);
            else if (symbolTable == SymbolTable.InvariantUtf16)
                return Utf16Formatter.TryFormat(value, buffer, out bytesWritten, format);
            else
                return TryFormatInt64(value, 0xffffffffffffffff, buffer, out bytesWritten, format, symbolTable);
        }

        #endregion Integer APIs

        #region Floating-point APIs

        public static bool TryFormat(double value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default, SymbolTable symbolTable = null)
        {
            if (format.IsDefault)
            {
                format = 'G';
            }

            symbolTable = symbolTable ?? SymbolTable.InvariantUtf8;

            switch (format.Symbol)
            {
                case 'G':
                    return CustomFormatter.TryFormatNumber(value, false, buffer, out bytesWritten, format, symbolTable);

                default:
                    throw new NotSupportedException();
            }
        }

        public static bool TryFormat(float value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default, SymbolTable symbolTable = null)
        {

            if (format.IsDefault)
            {
                format = 'G';
            }

            symbolTable = symbolTable ?? SymbolTable.InvariantUtf8;

            switch (format.Symbol)
            {
                case 'G':
                    return CustomFormatter.TryFormatNumber(value, true, buffer, out bytesWritten, format, symbolTable);

                default:
                    throw new NotSupportedException();
            }
        }

        #endregion Floating-point APIs
    }  
}
