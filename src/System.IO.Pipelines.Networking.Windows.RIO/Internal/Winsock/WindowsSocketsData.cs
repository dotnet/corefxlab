// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace System.IO.Pipelines.Networking.Windows.RIO.Internal.Winsock
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct WindowsSocketsData
    {
        internal short Version;
        internal short HighVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 257)]
        internal string Description;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
        internal string SystemStatus;
        internal short MaxSockets;
        internal short MaxDatagramSize;
        internal IntPtr VendorInfo;
    }
}