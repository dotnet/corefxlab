// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class PrimitiveParser
    {
        #region helpers

        private static bool IsTrue(ReadOnlySpan<byte> utf8Bytes)
        {
            if (utf8Bytes.Length < 4)
                return false;

            byte firstChar = utf8Bytes[0];
            if (firstChar != 't' && firstChar != 'T')
                return false;

            byte secondChar = utf8Bytes[1];
            if (secondChar != 'r' && secondChar != 'R')
                return false;

            byte thirdChar = utf8Bytes[2];
            if (thirdChar != 'u' && thirdChar != 'U')
                return false;

            byte fourthChar = utf8Bytes[3];
            if (fourthChar != 'e' && fourthChar != 'E')
                return false;

            return true;
        }

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

        private static bool IsFalse(ReadOnlySpan<byte> utf8Bytes)
        {
            if (utf8Bytes.Length < 5)
                return false;

            byte firstChar = utf8Bytes[0];
            if (firstChar != 'f' && firstChar != 'F')
                return false;

            byte secondChar = utf8Bytes[1];
            if (secondChar != 'a' && secondChar != 'A')
                return false;

            byte thirdChar = utf8Bytes[2];
            if (thirdChar != 'l' && thirdChar != 'L')
                return false;

            byte fourthChar = utf8Bytes[3];
            if (fourthChar != 's' && fourthChar != 'S')
                return false;

            byte fifthChar = utf8Bytes[4];
            if (fifthChar != 'e' && fifthChar != 'E')
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

        public static bool TryParseBoolean(ReadOnlySpan<byte> text, out bool value, out int bytesConsumed, EncodingData encoding = default(EncodingData))
        {
            bytesConsumed = 0;
            value = default(bool);

            if (encoding.IsInvariantUtf8)
            {
                return InvariantUtf8.TryParseBoolean(text, out value, out bytesConsumed);
            }

            if (encoding.IsInvariantUtf16)
            {
                ReadOnlySpan<char> textChars = text.Cast<byte, char>();
                int charactersConsumed;
                bool result = InvariantUtf16.TryParseBoolean(textChars, out value, out charactersConsumed);
                bytesConsumed = charactersConsumed * 2;
                return result;
            }

            return false;
        }
    }
}