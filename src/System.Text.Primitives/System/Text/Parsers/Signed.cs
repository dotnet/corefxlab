// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers.Text;
using System.Runtime.CompilerServices;

namespace System.Buffers
{
    static partial class Parsers
    {
        #region Helpers

        public const sbyte maxValueSbyteDiv10 = sbyte.MaxValue / 10;
        public const short maxValueShortDiv10 = short.MaxValue / 10;
        public const int maxValueIntDiv10 = int.MaxValue / 10;
        public const long maxValueLongDiv10 = long.MaxValue / 10;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDigit(int i)
        {
            return (uint)(i - '0') <= ('9' - '0');
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValid(SymbolTable.Symbol symbol)
        {
            return symbol <= SymbolTable.Symbol.D9;
        }

        // If parsedValue > (sbyte.MaxValue / 10), any more appended digits will cause overflow.
        // if parsedValue == (sbyte.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WillOverFlow(sbyte value, int nextDigit, int sign)
        {
            bool nextDigitTooLarge = nextDigit > 8 || (sign > 0 && nextDigit > 7);
            return (value > maxValueSbyteDiv10 || (value == maxValueSbyteDiv10 && nextDigitTooLarge));
        }

        // If parsedValue > (short.MaxValue / 10), any more appended digits will cause overflow.
        // if parsedValue == (short.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WillOverFlow(short value, int nextDigit, int sign)
        {
            bool nextDigitTooLarge = nextDigit > 8 || (sign > 0 && nextDigit > 7);
            return (value > maxValueShortDiv10 || (value == maxValueShortDiv10 && nextDigitTooLarge));
        }

        // If parsedValue > (int.MaxValue / 10), any more appended digits will cause overflow.
        // if parsedValue == (int.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WillOverFlow(int value, int nextDigit, int sign)
        {
            bool nextDigitTooLarge = nextDigit > 8 || (sign > 0 && nextDigit > 7);
            return (value > maxValueIntDiv10 || (value == maxValueIntDiv10 && nextDigitTooLarge));
        }

        // If parsedValue > (long.MaxValue / 10), any more appended digits will cause overflow.
        // if parsedValue == (long.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WillOverFlow(long value, int nextDigit, int sign)
        {
            bool nextDigitTooLarge = nextDigit > 8 || (sign > 0 && nextDigit > 7);
            return (value > maxValueLongDiv10 || (value == maxValueLongDiv10 && nextDigitTooLarge));
        }

        #endregion
    }
    public static partial class CustomParser
    {
        public static bool TryParseSByte(ReadOnlySpan<byte> text, out sbyte value, out int bytesConsumed, ParsedFormat format = default, SymbolTable symbolTable = null)
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
                    return Utf8Parser.Hex.TryParseSByte(text, out value, out bytesConsumed);
                }
                else
                {
                    return Utf8Parser.TryParseSByte(text, out value, out bytesConsumed);
                }
            }
            else if (symbolTable == SymbolTable.InvariantUtf16)
            {
                ReadOnlySpan<char> utf16Text = text.NonPortableCast<byte, char>();
                int charactersConsumed;
                bool result;
                if (Parsers.IsHexFormat(format))
                {
                    result = Utf16Parser.Hex.TryParseSByte(utf16Text, out value, out charactersConsumed);
                }
                else
                {
                    result = Utf16Parser.TryParseSByte(utf16Text, out value, out charactersConsumed);
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
                    value = default;
                    bytesConsumed = 0;
                    return false;
                }
            }

            if (nextSymbol > SymbolTable.Symbol.D9)
            {
                value = default;
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
                    value = default;
                    return false;
                }

