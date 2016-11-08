// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class PrimitiveParser
    {
        public static partial class InvariantUtf8
        {
            public unsafe static bool TryParseBoolean(byte* text, int length, out bool value)
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
            public unsafe static bool TryParseBoolean(byte* text, int length, out bool value, out int bytesConsumed)
            {
                value = default(bool);
                bytesConsumed = 0;
                if (IsTrue(text, length))
                {
                    bytesConsumed = 4;
                    value = true;
                    return true;
                }
                else if (IsFalse(text, length))
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
            public static bool TryParseBoolean(ReadOnlySpan<byte> text, out bool value)
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
            public static bool TryParseBoolean(ReadOnlySpan<byte> text, out bool value, out int bytesConsumed)
            {
                value = default(bool);
                bytesConsumed = 0;
                if (IsTrue(text))
                {
                    bytesConsumed = 4;
                    value = true;
                    return true;
                }
                else if (IsFalse(text))
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
        }
    }
}