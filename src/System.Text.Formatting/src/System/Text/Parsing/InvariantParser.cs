// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Formatting;

namespace System.Text.Parsing
{
    public static class InvariantParser
    {
        [CLSCompliant(false)]
        public static bool TryParse(ByteSpan text, FormattingData.Encoding encoding, out uint value, out int bytesConsumed)
        {
            Precondition.Require(encoding == FormattingData.Encoding.Utf8); // this is temporary

            Precondition.Require(text.Length > 0);
            Precondition.Require(encoding == FormattingData.Encoding.Utf8 || text.Length > 1);

            value = 0;
            bytesConsumed = 0;

            if (text[0] == '0') {
                bytesConsumed = 1;
                return true;
            }

            for (int charIndex = 0; charIndex < text.Length; charIndex++) {
                byte nextChar = text[charIndex];
                if (nextChar < '0' || nextChar > '9') {
                    if (bytesConsumed == 0) {
                        value = default(uint);
                        return false;
                    }
                    else {
                        return true;
                    }
                }
                uint candidate = value * 10;
                candidate += (uint)nextChar - '0';
                if (candidate > value) {
                    value = candidate;
                }
                else {
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

            value = 0;
            charsConsumed = 0;

            if(text[0] == '0')
            {
                charsConsumed = 1;
                return true;
            }

            for (int charIndex = index; charIndex < index + count; charIndex++)
            {
                char nextChar = text[charIndex];
                if(nextChar < '0' || nextChar > '9')
                {
                    if (charsConsumed == 0)
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
                candidate += (uint)nextChar - '0';
                if(candidate > value)
                {
                    value = candidate;
                }
                else
                {
                    return true;
                }

                charsConsumed++;
            }

            return true;
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
