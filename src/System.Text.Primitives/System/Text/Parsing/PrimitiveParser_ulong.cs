// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class PrimitiveParser
    {      
        public static bool TryParseUInt64(Utf8String text, Format.Parsed numericFormat, 
            out ulong value, out int bytesConsumed)
        {
            // Precondition replacement
            if (text.Length < 1)
            {
                value = 0;
                bytesConsumed = 0;
                return false;
            }

            value = 0;
            bytesConsumed = 0;

            for (int byteIndex = 0; byteIndex < text.Length; byteIndex++)
            {
                byte nextByteVal = (byte)((byte)text[byteIndex] - '0');
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
    }
}
