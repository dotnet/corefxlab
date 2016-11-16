// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class PrimitiveParser
    {
        public static bool TryParseUInt16(ReadOnlySpan<byte> text, out ushort value, out int bytesConsumed, EncodingData encoding = default(EncodingData), TextFormat format = default(TextFormat))
        {
            return TryParseUInt16(text, encoding, format, out value, out bytesConsumed);
        }
    }
}