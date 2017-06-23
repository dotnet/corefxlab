// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace System.Text
{
    public static partial class PrimitiveParser
    {
        public static bool TryParseDecimal(ReadOnlySpan<byte> text, out decimal value, out int bytesConsumed, TextEncoder encoder = null)
        {
            encoder = encoder ?? TextEncoder.Utf8;

            bytesConsumed = 0;
            value = default;

            if (encoder.IsInvariantUtf8)
            {
                return InvariantUtf8.TryParseDecimal(text, out value, out bytesConsumed);
            }
            else if (encoder.IsInvariantUtf16)
            {
                ReadOnlySpan<char> textChars = text.NonPortableCast<byte, char>();
                int charactersConsumed;
                bool result = InvariantUtf16.TryParseDecimal(textChars, out value, out charactersConsumed);
                bytesConsumed = charactersConsumed * sizeof(char);
                return result;
            }

            return false;
        }
    }
}