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
                value = default(bool);
                if (IsTrue(text, length))
                {
                    value = true;
                    return true;
                }
                else if (IsFalse(text, length))
                {
                    value = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            public unsafe static bool TryParseBoolean(char* text, int length, out bool value, out int charactersConsumed)
            {
                value = default(bool);
                charactersConsumed = 0;
                if (IsTrue(text, length))
                {
                    charactersConsumed = 4;
                    value = true;
                    return true;
                }
                else if (IsFalse(text, length))
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
            public static bool TryParseBoolean(ReadOnlySpan<char> text, out bool value)
            {
                value = default(bool);
                if (IsTrue(text))
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
                if (IsTrue(text))
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