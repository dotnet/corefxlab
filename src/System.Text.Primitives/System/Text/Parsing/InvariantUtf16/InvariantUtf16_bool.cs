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
            public unsafe static bool TryParseBoolean(char* text, int length, out bool value)
            {
                int consumedBytes;
                var span = new ReadOnlySpan<byte>(text, length);
                return PrimitiveParser2.TryParseBoolean(span, out value, out consumedBytes, EncodingData.InvariantUtf16);
            }
            public unsafe static bool TryParseBoolean(char* text, int length, out bool value, out int consumedBytes)
            {
                var span = new ReadOnlySpan<byte>(text, length);
                return PrimitiveParser2.TryParseBoolean(span, out value, out consumedBytes, EncodingData.InvariantUtf16);
            }
            public static bool TryParseBoolean(ReadOnlySpan<byte> text, out bool value)
            {
                int consumedBytes;
                return PrimitiveParser2.TryParseBoolean(text, out value, out consumedBytes, EncodingData.InvariantUtf16);
            }
            public static bool TryParseBoolean(ReadOnlySpan<byte> text, out bool value, out int consumedBytes)
            {
                return PrimitiveParser2.TryParseBoolean(text, out value, out consumedBytes, EncodingData.InvariantUtf16);
            }
        }
    }
}