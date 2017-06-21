// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text
{
    public static partial class PrimitiveParser
    {
        public static bool TryParseSByte(ReadOnlySpan<byte> text, out sbyte value, out int bytesConsumed, TextFormat format = default(TextFormat), SymbolTable symbolTable = null)
        {
            symbolTable = symbolTable ?? SymbolTable.InvariantUtf8;

            if (!format.IsDefault && format.HasPrecision)
            {
                throw new NotImplementedException("Format with precision not supported.");
            }

            if (symbolTable == SymbolTable.InvariantUtf8)
            {
                if (format.IsHexadecimal)
                {
                    return InvariantUtf8.Hex.TryParseSByte(text, out value, out bytesConsumed);
                }
                else
                {
                    return InvariantUtf8.TryParseSByte(text, out value, out bytesConsumed);
                }
            }
            else if (symbolTable == SymbolTable.InvariantUtf16)
            {
                ReadOnlySpan<char> utf16Text = text.NonPortableCast<byte, char>();
                int charsConsumed;
                bool result;
                if (format.IsHexadecimal)
                {
                    result = InvariantUtf16.Hex.TryParseSByte(utf16Text, out value, out charsConsumed);
                }
                else
                {
                    result = InvariantUtf16.TryParseSByte(utf16Text, out value, out charsConsumed);
                }
                bytesConsumed = charsConsumed * sizeof(char);
                return result;
            }

            if (format.IsHexadecimal)
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
                value = default(sbyte);
                bytesConsumed = 0;
                return false;
            }

            int sign = 1;
            if (nextSymbol == SymbolTable.Symbol.MinusSign)
            {
                sign = -1;
            }

            int signConsumed = 0;
            if (nextSymbol == SymbolTable.Symbol.PlusSign || nextSymbol == SymbolTable.Symbol.MinusSign)
            {
                signConsumed = thisSymbolConsumed;
                if (!symbolTable.TryParse(text.Slice(signConsumed), out nextSymbol, out thisSymbolConsumed))
                {
                    value = default(sbyte);
                    bytesConsumed = 0;
                    return false;
                }
            }

            if (nextSymbol > SymbolTable.Symbol.D9)
            {
                value = default(sbyte);
                bytesConsumed = 0;
                return false;
            }

            int parsedValue = (int)nextSymbol;
            int index = signConsumed + thisSymbolConsumed;

            while (index < text.Length)
            {
                bool success = symbolTable.TryParse(text.Slice(index), out nextSymbol, out thisSymbolConsumed);
                if (!success || nextSymbol > SymbolTable.Symbol.D9)
                {
                    bytesConsumed = index;
                    value = (sbyte)(parsedValue * sign);
                    return true;
                }

                // If parsedValue > (sbyte.MaxValue / 10), any more appended digits will cause overflow.
                // if parsedValue == (sbyte.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                bool positive = sign > 0;
                bool nextDigitTooLarge = nextSymbol > SymbolTable.Symbol.D8 || (positive && nextSymbol > SymbolTable.Symbol.D7);
                if (parsedValue > sbyte.MaxValue / 10 || (parsedValue == sbyte.MaxValue / 10 && nextDigitTooLarge))
                {
                    bytesConsumed = 0;
                    value = default(sbyte);
                    return false;
                }

                index += thisSymbolConsumed;
                parsedValue = parsedValue * 10 + (int)nextSymbol;
            }

            bytesConsumed = text.Length;
            value = (sbyte)(parsedValue * sign);
            return true;
        }


        public static bool TryParseInt16(ReadOnlySpan<byte> text, out short value, out int bytesConsumed, TextFormat format = default(TextFormat), SymbolTable symbolTable = null)
        {
            symbolTable = symbolTable ?? SymbolTable.InvariantUtf8;

            if (!format.IsDefault && format.HasPrecision)
            {
                throw new NotImplementedException("Format with precision not supported.");
            }

            if (symbolTable == SymbolTable.InvariantUtf8)
            {
                if (format.IsHexadecimal)
                {
                    return InvariantUtf8.Hex.TryParseInt16(text, out value, out bytesConsumed);
                }
                else
                {
                    return InvariantUtf8.TryParseInt16(text, out value, out bytesConsumed);
                }
            }
            else if (symbolTable == SymbolTable.InvariantUtf16)
            {
                ReadOnlySpan<char> utf16Text = text.NonPortableCast<byte, char>();
                int charsConsumed;
                bool result;
                if (format.IsHexadecimal)
                {
                    result = InvariantUtf16.Hex.TryParseInt16(utf16Text, out value, out charsConsumed);
                }
                else
                {
                    result = InvariantUtf16.TryParseInt16(utf16Text, out value, out charsConsumed);
                }
                bytesConsumed = charsConsumed * sizeof(char);
                return result;
            }

            if (format.IsHexadecimal)
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
                value = default(short);
                bytesConsumed = 0;
                return false;
            }

            int sign = 1;
            if ((SymbolTable.Symbol)nextSymbol == SymbolTable.Symbol.MinusSign)
            {
                sign = -1;
            }

            int signConsumed = 0;
            if (nextSymbol == SymbolTable.Symbol.PlusSign || nextSymbol == SymbolTable.Symbol.MinusSign)
            {
                signConsumed = thisSymbolConsumed;
                if (!symbolTable.TryParse(text.Slice(signConsumed), out nextSymbol, out thisSymbolConsumed))
                {
                    value = default(short);
                    bytesConsumed = 0;
                    return false;
                }
            }

            if (nextSymbol > SymbolTable.Symbol.D9)
            {
                value = default(short);
                bytesConsumed = 0;
                return false;
            }

            int parsedValue = (int)nextSymbol;
            int index = signConsumed + thisSymbolConsumed;

            while (index < text.Length)
            {
                bool success = symbolTable.TryParse(text.Slice(index), out nextSymbol, out thisSymbolConsumed);
                if (!success || nextSymbol > SymbolTable.Symbol.D9)
                {
                    bytesConsumed = index;
                    value = (short)(parsedValue * sign);
                    return true;
                }

                // If parsedValue > (short.MaxValue / 10), any more appended digits will cause overflow.
                // if parsedValue == (short.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                bool positive = sign > 0;
                bool nextDigitTooLarge = nextSymbol > SymbolTable.Symbol.D8 || (positive && nextSymbol > SymbolTable.Symbol.D7);
                if (parsedValue > short.MaxValue / 10 || (parsedValue == short.MaxValue / 10 && nextDigitTooLarge))
                {
                    bytesConsumed = 0;
                    value = default(short);
                    return false;
                }

                index += thisSymbolConsumed;
                parsedValue = parsedValue * 10 + (int)nextSymbol;
            }

            bytesConsumed = text.Length;
            value = (short)(parsedValue * sign);
            return true;
        }


        public static bool TryParseInt32(ReadOnlySpan<byte> text, out int value, out int bytesConsumed, TextFormat format = default(TextFormat), SymbolTable symbolTable = null)
        {
            symbolTable = symbolTable ?? SymbolTable.InvariantUtf8;

            if (!format.IsDefault && format.HasPrecision)
            {
                throw new NotImplementedException("Format with precision not supported.");
            }

            if (symbolTable == SymbolTable.InvariantUtf8)
            {
                if (format.IsHexadecimal)
                {
                    return InvariantUtf8.Hex.TryParseInt32(text, out value, out bytesConsumed);
                }
                else
                {
                    return InvariantUtf8.TryParseInt32(text, out value, out bytesConsumed);
                }
            }
            else if (symbolTable == SymbolTable.InvariantUtf16)
            {
                ReadOnlySpan<char> utf16Text = text.NonPortableCast<byte, char>();
                int charsConsumed;
                bool result;
                if (format.IsHexadecimal)
                {
                    result = InvariantUtf16.Hex.TryParseInt32(utf16Text, out value, out charsConsumed);
                }
                else
                {
                    result = InvariantUtf16.TryParseInt32(utf16Text, out value, out charsConsumed);
                }
                bytesConsumed = charsConsumed * sizeof(char);
                return result;
            }

            if (format.IsHexadecimal)
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
                value = default(int);
                bytesConsumed = 0;
                return false;
            }

            int sign = 1;
            if (nextSymbol == SymbolTable.Symbol.MinusSign)
            {
                sign = -1;
            }

            int signConsumed = 0;
            if (nextSymbol == SymbolTable.Symbol.PlusSign || nextSymbol == SymbolTable.Symbol.MinusSign)
            {
                signConsumed = thisSymbolConsumed;
                if (!symbolTable.TryParse(text.Slice(signConsumed), out nextSymbol, out thisSymbolConsumed))
                {
                    value = default(int);
                    bytesConsumed = 0;
                    return false;
                }
            }

            if (nextSymbol > SymbolTable.Symbol.D9)
            {
                value = default(int);
                bytesConsumed = 0;
                return false;
            }

            int parsedValue = (int)nextSymbol;
            int index = signConsumed + thisSymbolConsumed;

            while (index < text.Length)
            {
                bool success = symbolTable.TryParse(text.Slice(index), out nextSymbol, out thisSymbolConsumed);
                if (!success || nextSymbol > SymbolTable.Symbol.D9)
                {
                    bytesConsumed = index;
                    value = (int)(parsedValue * sign);
                    return true;
                }

                // If parsedValue > (int.MaxValue / 10), any more appended digits will cause overflow.
                // if parsedValue == (int.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                bool positive = sign > 0;
                bool nextDigitTooLarge = nextSymbol > SymbolTable.Symbol.D8 || (positive && nextSymbol > SymbolTable.Symbol.D7);
                if (parsedValue > int.MaxValue / 10 || (parsedValue == int.MaxValue / 10 && nextDigitTooLarge))
                {
                    bytesConsumed = 0;
                    value = default(int);
                    return false;
                }

                index += thisSymbolConsumed;
                parsedValue = parsedValue * 10 + (int)nextSymbol;
            }

            bytesConsumed = text.Length;
            value = (int)(parsedValue * sign);
            return true;
        }


        public static bool TryParseInt64(ReadOnlySpan<byte> text, out long value, out int bytesConsumed, TextFormat format = default(TextFormat), SymbolTable symbolTable = null)
        {
            symbolTable = symbolTable ?? SymbolTable.InvariantUtf8;

            if (!format.IsDefault && format.HasPrecision)
            {
                throw new NotImplementedException("Format with precision not supported.");
            }

            if (symbolTable == SymbolTable.InvariantUtf8)
            {
                if (format.IsHexadecimal)
                {
                    return InvariantUtf8.Hex.TryParseInt64(text, out value, out bytesConsumed);
                }
                else
                {
                    return InvariantUtf8.TryParseInt64(text, out value, out bytesConsumed);
                }
            }
            else if (symbolTable == SymbolTable.InvariantUtf16)
            {
                ReadOnlySpan<char> utf16Text = text.NonPortableCast<byte, char>();
                int charsConsumed;
                bool result;
                if (format.IsHexadecimal)
                {
                    result = InvariantUtf16.Hex.TryParseInt64(utf16Text, out value, out charsConsumed);
                }
                else
                {
                    result = InvariantUtf16.TryParseInt64(utf16Text, out value, out charsConsumed);
                }
                bytesConsumed = charsConsumed * sizeof(char);
                return result;
            }

            if (format.IsHexadecimal)
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
                value = default(long);
                bytesConsumed = 0;
                return false;
            }

            int sign = 1;
            if (nextSymbol == SymbolTable.Symbol.MinusSign)
            {
                sign = -1;
            }

            int signConsumed = 0;
            if (nextSymbol == SymbolTable.Symbol.PlusSign || nextSymbol == SymbolTable.Symbol.MinusSign)
            {
                signConsumed = thisSymbolConsumed;
                if (!symbolTable.TryParse(text.Slice(signConsumed), out nextSymbol, out thisSymbolConsumed))
                {
                    value = default(long);
                    bytesConsumed = 0;
                    return false;
                }
            }

            if (nextSymbol > SymbolTable.Symbol.D9)
            {
                value = default(long);
                bytesConsumed = 0;
                return false;
            }

            long parsedValue = (long)nextSymbol;
            int index = signConsumed + thisSymbolConsumed;

            while (index < text.Length)
            {
                bool success = symbolTable.TryParse(text.Slice(index), out nextSymbol, out thisSymbolConsumed);
                if (!success || nextSymbol > SymbolTable.Symbol.D9)
                {
                    bytesConsumed = index;
                    value = (long)(parsedValue * sign);
                    return true;
                }

                // If parsedValue > (long.MaxValue / 10), any more appended digits will cause overflow.
                // if parsedValue == (long.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                bool positive = sign > 0;
                bool nextDigitTooLarge = nextSymbol > SymbolTable.Symbol.D8 || (positive && nextSymbol > SymbolTable.Symbol.D7);
                if (parsedValue > long.MaxValue / 10 || (parsedValue == long.MaxValue / 10 && nextDigitTooLarge))
                {
                    bytesConsumed = 0;
                    value = default(long);
                    return false;
                }

                index += thisSymbolConsumed;
                parsedValue = parsedValue * 10 + (long)nextSymbol;
            }

            bytesConsumed = text.Length;
            value = (long)(parsedValue * sign);
            return true;
        }
    }
}
