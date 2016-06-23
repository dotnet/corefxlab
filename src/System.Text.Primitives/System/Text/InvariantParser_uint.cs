// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class InvariantParser
    {
        public static bool TryParse(string text, int index, int count, out uint value, out int charactersConsumed)
        {
            Precondition.Require(count > 0);
            Precondition.Require(text.Length >= index + count);

            unsafe
            {
                fixed (char* pText = text)
                {
                    char* pSubstring = pText + index;
                    var span = new Span<byte>((byte*)pSubstring, count << 1);
                    int bytesConsumed;
                    var result = TryParse(span, FormattingData.Encoding.Utf16, out value, out bytesConsumed);
                    charactersConsumed = bytesConsumed >> 1;
                    return result;
                }
            }
        }

        public static bool TryParse(ReadOnlySpan<char> text, out uint value, out int charactersConsumed)
        {
            Precondition.Require(text.Length > 0);

            ReadOnlySpan<byte> span = text.Cast<char, byte>();
            int bytesConsumed;
            var result = TryParse(span, FormattingData.Encoding.Utf16, out value, out bytesConsumed);
            charactersConsumed = bytesConsumed >> 1;
            return result;
        }

        public static bool TryParse(Utf8String utf8Text, out uint value, out int bytesConsumed)
        {
            Precondition.Require(utf8Text.Length > 0);

            value = 0;
            bytesConsumed = 0;

            for (int byteIndex = 0; byteIndex < utf8Text.Length; byteIndex++)
            {
                byte nextByte = (byte)utf8Text[byteIndex];
                if (nextByte < '0' || nextByte > '9')
                {
                    if (bytesConsumed == 0)
                    {
                        value = default(uint);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                uint candidate = value * 10;
                candidate += (uint)nextByte - '0';
                if (candidate >= value)
                {
                    value = candidate;
                }
                bytesConsumed++;
            }

            return true;
        }
        public static bool TryParse(byte[] utf8Text, int index, out uint value, out int bytesConsumed)
        {
            Precondition.Require(utf8Text.Length > 0);

            value = 0;
            bytesConsumed = 0;

            for (int byteIndex = index; byteIndex < utf8Text.Length; byteIndex++)
            {
                byte nextByte = utf8Text[byteIndex];
                if (nextByte < '0' || nextByte > '9')
                {
                    if (bytesConsumed == 0)
                    {
                        value = default(uint);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                uint candidate = value * 10;
                candidate += (uint)nextByte - '0';
                if (candidate >= value)
                {
                    value = candidate;
                }
                bytesConsumed++;
            }

            return true;
        }
        unsafe public static bool TryParse(byte* utf8Text, int index, int length, out uint value, out int bytesConsumed)
        {
            value = 0;
            bytesConsumed = 0;

            for (int byteIndex = index; byteIndex < length + index; byteIndex++)
            {
                byte nextByte = utf8Text[byteIndex];
                if (nextByte < '0' || nextByte > '9')
                {
                    if (bytesConsumed == 0)
                    {
                        value = default(uint);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                uint candidate = value * 10;
                candidate += (uint)nextByte - '0';
                if (candidate >= value)
                {
                    value = candidate;
                }
                bytesConsumed++;
            }
            return true;
        }

        public static bool TryParse(ReadOnlySpan<byte> utf8Text, FormattingData.Encoding encoding, out uint value, out int bytesConsumed)
        {
            Precondition.Require(utf8Text.Length > 0);
            Precondition.Require(encoding == FormattingData.Encoding.Utf8 || utf8Text.Length > 1);

            value = 0;
            bytesConsumed = 0;

            if (utf8Text[0] == '0')
            {
                if (encoding == FormattingData.Encoding.Utf16)
                {
                    bytesConsumed = 2;
                    return utf8Text[1] == 0;
                }
                bytesConsumed = 1;
                return true;
            }

            for (int byteIndex = 0; byteIndex < utf8Text.Length; byteIndex++)
            {
                byte nextByte = utf8Text[byteIndex];
                if (nextByte < '0' || nextByte > '9')
                {
                    if (bytesConsumed == 0)
                    {
                        value = default(uint);
                        return false;
                    }
                    else {
                        return true;
                    }
                }
                uint candidate = value * 10;
                candidate += (uint)nextByte - '0';
                if (candidate > value)
                {
                    value = candidate;
                }
                else {
                    return true;
                }
                bytesConsumed++;
                if (encoding == FormattingData.Encoding.Utf16)
                {
                    byteIndex++;
                    if (byteIndex >= utf8Text.Length || utf8Text[byteIndex] != 0)
                    {
                        return false;
                    }
                    bytesConsumed++;
                }
            }

            return true;
        }

        #region helpers
        public static bool TryParse(string utf8Text, int index, int count, out uint value)
        {
            int charactersConsumed;
            return TryParse(utf8Text, index, count, out value, out charactersConsumed);
        }

        public static bool TryParse(ReadOnlySpan<char> utf8Text, out uint value)
        {
            int charactersConsumed;
            return TryParse(utf8Text, out value, out charactersConsumed);
        }

        public static bool TryParse(Utf8String utf8Text, out uint value)
        {
            int bytesConsumed;
            return TryParse(utf8Text, out value, out bytesConsumed);
        }
        #endregion
    }
}
