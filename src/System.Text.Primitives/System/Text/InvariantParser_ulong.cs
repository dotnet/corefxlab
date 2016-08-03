// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class InvariantParser
    {      
        public static bool TryParse(Utf8String utf8Text, out ulong value, out int bytesConsumed)
        {
            // Precondition replacement
            if (utf8Text.Length < 1)
            {
                value = 0;
                bytesConsumed = 0;
                return false;
            }

            value = 0;
            bytesConsumed = 0;

            for (int byteIndex = 0; byteIndex < utf8Text.Length; byteIndex++)
            {
                byte nextByteVal = (byte)((byte)utf8Text[byteIndex] - '0');
                if (nextByteVal > 9)
                {
                    if (bytesConsumed == 0)
                    {
                        value = default(ulong);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else if (value > UInt64.MaxValue / 10) // overflow
                {
                    value = 0;
                    bytesConsumed = 0;
                    return false;
                }
                else if (UInt64.MaxValue - value * 10 < (ulong)(nextByteVal)) // overflow
                {
                    value = 0;
                    bytesConsumed = 0;
                    return false;
                }
                ulong candidate = value * 10 + nextByteVal ;

                value = candidate;
                bytesConsumed++;
            }

            return true;
        }
        /// <summary>
        /// Parses an unsigned 64-bit integer from a location within a UTF-8 byte array buffer.
        /// </summary>
        /// <param name="utf8Text">The UTF-8 buffer, passed as a byte array.</param>
        /// <param name="index">The index location of the value to be parsed within the buffer.</param>
        /// <param name="value">The parsed value.</param>
        /// <param name="bytesConsumed">The length (in bytes) of the unparsed value within the UTF-8 buffer.</param>
        /// <returns>True if parsing is successful; false otherwise.</returns>
		public static bool TryParse(byte[] utf8Text, int index, out ulong value, out int bytesConsumed)
        {
            // Precondition replacement
            if (utf8Text.Length < 1 || index < 0 || index >= utf8Text.Length)
            {
                value = 0;
                bytesConsumed = 0;
                return false;
            }

            value = 0;
            bytesConsumed = 0;

            for (int byteIndex = index; byteIndex < utf8Text.Length; byteIndex++) // loop through the byte array
            {
                byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                if (nextByteVal > 9) // if the next character is not a digit
                {
                    if (bytesConsumed == 0) // check to see if we've processed any digits at all
                    {
                        value = default(ulong); // if we haven't, set value to 0 and return false
                        return false;
                    }
                    else
                    {
                        return true; // otherwise return true
                    }
                }
                else if (value > UInt64.MaxValue / 10) // overflow
                {
                    value = 0;
                    bytesConsumed = 0;
                    return false;
                }
                else if (value > 0 && UInt64.MaxValue - value * 10 < (ulong)(nextByteVal)) // overflow
                {
                    value = 0;
                    bytesConsumed = 0;
                    return false;
                }
                ulong candidate = value * 10 + nextByteVal;

                value = candidate;
                bytesConsumed++;
            }

            return true;
        }
        unsafe public static bool TryParse(byte *utf8Text, int index, int length, out ulong value, out int bytesConsumed)
        {
            // Precondition replacement
            if (length < 1 || index < 0)
            {
                value = 0;
                bytesConsumed = 0;
                return false;
            }

            value = 0;
            bytesConsumed = 0;

            for (int byteIndex = index; byteIndex < length + index; byteIndex++)
            {
                byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                if (nextByteVal > 9)
                {
                    if (bytesConsumed == 0)
                    {
                        value = default(ulong);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else if (value > UInt64.MaxValue / 10) // overflow
                {
                    value = 0;
                    bytesConsumed = 0;
                    return false;
                }
                else if (value > 0 && UInt64.MaxValue - value * 10 < (ulong)(nextByteVal)) // overflow
                {
                    value = 0;
                    bytesConsumed = 0;
                    return false;
                }
                ulong candidate = value * 10 + nextByteVal;

                value = candidate;
                bytesConsumed++;
            }
            return true;
        }
    }
}
