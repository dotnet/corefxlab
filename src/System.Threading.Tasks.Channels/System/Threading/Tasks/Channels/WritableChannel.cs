// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Channels
{
    /// <summary>
    /// Provides a base class for channels that support writing elements.
    /// </summary>
    /// <typeparam name="T">Specifies the type of data that may be written to the channel.</typeparam>
    public abstract class WritableChannel<T>
    {
        /// <summary>Attempts to mark the channel as being completed, meaning no more data will be written to it.</summary>
        /// <param name="error">An <see cref="Exception"/> indicating the failure causing no more data to be written, or null for success.</param>
        /// <returns>
        /// true if this operation successfully completes the channel; otherwise, false if the channel could not be marked for completion,
        /// for example due to having already been marked as such, or due to not supporting completion.
        /// </returns>
        public virtual bool TryComplete(Exception error = null) => false;

        /// <summary>Attempts to write the specified item to the channel.</summary>
        /// <param name="item">The item to write.</param>
        /// <returns>true if the item was written; otherwise, false if it wasn't written.</returns>
        public abstract bool TryWrite(T item);

        /// <summary>Returns a <see cref="Task{Boolean}"/> that will complete when space is available to write an item.</summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the wait operation.</param>
        /// <returns>
        /// A <see cref="Task{Boolean}"/> that will complete with a <c>true</c> result when space is available to write an item
        /// or with a <c>false</c> result when no further writing will be permitted.
        /// </returns>
        public abstract Task<bool> WaitToWriteAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Asynchronously writes an item to the channel.</summary>
        /// <param name="item">The value to write to the channel.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the write operation.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous write operation.</returns>
        public virtual Task WriteAsync(T item, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                return
                    cancellationToken.IsCancellationRequested ? Task.FromCanceled<T>(cancellationToken) :
                    TryWrite(item) ? Task.CompletedTask :
                    WriteAsyncCore(item, cancellationToken);
            }
            catch (Exception e)
            {
                return Task.FromException(e);
            }

            async Task WriteAsyncCore(T innerItem, CancellationToken ct)
            {
                while (await WaitToWriteAsync(ct).ConfigureAwait(false))
                {
                    if (TryWrite(innerItem))
                    {
                        return;
                    }
                }

                throw ChannelUtilities.CreateInvalidCompletionException();
            }
        }

        /// <summary>Gets an awaiter used to wait for space to be available in the channel.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual ValueAwaiter<bool> GetAwaiter() => new ValueAwaiter<bool>(WaitToWriteAsync());

        /// <summary>Creates an observer for this channel.</summary>
        /// <returns>An observer that forwards to this channel.</returns>
        public virtual IObserver<T> AsObserver() => new ChannelObserver(this);

        /// <summary>Mark the channel as being complete, meaning no more items will be written to it.</summary>
        /// <param name="error">Optional Exception indicating a failure that's causing the channel to complete.</param>
        /// <exception cref="InvalidOperationException">The channel has already been marked as complete.</exception>
        public void Complete(Exception error = null)
        {
            if (!TryComplete(error))
            {
                throw ChannelUtilities.CreateInvalidCompletionException();
            }
        }

        /// <summary>Provides an observer for a Writable channel.</summary>
        private sealed class ChannelObserver : IObserver<T>
        {
            private readonly WritableChannel<T> _channel;

            internal ChannelObserver(WritableChannel<T> channel)
            {
                Debug.Assert(channel != null);
                _channel = channel;
            }

            public void OnCompleted() => _channel.Complete();

            public void OnError(Exception error) => _channel.Complete(error);

            public void OnNext(T value) => _channel.WriteAsync(value).GetAwaiter().GetResult();
        }
    }
}
