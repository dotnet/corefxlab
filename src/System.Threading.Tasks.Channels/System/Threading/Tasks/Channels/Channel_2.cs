// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks.Channels
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
        public abstract ReadableChannel<TRead> In { get; }

        /// <summary>Gets the writable half of this channel.</summary>
        public abstract WritableChannel<TWrite> Out { get; }

        // The following non-virtuals are all convenience members that wrap In and Out.

        /// <summary>Implicit cast from a channel to its readable half.</summary>
        /// <param name="channel">The channel being cast.</param>
        public static implicit operator ReadableChannel<TRead>(Channel<TWrite, TRead> channel) => channel.In;

        /// <summary>Implicit cast from a channel to its writable half.</summary>
        /// <param name="channel">The channel being cast.</param>
        public static implicit operator WritableChannel<TWrite>(Channel<TWrite, TRead> channel) => channel.Out;

        /// <summary>
        /// Gets a <see cref="Task"/> that completes when no more data will ever
        /// be available to be read from this channel.
        /// </summary>
        public Task Completion => In.Completion;

        /// <summary>Gets an awaiter used to read an item from the channel.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ValueAwaiter<TRead> GetAwaiter() => In.GetAwaiter();
        
        /// <summary>Asynchronously reads an item from the channel.</summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the read operation.</param>
        /// <returns>A <see cref="ValueTask{TResult}"/> that represents the asynchronous read operation.</returns>
        public ValueTask<TRead> ReadAsync(CancellationToken cancellationToken = default(CancellationToken)) => In.ReadAsync(cancellationToken);

        /// <summary>Attempts to mark the channel as being completed, meaning no more data will be written to it.</summary>
        /// <param name="error">An <see cref="Exception"/> indicating the failure causing no more data to be written, or null for success.</param>
        /// <returns>
        /// true if this operation successfully completes the channel; otherwise, false if the channel could not be marked for completion,
        /// for example due to having already been marked as such.
        /// </returns>
        public bool TryComplete(Exception error = null) => Out.TryComplete(error);

        /// <summary>Attempts to read an item to the channel.</summary>
        /// <param name="item">The read item, or a default value if no item could be read.</param>
        /// <returns>true if an item was read; otherwise, false if no item was read.</returns>
        public bool TryRead(out TRead item) => In.TryRead(out item);

        /// <summary>Attempts to write the specified item to the channel.</summary>
        /// <param name="item">The item to write.</param>
        /// <returns>true if the item was written; otherwise, false if it wasn't written.</returns>
        public bool TryWrite(TWrite item) => Out.TryWrite(item);

        /// <summary>Returns a <see cref="Task{Boolean}"/> that will complete when data is available to read.</summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the wait operation.</param>
        /// <returns>
        /// A <see cref="Task{Boolean}"/> that will complete with a <c>true</c> result when data is available to read
        /// or with a <c>false</c> result when no further data will ever be available to be read.
        /// </returns>
        public Task<bool> WaitToReadAsync(CancellationToken cancellationToken = default(CancellationToken)) => In.WaitToReadAsync(cancellationToken);

        /// <summary>Returns a <see cref="Task{Boolean}"/> that will complete when space is available to write an item.</summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the wait operation.</param>
        /// <returns>
        /// A <see cref="Task{Boolean}"/> that will complete with a <c>true</c> result when space is available to write an item
        /// or with a <c>false</c> result when no further writing will be permitted.
        /// </returns>
        public Task<bool> WaitToWriteAsync(CancellationToken cancellationToken = default(CancellationToken)) => Out.WaitToWriteAsync(cancellationToken);

        /// <summary>Asynchronously writes an item to the channel.</summary>
        /// <param name="item">The value to write to the channel.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the write operation.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous write operation.</returns>
        public Task WriteAsync(TWrite item, CancellationToken cancellationToken = default(CancellationToken)) => Out.WriteAsync(item, cancellationToken);

        /// <summary>Creates an observable for this channel.</summary>
        /// <returns>An observable that pulls data from this channel.</returns>
        public IObservable<TRead> AsObservable() => In.AsObservable();

        /// <summary>Creates an observer for this channel.</summary>
        /// <returns>An observer that forwards to this channel.</returns>
        public IObserver<TWrite> AsObserver() => Out.AsObserver();

        /// <summary>Mark the channel as being complete, meaning no more items will be written to it.</summary>
        /// <param name="error">Optional Exception indicating a failure that's causing the channel to complete.</param>
        /// <exception cref="InvalidOperationException">The channel has already been marked as complete.</exception>
        public void Complete(Exception error = null) => Out.Complete(error);

        /// <summary>Gets an async enumerator of the data in this channel.</summary>
        /// <param name="cancellationToken">The cancellation token to use to cancel the asynchronous enumeration.</param>
        /// <returns>The async enumerator.</returns>
        public IAsyncEnumerator<TRead> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken)) => In.GetAsyncEnumerator();
    }
}
