// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Buffers.Text
{
    public static partial class Utf16Parser
    {
        internal static partial class Hex
        {
            #region Byte
            public unsafe static bool TryParseByte(char* text, int length, out byte value)
            {
                if (length < 1)
                {
                    value = default;
                    return false;
                }
                char nextCharacter;
                byte nextDigit;

                // Cache Parsers.s_HexLookup in order to avoid static constructor checks
                byte[] hexLookup = Parsers.HexLookup;

                // Parse the first digit separately. If invalid here, we need to return false.
                nextCharacter = text[0];
                nextDigit = hexLookup[(byte)nextCharacter];
                if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                {
                    value = default;
                    return false;
                }
                uint parsedValue = nextDigit;

                if (length <= Parsers.ByteOverflowLengthHex)
                {
                    // Length is less than or equal to Parsers.ByteOverflowLengthHex; overflow is not possible
                    for (int index = 1; index < length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            value = (byte)(parsedValue);
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Parsers.ByteOverflowLengthHex; overflow is only possible after Parsers.ByteOverflowLengthHex
                    // digits. There may be no overflow after Parsers.ByteOverflowLengthHex if there are leading zeroes.
                    for (int index = 1; index < Parsers.ByteOverflowLengthHex; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            value = (byte)(parsedValue);
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                    for (int index = Parsers.ByteOverflowLengthHex; index < length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            value = (byte)(parsedValue);
                            return true;
                        }
                        // If we try to append a digit to anything larger than byte.MaxValue / 0x10, there will be overflow
                        if (parsedValue > byte.MaxValue / 0x10)
                        {
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }

                value = (byte)(parsedValue);
                return true;
            }

            public unsafe static bool TryParseByte(char* text, int length, out byte value, out int charactersConsumed)
            {
                if (length < 1)
                {
                    charactersConsumed = 0;
                    value = default;
                    return false;
                }
                char nextCharacter;
                byte nextDigit;

                // Cache Parsers.s_HexLookup in order to avoid static constructor checks
                byte[] hexLookup = Parsers.HexLookup;

                // Parse the first digit separately. If invalid here, we need to return false.
                nextCharacter = text[0];
                nextDigit = hexLookup[(byte)nextCharacter];
                if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                {
                    charactersConsumed = 0;
                    value = default;
                    return false;
                }
                uint parsedValue = nextDigit;

                if (length <= Parsers.ByteOverflowLengthHex)
                {
                    // Length is less than or equal to Parsers.ByteOverflowLengthHex; overflow is not possible
                    for (int index = 1; index < length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            charactersConsumed = index;
                            value = (byte)(parsedValue);
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Parsers.ByteOverflowLengthHex; overflow is only possible after Parsers.ByteOverflowLengthHex
                    // digits. There may be no overflow after Parsers.ByteOverflowLengthHex if there are leading zeroes.
                    for (int index = 1; index < Parsers.ByteOverflowLengthHex; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            charactersConsumed = index;
                            value = (byte)(parsedValue);
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                    for (int index = Parsers.ByteOverflowLengthHex; index < length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            charactersConsumed = index;
                            value = (byte)(parsedValue);
                            return true;
                        }
                        // If we try to append a digit to anything larger than byte.MaxValue / 0x10, there will be overflow
                        if (parsedValue > byte.MaxValue / 0x10)
                        {
                            charactersConsumed = 0;
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
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
                char nextCharacter;
                byte nextDigit;

                // Cache Parsers.s_HexLookup in order to avoid static constructor checks
                byte[] hexLookup = Parsers.HexLookup;

                // Parse the first digit separately. If invalid here, we need to return false.
                nextCharacter = text[0];
                nextDigit = hexLookup[(byte)nextCharacter];
                if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                {
                    value = default;
                    return false;
                }
                uint parsedValue = nextDigit;

                if (text.Length <= Parsers.ByteOverflowLengthHex)
                {
                    // Length is less than or equal to Parsers.ByteOverflowLengthHex; overflow is not possible
                    for (int index = 1; index < text.Length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            value = (byte)(parsedValue);
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Parsers.ByteOverflowLengthHex; overflow is only possible after Parsers.ByteOverflowLengthHex
                    // digits. There may be no overflow after Parsers.ByteOverflowLengthHex if there are leading zeroes.
                    for (int index = 1; index < Parsers.ByteOverflowLengthHex; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            value = (byte)(parsedValue);
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                    for (int index = Parsers.ByteOverflowLengthHex; index < text.Length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            value = (byte)(parsedValue);
                            return true;
                        }
                        // If we try to append a digit to anything larger than byte.MaxValue / 0x10, there will be overflow
                        if (parsedValue > byte.MaxValue / 0x10)
                        {
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
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
                char nextCharacter;
                byte nextDigit;

                // Cache Parsers.s_HexLookup in order to avoid static constructor checks
                byte[] hexLookup = Parsers.HexLookup;

                // Parse the first digit separately. If invalid here, we need to return false.
                nextCharacter = text[0];
                nextDigit = hexLookup[(byte)nextCharacter];
                if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                {
                    charactersConsumed = 0;
                    value = default;
                    return false;
                }
                uint parsedValue = nextDigit;

                if (text.Length <= Parsers.ByteOverflowLengthHex)
                {
                    // Length is less than or equal to Parsers.ByteOverflowLengthHex; overflow is not possible
                    for (int index = 1; index < text.Length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            charactersConsumed = index;
                            value = (byte)(parsedValue);
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Parsers.ByteOverflowLengthHex; overflow is only possible after Parsers.ByteOverflowLengthHex
                    // digits. There may be no overflow after Parsers.ByteOverflowLengthHex if there are leading zeroes.
                    for (int index = 1; index < Parsers.ByteOverflowLengthHex; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            charactersConsumed = index;
                            value = (byte)(parsedValue);
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                    for (int index = Parsers.ByteOverflowLengthHex; index < text.Length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            charactersConsumed = index;
                            value = (byte)(parsedValue);
                            return true;
                        }
                        // If we try to append a digit to anything larger than byte.MaxValue / 0x10, there will be overflow
                        if (parsedValue > byte.MaxValue / 0x10)
                        {
                            charactersConsumed = 0;
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }

                charactersConsumed = text.Length;
                value = (byte)(parsedValue);
                return true;
            }

            #endregion

            #region UInt16
            public unsafe static bool TryParseUInt16(char* text, int length, out ushort value)
            {
                if (length < 1)
                {
                    value = default;
                    return false;
                }
                char nextCharacter;
                byte nextDigit;

                // Cache Parsers.s_HexLookup in order to avoid static constructor checks
                byte[] hexLookup = Parsers.HexLookup;

                // Parse the first digit separately. If invalid here, we need to return false.
                nextCharacter = text[0];
                nextDigit = hexLookup[(byte)nextCharacter];
                if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                {
                    value = default;
                    return false;
                }
                uint parsedValue = nextDigit;

                if (length <= Parsers.Int16OverflowLengthHex)
                {
                    // Length is less than or equal to Parsers.Int16OverflowLengthHex; overflow is not possible
                    for (int index = 1; index < length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            value = (ushort)(parsedValue);
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Parsers.Int16OverflowLengthHex; overflow is only possible after Parsers.Int16OverflowLengthHex
                    // digits. There may be no overflow after Parsers.Int16OverflowLengthHex if there are leading zeroes.
                    for (int index = 1; index < Parsers.Int16OverflowLengthHex; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            value = (ushort)(parsedValue);
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                    for (int index = Parsers.Int16OverflowLengthHex; index < length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            value = (ushort)(parsedValue);
                            return true;
                        }
                        // If we try to append a digit to anything larger than ushort.MaxValue / 0x10, there will be overflow
                        if (parsedValue > ushort.MaxValue / 0x10)
                        {
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }

                value = (ushort)(parsedValue);
                return true;
            }

            public unsafe static bool TryParseUInt16(char* text, int length, out ushort value, out int charactersConsumed)
            {
                if (length < 1)
                {
                    charactersConsumed = 0;
                    value = default;
                    return false;
                }
                char nextCharacter;
                byte nextDigit;

                // Cache Parsers.s_HexLookup in order to avoid static constructor checks
                byte[] hexLookup = Parsers.HexLookup;

                // Parse the first digit separately. If invalid here, we need to return false.
                nextCharacter = text[0];
                nextDigit = hexLookup[(byte)nextCharacter];
                if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                {
                    charactersConsumed = 0;
                    value = default;
                    return false;
                }
                uint parsedValue = nextDigit;

                if (length <= Parsers.Int16OverflowLengthHex)
                {
                    // Length is less than or equal to Parsers.Int16OverflowLengthHex; overflow is not possible
                    for (int index = 1; index < length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            charactersConsumed = index;
                            value = (ushort)(parsedValue);
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Parsers.Int16OverflowLengthHex; overflow is only possible after Parsers.Int16OverflowLengthHex
                    // digits. There may be no overflow after Parsers.Int16OverflowLengthHex if there are leading zeroes.
                    for (int index = 1; index < Parsers.Int16OverflowLengthHex; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            charactersConsumed = index;
                            value = (ushort)(parsedValue);
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                    for (int index = Parsers.Int16OverflowLengthHex; index < length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            charactersConsumed = index;
                            value = (ushort)(parsedValue);
                            return true;
                        }
                        // If we try to append a digit to anything larger than ushort.MaxValue / 0x10, there will be overflow
                        if (parsedValue > ushort.MaxValue / 0x10)
                        {
                            charactersConsumed = 0;
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
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
                char nextCharacter;
                byte nextDigit;

                // Cache Parsers.s_HexLookup in order to avoid static constructor checks
                byte[] hexLookup = Parsers.HexLookup;

                // Parse the first digit separately. If invalid here, we need to return false.
                nextCharacter = text[0];
                nextDigit = hexLookup[(byte)nextCharacter];
                if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                {
                    value = default;
                    return false;
                }
                uint parsedValue = nextDigit;

                if (text.Length <= Parsers.Int16OverflowLengthHex)
                {
                    // Length is less than or equal to Parsers.Int16OverflowLengthHex; overflow is not possible
                    for (int index = 1; index < text.Length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            value = (ushort)(parsedValue);
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Parsers.Int16OverflowLengthHex; overflow is only possible after Parsers.Int16OverflowLengthHex
                    // digits. There may be no overflow after Parsers.Int16OverflowLengthHex if there are leading zeroes.
                    for (int index = 1; index < Parsers.Int16OverflowLengthHex; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            value = (ushort)(parsedValue);
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                    for (int index = Parsers.Int16OverflowLengthHex; index < text.Length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            value = (ushort)(parsedValue);
                            return true;
                        }
                        // If we try to append a digit to anything larger than ushort.MaxValue / 0x10, there will be overflow
                        if (parsedValue > ushort.MaxValue / 0x10)
                        {
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
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
                char nextCharacter;
                byte nextDigit;

                // Cache Parsers.s_HexLookup in order to avoid static constructor checks
                byte[] hexLookup = Parsers.HexLookup;

                // Parse the first digit separately. If invalid here, we need to return false.
                nextCharacter = text[0];
                nextDigit = hexLookup[(byte)nextCharacter];
                if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                {
                    charactersConsumed = 0;
                    value = default;
                    return false;
                }
                uint parsedValue = nextDigit;

                if (text.Length <= Parsers.Int16OverflowLengthHex)
                {
                    // Length is less than or equal to Parsers.Int16OverflowLengthHex; overflow is not possible
                    for (int index = 1; index < text.Length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            charactersConsumed = index;
                            value = (ushort)(parsedValue);
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Parsers.Int16OverflowLengthHex; overflow is only possible after Parsers.Int16OverflowLengthHex
                    // digits. There may be no overflow after Parsers.Int16OverflowLengthHex if there are leading zeroes.
                    for (int index = 1; index < Parsers.Int16OverflowLengthHex; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            charactersConsumed = index;
                            value = (ushort)(parsedValue);
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                    for (int index = Parsers.Int16OverflowLengthHex; index < text.Length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            charactersConsumed = index;
                            value = (ushort)(parsedValue);
                            return true;
                        }
                        // If we try to append a digit to anything larger than ushort.MaxValue / 0x10, there will be overflow
                        if (parsedValue > ushort.MaxValue / 0x10)
                        {
                            charactersConsumed = 0;
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }

                charactersConsumed = text.Length;
                value = (ushort)(parsedValue);
                return true;
            }

            #endregion

            #region UInt32
            public unsafe static bool TryParseUInt32(char* text, int length, out uint value)
            {
                if (length < 1)
                {
                    value = default;
                    return false;
                }
                char nextCharacter;
                byte nextDigit;

                // Cache Parsers.s_HexLookup in order to avoid static constructor checks
                byte[] hexLookup = Parsers.HexLookup;

                // Parse the first digit separately. If invalid here, we need to return false.
                nextCharacter = text[0];
                nextDigit = hexLookup[(byte)nextCharacter];
                if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                {
                    value = default;
                    return false;
                }
                uint parsedValue = nextDigit;

                if (length <= Parsers.Int32OverflowLengthHex)
                {
                    // Length is less than or equal to Parsers.Int32OverflowLengthHex; overflow is not possible
                    for (int index = 1; index < length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            value = parsedValue;
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Parsers.Int32OverflowLengthHex; overflow is only possible after Parsers.Int32OverflowLengthHex
                    // digits. There may be no overflow after Parsers.Int32OverflowLengthHex if there are leading zeroes.
                    for (int index = 1; index < Parsers.Int32OverflowLengthHex; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            value = parsedValue;
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                    for (int index = Parsers.Int32OverflowLengthHex; index < length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            value = parsedValue;
                            return true;
                        }
                        // If we try to append a digit to anything larger than uint.MaxValue / 0x10, there will be overflow
                        if (parsedValue > uint.MaxValue / 0x10)
                        {
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }

                value = parsedValue;
                return true;
            }

            public unsafe static bool TryParseUInt32(char* text, int length, out uint value, out int charactersConsumed)
            {
                if (length < 1)
                {
                    charactersConsumed = 0;
                    value = default;
                    return false;
                }
                char nextCharacter;
                byte nextDigit;

                // Cache Parsers.s_HexLookup in order to avoid static constructor checks
                byte[] hexLookup = Parsers.HexLookup;

                // Parse the first digit separately. If invalid here, we need to return false.
                nextCharacter = text[0];
                nextDigit = hexLookup[(byte)nextCharacter];
                if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                {
                    charactersConsumed = 0;
                    value = default;
                    return false;
                }
                uint parsedValue = nextDigit;

                if (length <= Parsers.Int32OverflowLengthHex)
                {
                    // Length is less than or equal to Parsers.Int32OverflowLengthHex; overflow is not possible
                    for (int index = 1; index < length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            charactersConsumed = index;
                            value = parsedValue;
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Parsers.Int32OverflowLengthHex; overflow is only possible after Parsers.Int32OverflowLengthHex
                    // digits. There may be no overflow after Parsers.Int32OverflowLengthHex if there are leading zeroes.
                    for (int index = 1; index < Parsers.Int32OverflowLengthHex; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            charactersConsumed = index;
                            value = parsedValue;
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                    for (int index = Parsers.Int32OverflowLengthHex; index < length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            charactersConsumed = index;
                            value = parsedValue;
                            return true;
                        }
                        // If we try to append a digit to anything larger than uint.MaxValue / 0x10, there will be overflow
                        if (parsedValue > uint.MaxValue / 0x10)
                        {
                            charactersConsumed = 0;
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
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
                char nextCharacter;
                byte nextDigit;

                // Cache Parsers.s_HexLookup in order to avoid static constructor checks
                byte[] hexLookup = Parsers.HexLookup;

                // Parse the first digit separately. If invalid here, we need to return false.
                nextCharacter = text[0];
                nextDigit = hexLookup[(byte)nextCharacter];
                if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                {
                    value = default;
                    return false;
                }
                uint parsedValue = nextDigit;

                if (text.Length <= Parsers.Int32OverflowLengthHex)
                {
                    // Length is less than or equal to Parsers.Int32OverflowLengthHex; overflow is not possible
                    for (int index = 1; index < text.Length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            value = parsedValue;
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Parsers.Int32OverflowLengthHex; overflow is only possible after Parsers.Int32OverflowLengthHex
                    // digits. There may be no overflow after Parsers.Int32OverflowLengthHex if there are leading zeroes.
                    for (int index = 1; index < Parsers.Int32OverflowLengthHex; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            value = parsedValue;
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                    for (int index = Parsers.Int32OverflowLengthHex; index < text.Length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            value = parsedValue;
                            return true;
                        }
                        // If we try to append a digit to anything larger than uint.MaxValue / 0x10, there will be overflow
                        if (parsedValue > uint.MaxValue / 0x10)
                        {
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
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
                char nextCharacter;
                byte nextDigit;

                // Cache Parsers.s_HexLookup in order to avoid static constructor checks
                byte[] hexLookup = Parsers.HexLookup;

                // Parse the first digit separately. If invalid here, we need to return false.
                nextCharacter = text[0];
                nextDigit = hexLookup[(byte)nextCharacter];
                if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                {
                    charactersConsumed = 0;
                    value = default;
                    return false;
                }
                uint parsedValue = nextDigit;

                if (text.Length <= Parsers.Int32OverflowLengthHex)
                {
                    // Length is less than or equal to Parsers.Int32OverflowLengthHex; overflow is not possible
                    for (int index = 1; index < text.Length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            charactersConsumed = index;
                            value = parsedValue;
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Parsers.Int32OverflowLengthHex; overflow is only possible after Parsers.Int32OverflowLengthHex
                    // digits. There may be no overflow after Parsers.Int32OverflowLengthHex if there are leading zeroes.
                    for (int index = 1; index < Parsers.Int32OverflowLengthHex; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            charactersConsumed = index;
                            value = parsedValue;
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                    for (int index = Parsers.Int32OverflowLengthHex; index < text.Length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            charactersConsumed = index;
                            value = parsedValue;
                            return true;
                        }
                        // If we try to append a digit to anything larger than uint.MaxValue / 0x10, there will be overflow
                        if (parsedValue > uint.MaxValue / 0x10)
                        {
                            charactersConsumed = 0;
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }

                charactersConsumed = text.Length;
                value = parsedValue;
                return true;
            }

            #endregion

            #region UInt64
            public unsafe static bool TryParseUInt64(char* text, int length, out ulong value)
            {
                if (length < 1)
                {
                    value = default;
                    return false;
                }
                char nextCharacter;
                byte nextDigit;

                // Cache Parsers.s_HexLookup in order to avoid static constructor checks
                byte[] hexLookup = Parsers.HexLookup;

                // Parse the first digit separately. If invalid here, we need to return false.
                nextCharacter = text[0];
                nextDigit = hexLookup[(byte)nextCharacter];
                if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                {
                    value = default;
                    return false;
                }
                ulong parsedValue = nextDigit;

                if (length <= Parsers.Int64OverflowLengthHex)
                {
                    // Length is less than or equal to Parsers.Int64OverflowLengthHex; overflow is not possible
                    for (int index = 1; index < length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            value = parsedValue;
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Parsers.Int64OverflowLengthHex; overflow is only possible after Parsers.Int64OverflowLengthHex
                    // digits. There may be no overflow after Parsers.Int64OverflowLengthHex if there are leading zeroes.
                    for (int index = 1; index < Parsers.Int64OverflowLengthHex; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            value = parsedValue;
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                    for (int index = Parsers.Int64OverflowLengthHex; index < length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            value = parsedValue;
                            return true;
                        }
                        // If we try to append a digit to anything larger than ulong.MaxValue / 0x10, there will be overflow
                        if (parsedValue > ulong.MaxValue / 0x10)
                        {
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }

                value = parsedValue;
                return true;
            }

            public unsafe static bool TryParseUInt64(char* text, int length, out ulong value, out int charactersConsumed)
            {
                if (length < 1)
                {
                    charactersConsumed = 0;
                    value = default;
                    return false;
                }
                char nextCharacter;
                byte nextDigit;

                // Cache Parsers.s_HexLookup in order to avoid static constructor checks
                byte[] hexLookup = Parsers.HexLookup;

                // Parse the first digit separately. If invalid here, we need to return false.
                nextCharacter = text[0];
                nextDigit = hexLookup[(byte)nextCharacter];
                if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                {
                    charactersConsumed = 0;
                    value = default;
                    return false;
                }
                ulong parsedValue = nextDigit;

                if (length <= Parsers.Int64OverflowLengthHex)
                {
                    // Length is less than or equal to Parsers.Int64OverflowLengthHex; overflow is not possible
                    for (int index = 1; index < length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            charactersConsumed = index;
                            value = parsedValue;
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Parsers.Int64OverflowLengthHex; overflow is only possible after Parsers.Int64OverflowLengthHex
                    // digits. There may be no overflow after Parsers.Int64OverflowLengthHex if there are leading zeroes.
                    for (int index = 1; index < Parsers.Int64OverflowLengthHex; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            charactersConsumed = index;
                            value = parsedValue;
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                    for (int index = Parsers.Int64OverflowLengthHex; index < length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            charactersConsumed = index;
                            value = parsedValue;
                            return true;
                        }
                        // If we try to append a digit to anything larger than ulong.MaxValue / 0x10, there will be overflow
                        if (parsedValue > ulong.MaxValue / 0x10)
                        {
                            charactersConsumed = 0;
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
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
                char nextCharacter;
                byte nextDigit;

                // Cache Parsers.s_HexLookup in order to avoid static constructor checks
                byte[] hexLookup = Parsers.HexLookup;

                // Parse the first digit separately. If invalid here, we need to return false.
                nextCharacter = text[0];
                nextDigit = hexLookup[(byte)nextCharacter];
                if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                {
                    value = default;
                    return false;
                }
                ulong parsedValue = nextDigit;

                if (text.Length <= Parsers.Int64OverflowLengthHex)
                {
                    // Length is less than or equal to Parsers.Int64OverflowLengthHex; overflow is not possible
                    for (int index = 1; index < text.Length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            value = parsedValue;
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Parsers.Int64OverflowLengthHex; overflow is only possible after Parsers.Int64OverflowLengthHex
                    // digits. There may be no overflow after Parsers.Int64OverflowLengthHex if there are leading zeroes.
                    for (int index = 1; index < Parsers.Int64OverflowLengthHex; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            value = parsedValue;
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                    for (int index = Parsers.Int64OverflowLengthHex; index < text.Length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            value = parsedValue;
                            return true;
                        }
                        // If we try to append a digit to anything larger than ulong.MaxValue / 0x10, there will be overflow
                        if (parsedValue > ulong.MaxValue / 0x10)
                        {
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
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
                char nextCharacter;
                byte nextDigit;

                // Cache Parsers.s_HexLookup in order to avoid static constructor checks
                byte[] hexLookup = Parsers.HexLookup;

                // Parse the first digit separately. If invalid here, we need to return false.
                nextCharacter = text[0];
                nextDigit = hexLookup[(byte)nextCharacter];
                if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                {
                    charactersConsumed = 0;
                    value = default;
                    return false;
                }
                ulong parsedValue = nextDigit;

                if (text.Length <= Parsers.Int64OverflowLengthHex)
                {
                    // Length is less than or equal to Parsers.Int64OverflowLengthHex; overflow is not possible
                    for (int index = 1; index < text.Length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            charactersConsumed = index;
                            value = parsedValue;
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }
                else
                {
                    // Length is greater than Parsers.Int64OverflowLengthHex; overflow is only possible after Parsers.Int64OverflowLengthHex
                    // digits. There may be no overflow after Parsers.Int64OverflowLengthHex if there are leading zeroes.
                    for (int index = 1; index < Parsers.Int64OverflowLengthHex; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            charactersConsumed = index;
                            value = parsedValue;
                            return true;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                    for (int index = Parsers.Int64OverflowLengthHex; index < text.Length; index++)
                    {
                        nextCharacter = text[index];
                        nextDigit = hexLookup[(byte)nextCharacter];
                        if (nextDigit == 0xFF || (nextCharacter >> 8) != 0)
                        {
                            charactersConsumed = index;
                            value = parsedValue;
                            return true;
                        }
                        // If we try to append a digit to anything larger than ulong.MaxValue / 0x10, there will be overflow
                        if (parsedValue > ulong.MaxValue / 0x10)
                        {
                            charactersConsumed = 0;
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }

                charactersConsumed = text.Length;
                value = parsedValue;
                return true;
            }

            #endregion

        }
    }
    
}
