// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace System.IO.Pipelines.Networking.Windows.RIO.Internal.Winsock
{
    [Flags]
    public enum RioSendFlags : uint
    {
        None = 0x00000000,
        DontNotify = 0x00000001,
        Defer = 0x00000002,
        CommitOnly = 0x00000008
    }
}