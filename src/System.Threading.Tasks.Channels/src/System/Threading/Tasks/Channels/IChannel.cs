// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Threading.Tasks.Channels
{
    /// <summary>Represents a channel to and from which data can be written and read.</summary>
    /// <typeparam name="T">Specifies the type of data items readable from and writable to the channel.</typeparam>
    public interface IChannel<T> : IChannel<T, T>
    {
    }

    /// <summary>Represents a channel to and from which data can be written and read.</summary>
    /// <typeparam name="TInput">Specifies the type of data items writable to the channel.</typeparam>
    /// <typeparam name="TOutput">Specifies the type of data items readable from the channel.</typeparam>
    public interface IChannel<TInput, TOutput> : IWriteableChannel<TInput>, IReadableChannel<TOutput>
    {
    }

    /// <summary>Represents a channel from which data can be read.</summary>
    /// <typeparam name="T">Specifies the type of data items readable from the channel.</typeparam>
    public interface IReadableChannel<T>
    {
        /// <summary>Asynchronously reads a data item from the channel.</summary>
        /// <param name="cancellationToken">The cancellation token to use to cancel the operation.</param>
        /// <returns>A task representing the asynchronous read operation.</returns>
        ValueTask<T> ReadAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Asynchronously waits for a data to be available.</summary>
        /// <param name="cancellationToken">The cancellation token to use to cancel the operation.</param>
        /// <returns>A task representing the asynchronous wait.</returns>
        Task<bool> WaitToReadAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Attempt to read an item from the channel.</summary>
        /// <param name="item">If the read succeeds, the resulting read item.</param>
        /// <returns>true if an item was read; false if no item could be read.</returns>
        bool TryRead(out T item);

        /// <summary>Gets a task that completes when the channel is completed and has no more data to be read.</summary>
        Task Completion { get; }
    }

    /// <summary>Represents a channel to which data can be written.</summary>
    /// <typeparam name="T">Specifies the type of data items writable to the channel.</typeparam>
    public interface IWriteableChannel<in T>
    {
        /// <summary>Asynchronously writes a data item to the channel.</summary>
        /// <param name="item">The item to write.</param>
        /// <param name="cancellationToken">The cancellation token to use to cancel the operation.</param>
        /// <returns>A task representing the asynchronous write operation.</returns>
        Task WriteAsync(T item, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Asynchronously waits for space to be available.</summary>
        /// <param name="cancellationToken">The cancellation token to use to cancel the operation.</param>
        /// <returns>A task representing the asynchronous wait.</returns>
        Task<bool> WaitToWriteAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Attempt to write an item to the channel.</summary>
        /// <param name="item">The item to write.</param>
        /// <returns>true if the item was written to the channel; otherwise, false.</returns>
        bool TryWrite(T item);

        /// <summary>Marks the channel is being complete, meaning no more items will be written to it.</summary>
        /// <param name="error">Optional Exception indicating a failure that's causing the channel to complete.</param>
        void Complete(Exception error = null);
    }
}
