// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
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