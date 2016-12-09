// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Threading.Tasks.Channels
{
    public static partial class Channel
    {
        /// <summary>Provides an async enumerator for the data in a channel.</summary>
        private sealed class AsyncEnumerator<T> : IAsyncEnumerator<T>
        {
            /// <summary>The channel being enumerated.</summary>
            private readonly IReadableChannel<T> _channel;
            /// <summary>Cancellation token used to cancel the enumeration.</summary>
            private readonly CancellationToken _cancellationToken;
            /// <summary>The current element of the enumeration.</summary>
            private T _current;

            internal AsyncEnumerator(IReadableChannel<T> channel, CancellationToken cancellationToken)
            {
                _channel = channel;
                _cancellationToken = cancellationToken;
            }

            public T Current => _current;

            public Task<bool> MoveNextAsync()
            {
                ValueTask<T> result = _channel.ReadAsync(_cancellationToken);

                if (result.IsCompletedSuccessfully)
                {
                    _current = result.Result;
                    return s_trueTask;
                }

                return result.AsTask().ContinueWith((t, s) =>
                {
                    AsyncEnumerator<T> thisRef = (AsyncEnumerator<T>)s;
                    try
                    {
                        thisRef._current = t.GetAwaiter().GetResult();
                        return true;
                    }
                    catch (ClosedChannelException)
                    {
                        return false;
                    }
                }, this, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
            }

        }
    }
}
