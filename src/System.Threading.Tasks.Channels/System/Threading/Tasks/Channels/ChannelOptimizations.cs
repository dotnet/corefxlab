// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.IO.Channels
{
    /// <summary>Provides settings for possible optimizations / constraints to use when creating a channel.</summary>
    public sealed class ChannelOptimizations
    {
        /// <summary>
        /// true if code using the channel guarantees that there will only ever be at most one write operation at a time;
        /// false if no such constraint is guaranteed.  If true, the channel may be able to optimize certain operations
        /// based on knowing about the single-writer guarantee. The default is false.
        /// </summary>
        public bool SingleWriter { get; set; }
        
        /// <summary>
        /// true if code using the channel guarantees that there will only ever be at most one read operation at a time;
        /// false if no such constraint is guaranteed.  If true, the channel may be able to optimize certain operations
        /// based on knowing about the single-reader guarantee. The default is false.
        /// </summary>
        public bool SingleReader { get; set; }

        /// <summary>
        /// true if operations performed on a channel may synchronously invoke continuations subscribed to notifications
        /// of pending async operations, e.g. if a ```TryWrite``` on a channel may synchronously invoke a continuation off
        /// a task previously returned from a <see cref="ReadableChannel{T}.ReadAsync(CancellationToken)"/> call. false if all
        /// continuations should be invoked asynchronously.  This can provide measurable throughput improvements by avoiding
        /// scheduling additional work items; however, it may come at the cost of reduced parallelism, as for example a producer
        /// may then be the one to execute work associated with a consumer, and if not done thoughtfully, this can lead
        /// to unexpected interactions. The default is false.
        /// </summary>
        public bool AllowSynchronousContinuations { get; set; }
    }
}
