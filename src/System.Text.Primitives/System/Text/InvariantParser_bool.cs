// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class InvariantParser
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

        public static bool TryParse(byte[] utf8Text, int index, FormattingData cultureAndEncodingInfo, Format.Parsed numericFormat,
            out bool value, out int bytesConsumed)
        {
            bytesConsumed = 0;
            value = default(bool);

            if (utf8Text.Length < 1 || index < 0 || index >= utf8Text.Length)
            {
                return false;
            }

            if (cultureAndEncodingInfo.IsInvariantUtf8)
            {

                byte firstByte = utf8Text[index];

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
                else if (IsTrue(utf8Text, index))
                {
                    bytesConsumed = 4;
                    value = true;
                    return true;
                }
                else if (IsFalse(utf8Text, index))
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

        public static unsafe bool TryParse(byte* utf8Text, int index, int length, FormattingData cultureAndEncodingInfo,
            Format.Parsed numericFormat, out bool value, out int bytesConsumed)
        {
            bytesConsumed = 0;
            value = default(bool);

            if (length < 1 || index < 0)
            {
                return false;
            }

            if (cultureAndEncodingInfo.IsInvariantUtf8)
            {
                byte firstByte = utf8Text[index];

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
                else if (IsTrue(utf8Text, index, length))
                {
                    bytesConsumed = 4;
                    value = true;
                    return true;
                }
                else if (IsFalse(utf8Text, index, length))
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
    }
}
