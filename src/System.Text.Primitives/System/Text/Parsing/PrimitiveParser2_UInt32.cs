// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class PrimitiveParser2
    {
        #region helpers
        #endregion

        public static bool TryParseUInt32(ReadOnlySpan<byte> text, out uint value, out int consumedBytes, EncodingData encoding = default(EncodingData), Format.Parsed format = default(Format.Parsed))
        {
            return PrimitiveParser.TryParseUInt32(text, encoding, format, out value, out consumedBytes);
        }
    }
}