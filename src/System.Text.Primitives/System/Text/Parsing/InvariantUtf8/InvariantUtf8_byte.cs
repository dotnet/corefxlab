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
            public unsafe static bool TryParseByte(byte* text, int length, out byte value)
            {
                int consumed;
                var span = new ReadOnlySpan<byte>(text, length);
                return PrimitiveParser.TryParseByte(span, out value, out consumed, EncodingData.InvariantUtf8);
            }
            public unsafe static bool TryParseByte(byte* text, int length, out byte value, out int bytesConsumed)
            {
                var span = new ReadOnlySpan<byte>(text, length);
                return PrimitiveParser.TryParseByte(span, out value, out bytesConsumed, EncodingData.InvariantUtf8);
            }
            public static bool TryParseByte(ReadOnlySpan<byte> text, out byte value)
            {
                int consumed;
                return PrimitiveParser.TryParseByte(text, out value, out consumed, EncodingData.InvariantUtf8);
            }
            public static bool TryParseByte(ReadOnlySpan<byte> text, out byte value, out int bytesConsumed)
            {
                return PrimitiveParser.TryParseByte(text, out value, out bytesConsumed, EncodingData.InvariantUtf8);
            }

            public static partial class Hex
            {
                public unsafe static bool TryParseByte(byte* text, int length, out byte value)
                {
                    int consumed;
                    var span = new ReadOnlySpan<byte>(text, length);
                    return PrimitiveParser.TryParseByte(span, out value, out consumed, EncodingData.InvariantUtf8, 'X');
                }
                public unsafe static bool TryParseByte(byte* text, int length, out byte value, out int bytesConsumed)
                {
                    var span = new ReadOnlySpan<byte>(text, length);
                    return PrimitiveParser.TryParseByte(span, out value, out bytesConsumed, EncodingData.InvariantUtf8, 'X');
                }
                public static bool TryParseByte(ReadOnlySpan<byte> text, out byte value)
                {
                    int consumed;
                    return PrimitiveParser.TryParseByte(text, out value, out consumed, EncodingData.InvariantUtf8, 'X');
                }
                public static bool TryParseByte(ReadOnlySpan<byte> text, out byte value, out int bytesConsumed)
                {
                    return PrimitiveParser.TryParseByte(text, out value, out bytesConsumed, EncodingData.InvariantUtf8, 'X');
                }
            }
        }
    }
}