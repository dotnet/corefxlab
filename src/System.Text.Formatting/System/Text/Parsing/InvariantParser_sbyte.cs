﻿using System.Diagnostics;
using System.Text.Formatting;
using System.Text.Utf8;

namespace System.Text.Parsing
{
    public static partial class InvariantParser
    {
        /// <summary>
        /// Parses an unsigned 64-bit integer from a location within a UTF-8 byte array buffer.
        /// </summary>
        /// <param name="text">The UTF-8 buffer, passed as a byte array.</param>
        /// <param name="startIndex">The index location of the value to be parsed within the buffer.</param>
        /// <param name="value">The parsed value.</param>
        /// <param name="bytesConsumed">The length (in bytes) of the unparsed value within the UTF-8 buffer.</param>
        /// <returns>True if parsing is successful; false otherwise.</returns>
		public static bool TryParse(byte[] text, int startIndex, out sbyte value, out int bytesConsumed)
        {
            Precondition.Require(text.Length > 0);

            value = 0;
            bytesConsumed = 0;
            bool negative = false;

            if (text[startIndex] == '-')
            {
                negative = true;
                startIndex++;
                bytesConsumed++;
            }

            for (int byteIndex = startIndex; byteIndex < text.Length; byteIndex++) // loop through the byte array
            {
                if (text[byteIndex] < '0' || text[byteIndex] > '9') // if the next character is not a digit
                {
                    if (bytesConsumed == 0) // check to see if we've processed any digits at all
                    {
                        value = default(int); // if we haven't, set value to 0 and return false
                        return false;
                    }
                    else
                    {
                        if (negative) // We check if the value is negative at the very end to save on comp time
                        {
                            value *= -1;
                        }
                        return true; // otherwise return true
                    }
                }
                sbyte candidate = (sbyte)(value * 10); // left shift the value
                candidate += (sbyte)(text[byteIndex] - '0'); // parse the current digit to a sbyte and add it to the temporary value
                if (candidate >= value) // if it was a digit 0-9, this should be true
                {
                    value = candidate;
                }
                else // for signed types this will occur at the min values as overflow occurs during addition, so we handle that
                {
                    if (candidate == sbyte.MinValue)
                    {
                        bytesConsumed++;
                        value = candidate;
                        return true;
                    }
                    if (negative) // We check if the value is negative at the very end to save on comp time
                    {
                        value *= -1;
                    }
                    return true;
                }
                bytesConsumed++; // increment the number of bytes consumed, then loop
            }

            if (negative) // We check if the value is negative at the very end to save on comp time
            {
                value *= -1;
            }
            return true;
        }

        unsafe public static bool TryParse(byte* text, int startIndex, out sbyte value, out int bytesConsumed)
        {
            value = 0;
            bytesConsumed = 0;
            bool negative = false;

            if (text[startIndex] == '-')
            {
                negative = true;
                startIndex++;
                bytesConsumed++;
            }

            for (int byteIndex = startIndex; ; byteIndex++)
            {
                if (text[byteIndex] < '0' || text[byteIndex] > '9')
                {
                    if (bytesConsumed == 0)
                    {
                        value = default(int);
                        return false;
                    }
                    else
                    {
                        if (negative) // We check if the value is negative at the very end to save on comp time
                        {
                            value *= -1;
                        }
                        return true;
                    }
                }
                sbyte candidate = (sbyte)(value * 10);
                candidate += (sbyte)(text[byteIndex] - '0');
                if (candidate >= value)
                {
                    value = candidate;
                }
                else // for signed types this will occur at the min values as overflow occurs during addition, so we handle that
                {
                    if (candidate == sbyte.MinValue)
                    {
                        bytesConsumed++;
                        value = candidate;
                        return true;
                    }
                    if (negative) // We check if the value is negative at the very end to save on comp time
                    {
                        value *= -1;
                    }
                    return true;
                }
                bytesConsumed++;
            }
        }
    }
}
