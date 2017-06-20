// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text
{
    public static partial class PrimitiveParser
    {
        public static partial class InvariantUtf8
        {
            #region SByte
            public unsafe static bool TryParseSByte(byte* text, int length, out sbyte value)
            {
                if (length < 1)
                {
                    value = default(sbyte);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = SByteOverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                int firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    value = default(sbyte);
                    return false;
                }
                int parsedValue = firstDigit;

                if (length < overflowLength)
                {
                    // Length is less than SByteOverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = (sbyte)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than SByteOverflowLength; overflow is only possible after SByteOverflowLength
                    // digits. There may be no overflow after SByteOverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = (sbyte)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = (sbyte)(parsedValue * sign);
                            return true;
                        }
                        // If parsedValue > (sbyte.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (sbyte.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > sbyte.MaxValue / 10 || parsedValue == sbyte.MaxValue / 10 && nextDigitTooLarge)
                        {
                            value = default(sbyte);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                value = (sbyte)(parsedValue * sign);
                return true;
            }

            public unsafe static bool TryParseSByte(byte* text, int length, out sbyte value, out int bytesConsumed)
            {
                if (length < 1)
                {
                    bytesConsumed = 0;
                    value = default(sbyte);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = SByteOverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                int firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    bytesConsumed = 0;
                    value = default(sbyte);
                    return false;
                }
                int parsedValue = firstDigit;

                if (length < overflowLength)
                {
                    // Length is less than SByteOverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            bytesConsumed = index;
                            value = (sbyte)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than SByteOverflowLength; overflow is only possible after SByteOverflowLength
                    // digits. There may be no overflow after SByteOverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            bytesConsumed = index;
                            value = (sbyte)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            bytesConsumed = index;
                            value = (sbyte)(parsedValue * sign);
                            return true;
                        }
                        // If parsedValue > (sbyte.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (sbyte.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > sbyte.MaxValue / 10 || parsedValue == sbyte.MaxValue / 10 && nextDigitTooLarge)
                        {
                            bytesConsumed = 0;
                            value = default(sbyte);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                bytesConsumed = length;
                value = (sbyte)(parsedValue * sign);
                return true;
            }

            public static bool TryParseSByte(ReadOnlySpan<byte> text, out sbyte value)
            {
                if (text.Length < 1)
                {
                    value = default(sbyte);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = SByteOverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                int firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    value = default(sbyte);
                    return false;
                }
                int parsedValue = firstDigit;

                if (text.Length < overflowLength)
                {
                    // Length is less than SByteOverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < text.Length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = (sbyte)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than SByteOverflowLength; overflow is only possible after SByteOverflowLength
                    // digits. There may be no overflow after SByteOverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = (sbyte)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < text.Length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = (sbyte)(parsedValue * sign);
                            return true;
                        }
                        // If parsedValue > (sbyte.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (sbyte.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > sbyte.MaxValue / 10 || parsedValue == sbyte.MaxValue / 10 && nextDigitTooLarge)
                        {
                            value = default(sbyte);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                value = (sbyte)(parsedValue * sign);
                return true;
            }

            public static bool TryParseSByte(ReadOnlySpan<byte> text, out sbyte value, out int bytesConsumed)
            {
                if (text.Length < 1)
                {
                    bytesConsumed = 0;
                    value = default(sbyte);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = SByteOverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                int firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    bytesConsumed = 0;
                    value = default(sbyte);
                    return false;
                }
                int parsedValue = firstDigit;

                if (text.Length < overflowLength)
                {
                    // Length is less than SByteOverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < text.Length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            bytesConsumed = index;
                            value = (sbyte)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than SByteOverflowLength; overflow is only possible after SByteOverflowLength
                    // digits. There may be no overflow after SByteOverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            bytesConsumed = index;
                            value = (sbyte)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < text.Length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            bytesConsumed = index;
                            value = (sbyte)(parsedValue * sign);
                            return true;
                        }
                        // If parsedValue > (sbyte.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (sbyte.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > sbyte.MaxValue / 10 || parsedValue == sbyte.MaxValue / 10 && nextDigitTooLarge)
                        {
                            bytesConsumed = 0;
                            value = default(sbyte);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                bytesConsumed = text.Length;
                value = (sbyte)(parsedValue * sign);
                return true;
            }

            #endregion

            #region Int16
            public unsafe static bool TryParseInt16(byte* text, int length, out short value)
            {
                if (length < 1)
                {
                    value = default(short);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = Int16OverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                int firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    value = default(short);
                    return false;
                }
                int parsedValue = firstDigit;

                if (length < overflowLength)
                {
                    // Length is less than Int16OverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = (short)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Int16OverflowLength; overflow is only possible after Int16OverflowLength
                    // digits. There may be no overflow after Int16OverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = (short)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = (short)(parsedValue * sign);
                            return true;
                        }
                        // If parsedValue > (short.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (short.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > short.MaxValue / 10 || parsedValue == short.MaxValue / 10 && nextDigitTooLarge)
                        {
                            value = default(short);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                value = (short)(parsedValue * sign);
                return true;
            }

            public unsafe static bool TryParseInt16(byte* text, int length, out short value, out int bytesConsumed)
            {
                if (length < 1)
                {
                    bytesConsumed = 0;
                    value = default(short);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = Int16OverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                int firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    bytesConsumed = 0;
                    value = default(short);
                    return false;
                }
                int parsedValue = firstDigit;

                if (length < overflowLength)
                {
                    // Length is less than Int16OverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            bytesConsumed = index;
                            value = (short)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Int16OverflowLength; overflow is only possible after Int16OverflowLength
                    // digits. There may be no overflow after Int16OverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            bytesConsumed = index;
                            value = (short)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            bytesConsumed = index;
                            value = (short)(parsedValue * sign);
                            return true;
                        }
                        // If parsedValue > (short.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (short.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > short.MaxValue / 10 || parsedValue == short.MaxValue / 10 && nextDigitTooLarge)
                        {
                            bytesConsumed = 0;
                            value = default(short);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                bytesConsumed = length;
                value = (short)(parsedValue * sign);
                return true;
            }

            public static bool TryParseInt16(ReadOnlySpan<byte> text, out short value)
            {
                if (text.Length < 1)
                {
                    value = default(short);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = Int16OverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                int firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    value = default(short);
                    return false;
                }
                int parsedValue = firstDigit;

                if (text.Length < overflowLength)
                {
                    // Length is less than Int16OverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < text.Length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = (short)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Int16OverflowLength; overflow is only possible after Int16OverflowLength
                    // digits. There may be no overflow after Int16OverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = (short)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < text.Length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = (short)(parsedValue * sign);
                            return true;
                        }
                        // If parsedValue > (short.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (short.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > short.MaxValue / 10 || parsedValue == short.MaxValue / 10 && nextDigitTooLarge)
                        {
                            value = default(short);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                value = (short)(parsedValue * sign);
                return true;
            }

            public static bool TryParseInt16(ReadOnlySpan<byte> text, out short value, out int bytesConsumed)
            {
                if (text.Length < 1)
                {
                    bytesConsumed = 0;
                    value = default(short);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = Int16OverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                int firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    bytesConsumed = 0;
                    value = default(short);
                    return false;
                }
                int parsedValue = firstDigit;

                if (text.Length < overflowLength)
                {
                    // Length is less than Int16OverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < text.Length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            bytesConsumed = index;
                            value = (short)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Int16OverflowLength; overflow is only possible after Int16OverflowLength
                    // digits. There may be no overflow after Int16OverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            bytesConsumed = index;
                            value = (short)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < text.Length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            bytesConsumed = index;
                            value = (short)(parsedValue * sign);
                            return true;
                        }
                        // If parsedValue > (short.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (short.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > short.MaxValue / 10 || parsedValue == short.MaxValue / 10 && nextDigitTooLarge)
                        {
                            bytesConsumed = 0;
                            value = default(short);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                bytesConsumed = text.Length;
                value = (short)(parsedValue * sign);
                return true;
            }

            #endregion

            #region Int32
            public unsafe static bool TryParseInt32(byte* text, int length, out int value)
            {
                if (length < 1)
                {
                    value = default(int);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = Int32OverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                int firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    value = default(int);
                    return false;
                }
                int parsedValue = firstDigit;

                if (length < overflowLength)
                {
                    // Length is less than Int32OverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Int32OverflowLength; overflow is only possible after Int32OverflowLength
                    // digits. There may be no overflow after Int32OverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = parsedValue * sign;
                            return true;
                        }
                        // If parsedValue > (int.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (int.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > int.MaxValue / 10 || parsedValue == int.MaxValue / 10 && nextDigitTooLarge)
                        {
                            value = default(int);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                value = parsedValue * sign;
                return true;
            }

            public unsafe static bool TryParseInt32(byte* text, int length, out int value, out int bytesConsumed)
            {
                if (length < 1)
                {
                    bytesConsumed = 0;
                    value = default(int);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = Int32OverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                int firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    bytesConsumed = 0;
                    value = default(int);
                    return false;
                }
                int parsedValue = firstDigit;

                if (length < overflowLength)
                {
                    // Length is less than Int32OverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            bytesConsumed = index;
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Int32OverflowLength; overflow is only possible after Int32OverflowLength
                    // digits. There may be no overflow after Int32OverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            bytesConsumed = index;
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            bytesConsumed = index;
                            value = parsedValue * sign;
                            return true;
                        }
                        // If parsedValue > (int.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (int.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > int.MaxValue / 10 || parsedValue == int.MaxValue / 10 && nextDigitTooLarge)
                        {
                            bytesConsumed = 0;
                            value = default(int);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                bytesConsumed = length;
                value = parsedValue * sign;
                return true;
            }

            public static bool TryParseInt32(ReadOnlySpan<byte> text, out int value)
            {
                if (text.Length < 1)
                {
                    value = default(int);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = Int32OverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                int firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    value = default(int);
                    return false;
                }
                int parsedValue = firstDigit;

                if (text.Length < overflowLength)
                {
                    // Length is less than Int32OverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < text.Length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Int32OverflowLength; overflow is only possible after Int32OverflowLength
                    // digits. There may be no overflow after Int32OverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < text.Length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = parsedValue * sign;
                            return true;
                        }
                        // If parsedValue > (int.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (int.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > int.MaxValue / 10 || parsedValue == int.MaxValue / 10 && nextDigitTooLarge)
                        {
                            value = default(int);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                value = parsedValue * sign;
                return true;
            }

            public static bool TryParseInt32(ReadOnlySpan<byte> text, out int value, out int bytesConsumed)
            {
                if (text.Length < 1)
                {
                    bytesConsumed = 0;
                    value = default(int);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = Int32OverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                int firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    bytesConsumed = 0;
                    value = default(int);
                    return false;
                }
                int parsedValue = firstDigit;

                if (text.Length < overflowLength)
                {
                    // Length is less than Int32OverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < text.Length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            bytesConsumed = index;
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Int32OverflowLength; overflow is only possible after Int32OverflowLength
                    // digits. There may be no overflow after Int32OverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            bytesConsumed = index;
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < text.Length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            bytesConsumed = index;
                            value = parsedValue * sign;
                            return true;
                        }
                        // If parsedValue > (int.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (int.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > int.MaxValue / 10 || parsedValue == int.MaxValue / 10 && nextDigitTooLarge)
                        {
                            bytesConsumed = 0;
                            value = default(int);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                bytesConsumed = text.Length;
                value = parsedValue * sign;
                return true;
            }

            #endregion

            #region Int64
            public unsafe static bool TryParseInt64(byte* text, int length, out long value)
            {
                if (length < 1)
                {
                    value = default(long);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = Int64OverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                long firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    value = default(long);
                    return false;
                }
                long parsedValue = firstDigit;

                if (length < overflowLength)
                {
                    // Length is less than Int64OverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < length; index++)
                    {
                        long nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Int64OverflowLength; overflow is only possible after Int64OverflowLength
                    // digits. There may be no overflow after Int64OverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        long nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < length; index++)
                    {
                        long nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = parsedValue * sign;
                            return true;
                        }
                        // If parsedValue > (long.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (long.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > long.MaxValue / 10 || parsedValue == long.MaxValue / 10 && nextDigitTooLarge)
                        {
                            value = default(long);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                value = parsedValue * sign;
                return true;
            }

            public unsafe static bool TryParseInt64(byte* text, int length, out long value, out int bytesConsumed)
            {
                if (length < 1)
                {
                    bytesConsumed = 0;
                    value = default(long);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = Int64OverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                long firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    bytesConsumed = 0;
                    value = default(long);
                    return false;
                }
                long parsedValue = firstDigit;

                if (length < overflowLength)
                {
                    // Length is less than Int64OverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < length; index++)
                    {
                        long nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            bytesConsumed = index;
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Int64OverflowLength; overflow is only possible after Int64OverflowLength
                    // digits. There may be no overflow after Int64OverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        long nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            bytesConsumed = index;
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < length; index++)
                    {
                        long nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            bytesConsumed = index;
                            value = parsedValue * sign;
                            return true;
                        }
                        // If parsedValue > (long.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (long.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > long.MaxValue / 10 || parsedValue == long.MaxValue / 10 && nextDigitTooLarge)
                        {
                            bytesConsumed = 0;
                            value = default(long);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                bytesConsumed = length;
                value = parsedValue * sign;
                return true;
            }

            public static bool TryParseInt64(ReadOnlySpan<byte> text, out long value)
            {
                if (text.Length < 1)
                {
                    value = default(long);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = Int64OverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                long firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    value = default(long);
                    return false;
                }
                long parsedValue = firstDigit;

                if (text.Length < overflowLength)
                {
                    // Length is less than Int64OverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < text.Length; index++)
                    {
                        long nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Int64OverflowLength; overflow is only possible after Int64OverflowLength
                    // digits. There may be no overflow after Int64OverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        long nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < text.Length; index++)
                    {
                        long nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = parsedValue * sign;
                            return true;
                        }
                        // If parsedValue > (long.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (long.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > long.MaxValue / 10 || parsedValue == long.MaxValue / 10 && nextDigitTooLarge)
                        {
                            value = default(long);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                value = parsedValue * sign;
                return true;
            }

            public static bool TryParseInt64(ReadOnlySpan<byte> text, out long value, out int bytesConsumed)
            {
                if (text.Length < 1)
                {
                    bytesConsumed = 0;
                    value = default(long);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = Int64OverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                long firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    bytesConsumed = 0;
                    value = default(long);
                    return false;
                }
                long parsedValue = firstDigit;

                if (text.Length < overflowLength)
                {
                    // Length is less than Int64OverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < text.Length; index++)
                    {
                        long nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            bytesConsumed = index;
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Int64OverflowLength; overflow is only possible after Int64OverflowLength
                    // digits. There may be no overflow after Int64OverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        long nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            bytesConsumed = index;
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < text.Length; index++)
                    {
                        long nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            bytesConsumed = index;
                            value = parsedValue * sign;
                            return true;
                        }
                        // If parsedValue > (long.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (long.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > long.MaxValue / 10 || parsedValue == long.MaxValue / 10 && nextDigitTooLarge)
                        {
                            bytesConsumed = 0;
                            value = default(long);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                bytesConsumed = text.Length;
                value = parsedValue * sign;
                return true;
            }

            #endregion

        }
        public static partial class InvariantUtf16
        {
            #region SByte
            public unsafe static bool TryParseSByte(char* text, int length, out sbyte value)
            {
                if (length < 1)
                {
                    value = default(sbyte);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = SByteOverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                int firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    value = default(sbyte);
                    return false;
                }
                int parsedValue = firstDigit;

                if (length < overflowLength)
                {
                    // Length is less than SByteOverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = (sbyte)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than SByteOverflowLength; overflow is only possible after SByteOverflowLength
                    // digits. There may be no overflow after SByteOverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = (sbyte)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = (sbyte)(parsedValue * sign);
                            return true;
                        }
                        // If parsedValue > (sbyte.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (sbyte.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > sbyte.MaxValue / 10 || parsedValue == sbyte.MaxValue / 10 && nextDigitTooLarge)
                        {
                            value = default(sbyte);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                value = (sbyte)(parsedValue * sign);
                return true;
            }

            public unsafe static bool TryParseSByte(char* text, int length, out sbyte value, out int charsConsumed)
            {
                if (length < 1)
                {
                    charsConsumed = 0;
                    value = default(sbyte);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = SByteOverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                int firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    charsConsumed = 0;
                    value = default(sbyte);
                    return false;
                }
                int parsedValue = firstDigit;

                if (length < overflowLength)
                {
                    // Length is less than SByteOverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            charsConsumed = index;
                            value = (sbyte)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than SByteOverflowLength; overflow is only possible after SByteOverflowLength
                    // digits. There may be no overflow after SByteOverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            charsConsumed = index;
                            value = (sbyte)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            charsConsumed = index;
                            value = (sbyte)(parsedValue * sign);
                            return true;
                        }
                        // If parsedValue > (sbyte.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (sbyte.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > sbyte.MaxValue / 10 || parsedValue == sbyte.MaxValue / 10 && nextDigitTooLarge)
                        {
                            charsConsumed = 0;
                            value = default(sbyte);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                charsConsumed = length;
                value = (sbyte)(parsedValue * sign);
                return true;
            }

            public static bool TryParseSByte(ReadOnlySpan<char> text, out sbyte value)
            {
                if (text.Length < 1)
                {
                    value = default(sbyte);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = SByteOverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                int firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    value = default(sbyte);
                    return false;
                }
                int parsedValue = firstDigit;

                if (text.Length < overflowLength)
                {
                    // Length is less than SByteOverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < text.Length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = (sbyte)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than SByteOverflowLength; overflow is only possible after SByteOverflowLength
                    // digits. There may be no overflow after SByteOverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = (sbyte)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < text.Length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = (sbyte)(parsedValue * sign);
                            return true;
                        }
                        // If parsedValue > (sbyte.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (sbyte.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > sbyte.MaxValue / 10 || parsedValue == sbyte.MaxValue / 10 && nextDigitTooLarge)
                        {
                            value = default(sbyte);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                value = (sbyte)(parsedValue * sign);
                return true;
            }

            public static bool TryParseSByte(ReadOnlySpan<char> text, out sbyte value, out int charsConsumed)
            {
                if (text.Length < 1)
                {
                    charsConsumed = 0;
                    value = default(sbyte);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = SByteOverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                int firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    charsConsumed = 0;
                    value = default(sbyte);
                    return false;
                }
                int parsedValue = firstDigit;

                if (text.Length < overflowLength)
                {
                    // Length is less than SByteOverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < text.Length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            charsConsumed = index;
                            value = (sbyte)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than SByteOverflowLength; overflow is only possible after SByteOverflowLength
                    // digits. There may be no overflow after SByteOverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            charsConsumed = index;
                            value = (sbyte)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < text.Length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            charsConsumed = index;
                            value = (sbyte)(parsedValue * sign);
                            return true;
                        }
                        // If parsedValue > (sbyte.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (sbyte.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > sbyte.MaxValue / 10 || parsedValue == sbyte.MaxValue / 10 && nextDigitTooLarge)
                        {
                            charsConsumed = 0;
                            value = default(sbyte);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                charsConsumed = text.Length;
                value = (sbyte)(parsedValue * sign);
                return true;
            }

            #endregion

            #region Int16
            public unsafe static bool TryParseInt16(char* text, int length, out short value)
            {
                if (length < 1)
                {
                    value = default(short);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = Int16OverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                int firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    value = default(short);
                    return false;
                }
                int parsedValue = firstDigit;

                if (length < overflowLength)
                {
                    // Length is less than Int16OverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = (short)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Int16OverflowLength; overflow is only possible after Int16OverflowLength
                    // digits. There may be no overflow after Int16OverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = (short)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = (short)(parsedValue * sign);
                            return true;
                        }
                        // If parsedValue > (short.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (short.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > short.MaxValue / 10 || parsedValue == short.MaxValue / 10 && nextDigitTooLarge)
                        {
                            value = default(short);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                value = (short)(parsedValue * sign);
                return true;
            }

            public unsafe static bool TryParseInt16(char* text, int length, out short value, out int charsConsumed)
            {
                if (length < 1)
                {
                    charsConsumed = 0;
                    value = default(short);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = Int16OverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                int firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    charsConsumed = 0;
                    value = default(short);
                    return false;
                }
                int parsedValue = firstDigit;

                if (length < overflowLength)
                {
                    // Length is less than Int16OverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            charsConsumed = index;
                            value = (short)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Int16OverflowLength; overflow is only possible after Int16OverflowLength
                    // digits. There may be no overflow after Int16OverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            charsConsumed = index;
                            value = (short)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            charsConsumed = index;
                            value = (short)(parsedValue * sign);
                            return true;
                        }
                        // If parsedValue > (short.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (short.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > short.MaxValue / 10 || parsedValue == short.MaxValue / 10 && nextDigitTooLarge)
                        {
                            charsConsumed = 0;
                            value = default(short);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                charsConsumed = length;
                value = (short)(parsedValue * sign);
                return true;
            }

            public static bool TryParseInt16(ReadOnlySpan<char> text, out short value)
            {
                if (text.Length < 1)
                {
                    value = default(short);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = Int16OverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                int firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    value = default(short);
                    return false;
                }
                int parsedValue = firstDigit;

                if (text.Length < overflowLength)
                {
                    // Length is less than Int16OverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < text.Length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = (short)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Int16OverflowLength; overflow is only possible after Int16OverflowLength
                    // digits. There may be no overflow after Int16OverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = (short)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < text.Length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = (short)(parsedValue * sign);
                            return true;
                        }
                        // If parsedValue > (short.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (short.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > short.MaxValue / 10 || parsedValue == short.MaxValue / 10 && nextDigitTooLarge)
                        {
                            value = default(short);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                value = (short)(parsedValue * sign);
                return true;
            }

            public static bool TryParseInt16(ReadOnlySpan<char> text, out short value, out int charsConsumed)
            {
                if (text.Length < 1)
                {
                    charsConsumed = 0;
                    value = default(short);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = Int16OverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                int firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    charsConsumed = 0;
                    value = default(short);
                    return false;
                }
                int parsedValue = firstDigit;

                if (text.Length < overflowLength)
                {
                    // Length is less than Int16OverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < text.Length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            charsConsumed = index;
                            value = (short)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Int16OverflowLength; overflow is only possible after Int16OverflowLength
                    // digits. There may be no overflow after Int16OverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            charsConsumed = index;
                            value = (short)(parsedValue * sign);
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < text.Length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            charsConsumed = index;
                            value = (short)(parsedValue * sign);
                            return true;
                        }
                        // If parsedValue > (short.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (short.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > short.MaxValue / 10 || parsedValue == short.MaxValue / 10 && nextDigitTooLarge)
                        {
                            charsConsumed = 0;
                            value = default(short);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                charsConsumed = text.Length;
                value = (short)(parsedValue * sign);
                return true;
            }

            #endregion

            #region Int32
            public unsafe static bool TryParseInt32(char* text, int length, out int value)
            {
                if (length < 1)
                {
                    value = default(int);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = Int32OverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                int firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    value = default(int);
                    return false;
                }
                int parsedValue = firstDigit;

                if (length < overflowLength)
                {
                    // Length is less than Int32OverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Int32OverflowLength; overflow is only possible after Int32OverflowLength
                    // digits. There may be no overflow after Int32OverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = parsedValue * sign;
                            return true;
                        }
                        // If parsedValue > (int.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (int.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > int.MaxValue / 10 || parsedValue == int.MaxValue / 10 && nextDigitTooLarge)
                        {
                            value = default(int);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                value = parsedValue * sign;
                return true;
            }

            public unsafe static bool TryParseInt32(char* text, int length, out int value, out int charsConsumed)
            {
                if (length < 1)
                {
                    charsConsumed = 0;
                    value = default(int);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = Int32OverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                int firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    charsConsumed = 0;
                    value = default(int);
                    return false;
                }
                int parsedValue = firstDigit;

                if (length < overflowLength)
                {
                    // Length is less than Int32OverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            charsConsumed = index;
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Int32OverflowLength; overflow is only possible after Int32OverflowLength
                    // digits. There may be no overflow after Int32OverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            charsConsumed = index;
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            charsConsumed = index;
                            value = parsedValue * sign;
                            return true;
                        }
                        // If parsedValue > (int.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (int.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > int.MaxValue / 10 || parsedValue == int.MaxValue / 10 && nextDigitTooLarge)
                        {
                            charsConsumed = 0;
                            value = default(int);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                charsConsumed = length;
                value = parsedValue * sign;
                return true;
            }

            public static bool TryParseInt32(ReadOnlySpan<char> text, out int value)
            {
                if (text.Length < 1)
                {
                    value = default(int);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = Int32OverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                int firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    value = default(int);
                    return false;
                }
                int parsedValue = firstDigit;

                if (text.Length < overflowLength)
                {
                    // Length is less than Int32OverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < text.Length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Int32OverflowLength; overflow is only possible after Int32OverflowLength
                    // digits. There may be no overflow after Int32OverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < text.Length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = parsedValue * sign;
                            return true;
                        }
                        // If parsedValue > (int.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (int.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > int.MaxValue / 10 || parsedValue == int.MaxValue / 10 && nextDigitTooLarge)
                        {
                            value = default(int);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                value = parsedValue * sign;
                return true;
            }

            public static bool TryParseInt32(ReadOnlySpan<char> text, out int value, out int charsConsumed)
            {
                if (text.Length < 1)
                {
                    charsConsumed = 0;
                    value = default(int);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = Int32OverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                int firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    charsConsumed = 0;
                    value = default(int);
                    return false;
                }
                int parsedValue = firstDigit;

                if (text.Length < overflowLength)
                {
                    // Length is less than Int32OverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < text.Length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            charsConsumed = index;
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Int32OverflowLength; overflow is only possible after Int32OverflowLength
                    // digits. There may be no overflow after Int32OverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            charsConsumed = index;
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < text.Length; index++)
                    {
                        int nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            charsConsumed = index;
                            value = parsedValue * sign;
                            return true;
                        }
                        // If parsedValue > (int.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (int.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > int.MaxValue / 10 || parsedValue == int.MaxValue / 10 && nextDigitTooLarge)
                        {
                            charsConsumed = 0;
                            value = default(int);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                charsConsumed = text.Length;
                value = parsedValue * sign;
                return true;
            }

            #endregion

            #region Int64
            public unsafe static bool TryParseInt64(char* text, int length, out long value)
            {
                if (length < 1)
                {
                    value = default(long);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = Int64OverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                long firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    value = default(long);
                    return false;
                }
                long parsedValue = firstDigit;

                if (length < overflowLength)
                {
                    // Length is less than Int64OverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < length; index++)
                    {
                        long nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Int64OverflowLength; overflow is only possible after Int64OverflowLength
                    // digits. There may be no overflow after Int64OverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        long nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < length; index++)
                    {
                        long nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = parsedValue * sign;
                            return true;
                        }
                        // If parsedValue > (long.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (long.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > long.MaxValue / 10 || parsedValue == long.MaxValue / 10 && nextDigitTooLarge)
                        {
                            value = default(long);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                value = parsedValue * sign;
                return true;
            }

            public unsafe static bool TryParseInt64(char* text, int length, out long value, out int charsConsumed)
            {
                if (length < 1)
                {
                    charsConsumed = 0;
                    value = default(long);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = Int64OverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                long firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    charsConsumed = 0;
                    value = default(long);
                    return false;
                }
                long parsedValue = firstDigit;

                if (length < overflowLength)
                {
                    // Length is less than Int64OverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < length; index++)
                    {
                        long nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            charsConsumed = index;
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Int64OverflowLength; overflow is only possible after Int64OverflowLength
                    // digits. There may be no overflow after Int64OverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        long nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            charsConsumed = index;
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < length; index++)
                    {
                        long nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            charsConsumed = index;
                            value = parsedValue * sign;
                            return true;
                        }
                        // If parsedValue > (long.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (long.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > long.MaxValue / 10 || parsedValue == long.MaxValue / 10 && nextDigitTooLarge)
                        {
                            charsConsumed = 0;
                            value = default(long);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                charsConsumed = length;
                value = parsedValue * sign;
                return true;
            }

            public static bool TryParseInt64(ReadOnlySpan<char> text, out long value)
            {
                if (text.Length < 1)
                {
                    value = default(long);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = Int64OverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                long firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    value = default(long);
                    return false;
                }
                long parsedValue = firstDigit;

                if (text.Length < overflowLength)
                {
                    // Length is less than Int64OverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < text.Length; index++)
                    {
                        long nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Int64OverflowLength; overflow is only possible after Int64OverflowLength
                    // digits. There may be no overflow after Int64OverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        long nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < text.Length; index++)
                    {
                        long nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            value = parsedValue * sign;
                            return true;
                        }
                        // If parsedValue > (long.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (long.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > long.MaxValue / 10 || parsedValue == long.MaxValue / 10 && nextDigitTooLarge)
                        {
                            value = default(long);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                value = parsedValue * sign;
                return true;
            }

            public static bool TryParseInt64(ReadOnlySpan<char> text, out long value, out int charsConsumed)
            {
                if (text.Length < 1)
                {
                    charsConsumed = 0;
                    value = default(long);
                    return false;
                }

                int indexOfFirstDigit = 0;
                int sign = 1;
                if (text[0] == '-')
                {
                    indexOfFirstDigit = 1;
                    sign = -1;
                }
                else if (text[0] == '+')
                {
                    indexOfFirstDigit = 1;
                }

                int overflowLength = Int64OverflowLength + indexOfFirstDigit;

                // Parse the first digit separately. If invalid here, we need to return false.
                long firstDigit = text[indexOfFirstDigit] - 48; // '0'
                if (firstDigit < 0 || firstDigit > 9)
                {
                    charsConsumed = 0;
                    value = default(long);
                    return false;
                }
                long parsedValue = firstDigit;

                if (text.Length < overflowLength)
                {
                    // Length is less than Int64OverflowLength; overflow is not possible
                    for (int index = indexOfFirstDigit + 1; index < text.Length; index++)
                    {
                        long nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            charsConsumed = index;
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Int64OverflowLength; overflow is only possible after Int64OverflowLength
                    // digits. There may be no overflow after Int64OverflowLength if there are leading zeroes.
                    for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                    {
                        long nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            charsConsumed = index;
                            value = parsedValue * sign;
                            return true;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                    for (int index = overflowLength - 1; index < text.Length; index++)
                    {
                        long nextDigit = text[index] - 48; // '0'
                        if (nextDigit < 0 || nextDigit > 9)
                        {
                            charsConsumed = index;
                            value = parsedValue * sign;
                            return true;
                        }
                        // If parsedValue > (long.MaxValue / 10), any more appended digits will cause overflow.
                        // if parsedValue == (long.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                        bool positive = sign > 0;
                        bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                        if (parsedValue > long.MaxValue / 10 || parsedValue == long.MaxValue / 10 && nextDigitTooLarge)
                        {
                            charsConsumed = 0;
                            value = default(long);
                            return false;
                        }
                        parsedValue = parsedValue * 10 + nextDigit;
                    }
                }

                charsConsumed = text.Length;
                value = parsedValue * sign;
                return true;
            }

            #endregion

        }
    }
}
