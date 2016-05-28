// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace System.Threading.Tasks.Channels
{
    public static partial class Channel
    {
        /// <summary>Provides a channel around an enumerable.</summary>
        private sealed class EnumerableChannel<T> : IReadableChannel<T>
        {
            /// <summary>A task completed when the channel is done.</summary>
            private readonly TaskCompletionSource<VoidResult> _completion = new TaskCompletionSource<VoidResult>(TaskCreationOptions.RunContinuationsAsynchronously);
            /// <summary>The wrapped enumerator.</summary>
            private readonly IEnumerator<T> _source;
            /// <summary>true if the next item to yield is _source.Current; false if _source.MoveNext() needs to be invoked.</summary>
            private bool _currentIsNext;

            /// <summary>The object used to synchronize access to state in the channel.</summary>
            private object SyncObj => _completion;

            /// <summary>Initializes the channel, getting an enumerator from the enumerable.</summary>
            internal EnumerableChannel(IEnumerable<T> source)
            {
                _source = source.GetEnumerator();
            }

            public Task Completion => _completion.Task;

            public ValueTask<T> ReadAsync(CancellationToken cancellationToken)
            {
                // Fast-path cancellation check
                if (cancellationToken.IsCancellationRequested)
                    return new ValueTask<T>(Task.FromCanceled<T>(cancellationToken));

                lock (SyncObj)
                {
                    // If we're not yet completed, ensure that _source.Current is the next item
                    // to yield, and yield it.
                    if (!_completion.Task.IsCompleted && (_currentIsNext || MoveNext()))
                    {
                        _currentIsNext = false;
                        return new ValueTask<T>(_source.Current);
                    }
                }

                // No more data is available.  Fail.
                return new ValueTask<T>(Task.FromException<T>(CreateInvalidCompletionException()));
            }

            public bool TryRead(out T item)
            {
                lock (SyncObj)
                {
                    // If we're not yet completed, ensure that _source.Current is the next item
                    // to yield, and yield it.
                    if (!_completion.Task.IsCompleted && (_currentIsNext || MoveNext()))
                    {
                        _currentIsNext = false;
                        item = _source.Current;
                        return true;
                    }
                }

                // No more data is available.
                item = default(T);
                return false;
            }

            public Task<bool> WaitToReadAsync(CancellationToken cancellationToken)
            {
                // Fast-path cancellation check
                if (cancellationToken.IsCancellationRequested)
                    return Task.FromCanceled<bool>(cancellationToken);

                lock (SyncObj)
                {
                    // If we've already pushed the enumerator ahead in a previous call and _source.Current
                    // is the next value to yield, data is available.
                    if (_currentIsNext)
                        return s_trueTask;

                    // If we're already completed, no more data is available.
                    if (_completion.Task.IsCompleted)
                        return s_falseTask;

                    // Otherwise, move next.
                    return MoveNext() ? s_trueTask : s_falseTask;
                }
            }

            private bool MoveNext()
            {
                // Move next on the enumerator.
                if (_source.MoveNext())
                {
                    _currentIsNext = true;
                    return true;
                }

                // There's no more data available.  Complete the channel.
                _source.Dispose();
                _completion.TrySetResult(default(VoidResult));
                return false;
            }
        }
    }
}
