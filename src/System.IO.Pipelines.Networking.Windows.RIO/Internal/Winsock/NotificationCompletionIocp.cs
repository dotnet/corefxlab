// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
