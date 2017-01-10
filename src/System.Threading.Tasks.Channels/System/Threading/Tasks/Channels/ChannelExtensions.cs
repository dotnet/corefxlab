// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// TODO: All of these would become virtuals on Readable/WritableChannel

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
        public static IObservable<T> AsObservable<T>(this ReadableChannel<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return (IObservable<T>)s_channelToObservable.GetValue(
                source, 
                s => new ChannelObservable<T>((ReadableChannel<T>)s));
        }

        /// <summary>Creates an observer for a Writable channel.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <param name="target">The channel to be treated as an observer.</param>
        /// <returns>An observer that forwards to the specified channel.</returns>
        public static IObserver<T> AsObserver<T>(this WritableChannel<T> target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            return new ChannelObserver<T>(target);
        }

        /// <summary>Gets an async enumerator of the data in the channel.</summary>
        /// <typeparam name="T">Specifies the type of data being enumerated.</typeparam>
        /// <param name="channel">The channel from which to read data.</param>
        /// <param name="cancellationToken">The cancellation token to use to cancel the asynchronous enumeration.</param>
        /// <returns>The async enumerator.</returns>
        public static IAsyncEnumerator<T> GetAsyncEnumerator<T>(
            this ReadableChannel<T> channel, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (channel == null)
            {
                throw new ArgumentNullException(nameof(channel));
            }

            return new AsyncEnumerator<T>(channel, cancellationToken);
        }

        /// <summary>Mark the channel as being complete, meaning no more items will be written to it.</summary>
        /// <param name="channel">The channel to mark as complete.</param>
        /// <param name="error">Optional Exception indicating a failure that's causing the channel to complete.</param>
        /// <exception cref="InvalidOperationException">The channel has already been marked as complete.</exception>
        public static void Complete<T>(this WritableChannel<T> channel, Exception error = null)
        {
            if (channel == null)
            {
                throw new ArgumentNullException(nameof(channel));
            }

            if (!channel.TryComplete(error))
            {
                throw ChannelUtilities.CreateInvalidCompletionException();
            }
        }

        /// <summary>Provides an observer for a Writable channel.</summary>
        internal sealed class ChannelObserver<T> : IObserver<T>
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

        /// <summary>Provides an observable for a readable channel.</summary>
        internal sealed class ChannelObservable<T> : IObservable<T>
        {
            private readonly List<IObserver<T>> _observers = new List<IObserver<T>>();
            private readonly ReadableChannel<T> _channel;
            private bool _active;

            internal ChannelObservable(ReadableChannel<T> channel)
            {
                Debug.Assert(channel != null);
                _channel = channel;
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                if (observer == null)
                {
                    throw new ArgumentNullException(nameof(observer));
                }

                lock (_observers)
                {
                    _observers.Add(observer);
                    QueueLoopIfNecessary();
                }
                return new Unsubscribe(this, observer);
            }

            private void QueueLoopIfNecessary()
            {
                Debug.Assert(Monitor.IsEntered(_observers));
                if (!_active && _observers.Count > 0)
                {
                    _active = true;
                    Task.Factory.StartNew(s => ((ChannelObservable<T>)s).ForwardLoopAsync(), this,
                        CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
                }
            }

            private async void ForwardLoopAsync()
            {
                Exception error = null;
                try
                {
                    while (await _channel.WaitToReadAsync().ConfigureAwait(false))
                    {
                        lock (_observers)
                        {
                            if (_observers.Count == 0)
                            {
                                break;
                            }

                            T item;
                            if (_channel.TryRead(out item))
                            {
                                foreach (IObserver<T> observer in _observers)
                                {
                                    observer.OnNext(item);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    error = e;
                }
                finally
                {
                    lock (_observers)
                    {
                        _active = false;
                        if (_channel.Completion.IsCompleted)
                        {
                            if (error == null)
                            {
                                if (_channel.Completion.IsFaulted)
                                {
                                    error = _channel.Completion.Exception.InnerException;
                                    Debug.Assert(error != null);
                                }
                                else if (_channel.Completion.IsCanceled)
                                {
                                    try { _channel.Completion.GetAwaiter().GetResult(); }
                                    catch (Exception e) { error = e; }
                                    Debug.Assert(error != null);
                                }
                            }

                            foreach (IObserver<T> observer in _observers)
                            {
                                if (error != null)
                                {
                                    observer.OnError(error);
                                }
                                else
                                {
                                    observer.OnCompleted();
                                }
                            }

                            _observers.Clear();
                        }
                        else
                        {
                            QueueLoopIfNecessary();
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
                        }
                    }
                }
            }
        }

        /// <summary>Provides an async enumerator for the data in a channel.</summary>
        internal sealed class AsyncEnumerator<T> : IAsyncEnumerator<T>
        {
            /// <summary>The channel being enumerated.</summary>
            private readonly ReadableChannel<T> _channel;
            /// <summary>Cancellation token used to cancel the enumeration.</summary>
            private readonly CancellationToken _cancellationToken;
            /// <summary>The current element of the enumeration.</summary>
            private T _current;

            internal AsyncEnumerator(ReadableChannel<T> channel, CancellationToken cancellationToken)
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
