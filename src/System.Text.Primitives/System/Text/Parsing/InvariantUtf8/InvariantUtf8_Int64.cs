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
            public unsafe static bool TryParseInt64(byte* text, int length, out long value)
            {
                int consumed;
                var span = new ReadOnlySpan<byte>(text, length);
                return PrimitiveParser.TryParseInt64(span, out value, out consumed, EncodingData.InvariantUtf8);
            }
            public unsafe static bool TryParseInt64(byte* text, int length, out long value, out int bytesConsumed)
            {
                var span = new ReadOnlySpan<byte>(text, length);
                return PrimitiveParser.TryParseInt64(span, out value, out bytesConsumed, EncodingData.InvariantUtf8);
            }
            public static bool TryParseInt64(ReadOnlySpan<byte> text, out long value)
            {
                int consumed;
                return PrimitiveParser.TryParseInt64(text, out value, out consumed, EncodingData.InvariantUtf8);
            }
            public static bool TryParseInt64(ReadOnlySpan<byte> text, out long value, out int bytesConsumed)
            {
                return PrimitiveParser.TryParseInt64(text, out value, out bytesConsumed, EncodingData.InvariantUtf8);
            }

            public static partial class Hex
            {
                public unsafe static bool TryParseInt64(byte* text, int length, out long value)
                {
                    int consumed;
                    var span = new ReadOnlySpan<byte>(text, length);
                    return PrimitiveParser.TryParseInt64(span, out value, out consumed, EncodingData.InvariantUtf8, 'X');
                }
                public unsafe static bool TryParseInt64(byte* text, int length, out long value, out int bytesConsumed)
                {
                    var span = new ReadOnlySpan<byte>(text, length);
                    return PrimitiveParser.TryParseInt64(span, out value, out bytesConsumed, EncodingData.InvariantUtf8, 'X');
                }
                public static bool TryParseInt64(ReadOnlySpan<byte> text, out long value)
                {
                    int consumed;
                    return PrimitiveParser.TryParseInt64(text, out value, out consumed, EncodingData.InvariantUtf8, 'X');
                }
                public static bool TryParseInt64(ReadOnlySpan<byte> text, out long value, out int bytesConsumed)
                {
                    return PrimitiveParser.TryParseInt64(text, out value, out bytesConsumed, EncodingData.InvariantUtf8, 'X');
                }
            }
        }
    }
}