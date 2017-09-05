// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Threading.Tasks.Channels
{
    /// <summary>Provides static methods for creating channels.</summary>
    public static class Channel
    {
        /// <summary>Creates an unbounded channel usable by any number of readers and writers concurrently.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <param name="optimizations">Controls optimizations that may be applied to the channel.</param>
        /// <returns>The created channel.</returns>
        public static Channel<T> CreateUnbounded<T>(ChannelOptimizations optimizations = null) =>
            optimizations == null ? new UnboundedChannel<T>(runContinuationsAsynchronously: true) :
            optimizations.SingleReader ? new SingleConsumerUnboundedChannel<T>(!optimizations.AllowSynchronousContinuations) :
            (Channel<T>)new UnboundedChannel<T>(!optimizations.AllowSynchronousContinuations);

        /// <summary>Creates a channel with the specified bounded size.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <param name="bufferedCapacity">The maximum number of elements the channel can store.</param>
        /// <param name="mode">The behavior to use when writing to a full channel.</param>
        /// <param name="optimizations">Controls optimizations that may be applied to the channel.</param>
        /// <returns>The created channel.</returns>
        public static Channel<T> CreateBounded<T>(int bufferedCapacity, BoundedChannelFullMode mode = BoundedChannelFullMode.Wait, ChannelOptimizations optimizations = null)
        {
            if (bufferedCapacity < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(bufferedCapacity));
            }
            if (mode != BoundedChannelFullMode.DropNewest &&
                mode != BoundedChannelFullMode.DropOldest &&
                mode != BoundedChannelFullMode.Wait)
            {
                throw new ArgumentOutOfRangeException(nameof(mode));
            }

            bool runContinuationsAsynchronously = optimizations == null || !optimizations.AllowSynchronousContinuations;
            return new BoundedChannel<T>(bufferedCapacity, mode, runContinuationsAsynchronously);
        }

        /// <summary>Creates a channel that doesn't buffer any items.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <param name="optimizations">Controls optimizations that may be applied to the channel.</param>
        /// <returns>The created channel.</returns>
        public static Channel<T> CreateUnbuffered<T>(ChannelOptimizations optimizations = null) => new UnbufferedChannel<T>();
    }
}
