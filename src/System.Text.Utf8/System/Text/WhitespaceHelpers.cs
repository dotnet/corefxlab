// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Utf8.Resources;

namespace System.Text
{
    internal static class WhitespaceHelpers
    {
        public static ReadOnlySpan<byte> TrimEndUtf8(ReadOnlySpan<byte> data)
        {
            while (data.Length > 0)
            {
                uint firstCodeUnit = data[data.Length - 1];

                if (firstCodeUnit == 0x0020U /* SPACE U+0020 */ || UnicodeHelpers.IsInRangeInclusive(firstCodeUnit, 0x0009U, 0x000DU) /* U+0009..U+000D C0 controls */)
                {
                    data = data.Slice(1);
                    continue;
                }

                if (UnicodeHelpers.IsAsciiCodePoint(firstCodeUnit))
                {
                    break; // non-whitespace ASCII character, we're finished
                }

                // TODO: On full framework, figure out a replacement to GetUnicodeCategory(int)

                UnicodeScalar currentScalar = ReadLastScalar(data);

                if (UnicodeHelpers.IsInRangeInclusive(
                    (uint)CharUnicodeInfo.GetUnicodeCategory((int)currentScalar.Value),
                    (uint)UnicodeCategory.SpaceSeparator,
                    (uint)UnicodeCategory.ParagraphSeparator))
                {
                    data = data.Slice(0, data.Length - currentScalar.Utf8SequenceLength);
                    continue;
                }

                // non-whitespace multi-code unit character, we're finished
                break;
            }

            return data;
        }

        public static ReadOnlySpan<byte> TrimStartUtf8(ReadOnlySpan<byte> data)
        {
            while (data.Length > 0)
            {
                // The list of Unicode whitespace characters is given by
                // http://unicode.org/cldr/utility/list-unicodeset.jsp?a=%5Cp%7Bwhitespace%7D

                // Per the Unicode stability policy, properties on assigned characters cannot be modified
                // in a way that would change their identity. This means that no new whitespace characters
                // will be added within the ASCII range, so we can special-case all of them right now.
                // We special-case roughly in order of likelihood.

                uint firstCodeUnit = data[0];

                if (firstCodeUnit == 0x0020U /* SPACE U+0020 */ || UnicodeHelpers.IsInRangeInclusive(firstCodeUnit, 0x0009U, 0x000DU) /* U+0009..U+000D C0 controls */)
                {
                    data = data.Slice(1);
                    continue;
                }

                if (UnicodeHelpers.IsAsciiCodePoint(firstCodeUnit))
                {
                    break; // non-whitespace ASCII character, we're finished
                }

                // TODO: On full framework, figure out a replacement to GetUnicodeCategory(int)

                UnicodeScalar currentScalar = ReadFirstScalar(data);

                if (UnicodeHelpers.IsInRangeInclusive(
                    (uint)CharUnicodeInfo.GetUnicodeCategory((int)currentScalar.Value),
                    (uint)UnicodeCategory.SpaceSeparator,
                    (uint)UnicodeCategory.ParagraphSeparator))
                {
                    data = data.Slice(currentScalar.Utf8SequenceLength);
                    continue;
                }

                // non-whitespace multi-code unit character, we're finished
                break;
            }

            return data;
        }

        // TODO: Implement me
        private static UnicodeScalar ReadFirstScalar(ReadOnlySpan<byte> data) => throw null;

        // TODO: Implement me
        private static UnicodeScalar ReadLastScalar(ReadOnlySpan<byte> data) => throw null;
    }
}
