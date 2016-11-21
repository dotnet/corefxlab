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
            static int UINT_OVERFLOW_LENGTH = 10;
            
            public unsafe static bool TryParseUInt32(byte* text, int length, out uint value)
            {
                value = default(uint);
                if (length < 1)
                {
                    return false;
                }

                // Parse the first digit separately. If invalid here, we need to return false.
                uint firstDigit = text[0] - 48u; // '0'
                if (firstDigit > 9)
                {
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
                value = default(uint);
                if (length < 1)
                {
                    bytesConsumed = 0;
                    return false;
                }

                // Parse the first digit separately. If invalid here, we need to return false.
                uint firstDigit = text[0] - 48u; // '0'
                if (firstDigit > 9)
                {
                    bytesConsumed = 0;
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
                int consumed;
                return PrimitiveParser.TryParseUInt32(text, out value, out consumed, EncodingData.InvariantUtf8);
            }
            public static bool TryParseUInt32(ReadOnlySpan<byte> text, out uint value, out int bytesConsumed)
            {
                return PrimitiveParser.TryParseUInt32(text, out value, out bytesConsumed, EncodingData.InvariantUtf8);
            }

            public static partial class Hex
            {
                public unsafe static bool TryParseUInt32(byte* text, int length, out uint value)
                {
                    int consumed;
                    var span = new ReadOnlySpan<byte>(text, length);
                    return PrimitiveParser.TryParseUInt32(span, out value, out consumed, EncodingData.InvariantUtf8, 'X');
                }
                public unsafe static bool TryParseUInt32(byte* text, int length, out uint value, out int bytesConsumed)
                {
                    var span = new ReadOnlySpan<byte>(text, length);
                    return PrimitiveParser.TryParseUInt32(span, out value, out bytesConsumed, EncodingData.InvariantUtf8, 'X');
                }
                public static bool TryParseUInt32(ReadOnlySpan<byte> text, out uint value)
                {
                    int consumed;
                    return PrimitiveParser.TryParseUInt32(text, out value, out consumed, EncodingData.InvariantUtf8, 'X');
                }
                public static bool TryParseUInt32(ReadOnlySpan<byte> text, out uint value, out int bytesConsumed)
                {
                    return PrimitiveParser.TryParseUInt32(text, out value, out bytesConsumed, EncodingData.InvariantUtf8, 'X');
                }
            }
        }
    }
}