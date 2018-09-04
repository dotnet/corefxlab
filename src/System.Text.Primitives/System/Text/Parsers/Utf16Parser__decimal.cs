// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Buffers.Text
{
    public static partial class Utf16Parser
    {
        unsafe static bool TryParseDecimal(char* text, int length, out decimal value)
        {
            var span = new ReadOnlySpan<char>(text, length);
            return TryParseDecimal(span, out value, out int consumed);
        }
        unsafe static bool TryParseDecimal(char* text, int length, out decimal value, out int charactersConsumed)
        {
            var span = new ReadOnlySpan<char>(text, length);
            return TryParseDecimal(span, out value, out charactersConsumed);
        }
        public static bool TryParseDecimal(ReadOnlySpan<char> text, out decimal value)
        {
            return TryParseDecimal(text, out value, out int consumed);
        }
        public static bool TryParseDecimal(ReadOnlySpan<char> text, out decimal value, out int charactersConsumed)
        {
            // Precondition replacement
            if (text.Length < 1)
            {
                value = 0;
                charactersConsumed = 0;
                return false;
            }

            value = 0.0M;
            charactersConsumed = 0;
            string decimalString = "";
            bool decimalPlace = false, signed = false;

            int indexOfFirstDigit = 0;
            if (text[0] == '-' || text[0] == '+')
            {
                signed = true;
                decimalString += text[0];
                indexOfFirstDigit = 1;
                charactersConsumed++;
            }

            for (int charIndex = indexOfFirstDigit; charIndex < text.Length; charIndex++)
            {
                char nextChar = text[charIndex];
                char nextCharVal = (char)(nextChar - '0');

                if (nextCharVal > 9)
                {
                    if (!decimalPlace && nextChar == '.')
                    {
                        charactersConsumed++;
                        decimalPlace = true;
                        decimalString += nextChar;
                    }
                    else if ((decimalPlace && signed && charactersConsumed == 2) || ((signed || decimalPlace) && charactersConsumed == 1))
                    {
                        value = 0;
                        charactersConsumed = 0;
                        return false;
                    }
                    else
                    {
                        if (decimal.TryParse(decimalString, out value))
                        {
                            return true;
                        }
                        else
                        {
                            charactersConsumed = 0;
                            return false;
                        }
                    }
                }
                else
                {
                    charactersConsumed++;
                    decimalString += nextChar;
                }
            }

            if ((decimalPlace && signed && charactersConsumed == 2) || ((signed || decimalPlace) && charactersConsumed == 1))
            {
                value = 0;
                charactersConsumed = 0;
                return false;
            }
            else
            {
                if (decimal.TryParse(decimalString, out value))
                {
                    return true;
                }
                else
                {
                    charactersConsumed = 0;
                    return false;
                }
            }
        }
    }
}
