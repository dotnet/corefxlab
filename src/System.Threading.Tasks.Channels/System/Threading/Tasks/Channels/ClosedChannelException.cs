// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
