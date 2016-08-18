// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Utf8;

namespace System.Text
{
    public static partial class InvariantParser
    {
        public static bool TryParse(byte[] utf8Text, int index, out Uri value, out int bytesConsumed)
        {
            // Precondition replacement
            if (utf8Text.Length < 1 || index < 0 || index >= utf8Text.Length)
            {
                value = default(Uri);
                bytesConsumed = 0;
                return false;
            }

            StringBuilder sb = new StringBuilder();
            bytesConsumed = 0;

            for (int byteIndex = index; byteIndex < utf8Text.Length; byteIndex++)
            {
                byte nextByte = utf8Text[byteIndex];

                if (nextByte == '#' || nextByte == '&' || nextByte == '=' || nextByte == '?' || nextByte == '_' || nextByte == '\\' || nextByte == ':' ||
                    nextByte >= '-' && nextByte <= '9' || nextByte >= 'A' && nextByte <= 'Z' || nextByte >= 'a' && nextByte <= 'z')
                {
                    sb.Append((char)nextByte);
                    bytesConsumed++;
                }
                else
                {
                    if (bytesConsumed == 0)
                    {
                        value = default(Uri);
                        return false;
                    }
                    else
                    {
                        if (Uri.TryCreate(sb.ToString(), UriKind.Absolute, out value))
                        {
                            return true;
                        }
                        else
                        {
                            value = default(Uri);
                            bytesConsumed = 0;
                            return false;
                        }
                    }
                }
            }

            if (bytesConsumed == 0)
            {
                value = default(Uri);
                return false;
            }
            else
            {
                if (Uri.TryCreate(sb.ToString(), UriKind.Absolute, out value))
                {
                    return true;
                }
                else
                {
                    value = default(Uri);
                    bytesConsumed = 0;
                    return false;
                }
            }
        }

        public static unsafe bool TryParse(byte* utf8Text, int index, int length, out Uri value, out int bytesConsumed)
        {
            // Precondition replacement
            if (length < 1 || index < 0)
            {
                value = default(Uri);
                bytesConsumed = 0;
                return false;
            }

            StringBuilder sb = new StringBuilder();
            bytesConsumed = 0;

            for (int byteIndex = index; byteIndex < length; byteIndex++)
            {
                byte nextByte = utf8Text[byteIndex];

                if (nextByte == '#' || nextByte == '&' || nextByte == '=' || nextByte == '?' || nextByte == '_' || nextByte == '\\' || nextByte == ':' ||
                    nextByte >= '-' && nextByte <= '9' || nextByte >= 'A' && nextByte <= 'Z' || nextByte >= 'a' && nextByte <= 'z')
                {
                    sb.Append((char)nextByte);
                    bytesConsumed++;
                }
                else
                {
                    if (bytesConsumed == 0)
                    {
                        value = default(Uri);
                        return false;
                    }
                    else
                    {
                        if (Uri.TryCreate(sb.ToString(), UriKind.Absolute, out value))
                        {
                            return true;
                        }
                        else
                        {
                            value = default(Uri);
                            bytesConsumed = 0;
                            return false;
                        }
                    }
                }
            }



            if (bytesConsumed == 0)
            {
                value = default(Uri);
                return false;
            }
            else
            {
                if (Uri.TryCreate(sb.ToString(), UriKind.Absolute, out value))
                {
                    return true;
                }
                else
                {
                    value = default(Uri);
                    bytesConsumed = 0;
                    return false;
                }
            }
        }
    }
}
