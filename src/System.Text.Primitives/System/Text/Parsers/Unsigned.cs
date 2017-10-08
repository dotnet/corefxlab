// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers.Text;

namespace System.Buffers.Text
{
    public static partial class CustomParser
    {
        public static bool TryParseByte(ReadOnlySpan<byte> text, out byte value, out int bytesConsumed, ParsedFormat format = default, SymbolTable symbolTable = null)
        {
            symbolTable = symbolTable ?? SymbolTable.InvariantUtf8;

            if (!format.IsDefault && format.HasPrecision)
            {
                throw new NotImplementedException("Format with precision not supported.");
            }

            if (symbolTable == SymbolTable.InvariantUtf8)
            {
                if (Parsers.IsHexFormat(format))
                {
                    return Utf8Parser.Hex.TryParseByte(text, out value, out bytesConsumed);
                }
                else
                {
                    return Utf8Parser.TryParseByte(text, out value, out bytesConsumed);
                }
            }
            else if (symbolTable == SymbolTable.InvariantUtf16)
            {
                ReadOnlySpan<char> utf16Text = text.NonPortableCast<byte, char>();
                int charactersConsumed;
                bool result;
                if (Parsers.IsHexFormat(format))
                {
                    result = Utf16Parser.Hex.TryParseByte(utf16Text, out value, out charactersConsumed);
                }
                else
                {
                    result = Utf16Parser.TryParseByte(utf16Text, out value, out charactersConsumed);
                }
                bytesConsumed = charactersConsumed * sizeof(char);
                return result;
            }

            if (Parsers.IsHexFormat(format))
            {
                throw new NotImplementedException("The only supported encodings for hexadecimal parsing are InvariantUtf8 and InvariantUtf16.");
            }

            if (!(format.IsDefault || format.Symbol == 'G' || format.Symbol == 'g'))
            {
                throw new NotImplementedException(String.Format("Format '{0}' not supported.", format.Symbol));
            }

            SymbolTable.Symbol nextSymbol;
            int thisSymbolConsumed;
            if (!symbolTable.TryParse(text, out nextSymbol, out thisSymbolConsumed))
            {
                value = default;
                bytesConsumed = 0;
                return false;
            }

            if (nextSymbol > SymbolTable.Symbol.D9)
            {
                value = default;
                bytesConsumed = 0;
                return false;
            }

            uint parsedValue = (uint)nextSymbol;
            int index = thisSymbolConsumed;

            while (index < text.Length)
            {
                bool success = symbolTable.TryParse(text.Slice(index), out nextSymbol, out thisSymbolConsumed);
                if (!success || nextSymbol > SymbolTable.Symbol.D9)
                {
                    bytesConsumed = index;
                    value = (byte)parsedValue;
                    return true;
                }

                // If parsedValue > (byte.MaxValue / 10), any more appended digits will cause overflow.
                // if parsedValue == (byte.MaxValue / 10), any nextDigit greater than 5 implies overflow.
                if (parsedValue > byte.MaxValue / 10 || (parsedValue == byte.MaxValue / 10 && nextSymbol > SymbolTable.Symbol.D5))
                {
                    bytesConsumed = 0;
                    value = default;
                    return false;
                }

                index += thisSymbolConsumed;
                parsedValue = parsedValue * 10 + (uint)nextSymbol;
            }

            bytesConsumed = text.Length;
            value = (byte)parsedValue;
            return true;
        }

