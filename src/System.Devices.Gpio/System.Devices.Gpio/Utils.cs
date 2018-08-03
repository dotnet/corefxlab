// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Runtime.InteropServices;

namespace System.Devices.Gpio
{
    internal static class Utils
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

        internal static IOException CreateIOException(string message, int result)
        {
            Interop.ErrorInfo info = Interop.Sys.GetLastErrorInfo();
            message = $"{message}\nResult: {result} {info}";

            return new IOException(message);
        }
    }
}
