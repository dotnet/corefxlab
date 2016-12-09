﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class PrimitiveParser
    {
        public static partial class InvariantUtf8
        {
            public unsafe static bool TryParseUInt32(byte* text, int length, out uint value)
            {
                if (length < 1)
                {
                    value = default(uint);
                    return false;
                }

                // Parse the first digit separately. If invalid here, we need to return false.
                uint firstDigit = text[0] - 48u; // '0'
                if (firstDigit > 9)
                {
                    value = default(uint);
                    return false;
                }
                value = firstDigit;

                if (length < UInt32OverflowLength)
                {
                    // Length is less than OVERFLOW_LENGTH; overflow is not possible
                    for (int index = 1; index < length; index++)
                    {
                        uint nextDigit = text[index] - 48u; // '0'
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
                    for (int index = 1; index < UInt32OverflowLength - 1; index++)
                    {
                        uint nextDigit = text[index] - 48u; // '0'
                        if (nextDigit > 9)
                        {
                            return true;
                        }
                        value = value * 10 + nextDigit;
                    }
                    for (int index = UInt32OverflowLength - 1; index < length; index++)
                    {
                        uint nextDigit = text[index] - 48u; // '0'
                        if (nextDigit > 9)
                        {
                            return true;
                        }
                        // uint.MaxValue is a constant 4294967295
                        // uint.MaxValue / 10 is 429496729 (integer division truncated)
                        // (value > uint.MaxValue / 10) implies overflow when appended to; e.g., 4294967300 > 4294967295
                        // if value == 429496729, any nextDigit greater than 5 implies overflow
                        if (value > uint.MaxValue / 10 || (value == uint.MaxValue / 10 && nextDigit > 5))
                        {
                            value = default(uint);
                            return false;
                        }
                        value = value * 10 + nextDigit;
                    }
                }
                
                return true;
            }
            
            public unsafe static bool TryParseUInt32(byte* text, int length, out uint value, out int bytesConsumed)
            {
                if (length < 1)
                {
                    bytesConsumed = 0;
                    value = default(uint);
                    return false;
                }

                // Parse the first digit separately. If invalid here, we need to return false.
                uint firstDigit = text[0] - 48u; // '0'
                if (firstDigit > 9)
                {
                    bytesConsumed = 0;
                    value = default(uint);
                    return false;
                }
                value = firstDigit;

                if (length < UInt32OverflowLength)
                {
                    // Length is less than OVERFLOW_LENGTH; overflow is not possible
                    for (int index = 1; index < length; index++)
                    {
                        uint nextDigit = text[index] - 48u; // '0'
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
                    for (int index = 1; index < UInt32OverflowLength - 1; index++)
                    {
                        uint nextDigit = text[index] - 48u; // '0'
                        if (nextDigit > 9)
                        {
                            bytesConsumed = index;
                            return true;
                        }
                        value = value * 10 + nextDigit;
                    }
                    for (int index = UInt32OverflowLength - 1; index < length; index++)
                    {
                        uint nextDigit = text[index] - 48u; // '0'
                        if (nextDigit > 9)
                        {
                            bytesConsumed = index;
                            return true;
                        }
                        // uint.MaxValue is a constant 4294967295
                        // uint.MaxValue / 10 is 429496729 (integer division truncated)
                        // (value > uint.MaxValue / 10) implies overflow when appended to; e.g., 4294967300 > 4294967295
                        // if value == 429496729, any nextDigit greater than 5 implies overflow
                        if (value > uint.MaxValue / 10 || (value == uint.MaxValue / 10 && nextDigit > 5))
                        {
                            bytesConsumed = 0;
                            value = default(uint);
                            return false;
                        }
                        value = value * 10 + nextDigit;
                    }
                }

                bytesConsumed = length;
                return true;
            }
            public static bool TryParseUInt32(ReadOnlySpan<byte> text, out uint value)
            {
                if (text.Length < 1)
                {
                    value = default(uint);
                    return false;
                }

                // Parse the first digit separately. If invalid here, we need to return false.
                uint firstDigit = text[0] - 48u; // '0'
                if (firstDigit > 9)
                {
                    value = default(uint);
                    return false;
                }
                value = firstDigit;

                if (text.Length < UInt32OverflowLength)
                {
                    // Length is less than OVERFLOW_LENGTH; overflow is not possible
                    for (int index = 1; index < text.Length; index++)
                    {
                        uint nextDigit = text[index] - 48u; // '0'
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
                    for (int index = 1; index < UInt32OverflowLength - 1; index++)
                    {
                        uint nextDigit = text[index] - 48u; // '0'
                        if (nextDigit > 9)
                        {
                            return true;
                        }
                        value = value * 10 + nextDigit;
                    }
                    for (int index = UInt32OverflowLength - 1; index < text.Length; index++)
                    {
                        uint nextDigit = text[index] - 48u; // '0'
                        if (nextDigit > 9)
                        {
                            return true;
                        }
                        // uint.MaxValue is a constant 4294967295
                        // uint.MaxValue / 10 is 429496729 (integer division truncated)
                        // (value > uint.MaxValue / 10) implies overflow when appended to; e.g., 4294967300 > 4294967295
                        // if value == 429496729, any nextDigit greater than 5 implies overflow
                        if (value > uint.MaxValue / 10 || (value == uint.MaxValue / 10 && nextDigit > 5))
                        {
                            value = default(uint);
                            return false;
                        }
                        value = value * 10 + nextDigit;
                    }
                }

                return true;
            }
            public static bool TryParseUInt32(ReadOnlySpan<byte> text, out uint value, out int bytesConsumed)
            {
                if (text.Length < 1)
                {
                    bytesConsumed = 0;
                    value = default(uint);
                    return false;
                }

                // Parse the first digit separately. If invalid here, we need to return false.
                uint firstDigit = text[0] - 48u; // '0'
                if (firstDigit > 9)
                {
                    bytesConsumed = 0;
                    value = default(uint);
                    return false;
                }
                value = firstDigit;

                if (text.Length < UInt32OverflowLength)
                {
                    // Length is less than OVERFLOW_LENGTH; overflow is not possible
                    for (int index = 1; index < text.Length; index++)
                    {
                        uint nextDigit = text[index] - 48u; // '0'
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
                    for (int index = 1; index < UInt32OverflowLength - 1; index++)
                    {
                        uint nextDigit = text[index] - 48u; // '0'
                        if (nextDigit > 9)
                        {
                            bytesConsumed = index;
                            return true;
                        }
                        value = value * 10 + nextDigit;
                    }
                    for (int index = UInt32OverflowLength - 1; index < text.Length; index++)
                    {
                        uint nextDigit = text[index] - 48u; // '0'
                        if (nextDigit > 9)
                        {
                            bytesConsumed = index;
                            return true;
                        }
                        // uint.MaxValue is a constant 4294967295
                        // uint.MaxValue / 10 is 429496729 (integer division truncated)
                        // (value > uint.MaxValue / 10) implies overflow when appended to; e.g., 4294967300 > 4294967295
                        // if value == 429496729, any nextDigit greater than 5 implies overflow
                        if (value > uint.MaxValue / 10 || (value == uint.MaxValue / 10 && nextDigit > 5))
                        {
                            bytesConsumed = 0;
                            value = default(uint);
                            return false;
                        }
                        value = value * 10 + nextDigit;
                    }
                }

                bytesConsumed = text.Length;
                return true;
            }
            
            /// <summary>
            /// These hex parsing APIs are designed to be similar in behavior to the BCL's
            /// uint.TryParse method called with NumberStyles.HexNumber and CultureInfo.InvariantCulture.
            /// They are case-insensitive and do not support the "0x" prefix.
            /// </summary>
            public static partial class Hex
            {
                public unsafe static bool TryParseUInt32(byte* text, int length, out uint value)
                {
                    if (length < 1)
                    {
                        value = default(uint);
                        return false;
                    }

                    // Cache s_hexLookup in order to avoid static constructor checks
                    byte[] hexLookup = s_HexLookup;

                    // Parse the first digit separately. If invalid here, we need to return false.
                    byte firstByte = text[0];
                    byte firstDigit = hexLookup[firstByte];
                    if (firstDigit == 0xFF)
                    {
                        value = default(uint);
                        return false;
                    }
                    value = firstDigit;

                    if (length <= UInt32OverflowLengthHex)
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
                        for (int index = 1; index < UInt32OverflowLengthHex; index++)
                        {
                            byte nextByte = text[index];
                            byte nextDigit = hexLookup[nextByte];
                            if (nextDigit == 0xFF)
                            {
                                return true;
                            }
                            value = value * 0x10 + nextDigit;
                        }
                        for (int index = UInt32OverflowLengthHex; index < length; index++)
                        {
                            byte nextByte = text[index];
                            byte nextDigit = hexLookup[nextByte];
                            if (nextDigit == 0xFF)
                            {
                                return true;
                            }
                            // uint.MaxValue is a constant 0xFFFFFFFF
                            // If we try to append a digit to anything larger than 0x0FFFFFFF, there will be overflow
                            if (value > uint.MaxValue / 0x10)
                            {
                                value = default(uint);
                                return false;
                            }
                            value = value * 0x10 + nextDigit;
                        }
                    }

                    return true;
                }
                public unsafe static bool TryParseUInt32(byte* text, int length, out uint value, out int bytesConsumed)
                {
                    if (length < 1)
                    {
                        bytesConsumed = 0;
                        value = default(uint);
                        return false;
                    }

                    // Cache s_hexLookup in order to avoid static constructor checks
                    byte[] hexLookup = s_HexLookup;

                    // Parse the first digit separately. If invalid here, we need to return false.
                    byte firstByte = text[0];
                    byte firstDigit = hexLookup[firstByte];
                    if (firstDigit == 0xFF)
                    {
                        bytesConsumed = 0;
                        value = default(uint);
                        return false;
                    }
                    value = firstDigit;

                    if (length <= UInt32OverflowLengthHex)
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
                        for (int index = 1; index < UInt32OverflowLengthHex; index++)
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
                        for (int index = UInt32OverflowLengthHex; index < length; index++)
                        {
                            byte nextByte = text[index];
                            byte nextDigit = hexLookup[nextByte];
                            if (nextDigit == 0xFF)
                            {
                                bytesConsumed = index;
                                return true;
                            }
                            // uint.MaxValue is a constant 0xFFFFFFFF
                            // If we try to append a digit to anything larger than 0x0FFFFFFF, there will be overflow
                            if (value > uint.MaxValue / 0x10)
                            {
                                bytesConsumed = 0;
                                value = default(uint);
                                return false;
                            }
                            value = value * 0x10 + nextDigit;
                        }
                    }

                    bytesConsumed = length;
                    return true;
                }
                public static bool TryParseUInt32(ReadOnlySpan<byte> text, out uint value)
                {
                    if (text.Length < 1)
                    {
                        value = default(uint);
                        return false;
                    }

                    // Cache s_hexLookup in order to avoid static constructor checks
                    byte[] hexLookup = s_HexLookup;

                    // Parse the first digit separately. If invalid here, we need to return false.
                    byte firstByte = text[0];
                    byte firstDigit = hexLookup[firstByte];
                    if (firstDigit == 0xFF)
                    {
                        value = default(uint);
                        return false;
                    }
                    value = firstDigit;

                    if (text.Length <= UInt32OverflowLengthHex)
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
                        for (int index = 1; index < UInt32OverflowLengthHex; index++)
                        {
                            byte nextByte = text[index];
                            byte nextDigit = hexLookup[nextByte];
                            if (nextDigit == 0xFF)
                            {
                                return true;
                            }
                            value = value * 0x10 + nextDigit;
                        }
                        for (int index = UInt32OverflowLengthHex; index < text.Length; index++)
                        {
                            byte nextByte = text[index];
                            byte nextDigit = hexLookup[nextByte];
                            if (nextDigit == 0xFF)
                            {
                                return true;
                            }
                            // uint.MaxValue is a constant 0xFFFFFFFF
                            // If we try to append a digit to anything larger than 0x0FFFFFFF, there will be overflow
                            if (value > uint.MaxValue / 0x10)
                            {
                                value = default(uint);
                                return false;
                            }
                            value = value * 0x10 + nextDigit;
                        }
                    }

                    return true;
                }
                public static bool TryParseUInt32(ReadOnlySpan<byte> text, out uint value, out int bytesConsumed)
                {
                    if (text.Length < 1)
                    {
                        bytesConsumed = 0;
                        value = default(uint);
                        return false;
                    }

                    // Cache s_hexLookup in order to avoid static constructor checks
                    byte[] hexLookup = s_HexLookup;

                    // Parse the first digit separately. If invalid here, we need to return false.
                    byte firstByte = text[0];
                    byte firstDigit = hexLookup[firstByte];
                    if (firstDigit == 0xFF)
                    {
                        bytesConsumed = 0;
                        value = default(uint);
                        return false;
                    }
                    value = firstDigit;

                    if (text.Length <= UInt32OverflowLengthHex)
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
                        for (int index = 1; index < UInt32OverflowLengthHex; index++)
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
                        for (int index = UInt32OverflowLengthHex; index < text.Length; index++)
                        {
                            byte nextByte = text[index];
                            byte nextDigit = hexLookup[nextByte];
                            if (nextDigit == 0xFF)
                            {
                                bytesConsumed = index;
                                return true;
                            }
                            // uint.MaxValue is a constant 0xFFFFFFFF
                            // If we try to append a digit to anything larger than 0x0FFFFFFF, there will be overflow
                            if (value > uint.MaxValue / 0x10)
                            {
                                bytesConsumed = 0;
                                value = default(uint);
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