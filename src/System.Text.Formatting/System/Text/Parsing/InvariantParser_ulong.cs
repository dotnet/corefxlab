// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Formatting;
using System.Text.Utf8;

namespace System.Text.Parsing
{
    public static partial class InvariantParser
    {      
        public static bool TryParse(Utf8String text, out ulong value, out int bytesConsumed)
        {
            Precondition.Require(text.Length > 0);

            value = 0;
            bytesConsumed = 0;

            for (int byteIndex = 0; byteIndex < text.Length; byteIndex++)
            {
                byte nextByte = (byte)text[byteIndex];
                if (nextByte < '0' || nextByte > '9')
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
                ulong candidate = value * 10;
                candidate += (ulong)nextByte - '0';
                if (candidate >= value)
                {
                    value = candidate;
                }
                else
                {
                    return true;
                }
                bytesConsumed++;
            }

            return true;
        }
        /// <summary>
        /// Parses an unsigned 64-bit integer from a location within a UTF-8 byte array buffer.
        /// </summary>
        /// <param name="text">The UTF-8 buffer, passed as a byte array.</param>
        /// <param name="startIndex">The index location of the value to be parsed within the buffer.</param>
        /// <param name="value">The parsed value.</param>
        /// <param name="bytesConsumed">The length (in bytes) of the unparsed value within the UTF-8 buffer.</param>
        /// <returns>True if parsing is successful; false otherwise.</returns>
		public static bool TryParse(byte[] text, int startIndex, out ulong value, out int bytesConsumed)
        {
            Precondition.Require(text.Length > 0);

            value = 0;
            bytesConsumed = 0;

            for (int byteIndex = startIndex; byteIndex < text.Length; byteIndex++) // loop through the byte array
            {
                if (text[byteIndex] < '0' || text[byteIndex] > '9') // if the next character is not a digit
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
                ulong candidate = value * 10; // left shift the value
                candidate += (ulong)text[byteIndex] - '0'; // parse the current digit to a ulong and add it to the temporary value
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
        unsafe public static bool TryParse(byte *text, int startIndex, out ulong value, out int bytesConsumed)
        {
            value = 0;
            bytesConsumed = 0;

            for (int byteIndex = startIndex; ; byteIndex++)
            {
                if (text[byteIndex] < '0' || text[byteIndex] > '9')
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
                ulong candidate = value * 10;
                candidate += (ulong)text[byteIndex] - '0';
                if (candidate >= value)
                {
                    value = candidate;
                }
                else
                {
                    return true;
                }
                bytesConsumed++;
            }
        }
    }
}
