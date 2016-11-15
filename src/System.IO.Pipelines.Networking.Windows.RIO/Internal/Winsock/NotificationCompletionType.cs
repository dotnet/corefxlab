// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.IO.Pipelines.Networking.Windows.RIO.Internal.Winsock
{
    public enum NotificationCompletionType : int
    {
        Polling = 0,
        EventCompletion = 1,
        IocpCompletion = 2
    }
}
