// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Buffers.Text
{
    public static partial class Utf16Parser
    {
        unsafe static bool TryParseBoolean(char* text, int length, out bool value)
        {
            if (length >= 4)
            {
                if ((text[0] == 'T' || text[0] == 't') &&
                    (text[1] == 'R' || text[1] == 'r') &&
                    (text[2] == 'U' || text[2] == 'u') &&
                    (text[3] == 'E' || text[3] == 'e'))
                {
                    // No need to set consumed
                    value = true;
                    return true;
                }
                if (length >= 5)
                {
                    if ((text[0] == 'F' || text[0] == 'f') &&
                        (text[1] == 'A' || text[1] == 'a') &&
                        (text[2] == 'L' || text[2] == 'l') &&
                        (text[3] == 'S' || text[3] == 's') &&
                        (text[4] == 'E' || text[4] == 'e'))
                    {
                        // No need to set consumed
                        value = false;
                        return true;
                    }
                }
            }
            // No need to set consumed
            value = default;
            return false;
        }
        unsafe static bool TryParseBoolean(char* text, int length, out bool value, out int charactersConsumed)
        {
            if (length >= 4)
            {
                if ((text[0] == 'T' || text[0] == 't') &&
                    (text[1] == 'R' || text[1] == 'r') &&
                    (text[2] == 'U' || text[2] == 'u') &&
                    (text[3] == 'E' || text[3] == 'e'))
                {
                    charactersConsumed = 4;
                    value = true;
                    return true;
                }
                if (length >= 5)
                {
                    if ((text[0] == 'F' || text[0] == 'f') &&
                        (text[1] == 'A' || text[1] == 'a') &&
                        (text[2] == 'L' || text[2] == 'l') &&
                        (text[3] == 'S' || text[3] == 's') &&
                        (text[4] == 'E' || text[4] == 'e'))
                    {
                        charactersConsumed = 5;
                        value = false;
                        return true;
                    }
                }
            }
            charactersConsumed = 0;
            value = default;
            return false;
        }
        public static bool TryParseBoolean(ReadOnlySpan<char> text, out bool value)
        {
            if (text.Length >= 4)
            {
                if ((text[0] == 'T' || text[0] == 't') &&
                    (text[1] == 'R' || text[1] == 'r') &&
                    (text[2] == 'U' || text[2] == 'u') &&
                    (text[3] == 'E' || text[3] == 'e'))
                {
                    // No need to set consumed
                    value = true;
                    return true;
                }
                if (text.Length >= 5)
                {
                    if ((text[0] == 'F' || text[0] == 'f') &&
                        (text[1] == 'A' || text[1] == 'a') &&
                        (text[2] == 'L' || text[2] == 'l') &&
                        (text[3] == 'S' || text[3] == 's') &&
                        (text[4] == 'E' || text[4] == 'e'))
                    {
                        // No need to set consumed
                        value = false;
                        return true;
                    }
                }
            }
            // No need to set consumed
            value = default;
            return false;
        }
        public static bool TryParseBoolean(ReadOnlySpan<char> text, out bool value, out int charactersConsumed)
        {
            if (text.Length >= 4)
            {
                if ((text[0] == 'T' || text[0] == 't') &&
                    (text[1] == 'R' || text[1] == 'r') &&
                    (text[2] == 'U' || text[2] == 'u') &&
                    (text[3] == 'E' || text[3] == 'e'))
                {
                    charactersConsumed = 4;
                    value = true;
                    return true;
                }
                if (text.Length >= 5)
                {
                    if ((text[0] == 'F' || text[0] == 'f') &&
                        (text[1] == 'A' || text[1] == 'a') &&
                        (text[2] == 'L' || text[2] == 'l') &&
                        (text[3] == 'S' || text[3] == 's') &&
                        (text[4] == 'E' || text[4] == 'e'))
                    {
                        charactersConsumed = 5;
                        value = false;
                        return true;
                    }
                }
            }
            charactersConsumed = 0;
            value = default;
            return false;
        }
    }

}