        public static bool TryParseUInt16(ReadOnlySpan<byte> text, out ushort value, out int bytesConsumed, ParsedFormat format = default, SymbolTable symbolTable = null)
        {
            symbolTable = symbolTable ?? SymbolTable.InvariantUtf8;

            if (!format.IsDefault && format.HasPrecision)
            {
                throw new NotImplementedException("Format with precision not supported.");
            }

            if (symbolTable == SymbolTable.InvariantUtf8)
            {
                if (Parsers.IsHexFormat(format))
                {
                    return Utf8Parser.Hex.TryParseUInt16(text, out value, out bytesConsumed);
                }
                else
                {
                    return Utf8Parser.TryParseUInt16(text, out value, out bytesConsumed);
                }
            }
            else if (symbolTable == SymbolTable.InvariantUtf16)
            {
                ReadOnlySpan<char> utf16Text = text.NonPortableCast<byte, char>();
                int charactersConsumed;
                bool result;
                if (Parsers.IsHexFormat(format))
                {
                    result = Utf16Parser.Hex.TryParseUInt16(utf16Text, out value, out charactersConsumed);
                }
                else
                {
                    result = Utf16Parser.TryParseUInt16(utf16Text, out value, out charactersConsumed);
                }
                bytesConsumed = charactersConsumed * sizeof(char);
                return result;
            }

            if (Parsers.IsHexFormat(format))
            {
                throw new NotImplementedException("The only supported encodings for hexadecimal parsing are InvariantUtf8 and InvariantUtf16.");
            }

            if (!(format.IsDefault || format.Symbol == 'G' || format.Symbol == 'g'))
            {
                throw new NotImplementedException(String.Format("Format '{0}' not supported.", format.Symbol));
            }

            SymbolTable.Symbol nextSymbol;
            int thisSymbolConsumed;
            if (!symbolTable.TryParse(text, out nextSymbol, out thisSymbolConsumed))
            {
                value = default;
                bytesConsumed = 0;
                return false;
            }

            if (nextSymbol > SymbolTable.Symbol.D9)
            {
                value = default;
                bytesConsumed = 0;
                return false;
            }

            uint parsedValue = (uint)nextSymbol;
            int index = thisSymbolConsumed;

            while (index < text.Length)
            {
                bool success = symbolTable.TryParse(text.Slice(index), out nextSymbol, out thisSymbolConsumed);
                if (!success || nextSymbol > SymbolTable.Symbol.D9)
                {
                    bytesConsumed = index;
                    value = (ushort)parsedValue;
                    return true;
                }

                // If parsedValue > (ushort.MaxValue / 10), any more appended digits will cause overflow.
                // if parsedValue == (ushort.MaxValue / 10), any nextDigit greater than 5 implies overflow.
                if (parsedValue > ushort.MaxValue / 10 || (parsedValue == ushort.MaxValue / 10 && nextSymbol > SymbolTable.Symbol.D5))
                {
                    bytesConsumed = 0;
                    value = default;
                    return false;
                }

                index += thisSymbolConsumed;
                parsedValue = parsedValue * 10 + (uint)nextSymbol;
            }

            bytesConsumed = text.Length;
            value = (ushort)parsedValue;
            return true;
        }

        public static bool TryParseUInt32(ReadOnlySpan<byte> text, out uint value, out int bytesConsumed, ParsedFormat format = default, SymbolTable symbolTable = null)
        {
            symbolTable = symbolTable ?? SymbolTable.InvariantUtf8;

            if (!format.IsDefault && format.HasPrecision)
            {
                throw new NotImplementedException("Format with precision not supported.");
            }

            if (symbolTable == SymbolTable.InvariantUtf8)
            {
                if (Parsers.IsHexFormat(format))
                {
                    return Utf8Parser.Hex.TryParseUInt32(text, out value, out bytesConsumed);
                }
                else
                {
                    return Utf8Parser.TryParseUInt32(text, out value, out bytesConsumed);
                }
            }
            else if (symbolTable == SymbolTable.InvariantUtf16)
            {
                ReadOnlySpan<char> utf16Text = text.NonPortableCast<byte, char>();
                int charactersConsumed;
                bool result;
                if (Parsers.IsHexFormat(format))
                {
                    result = Utf16Parser.Hex.TryParseUInt32(utf16Text, out value, out charactersConsumed);
                }
                else
                {
                    result = Utf16Parser.TryParseUInt32(utf16Text, out value, out charactersConsumed);
                }
                bytesConsumed = charactersConsumed * sizeof(char);
                return result;
            }

            if (Parsers.IsHexFormat(format))
            {
                throw new NotImplementedException("The only supported encodings for hexadecimal parsing are InvariantUtf8 and InvariantUtf16.");
            }

            if (!(format.IsDefault || format.Symbol == 'G' || format.Symbol == 'g'))
            {
                throw new NotImplementedException(String.Format("Format '{0}' not supported.", format.Symbol));
            }

            SymbolTable.Symbol nextSymbol;
            int thisSymbolConsumed;
            if (!symbolTable.TryParse(text, out nextSymbol, out thisSymbolConsumed))
            {
                value = default;
                bytesConsumed = 0;
                return false;
            }

            if (nextSymbol > SymbolTable.Symbol.D9)
            {
                value = default;
                bytesConsumed = 0;
                return false;
            }

            uint parsedValue = (uint)nextSymbol;
            int index = thisSymbolConsumed;

            while (index < text.Length)
            {
                bool success = symbolTable.TryParse(text.Slice(index), out nextSymbol, out thisSymbolConsumed);
                if (!success || nextSymbol > SymbolTable.Symbol.D9)
                {
                    bytesConsumed = index;
                    value = (uint)parsedValue;
                    return true;
                }

                // If parsedValue > (uint.MaxValue / 10), any more appended digits will cause overflow.
                // if parsedValue == (uint.MaxValue / 10), any nextDigit greater than 5 implies overflow.
                if (parsedValue > uint.MaxValue / 10 || (parsedValue == uint.MaxValue / 10 && nextSymbol > SymbolTable.Symbol.D5))
                {
                    bytesConsumed = 0;
                    value = default;
                    return false;
                }

                index += thisSymbolConsumed;
                parsedValue = parsedValue * 10 + (uint)nextSymbol;
            }

            bytesConsumed = text.Length;
            value = (uint)parsedValue;
            return true;
        }

