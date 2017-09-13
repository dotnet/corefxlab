// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


namespace System.IO.Channels
{
    /// <summary>
    /// Provides a base class for channels that support reading elements of type <typeparamref name="TRead"/>
    /// and writing elements of type <typeparamref name="TWrite"/>.
    /// </summary>
    /// <typeparam name="TWrite">Specifies the type of data that may be written to the channel.</typeparam>
    /// <typeparam name="TRead">Specifies the type of data that may be read from the channel.</typeparam>
    public abstract class Channel<TWrite, TRead>
    {
        /// <summary>Gets the readable half of this channel.</summary>
        public ReadableChannel<TRead> In { get; protected set; }

        /// <summary>Gets the writable half of this channel.</summary>
        public WritableChannel<TWrite> Out { get; protected set; }

        /// <summary>Implicit cast from a channel to its readable half.</summary>
        /// <param name="channel">The channel being cast.</param>
        public static implicit operator ReadableChannel<TRead>(Channel<TWrite, TRead> channel) => channel.In;

        /// <summary>Implicit cast from a channel to its writable half.</summary>
        /// <param name="channel">The channel being cast.</param>
        public static implicit operator WritableChannel<TWrite>(Channel<TWrite, TRead> channel) => channel.Out;
    }
}
