// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class PrimitiveParser
    {
        public static partial class InvariantUtf16
        {
            public unsafe static bool TryParseBoolean(char* text, int length, out bool value)
            {
                ReadOnlySpan<char> span = new ReadOnlySpan<char>(text, length);
                return TryParseBoolean(span, out value);
            }
            public unsafe static bool TryParseBoolean(char* text, int length, out bool value, out int charactersConsumed)
            {
                ReadOnlySpan<char> span = new ReadOnlySpan<char>(text, length);
                return TryParseBoolean(span, out value, out charactersConsumed);
            }
            public static bool TryParseBoolean(ReadOnlySpan<char> text, out bool value)
            {
                value = default(bool);
                if (text.Length < 1)
                {
                    return false;
                }

                char firstCodeUnit = text[0];

                if (firstCodeUnit == '1')
                {
                    value = true;
                    return true;
                }
                else if (firstCodeUnit == '0')
                {
                    value = false;
                    return true;
                }
                else if (IsTrue(text))
                {
                    value = true;
                    return true;
                }
                else if (IsFalse(text))
                {
                    value = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            public static bool TryParseBoolean(ReadOnlySpan<char> text, out bool value, out int charactersConsumed)
            {
                value = default(bool);
                charactersConsumed = 0;
                if (text.Length < 1)
                {
                    return false;
                }

                char firstCodeUnit = text[0];

                if (firstCodeUnit == '1')
                {
                    charactersConsumed = 1;
                    value = true;
                    return true;
                }
                else if (firstCodeUnit == '0')
                {
                    charactersConsumed = 1;
                    value = false;
                    return true;
                }
                else if (IsTrue(text))
                {
                    charactersConsumed = 4;
                    value = true;
                    return true;
                }
                else if (IsFalse(text))
                {
                    charactersConsumed = 5;
                    value = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}