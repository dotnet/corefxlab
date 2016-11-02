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
                int consumed;
                var span = new ReadOnlySpan<byte>(text, length * 2);
                return PrimitiveParser.TryParseBoolean(span, out value, out consumed, EncodingData.InvariantUtf16);
            }
            public unsafe static bool TryParseBoolean(char* text, int length, out bool value, out int consumed)
            {
                var span = new ReadOnlySpan<byte>(text, length * 2);
                int bytesConsumed;
                bool result = PrimitiveParser.TryParseBoolean(span, out value, out bytesConsumed, EncodingData.InvariantUtf16);
                consumed = bytesConsumed / 2;
                return result;
            }
            public static bool TryParseBoolean(ReadOnlySpan<char> text, out bool value)
            {
                int consumed;
                var byteSpan = text.Cast<char, byte>();
                return PrimitiveParser.TryParseBoolean(byteSpan, out value, out consumed, EncodingData.InvariantUtf16);
            }
            public static bool TryParseBoolean(ReadOnlySpan<char> text, out bool value, out int consumed)
            {
                var byteSpan = text.Cast<char, byte>();
                int bytesConsumed;
                bool result = PrimitiveParser.TryParseBoolean(byteSpan, out value, out bytesConsumed, EncodingData.InvariantUtf16);
                consumed = bytesConsumed / 2;
                return result;
            }
        }
    }
}