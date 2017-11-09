// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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