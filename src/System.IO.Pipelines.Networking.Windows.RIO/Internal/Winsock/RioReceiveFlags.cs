// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.IO.Pipelines.Networking.Windows.RIO.Internal.Winsock
{
    public enum RioReceiveFlags : uint
    {
        None = 0x00000000,
        DontNotify = 0x00000001,
        Defer = 0x00000002,
        Waitall = 0x00000004,
        CommitOnly = 0x00000008
    }
}