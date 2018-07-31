// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    public static class Utils
    {
        internal static ulong ValueFromBuffer(byte[] buffer)
        {
            ulong result = 0;

            for (int i = 0; i < buffer.Length; ++i)
            {
                result = (result << 8) | buffer[i];
            }

            return result;
        }
    }
}
