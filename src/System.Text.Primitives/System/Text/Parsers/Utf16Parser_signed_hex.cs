// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Buffers.Text
{
    public static partial class Utf16Parser
    {
        internal static partial class Hex
        {
            #region SByte
            public unsafe static bool TryParseSByte(char* text, int length, out sbyte value)
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
                            value = (sbyte)(parsedValue);
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
                            value = (sbyte)(parsedValue);
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
                            value = (sbyte)(parsedValue);
                            return true;
                        }
                        // If we try to append a digit to anything larger than -(sbyte.MinValue / 0x08), there will be overflow
                        if (parsedValue >= -(sbyte.MinValue / 0x08))
                        {
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }

                value = (sbyte)(parsedValue);
                return true;
            }

            public unsafe static bool TryParseSByte(char* text, int length, out sbyte value, out int charactersConsumed)
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
                            value = (sbyte)(parsedValue);
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
                            value = (sbyte)(parsedValue);
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
                            value = (sbyte)(parsedValue);
                            return true;
                        }
                        // If we try to append a digit to anything larger than -(sbyte.MinValue / 0x08), there will be overflow
                        if (parsedValue >= -(sbyte.MinValue / 0x08))
                        {
                            charactersConsumed = 0;
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }

                charactersConsumed = length;
                value = (sbyte)(parsedValue);
                return true;
            }

            public static bool TryParseSByte(ReadOnlySpan<char> text, out sbyte value)
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
                            value = (sbyte)(parsedValue);
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
                            value = (sbyte)(parsedValue);
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
                            value = (sbyte)(parsedValue);
                            return true;
                        }
                        // If we try to append a digit to anything larger than -(sbyte.MinValue / 0x08), there will be overflow
                        if (parsedValue >= -(sbyte.MinValue / 0x08))
                        {
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }

                value = (sbyte)(parsedValue);
                return true;
            }

            public static bool TryParseSByte(ReadOnlySpan<char> text, out sbyte value, out int charactersConsumed)
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
                            value = (sbyte)(parsedValue);
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
                            value = (sbyte)(parsedValue);
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
                            value = (sbyte)(parsedValue);
                            return true;
                        }
                        // If we try to append a digit to anything larger than -(sbyte.MinValue / 0x08), there will be overflow
                        if (parsedValue >= -(sbyte.MinValue / 0x08))
                        {
                            charactersConsumed = 0;
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }

                charactersConsumed = text.Length;
                value = (sbyte)(parsedValue);
                return true;
            }

            #endregion

            #region Int16
            public unsafe static bool TryParseInt16(char* text, int length, out short value)
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
                            value = (short)(parsedValue);
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
                            value = (short)(parsedValue);
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
                            value = (short)(parsedValue);
                            return true;
                        }
                        // If we try to append a digit to anything larger than -(short.MinValue / 0x08), there will be overflow
                        if (parsedValue >= -(short.MinValue / 0x08))
                        {
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }

                value = (short)(parsedValue);
                return true;
            }

            public unsafe static bool TryParseInt16(char* text, int length, out short value, out int charactersConsumed)
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
                            value = (short)(parsedValue);
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
                            value = (short)(parsedValue);
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
                            value = (short)(parsedValue);
                            return true;
                        }
                        // If we try to append a digit to anything larger than -(short.MinValue / 0x08), there will be overflow
                        if (parsedValue >= -(short.MinValue / 0x08))
                        {
                            charactersConsumed = 0;
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }

                charactersConsumed = length;
                value = (short)(parsedValue);
                return true;
            }

            public static bool TryParseInt16(ReadOnlySpan<char> text, out short value)
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
                            value = (short)(parsedValue);
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
                            value = (short)(parsedValue);
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
                            value = (short)(parsedValue);
                            return true;
                        }
                        // If we try to append a digit to anything larger than -(short.MinValue / 0x08), there will be overflow
                        if (parsedValue >= -(short.MinValue / 0x08))
                        {
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }

                value = (short)(parsedValue);
                return true;
            }

            public static bool TryParseInt16(ReadOnlySpan<char> text, out short value, out int charactersConsumed)
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
                            value = (short)(parsedValue);
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
                            value = (short)(parsedValue);
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
                            value = (short)(parsedValue);
                            return true;
                        }
                        // If we try to append a digit to anything larger than -(short.MinValue / 0x08), there will be overflow
                        if (parsedValue >= -(short.MinValue / 0x08))
                        {
                            charactersConsumed = 0;
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }

                charactersConsumed = text.Length;
                value = (short)(parsedValue);
                return true;
            }

            #endregion

            #region Int32
            public unsafe static bool TryParseInt32(char* text, int length, out int value)
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
                            value = (int)(parsedValue);
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
                            value = (int)(parsedValue);
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
                            value = (int)(parsedValue);
                            return true;
                        }
                        // If we try to append a digit to anything larger than -(int.MinValue / 0x08), there will be overflow
                        if (parsedValue >= -(int.MinValue / 0x08))
                        {
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }

                value = (int)(parsedValue);
                return true;
            }

            public unsafe static bool TryParseInt32(char* text, int length, out int value, out int charactersConsumed)
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
                            value = (int)(parsedValue);
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
                            value = (int)(parsedValue);
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
                            value = (int)(parsedValue);
                            return true;
                        }
                        // If we try to append a digit to anything larger than -(int.MinValue / 0x08), there will be overflow
                        if (parsedValue >= -(int.MinValue / 0x08))
                        {
                            charactersConsumed = 0;
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }

                charactersConsumed = length;
                value = (int)(parsedValue);
                return true;
            }

            public static bool TryParseInt32(ReadOnlySpan<char> text, out int value)
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
                            value = (int)(parsedValue);
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
                            value = (int)(parsedValue);
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
                            value = (int)(parsedValue);
                            return true;
                        }
                        // If we try to append a digit to anything larger than -(int.MinValue / 0x08), there will be overflow
                        if (parsedValue >= -(int.MinValue / 0x08))
                        {
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }

                value = (int)(parsedValue);
                return true;
            }

            public static bool TryParseInt32(ReadOnlySpan<char> text, out int value, out int charactersConsumed)
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
                            value = (int)(parsedValue);
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
                            value = (int)(parsedValue);
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
                            value = (int)(parsedValue);
                            return true;
                        }
                        // If we try to append a digit to anything larger than -(int.MinValue / 0x08), there will be overflow
                        if (parsedValue >= -(int.MinValue / 0x08))
                        {
                            charactersConsumed = 0;
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }

                charactersConsumed = text.Length;
                value = (int)(parsedValue);
                return true;
            }

            #endregion

            #region Int64
            public unsafe static bool TryParseInt64(char* text, int length, out long value)
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
                            value = (long)(parsedValue);
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
                            value = (long)(parsedValue);
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
                            value = (long)(parsedValue);
                            return true;
                        }
                        // If we try to append a digit to anything larger than -(long.MinValue / 0x08), there will be overflow
                        if (parsedValue >= -(long.MinValue / 0x08))
                        {
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }

                value = (long)(parsedValue);
                return true;
            }

            public unsafe static bool TryParseInt64(char* text, int length, out long value, out int charactersConsumed)
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
                            value = (long)(parsedValue);
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
                            value = (long)(parsedValue);
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
                            value = (long)(parsedValue);
                            return true;
                        }
                        // If we try to append a digit to anything larger than -(long.MinValue / 0x08), there will be overflow
                        if (parsedValue >= -(long.MinValue / 0x08))
                        {
                            charactersConsumed = 0;
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }

                charactersConsumed = length;
                value = (long)(parsedValue);
                return true;
            }

            public static bool TryParseInt64(ReadOnlySpan<char> text, out long value)
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
                            value = (long)(parsedValue);
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
                            value = (long)(parsedValue);
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
                            value = (long)(parsedValue);
                            return true;
                        }
                        // If we try to append a digit to anything larger than -(long.MinValue / 0x08), there will be overflow
                        if (parsedValue >= -(long.MinValue / 0x08))
                        {
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }

                value = (long)(parsedValue);
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
                            value = (long)(parsedValue);
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
                            value = (long)(parsedValue);
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
                            value = (long)(parsedValue);
                            return true;
                        }
                        // If we try to append a digit to anything larger than -(long.MinValue / 0x08), there will be overflow
                        if (parsedValue >= -(long.MinValue / 0x08))
                        {
                            charactersConsumed = 0;
                            value = default;
                            return false;
                        }
                        parsedValue = (parsedValue << 4) + nextDigit;
                    }
                }

                charactersConsumed = text.Length;
                value = (long)(parsedValue);
                return true;
            }

            #endregion

        }
    }  
}
