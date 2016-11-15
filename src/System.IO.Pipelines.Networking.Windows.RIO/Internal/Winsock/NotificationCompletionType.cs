// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.IO.Pipelines.Networking.Windows.RIO.Internal.Winsock
{
    public enum NotificationCompletionType : int
    {
        Polling = 0,
        EventCompletion = 1,
        IocpCompletion = 2
    }
}
