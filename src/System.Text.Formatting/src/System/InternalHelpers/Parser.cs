// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace System.Text.Formatting
{
    internal static class InvariantParser
    {
        public static bool TryParse(string text, int index, int count, out uint value, out int charsConsumed)
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

        public static bool TryParse(string text, int index, int count, out uint value)
        {
            int charsConsumed;
            return TryParse(text, index, count, out value, out charsConsumed);
        }

        public static bool TryParse(ReadOnlySpan<char> text, out uint value, out int charsConsumed)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse(ReadOnlySpan<char> text, out uint value)
        {
            int charsConsumed;
            return TryParse(text, out value, out charsConsumed);
        }     
    }
}
