// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.IO.Pipelines.Networking.Windows.RIO.Internal.Winsock
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct NotificationCompletionIocp
    {
        public IntPtr IocpHandle;
        public ulong QueueCorrelation;
        public NativeOverlapped* Overlapped;
    }
}
