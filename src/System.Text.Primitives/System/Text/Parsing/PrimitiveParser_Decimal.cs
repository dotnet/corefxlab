// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class PrimitiveParser
    {
        public static bool TryParseDecimal(ReadOnlySpan<byte> text, out decimal value, out int bytesConsumed, EncodingData encoding = default(EncodingData))
        {
            encoding = encoding.IsDefault ? EncodingData.InvariantUtf8 : encoding;

            bytesConsumed = 0;
            value = default(decimal);

            if (encoding.IsInvariantUtf8)
            {
                return InvariantUtf8.TryParseDecimal(text, out value, out bytesConsumed);
            }
            else if (encoding.IsInvariantUtf16)
            {
                ReadOnlySpan<char> textChars = text.Cast<byte, char>();
                int charactersConsumed;
                bool result = InvariantUtf16.TryParseDecimal(textChars, out value, out charactersConsumed);
                bytesConsumed = charactersConsumed * sizeof(char);
                return result;
            }

            return false;
        }
    }
}