        public static bool TryParseUInt64(ReadOnlySpan<byte> text, out ulong value, out int bytesConsumed, ParsedFormat format = default, SymbolTable symbolTable = null)
        {
            symbolTable = symbolTable ?? SymbolTable.InvariantUtf8;

            if (!format.IsDefault && format.HasPrecision)
            {
                throw new NotImplementedException("Format with precision not supported.");
            }

            if (symbolTable == SymbolTable.InvariantUtf8)
            {
                if (Parsers.IsHexFormat(format))
                {
                    return Utf8Parser.Hex.TryParseUInt64(text, out value, out bytesConsumed);
                }
                else
                {
                    return Utf8Parser.TryParseUInt64(text, out value, out bytesConsumed);
                }
            }
            else if (symbolTable == SymbolTable.InvariantUtf16)
            {
                ReadOnlySpan<char> utf16Text = text.NonPortableCast<byte, char>();
                int charactersConsumed;
                bool result;
                if (Parsers.IsHexFormat(format))
                {
                    result = Utf16Parser.Hex.TryParseUInt64(utf16Text, out value, out charactersConsumed);
                }
                else
                {
                    result = Utf16Parser.TryParseUInt64(utf16Text, out value, out charactersConsumed);
                }
                bytesConsumed = charactersConsumed * sizeof(char);
                return result;
            }

            if (Parsers.IsHexFormat(format))
            {
                throw new NotImplementedException("The only supported encodings for hexadecimal parsing are InvariantUtf8 and InvariantUtf16.");
            }

            if (!(format.IsDefault || format.Symbol == 'G' || format.Symbol == 'g'))
            {
                throw new NotImplementedException(String.Format("Format '{0}' not supported.", format.Symbol));
            }

            SymbolTable.Symbol nextSymbol;
            int thisSymbolConsumed;
            if (!symbolTable.TryParse(text, out nextSymbol, out thisSymbolConsumed))
            {
                value = default;
                bytesConsumed = 0;
                return false;
            }

            if (nextSymbol > SymbolTable.Symbol.D9)
            {
                value = default;
                bytesConsumed = 0;
                return false;
            }

            ulong parsedValue = (uint)nextSymbol;
            int index = thisSymbolConsumed;

            while (index < text.Length)
            {
                bool success = symbolTable.TryParse(text.Slice(index), out nextSymbol, out thisSymbolConsumed);
                if (!success || nextSymbol > SymbolTable.Symbol.D9)
                {
                    bytesConsumed = index;
                    value = (ulong)parsedValue;
                    return true;
                }

                // If parsedValue > (ulong.MaxValue / 10), any more appended digits will cause overflow.
                // if parsedValue == (ulong.MaxValue / 10), any nextDigit greater than 5 implies overflow.
                if (parsedValue > ulong.MaxValue / 10 || (parsedValue == ulong.MaxValue / 10 && nextSymbol > SymbolTable.Symbol.D5))
                {
                    bytesConsumed = 0;
                    value = default;
                    return false;
                }

                index += thisSymbolConsumed;
                parsedValue = parsedValue * 10 + (uint)nextSymbol;
            }

            bytesConsumed = text.Length;
            value = (ulong)parsedValue;
            return true;
        }
    }
}
