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
		public static bool TryParse(byte[] utf8Text, int index, out ushort value, out int bytesConsumed)
        {
            Precondition.Require(utf8Text.Length > 0);

            value = 0;
            bytesConsumed = 0;

            for (int byteIndex = index; byteIndex < utf8Text.Length; byteIndex++) // loop through the byte array
            {
                byte nextByte = utf8Text[byteIndex];
                if (nextByte < '0' || nextByte > '9') // if the next character is not a digit
                {
                    if (bytesConsumed == 0) // check to see if we've processed any digits at all
                    {
                        value = default(ushort); // if we haven't, set value to 0 and return false
                        return false;
                    }
                    else
                    {
                        return true; // otherwise return true
                    }
                }
                ushort candidate = (ushort)(value * 10); // left shift the value
                candidate += (ushort)(nextByte - '0'); // parse the current digit to a ushort and add it to the temporary value
                if (candidate >= value) // if it was a digit 0-9, this should be true
                {
                    value = candidate;
                }
                else // this should never happen, but just in case
                {
                    return true;
                }
                bytesConsumed++; // increment the number of bytes consumed, then loop
            }

            return true;
        }
        unsafe public static bool TryParse(byte* utf8Text, int index, int length, out ushort value, out int bytesConsumed)
        {
            value = 0;
            bytesConsumed = 0;

            for (int byteIndex = index; byteIndex < length + index; byteIndex++)
            {
                byte nextByte = utf8Text[byteIndex];
                if (nextByte < '0' || nextByte > '9')
                {
                    if (bytesConsumed == 0)
                    {
                        value = default(ushort);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                ushort candidate = (ushort)(value * 10);
                candidate += (ushort)(nextByte - '0');
                if (candidate >= value)
                {
                    value = candidate;
                }
                bytesConsumed++;
            }
            return true;
        }
    }
}
