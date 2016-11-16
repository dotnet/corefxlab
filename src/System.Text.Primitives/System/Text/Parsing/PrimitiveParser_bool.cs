// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class PrimitiveParser
    {
        #region Helper Methods
        private static bool IsTrue(byte[] utf8Text, int index)
        {
            if (utf8Text.Length - index < 4)
                return false;

            byte firstChar = utf8Text[index];
            if (firstChar != 't' && firstChar != 'T')
                return false;

            byte secondChar = utf8Text[index + 1];
            if (secondChar != 'r' && secondChar != 'R')
                return false;

            byte thirdChar = utf8Text[index + 2];
            if (thirdChar != 'u' && thirdChar != 'U')
                return false;

            byte fourthChar = utf8Text[index + 3];
            if (fourthChar != 'e' && fourthChar != 'E')
                return false;

            return true;
        }

        private static bool IsTrue(string text, int index)
        {
            if (text.Length - index < 4)
                return false;

            char firstChar = text[index];
            if (firstChar != 't' && firstChar != 'T')
                return false;

            char secondChar = text[index + 1];
            if (secondChar != 'r' && secondChar != 'R')
                return false;

            char thirdChar = text[index + 2];
            if (thirdChar != 'u' && thirdChar != 'U')
                return false;

            char fourthChar = text[index + 3];
            if (fourthChar != 'e' && fourthChar != 'E')
                return false;

            return true;
        }

        private static bool IsTrue(Utf8String text)
        {
            if (text.Length < 4)
                return false;

            byte firstChar = text[0];
            if (firstChar != 't' && firstChar != 'T')
                return false;

            byte secondChar = text[1];
            if (secondChar != 'r' && secondChar != 'R')
                return false;

            byte thirdChar = text[2];
            if (thirdChar != 'u' && thirdChar != 'U')
                return false;

            byte fourthChar = text[3];
            if (fourthChar != 'e' && fourthChar != 'E')
                return false;

            return true;
        }

        private unsafe static bool IsTrue(byte* utf8Text, int index, int length)
        {
            if (length < 4)
                return false;

            byte firstChar = utf8Text[index];
            if (firstChar != 't' && firstChar != 'T')
                return false;

            byte secondChar = utf8Text[index + 1];
            if (secondChar != 'r' && secondChar != 'R')
                return false;

            byte thirdChar = utf8Text[index + 2];
            if (thirdChar != 'u' && thirdChar != 'U')
                return false;

            byte fourthChar = utf8Text[index + 3];
            if (fourthChar != 'e' && fourthChar != 'E')
                return false;

            return true;
        }

        private static bool IsFalse(byte[] utf8Text, int index)
        {
            if (utf8Text.Length - index < 5)
                return false;

            byte firstChar = utf8Text[index];
            if (firstChar != 'f' && firstChar != 'F')
                return false;

            byte secondChar = utf8Text[index + 1];
            if (secondChar != 'a' && secondChar != 'A')
                return false;

            byte thirdChar = utf8Text[index + 2];
            if (thirdChar != 'l' && thirdChar != 'L')
                return false;

            byte fourthChar = utf8Text[index + 3];
            if (fourthChar != 's' && fourthChar != 'S')
                return false;

            byte fifthChar = utf8Text[index + 4];
            if (fifthChar != 'e' && fifthChar != 'E')
                return false;

            return true;
        }

        private static bool IsFalse(string text, int index)
        {
            if (text.Length - index < 5)
                return false;

            char firstChar = text[index];
            if (firstChar != 'f' && firstChar != 'F')
                return false;

            char secondChar = text[index + 1];
            if (secondChar != 'a' && secondChar != 'A')
                return false;

            char thirdChar = text[index + 2];
            if (thirdChar != 'l' && thirdChar != 'L')
                return false;

            char fourthChar = text[index + 3];
            if (fourthChar != 's' && fourthChar != 'S')
                return false;

            char fifthChar = text[index + 4];
            if (fifthChar != 'e' && fifthChar != 'E')
                return false;

            return true;
        }

        private static bool IsFalse(Utf8String text)
        {
            if (text.Length < 5)
                return false;

            byte firstChar = text[0];
            if (firstChar != 'f' && firstChar != 'F')
                return false;

            byte secondChar = text[1];
            if (secondChar != 'a' && secondChar != 'A')
                return false;

            byte thirdChar = text[2];
            if (thirdChar != 'l' && thirdChar != 'L')
                return false;

            byte fourthChar = text[3];
            if (fourthChar != 's' && fourthChar != 'S')
                return false;

            byte fifthChar = text[4];
            if (fifthChar != 'e' && fifthChar != 'E')
                return false;

            return true;
        }

        private unsafe static bool IsFalse(byte* utf8Text, int index, int length)
        {
            if (length < 5)
                return false;

            byte firstChar = utf8Text[index];
            if (firstChar != 'f' && firstChar != 'F')
                return false;

            byte secondChar = utf8Text[index + 1];
            if (secondChar != 'a' && secondChar != 'A')
                return false;

            byte thirdChar = utf8Text[index + 2];
            if (thirdChar != 'l' && thirdChar != 'L')
                return false;

            byte fourthChar = utf8Text[index + 3];
            if (fourthChar != 's' && fourthChar != 'S')
                return false;

            byte fifthChar = utf8Text[index + 4];
            if (fifthChar != 'e' && fifthChar != 'E')
                return false;

            return true;
        }
        #endregion

        public static bool TryParseBoolean(ReadOnlySpan<byte> text, EncodingData encoding, TextFormat format, out bool value, out int bytesConsumed)
        {
            bytesConsumed = 0;
            value = default(bool);
            if (text.Length < 1) {
                return false;
            }

            if (encoding.IsInvariantUtf8) {

                byte firstCodeUnit = text[0];

                if (firstCodeUnit == '1') {
                    bytesConsumed = 1;
                    value = true;
                    return true;
                }
                else if (firstCodeUnit == '0') {
                    bytesConsumed = 1;
                    value = false;
                    return true;
                }
                else if (IsTrue(text)) {
                    bytesConsumed = 4;
                    value = true;
                    return true;
                }
                else if (IsFalse(text)) {
                    bytesConsumed = 5;
                    value = false;
                    return true;
                }
                else {
                    return false;
                }
            }

            return false;
        }

        public static bool TryParseBoolean(byte[] text, int index, EncodingData encoding, TextFormat format,
            out bool value, out int bytesConsumed)
        {
            bytesConsumed = 0;
            value = default(bool);

            if (text.Length < 1 || index < 0 || index >= text.Length)
            {
                return false;
            }

            if (encoding.IsInvariantUtf8)
            {

                byte firstByte = text[index];

                if (firstByte == '1')
                {
                    bytesConsumed = 1;
                    value = true;
                    return true;
                }
                else if (firstByte == '0')
                {
                    bytesConsumed = 1;
                    value = false;
                    return true;
                }
                else if (IsTrue(text, index))
                {
                    bytesConsumed = 4;
                    value = true;
                    return true;
                }
                else if (IsFalse(text, index))
                {
                    bytesConsumed = 5;
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

        public static unsafe bool TryParseBoolean(byte* text, int index, int length, EncodingData encoding,
            TextFormat numericFormat, out bool value, out int bytesConsumed)
        {
            bytesConsumed = 0;
            value = default(bool);

            if (length < 1 || index < 0)
            {
                return false;
            }

            if (encoding.IsInvariantUtf8)
            {
                byte firstByte = text[index];

                if (firstByte == '1')
                {
                    bytesConsumed = 1;
                    value = true;
                    return true;
                }
                else if (firstByte == '0')
                {
                    bytesConsumed = 1;
                    value = false;
                    return true;
                }
                else if (IsTrue(text, index, length))
                {
                    bytesConsumed = 4;
                    value = true;
                    return true;
                }
                else if (IsFalse(text, index, length))
                {
                    bytesConsumed = 5;
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

        public static bool TryParseBoolean(string text, int index, TextFormat format,
          out bool value, out int charactersConsumed)
        {
            charactersConsumed = 0;
            value = default(bool);

            if (text.Length < 1 || index < 0 || index >= text.Length) {
                return false;
            }

            char firstCodeUnit = text[index];

            if (firstCodeUnit == '1') {
                charactersConsumed = 1;
                value = true;
                return true;
            }
            else if (firstCodeUnit == '0') {
                charactersConsumed = 1;
                value = false;
                return true;
            }
            else if (IsTrue(text, index)) {
                charactersConsumed = 4;
                value = true;
                return true;
            }
            else if (IsFalse(text, index)) {
                charactersConsumed = 5;
                value = false;
                return true;
            }
            else {
                return false;
            }
        }

        public static bool TryParseBoolean(Utf8String text, TextFormat format,
            out bool value, out int bytesConsumed)
        {
            bytesConsumed = 0;
            value = default(bool);
            if (text.Length < 1) {
                return false;
            }

            byte firstCodeUnit = text[0];

            if (firstCodeUnit == '1') {
                bytesConsumed = 1;
                value = true;
                return true;
            }
            else if (firstCodeUnit == '0') {
                bytesConsumed = 1;
                value = false;
                return true;
            }
            else if (IsTrue(text)) {
                bytesConsumed = 4;
                value = true;
                return true;
            }
            else if (IsFalse(text)) {
                bytesConsumed = 5;
                value = false;
                return true;
            }
            else {
                return false;
            }
        }
    }
}
