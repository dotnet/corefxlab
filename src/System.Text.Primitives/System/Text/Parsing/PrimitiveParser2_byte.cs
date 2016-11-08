// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class PrimitiveParser
    {
        public static bool TryParseByte(ReadOnlySpan<byte> text, out byte value, out int bytesConsumed, EncodingData encoding = default(EncodingData), Format.Parsed format = default(Format.Parsed))
        {
            return TryParseByte(text, encoding, format, out value, out bytesConsumed);
        }
    }
}