// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Threading.Tasks.Channels
{
    /// <summary>Exception thrown when a channel is used after it's been closed.</summary>
    public class ClosedChannelException : InvalidOperationException
    {
        public ClosedChannelException() : base(Properties.Resources.ClosedChannelException_DefaultMessage) { }

        public ClosedChannelException(string message) : base(message) { }

        public ClosedChannelException(string message, Exception innerException) : base(message, innerException) { } 
    }
}
