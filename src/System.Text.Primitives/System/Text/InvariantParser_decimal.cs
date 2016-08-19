// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class InvariantParser
    {
        public static bool TryParse(byte[] utf8Text, int index, out decimal value, out int bytesConsumed)
        {
            // Precondition replacement
            if (utf8Text.Length < 1 || index < 0 || index >= utf8Text.Length)
            {
                value = 0;
                bytesConsumed = 0;
                return false;
            }

            value = 0.0M;
            bytesConsumed = 0;
            string decimalString = "";
            bool decimalPlace = false, signed = false;

            if (utf8Text[index] == '-' || utf8Text[index] == '+')
            {
                signed = true;
                decimalString += (char)utf8Text[index];
                index++;
                bytesConsumed++;
            }

            for (int byteIndex = index; byteIndex < utf8Text.Length; byteIndex++)
            {
                byte nextByte = utf8Text[byteIndex];
                byte nextByteVal = (byte)(nextByte - '0');

                if (nextByteVal > 9)
                {
                    if (!decimalPlace && nextByte == '.')
                    {
                        bytesConsumed++;
                        decimalPlace = true;
                        decimalString += (char)nextByte;
                    }
                    else if ((decimalPlace && signed && bytesConsumed == 2) || ((signed || decimalPlace) && bytesConsumed == 1))
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else
                    {
                        if (decimal.TryParse(decimalString, out value))
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
                    bytesConsumed++;
                    decimalString += (char)nextByte;
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
                if (decimal.TryParse(decimalString, out value))
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

        public unsafe static bool TryParse(byte* utf8Text, int index, int length, out decimal value, out int bytesConsumed)
        {
            // Precondition replacement
            if (length < 1 || index < 0)
            {
                value = 0;
                bytesConsumed = 0;
                return false;
            }

            value = 0.0M;
            bytesConsumed = 0;
            string decimalString = "";
            bool decimalPlace = false, signed = false;

            if (utf8Text[index] == '-' || utf8Text[index] == '+')
            {
                signed = true;
                decimalString += (char)utf8Text[index];
                index++;
                bytesConsumed++;
            }

            for (int byteIndex = index; byteIndex < length; byteIndex++)
            {
                byte nextByte = utf8Text[byteIndex];
                byte nextByteVal = (byte)(nextByte - '0');

                if (nextByteVal > 9)
                {
                    if (!decimalPlace && nextByte == '.')
                    {
                        bytesConsumed++;
                        decimalPlace = true;
                        decimalString += (char)nextByte;
                    }
                    else if ((decimalPlace && signed && bytesConsumed == 2) || ((signed || decimalPlace) && bytesConsumed == 1))
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else
                    {
                        if (decimal.TryParse(decimalString, out value))
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
                    bytesConsumed++;
                    decimalString += (char)nextByte;
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
                if (decimal.TryParse(decimalString, out value))
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
