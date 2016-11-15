// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.InteropServices;

namespace System.IO.Pipelines.Networking.Windows.RIO.Internal.Winsock
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NotificationCompletion
    {
        public NotificationCompletionType Type;
        public NotificationCompletionIocp Iocp;
    }
}
