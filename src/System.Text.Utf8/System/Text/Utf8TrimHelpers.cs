// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Text
{
    internal static class Utf8TrimHelpers
    {
        public static ReadOnlySpan<byte> TrimSingleScalar(ReadOnlySpan<byte> data, UnicodeScalar scalarToTrim, TrimType trimType)
        {
            if (scalarToTrim.IsAscii)
            {
                // Asked to trim an ASCII value.

                if ((trimType & TrimType.Start) != 0)
                {
                    int i = 0;
                    for (; i < data.Length; i++)
                    {
                        if (data[i] != (byte)scalarToTrim.Value)
                        {
                            break;
                        }
                    }

                    data = data.Slice(i);
                }

                if ((trimType & TrimType.End) != 0)
                {
                    int i = data.Length - 1;
                    for (; i >= 0; i--)
                    {
                        if (data[i] != (byte)scalarToTrim.Value)
                        {
                            break;
                        }
                    }

                    data = data.Slice(0, i);
                }
            }
            else
            {
                Span<byte> scalarAsUtf8 = stackalloc byte[4]; // worst case scenario
                scalarAsUtf8 = scalarAsUtf8.Slice(0, scalarToTrim.ToUtf8(MemoryMarshal.Cast<byte, Utf8Char>(scalarAsUtf8)));

                // TODO: Can optimize for scalars in range U+0080..U+07FF (2 UTF-8 code units) by reading words at a time.

                if ((trimType & TrimType.Start) != 0)
                {
                    while (data.StartsWith(scalarAsUtf8))
                    {
                        data = data.Slice(scalarAsUtf8.Length);
                    }
                }

                if ((trimType & TrimType.End) != 0)
                {
                    while (data.EndsWith(scalarAsUtf8))
                    {
                        data = data.Slice(data.Length - scalarAsUtf8.Length);
                    }
                }
            }

            return data;
        }

        public static ReadOnlySpan<byte> TrimMultipleScalars(ReadOnlySpan<byte> data, ReadOnlySpan<UnicodeScalar> scalarsToTrim, TrimType trimType)
        {
            ReadOnlySpan<Utf8Char> utf8Data = MemoryMarshal.Cast<byte, Utf8Char>(data);

            if ((trimType & TrimType.Start) != 0)
            {
            Continue:
                var (status, scalar, bytesConsumed) = UnicodeReader.PeekFirstScalar(utf8Data);
                if (status == SequenceValidity.ValidSequence)
                {
                    foreach (var targetScalar in scalarsToTrim)
                    {
                        if (scalar == targetScalar)
                        {
                            utf8Data = utf8Data.Slice(bytesConsumed);
                            goto Continue;
                        }
                    }
                }
            }

            if ((trimType & TrimType.End) != 0)
            {
            Continue:
                var (status, scalar, bytesConsumed) = UnicodeReader.PeekLastScalar(utf8Data);
                if (status == SequenceValidity.ValidSequence)
                {
                    foreach (var targetScalar in scalarsToTrim)
                    {
                        if (scalar == targetScalar)
                        {
                            utf8Data = utf8Data.Slice(0, utf8Data.Length - bytesConsumed);
                            goto Continue;
                        }
                    }
                }
            }

            return MemoryMarshal.Cast<Utf8Char, byte>(utf8Data);
        }

        public static ReadOnlySpan<byte> TrimWhiteSpace(ReadOnlySpan<byte> data, TrimType trimType)
        {
            if ((trimType & TrimType.Start) != 0)
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

                    uint nextScalar = Utf8Enumeration.ReadFirstScalar(data, out int sequenceLength);

                    if (UnicodeHelpers.IsInRangeInclusive(
                        (uint)CharUnicodeInfo.GetUnicodeCategory((int)nextScalar),
                        (uint)UnicodeCategory.SpaceSeparator,
                        (uint)UnicodeCategory.ParagraphSeparator))
                    {
                        data = data.Slice(sequenceLength);
                        continue;
                    }

                    // non-whitespace multi-code unit character, we're finished
                    break;
                }
            }

            if ((trimType & TrimType.End) != 0)
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

                    uint currentScalar = Utf8Enumeration.ReadLastScalar(data, out int sequenceLength);

                    if (UnicodeHelpers.IsInRangeInclusive(
                        (uint)CharUnicodeInfo.GetUnicodeCategory((int)currentScalar),
                        (uint)UnicodeCategory.SpaceSeparator,
                        (uint)UnicodeCategory.ParagraphSeparator))
                    {
                        data = data.Slice(0, data.Length - sequenceLength);
                        continue;
                    }

                    // non-whitespace multi-code unit character, we're finished
                    break;
                }
            }

            return data;
        }
    }
}
