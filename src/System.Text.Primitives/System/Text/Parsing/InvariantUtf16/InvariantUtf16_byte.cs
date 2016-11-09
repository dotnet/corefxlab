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
            public unsafe static bool TryParseByte(char* text, int length, out byte value)
            {
                int consumed;
                var span = new ReadOnlySpan<byte>(text, length * sizeof(char));
                return PrimitiveParser.TryParseByte(span, out value, out consumed, EncodingData.InvariantUtf16);
            }
            public unsafe static bool TryParseByte(char* text, int length, out byte value, out int charactersConsumed)
            {
                var span = new ReadOnlySpan<byte>(text, length * sizeof(char));
                int bytesConsumed;
                bool result = PrimitiveParser.TryParseByte(span, out value, out bytesConsumed, EncodingData.InvariantUtf16);
                charactersConsumed = bytesConsumed / sizeof(char);
                return result;
            }
            public static bool TryParseByte(ReadOnlySpan<char> text, out byte value)
            {
                int consumed;
                var byteSpan = text.Cast<char, byte>();
                return PrimitiveParser.TryParseByte(byteSpan, out value, out consumed, EncodingData.InvariantUtf16);
            }
            public static bool TryParseByte(ReadOnlySpan<char> text, out byte value, out int charactersConsumed)
            {
                var byteSpan = text.Cast<char, byte>();
                int bytesConsumed;
                bool result = PrimitiveParser.TryParseByte(byteSpan, out value, out bytesConsumed, EncodingData.InvariantUtf16);
                charactersConsumed = bytesConsumed / sizeof(char);
                return result;
            }

            public static partial class Hex
            {
                public unsafe static bool TryParseByte(char* text, int length, out byte value)
                {
                    int consumed;
                    var span = new ReadOnlySpan<byte>(text, length * sizeof(char));
                    return PrimitiveParser.TryParseByte(span, out value, out consumed, EncodingData.InvariantUtf16, 'X');
                }
                public unsafe static bool TryParseByte(char* text, int length, out byte value, out int charactersConsumed)
                {
                    var span = new ReadOnlySpan<byte>(text, length * sizeof(char));
                    int bytesConsumed;
                    bool result = PrimitiveParser.TryParseByte(span, out value, out bytesConsumed, EncodingData.InvariantUtf16, 'X');
                    charactersConsumed = bytesConsumed / sizeof(char);
                    return result;
                }
                public static bool TryParseByte(ReadOnlySpan<char> text, out byte value)
                {
                    int consumed;
                    var byteSpan = text.Cast<char, byte>();
                    return PrimitiveParser.TryParseByte(byteSpan, out value, out consumed, EncodingData.InvariantUtf16, 'X');
                }
                public static bool TryParseByte(ReadOnlySpan<char> text, out byte value, out int charactersConsumed)
                {
                    var byteSpan = text.Cast<char, byte>();
                    int bytesConsumed;
                    bool result = PrimitiveParser.TryParseByte(byteSpan, out value, out bytesConsumed, EncodingData.InvariantUtf16, 'X');
                    charactersConsumed = bytesConsumed / sizeof(char);
                    return result;
                }
            }
        }
    }
}