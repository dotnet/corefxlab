// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class PrimitiveParser2
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
                throw new NotImplementedException();
            }

            return false;
        }
    }
}