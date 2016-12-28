// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class PrimitiveParser
    {
        public static bool TryParseSByte(ReadOnlySpan<byte> text, out sbyte value, out int bytesConsumed, EncodingData encoding = default(EncodingData), TextFormat format = default(TextFormat))
        {
            return Internal.InternalParser.TryParseSByte(text, format, encoding, out value, out bytesConsumed);
        }
    }
}