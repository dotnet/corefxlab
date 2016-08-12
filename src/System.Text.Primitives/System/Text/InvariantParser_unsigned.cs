// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class InvariantParser
    {
		#region byte
		public static bool TryParse(byte[] utf8Text, int index, FormattingData cultureAndEncodingInfo, Format.Parsed numericFormat,
            out byte value, out int bytesConsumed)
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

            if (cultureAndEncodingInfo.IsInvariantUtf8)
            {
                for (int byteIndex = index; byteIndex < utf8Text.Length; byteIndex++) // loop through the byte array
                {
                    byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                    if (nextByteVal > 9) // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
										 // to greater than 9 since byte is an unsigned type.
                    {
                        if (bytesConsumed == 0) // check to see if we've processed any digits at all
                        {
                            value = default(byte); // if we haven't, set value to 0 and return false
                            return false;
                        }
                        else
                        {
                            return true; // otherwise return true
                        }
                    }
                    else if (value > Byte.MaxValue / 10)
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else if (value > 0 && Byte.MaxValue - value * 10 < nextByteVal) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }

                    value = (byte)(value * 10 + nextByteVal); // left shift the value and add the nextByte
                    bytesConsumed++;
                }
				return true;
            }
            else if (cultureAndEncodingInfo.IsInvariantUtf16)
            {
                for (int byteIndex = index; byteIndex < utf8Text.Length - 1; byteIndex += 2) // loop through the byte array two bytes at a time for UTF-16
                {
                    byte byteAfterNext = utf8Text[byteIndex + 1];
                    byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                    if (nextByteVal > 9 || byteAfterNext != 0)  // if the second byte isn't zero, this isn't an ASCII-equivalent code unit and we can quit here
																// if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
																// to greater than 9 since byte is an unsigned type.
                    {
                        if (bytesConsumed == 0) // check to see if we've processed any digits at all
                        {
                            value = default(byte); // if we haven't, set value to 0 and return false
                            return false;
                        }
                        else
                        {
                            return true; // otherwise return true
                        }
                    }
                    else if (value > Byte.MaxValue / 10)
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else if (value > 0 && Byte.MaxValue - value * 10 < nextByteVal) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }

                    value = (byte)(value * 10 + nextByteVal); // left shift the value and add the nextByte
                    bytesConsumed += 2;
                }
                return true;
            }

            return false;
        }

		public static unsafe bool TryParse(byte* utf8Text, int index, int length, FormattingData cultureAndEncodingInfo, Format.Parsed numericFormat, 
            out byte value, out int bytesConsumed)
        {
            // Precondition replacement
            if (length < 1 || index < 0)
            {
                value = 0;
                bytesConsumed = 0;
                return false;
            }

            value = default(byte);
            bytesConsumed = 0;

            if (cultureAndEncodingInfo.IsInvariantUtf8)
            {
                for (int byteIndex = index; byteIndex < length + index; byteIndex++)
                {
                    byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                    if (nextByteVal > 9) // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
										 // to greater than 9 since byte is an unsigned type.
                    {
                        if (bytesConsumed == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else if (value > Byte.MaxValue / 10) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else if (value > 0 && Byte.MaxValue - value * 10 < nextByteVal) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    value = (byte)(value * 10 + nextByteVal); // left shift the value and add the nextByte
                    bytesConsumed++; // increment the number of bytes consumed, then loop
                }
                return true;
            }
            else if (cultureAndEncodingInfo.IsInvariantUtf16)
            {
                for (int byteIndex = index; byteIndex < length + index - 1; byteIndex += 2) // loop through the byte array two bytes at a time for UTF-16
                {
                    byte byteAfterNext = utf8Text[byteIndex + 1];
                    byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                    if (nextByteVal > 9 || byteAfterNext != 0) // if the second byte isn't zero, this isn't an ASCII-equivalent code unit and we can quit here
															   // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
															   // to greater than 9 since byte is an unsigned type.
                    {
                        if (bytesConsumed == 0) // check to see if we've processed any digits at all
                        {
                            return false;
                        }
                        else
                        {
                            return true; // otherwise return true
                        }
                    }
                    else if (value > Byte.MaxValue / 10)
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else if (value > 0 && Byte.MaxValue - value * 10 < nextByteVal) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }

                    value = (byte)(value * 10 + nextByteVal); // left shift the value and add the nextByte
                    bytesConsumed += 2;
                }
                return true;
            }

            return false;
        }
		#endregion

		#region ushort
		public static bool TryParse(byte[] utf8Text, int index, FormattingData cultureAndEncodingInfo, Format.Parsed numericFormat,
            out ushort value, out int bytesConsumed)
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

            if (cultureAndEncodingInfo.IsInvariantUtf8)
            {
                for (int byteIndex = index; byteIndex < utf8Text.Length; byteIndex++) // loop through the byte array
                {
                    byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                    if (nextByteVal > 9) // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
										 // to greater than 9 since byte is an unsigned type.
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
                    else if (value > UInt16.MaxValue / 10)
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else if (value > 0 && UInt16.MaxValue - value * 10 < nextByteVal) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }

                    value = (ushort)(value * 10 + nextByteVal); // left shift the value and add the nextByte
                    bytesConsumed++;
                }
				return true;
            }
            else if (cultureAndEncodingInfo.IsInvariantUtf16)
            {
                for (int byteIndex = index; byteIndex < utf8Text.Length - 1; byteIndex += 2) // loop through the byte array two bytes at a time for UTF-16
                {
                    byte byteAfterNext = utf8Text[byteIndex + 1];
                    byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                    if (nextByteVal > 9 || byteAfterNext != 0)  // if the second byte isn't zero, this isn't an ASCII-equivalent code unit and we can quit here
																// if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
																// to greater than 9 since byte is an unsigned type.
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
                    else if (value > UInt16.MaxValue / 10)
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else if (value > 0 && UInt16.MaxValue - value * 10 < nextByteVal) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }

                    value = (ushort)(value * 10 + nextByteVal); // left shift the value and add the nextByte
                    bytesConsumed += 2;
                }
                return true;
            }

            return false;
        }

		public static unsafe bool TryParse(byte* utf8Text, int index, int length, FormattingData cultureAndEncodingInfo, Format.Parsed numericFormat, 
            out ushort value, out int bytesConsumed)
        {
            // Precondition replacement
            if (length < 1 || index < 0)
            {
                value = 0;
                bytesConsumed = 0;
                return false;
            }

            value = default(ushort);
            bytesConsumed = 0;

            if (cultureAndEncodingInfo.IsInvariantUtf8)
            {
                for (int byteIndex = index; byteIndex < length + index; byteIndex++)
                {
                    byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                    if (nextByteVal > 9) // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
										 // to greater than 9 since byte is an unsigned type.
                    {
                        if (bytesConsumed == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else if (value > UInt16.MaxValue / 10) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else if (value > 0 && UInt16.MaxValue - value * 10 < nextByteVal) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    value = (ushort)(value * 10 + nextByteVal); // left shift the value and add the nextByte
                    bytesConsumed++; // increment the number of bytes consumed, then loop
                }
                return true;
            }
            else if (cultureAndEncodingInfo.IsInvariantUtf16)
            {
                for (int byteIndex = index; byteIndex < length + index - 1; byteIndex += 2) // loop through the byte array two bytes at a time for UTF-16
                {
                    byte byteAfterNext = utf8Text[byteIndex + 1];
                    byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                    if (nextByteVal > 9 || byteAfterNext != 0) // if the second byte isn't zero, this isn't an ASCII-equivalent code unit and we can quit here
															   // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
															   // to greater than 9 since byte is an unsigned type.
                    {
                        if (bytesConsumed == 0) // check to see if we've processed any digits at all
                        {
                            return false;
                        }
                        else
                        {
                            return true; // otherwise return true
                        }
                    }
                    else if (value > UInt16.MaxValue / 10)
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else if (value > 0 && UInt16.MaxValue - value * 10 < nextByteVal) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }

                    value = (ushort)(value * 10 + nextByteVal); // left shift the value and add the nextByte
                    bytesConsumed += 2;
                }
                return true;
            }

            return false;
        }
		#endregion

		#region uint
		public static bool TryParse(byte[] utf8Text, int index, FormattingData cultureAndEncodingInfo, Format.Parsed numericFormat,
            out uint value, out int bytesConsumed)
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

            if (cultureAndEncodingInfo.IsInvariantUtf8)
            {
                for (int byteIndex = index; byteIndex < utf8Text.Length; byteIndex++) // loop through the byte array
                {
                    byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                    if (nextByteVal > 9) // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
										 // to greater than 9 since byte is an unsigned type.
                    {
                        if (bytesConsumed == 0) // check to see if we've processed any digits at all
                        {
                            value = default(uint); // if we haven't, set value to 0 and return false
                            return false;
                        }
                        else
                        {
                            return true; // otherwise return true
                        }
                    }
                    else if (value > UInt32.MaxValue / 10)
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else if (value > 0 && UInt32.MaxValue - value * 10 < nextByteVal) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }

                    value = (uint)(value * 10 + nextByteVal); // left shift the value and add the nextByte
                    bytesConsumed++;
                }
				return true;
            }
            else if (cultureAndEncodingInfo.IsInvariantUtf16)
            {
                for (int byteIndex = index; byteIndex < utf8Text.Length - 1; byteIndex += 2) // loop through the byte array two bytes at a time for UTF-16
                {
                    byte byteAfterNext = utf8Text[byteIndex + 1];
                    byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                    if (nextByteVal > 9 || byteAfterNext != 0)  // if the second byte isn't zero, this isn't an ASCII-equivalent code unit and we can quit here
																// if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
																// to greater than 9 since byte is an unsigned type.
                    {
                        if (bytesConsumed == 0) // check to see if we've processed any digits at all
                        {
                            value = default(uint); // if we haven't, set value to 0 and return false
                            return false;
                        }
                        else
                        {
                            return true; // otherwise return true
                        }
                    }
                    else if (value > UInt32.MaxValue / 10)
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else if (value > 0 && UInt32.MaxValue - value * 10 < nextByteVal) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }

                    value = (uint)(value * 10 + nextByteVal); // left shift the value and add the nextByte
                    bytesConsumed += 2;
                }
                return true;
            }

            return false;
        }

		public static unsafe bool TryParse(byte* utf8Text, int index, int length, FormattingData cultureAndEncodingInfo, Format.Parsed numericFormat, 
            out uint value, out int bytesConsumed)
        {
            // Precondition replacement
            if (length < 1 || index < 0)
            {
                value = 0;
                bytesConsumed = 0;
                return false;
            }

            value = default(uint);
            bytesConsumed = 0;

            if (cultureAndEncodingInfo.IsInvariantUtf8)
            {
                for (int byteIndex = index; byteIndex < length + index; byteIndex++)
                {
                    byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                    if (nextByteVal > 9) // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
										 // to greater than 9 since byte is an unsigned type.
                    {
                        if (bytesConsumed == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else if (value > UInt32.MaxValue / 10) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else if (value > 0 && UInt32.MaxValue - value * 10 < nextByteVal) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    value = (uint)(value * 10 + nextByteVal); // left shift the value and add the nextByte
                    bytesConsumed++; // increment the number of bytes consumed, then loop
                }
                return true;
            }
            else if (cultureAndEncodingInfo.IsInvariantUtf16)
            {
                for (int byteIndex = index; byteIndex < length + index - 1; byteIndex += 2) // loop through the byte array two bytes at a time for UTF-16
                {
                    byte byteAfterNext = utf8Text[byteIndex + 1];
                    byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                    if (nextByteVal > 9 || byteAfterNext != 0) // if the second byte isn't zero, this isn't an ASCII-equivalent code unit and we can quit here
															   // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
															   // to greater than 9 since byte is an unsigned type.
                    {
                        if (bytesConsumed == 0) // check to see if we've processed any digits at all
                        {
                            return false;
                        }
                        else
                        {
                            return true; // otherwise return true
                        }
                    }
                    else if (value > UInt32.MaxValue / 10)
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else if (value > 0 && UInt32.MaxValue - value * 10 < nextByteVal) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }

                    value = (uint)(value * 10 + nextByteVal); // left shift the value and add the nextByte
                    bytesConsumed += 2;
                }
                return true;
            }

            return false;
        }
		#endregion

		#region ulong
		public static bool TryParse(byte[] utf8Text, int index, FormattingData cultureAndEncodingInfo, Format.Parsed numericFormat,
            out ulong value, out int bytesConsumed)
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

            if (cultureAndEncodingInfo.IsInvariantUtf8)
            {
                for (int byteIndex = index; byteIndex < utf8Text.Length; byteIndex++) // loop through the byte array
                {
                    byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                    if (nextByteVal > 9) // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
										 // to greater than 9 since byte is an unsigned type.
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
                    else if (value > UInt64.MaxValue / 10)
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else if (value > 0 && UInt64.MaxValue - value * 10 < nextByteVal) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }

                    value = (ulong)(value * 10 + nextByteVal); // left shift the value and add the nextByte
                    bytesConsumed++;
                }
				return true;
            }
            else if (cultureAndEncodingInfo.IsInvariantUtf16)
            {
                for (int byteIndex = index; byteIndex < utf8Text.Length - 1; byteIndex += 2) // loop through the byte array two bytes at a time for UTF-16
                {
                    byte byteAfterNext = utf8Text[byteIndex + 1];
                    byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                    if (nextByteVal > 9 || byteAfterNext != 0)  // if the second byte isn't zero, this isn't an ASCII-equivalent code unit and we can quit here
																// if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
																// to greater than 9 since byte is an unsigned type.
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
                    else if (value > UInt64.MaxValue / 10)
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else if (value > 0 && UInt64.MaxValue - value * 10 < nextByteVal) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }

                    value = (ulong)(value * 10 + nextByteVal); // left shift the value and add the nextByte
                    bytesConsumed += 2;
                }
                return true;
            }

            return false;
        }

		public static unsafe bool TryParse(byte* utf8Text, int index, int length, FormattingData cultureAndEncodingInfo, Format.Parsed numericFormat, 
            out ulong value, out int bytesConsumed)
        {
            // Precondition replacement
            if (length < 1 || index < 0)
            {
                value = 0;
                bytesConsumed = 0;
                return false;
            }

            value = default(ulong);
            bytesConsumed = 0;

            if (cultureAndEncodingInfo.IsInvariantUtf8)
            {
                for (int byteIndex = index; byteIndex < length + index; byteIndex++)
                {
                    byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                    if (nextByteVal > 9) // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
										 // to greater than 9 since byte is an unsigned type.
                    {
                        if (bytesConsumed == 0)
                        {
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
                    else if (value > 0 && UInt64.MaxValue - value * 10 < nextByteVal) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    value = (ulong)(value * 10 + nextByteVal); // left shift the value and add the nextByte
                    bytesConsumed++; // increment the number of bytes consumed, then loop
                }
                return true;
            }
            else if (cultureAndEncodingInfo.IsInvariantUtf16)
            {
                for (int byteIndex = index; byteIndex < length + index - 1; byteIndex += 2) // loop through the byte array two bytes at a time for UTF-16
                {
                    byte byteAfterNext = utf8Text[byteIndex + 1];
                    byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                    if (nextByteVal > 9 || byteAfterNext != 0) // if the second byte isn't zero, this isn't an ASCII-equivalent code unit and we can quit here
															   // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
															   // to greater than 9 since byte is an unsigned type.
                    {
                        if (bytesConsumed == 0) // check to see if we've processed any digits at all
                        {
                            return false;
                        }
                        else
                        {
                            return true; // otherwise return true
                        }
                    }
                    else if (value > UInt64.MaxValue / 10)
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else if (value > 0 && UInt64.MaxValue - value * 10 < nextByteVal) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }

                    value = (ulong)(value * 10 + nextByteVal); // left shift the value and add the nextByte
                    bytesConsumed += 2;
                }
                return true;
            }

            return false;
        }
		#endregion

	}
}