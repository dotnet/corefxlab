// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class PrimitiveParser
    {
        public static partial class InvariantUtf8
        {
            #region static
            private static readonly int UINT_OVERFLOW_LENGTH = 10;
            private static readonly int UINT_OVERFLOW_LENGTH_HEX = 8;

            static readonly int[] hexLookup =
            {
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,             // 15
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,             // 31
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,             // 47
                0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, -1, -1, -1, -1, -1, -1,   // 63
                -1, 0xA, 0xB, 0xC, 0xD, 0xE, 0xF, -1, -1, -1, -1, -1, -1, -1, -1, -1,       // 79
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,             // 95
                -1, 0xa, 0xb, 0xc, 0xd, 0xe, 0xf, -1, -1, -1, -1, -1, -1, -1, -1, -1,       // 111
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,             // 127
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,             // 143
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,             // 159
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,             // 175
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,             // 191
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,             // 207
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,             // 223
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,             // 239
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1              // 255
            };
            #endregion
            
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

                if (length < UINT_OVERFLOW_LENGTH)
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
                    for (int index = 1; index < UINT_OVERFLOW_LENGTH - 1; index++)
                    {
                        uint nextDigit = text[index] - 48u; // '0'
                        if (nextDigit > 9)
                        {
                            return true;
                        }
                        value = value * 10 + nextDigit;
                    }
                    for (int index = UINT_OVERFLOW_LENGTH - 1; index < length; index++)
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

                if (length < UINT_OVERFLOW_LENGTH)
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
                    for (int index = 1; index < UINT_OVERFLOW_LENGTH - 1; index++)
                    {
                        uint nextDigit = text[index] - 48u; // '0'
                        if (nextDigit > 9)
                        {
                            bytesConsumed = index;
                            return true;
                        }
                        value = value * 10 + nextDigit;
                    }
                    for (int index = UINT_OVERFLOW_LENGTH - 1; index < length; index++)
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

                if (text.Length < UINT_OVERFLOW_LENGTH)
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
                    for (int index = 1; index < UINT_OVERFLOW_LENGTH - 1; index++)
                    {
                        uint nextDigit = text[index] - 48u; // '0'
                        if (nextDigit > 9)
                        {
                            return true;
                        }
                        value = value * 10 + nextDigit;
                    }
                    for (int index = UINT_OVERFLOW_LENGTH - 1; index < text.Length; index++)
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

                if (text.Length < UINT_OVERFLOW_LENGTH)
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
                    for (int index = 1; index < UINT_OVERFLOW_LENGTH - 1; index++)
                    {
                        uint nextDigit = text[index] - 48u; // '0'
                        if (nextDigit > 9)
                        {
                            bytesConsumed = index;
                            return true;
                        }
                        value = value * 10 + nextDigit;
                    }
                    for (int index = UINT_OVERFLOW_LENGTH - 1; index < text.Length; index++)
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
            
            public static partial class Hex
            {
                public unsafe static bool TryParseUInt32(byte* text, int length, out uint value)
                {
                    if (length < 1)
                    {
                        value = default(uint);
                        return false;
                    }

                    // Parse the first digit separately. If invalid here, we need to return false.
                    byte firstByte = text[0];
                    int firstDigit = hexLookup[firstByte];
                    if (firstDigit == -1)
                    {
                        value = default(uint);
                        return false;
                    }
                    value = (uint)firstDigit;

                    if (length <= UINT_OVERFLOW_LENGTH_HEX)
                    {
                        // Length is less than or equal to OVERFLOW_LENGTH; overflow is not possible
                        for (int index = 1; index < length; index++)
                        {
                            byte nextByte = text[index];
                            int nextDigit = hexLookup[nextByte];
                            if (nextDigit == -1)
                            {
                                return true;
                            }
                            value = value * 0x10 + (uint)nextDigit;
                        }
                    }
                    else
                    {
                        // Length is greater than OVERFLOW_LENGTH; overflow is only possible after OVERFLOW_LENGTH
                        // digits. There may be no overflow after OVERFLOW_LENGTH if there are leading zeroes.
                        for (int index = 1; index < UINT_OVERFLOW_LENGTH_HEX; index++)
                        {
                            byte nextByte = text[index];
                            int nextDigit = hexLookup[nextByte];
                            if (nextDigit == -1)
                            {
                                return true;
                            }
                            value = value * 0x10 + (uint)nextDigit;
                        }
                        for (int index = UINT_OVERFLOW_LENGTH_HEX; index < length; index++)
                        {
                            byte nextByte = text[index];
                            int nextDigit = hexLookup[nextByte];
                            if (nextDigit == -1)
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
                            value = value * 0x10 + (uint)nextDigit;
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
                    byte firstByte = text[0];
                    int firstDigit = hexLookup[firstByte];
                    if (firstDigit == -1)
                    {
                        bytesConsumed = 0;
                        value = default(uint);
                        return false;
                    }
                    value = (uint)firstDigit;

                    if (length <= UINT_OVERFLOW_LENGTH_HEX)
                    {
                        // Length is less than or equal to OVERFLOW_LENGTH; overflow is not possible
                        for (int index = 1; index < length; index++)
                        {
                            byte nextByte = text[index];
                            int nextDigit = hexLookup[nextByte];
                            if (nextDigit == -1)
                            {
                                bytesConsumed = index;
                                return true;
                            }
                            value = value * 0x10 + (uint)nextDigit;
                        }
                    }
                    else
                    {
                        // Length is greater than OVERFLOW_LENGTH; overflow is only possible after OVERFLOW_LENGTH
                        // digits. There may be no overflow after OVERFLOW_LENGTH if there are leading zeroes.
                        for (int index = 1; index < UINT_OVERFLOW_LENGTH_HEX; index++)
                        {
                            byte nextByte = text[index];
                            int nextDigit = hexLookup[nextByte];
                            if (nextDigit == -1)
                            {
                                bytesConsumed = index;
                                return true;
                            }
                            value = value * 0x10 + (uint)nextDigit;
                        }
                        for (int index = UINT_OVERFLOW_LENGTH_HEX; index < length; index++)
                        {
                            byte nextByte = text[index];
                            int nextDigit = hexLookup[nextByte];
                            if (nextDigit == -1)
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
                            value = value * 0x10 + (uint)nextDigit;
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
                    byte firstByte = text[0];
                    int firstDigit = hexLookup[firstByte];
                    if (firstDigit == -1)
                    {
                        value = default(uint);
                        return false;
                    }
                    value = (uint)firstDigit;

                    if (text.Length <= UINT_OVERFLOW_LENGTH_HEX)
                    {
                        // Length is less than or equal to OVERFLOW_LENGTH; overflow is not possible
                        for (int index = 1; index < text.Length; index++)
                        {
                            byte nextByte = text[index];
                            int nextDigit = hexLookup[nextByte];
                            if (nextDigit == -1)
                            {
                                return true;
                            }
                            value = value * 0x10 + (uint)nextDigit;
                        }
                    }
                    else
                    {
                        // Length is greater than OVERFLOW_LENGTH; overflow is only possible after OVERFLOW_LENGTH
                        // digits. There may be no overflow after OVERFLOW_LENGTH if there are leading zeroes.
                        for (int index = 1; index < UINT_OVERFLOW_LENGTH_HEX; index++)
                        {
                            byte nextByte = text[index];
                            int nextDigit = hexLookup[nextByte];
                            if (nextDigit == -1)
                            {
                                return true;
                            }
                            value = value * 0x10 + (uint)nextDigit;
                        }
                        for (int index = UINT_OVERFLOW_LENGTH_HEX; index < text.Length; index++)
                        {
                            byte nextByte = text[index];
                            int nextDigit = hexLookup[nextByte];
                            if (nextDigit == -1)
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
                            value = value * 0x10 + (uint)nextDigit;
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
                    byte firstByte = text[0];
                    int firstDigit = hexLookup[firstByte];
                    if (firstDigit == -1)
                    {
                        bytesConsumed = 0;
                        value = default(uint);
                        return false;
                    }
                    value = (uint)firstDigit;

                    if (text.Length <= UINT_OVERFLOW_LENGTH_HEX)
                    {
                        // Length is less than or equal to OVERFLOW_LENGTH; overflow is not possible
                        for (int index = 1; index < text.Length; index++)
                        {
                            byte nextByte = text[index];
                            int nextDigit = hexLookup[nextByte];
                            if (nextDigit == -1)
                            {
                                bytesConsumed = index;
                                return true;
                            }
                            value = value * 0x10 + (uint)nextDigit;
                        }
                    }
                    else
                    {
                        // Length is greater than OVERFLOW_LENGTH; overflow is only possible after OVERFLOW_LENGTH
                        // digits. There may be no overflow after OVERFLOW_LENGTH if there are leading zeroes.
                        for (int index = 1; index < UINT_OVERFLOW_LENGTH_HEX; index++)
                        {
                            byte nextByte = text[index];
                            int nextDigit = hexLookup[nextByte];
                            if (nextDigit == -1)
                            {
                                bytesConsumed = index;
                                return true;
                            }
                            value = value * 0x10 + (uint)nextDigit;
                        }
                        for (int index = UINT_OVERFLOW_LENGTH_HEX; index < text.Length; index++)
                        {
                            byte nextByte = text[index];
                            int nextDigit = hexLookup[nextByte];
                            if (nextDigit == -1)
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
                            value = value * 0x10 + (uint)nextDigit;
                        }
                    }

                    bytesConsumed = text.Length;
                    return true;
                }
            }
        }
    }
}