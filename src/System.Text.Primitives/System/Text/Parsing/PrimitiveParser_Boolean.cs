// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class PrimitiveParser
    {
        public static bool TryParseBoolean(ReadOnlySpan<byte> text, out bool value, out int bytesConsumed, EncodingData encoding = default(EncodingData))
        {
            encoding = encoding.IsDefault ? EncodingData.InvariantUtf8 : encoding;

            bytesConsumed = 0;
            value = default(bool);

            if (encoding.IsInvariantUtf8)
            {
                return InvariantUtf8.TryParseBoolean(text, out value, out bytesConsumed);
            }
            if (encoding.IsInvariantUtf16)
            {
                ReadOnlySpan<char> textChars = text.Cast<byte, char>();
                int charactersConsumed;
                bool result = InvariantUtf16.TryParseBoolean(textChars, out value, out charactersConsumed);
                bytesConsumed = charactersConsumed * sizeof(char);
                return result;
            }

            return false;
        }
    }
}