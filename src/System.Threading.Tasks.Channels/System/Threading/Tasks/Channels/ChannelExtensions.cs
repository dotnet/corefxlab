// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks.Channels
{
    /// <summary>Provides extension methods for manipulating channel instances.</summary>
    public static class ChannelExtensions
    {
        /// <summary>Table mapping from a channel to the shared observable wrapping it.</summary>
        private static readonly ConditionalWeakTable<object, object> s_channelToObservable = new ConditionalWeakTable<object, object>();

        /// <summary>Creates an observable for a channel.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <param name="source">The channel to be treated as an observable.</param>
        /// <returns>An observable that pulls data from the source.</returns>
        public static IObservable<T> AsObservable<T>(this IReadableChannel<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return (IObservable<T>)s_channelToObservable.GetValue(
                source, 
                s => new ChannelObservable<T>((IReadableChannel<T>)s));
        }

        /// <summary>Creates an observer for a writeable channel.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <param name="target">The channel to be treated as an observer.</param>
        /// <returns>An observer that forwards to the specified channel.</returns>
        public static IObserver<T> AsObserver<T>(this IWritableChannel<T> target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            return new ChannelObserver<T>(target);
        }

        /// <summary>Gets an awaiter that enables directly awaiting a channel to read data from it.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <param name="channel">The channel to await and from which to read.</param>
        /// <returns>An awaiter for reading data from the channel.</returns>
        /// <remarks>
        /// Getting the awaiter will initiate a read operation on the channel.
        /// </remarks>
        public static ValueTaskAwaiter<T> GetAwaiter<T>(this IReadableChannel<T> channel)
        {
            // No explicit null check.  await'ing something that's null can produce a NullReferenceException.
            return channel.ReadAsync().GetAwaiter();
        }

        /// <summary>Gets an async enumerator of the data in the channel.</summary>
        /// <typeparam name="T">Specifies the type of data being enumerated.</typeparam>
        /// <param name="channel">The channel from which to read data.</param>
        /// <param name="cancellationToken">The cancellation token to use to cancel the asynchronous enumeration.</param>
        /// <returns>The async enumerator.</returns>
        public static IAsyncEnumerator<T> GetAsyncEnumerator<T>(
            this IReadableChannel<T> channel, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (channel == null)
                throw new ArgumentNullException(nameof(channel));

            return new AsyncEnumerator<T>(channel, cancellationToken);
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

        /// <summary>Mark the channel as being complete, meaning no more items will be written to it.</summary>
        /// <param name="channel">The channel to mark as complete.</param>
        /// <param name="error">Optional Exception indicating a failure that's causing the channel to complete.</param>
        /// <exception cref="InvalidOperationException">The channel has already been marked as complete.</exception>
        public static void Complete<T>(this IWritableChannel<T> channel, Exception error = null)
        {
            if (channel == null)
                throw new ArgumentNullException(nameof(channel));
            if (!channel.TryComplete(error))
                throw ChannelUtilities.CreateInvalidCompletionException();
        }

        /// <summary>Provides an observer for a writeable channel.</summary>
        internal sealed class ChannelObserver<T> : IObserver<T>
        {
            private readonly IWritableChannel<T> _channel;

            internal ChannelObserver(IWritableChannel<T> channel)
            {
                Debug.Assert(channel != null);
                _channel = channel;
            }

            public void OnCompleted() => _channel.Complete();

            public void OnError(Exception error) => _channel.Complete(error);

            public void OnNext(T value) => _channel.WriteAsync(value).GetAwaiter().GetResult();
        }

        /// <summary>Provides an observable for a readable channel.</summary>
        internal sealed class ChannelObservable<T> : IObservable<T>
        {
            private readonly List<IObserver<T>> _observers = new List<IObserver<T>>();
            private readonly IReadableChannel<T> _channel;
            private CancellationTokenSource _cancelCurrentOperation;

            internal ChannelObservable(IReadableChannel<T> channel)
            {
                Debug.Assert(channel != null);
                _channel = channel;
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                if (observer == null)
                    throw new ArgumentNullException(nameof(observer));

                lock (_observers)
                {
                    _observers.Add(observer);
                    if (_cancelCurrentOperation == null)
                    {
                        _cancelCurrentOperation = new CancellationTokenSource();
                        CancellationToken token = _cancelCurrentOperation.Token;
                        Task.Run(() => ForwardAsync(token));
                    }
                    return new Unsubscribe(this, observer);
                }
            }

            private async void ForwardAsync(CancellationToken cancellationToken)
            {
                Exception error = null;
                try
                {
                    IAsyncEnumerator<T> e = _channel.GetAsyncEnumerator(cancellationToken);
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        lock (_observers)
                        {
                            for (int i = 0; i < _observers.Count; i++)
                            {
                                _observers[i].OnNext(e.Current);
                            }
                        }
                    }
                    await _channel.Completion.ConfigureAwait(false);
                }
                catch (Exception exc)
                {
                    error = exc;
                }
                finally
                {
                    lock (_observers)
                    {
                        for (int i = 0; i < _observers.Count; i++)
                        {
                            if (error != null)
                                _observers[i].OnError(error);
                            else
                                _observers[i].OnCompleted();
                        }
                    }
                }
            }

            private sealed class Unsubscribe : IDisposable
            {
                private readonly ChannelObservable<T> _observable;
                private IObserver<T> _observer;

                internal Unsubscribe(ChannelObservable<T> observable, IObserver<T> observer)
                {
                    Debug.Assert(observable != null);
                    Debug.Assert(observer != null);

                    _observable = observable;
                    _observer = observer;
                }

                public void Dispose()
                {
                    lock (_observable._observers)
                    {
                        if (_observer != null)
                        {
                            bool removed = _observable._observers.Remove(_observer);
                            Debug.Assert(removed);
                            _observer = null;

                            if (_observable._observers.Count == 0)
                            {
                                _observable._cancelCurrentOperation.Cancel();
                                _observable._cancelCurrentOperation = null;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>Provides an async enumerator for the data in a channel.</summary>
        internal sealed class AsyncEnumerator<T> : IAsyncEnumerator<T>
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
                    return ChannelUtilities.TrueTask;
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
