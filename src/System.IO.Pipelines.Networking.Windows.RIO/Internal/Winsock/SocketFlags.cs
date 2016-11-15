// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.IO.Pipelines.Networking.Windows.RIO.Internal.Winsock
{
    public enum SocketFlags : uint
    {
        Overlapped = 0x01,
        MultipointCRoot = 0x02,
        MultipointCLeaf = 0x04,
        MultipointDRoot = 0x08,
        MultipointDLeaf = 0x10,
        AccessSystemSecurity = 0x40,
        NoHandleInherit = 0x80,
        RegisteredIO = 0x100
    }
}