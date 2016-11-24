// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class PrimitiveParser
    {
        public static partial class InvariantUtf8
        {
            public unsafe static bool TryParseUInt64(byte* text, int length, out ulong value)
            {
                if (length < 1)
                {
                    value = default(ulong);
                    return false;
                }

                // Parse the first digit separately. If invalid here, we need to return false.
                ulong firstDigit = text[0] - 48u; // '0'
                if (firstDigit > 9)
                {
                    value = default(ulong);
                    return false;
                }
                value = firstDigit;

                if (length < UInt64OverflowLength)
                {
                    // Length is less than OVERFLOW_LENGTH; overflow is not possible
                    for (int index = 1; index < length; index++)
                    {
                        ulong nextDigit = text[index] - 48u; // '0'
                        if (nextDigit > 9)
                        {
                            return true;
                        }
                        value = value * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than OVERFLOW_LENGTH; overflow is only possible after OVERFLOW_LENGTH
                    // digits. There may be no overflow after OVERFLOW_LENGTH if there are leading zeroes.
                    for (int index = 1; index < UInt64OverflowLength - 1; index++)
                    {
                        ulong nextDigit = text[index] - 48u; // '0'
                        if (nextDigit > 9)
                        {
                            return true;
                        }
                        value = value * 10 + nextDigit;
                    }
                    for (int index = UInt64OverflowLength - 1; index < length; index++)
                    {
                        ulong nextDigit = text[index] - 48u; // '0'
                        if (nextDigit > 9)
                        {
                            return true;
                        }
                        // ulong.MaxValue is a constant 18446744073709551615
                        // ulong.MaxValue / 10 is 1844674407370955161
                        // (value > ulong.MaxValue / 10) implies overflow when appended to; e.g., 18446744073709551620 > 18446744073709551615
                        // if value == 1844674407370955161, any nextDigit greater than 5 implies overflow
                        if (value > ulong.MaxValue / 10 || (value == ulong.MaxValue / 10 && nextDigit > 5))
                        {
                            value = default(ulong);
                            return false;
                        }
                        value = value * 10 + nextDigit;
                    }
                }

                return true;
            }
            public unsafe static bool TryParseUInt64(byte* text, int length, out ulong value, out int bytesConsumed)
            {
                if (length < 1)
                {
                    value = default(ulong);
                    bytesConsumed = 0;
                    return false;
                }

                // Parse the first digit separately. If invalid here, we need to return false.
                ulong firstDigit = text[0] - 48u; // '0'
                if (firstDigit > 9)
                {
                    value = default(ulong);
                    bytesConsumed = 0;
                    return false;
                }
                value = firstDigit;

                if (length < UInt64OverflowLength)
                {
                    // Length is less than OVERFLOW_LENGTH; overflow is not possible
                    for (int index = 1; index < length; index++)
                    {
                        ulong nextDigit = text[index] - 48u; // '0'
                        if (nextDigit > 9)
                        {
                            bytesConsumed = index;
                            return true;
                        }
                        value = value * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than OVERFLOW_LENGTH; overflow is only possible after OVERFLOW_LENGTH
                    // digits. There may be no overflow after OVERFLOW_LENGTH if there are leading zeroes.
                    for (int index = 1; index < UInt64OverflowLength - 1; index++)
                    {
                        ulong nextDigit = text[index] - 48u; // '0'
                        if (nextDigit > 9)
                        {
                            bytesConsumed = index;
                            return true;
                        }
                        value = value * 10 + nextDigit;
                    }
                    for (int index = UInt64OverflowLength - 1; index < length; index++)
                    {
                        ulong nextDigit = text[index] - 48u; // '0'
                        if (nextDigit > 9)
                        {
                            bytesConsumed = index;
                            return true;
                        }
                        // ulong.MaxValue is a constant 18446744073709551615
                        // ulong.MaxValue / 10 is 1844674407370955161
                        // (value > ulong.MaxValue / 10) implies overflow when appended to; e.g., 18446744073709551620 > 18446744073709551615
                        // if value == 1844674407370955161, any nextDigit greater than 5 implies overflow
                        if (value > ulong.MaxValue / 10 || (value == ulong.MaxValue / 10 && nextDigit > 5))
                        {
                            value = default(ulong);
                            bytesConsumed = 0;
                            return false;
                        }
                        value = value * 10 + nextDigit;
                    }
                }

                bytesConsumed = length;
                return true;
            }
            public static bool TryParseUInt64(ReadOnlySpan<byte> text, out ulong value)
            {
                if (text.Length < 1)
                {
                    value = default(ulong);
                    return false;
                }

                // Parse the first digit separately. If invalid here, we need to return false.
                ulong firstDigit = text[0] - 48u; // '0'
                if (firstDigit > 9)
                {
                    value = default(ulong);
                    return false;
                }
                value = firstDigit;

                if (text.Length < UInt64OverflowLength)
                {
                    // Length is less than OVERFLOW_LENGTH; overflow is not possible
                    for (int index = 1; index < text.Length; index++)
                    {
                        ulong nextDigit = text[index] - 48u; // '0'
                        if (nextDigit > 9)
                        {
                            return true;
                        }
                        value = value * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than OVERFLOW_LENGTH; overflow is only possible after OVERFLOW_LENGTH
                    // digits. There may be no overflow after OVERFLOW_LENGTH if there are leading zeroes.
                    for (int index = 1; index < UInt64OverflowLength - 1; index++)
                    {
                        ulong nextDigit = text[index] - 48u; // '0'
                        if (nextDigit > 9)
                        {
                            return true;
                        }
                        value = value * 10 + nextDigit;
                    }
                    for (int index = UInt64OverflowLength - 1; index < text.Length; index++)
                    {
                        ulong nextDigit = text[index] - 48u; // '0'
                        if (nextDigit > 9)
                        {
                            return true;
                        }
                        // ulong.MaxValue is a constant 18446744073709551615
                        // ulong.MaxValue / 10 is 1844674407370955161
                        // (value > ulong.MaxValue / 10) implies overflow when appended to; e.g., 18446744073709551620 > 18446744073709551615
                        // if value == 1844674407370955161, any nextDigit greater than 5 implies overflow
                        if (value > ulong.MaxValue / 10 || (value == ulong.MaxValue / 10 && nextDigit > 5))
                        {
                            value = default(ulong);
                            return false;
                        }
                        value = value * 10 + nextDigit;
                    }
                }

                return true;
            }
            public static bool TryParseUInt64(ReadOnlySpan<byte> text, out ulong value, out int bytesConsumed)
            {
                if (text.Length < 1)
                {
                    value = default(ulong);
                    bytesConsumed = 0;
                    return false;
                }

                // Parse the first digit separately. If invalid here, we need to return false.
                ulong firstDigit = text[0] - 48u; // '0'
                if (firstDigit > 9)
                {
                    value = default(ulong);
                    bytesConsumed = 0;
                    return false;
                }
                value = firstDigit;

                if (text.Length < UInt64OverflowLength)
                {
                    // Length is less than OVERFLOW_LENGTH; overflow is not possible
                    for (int index = 1; index < text.Length; index++)
                    {
                        ulong nextDigit = text[index] - 48u; // '0'
                        if (nextDigit > 9)
                        {
                            bytesConsumed = index;
                            return true;
                        }
                        value = value * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than OVERFLOW_LENGTH; overflow is only possible after OVERFLOW_LENGTH
                    // digits. There may be no overflow after OVERFLOW_LENGTH if there are leading zeroes.
                    for (int index = 1; index < UInt64OverflowLength - 1; index++)
                    {
                        ulong nextDigit = text[index] - 48u; // '0'
                        if (nextDigit > 9)
                        {
                            bytesConsumed = index;
                            return true;
                        }
                        value = value * 10 + nextDigit;
                    }
                    for (int index = UInt64OverflowLength - 1; index < text.Length; index++)
                    {
                        ulong nextDigit = text[index] - 48u; // '0'
                        if (nextDigit > 9)
                        {
                            bytesConsumed = index;
                            return true;
                        }
                        // ulong.MaxValue is a constant 18446744073709551615
                        // ulong.MaxValue / 10 is 1844674407370955161
                        // (value > ulong.MaxValue / 10) implies overflow when appended to; e.g., 18446744073709551620 > 18446744073709551615
                        // if value == 1844674407370955161, any nextDigit greater than 5 implies overflow
                        if (value > ulong.MaxValue / 10 || (value == ulong.MaxValue / 10 && nextDigit > 5))
                        {
                            value = default(ulong);
                            bytesConsumed = 0;
                            return false;
                        }
                        value = value * 10 + nextDigit;
                    }
                }

                bytesConsumed = text.Length;
                return true;
            }

            public static partial class Hex
            {
                public unsafe static bool TryParseUInt64(byte* text, int length, out ulong value)
                {
                    if (length < 1)
                    {
                        value = default(ulong);
                        return false;
                    }

                    // Cache s_hexLookup in order to avoid static constructor checks
                    byte[] hexLookup = s_HexLookup;

                    // Parse the first digit separately. If invalid here, we need to return false.
                    byte firstByte = text[0];
                    byte firstDigit = hexLookup[firstByte];
                    if (firstDigit == 0xFF)
                    {
                        value = default(ulong);
                        return false;
                    }
                    value = firstDigit;

                    if (length <= UInt64OverflowLengthHex)
                    {
                        // Length is less than or equal to OVERFLOW_LENGTH; overflow is not possible
                        for (int index = 1; index < length; index++)
                        {
                            byte nextByte = text[index];
                            byte nextDigit = hexLookup[nextByte];
                            if (nextDigit == 0xFF)
                            {
                                return true;
                            }
                            value = value * 0x10 + nextDigit;
                        }
                    }
                    else
                    {
                        // Length is greater than OVERFLOW_LENGTH; overflow is only possible after OVERFLOW_LENGTH
                        // digits. There may be no overflow after OVERFLOW_LENGTH if there are leading zeroes.
                        for (int index = 1; index < UInt64OverflowLengthHex; index++)
                        {
                            byte nextByte = text[index];
                            byte nextDigit = hexLookup[nextByte];
                            if (nextDigit == 0xFF)
                            {
                                return true;
                            }
                            value = value * 0x10 + nextDigit;
                        }
                        for (int index = UInt64OverflowLengthHex; index < length; index++)
                        {
                            byte nextByte = text[index];
                            byte nextDigit = hexLookup[nextByte];
                            if (nextDigit == 0xFF)
                            {
                                return true;
                            }
                            // ulong.MaxValue is a constant 0xFFFFFFFFFFFFFFFF
                            // If we try to append a digit to anything larger than 0x0FFFFFFFFFFFFFFF, there will be overflow
                            if (value > ulong.MaxValue / 0x10)
                            {
                                value = default(ulong);
                                return false;
                            }
                            value = value * 0x10 + nextDigit;
                        }
                    }

                    return true;
                }
                public unsafe static bool TryParseUInt64(byte* text, int length, out ulong value, out int bytesConsumed)
                {
                    if (length < 1)
                    {
                        value = default(ulong);
                        bytesConsumed = 0;
                        return false;
                    }

                    // Cache s_hexLookup in order to avoid static constructor checks
                    byte[] hexLookup = s_HexLookup;

                    // Parse the first digit separately. If invalid here, we need to return false.
                    byte firstByte = text[0];
                    byte firstDigit = hexLookup[firstByte];
                    if (firstDigit == 0xFF)
                    {
                        value = default(ulong);
                        bytesConsumed = 0;
                        return false;
                    }
                    value = firstDigit;

                    if (length <= UInt64OverflowLengthHex)
                    {
                        // Length is less than or equal to OVERFLOW_LENGTH; overflow is not possible
                        for (int index = 1; index < length; index++)
                        {
                            byte nextByte = text[index];
                            byte nextDigit = hexLookup[nextByte];
                            if (nextDigit == 0xFF)
                            {
                                bytesConsumed = index;
                                return true;
                            }
                            value = value * 0x10 + nextDigit;
                        }
                    }
                    else
                    {
                        // Length is greater than OVERFLOW_LENGTH; overflow is only possible after OVERFLOW_LENGTH
                        // digits. There may be no overflow after OVERFLOW_LENGTH if there are leading zeroes.
                        for (int index = 1; index < UInt64OverflowLengthHex; index++)
                        {
                            byte nextByte = text[index];
                            byte nextDigit = hexLookup[nextByte];
                            if (nextDigit == 0xFF)
                            {
                                bytesConsumed = index;
                                return true;
                            }
                            value = value * 0x10 + nextDigit;
                        }
                        for (int index = UInt64OverflowLengthHex; index < length; index++)
                        {
                            byte nextByte = text[index];
                            byte nextDigit = hexLookup[nextByte];
                            if (nextDigit == 0xFF)
                            {
                                bytesConsumed = index;
                                return true;
                            }
                            // ulong.MaxValue is a constant 0xFFFFFFFFFFFFFFFF
                            // If we try to append a digit to anything larger than 0x0FFFFFFFFFFFFFFF, there will be overflow
                            if (value > ulong.MaxValue / 0x10)
                            {
                                value = default(ulong);
                                bytesConsumed = 0;
                                return false;
                            }
                            value = value * 0x10 + nextDigit;
                        }
                    }

                    bytesConsumed = length;
                    return true;
                }
                public static bool TryParseUInt64(ReadOnlySpan<byte> text, out ulong value)
                {
                    if (text.Length < 1)
                    {
                        value = default(ulong);
                        return false;
                    }

                    // Cache s_hexLookup in order to avoid static constructor checks
                    byte[] hexLookup = s_HexLookup;

                    // Parse the first digit separately. If invalid here, we need to return false.
                    byte firstByte = text[0];
                    byte firstDigit = hexLookup[firstByte];
                    if (firstDigit == 0xFF)
                    {
                        value = default(ulong);
                        return false;
                    }
                    value = firstDigit;

                    if (text.Length <= UInt64OverflowLengthHex)
                    {
                        // Length is less than or equal to OVERFLOW_LENGTH; overflow is not possible
                        for (int index = 1; index < text.Length; index++)
                        {
                            byte nextByte = text[index];
                            byte nextDigit = hexLookup[nextByte];
                            if (nextDigit == 0xFF)
                            {
                                return true;
                            }
                            value = value * 0x10 + nextDigit;
                        }
                    }
                    else
                    {
                        // Length is greater than OVERFLOW_LENGTH; overflow is only possible after OVERFLOW_LENGTH
                        // digits. There may be no overflow after OVERFLOW_LENGTH if there are leading zeroes.
                        for (int index = 1; index < UInt64OverflowLengthHex; index++)
                        {
                            byte nextByte = text[index];
                            byte nextDigit = hexLookup[nextByte];
                            if (nextDigit == 0xFF)
                            {
                                return true;
                            }
                            value = value * 0x10 + nextDigit;
                        }
                        for (int index = UInt64OverflowLengthHex; index < text.Length; index++)
                        {
                            byte nextByte = text[index];
                            byte nextDigit = hexLookup[nextByte];
                            if (nextDigit == 0xFF)
                            {
                                return true;
                            }
                            // ulong.MaxValue is a constant 0xFFFFFFFFFFFFFFFF
                            // If we try to append a digit to anything larger than 0x0FFFFFFFFFFFFFFF, there will be overflow
                            if (value > ulong.MaxValue / 0x10)
                            {
                                value = default(ulong);
                                return false;
                            }
                            value = value * 0x10 + nextDigit;
                        }
                    }

                    return true;
                }
                public static bool TryParseUInt64(ReadOnlySpan<byte> text, out ulong value, out int bytesConsumed)
                {
                    if (text.Length < 1)
                    {
                        value = default(ulong);
                        bytesConsumed = 0;
                        return false;
                    }

                    // Cache s_hexLookup in order to avoid static constructor checks
                    byte[] hexLookup = s_HexLookup;

                    // Parse the first digit separately. If invalid here, we need to return false.
                    byte firstByte = text[0];
                    byte firstDigit = hexLookup[firstByte];
                    if (firstDigit == 0xFF)
                    {
                        value = default(ulong);
                        bytesConsumed = 0;
                        return false;
                    }
                    value = firstDigit;

                    if (text.Length <= UInt64OverflowLengthHex)
                    {
                        // Length is less than or equal to OVERFLOW_LENGTH; overflow is not possible
                        for (int index = 1; index < text.Length; index++)
                        {
                            byte nextByte = text[index];
                            byte nextDigit = hexLookup[nextByte];
                            if (nextDigit == 0xFF)
                            {
                                bytesConsumed = index;
                                return true;
                            }
                            value = value * 0x10 + nextDigit;
                        }
                    }
                    else
                    {
                        // Length is greater than OVERFLOW_LENGTH; overflow is only possible after OVERFLOW_LENGTH
                        // digits. There may be no overflow after OVERFLOW_LENGTH if there are leading zeroes.
                        for (int index = 1; index < UInt64OverflowLengthHex; index++)
                        {
                            byte nextByte = text[index];
                            byte nextDigit = hexLookup[nextByte];
                            if (nextDigit == 0xFF)
                            {
                                bytesConsumed = index;
                                return true;
                            }
                            value = value * 0x10 + nextDigit;
                        }
                        for (int index = UInt64OverflowLengthHex; index < text.Length; index++)
                        {
                            byte nextByte = text[index];
                            byte nextDigit = hexLookup[nextByte];
                            if (nextDigit == 0xFF)
                            {
                                bytesConsumed = index;
                                return true;
                            }
                            // ulong.MaxValue is a constant 0xFFFFFFFFFFFFFFFF
                            // If we try to append a digit to anything larger than 0x0FFFFFFFFFFFFFFF, there will be overflow
                            if (value > ulong.MaxValue / 0x10)
                            {
                                value = default(ulong);
                                bytesConsumed = 0;
                                return false;
                            }
                            value = value * 0x10 + nextDigit;
                        }
                    }

                    bytesConsumed = text.Length;
                    return true;
                }
            }
        }
    }
}