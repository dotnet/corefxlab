// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Threading.Tasks.Channels
{
    /// <summary>Provides static methods for creating channels.</summary>
    public static partial class Channel
    {
        /// <summary>Sentinel value to indicate an infinite bound.</summary>
        public const int Unbounded = -1;

        /// <summary>
        /// Creates a buffered channel.  If the specified <paramref name="bufferedCapacity"/> is not <see cref="Unbounded"/>,
        /// the channel may only store up to that number of items; attempts to store more than that will result in writes
        /// being delayed.
        /// </summary>
        /// <typeparam name="T">Specifies the type of data stored in the channel.</typeparam>
        /// <returns>The new channel.</returns>
        public static IChannel<T> Create<T>(int bufferedCapacity = Unbounded, bool singleReaderWriter = false)
        {
            if (bufferedCapacity <= 0 && bufferedCapacity != Unbounded)
                throw new ArgumentOutOfRangeException(nameof(bufferedCapacity));

            return bufferedCapacity == Unbounded ?
                singleReaderWriter ? 
                    (IChannel<T>)new SpscUnboundedChannel<T>() : 
                    new UnboundedChannel<T>() :
                new BoundedChannel<T>(bufferedCapacity);
        }

        /// <summary>
        /// Creates an unbuffered channel.  As the resulting channel is unbuffered, readers and writers will not complete
        /// until a corresponding reader or writer is available.
        /// </summary>
        /// <typeparam name="T">Specifies the type of data stored in the channel.</typeparam>
        /// <returns>The new channel.</returns>
        public static IChannel<T> CreateUnbuffered<T>() => new UnbufferedChannel<T>();

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
