﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class PrimitiveParser
    {
        public static partial class InvariantUtf16
        {
            public unsafe static bool TryParseInt32(char* text, int length, out int value)
            {
                int consumed;
                var span = new ReadOnlySpan<byte>(text, length * 2);
                return PrimitiveParser.TryParseInt32(span, out value, out consumed, EncodingData.InvariantUtf16);
            }
            public unsafe static bool TryParseInt32(char* text, int length, out int value, out int charactersConsumed)
            {
                var span = new ReadOnlySpan<byte>(text, length * 2);
                int bytesConsumed;
                bool result = PrimitiveParser.TryParseInt32(span, out value, out bytesConsumed, EncodingData.InvariantUtf16);
                charactersConsumed = bytesConsumed / 2;
                return result;
            }
            public static bool TryParseInt32(ReadOnlySpan<char> text, out int value)
            {
                int consumed;
                var byteSpan = text.Cast<char, byte>();
                return PrimitiveParser.TryParseInt32(byteSpan, out value, out consumed, EncodingData.InvariantUtf16);
            }
            public static bool TryParseInt32(ReadOnlySpan<char> text, out int value, out int charactersConsumed)
            {
                var byteSpan = text.Cast<char, byte>();
                int bytesConsumed;
                bool result = PrimitiveParser.TryParseInt32(byteSpan, out value, out bytesConsumed, EncodingData.InvariantUtf16);
                charactersConsumed = bytesConsumed / 2;
                return result;
            }

            public static partial class Hex
            {
                public unsafe static bool TryParseInt32(char* text, int length, out int value)
                {
                    int consumed;
                    var span = new ReadOnlySpan<byte>(text, length * 2);
                    return PrimitiveParser.TryParseInt32(span, out value, out consumed, EncodingData.InvariantUtf16, 'X');
                }
                public unsafe static bool TryParseInt32(char* text, int length, out int value, out int charactersConsumed)
                {
                    var span = new ReadOnlySpan<byte>(text, length * 2);
                    int bytesConsumed;
                    bool result = PrimitiveParser.TryParseInt32(span, out value, out bytesConsumed, EncodingData.InvariantUtf16, 'X');
                    charactersConsumed = bytesConsumed / 2;
                    return result;
                }
                public static bool TryParseInt32(ReadOnlySpan<char> text, out int value)
                {
                    int consumed;
                    var byteSpan = text.Cast<char, byte>();
                    return PrimitiveParser.TryParseInt32(byteSpan, out value, out consumed, EncodingData.InvariantUtf16, 'X');
                }
                public static bool TryParseInt32(ReadOnlySpan<char> text, out int value, out int charactersConsumed)
                {
                    var byteSpan = text.Cast<char, byte>();
                    int bytesConsumed;
                    bool result = PrimitiveParser.TryParseInt32(byteSpan, out value, out bytesConsumed, EncodingData.InvariantUtf16, 'X');
                    charactersConsumed = bytesConsumed / 2;
                    return result;
                }
            }
        }
    }
}