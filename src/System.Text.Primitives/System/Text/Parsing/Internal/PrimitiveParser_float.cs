// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text.Internal
{
    public static partial class InternalParser
    {
        public static bool TryParseSingle(byte[] text, int index, EncodingData encoding, TextFormat format,
            out float value, out int bytesConsumed)
        {
            // Precondition replacement
            if (text.Length < 1 || index < 0 || index >= text.Length)
            {
                value = 0;
                bytesConsumed = 0;
                return false;
            }

            value = 0f;
            bytesConsumed = 0;

            if (encoding.IsInvariantUtf8)
            {
                string floatString = "";
                bool decimalPlace = false, e = false, signed = false, digitLast = false, eLast = false;

                if ((text.Length - index) >= 3 && text[index] == 'N' && text[index + 1] == 'a' && text[index + 2] == 'N')
                {
                    value = float.NaN;
                    bytesConsumed = 3;
                    return true;
                }
                if (text[index] == '-' || text[index] == '+')
                {
                    signed = true;
                    floatString += (char)text[index];
                    index++;
                    bytesConsumed++;
                }
                if ((text.Length - index) >= 8 && text[index] == 'I' && text[index + 1] == 'n' &&
                    text[index + 2] == 'f' && text[index + 3] == 'i' && text[index + 4] == 'n' &&
                    text[index + 5] == 'i' && text[index + 6] == 't' && text[index + 7] == 'y')
                {
                    if (signed && text[index - 1] == '-')
                    {
                        value = float.NegativeInfinity;
                    }
                    else
                    {
                        value = float.PositiveInfinity;
                    }
                    bytesConsumed += 8;
                    return true;
                }

                for (int byteIndex = index; byteIndex < text.Length; byteIndex++)
                {
                    byte nextByte = text[byteIndex];
                    byte nextByteVal = (byte)(nextByte - '0');

                    if (nextByteVal > 9)
                    {
                        if (!decimalPlace && nextByte == '.')
                        {
                            if (digitLast)
                            {
                                digitLast = false;
                            }
                            if (eLast)
                            {
                                eLast = false;
                            }
                            bytesConsumed++;
                            decimalPlace = true;
                            floatString += (char)nextByte;
                        }
                        else if (!e && nextByte == 'e' || nextByte == 'E')
                        {
                            e = true;
                            eLast = true;
                            bytesConsumed++;
                            floatString += (char)nextByte;
                        }
                        else if (eLast && nextByte == '+' || nextByte == '-')
                        {
                            eLast = false;
                            bytesConsumed++;
                            floatString += (char)nextByte;
                        }
                        else if ((decimalPlace && signed && bytesConsumed == 2) || ((signed || decimalPlace) && bytesConsumed == 1))
                        {
                            value = 0;
                            bytesConsumed = 0;
                            return false;
                        }
                        else
                        {
                            if (float.TryParse(floatString, out value))
                            {
                                return true;
                            }
                            else
                            {
                                bytesConsumed = 0;
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (eLast)
                            eLast = false;
                        if (!digitLast)
                            digitLast = true;
                        bytesConsumed++;
                        floatString += (char)nextByte;
                    }
                }

                if ((decimalPlace && signed && bytesConsumed == 2) || ((signed || decimalPlace) && bytesConsumed == 1))
                {
                    value = 0;
                    bytesConsumed = 0;
                    return false;
                }
                else
                {
                    if (float.TryParse(floatString, out value))
                    {
                        return true;
                    }
                    else
                    {
                        bytesConsumed = 0;
                        return false;
                    }
                }
            }

            return false;
        }

        public unsafe static bool TryParseSingle(byte* text, int index, int length, EncodingData encoding,
            TextFormat format, out float value, out int bytesConsumed)
        {
            // Precondition replacement
            if (length < 1 || index < 0)
            {
                value = 0;
                bytesConsumed = 0;
                return false;
            }

            value = 0f;
            bytesConsumed = 0;

            if (encoding.IsInvariantUtf8)
            {
                string floatString = "";
                bool decimalPlace = false, e = false, signed = false, digitLast = false, eLast = false;

                if ((length) >= 3 && text[index] == 'N' && text[index + 1] == 'a' && text[index + 2] == 'N')
                {
                    value = float.NaN;
                    bytesConsumed = 3;
                    return true;
                }
                if (text[index] == '-' || text[index] == '+')
                {
                    signed = true;
                    floatString += (char)text[index];
                    index++;
                    bytesConsumed++;
                }
                if ((length - index) >= 8 && text[index] == 'I' && text[index + 1] == 'n' &&
                    text[index + 2] == 'f' && text[index + 3] == 'i' && text[index + 4] == 'n' &&
                    text[index + 5] == 'i' && text[index + 6] == 't' && text[index + 7] == 'y')
                {
                    if (signed && text[index - 1] == '-')
                    {
                        value = float.NegativeInfinity;
                    }
                    else
                    {
                        value = float.PositiveInfinity;
                    }
                    bytesConsumed += 8;
                    return true;
                }

                for (int byteIndex = index; byteIndex < length; byteIndex++)
                {
                    byte nextByte = text[byteIndex];
                    byte nextByteVal = (byte)(nextByte - '0');

                    if (nextByteVal > 9)
                    {
                        if (!decimalPlace && nextByte == '.')
                        {
                            if (digitLast)
                            {
                                digitLast = false;
                            }
                            if (eLast)
                            {
                                eLast = false;
                            }
                            bytesConsumed++;
                            decimalPlace = true;
                            floatString += (char)nextByte;
                        }
                        else if (!e && nextByte == 'e' || nextByte == 'E')
                        {
                            e = true;
                            eLast = true;
                            bytesConsumed++;
                            floatString += (char)nextByte;
                        }
                        else if (eLast && nextByte == '+' || nextByte == '-')
                        {
                            eLast = false;
                            bytesConsumed++;
                            floatString += (char)nextByte;
                        }
                        else if ((decimalPlace && signed && bytesConsumed == 2) || ((signed || decimalPlace) && bytesConsumed == 1))
                        {
                            value = 0;
                            bytesConsumed = 0;
                            return false;
                        }
                        else
                        {
                            if (float.TryParse(floatString, out value))
                            {
                                return true;
                            }
                            else
                            {
                                bytesConsumed = 0;
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (eLast)
                            eLast = false;
                        if (!digitLast)
                            digitLast = true;
                        bytesConsumed++;
                        floatString += (char)nextByte;
                    }
                }

                if ((decimalPlace && signed && bytesConsumed == 2) || ((signed || decimalPlace) && bytesConsumed == 1))
                {
                    value = 0;
                    bytesConsumed = 0;
                    return false;
                }
                else
                {
                    if (float.TryParse(floatString, out value))
                    {
                        return true;
                    }
                    else
                    {
                        bytesConsumed = 0;
                        return false;
                    }
                }
            }
            return false;
        }
    }
}
