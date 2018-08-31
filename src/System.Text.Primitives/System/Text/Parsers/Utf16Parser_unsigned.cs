// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Buffers.Text
{
    public static partial class Utf16Parser
    {
        #region Byte
        unsafe static bool TryParseByte(char* text, int length, out byte value)
        {
            if (length < 1)
            {
                value = default;
                return false;
            }

            // Parse the first digit separately. If invalid here, we need to return false.
            uint firstDigit = text[0] - 48u; // '0'
            if (firstDigit > 9)
            {
                value = default;
                return false;
            }
            uint parsedValue = firstDigit;

            if (length < Parsers.ByteOverflowLength)
            {
                // Length is less than Parsers.ByteOverflowLength; overflow is not possible
                for (int index = 1; index < length; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        value = (byte)(parsedValue);
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }
            else
            {
                // Length is greater than Parsers.ByteOverflowLength; overflow is only possible after Parsers.ByteOverflowLength
                // digits. There may be no overflow after Parsers.ByteOverflowLength if there are leading zeroes.
                for (int index = 1; index < Parsers.ByteOverflowLength - 1; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        value = (byte)(parsedValue);
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
                for (int index = Parsers.ByteOverflowLength - 1; index < length; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        value = (byte)(parsedValue);
                        return true;
                    }
                    // If parsedValue > (byte.MaxValue / 10), any more appended digits will cause overflow.
                    // if parsedValue == (byte.MaxValue / 10), any nextDigit greater than 5 implies overflow.
                    if (parsedValue > byte.MaxValue / 10 || (parsedValue == byte.MaxValue / 10 && nextDigit > 5))
                    {
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            value = (byte)(parsedValue);
            return true;
        }

        unsafe static bool TryParseByte(char* text, int length, out byte value, out int charactersConsumed)
        {
            if (length < 1)
            {
                charactersConsumed = 0;
                value = default;
                return false;
            }

            // Parse the first digit separately. If invalid here, we need to return false.
            uint firstDigit = text[0] - 48u; // '0'
            if (firstDigit > 9)
            {
                charactersConsumed = 0;
                value = default;
                return false;
            }
            uint parsedValue = firstDigit;

            if (length < Parsers.ByteOverflowLength)
            {
                // Length is less than Parsers.ByteOverflowLength; overflow is not possible
                for (int index = 1; index < length; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = (byte)(parsedValue);
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }
            else
            {
                // Length is greater than Parsers.ByteOverflowLength; overflow is only possible after Parsers.ByteOverflowLength
                // digits. There may be no overflow after Parsers.ByteOverflowLength if there are leading zeroes.
                for (int index = 1; index < Parsers.ByteOverflowLength - 1; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = (byte)(parsedValue);
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
                for (int index = Parsers.ByteOverflowLength - 1; index < length; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = (byte)(parsedValue);
                        return true;
                    }
                    // If parsedValue > (byte.MaxValue / 10), any more appended digits will cause overflow.
                    // if parsedValue == (byte.MaxValue / 10), any nextDigit greater than 5 implies overflow.
                    if (parsedValue > byte.MaxValue / 10 || (parsedValue == byte.MaxValue / 10 && nextDigit > 5))
                    {
                        charactersConsumed = 0;
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            charactersConsumed = length;
            value = (byte)(parsedValue);
            return true;
        }

        public static bool TryParseByte(ReadOnlySpan<char> text, out byte value)
        {
            if (text.Length < 1)
            {
                value = default;
                return false;
            }

            // Parse the first digit separately. If invalid here, we need to return false.
            uint firstDigit = text[0] - 48u; // '0'
            if (firstDigit > 9)
            {
                value = default;
                return false;
            }
            uint parsedValue = firstDigit;

            if (text.Length < Parsers.ByteOverflowLength)
            {
                // Length is less than Parsers.ByteOverflowLength; overflow is not possible
                for (int index = 1; index < text.Length; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        value = (byte)(parsedValue);
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }
            else
            {
                // Length is greater than Parsers.ByteOverflowLength; overflow is only possible after Parsers.ByteOverflowLength
                // digits. There may be no overflow after Parsers.ByteOverflowLength if there are leading zeroes.
                for (int index = 1; index < Parsers.ByteOverflowLength - 1; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        value = (byte)(parsedValue);
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
                for (int index = Parsers.ByteOverflowLength - 1; index < text.Length; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        value = (byte)(parsedValue);
                        return true;
                    }
                    // If parsedValue > (byte.MaxValue / 10), any more appended digits will cause overflow.
                    // if parsedValue == (byte.MaxValue / 10), any nextDigit greater than 5 implies overflow.
                    if (parsedValue > byte.MaxValue / 10 || (parsedValue == byte.MaxValue / 10 && nextDigit > 5))
                    {
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            value = (byte)(parsedValue);
            return true;
        }

        public static bool TryParseByte(ReadOnlySpan<char> text, out byte value, out int charactersConsumed)
        {
            if (text.Length < 1)
            {
                charactersConsumed = 0;
                value = default;
                return false;
            }

            // Parse the first digit separately. If invalid here, we need to return false.
            uint firstDigit = text[0] - 48u; // '0'
            if (firstDigit > 9)
            {
                charactersConsumed = 0;
                value = default;
                return false;
            }
            uint parsedValue = firstDigit;

            if (text.Length < Parsers.ByteOverflowLength)
            {
                // Length is less than Parsers.ByteOverflowLength; overflow is not possible
                for (int index = 1; index < text.Length; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = (byte)(parsedValue);
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }
            else
            {
                // Length is greater than Parsers.ByteOverflowLength; overflow is only possible after Parsers.ByteOverflowLength
                // digits. There may be no overflow after Parsers.ByteOverflowLength if there are leading zeroes.
                for (int index = 1; index < Parsers.ByteOverflowLength - 1; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = (byte)(parsedValue);
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
                for (int index = Parsers.ByteOverflowLength - 1; index < text.Length; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = (byte)(parsedValue);
                        return true;
                    }
                    // If parsedValue > (byte.MaxValue / 10), any more appended digits will cause overflow.
                    // if parsedValue == (byte.MaxValue / 10), any nextDigit greater than 5 implies overflow.
                    if (parsedValue > byte.MaxValue / 10 || (parsedValue == byte.MaxValue / 10 && nextDigit > 5))
                    {
                        charactersConsumed = 0;
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            charactersConsumed = text.Length;
            value = (byte)(parsedValue);
            return true;
        }

        #endregion

        #region UInt16
        unsafe static bool TryParseUInt16(char* text, int length, out ushort value)
        {
            if (length < 1)
            {
                value = default;
                return false;
            }

            // Parse the first digit separately. If invalid here, we need to return false.
            uint firstDigit = text[0] - 48u; // '0'
            if (firstDigit > 9)
            {
                value = default;
                return false;
            }
            uint parsedValue = firstDigit;

            if (length < Parsers.Int16OverflowLength)
            {
                // Length is less than Parsers.Int16OverflowLength; overflow is not possible
                for (int index = 1; index < length; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        value = (ushort)(parsedValue);
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }
            else
            {
                // Length is greater than Parsers.Int16OverflowLength; overflow is only possible after Parsers.Int16OverflowLength
                // digits. There may be no overflow after Parsers.Int16OverflowLength if there are leading zeroes.
                for (int index = 1; index < Parsers.Int16OverflowLength - 1; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        value = (ushort)(parsedValue);
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
                for (int index = Parsers.Int16OverflowLength - 1; index < length; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        value = (ushort)(parsedValue);
                        return true;
                    }
                    // If parsedValue > (ushort.MaxValue / 10), any more appended digits will cause overflow.
                    // if parsedValue == (ushort.MaxValue / 10), any nextDigit greater than 5 implies overflow.
                    if (parsedValue > ushort.MaxValue / 10 || (parsedValue == ushort.MaxValue / 10 && nextDigit > 5))
                    {
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            value = (ushort)(parsedValue);
            return true;
        }

        unsafe static bool TryParseUInt16(char* text, int length, out ushort value, out int charactersConsumed)
        {
            if (length < 1)
            {
                charactersConsumed = 0;
                value = default;
                return false;
            }

            // Parse the first digit separately. If invalid here, we need to return false.
            uint firstDigit = text[0] - 48u; // '0'
            if (firstDigit > 9)
            {
                charactersConsumed = 0;
                value = default;
                return false;
            }
            uint parsedValue = firstDigit;

            if (length < Parsers.Int16OverflowLength)
            {
                // Length is less than Parsers.Int16OverflowLength; overflow is not possible
                for (int index = 1; index < length; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = (ushort)(parsedValue);
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }
            else
            {
                // Length is greater than Parsers.Int16OverflowLength; overflow is only possible after Parsers.Int16OverflowLength
                // digits. There may be no overflow after Parsers.Int16OverflowLength if there are leading zeroes.
                for (int index = 1; index < Parsers.Int16OverflowLength - 1; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = (ushort)(parsedValue);
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
                for (int index = Parsers.Int16OverflowLength - 1; index < length; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = (ushort)(parsedValue);
                        return true;
                    }
                    // If parsedValue > (ushort.MaxValue / 10), any more appended digits will cause overflow.
                    // if parsedValue == (ushort.MaxValue / 10), any nextDigit greater than 5 implies overflow.
                    if (parsedValue > ushort.MaxValue / 10 || (parsedValue == ushort.MaxValue / 10 && nextDigit > 5))
                    {
                        charactersConsumed = 0;
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            charactersConsumed = length;
            value = (ushort)(parsedValue);
            return true;
        }

        public static bool TryParseUInt16(ReadOnlySpan<char> text, out ushort value)
        {
            if (text.Length < 1)
            {
                value = default;
                return false;
            }

            // Parse the first digit separately. If invalid here, we need to return false.
            uint firstDigit = text[0] - 48u; // '0'
            if (firstDigit > 9)
            {
                value = default;
                return false;
            }
            uint parsedValue = firstDigit;

            if (text.Length < Parsers.Int16OverflowLength)
            {
                // Length is less than Parsers.Int16OverflowLength; overflow is not possible
                for (int index = 1; index < text.Length; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        value = (ushort)(parsedValue);
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }
            else
            {
                // Length is greater than Parsers.Int16OverflowLength; overflow is only possible after Parsers.Int16OverflowLength
                // digits. There may be no overflow after Parsers.Int16OverflowLength if there are leading zeroes.
                for (int index = 1; index < Parsers.Int16OverflowLength - 1; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        value = (ushort)(parsedValue);
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
                for (int index = Parsers.Int16OverflowLength - 1; index < text.Length; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        value = (ushort)(parsedValue);
                        return true;
                    }
                    // If parsedValue > (ushort.MaxValue / 10), any more appended digits will cause overflow.
                    // if parsedValue == (ushort.MaxValue / 10), any nextDigit greater than 5 implies overflow.
                    if (parsedValue > ushort.MaxValue / 10 || (parsedValue == ushort.MaxValue / 10 && nextDigit > 5))
                    {
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            value = (ushort)(parsedValue);
            return true;
        }

        public static bool TryParseUInt16(ReadOnlySpan<char> text, out ushort value, out int charactersConsumed)
        {
            if (text.Length < 1)
            {
                charactersConsumed = 0;
                value = default;
                return false;
            }

            // Parse the first digit separately. If invalid here, we need to return false.
            uint firstDigit = text[0] - 48u; // '0'
            if (firstDigit > 9)
            {
                charactersConsumed = 0;
                value = default;
                return false;
            }
            uint parsedValue = firstDigit;

            if (text.Length < Parsers.Int16OverflowLength)
            {
                // Length is less than Parsers.Int16OverflowLength; overflow is not possible
                for (int index = 1; index < text.Length; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = (ushort)(parsedValue);
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }
            else
            {
                // Length is greater than Parsers.Int16OverflowLength; overflow is only possible after Parsers.Int16OverflowLength
                // digits. There may be no overflow after Parsers.Int16OverflowLength if there are leading zeroes.
                for (int index = 1; index < Parsers.Int16OverflowLength - 1; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = (ushort)(parsedValue);
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
                for (int index = Parsers.Int16OverflowLength - 1; index < text.Length; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = (ushort)(parsedValue);
                        return true;
                    }
                    // If parsedValue > (ushort.MaxValue / 10), any more appended digits will cause overflow.
                    // if parsedValue == (ushort.MaxValue / 10), any nextDigit greater than 5 implies overflow.
                    if (parsedValue > ushort.MaxValue / 10 || (parsedValue == ushort.MaxValue / 10 && nextDigit > 5))
                    {
                        charactersConsumed = 0;
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            charactersConsumed = text.Length;
            value = (ushort)(parsedValue);
            return true;
        }

        #endregion

        #region UInt32
        unsafe static bool TryParseUInt32(char* text, int length, out uint value)
        {
            if (length < 1)
            {
                value = default;
                return false;
            }

            // Parse the first digit separately. If invalid here, we need to return false.
            uint firstDigit = text[0] - 48u; // '0'
            if (firstDigit > 9)
            {
                value = default;
                return false;
            }
            uint parsedValue = firstDigit;

            if (length < Parsers.Int32OverflowLength)
            {
                // Length is less than Parsers.Int32OverflowLength; overflow is not possible
                for (int index = 1; index < length; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        value = parsedValue;
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }
            else
            {
                // Length is greater than Parsers.Int32OverflowLength; overflow is only possible after Parsers.Int32OverflowLength
                // digits. There may be no overflow after Parsers.Int32OverflowLength if there are leading zeroes.
                for (int index = 1; index < Parsers.Int32OverflowLength - 1; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        value = parsedValue;
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
                for (int index = Parsers.Int32OverflowLength - 1; index < length; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        value = parsedValue;
                        return true;
                    }
                    // If parsedValue > (uint.MaxValue / 10), any more appended digits will cause overflow.
                    // if parsedValue == (uint.MaxValue / 10), any nextDigit greater than 5 implies overflow.
                    if (parsedValue > uint.MaxValue / 10 || (parsedValue == uint.MaxValue / 10 && nextDigit > 5))
                    {
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            value = parsedValue;
            return true;
        }

        unsafe static bool TryParseUInt32(char* text, int length, out uint value, out int charactersConsumed)
        {
            if (length < 1)
            {
                charactersConsumed = 0;
                value = default;
                return false;
            }

            // Parse the first digit separately. If invalid here, we need to return false.
            uint firstDigit = text[0] - 48u; // '0'
            if (firstDigit > 9)
            {
                charactersConsumed = 0;
                value = default;
                return false;
            }
            uint parsedValue = firstDigit;

            if (length < Parsers.Int32OverflowLength)
            {
                // Length is less than Parsers.Int32OverflowLength; overflow is not possible
                for (int index = 1; index < length; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = parsedValue;
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }
            else
            {
                // Length is greater than Parsers.Int32OverflowLength; overflow is only possible after Parsers.Int32OverflowLength
                // digits. There may be no overflow after Parsers.Int32OverflowLength if there are leading zeroes.
                for (int index = 1; index < Parsers.Int32OverflowLength - 1; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = parsedValue;
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
                for (int index = Parsers.Int32OverflowLength - 1; index < length; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = parsedValue;
                        return true;
                    }
                    // If parsedValue > (uint.MaxValue / 10), any more appended digits will cause overflow.
                    // if parsedValue == (uint.MaxValue / 10), any nextDigit greater than 5 implies overflow.
                    if (parsedValue > uint.MaxValue / 10 || (parsedValue == uint.MaxValue / 10 && nextDigit > 5))
                    {
                        charactersConsumed = 0;
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            charactersConsumed = length;
            value = parsedValue;
            return true;
        }

        public static bool TryParseUInt32(ReadOnlySpan<char> text, out uint value)
        {
            if (text.Length < 1)
            {
                value = default;
                return false;
            }

            // Parse the first digit separately. If invalid here, we need to return false.
            uint firstDigit = text[0] - 48u; // '0'
            if (firstDigit > 9)
            {
                value = default;
                return false;
            }
            uint parsedValue = firstDigit;

            if (text.Length < Parsers.Int32OverflowLength)
            {
                // Length is less than Parsers.Int32OverflowLength; overflow is not possible
                for (int index = 1; index < text.Length; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        value = parsedValue;
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }
            else
            {
                // Length is greater than Parsers.Int32OverflowLength; overflow is only possible after Parsers.Int32OverflowLength
                // digits. There may be no overflow after Parsers.Int32OverflowLength if there are leading zeroes.
                for (int index = 1; index < Parsers.Int32OverflowLength - 1; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        value = parsedValue;
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
                for (int index = Parsers.Int32OverflowLength - 1; index < text.Length; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        value = parsedValue;
                        return true;
                    }
                    // If parsedValue > (uint.MaxValue / 10), any more appended digits will cause overflow.
                    // if parsedValue == (uint.MaxValue / 10), any nextDigit greater than 5 implies overflow.
                    if (parsedValue > uint.MaxValue / 10 || (parsedValue == uint.MaxValue / 10 && nextDigit > 5))
                    {
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            value = parsedValue;
            return true;
        }

        public static bool TryParseUInt32(ReadOnlySpan<char> text, out uint value, out int charactersConsumed)
        {
            if (text.Length < 1)
            {
                charactersConsumed = 0;
                value = default;
                return false;
            }

            // Parse the first digit separately. If invalid here, we need to return false.
            uint firstDigit = text[0] - 48u; // '0'
            if (firstDigit > 9)
            {
                charactersConsumed = 0;
                value = default;
                return false;
            }
            uint parsedValue = firstDigit;

            if (text.Length < Parsers.Int32OverflowLength)
            {
                // Length is less than Parsers.Int32OverflowLength; overflow is not possible
                for (int index = 1; index < text.Length; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = parsedValue;
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }
            else
            {
                // Length is greater than Parsers.Int32OverflowLength; overflow is only possible after Parsers.Int32OverflowLength
                // digits. There may be no overflow after Parsers.Int32OverflowLength if there are leading zeroes.
                for (int index = 1; index < Parsers.Int32OverflowLength - 1; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = parsedValue;
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
                for (int index = Parsers.Int32OverflowLength - 1; index < text.Length; index++)
                {
                    uint nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = parsedValue;
                        return true;
                    }
                    // If parsedValue > (uint.MaxValue / 10), any more appended digits will cause overflow.
                    // if parsedValue == (uint.MaxValue / 10), any nextDigit greater than 5 implies overflow.
                    if (parsedValue > uint.MaxValue / 10 || (parsedValue == uint.MaxValue / 10 && nextDigit > 5))
                    {
                        charactersConsumed = 0;
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            charactersConsumed = text.Length;
            value = parsedValue;
            return true;
        }

        #endregion

        #region UInt64
        unsafe static bool TryParseUInt64(char* text, int length, out ulong value)
        {
            if (length < 1)
            {
                value = default;
                return false;
            }

            // Parse the first digit separately. If invalid here, we need to return false.
            ulong firstDigit = text[0] - 48u; // '0'
            if (firstDigit > 9)
            {
                value = default;
                return false;
            }
            ulong parsedValue = firstDigit;

            if (length < Parsers.Int64OverflowLength)
            {
                // Length is less than Parsers.Int64OverflowLength; overflow is not possible
                for (int index = 1; index < length; index++)
                {
                    ulong nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        value = parsedValue;
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }
            else
            {
                // Length is greater than Parsers.Int64OverflowLength; overflow is only possible after Parsers.Int64OverflowLength
                // digits. There may be no overflow after Parsers.Int64OverflowLength if there are leading zeroes.
                for (int index = 1; index < Parsers.Int64OverflowLength - 1; index++)
                {
                    ulong nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        value = parsedValue;
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
                for (int index = Parsers.Int64OverflowLength - 1; index < length; index++)
                {
                    ulong nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        value = parsedValue;
                        return true;
                    }
                    // If parsedValue > (ulong.MaxValue / 10), any more appended digits will cause overflow.
                    // if parsedValue == (ulong.MaxValue / 10), any nextDigit greater than 5 implies overflow.
                    if (parsedValue > ulong.MaxValue / 10 || (parsedValue == ulong.MaxValue / 10 && nextDigit > 5))
                    {
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            value = parsedValue;
            return true;
        }

        unsafe static bool TryParseUInt64(char* text, int length, out ulong value, out int charactersConsumed)
        {
            if (length < 1)
            {
                charactersConsumed = 0;
                value = default;
                return false;
            }

            // Parse the first digit separately. If invalid here, we need to return false.
            ulong firstDigit = text[0] - 48u; // '0'
            if (firstDigit > 9)
            {
                charactersConsumed = 0;
                value = default;
                return false;
            }
            ulong parsedValue = firstDigit;

            if (length < Parsers.Int64OverflowLength)
            {
                // Length is less than Parsers.Int64OverflowLength; overflow is not possible
                for (int index = 1; index < length; index++)
                {
                    ulong nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = parsedValue;
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }
            else
            {
                // Length is greater than Parsers.Int64OverflowLength; overflow is only possible after Parsers.Int64OverflowLength
                // digits. There may be no overflow after Parsers.Int64OverflowLength if there are leading zeroes.
                for (int index = 1; index < Parsers.Int64OverflowLength - 1; index++)
                {
                    ulong nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = parsedValue;
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
                for (int index = Parsers.Int64OverflowLength - 1; index < length; index++)
                {
                    ulong nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = parsedValue;
                        return true;
                    }
                    // If parsedValue > (ulong.MaxValue / 10), any more appended digits will cause overflow.
                    // if parsedValue == (ulong.MaxValue / 10), any nextDigit greater than 5 implies overflow.
                    if (parsedValue > ulong.MaxValue / 10 || (parsedValue == ulong.MaxValue / 10 && nextDigit > 5))
                    {
                        charactersConsumed = 0;
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            charactersConsumed = length;
            value = parsedValue;
            return true;
        }

        public static bool TryParseUInt64(ReadOnlySpan<char> text, out ulong value)
        {
            if (text.Length < 1)
            {
                value = default;
                return false;
            }

            // Parse the first digit separately. If invalid here, we need to return false.
            ulong firstDigit = text[0] - 48u; // '0'
            if (firstDigit > 9)
            {
                value = default;
                return false;
            }
            ulong parsedValue = firstDigit;

            if (text.Length < Parsers.Int64OverflowLength)
            {
                // Length is less than Parsers.Int64OverflowLength; overflow is not possible
                for (int index = 1; index < text.Length; index++)
                {
                    ulong nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        value = parsedValue;
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }
            else
            {
                // Length is greater than Parsers.Int64OverflowLength; overflow is only possible after Parsers.Int64OverflowLength
                // digits. There may be no overflow after Parsers.Int64OverflowLength if there are leading zeroes.
                for (int index = 1; index < Parsers.Int64OverflowLength - 1; index++)
                {
                    ulong nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        value = parsedValue;
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
                for (int index = Parsers.Int64OverflowLength - 1; index < text.Length; index++)
                {
                    ulong nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        value = parsedValue;
                        return true;
                    }
                    // If parsedValue > (ulong.MaxValue / 10), any more appended digits will cause overflow.
                    // if parsedValue == (ulong.MaxValue / 10), any nextDigit greater than 5 implies overflow.
                    if (parsedValue > ulong.MaxValue / 10 || (parsedValue == ulong.MaxValue / 10 && nextDigit > 5))
                    {
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            value = parsedValue;
            return true;
        }

        public static bool TryParseUInt64(ReadOnlySpan<char> text, out ulong value, out int charactersConsumed)
        {
            if (text.Length < 1)
            {
                charactersConsumed = 0;
                value = default;
                return false;
            }

            // Parse the first digit separately. If invalid here, we need to return false.
            ulong firstDigit = text[0] - 48u; // '0'
            if (firstDigit > 9)
            {
                charactersConsumed = 0;
                value = default;
                return false;
            }
            ulong parsedValue = firstDigit;

            if (text.Length < Parsers.Int64OverflowLength)
            {
                // Length is less than Parsers.Int64OverflowLength; overflow is not possible
                for (int index = 1; index < text.Length; index++)
                {
                    ulong nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = parsedValue;
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }
            else
            {
                // Length is greater than Parsers.Int64OverflowLength; overflow is only possible after Parsers.Int64OverflowLength
                // digits. There may be no overflow after Parsers.Int64OverflowLength if there are leading zeroes.
                for (int index = 1; index < Parsers.Int64OverflowLength - 1; index++)
                {
                    ulong nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = parsedValue;
                        return true;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
                for (int index = Parsers.Int64OverflowLength - 1; index < text.Length; index++)
                {
                    ulong nextDigit = text[index] - 48u; // '0'
                    if (nextDigit > 9)
                    {
                        charactersConsumed = index;
                        value = parsedValue;
                        return true;
                    }
                    // If parsedValue > (ulong.MaxValue / 10), any more appended digits will cause overflow.
                    // if parsedValue == (ulong.MaxValue / 10), any nextDigit greater than 5 implies overflow.
                    if (parsedValue > ulong.MaxValue / 10 || (parsedValue == ulong.MaxValue / 10 && nextDigit > 5))
                    {
                        charactersConsumed = 0;
                        value = default;
                        return false;
                    }
                    parsedValue = parsedValue * 10 + nextDigit;
                }
            }

            charactersConsumed = text.Length;
            value = parsedValue;
            return true;
        }

        #endregion

    } 
}
