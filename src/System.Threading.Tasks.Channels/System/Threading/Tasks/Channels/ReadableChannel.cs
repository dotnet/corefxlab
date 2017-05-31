// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks.Channels
{
    /// <summary>
    /// Provides a base class for channels that support reading elements.
    /// </summary>
    /// <typeparam name="T">Specifies the type of data that may be read from the channel.</typeparam>
    public abstract class ReadableChannel<T>
    {
        /// <summary>
        /// Gets a <see cref="Task"/> that completes when no more data will ever
        /// be available to be read from this channel.
        /// </summary>
        public abstract Task Completion { get; }

        /// <summary>Gets an awaiter used to read an item from the channel.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual ValueAwaiter<T> GetAwaiter() => new ValueAwaiter<T>(ReadAsync());

        /// <summary>Asynchronously reads an item from the channel.</summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the read operation.</param>
        /// <returns>A <see cref="ValueTask{TResult}"/> that represents the asynchronous read operation.</returns>
        public abstract ValueTask<T> ReadAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Attempts to read an item to the channel.</summary>
        /// <param name="item">The read item, or a default value if no item could be read.</param>
        /// <returns>true if an item was read; otherwise, false if no item was read.</returns>
        public abstract bool TryRead(out T item);

        /// <summary>Returns a <see cref="Task{Boolean}"/> that will complete when data is available to read.</summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the wait operation.</param>
        /// <returns>
        /// A <see cref="Task{Boolean}"/> that will complete with a <c>true</c> result when data is available to read
        /// or with a <c>false</c> result when no further data will ever be available to be read.
        /// </returns>
        public abstract Task<bool> WaitToReadAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Table mapping from a channel to the shared observable wrapping it.</summary>
        private static readonly ConditionalWeakTable<ReadableChannel<T>, ChannelObservable> s_channelToObservable =
            new ConditionalWeakTable<ReadableChannel<T>, ChannelObservable>();

        /// <summary>Creates an observable for this channel.</summary>
        /// <returns>An observable that pulls data from this channel.</returns>
        public virtual IObservable<T> AsObservable() => s_channelToObservable.GetValue(this, s => new ChannelObservable(s));

        /// <summary>Provides an observable for a readable channel.</summary>
        internal sealed class ChannelObservable : IObservable<T>
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
                    Task.Factory.StartNew(s => ((ChannelObservable)s).ForwardLoopAsync(), this,
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
                catch (ClosedChannelException exc)
                {
                    error = exc.InnerException;
                }
                catch (Exception exc)
                {
                    error = exc;
                }
                finally
                {
                    lock (_observers)
                    {
                        _active = false;
                        if (_channel.Completion.IsCompleted)
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
                private readonly ChannelObservable _observable;
                private IObserver<T> _observer;

                internal Unsubscribe(ChannelObservable observable, IObserver<T> observer)
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

        /// <summary>Gets an async enumerator of the data in this channel.</summary>
        /// <param name="cancellationToken">The cancellation token to use to cancel the asynchronous enumeration.</param>
        /// <returns>The async enumerator.</returns>
        public virtual IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken)) =>
            new AsyncEnumerator(this, cancellationToken);

        /// <summary>Provides an async enumerator for the data in a channel.</summary>
        private sealed class AsyncEnumerator : IAsyncEnumerator<T>
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
                    var thisRef = (AsyncEnumerator)s;
                    if (t.IsFaulted && t.Exception.InnerException is ClosedChannelException cce && cce.InnerException == null)
                    {
                        return false;
                    }
                    thisRef._current = t.GetAwaiter().GetResult();
                    return true;
                }, this, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
            }
        }
    }
}
