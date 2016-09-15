// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class PrimitiveParser
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
                    var result = TryParse(span, EncodingData.Encoding.Utf16, out value, out bytesConsumed);
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
            var result = TryParse(span, EncodingData.Encoding.Utf16, out value, out bytesConsumed);
            charactersConsumed = bytesConsumed >> 1;
            return result;
        }

        public static bool TryParse(Utf8String text, out uint value, out int bytesConsumed)
        {
            // Precondition replacement
            if (text.Length < 1)
            {
                value = 0;
                bytesConsumed = 0;
                return false;
            }

            value = 0;
            bytesConsumed = 0;

            for (int byteIndex = 0; byteIndex < text.Length; byteIndex++)
            {
                byte nextByteVal = (byte)((byte)text[byteIndex] - '0');
                if (nextByteVal > 9)
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
                else if (value > UInt32.MaxValue / 10) // overflow
                {
                    value = 0;
                    bytesConsumed = 0;
                    return false;
                }
                else if (value > 0 && UInt32.MaxValue - value * 10 < nextByteVal) // overflow
                {
                    value = 0;
                    bytesConsumed = 0;
                    return false;
                }
                uint candidate = value * 10 + nextByteVal;

                value = candidate;
                bytesConsumed++;
            }
            return true;
        }

        public static bool TryParse(ReadOnlySpan<byte> text, EncodingData.Encoding encoding, out uint value, out int bytesConsumed)
        {
            Precondition.Require(text.Length > 0);
            Precondition.Require(encoding == EncodingData.Encoding.Utf8 || text.Length > 1);

            value = 0;
            bytesConsumed = 0;

            if (text[0] == '0')
            {
                if (encoding == EncodingData.Encoding.Utf16)
                {
                    bytesConsumed = 2;
                    return text[1] == 0;
                }
                bytesConsumed = 1;
                return true;
            }

            for (int byteIndex = 0; byteIndex < text.Length; byteIndex++)
            {
                byte nextByte = text[byteIndex];
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
                if (candidate > value)
                {
                    value = candidate;
                }
                else
                {
                    return true;
                }
                bytesConsumed++;
                if (encoding == EncodingData.Encoding.Utf16)
                {
                    byteIndex++;
                    if (byteIndex >= text.Length || text[byteIndex] != 0)
                    {
                        return false;
                    }
                    bytesConsumed++;
                }
            }

            return true;
        }

        #region helpers
        public static bool TryParse(string text, int index, int count, out uint value)
        {
            int charactersConsumed;
            return TryParse(text, index, count, out value, out charactersConsumed);
        }

        public static bool TryParse(ReadOnlySpan<char> text, out uint value)
        {
            int charactersConsumed;
            return TryParse(text, out value, out charactersConsumed);
        }

        public static bool TryParse(Utf8String text, out uint value)
        {
            int bytesConsumed;
            return TryParse(text, out value, out bytesConsumed);
        }
        #endregion
    }
}
