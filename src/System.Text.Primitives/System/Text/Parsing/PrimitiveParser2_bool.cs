// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class PrimitiveParser
    {
        #region helpers

        private static bool IsTrue(ReadOnlySpan<char> utf16Chars)
        {
            if (utf16Chars.Length < 4)
                return false;

            char firstChar = utf16Chars[0];
            if (firstChar != 't' && firstChar != 'T')
                return false;

            char secondChar = utf16Chars[1];
            if (secondChar != 'r' && secondChar != 'R')
                return false;

            char thirdChar = utf16Chars[2];
            if (thirdChar != 'u' && thirdChar != 'U')
                return false;

            char fourthChar = utf16Chars[3];
            if (fourthChar != 'e' && fourthChar != 'E')
                return false;

            return true;
        }

        private static bool IsFalse(ReadOnlySpan<char> utf16Chars)
        {
            if (utf16Chars.Length < 5)
                return false;

            char firstChar = utf16Chars[0];
            if (firstChar != 'f' && firstChar != 'F')
                return false;

            char secondChar = utf16Chars[1];
            if (secondChar != 'a' && secondChar != 'A')
                return false;

            char thirdChar = utf16Chars[2];
            if (thirdChar != 'l' && thirdChar != 'L')
                return false;

            char fourthChar = utf16Chars[3];
            if (fourthChar != 's' && fourthChar != 'S')
                return false;

            char fifthChar = utf16Chars[4];
            if (fifthChar != 'e' && fifthChar != 'E')
                return false;

            return true;
        }

        #endregion

        public static bool TryParseBoolean(ReadOnlySpan<byte> text, out bool value, out int consumedBytes, EncodingData encoding = default(EncodingData))
        {
            consumedBytes = 0;
            value = default(bool);
            if (text.Length < 1)
            {
                return false;
            }

            if (encoding.IsInvariantUtf8)
            {
                byte firstCodeUnit = text[0];

                if (firstCodeUnit == '1')
                {
                    consumedBytes = 1;
                    value = true;
                    return true;
                }
                else if (firstCodeUnit == '0')
                {
                    consumedBytes = 1;
                    value = false;
                    return true;
                }
                else if (IsTrue(text))
                {
                    consumedBytes = 4;
                    value = true;
                    return true;
                }
                else if (IsFalse(text))
                {
                    consumedBytes = 5;
                    value = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (encoding.IsInvariantUtf16)
            {
                ReadOnlySpan<char> textChars = text.Cast<byte, char>();
                char firstCodeUnit = textChars[0];

                if (firstCodeUnit == '1')
                {
                    consumedBytes = 2;
                    value = true;
                    return true;
                }
                else if (firstCodeUnit == '0')
                {
                    consumedBytes = 2;
                    value = false;
                    return true;
                }
                else if (IsTrue(textChars))
                {
                    consumedBytes = 8;
                    value = true;
                    return true;
                }
                else if (IsFalse(textChars))
                {
                    consumedBytes = 10;
                    value = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
    }
}