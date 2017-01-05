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
        /// <returns>The created channel.</returns>
        public static IChannel<T> CreateUnbounded<T>() => new UnboundedChannel<T>();

        /// <summary>Creates an unbounded channel usable by any number of readers and writers concurrently.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <param name="optimizations">Controls optimizations that may be applied to the channel.</param>
        /// <returns>The created channel.</returns>
        public static IChannel<T> CreateUnbounded<T>(ChannelOptimizations optimizations)
        {
            if (optimizations == null)
            {
                throw new ArgumentNullException(nameof(optimizations));
            }

            return optimizations.SingleReader ?
                (IChannel<T>)new SingleConsumerUnboundedChannel<T>() :
                new UnboundedChannel<T>();
        }

        /// <summary>Creates a channel that doesn't buffer any items.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <returns>The created channel.</returns>
        public static IChannel<T> CreateUnbuffered<T>() => new UnbufferedChannel<T>();

        /// <summary>Creates a channel with the specified bounded size.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <param name="bufferedCapacity">The maximum number of elements the channel can store.</param>
        /// <param name="mode">The behavior to use when writing to a full channel.</param>
        /// <returns>The created channel.</returns>
        public static IChannel<T> CreateBounded<T>(int bufferedCapacity, BoundedChannelFullMode mode = BoundedChannelFullMode.Wait)
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

            return new BoundedChannel<T>(bufferedCapacity, mode);
        }

        /// <summary>Creates a case-select builder and adds a case for channel reading.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <param name="channel">The channel from which to read.</param>
        /// <param name="action">The action to invoke with data read from the channel.</param>
        /// <returns>This builder.</returns>
        public static CaseBuilder CaseRead<T>(IReadableChannel<T> channel, Action<T> action) => new CaseBuilder().CaseRead(channel, action);

        /// <summary>Creates a case-select builder and adds a case for channel reading.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <param name="channel">The channel from which to read.</param>
        /// <param name="func">The asynchronous function to invoke with data read from the channel.</param>
        /// <returns>This builder.</returns>
        public static CaseBuilder CaseRead<T>(IReadableChannel<T> channel, Func<T, Task> func) => new CaseBuilder().CaseRead(channel, func);

        /// <summary>Creates a case-select builder and adds a case for channel writing.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <param name="channel">The channel to which to write.</param>
        /// <param name="item">The data to write to the channel</param>
        /// <param name="action">The action to invoke after the data has been written.</param>
        /// <returns>This builder.</returns>
        public static CaseBuilder CaseWrite<T>(IWritableChannel<T> channel, T item, Action action) => new CaseBuilder().CaseWrite(channel, item, action);

        /// <summary>Creates a case-select builder and adds a case for channel writing.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <param name="channel">The channel to which to write.</param>
        /// <param name="item">The data to write to the channel</param>
        /// <param name="func">The asynchronous function to invoke after the data has been written.</param>
        /// <returns>This builder.</returns>
        public static CaseBuilder CaseWrite<T>(IWritableChannel<T> channel, T item, Func<Task> func) => new CaseBuilder().CaseWrite(channel, item, func);
    }
}
