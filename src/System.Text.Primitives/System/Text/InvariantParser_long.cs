using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class InvariantParser
    {
        /// <summary>
        /// Parses an unsigned 64-bit integer from a location within a UTF-8 byte array buffer.
        /// </summary>
        /// <param name="utf8Text">The UTF-8 buffer, passed as a byte array.</param>
        /// <param name="index">The index location of the value to be parsed within the buffer.</param>
        /// <param name="value">The parsed value.</param>
        /// <param name="bytesConsumed">The length (in bytes) of the unparsed value within the UTF-8 buffer.</param>
        /// <returns>True if parsing is successful; false otherwise.</returns>
		public static bool TryParse(byte[] utf8Text, int index, out long value, out int bytesConsumed)
        {
            Precondition.Require(utf8Text.Length > 0);

            value = 0;
            bytesConsumed = 0;
            bool negative = false;
            bool signed = false;

            if (utf8Text[index] == '-')
            {
                negative = true;
                signed = true;
                index++;
                bytesConsumed++;
            }
            else if (utf8Text[index] == '+')
            {
                signed = true;
                index++;
                bytesConsumed++;
            }

            for (int byteIndex = index; byteIndex < utf8Text.Length; byteIndex++) // loop through the byte array
            {
                byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                if (nextByteVal > 9) // if the next character is not a digit
                {
                    if (bytesConsumed == 1 && signed) // if the first character happened to be a '-' or a '+', we reset the byte counter so logic proceeds as normal.
                    {
                        bytesConsumed = 0;
                    }
                    if (bytesConsumed == 0) // check to see if we've processed any digits at all
                    {
                        value = default(int); // if we haven't, set value to 0 and return false
                        return false;
                    }
                    else
                    {
                        if (negative && value != Int64.MinValue) // We check if the value is negative at the very end to save on comp time
                        {
                            value = -value;
                        }
                        return true; // otherwise return true
                    }
                }
                else if (value > Int64.MaxValue / 10) // overflow
                {
                    value = 0;
                    bytesConsumed = 0;
                    return false;
                }
                else if (value > 0 && (Int64.MaxValue - value * 10) + (negative ? 1 : 0) < nextByteVal) // overflow (+1 allows for overflow for min value)
                {
                    value = 0;
                    bytesConsumed = 0;
                    return false;
                }
                long candidate = value * 10 + (nextByteVal); // parse the current digit to a long and add it to the left-shifted 
                value = candidate;
                bytesConsumed++; // increment the number of bytes consumed, then loop
            }

            if (negative) // We check if the value is negative at the very end to save on comp time
            {
                value = -value;
            }
            return true;
        }

        unsafe public static bool TryParse(byte* utf8Text, int index, int length, out long value, out int bytesConsumed)
        {
            value = 0;
            bytesConsumed = 0;
            bool negative = false;
            bool signed = false;

            if (utf8Text[index] == '-')
            {
                negative = true;
                signed = true;
                index++;
                bytesConsumed++;
            }
            else if (utf8Text[index] == '+')
            {
                signed = true;
                index++;
                bytesConsumed++;
            }

            for (int byteIndex = index; byteIndex < length + index; byteIndex++)
            {
                byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                if (nextByteVal > 9)
                {
                    if (bytesConsumed == 1 && signed) // if the first character happened to be a '-', we reset the byte counter so logic proceeds as normal.
                    {
                        bytesConsumed = 0;
                    }
                    if (bytesConsumed == 0)
                    {
                        value = default(int);
                        return false;
                    }
                    else
                    {
                        if (negative && value != Int64.MinValue) // We check if the value is negative at the very end to save on comp time
                        {
                            value = -value;
                        }
                        return true; // otherwise return true
                    }
                }
                else if (value > Int64.MaxValue / 10) // overflow
                {
                    value = 0;
                    bytesConsumed = 0;
                    return false;
                }
                else if (value > 0 && (Int64.MaxValue - value * 10) + (negative ? 1 : 0) < nextByteVal) // overflow (+1 allows for overflow for min value)
                {
                    value = 0;
                    bytesConsumed = 0;
                    return false;
                }
                long candidate = value * 10 + (nextByteVal); // parse the current digit to a long and add it to the left-shifted 
                value = candidate;
                bytesConsumed++; // increment the number of bytes consumed, then loop
            }
            return true;
        }
    }
}
