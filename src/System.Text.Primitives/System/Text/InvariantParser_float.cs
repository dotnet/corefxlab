using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class InvariantParser
    {
        public static bool TryParse(byte[] utf8Text, int index, out float value, out int bytesConsumed)
        {
            // Precondition replacement
            if (utf8Text.Length < 1 || index < 0 || index >= utf8Text.Length)
            {
                value = 0;
                bytesConsumed = 0;
                return false;
            }

            value = 0f;
            bytesConsumed = 0;
            string floatString = "";
            bool decimalPlace = false;
            bool signed = false;

            if (utf8Text[index] == '-' || utf8Text[index] == '+')
            {
                signed = true;
                floatString += (char)utf8Text[index];
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
    }
}
