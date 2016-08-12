// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class InvariantParser
    {
		#region sbyte
		public static bool TryParse(byte[] utf8Text, int index, FormattingData cultureAndEncodingInfo, Format.Parsed numericFormat,
            out sbyte value, out int bytesConsumed)
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
            bool negative = false;
            bool signed = false;

            if (cultureAndEncodingInfo.IsInvariantUtf8)
            {
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
                    if (nextByteVal > 9) // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
										 // to greater than 9 since byte is an unsigned type.
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
                            if (negative && value != SByte.MinValue) // We check if the value is negative at the very end to save on comp time
                            {
                                value = (sbyte)-value;
                            }
                            return true; // otherwise return true
                        }
                    }
                    else if (value > SByte.MaxValue / 10) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else if (value > 0 && (SByte.MaxValue - value * 10) + (negative ? 1 : 0) < nextByteVal) // overflow (+1 allows for overflow for min value)
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    sbyte candidate = (sbyte)(value * 10 + nextByteVal); // parse the current digit to a sbyte and add it to the left-shifted value
                    value = candidate;
                    bytesConsumed++; // increment the number of bytes consumed, then loop
                }

                if (negative && value != SByte.MinValue) // We check if the value is negative at the very end to save on comp time
                {
                    value = (sbyte)-value;
                }
                return true;
            }
            else if (cultureAndEncodingInfo.IsInvariantUtf16)
            {
                if (utf8Text[index] == '-')
                {
                    negative = true;
                    signed = true;
                    index += 2;
                    bytesConsumed += 2;
                }
                else if (utf8Text[index] == '+')
                {
                    signed = true;
                    index += 2;
                    bytesConsumed += 2;
                }

                for (int byteIndex = index; byteIndex < utf8Text.Length - 1; byteIndex += 2) // loop through the byte array two bytes at a time for UTF-16
                {
                    byte byteAfterNext = utf8Text[byteIndex + 1];
                    byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                    if (nextByteVal > 9 || byteAfterNext != 0) // if the second byte isn't zero, this isn't an ASCII-equivalent code unit and we can quit here
															   // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
															   // to greater than 9 since byte is an unsigned type.
                    {
                        if (bytesConsumed == 2 && signed) // if the first character happened to be a '-' or a '+', we reset the byte counter so logic proceeds as normal
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
                            if (negative && value != SByte.MinValue) // We check if the value is negative at the very end to save on comp time
                            {
                                value = (sbyte)-value;
                            }
                            return true; // otherwise return true
                        }
                    }
                    else if (value > SByte.MaxValue / 10) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else if (value > 0 && (SByte.MaxValue - value * 10) + (negative ? 1 : 0) < nextByteVal) // overflow (+1 allows for overflow for min value)
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    sbyte candidate = (sbyte)(value * 10 + nextByteVal); // parse the current digit to a sbyte and add it to the left-shifted value
                    value = candidate;
                    bytesConsumed += 2; // increment the number of bytes consumed, then loop
                }

                if (negative && value != SByte.MinValue) // We check if the value is negative at the very end to save on comp time
                {
                    value = (sbyte)-value;
                }
                return true;
            }

            return false;
        }

		public static unsafe bool TryParse(byte* utf8Text, int index, int length, FormattingData cultureAndEncodingInfo, Format.Parsed numericFormat, 
            out sbyte value, out int bytesConsumed)
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
            bool negative = false;
            bool signed = false;

            if (cultureAndEncodingInfo.IsInvariantUtf8)
            {
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
                    if (nextByteVal > 9) // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
										 // to greater than 9 since byte is an unsigned type.
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
                            if (negative && value != SByte.MinValue) // We check if the value is negative at the very end to save on comp time
                            {
                                value = (sbyte)-value;
                            }
                            return true;
                        }
                    }
                    else if (value > SByte.MaxValue / 10) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else if (value > 0 && (SByte.MaxValue - value * 10) + (negative ? 1 : 0) < nextByteVal) // overflow (+1 allows for overflow for min value)
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    sbyte candidate = (sbyte)(value * 10 + nextByteVal); // parse the current digit to a sbyte and add it to the left-shifted value
                    value = candidate;
                    bytesConsumed++; // increment the number of bytes consumed, then loop
                }
                if (negative && value != SByte.MinValue) // We check if the value is negative at the very end to save on comp time
                {
                    value = (sbyte)-value;
                }
                return true;
            }
            else if (cultureAndEncodingInfo.IsInvariantUtf16)
            {
                if (utf8Text[index] == '-')
                {
                    negative = true;
                    signed = true;
                    index += 2;
                    bytesConsumed += 2;
                }
                else if (utf8Text[index] == '+')
                {
                    signed = true;
                    index += 2;
                    bytesConsumed += 2;
                }

                for (int byteIndex = index; byteIndex < length + index - 1; byteIndex += 2) // loop through the byte array two bytes at a time for UTF-16
                {
                    byte byteAfterNext = utf8Text[byteIndex + 1];
                    byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                    if (nextByteVal > 9 || byteAfterNext != 0) // if the second byte isn't zero, this isn't an ASCII-equivalent code unit and we can quit here
															   // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
															   // to greater than 9 since byte is an unsigned type.
                    {
                        if (bytesConsumed == 2 && signed) // if the first character happened to be a '-' or a '+', we reset the byte counter so logic proceeds as normal
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
                            if (negative && value != SByte.MinValue) // We check if the value is negative at the very end to save on comp time
                            {
                                value = (sbyte)-value;
                            }
                            return true; // otherwise return true
                        }
                    }
                    else if (value > SByte.MaxValue / 10) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else if (value > 0 && (SByte.MaxValue - value * 10) + (negative ? 1 : 0) < nextByteVal) // overflow (+1 allows for overflow for min value)
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    sbyte candidate = (sbyte)(value * 10 + nextByteVal); // parse the current digit to a sbyte and add it to the left-shifted value
                    value = candidate;
                    bytesConsumed += 2; // increment the number of bytes consumed, then loop
                }

                if (negative && value != SByte.MinValue) // We check if the value is negative at the very end to save on comp time
                {
                    value = (sbyte)-value;
                }
                return true;
            }
            return false;
        }
		#endregion

		#region short
		public static bool TryParse(byte[] utf8Text, int index, FormattingData cultureAndEncodingInfo, Format.Parsed numericFormat,
            out short value, out int bytesConsumed)
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
            bool negative = false;
            bool signed = false;

            if (cultureAndEncodingInfo.IsInvariantUtf8)
            {
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
                    if (nextByteVal > 9) // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
										 // to greater than 9 since byte is an unsigned type.
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
                            if (negative && value != Int16.MinValue) // We check if the value is negative at the very end to save on comp time
                            {
                                value = (short)-value;
                            }
                            return true; // otherwise return true
                        }
                    }
                    else if (value > Int16.MaxValue / 10) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else if (value > 0 && (Int16.MaxValue - value * 10) + (negative ? 1 : 0) < nextByteVal) // overflow (+1 allows for overflow for min value)
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    short candidate = (short)(value * 10 + nextByteVal); // parse the current digit to a short and add it to the left-shifted value
                    value = candidate;
                    bytesConsumed++; // increment the number of bytes consumed, then loop
                }

                if (negative && value != Int16.MinValue) // We check if the value is negative at the very end to save on comp time
                {
                    value = (short)-value;
                }
                return true;
            }
            else if (cultureAndEncodingInfo.IsInvariantUtf16)
            {
                if (utf8Text[index] == '-')
                {
                    negative = true;
                    signed = true;
                    index += 2;
                    bytesConsumed += 2;
                }
                else if (utf8Text[index] == '+')
                {
                    signed = true;
                    index += 2;
                    bytesConsumed += 2;
                }

                for (int byteIndex = index; byteIndex < utf8Text.Length - 1; byteIndex += 2) // loop through the byte array two bytes at a time for UTF-16
                {
                    byte byteAfterNext = utf8Text[byteIndex + 1];
                    byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                    if (nextByteVal > 9 || byteAfterNext != 0) // if the second byte isn't zero, this isn't an ASCII-equivalent code unit and we can quit here
															   // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
															   // to greater than 9 since byte is an unsigned type.
                    {
                        if (bytesConsumed == 2 && signed) // if the first character happened to be a '-' or a '+', we reset the byte counter so logic proceeds as normal
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
                            if (negative && value != Int16.MinValue) // We check if the value is negative at the very end to save on comp time
                            {
                                value = (short)-value;
                            }
                            return true; // otherwise return true
                        }
                    }
                    else if (value > Int16.MaxValue / 10) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else if (value > 0 && (Int16.MaxValue - value * 10) + (negative ? 1 : 0) < nextByteVal) // overflow (+1 allows for overflow for min value)
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    short candidate = (short)(value * 10 + nextByteVal); // parse the current digit to a short and add it to the left-shifted value
                    value = candidate;
                    bytesConsumed += 2; // increment the number of bytes consumed, then loop
                }

                if (negative && value != Int16.MinValue) // We check if the value is negative at the very end to save on comp time
                {
                    value = (short)-value;
                }
                return true;
            }

            return false;
        }

		public static unsafe bool TryParse(byte* utf8Text, int index, int length, FormattingData cultureAndEncodingInfo, Format.Parsed numericFormat, 
            out short value, out int bytesConsumed)
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
            bool negative = false;
            bool signed = false;

            if (cultureAndEncodingInfo.IsInvariantUtf8)
            {
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
                    if (nextByteVal > 9) // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
										 // to greater than 9 since byte is an unsigned type.
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
                            if (negative && value != Int16.MinValue) // We check if the value is negative at the very end to save on comp time
                            {
                                value = (short)-value;
                            }
                            return true;
                        }
                    }
                    else if (value > Int16.MaxValue / 10) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else if (value > 0 && (Int16.MaxValue - value * 10) + (negative ? 1 : 0) < nextByteVal) // overflow (+1 allows for overflow for min value)
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    short candidate = (short)(value * 10 + nextByteVal); // parse the current digit to a short and add it to the left-shifted value
                    value = candidate;
                    bytesConsumed++; // increment the number of bytes consumed, then loop
                }
                if (negative && value != Int16.MinValue) // We check if the value is negative at the very end to save on comp time
                {
                    value = (short)-value;
                }
                return true;
            }
            else if (cultureAndEncodingInfo.IsInvariantUtf16)
            {
                if (utf8Text[index] == '-')
                {
                    negative = true;
                    signed = true;
                    index += 2;
                    bytesConsumed += 2;
                }
                else if (utf8Text[index] == '+')
                {
                    signed = true;
                    index += 2;
                    bytesConsumed += 2;
                }

                for (int byteIndex = index; byteIndex < length + index - 1; byteIndex += 2) // loop through the byte array two bytes at a time for UTF-16
                {
                    byte byteAfterNext = utf8Text[byteIndex + 1];
                    byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                    if (nextByteVal > 9 || byteAfterNext != 0) // if the second byte isn't zero, this isn't an ASCII-equivalent code unit and we can quit here
															   // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
															   // to greater than 9 since byte is an unsigned type.
                    {
                        if (bytesConsumed == 2 && signed) // if the first character happened to be a '-' or a '+', we reset the byte counter so logic proceeds as normal
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
                            if (negative && value != Int16.MinValue) // We check if the value is negative at the very end to save on comp time
                            {
                                value = (short)-value;
                            }
                            return true; // otherwise return true
                        }
                    }
                    else if (value > Int16.MaxValue / 10) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else if (value > 0 && (Int16.MaxValue - value * 10) + (negative ? 1 : 0) < nextByteVal) // overflow (+1 allows for overflow for min value)
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    short candidate = (short)(value * 10 + nextByteVal); // parse the current digit to a short and add it to the left-shifted value
                    value = candidate;
                    bytesConsumed += 2; // increment the number of bytes consumed, then loop
                }

                if (negative && value != Int16.MinValue) // We check if the value is negative at the very end to save on comp time
                {
                    value = (short)-value;
                }
                return true;
            }
            return false;
        }
		#endregion

		#region int
		public static bool TryParse(byte[] utf8Text, int index, FormattingData cultureAndEncodingInfo, Format.Parsed numericFormat,
            out int value, out int bytesConsumed)
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
            bool negative = false;
            bool signed = false;

            if (cultureAndEncodingInfo.IsInvariantUtf8)
            {
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
                    if (nextByteVal > 9) // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
										 // to greater than 9 since byte is an unsigned type.
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
                            if (negative && value != Int32.MinValue) // We check if the value is negative at the very end to save on comp time
                            {
                                value = (int)-value;
                            }
                            return true; // otherwise return true
                        }
                    }
                    else if (value > Int32.MaxValue / 10) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else if (value > 0 && (Int32.MaxValue - value * 10) + (negative ? 1 : 0) < nextByteVal) // overflow (+1 allows for overflow for min value)
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    int candidate = (int)(value * 10 + nextByteVal); // parse the current digit to a int and add it to the left-shifted value
                    value = candidate;
                    bytesConsumed++; // increment the number of bytes consumed, then loop
                }

                if (negative && value != Int32.MinValue) // We check if the value is negative at the very end to save on comp time
                {
                    value = (int)-value;
                }
                return true;
            }
            else if (cultureAndEncodingInfo.IsInvariantUtf16)
            {
                if (utf8Text[index] == '-')
                {
                    negative = true;
                    signed = true;
                    index += 2;
                    bytesConsumed += 2;
                }
                else if (utf8Text[index] == '+')
                {
                    signed = true;
                    index += 2;
                    bytesConsumed += 2;
                }

                for (int byteIndex = index; byteIndex < utf8Text.Length - 1; byteIndex += 2) // loop through the byte array two bytes at a time for UTF-16
                {
                    byte byteAfterNext = utf8Text[byteIndex + 1];
                    byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                    if (nextByteVal > 9 || byteAfterNext != 0) // if the second byte isn't zero, this isn't an ASCII-equivalent code unit and we can quit here
															   // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
															   // to greater than 9 since byte is an unsigned type.
                    {
                        if (bytesConsumed == 2 && signed) // if the first character happened to be a '-' or a '+', we reset the byte counter so logic proceeds as normal
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
                            if (negative && value != Int32.MinValue) // We check if the value is negative at the very end to save on comp time
                            {
                                value = (int)-value;
                            }
                            return true; // otherwise return true
                        }
                    }
                    else if (value > Int32.MaxValue / 10) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else if (value > 0 && (Int32.MaxValue - value * 10) + (negative ? 1 : 0) < nextByteVal) // overflow (+1 allows for overflow for min value)
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    int candidate = (int)(value * 10 + nextByteVal); // parse the current digit to a int and add it to the left-shifted value
                    value = candidate;
                    bytesConsumed += 2; // increment the number of bytes consumed, then loop
                }

                if (negative && value != Int32.MinValue) // We check if the value is negative at the very end to save on comp time
                {
                    value = (int)-value;
                }
                return true;
            }

            return false;
        }

		public static unsafe bool TryParse(byte* utf8Text, int index, int length, FormattingData cultureAndEncodingInfo, Format.Parsed numericFormat, 
            out int value, out int bytesConsumed)
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
            bool negative = false;
            bool signed = false;

            if (cultureAndEncodingInfo.IsInvariantUtf8)
            {
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
                    if (nextByteVal > 9) // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
										 // to greater than 9 since byte is an unsigned type.
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
                            if (negative && value != Int32.MinValue) // We check if the value is negative at the very end to save on comp time
                            {
                                value = (int)-value;
                            }
                            return true;
                        }
                    }
                    else if (value > Int32.MaxValue / 10) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else if (value > 0 && (Int32.MaxValue - value * 10) + (negative ? 1 : 0) < nextByteVal) // overflow (+1 allows for overflow for min value)
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    int candidate = (int)(value * 10 + nextByteVal); // parse the current digit to a int and add it to the left-shifted value
                    value = candidate;
                    bytesConsumed++; // increment the number of bytes consumed, then loop
                }
                if (negative && value != Int32.MinValue) // We check if the value is negative at the very end to save on comp time
                {
                    value = (int)-value;
                }
                return true;
            }
            else if (cultureAndEncodingInfo.IsInvariantUtf16)
            {
                if (utf8Text[index] == '-')
                {
                    negative = true;
                    signed = true;
                    index += 2;
                    bytesConsumed += 2;
                }
                else if (utf8Text[index] == '+')
                {
                    signed = true;
                    index += 2;
                    bytesConsumed += 2;
                }

                for (int byteIndex = index; byteIndex < length + index - 1; byteIndex += 2) // loop through the byte array two bytes at a time for UTF-16
                {
                    byte byteAfterNext = utf8Text[byteIndex + 1];
                    byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                    if (nextByteVal > 9 || byteAfterNext != 0) // if the second byte isn't zero, this isn't an ASCII-equivalent code unit and we can quit here
															   // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
															   // to greater than 9 since byte is an unsigned type.
                    {
                        if (bytesConsumed == 2 && signed) // if the first character happened to be a '-' or a '+', we reset the byte counter so logic proceeds as normal
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
                            if (negative && value != Int32.MinValue) // We check if the value is negative at the very end to save on comp time
                            {
                                value = (int)-value;
                            }
                            return true; // otherwise return true
                        }
                    }
                    else if (value > Int32.MaxValue / 10) // overflow
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    else if (value > 0 && (Int32.MaxValue - value * 10) + (negative ? 1 : 0) < nextByteVal) // overflow (+1 allows for overflow for min value)
                    {
                        value = 0;
                        bytesConsumed = 0;
                        return false;
                    }
                    int candidate = (int)(value * 10 + nextByteVal); // parse the current digit to a int and add it to the left-shifted value
                    value = candidate;
                    bytesConsumed += 2; // increment the number of bytes consumed, then loop
                }

                if (negative && value != Int32.MinValue) // We check if the value is negative at the very end to save on comp time
                {
                    value = (int)-value;
                }
                return true;
            }
            return false;
        }
		#endregion

		#region long
		public static bool TryParse(byte[] utf8Text, int index, FormattingData cultureAndEncodingInfo, Format.Parsed numericFormat,
            out long value, out int bytesConsumed)
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
            bool negative = false;
            bool signed = false;

            if (cultureAndEncodingInfo.IsInvariantUtf8)
            {
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
                    if (nextByteVal > 9) // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
										 // to greater than 9 since byte is an unsigned type.
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
                                value = (long)-value;
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
                    long candidate = (long)(value * 10 + nextByteVal); // parse the current digit to a long and add it to the left-shifted value
                    value = candidate;
                    bytesConsumed++; // increment the number of bytes consumed, then loop
                }

                if (negative && value != Int64.MinValue) // We check if the value is negative at the very end to save on comp time
                {
                    value = (long)-value;
                }
                return true;
            }
            else if (cultureAndEncodingInfo.IsInvariantUtf16)
            {
                if (utf8Text[index] == '-')
                {
                    negative = true;
                    signed = true;
                    index += 2;
                    bytesConsumed += 2;
                }
                else if (utf8Text[index] == '+')
                {
                    signed = true;
                    index += 2;
                    bytesConsumed += 2;
                }

                for (int byteIndex = index; byteIndex < utf8Text.Length - 1; byteIndex += 2) // loop through the byte array two bytes at a time for UTF-16
                {
                    byte byteAfterNext = utf8Text[byteIndex + 1];
                    byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                    if (nextByteVal > 9 || byteAfterNext != 0) // if the second byte isn't zero, this isn't an ASCII-equivalent code unit and we can quit here
															   // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
															   // to greater than 9 since byte is an unsigned type.
                    {
                        if (bytesConsumed == 2 && signed) // if the first character happened to be a '-' or a '+', we reset the byte counter so logic proceeds as normal
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
                                value = (long)-value;
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
                    long candidate = (long)(value * 10 + nextByteVal); // parse the current digit to a long and add it to the left-shifted value
                    value = candidate;
                    bytesConsumed += 2; // increment the number of bytes consumed, then loop
                }

                if (negative && value != Int64.MinValue) // We check if the value is negative at the very end to save on comp time
                {
                    value = (long)-value;
                }
                return true;
            }

            return false;
        }

		public static unsafe bool TryParse(byte* utf8Text, int index, int length, FormattingData cultureAndEncodingInfo, Format.Parsed numericFormat, 
            out long value, out int bytesConsumed)
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
            bool negative = false;
            bool signed = false;

            if (cultureAndEncodingInfo.IsInvariantUtf8)
            {
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
                    if (nextByteVal > 9) // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
										 // to greater than 9 since byte is an unsigned type.
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
                                value = (long)-value;
                            }
                            return true;
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
                    long candidate = (long)(value * 10 + nextByteVal); // parse the current digit to a long and add it to the left-shifted value
                    value = candidate;
                    bytesConsumed++; // increment the number of bytes consumed, then loop
                }
                if (negative && value != Int64.MinValue) // We check if the value is negative at the very end to save on comp time
                {
                    value = (long)-value;
                }
                return true;
            }
            else if (cultureAndEncodingInfo.IsInvariantUtf16)
            {
                if (utf8Text[index] == '-')
                {
                    negative = true;
                    signed = true;
                    index += 2;
                    bytesConsumed += 2;
                }
                else if (utf8Text[index] == '+')
                {
                    signed = true;
                    index += 2;
                    bytesConsumed += 2;
                }

                for (int byteIndex = index; byteIndex < length + index - 1; byteIndex += 2) // loop through the byte array two bytes at a time for UTF-16
                {
                    byte byteAfterNext = utf8Text[byteIndex + 1];
                    byte nextByteVal = (byte)(utf8Text[byteIndex] - '0');
                    if (nextByteVal > 9 || byteAfterNext != 0) // if the second byte isn't zero, this isn't an ASCII-equivalent code unit and we can quit here
															   // if nextByteVal > 9, we know it is not a digit because any value less than '0' will overflow
															   // to greater than 9 since byte is an unsigned type.
                    {
                        if (bytesConsumed == 2 && signed) // if the first character happened to be a '-' or a '+', we reset the byte counter so logic proceeds as normal
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
                                value = (long)-value;
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
                    long candidate = (long)(value * 10 + nextByteVal); // parse the current digit to a long and add it to the left-shifted value
                    value = candidate;
                    bytesConsumed += 2; // increment the number of bytes consumed, then loop
                }

                if (negative && value != Int64.MinValue) // We check if the value is negative at the very end to save on comp time
                {
                    value = (long)-value;
                }
                return true;
            }
            return false;
        }
		#endregion

	}
}