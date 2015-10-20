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
    public interface IChannel<TInput, TOutput> : IWritableChannel<TInput>, IReadableChannel<TOutput>
    {
    }

    /// <summary>Represents a channel from which data can be read.</summary>
    /// <typeparam name="T">Specifies the type of data items readable from the channel.</typeparam>
    public interface IReadableChannel<T>
    {
        /// <summary>Asynchronously reads a data item from the channel.</summary>
        /// <param name="cancellationToken">The cancellation token to use to cancel the operation.</param>
        /// <returns>A task representing the asynchronous read operation.</returns>
        /// <remarks>
        /// The returned task will complete unsuccessfully if the channel is closed and emptied
        /// before the read can be satisfied.
        /// </remarks>
        ValueTask<T> ReadAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Asynchronously waits for a data to be available.</summary>
        /// <param name="cancellationToken">The cancellation token to use to cancel the operation.</param>
        /// <returns>
        /// A task representing the asynchronous wait.  The task will complete with a result of true
        /// when the channel can be read from (however, concurrent access could change that status).
        /// The task will complete with a result of false when the channel is closed and emptied and won't
       ///  be producing any more readable data.
        /// </returns>
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
    public interface IWritableChannel<in T>
    {
        /// <summary>Asynchronously writes a data item to the channel.</summary>
        /// <param name="item">The item to write.</param>
        /// <param name="cancellationToken">The cancellation token to use to cancel the operation.</param>
        /// <returns>A task representing the asynchronous write operation.</returns>
        /// <remarks>The returned task will be complete unsuccessfully if the channel is closed before the write completes.</remarks>
        Task WriteAsync(T item, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Asynchronously waits for space to be available.</summary>
        /// <param name="cancellationToken">The cancellation token to use to cancel the operation.</param>
        /// <returns>
        /// A task representing the asynchronous wait.  The task will complete with a result of true
        /// when the channel can be written to (however, concurrent access could change that status).
        /// The task will complete with a result of false when the channel is closed and won't be
        /// accepting any more data.
        /// </returns>
        Task<bool> WaitToWriteAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Attempt to write an item to the channel.</summary>
        /// <param name="item">The item to write.</param>
        /// <returns>true if the item was written to the channel; otherwise, false.</returns>
        bool TryWrite(T item);

        /// <summary>Attempt to mark the channel as being complete, meaning no more items will be written to it.</summary>
        /// <param name="error">Optional Exception indicating a failure that's causing the channel to complete.</param>
        /// <returns>true if the channel is successfully marked as complete; otherwise, false if the channel is already marked complete.</returns>
        bool TryComplete(Exception error = null);
    }
}
