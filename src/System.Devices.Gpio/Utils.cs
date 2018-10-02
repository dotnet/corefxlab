// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Runtime.CompilerServices;

namespace System.Devices.Gpio
{
    internal static class Utils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort SwapBytes(ushort value)
        {
            value = (ushort)((value << 8) | (value >> 8));
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint SwapBytes(uint value)
        {
            value = (uint)((SwapBytes((ushort)(value & ushort.MaxValue)) << 16) | SwapBytes((ushort)(value >> 16)));
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong SwapBytes(ulong value)
        {
            value = (SwapBytes((uint)(value & uint.MaxValue)) << 32) | SwapBytes((uint)(value >> 32));
            return value;
        }

        public static IOException CreateIOException(string message, int result)
        {
            Interop.ErrorInfo info = Interop.Sys.GetLastErrorInfo();
            message = $"{message}\nResult: {result} {info}";
            return new IOException(message);
        }
    }
}
