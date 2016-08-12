// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class InvariantParser
    {      
        public static bool TryParse(Utf8String utf8Text, FormattingData cultureAndEncodingInfo, Format.Parsed numericFormat, 
            out ulong value, out int bytesConsumed)
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

            if (cultureAndEncodingInfo.IsInvariantUtf8)
            {
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
                    ulong candidate = value * 10 + nextByteVal;

                    value = candidate;
                    bytesConsumed++;
                }

                return true;
            }

            return false;
        }
    }
}
