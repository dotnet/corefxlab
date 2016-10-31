// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class PrimitiveParser2
    {
        public static partial class InvariantUtf16
        {
            public unsafe static bool TryParseUInt32(char* text, int length, out uint value)
            {
                int consumed;
                var span = new ReadOnlySpan<byte>(text, length);
                return PrimitiveParser2.TryParseUInt32(span, out value, out consumed, EncodingData.InvariantUtf16);
            }
            public unsafe static bool TryParseUInt32(char* text, int length, out uint value, out int consumed)
            {
                var span = new ReadOnlySpan<byte>(text, length);
                return PrimitiveParser2.TryParseUInt32(span, out value, out consumed, EncodingData.InvariantUtf16);
            }
            public static bool TryParseUInt32(ReadOnlySpan<byte> text, out uint value)
            {
                int consumed;
                return PrimitiveParser2.TryParseUInt32(text, out value, out consumed, EncodingData.InvariantUtf16);
            }
            public static bool TryParseUInt32(ReadOnlySpan<byte> text, out uint value, out int consumed)
            {
                return PrimitiveParser2.TryParseUInt32(text, out value, out consumed, EncodingData.InvariantUtf16);
            }
            public static partial class Hex
            {
                public unsafe static bool TryParseUInt32(char* text, int length, out uint value)
                {
                    int consumed;
                    var span = new ReadOnlySpan<byte>(text, length);
                    return PrimitiveParser2.TryParseUInt32(span, out value, out consumed, EncodingData.InvariantUtf16, 'X');
                }
                public unsafe static bool TryParseUInt32(char* text, int length, out uint value, out int consumed)
                {
                    var span = new ReadOnlySpan<byte>(text, length);
                    return PrimitiveParser2.TryParseUInt32(span, out value, out consumed, EncodingData.InvariantUtf16, 'X');
                }
                public static bool TryParseUInt32(ReadOnlySpan<byte> text, out uint value)
                {
                    int consumed;
                    return PrimitiveParser2.TryParseUInt32(text, out value, out consumed, EncodingData.InvariantUtf16, 'X');
                }
                public static bool TryParseUInt32(ReadOnlySpan<byte> text, out uint value, out int consumed)
                {
                    return PrimitiveParser2.TryParseUInt32(text, out value, out consumed, EncodingData.InvariantUtf16, 'X');
                }
            }
        }
    }
}