                index += thisSymbolConsumed;
                parsedValue = parsedValue * 10 + (int)nextSymbol;
            }

            bytesConsumed = text.Length;
            value = (sbyte)(parsedValue * sign);
            return true;
        }

        public static bool TryParseInt16(ReadOnlySpan<byte> text, out short value, out int bytesConsumed, ParsedFormat format = default, SymbolTable symbolTable = null)
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
                    return Utf8Parser.Hex.TryParseInt16(text, out value, out bytesConsumed);
                }
                else
                {
                    return Utf8Parser.TryParseInt16(text, out value, out bytesConsumed);
                }
            }
            else if (symbolTable == SymbolTable.InvariantUtf16)
            {
                ReadOnlySpan<char> utf16Text = text.NonPortableCast<byte, char>();
                int charactersConsumed;
                bool result;
                if (Parsers.IsHexFormat(format))
                {
                    result = Utf16Parser.Hex.TryParseInt16(utf16Text, out value, out charactersConsumed);
                }
                else
                {
                    result = Utf16Parser.TryParseInt16(utf16Text, out value, out charactersConsumed);
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
                    value = default;
                    bytesConsumed = 0;
                    return false;
                }
            }

            if (nextSymbol > SymbolTable.Symbol.D9)
            {
                value = default;
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
                    value = default;
                    return false;
                }

                index += thisSymbolConsumed;
                parsedValue = parsedValue * 10 + (int)nextSymbol;
            }

            bytesConsumed = text.Length;
            value = (short)(parsedValue * sign);
            return true;
        }

        public static bool TryParseInt32(ReadOnlySpan<byte> text, out int value, out int bytesConsumed, ParsedFormat format = default, SymbolTable symbolTable = null)
        {
            bool isDefault = format.IsDefault;
            symbolTable = symbolTable ?? SymbolTable.InvariantUtf8;

            if (!isDefault && format.HasPrecision)
            {
                throw new NotImplementedException("Format with precision not supported.");
            }

            bool isHex = Parsers.IsHexFormat(format);

            if (symbolTable == SymbolTable.InvariantUtf8)
            {
                return isHex ? Utf8Parser.Hex.TryParseInt32(text, out value, out bytesConsumed) :
                        Utf8Parser.TryParseInt32(text, out value, out bytesConsumed);
            }
            else if (symbolTable == SymbolTable.InvariantUtf16)
            {
                /*return isHex ? InvariantUtf16.Hex.TryParseInt32(text, out value, out bytesConsumed) :
                    InvariantUtf16.TryParseInt32(text, out value, out bytesConsumed);*/
                ReadOnlySpan<char> utf16Text = text.NonPortableCast<byte, char>();
                bool result = isHex ? Utf16Parser.Hex.TryParseInt32(utf16Text, out value, out int charactersConsumed) :
                    Utf16Parser.TryParseInt32(utf16Text, out value, out charactersConsumed);
                bytesConsumed = charactersConsumed * sizeof(char);
                return result;
            }

            if (isHex)
            {
                throw new NotImplementedException("The only supported encodings for hexadecimal parsing are InvariantUtf8 and InvariantUtf16.");
            }

            if (!(isDefault || format.Symbol == 'G' || format.Symbol == 'g'))
            {
                throw new NotImplementedException(String.Format("Format '{0}' not supported.", format.Symbol));
            }

            int textLength = text.Length;
            if (textLength < 1) goto FalseExit;

            if (!symbolTable.TryParse(text, out SymbolTable.Symbol symbol, out int consumed)) goto FalseExit;

            sbyte sign = 1;
            int index = 0;
            if (symbol == SymbolTable.Symbol.MinusSign)
            {
                sign = -1;
                index += consumed;
                if (index >= textLength) goto FalseExit;
                if (!symbolTable.TryParse(text.Slice(index), out symbol, out consumed)) goto FalseExit;
            }
            else if (symbol == SymbolTable.Symbol.PlusSign)
            {
                index += consumed;
                if (index >= textLength) goto FalseExit;
                if (!symbolTable.TryParse(text.Slice(index), out symbol, out consumed)) goto FalseExit;
            }

            int answer = 0;
            if (Parsers.IsValid(symbol))
            {
                int numBytes = consumed;
                if (symbol == SymbolTable.Symbol.D0)
                {
                    do
                    {
                        index += consumed;
                        if (index >= textLength) goto Done;
                        if (!symbolTable.TryParse(text.Slice(index), out symbol, out consumed)) goto Done;
                    } while (symbol == SymbolTable.Symbol.D0);
                    if (!Parsers.IsValid(symbol)) goto Done;
                }

                int firstNonZeroDigitIndex = index;
                if (textLength - firstNonZeroDigitIndex < Parsers.Int32OverflowLength * numBytes)
                {
                    do
                    {
                        answer = answer * 10 + (int)symbol;
                        index += consumed;
                        if (index >= textLength) goto Done;
                        if (!symbolTable.TryParse(text.Slice(index), out symbol, out consumed)) goto Done;
                    } while (Parsers.IsValid(symbol));
                }
                else
                {
                    do
                    {
                        answer = answer * 10 + (int)symbol;
                        index += consumed;
                        if (index - firstNonZeroDigitIndex == (Parsers.Int32OverflowLength - 1) * numBytes)
                        {
                            if (!symbolTable.TryParse(text.Slice(index), out symbol, out consumed)) goto Done;
                            if (Parsers.IsValid(symbol))
                            {
                                if (Parsers.WillOverFlow(answer, (int)symbol, sign)) goto FalseExit;
                                answer = answer * 10 + (int)symbol;
                                index += consumed;
                            }
                            goto Done;
                        }
                        if (!symbolTable.TryParse(text.Slice(index), out symbol, out consumed)) goto Done;
                    } while (Parsers.IsValid(symbol));
                }
                goto Done;
            }

            FalseExit:
            bytesConsumed = 0;
            value = 0;
            return false;

            Done:
            bytesConsumed = index;
            value = answer * sign;
            return true;
        }

        public static bool TryParseInt64(ReadOnlySpan<byte> text, out long value, out int bytesConsumed, ParsedFormat format = default, SymbolTable symbolTable = null)
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
                    return Utf8Parser.Hex.TryParseInt64(text, out value, out bytesConsumed);
                }
                else
                {
                    return Utf8Parser.TryParseInt64(text, out value, out bytesConsumed);
                }
            }
            else if (symbolTable == SymbolTable.InvariantUtf16)
            {
                ReadOnlySpan<char> utf16Text = text.NonPortableCast<byte, char>();
                int charactersConsumed;
                bool result;
                if (Parsers.IsHexFormat(format))
                {
                    result = Utf16Parser.Hex.TryParseInt64(utf16Text, out value, out charactersConsumed);
                }
                else
                {
                    result = Utf16Parser.TryParseInt64(utf16Text, out value, out charactersConsumed);
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
                    value = default;
                    bytesConsumed = 0;
                    return false;
                }
            }

            if (nextSymbol > SymbolTable.Symbol.D9)
            {
                value = default;
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
                    value = default;
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
