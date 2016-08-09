// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class InvariantParser
    {
        public static bool TryParse(byte[] utf8Text, int index, out double value, out int bytesConsumed)
        {
            // Precondition replacement
            if (utf8Text.Length < 1 || index < 0 || index >= utf8Text.Length)
            {
                value = 0;
                bytesConsumed = 0;
                return false;
            }

            value = 0.0;
            bytesConsumed = 0;
            string doubleString = "";
            bool decimalPlace = false, e = false, signed = false, digitLast = false, eLast = false;

            if ((utf8Text.Length - index) >= 3 && utf8Text[index] == 'N' && utf8Text[index + 1] == 'a' && utf8Text[index + 2] == 'N')
            {
                value = double.NaN;
                bytesConsumed = 3;
                return true;
            }
            if (utf8Text[index] == '-' || utf8Text[index] == '+')
            {
                signed = true;
                doubleString += (char)utf8Text[index];
                index++;
                bytesConsumed++;
            }
            if ((utf8Text.Length - index) >= 8 && utf8Text[index] == 'I' && utf8Text[index + 1] == 'n' &&
                utf8Text[index + 2] == 'f' && utf8Text[index + 3] == 'i' && utf8Text[index + 4] == 'n' &&
                utf8Text[index + 5] == 'i' && utf8Text[index + 6] == 't' && utf8Text[index + 7] == 'y')
            {
                if (signed && utf8Text[index - 1] == '-')
                {
                    value = double.NegativeInfinity;
                }
                else
                {
                    value = double.PositiveInfinity;
                }
                bytesConsumed += 8;
                return true;
            }

            for (int byteIndex = index; byteIndex < utf8Text.Length; byteIndex++)
            {
                byte nextByte = utf8Text[byteIndex];
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
                        doubleString += (char)nextByte;
                    }
                    else if (!e && nextByte == 'e' || nextByte == 'E')
                    {
                        e = true;
                        eLast = true;
                        bytesConsumed++;
                        doubleString += (char)nextByte;
                    }
                    else if (eLast && nextByte == '+' || nextByte == '-')
                    {
                        eLast = false;
                        bytesConsumed++;
                        doubleString += (char)nextByte;
                    }
                    else if ((decimalPlace && signed && bytesConsumed == 2) || ((signed || decimalPlace) && bytesConsumed == 1))
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else
                    {
                        if (double.TryParse(doubleString, out value))
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
                    doubleString += (char)nextByte;
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
                if (double.TryParse(doubleString, out value))
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

        public unsafe static bool TryParse(byte* utf8Text, int index, int length, out double value, out int bytesConsumed)
        {
            // Precondition replacement
            if (length < 1 || index < 0)
            {
                value = 0;
                bytesConsumed = 0;
                return false;
            }

            value = 0.0;
            bytesConsumed = 0;
            string doubleString = "";
            bool decimalPlace = false, e = false, signed = false, digitLast = false, eLast = false;

            if ((length) >= 3 && utf8Text[index] == 'N' && utf8Text[index + 1] == 'a' && utf8Text[index + 2] == 'N')
            {
                value = double.NaN;
                bytesConsumed = 3;
                return true;
            }
            if (utf8Text[index] == '-' || utf8Text[index] == '+')
            {
                signed = true;
                doubleString += (char)utf8Text[index];
                index++;
                bytesConsumed++;
            }
            if ((length - index) >= 8 && utf8Text[index] == 'I' && utf8Text[index + 1] == 'n' &&
                utf8Text[index + 2] == 'f' && utf8Text[index + 3] == 'i' && utf8Text[index + 4] == 'n' &&
                utf8Text[index + 5] == 'i' && utf8Text[index + 6] == 't' && utf8Text[index + 7] == 'y')
            {
                if (signed && utf8Text[index - 1] == '-')
                {
                    value = double.NegativeInfinity;
                }
                else
                {
                    value = double.PositiveInfinity;
                }
                bytesConsumed += 8;
                return true;
            }

            for (int byteIndex = index; byteIndex < length; byteIndex++)
            {
                byte nextByte = utf8Text[byteIndex];
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
                        doubleString += (char)nextByte;
                    }
                    else if (!e && nextByte == 'e' || nextByte == 'E')
                    {
                        e = true;
                        eLast = true;
                        bytesConsumed++;
                        doubleString += (char)nextByte;
                    }
                    else if (eLast && nextByte == '+' || nextByte == '-')
                    {
                        eLast = false;
                        bytesConsumed++;
                        doubleString += (char)nextByte;
                    }
                    else if ((decimalPlace && signed && bytesConsumed == 2) || ((signed || decimalPlace) && bytesConsumed == 1))
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else
                    {
                        if (double.TryParse(doubleString, out value))
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
                    doubleString += (char)nextByte;
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
                if (double.TryParse(doubleString, out value))
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
    }
}
