// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;

namespace System.Threading.Tasks.Channels
{
    public static partial class Channel
    {
        /// <summary>Provides an observer for a writeable channel.</summary>
        private sealed class ChannelObserver<T> : IObserver<T>
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
        private sealed class ChannelObservable<T> : IObservable<T>
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

    }
}
