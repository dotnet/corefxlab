// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Threading.Tasks.Channels
{
    /// <summary>Exception thrown when a channel is used after it's been closed.</summary>
    public class ClosedChannelException : InvalidOperationException
    {
        /// <summary>Initializes a new instance of the <see cref="ClosedChannelException"/> class.</summary>
        public ClosedChannelException() : base(Properties.Resources.ClosedChannelException_DefaultMessage) { }

        /// <summary>Initializes a new instance of the <see cref="ClosedChannelException"/> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        public ClosedChannelException(string message) : base(message) { }

        /// <summary>Initializes a new instance of the <see cref="ClosedChannelException"/> class.</summary>
        /// <param name="innerException">The exception that is the cause of this exception.</param>
        public ClosedChannelException(Exception innerException) : base(Properties.Resources.ClosedChannelException_DefaultMessage, innerException) { }

        /// <summary>Initializes a new instance of the <see cref="ClosedChannelException"/> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of this exception.</param>
        public ClosedChannelException(string message, Exception innerException) : base(message, innerException) { }
    }
}
