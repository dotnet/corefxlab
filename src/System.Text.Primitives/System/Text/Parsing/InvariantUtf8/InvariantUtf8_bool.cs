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
                int consumed;
                var span = new ReadOnlySpan<byte>(text, length);
                return PrimitiveParser.TryParseBoolean(span, out value, out consumed, EncodingData.InvariantUtf8);
            }
            public unsafe static bool TryParseBoolean(byte* text, int length, out bool value, out int consumed)
            {
                var span = new ReadOnlySpan<byte>(text, length);
                return PrimitiveParser.TryParseBoolean(span, out value, out consumed, EncodingData.InvariantUtf8);
            }
            public static bool TryParseBoolean(ReadOnlySpan<byte> text, out bool value)
            {
                int consumed;
                return PrimitiveParser.TryParseBoolean(text, out value, out consumed, EncodingData.InvariantUtf8);
            }
            public static bool TryParseBoolean(ReadOnlySpan<byte> text, out bool value, out int consumed)
            {
                return PrimitiveParser.TryParseBoolean(text, out value, out consumed, EncodingData.InvariantUtf8);
            }
        }
    }
}