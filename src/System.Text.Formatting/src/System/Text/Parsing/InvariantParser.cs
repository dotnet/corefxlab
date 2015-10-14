// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Formatting;
using System.Text.Utf8;

namespace System.Text.Parsing
{
    public static class InvariantParser
    {
        [CLSCompliant(false)]
        public static bool TryParse(ByteSpan text, FormattingData.Encoding encoding, out uint value, out int bytesConsumed)
        {
            Precondition.Require(text.Length > 0);
            Precondition.Require(encoding == FormattingData.Encoding.Utf8 || text.Length > 1);

            value = 0;
            bytesConsumed = 0;

            if (text[0] == '0') {
                bytesConsumed = 1;
                if (encoding == FormattingData.Encoding.Utf16)
                {
                    return text[1] == 0;
                }
                return true;
            }

            for (int byteIndex = 0; byteIndex < text.Length; byteIndex++) {
                byte nextByte = text[byteIndex];
                if (nextByte < '0' || nextByte > '9') {
                    if (bytesConsumed == 0) {
                        value = default(uint);
                        return false;
                    }
                    else {
                        return true;
                    }
                }
                uint candidate = value * 10;
                candidate += (uint)nextByte - '0';
                if (candidate > value) {
                    value = candidate;
                }
                else {
                    return true;
                }
                bytesConsumed++;
                if (encoding == FormattingData.Encoding.Utf16)
                {
                    byteIndex++;
                    if(byteIndex >= text.Length || text[byteIndex] != 0)
                    {
                        return false;
                    }
                    bytesConsumed++;
                }
            }

            return true;
        }

        [CLSCompliant(false)]
        public static bool TryParse(Utf8String text, out uint value, out int bytesConsumed)
        {
            Precondition.Require(text.Length > 0);

            value = 0;
            bytesConsumed = 0;

            for (int byteIndex = 0; byteIndex < text.Length; byteIndex++)
            {
                byte nextByte = (byte)text[byteIndex];
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
                else
                {
                    return true;
                }
                bytesConsumed++;
            }

            return true;
        }

        [CLSCompliant(false)]
        public static bool TryParse(Utf8String text, out ulong value, out int bytesConsumed)
        {
            Precondition.Require(text.Length > 0);

            value = 0;
            bytesConsumed = 0;

            for (int byteIndex = 0; byteIndex < text.Length; byteIndex++)
            {
                byte nextByte = (byte)text[byteIndex];
                if (nextByte < '0' || nextByte > '9')
                {
                    if (bytesConsumed == 0)
                    {
                        value = default(ulong);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                ulong candidate = value * 10;
                candidate += (ulong)nextByte - '0';
                if (candidate >= value)
                {
                    value = candidate;
                }
                else
                {
                    return true;
                }
                bytesConsumed++;
            }

            return true;
        }

        internal static bool TryParse(string text, int index, int count, out uint value, out int charsConsumed)
        {
            Precondition.Require(count > 0);
            Precondition.Require(text.Length >= index + count);
            
            unsafe
            {
                fixed(char* pText = text)
                {
                    char* pSubstring = pText + index;
                    var span = new ByteSpan((byte*)pSubstring, count << 1);
                    int bytesConsumed;
                    var result = TryParse(span, FormattingData.Encoding.Utf16, out value, out bytesConsumed);
                    charsConsumed = bytesConsumed << 1;
                    return result;
                }
            }
        }

        internal static bool TryParse(string text, int index, int count, out uint value)
        {
            int charsConsumed;
            return TryParse(text, index, count, out value, out charsConsumed);
        }

        internal static bool TryParse(ReadOnlySpan<char> text, out uint value, out int charsConsumed)
        {
            throw new NotImplementedException();
        }

        internal static bool TryParse(ReadOnlySpan<char> text, out uint value)
        {
            int charsConsumed;
            return TryParse(text, out value, out charsConsumed);
        }     
    }
}
