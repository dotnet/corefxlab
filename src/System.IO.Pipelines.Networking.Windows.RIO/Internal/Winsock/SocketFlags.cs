// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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