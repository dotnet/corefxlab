// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.InteropServices;

namespace System.IO.Pipelines.Networking.Windows.RIO.Internal.Winsock
{
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct Ipv4InternetAddress
    {
        [FieldOffset(0)]
        public byte Byte1;
        [FieldOffset(1)]
        public byte Byte2;
        [FieldOffset(2)]
        public byte Byte3;
        [FieldOffset(3)]
        public byte Byte4;
    }
}