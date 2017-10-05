// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers.Text
{
    public static partial class Utf8Parser
    {
        #region SByte
        unsafe static bool TryParseSByte(byte* text, int length, out sbyte value)
        {
            if (length < 1)
            {
                value = default;
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

            int overflowLength = Parsers.ByteOverflowLength + indexOfFirstDigit;

            // Parse the first digit separately. If invalid here, we need to return false.
            int firstDigit = text[indexOfFirstDigit] - 48; // '0'
            if (firstDigit < 0 || firstDigit > 9)
            {
                value = default;
                return false;
            }
            int parsedValue = firstDigit;

            if (length < overflowLength)
            {
                // Length is less than SParsers.ByteOverflowLength; overflow is not possible
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
                // Length is greater than SParsers.ByteOverflowLength; overflow is only possible after SParsers.ByteOverflowLength
                // digits. There may be no overflow after SParsers.ByteOverflowLength if there are leading zeroes.
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
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            value = (sbyte)(parsedValue * sign);
            return true;
        }

        unsafe static bool TryParseSByte(byte* text, int length, out sbyte value, out int bytesConsumed)
        {
            if (length < 1)
            {
                bytesConsumed = 0;
                value = default;
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

            int overflowLength = Parsers.ByteOverflowLength + indexOfFirstDigit;

            // Parse the first digit separately. If invalid here, we need to return false.
            int firstDigit = text[indexOfFirstDigit] - 48; // '0'
            if (firstDigit < 0 || firstDigit > 9)
            {
                bytesConsumed = 0;
                value = default;
                return false;
            }
            int parsedValue = firstDigit;

            if (length < overflowLength)
            {
                // Length is less than SParsers.ByteOverflowLength; overflow is not possible
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
                // Length is greater than SParsers.ByteOverflowLength; overflow is only possible after SParsers.ByteOverflowLength
                // digits. There may be no overflow after SParsers.ByteOverflowLength if there are leading zeroes.
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
                        value = default;
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
            return TryParseSByte(text, out value, out int bytesConsumed);
        }

        public static bool TryParseSByte(ReadOnlySpan<byte> text, out sbyte value, out int bytesConsumed)
        {
            if (text.Length < 1) goto FalseExit;

            int sign = 1;
            int index = 0;
            int num = text[index];
            if (num == '-')
            {
                sign = -1;
                index++;
                if ((uint)index >= (uint)text.Length) goto FalseExit;
                num = text[index];
            }
            else if (num == '+')
            {
                index++;
                if ((uint)index >= (uint)text.Length) goto FalseExit;
                num = text[index];
            }

            int answer = 0;

            if (Parsers.IsDigit(num))
            {
                if (num == '0')
                {
                    do
                    {
                        index++;
                        if ((uint)index >= (uint)text.Length) goto Done;
                        num = text[index];
                    } while (num == '0');
                    if (!Parsers.IsDigit(num)) goto Done;
                }

                answer = num - '0';
                index++;

                if ((uint)index >= (uint)text.Length) goto Done;
                num = text[index];
                if (!Parsers.IsDigit(num)) goto Done;
                index++;
                answer = 10 * answer + num - '0';

                // Potential overflow
                if ((uint)index >= (uint)text.Length) goto Done;
                num = text[index];
                if (!Parsers.IsDigit(num)) goto Done;
                if (answer > SByte.MaxValue / 10 + 1) goto FalseExit; // Overflow
                answer = answer * 10 + num - '0';

                // if sign < 0, (-1 * sign + 1) / 2 = 1
                // else, (-1 * sign + 1) / 2 = 0
                if ((uint)answer > (uint)SByte.MaxValue + (-1 * sign + 1) / 2) goto FalseExit; // Overflow
                index++;
                if ((uint)index >= (uint)text.Length) goto Done;
                if (!Parsers.IsDigit(text[index])) goto Done;

                // Guaranteed overflow
                goto FalseExit;
            }

            FalseExit:
            bytesConsumed = default;
            value = default;
            return false;

            Done:
            bytesConsumed = index;
            value = (SByte)(answer * sign);
            return true;
        }


        #endregion

        #region Int16
        unsafe static bool TryParseInt16(byte* text, int length, out short value)
        {
            if (length < 1)
            {
                value = default;
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

            int overflowLength = Parsers.Int16OverflowLength + indexOfFirstDigit;

            // Parse the first digit separately. If invalid here, we need to return false.
            int firstDigit = text[indexOfFirstDigit] - 48; // '0'
            if (firstDigit < 0 || firstDigit > 9)
            {
                value = default;
                return false;
            }
            int parsedValue = firstDigit;

            if (length < overflowLength)
            {
                // Length is less than Parsers.Int16OverflowLength; overflow is not possible
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
                // Length is greater than Parsers.Int16OverflowLength; overflow is only possible after Parsers.Int16OverflowLength
                // digits. There may be no overflow after Parsers.Int16OverflowLength if there are leading zeroes.
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
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            value = (short)(parsedValue * sign);
            return true;
        }

        unsafe static bool TryParseInt16(byte* text, int length, out short value, out int bytesConsumed)
        {
            if (length < 1)
            {
                bytesConsumed = 0;
                value = default;
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

            int overflowLength = Parsers.Int16OverflowLength + indexOfFirstDigit;

            // Parse the first digit separately. If invalid here, we need to return false.
            int firstDigit = text[indexOfFirstDigit] - 48; // '0'
            if (firstDigit < 0 || firstDigit > 9)
            {
                bytesConsumed = 0;
                value = default;
                return false;
            }
            int parsedValue = firstDigit;

            if (length < overflowLength)
            {
                // Length is less than Parsers.Int16OverflowLength; overflow is not possible
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
                // Length is greater than Parsers.Int16OverflowLength; overflow is only possible after Parsers.Int16OverflowLength
                // digits. There may be no overflow after Parsers.Int16OverflowLength if there are leading zeroes.
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
                        value = default;
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
            return TryParseInt16(text, out value, out int bytesConsumed);
        }

        public static bool TryParseInt16(ReadOnlySpan<byte> text, out short value, out int bytesConsumed)
        {
            if (text.Length < 1) goto FalseExit;

            int sign = 1;
            int index = 0;
            int num = text[index];
            if (num == '-')
            {
                sign = -1;
                index++;
                if ((uint)index >= (uint)text.Length) goto FalseExit;
                num = text[index];
            }
            else if (num == '+')
            {
                index++;
                if ((uint)index >= (uint)text.Length) goto FalseExit;
                num = text[index];
            }

            int answer = 0;

            if (Parsers.IsDigit(num))
            {
                if (num == '0')
                {
                    do
                    {
                        index++;
                        if ((uint)index >= (uint)text.Length) goto Done;
                        num = text[index];
                    } while (num == '0');
                    if (!Parsers.IsDigit(num)) goto Done;
                }

                answer = num - '0';
                index++;

                if ((uint)index >= (uint)text.Length) goto Done;
                num = text[index];
                if (!Parsers.IsDigit(num)) goto Done;
                index++;
                answer = 10 * answer + num - '0';

                if ((uint)index >= (uint)text.Length) goto Done;
                num = text[index];
                if (!Parsers.IsDigit(num)) goto Done;
                index++;
                answer = 10 * answer + num - '0';

                if ((uint)index >= (uint)text.Length) goto Done;
                num = text[index];
                if (!Parsers.IsDigit(num)) goto Done;
                index++;
                answer = 10 * answer + num - '0';

                // Potential overflow
                if ((uint)index >= (uint)text.Length) goto Done;
                num = text[index];
                if (!Parsers.IsDigit(num)) goto Done;
                if (answer > Int16.MaxValue / 10 + 1) goto FalseExit; // Overflow
                answer = answer * 10 + num - '0';

                // if sign < 0, (-1 * sign + 1) / 2 = 1
                // else, (-1 * sign + 1) / 2 = 0
                if ((uint)answer > (uint)Int16.MaxValue + (-1 * sign + 1) / 2) goto FalseExit; // Overflow
                index++;
                if ((uint)index >= (uint)text.Length) goto Done;
                if (!Parsers.IsDigit(text[index])) goto Done;

                // Guaranteed overflow
                goto FalseExit;
            }

            FalseExit:
            bytesConsumed = default;
            value = default;
            return false;

            Done:
            bytesConsumed = index;
            value = (short)(answer * sign);
            return true;
        }

        #endregion

        #region Int32
        unsafe static bool TryParseInt32(byte* text, int length, out int value)
        {
            if (length < 1)
            {
                value = default;
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

            int overflowLength = Parsers.Int32OverflowLength + indexOfFirstDigit;

            // Parse the first digit separately. If invalid here, we need to return false.
            int firstDigit = text[indexOfFirstDigit] - 48; // '0'
            if (firstDigit < 0 || firstDigit > 9)
            {
                value = default;
                return false;
            }
            int parsedValue = firstDigit;

            if (length < overflowLength)
            {
                // Length is less than Parsers.Int32OverflowLength; overflow is not possible
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
                // Length is greater than Parsers.Int32OverflowLength; overflow is only possible after Parsers.Int32OverflowLength
                // digits. There may be no overflow after Parsers.Int32OverflowLength if there are leading zeroes.
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
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            value = parsedValue * sign;
            return true;
        }

        unsafe static bool TryParseInt32(byte* text, int length, out int value, out int bytesConsumed)
        {
            if (length < 1)
            {
                bytesConsumed = 0;
                value = default;
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

            int overflowLength = Parsers.Int32OverflowLength + indexOfFirstDigit;

            // Parse the first digit separately. If invalid here, we need to return false.
            int firstDigit = text[indexOfFirstDigit] - 48; // '0'
            if (firstDigit < 0 || firstDigit > 9)
            {
                bytesConsumed = 0;
                value = default;
                return false;
            }
            int parsedValue = firstDigit;

            if (length < overflowLength)
            {
                // Length is less than Parsers.Int32OverflowLength; overflow is not possible
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
                // Length is greater than Parsers.Int32OverflowLength; overflow is only possible after Parsers.Int32OverflowLength
                // digits. There may be no overflow after Parsers.Int32OverflowLength if there are leading zeroes.
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
                        value = default;
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
            return TryParseInt32(text, out value, out int bytesConsumed);
        }

        public static bool TryParseInt32(ReadOnlySpan<byte> text, out int value, out int bytesConsumed)
        {
            if (text.Length < 1) goto FalseExit;

            int sign = 1;
            int index = 0;
            int num = text[index];
            if (num == '-')
            {
                sign = -1;
                index++;
                if ((uint)index >= (uint)text.Length) goto FalseExit;
                num = text[index];
            }
            else if (num == '+')
            {
                index++;
                if ((uint)index >= (uint)text.Length) goto FalseExit;
                num = text[index];
            }

            int answer = 0;

            if (Parsers.IsDigit(num))
            {
                if (num == '0')
                {
                    do
                    {
                        index++;
                        if ((uint)index >= (uint)text.Length) goto Done;
                        num = text[index];
                    } while (num == '0');
                    if (!Parsers.IsDigit(num)) goto Done;
                }

                answer = num - '0';
                index++;

                if ((uint)index >= (uint)text.Length) goto Done;
                num = text[index];
                if (!Parsers.IsDigit(num)) goto Done;
                index++;
                answer = 10 * answer + num - '0';

                if ((uint)index >= (uint)text.Length) goto Done;
                num = text[index];
                if (!Parsers.IsDigit(num)) goto Done;
                index++;
                answer = 10 * answer + num - '0';

                if ((uint)index >= (uint)text.Length) goto Done;
                num = text[index];
                if (!Parsers.IsDigit(num)) goto Done;
                index++;
                answer = 10 * answer + num - '0';

                if ((uint)index >= (uint)text.Length) goto Done;
                num = text[index];
                if (!Parsers.IsDigit(num)) goto Done;
                index++;
                answer = 10 * answer + num - '0';

                if ((uint)index >= (uint)text.Length) goto Done;
                num = text[index];
                if (!Parsers.IsDigit(num)) goto Done;
                index++;
                answer = 10 * answer + num - '0';

                if ((uint)index >= (uint)text.Length) goto Done;
                num = text[index];
                if (!Parsers.IsDigit(num)) goto Done;
                index++;
                answer = 10 * answer + num - '0';

                if ((uint)index >= (uint)text.Length) goto Done;
                num = text[index];
                if (!Parsers.IsDigit(num)) goto Done;
                index++;
                answer = 10 * answer + num - '0';

                if ((uint)index >= (uint)text.Length) goto Done;
                num = text[index];
                if (!Parsers.IsDigit(num)) goto Done;
                index++;
                answer = 10 * answer + num - '0';

                // Potential overflow
                if ((uint)index >= (uint)text.Length) goto Done;
                num = text[index];
                if (!Parsers.IsDigit(num)) goto Done;
                if (answer > Int32.MaxValue / 10 + 1) goto FalseExit; // Overflow
                answer = answer * 10 + num - '0';

                // if sign < 0, (-1 * sign + 1) / 2 = 1
                // else, (-1 * sign + 1) / 2 = 0
                if ((uint)answer > (uint)Int32.MaxValue + (-1 * sign + 1) / 2) goto FalseExit; // Overflow
                index++;
                if ((uint)index >= (uint)text.Length) goto Done;
                if (!Parsers.IsDigit(text[index])) goto Done;

                // Guaranteed overflow
                goto FalseExit;
            }

            FalseExit:
            bytesConsumed = default;
            value = default;
            return false;

            Done:
            bytesConsumed = index;
            value = answer * sign;
            return true;
        }

        #endregion

        #region Int64
        unsafe static bool TryParseInt64(byte* text, int length, out long value)
        {
            if (length < 1)
            {
                value = default;
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

            int overflowLength = Parsers.Int64OverflowLength + indexOfFirstDigit;

            // Parse the first digit separately. If invalid here, we need to return false.
            long firstDigit = text[indexOfFirstDigit] - 48; // '0'
            if (firstDigit < 0 || firstDigit > 9)
            {
                value = default;
                return false;
            }
            long parsedValue = firstDigit;

            if (length < overflowLength)
            {
                // Length is less than Parsers.Int64OverflowLength; overflow is not possible
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
                // Length is greater than Parsers.Int64OverflowLength; overflow is only possible after Parsers.Int64OverflowLength
                // digits. There may be no overflow after Parsers.Int64OverflowLength if there are leading zeroes.
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
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            value = parsedValue * sign;
            return true;
        }

        unsafe static bool TryParseInt64(byte* text, int length, out long value, out int bytesConsumed)
        {
            if (length < 1)
            {
                bytesConsumed = 0;
                value = default;
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

            int overflowLength = Parsers.Int64OverflowLength + indexOfFirstDigit;

            // Parse the first digit separately. If invalid here, we need to return false.
            long firstDigit = text[indexOfFirstDigit] - 48; // '0'
            if (firstDigit < 0 || firstDigit > 9)
            {
                bytesConsumed = 0;
                value = default;
                return false;
            }
            long parsedValue = firstDigit;

            if (length < overflowLength)
            {
                // Length is less than Parsers.Int64OverflowLength; overflow is not possible
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
                // Length is greater than Parsers.Int64OverflowLength; overflow is only possible after Parsers.Int64OverflowLength
                // digits. There may be no overflow after Parsers.Int64OverflowLength if there are leading zeroes.
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
                        value = default;
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
                value = default;
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

            int overflowLength = Parsers.Int64OverflowLength + indexOfFirstDigit;

            // Parse the first digit separately. If invalid here, we need to return false.
            long firstDigit = text[indexOfFirstDigit] - 48; // '0'
            if (firstDigit < 0 || firstDigit > 9)
            {
                value = default;
                return false;
            }
            long parsedValue = firstDigit;

            if (text.Length < overflowLength)
            {
                // Length is less than Parsers.Int64OverflowLength; overflow is not possible
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
                // Length is greater than Parsers.Int64OverflowLength; overflow is only possible after Parsers.Int64OverflowLength
                // digits. There may be no overflow after Parsers.Int64OverflowLength if there are leading zeroes.
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
                        value = default;
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
                value = default;
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

            int overflowLength = Parsers.Int64OverflowLength + indexOfFirstDigit;

            // Parse the first digit separately. If invalid here, we need to return false.
            long firstDigit = text[indexOfFirstDigit] - 48; // '0'
            if (firstDigit < 0 || firstDigit > 9)
            {
                bytesConsumed = 0;
                value = default;
                return false;
            }
            long parsedValue = firstDigit;

            if (text.Length < overflowLength)
            {
                // Length is less than Parsers.Int64OverflowLength; overflow is not possible
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
                // Length is greater than Parsers.Int64OverflowLength; overflow is only possible after Parsers.Int64OverflowLength
                // digits. There may be no overflow after Parsers.Int64OverflowLength if there are leading zeroes.
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
                        value = default;
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
    public static partial class Utf16Parser
    {
        #region SByte
        unsafe static bool TryParseSByte(char* text, int length, out sbyte value)
        {
            if (length < 1)
            {
                value = default;
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

            int overflowLength = Parsers.ByteOverflowLength + indexOfFirstDigit;

            // Parse the first digit separately. If invalid here, we need to return false.
            int firstDigit = text[indexOfFirstDigit] - 48; // '0'
            if (firstDigit < 0 || firstDigit > 9)
            {
                value = default;
                return false;
            }
            int parsedValue = firstDigit;

            if (length < overflowLength)
            {
                // Length is less than SParsers.ByteOverflowLength; overflow is not possible
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
                // Length is greater than SParsers.ByteOverflowLength; overflow is only possible after SParsers.ByteOverflowLength
                // digits. There may be no overflow after SParsers.ByteOverflowLength if there are leading zeroes.
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
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            value = (sbyte)(parsedValue * sign);
            return true;
        }

        unsafe static bool TryParseSByte(char* text, int length, out sbyte value, out int charactersConsumed)
        {
            if (length < 1)
            {
                charactersConsumed = 0;
                value = default;
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

            int overflowLength = Parsers.ByteOverflowLength + indexOfFirstDigit;

            // Parse the first digit separately. If invalid here, we need to return false.
            int firstDigit = text[indexOfFirstDigit] - 48; // '0'
            if (firstDigit < 0 || firstDigit > 9)
            {
                charactersConsumed = 0;
                value = default;
                return false;
            }
            int parsedValue = firstDigit;

            if (length < overflowLength)
            {
                // Length is less than SParsers.ByteOverflowLength; overflow is not possible
                for (int index = indexOfFirstDigit + 1; index < length; index++)
                {
                    int nextDigit = text[index] - 48; // '0'
                    if (nextDigit < 0 || nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = (sbyte)(parsedValue * sign);
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }
            else
            {
                // Length is greater than SParsers.ByteOverflowLength; overflow is only possible after SParsers.ByteOverflowLength
                // digits. There may be no overflow after SParsers.ByteOverflowLength if there are leading zeroes.
                for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                {
                    int nextDigit = text[index] - 48; // '0'
                    if (nextDigit < 0 || nextDigit > 9)
                    {
                        charactersConsumed = index;
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
                        charactersConsumed = index;
                        value = (sbyte)(parsedValue * sign);
                        return true;
                    }
                    // If parsedValue > (sbyte.MaxValue / 10), any more appended digits will cause overflow.
                    // if parsedValue == (sbyte.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                    bool positive = sign > 0;
                    bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                    if (parsedValue > sbyte.MaxValue / 10 || parsedValue == sbyte.MaxValue / 10 && nextDigitTooLarge)
                    {
                        charactersConsumed = 0;
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            charactersConsumed = length;
            value = (sbyte)(parsedValue * sign);
            return true;
        }

        public static bool TryParseSByte(ReadOnlySpan<char> text, out sbyte value)
        {
            return TryParseSByte(text, out value, out int bytesConsumed);
        }

        public static bool TryParseSByte(ReadOnlySpan<char> text, out sbyte value, out int charactersConsumed)
        {
            if (text.Length < 1)
            {
                charactersConsumed = 0;
                value = default;
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

            if (indexOfFirstDigit >= text.Length)
            {
                charactersConsumed = 0;
                value = default;
                return false;
            }

            int overflowLength = Parsers.ByteOverflowLength + indexOfFirstDigit;

            // Parse the first digit separately. If invalid here, we need to return false.
            int firstDigit = text[indexOfFirstDigit] - 48; // '0'
            if (firstDigit < 0 || firstDigit > 9)
            {
                charactersConsumed = 0;
                value = default;
                return false;
            }
            int parsedValue = firstDigit;

            if (text.Length < overflowLength)
            {
                // Length is less than SParsers.ByteOverflowLength; overflow is not possible
                for (int index = indexOfFirstDigit + 1; index < text.Length; index++)
                {
                    int nextDigit = text[index] - 48; // '0'
                    if (nextDigit < 0 || nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = (sbyte)(parsedValue * sign);
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }
            else
            {
                // Length is greater than SParsers.ByteOverflowLength; overflow is only possible after SParsers.ByteOverflowLength
                // digits. There may be no overflow after SParsers.ByteOverflowLength if there are leading zeroes.
                for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                {
                    int nextDigit = text[index] - 48; // '0'
                    if (nextDigit < 0 || nextDigit > 9)
                    {
                        charactersConsumed = index;
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
                        charactersConsumed = index;
                        value = (sbyte)(parsedValue * sign);
                        return true;
                    }
                    // If parsedValue > (sbyte.MaxValue / 10), any more appended digits will cause overflow.
                    // if parsedValue == (sbyte.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                    bool positive = sign > 0;
                    bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                    if (parsedValue > sbyte.MaxValue / 10 || parsedValue == sbyte.MaxValue / 10 && nextDigitTooLarge)
                    {
                        charactersConsumed = 0;
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            charactersConsumed = text.Length;
            value = (sbyte)(parsedValue * sign);
            return true;
        }

        #endregion

        #region Int16
        unsafe static bool TryParseInt16(char* text, int length, out short value)
        {
            if (length < 1)
            {
                value = default;
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

            int overflowLength = Parsers.Int16OverflowLength + indexOfFirstDigit;

            // Parse the first digit separately. If invalid here, we need to return false.
            int firstDigit = text[indexOfFirstDigit] - 48; // '0'
            if (firstDigit < 0 || firstDigit > 9)
            {
                value = default;
                return false;
            }
            int parsedValue = firstDigit;

            if (length < overflowLength)
            {
                // Length is less than Parsers.Int16OverflowLength; overflow is not possible
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
                // Length is greater than Parsers.Int16OverflowLength; overflow is only possible after Parsers.Int16OverflowLength
                // digits. There may be no overflow after Parsers.Int16OverflowLength if there are leading zeroes.
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
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            value = (short)(parsedValue * sign);
            return true;
        }

        unsafe static bool TryParseInt16(char* text, int length, out short value, out int charactersConsumed)
        {
            if (length < 1)
            {
                charactersConsumed = 0;
                value = default;
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

            int overflowLength = Parsers.Int16OverflowLength + indexOfFirstDigit;

            // Parse the first digit separately. If invalid here, we need to return false.
            int firstDigit = text[indexOfFirstDigit] - 48; // '0'
            if (firstDigit < 0 || firstDigit > 9)
            {
                charactersConsumed = 0;
                value = default;
                return false;
            }
            int parsedValue = firstDigit;

            if (length < overflowLength)
            {
                // Length is less than Parsers.Int16OverflowLength; overflow is not possible
                for (int index = indexOfFirstDigit + 1; index < length; index++)
                {
                    int nextDigit = text[index] - 48; // '0'
                    if (nextDigit < 0 || nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = (short)(parsedValue * sign);
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }
            else
            {
                // Length is greater than Parsers.Int16OverflowLength; overflow is only possible after Parsers.Int16OverflowLength
                // digits. There may be no overflow after Parsers.Int16OverflowLength if there are leading zeroes.
                for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                {
                    int nextDigit = text[index] - 48; // '0'
                    if (nextDigit < 0 || nextDigit > 9)
                    {
                        charactersConsumed = index;
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
                        charactersConsumed = index;
                        value = (short)(parsedValue * sign);
                        return true;
                    }
                    // If parsedValue > (short.MaxValue / 10), any more appended digits will cause overflow.
                    // if parsedValue == (short.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                    bool positive = sign > 0;
                    bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                    if (parsedValue > short.MaxValue / 10 || parsedValue == short.MaxValue / 10 && nextDigitTooLarge)
                    {
                        charactersConsumed = 0;
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            charactersConsumed = length;
            value = (short)(parsedValue * sign);
            return true;
        }

        public static bool TryParseInt16(ReadOnlySpan<char> text, out short value)
        {
            return TryParseInt16(text, out value, out int charactersConsumed);
        }

        public static bool TryParseInt16(ReadOnlySpan<char> text, out short value, out int charactersConsumed)
        {
            if (text.Length < 1)
            {
                charactersConsumed = 0;
                value = default;
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

            if (indexOfFirstDigit >= text.Length)
            {
                charactersConsumed = 0;
                value = default;
                return false;
            }

            int overflowLength = Parsers.Int16OverflowLength + indexOfFirstDigit;

            // Parse the first digit separately. If invalid here, we need to return false.
            int firstDigit = text[indexOfFirstDigit] - 48; // '0'
            if (firstDigit < 0 || firstDigit > 9)
            {
                charactersConsumed = 0;
                value = default;
                return false;
            }
            int parsedValue = firstDigit;

            if (text.Length < overflowLength)
            {
                // Length is less than Parsers.Int16OverflowLength; overflow is not possible
                for (int index = indexOfFirstDigit + 1; index < text.Length; index++)
                {
                    int nextDigit = text[index] - 48; // '0'
                    if (nextDigit < 0 || nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = (short)(parsedValue * sign);
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }
            else
            {
                // Length is greater than Parsers.Int16OverflowLength; overflow is only possible after Parsers.Int16OverflowLength
                // digits. There may be no overflow after Parsers.Int16OverflowLength if there are leading zeroes.
                for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                {
                    int nextDigit = text[index] - 48; // '0'
                    if (nextDigit < 0 || nextDigit > 9)
                    {
                        charactersConsumed = index;
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
                        charactersConsumed = index;
                        value = (short)(parsedValue * sign);
                        return true;
                    }
                    // If parsedValue > (short.MaxValue / 10), any more appended digits will cause overflow.
                    // if parsedValue == (short.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                    bool positive = sign > 0;
                    bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                    if (parsedValue > short.MaxValue / 10 || parsedValue == short.MaxValue / 10 && nextDigitTooLarge)
                    {
                        charactersConsumed = 0;
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            charactersConsumed = text.Length;
            value = (short)(parsedValue * sign);
            return true;
        }

        #endregion

        #region Int32
        unsafe static bool TryParseInt32(char* text, int length, out int value)
        {
            if (length < 1)
            {
                value = default;
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

            int overflowLength = Parsers.Int32OverflowLength + indexOfFirstDigit;

            // Parse the first digit separately. If invalid here, we need to return false.
            int firstDigit = text[indexOfFirstDigit] - 48; // '0'
            if (firstDigit < 0 || firstDigit > 9)
            {
                value = default;
                return false;
            }
            int parsedValue = firstDigit;

            if (length < overflowLength)
            {
                // Length is less than Parsers.Int32OverflowLength; overflow is not possible
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
                // Length is greater than Parsers.Int32OverflowLength; overflow is only possible after Parsers.Int32OverflowLength
                // digits. There may be no overflow after Parsers.Int32OverflowLength if there are leading zeroes.
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
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            value = parsedValue * sign;
            return true;
        }

        unsafe static bool TryParseInt32(char* text, int length, out int value, out int charactersConsumed)
        {
            if (length < 1)
            {
                charactersConsumed = 0;
                value = default;
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

            int overflowLength = Parsers.Int32OverflowLength + indexOfFirstDigit;

            // Parse the first digit separately. If invalid here, we need to return false.
            int firstDigit = text[indexOfFirstDigit] - 48; // '0'
            if (firstDigit < 0 || firstDigit > 9)
            {
                charactersConsumed = 0;
                value = default;
                return false;
            }
            int parsedValue = firstDigit;

            if (length < overflowLength)
            {
                // Length is less than Parsers.Int32OverflowLength; overflow is not possible
                for (int index = indexOfFirstDigit + 1; index < length; index++)
                {
                    int nextDigit = text[index] - 48; // '0'
                    if (nextDigit < 0 || nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = parsedValue * sign;
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }
            else
            {
                // Length is greater than Parsers.Int32OverflowLength; overflow is only possible after Parsers.Int32OverflowLength
                // digits. There may be no overflow after Parsers.Int32OverflowLength if there are leading zeroes.
                for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                {
                    int nextDigit = text[index] - 48; // '0'
                    if (nextDigit < 0 || nextDigit > 9)
                    {
                        charactersConsumed = index;
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
                        charactersConsumed = index;
                        value = parsedValue * sign;
                        return true;
                    }
                    // If parsedValue > (int.MaxValue / 10), any more appended digits will cause overflow.
                    // if parsedValue == (int.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                    bool positive = sign > 0;
                    bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                    if (parsedValue > int.MaxValue / 10 || parsedValue == int.MaxValue / 10 && nextDigitTooLarge)
                    {
                        charactersConsumed = 0;
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            charactersConsumed = length;
            value = parsedValue * sign;
            return true;
        }

        public static bool TryParseInt32(ReadOnlySpan<char> text, out int value)
        {
            return TryParseInt32(text, out value, out int charactersConsumed);
        }

        public static bool TryParseInt32(ReadOnlySpan<char> text, out int value, out int charactersConsumed)
        {
            if (text.Length < 1)
            {
                charactersConsumed = 0;
                value = default;
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

            if (indexOfFirstDigit >= text.Length)
            {
                charactersConsumed = 0;
                value = default;
                return false;
            }

            int overflowLength = Parsers.Int32OverflowLength + indexOfFirstDigit;

            // Parse the first digit separately. If invalid here, we need to return false.
            int firstDigit = text[indexOfFirstDigit] - 48; // '0'
            if (firstDigit < 0 || firstDigit > 9)
            {
                charactersConsumed = 0;
                value = default;
                return false;
            }
            int parsedValue = firstDigit;

            if (text.Length < overflowLength)
            {
                // Length is less than Parsers.Int32OverflowLength; overflow is not possible
                for (int index = indexOfFirstDigit + 1; index < text.Length; index++)
                {
                    int nextDigit = text[index] - 48; // '0'
                    if (nextDigit < 0 || nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = parsedValue * sign;
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }
            else
            {
                // Length is greater than Parsers.Int32OverflowLength; overflow is only possible after Parsers.Int32OverflowLength
                // digits. There may be no overflow after Parsers.Int32OverflowLength if there are leading zeroes.
                for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                {
                    int nextDigit = text[index] - 48; // '0'
                    if (nextDigit < 0 || nextDigit > 9)
                    {
                        charactersConsumed = index;
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
                        charactersConsumed = index;
                        value = parsedValue * sign;
                        return true;
                    }
                    // If parsedValue > (int.MaxValue / 10), any more appended digits will cause overflow.
                    // if parsedValue == (int.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                    bool positive = sign > 0;
                    bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                    if (parsedValue > int.MaxValue / 10 || parsedValue == int.MaxValue / 10 && nextDigitTooLarge)
                    {
                        charactersConsumed = 0;
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            charactersConsumed = text.Length;
            value = parsedValue * sign;
            return true;
        }

        #endregion

        #region Int64
        unsafe static bool TryParseInt64(char* text, int length, out long value)
        {
            if (length < 1)
            {
                value = default;
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

            int overflowLength = Parsers.Int64OverflowLength + indexOfFirstDigit;

            // Parse the first digit separately. If invalid here, we need to return false.
            long firstDigit = text[indexOfFirstDigit] - 48; // '0'
            if (firstDigit < 0 || firstDigit > 9)
            {
                value = default;
                return false;
            }
            long parsedValue = firstDigit;

            if (length < overflowLength)
            {
                // Length is less than Parsers.Int64OverflowLength; overflow is not possible
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
                // Length is greater than Parsers.Int64OverflowLength; overflow is only possible after Parsers.Int64OverflowLength
                // digits. There may be no overflow after Parsers.Int64OverflowLength if there are leading zeroes.
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
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            value = parsedValue * sign;
            return true;
        }

        unsafe static bool TryParseInt64(char* text, int length, out long value, out int charactersConsumed)
        {
            if (length < 1)
            {
                charactersConsumed = 0;
                value = default;
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

            int overflowLength = Parsers.Int64OverflowLength + indexOfFirstDigit;

            // Parse the first digit separately. If invalid here, we need to return false.
            long firstDigit = text[indexOfFirstDigit] - 48; // '0'
            if (firstDigit < 0 || firstDigit > 9)
            {
                charactersConsumed = 0;
                value = default;
                return false;
            }
            long parsedValue = firstDigit;

            if (length < overflowLength)
            {
                // Length is less than Parsers.Int64OverflowLength; overflow is not possible
                for (int index = indexOfFirstDigit + 1; index < length; index++)
                {
                    long nextDigit = text[index] - 48; // '0'
                    if (nextDigit < 0 || nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = parsedValue * sign;
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }
            else
            {
                // Length is greater than Parsers.Int64OverflowLength; overflow is only possible after Parsers.Int64OverflowLength
                // digits. There may be no overflow after Parsers.Int64OverflowLength if there are leading zeroes.
                for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                {
                    long nextDigit = text[index] - 48; // '0'
                    if (nextDigit < 0 || nextDigit > 9)
                    {
                        charactersConsumed = index;
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
                        charactersConsumed = index;
                        value = parsedValue * sign;
                        return true;
                    }
                    // If parsedValue > (long.MaxValue / 10), any more appended digits will cause overflow.
                    // if parsedValue == (long.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                    bool positive = sign > 0;
                    bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                    if (parsedValue > long.MaxValue / 10 || parsedValue == long.MaxValue / 10 && nextDigitTooLarge)
                    {
                        charactersConsumed = 0;
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            charactersConsumed = length;
            value = parsedValue * sign;
            return true;
        }

        public static bool TryParseInt64(ReadOnlySpan<char> text, out long value)
        {
            if (text.Length < 1)
            {
                value = default;
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

            int overflowLength = Parsers.Int64OverflowLength + indexOfFirstDigit;

            // Parse the first digit separately. If invalid here, we need to return false.
            long firstDigit = text[indexOfFirstDigit] - 48; // '0'
            if (firstDigit < 0 || firstDigit > 9)
            {
                value = default;
                return false;
            }
            long parsedValue = firstDigit;

            if (text.Length < overflowLength)
            {
                // Length is less than Parsers.Int64OverflowLength; overflow is not possible
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
                // Length is greater than Parsers.Int64OverflowLength; overflow is only possible after Parsers.Int64OverflowLength
                // digits. There may be no overflow after Parsers.Int64OverflowLength if there are leading zeroes.
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
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            value = parsedValue * sign;
            return true;
        }

        public static bool TryParseInt64(ReadOnlySpan<char> text, out long value, out int charactersConsumed)
        {
            if (text.Length < 1)
            {
                charactersConsumed = 0;
                value = default;
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

            int overflowLength = Parsers.Int64OverflowLength + indexOfFirstDigit;

            // Parse the first digit separately. If invalid here, we need to return false.
            long firstDigit = text[indexOfFirstDigit] - 48; // '0'
            if (firstDigit < 0 || firstDigit > 9)
            {
                charactersConsumed = 0;
                value = default;
                return false;
            }
            long parsedValue = firstDigit;

            if (text.Length < overflowLength)
            {
                // Length is less than Parsers.Int64OverflowLength; overflow is not possible
                for (int index = indexOfFirstDigit + 1; index < text.Length; index++)
                {
                    long nextDigit = text[index] - 48; // '0'
                    if (nextDigit < 0 || nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = parsedValue * sign;
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }
            else
            {
                // Length is greater than Parsers.Int64OverflowLength; overflow is only possible after Parsers.Int64OverflowLength
                // digits. There may be no overflow after Parsers.Int64OverflowLength if there are leading zeroes.
                for (int index = indexOfFirstDigit + 1; index < overflowLength - 1; index++)
                {
                    long nextDigit = text[index] - 48; // '0'
                    if (nextDigit < 0 || nextDigit > 9)
                    {
                        charactersConsumed = index;
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
                        charactersConsumed = index;
                        value = parsedValue * sign;
                        return true;
                    }
                    // If parsedValue > (long.MaxValue / 10), any more appended digits will cause overflow.
                    // if parsedValue == (long.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
                    bool positive = sign > 0;
                    bool nextDigitTooLarge = nextDigit > 8 || (positive && nextDigit > 7);
                    if (parsedValue > long.MaxValue / 10 || parsedValue == long.MaxValue / 10 && nextDigitTooLarge)
                    {
                        charactersConsumed = 0;
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            charactersConsumed = text.Length;
            value = parsedValue * sign;
            return true;
        }

        #endregion

    }
}
