// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Threading.Tasks.Channels
{
    public static partial class Channel
    {
        internal abstract class Interactor<T> : TaskCompletionSource<T>
        {
            protected Interactor() : base(TaskCreationOptions.RunContinuationsAsynchronously) { }

            internal bool Success(T item)
            {
                bool transitionedToCompleted = TrySetResult(item);
                if (transitionedToCompleted)
                {
                    Dispose();
                }
                return transitionedToCompleted;
            }

            internal bool Fail(Exception exception)
            {
                bool transitionedToCompleted = TrySetException(exception);
                if (transitionedToCompleted)
                {
                    Dispose();
                }
                return transitionedToCompleted;
            }

            protected virtual void Dispose() { }
        }

        internal class Reader<T> : Interactor<T>
        {
            internal static Reader<T> Create(CancellationToken cancellationToken) =>
                cancellationToken.CanBeCanceled ?
                    new CancelableReader<T>(cancellationToken) :
                    new Reader<T>();
        }

        internal class Writer<T> : Interactor<VoidResult>
        {
            internal T Item { get; private set; }

            internal static Writer<T> Create(CancellationToken cancellationToken, T item)
            {
                Writer<T> w = cancellationToken.CanBeCanceled ?
                    new CancelableWriter<T>(cancellationToken) :
                    new Writer<T>();
                w.Item = item;
                return w;
            }
        }

        internal sealed class CancelableReader<T> : Reader<T>
        {
            private CancellationToken _token;
            private CancellationTokenRegistration _registration;

            internal CancelableReader(CancellationToken cancellationToken)
            {
                _token = cancellationToken;
                _registration = cancellationToken.Register(s => {
                    var thisRef = (CancelableReader<T>)s;
                    thisRef.TrySetCanceled(thisRef._token);
                }, this);
            }

            protected override void Dispose() => _registration.Dispose();
        }

        internal sealed class CancelableWriter<T> : Writer<T>
        {
            private CancellationToken _token;
            private CancellationTokenRegistration _registration;

            internal CancelableWriter(CancellationToken cancellationToken)
            {
                _token = cancellationToken;
                _registration = cancellationToken.Register(s => {
                    var thisRef = (CancelableWriter<T>)s;
                    thisRef.TrySetCanceled(thisRef._token);
                }, this);
            }

            protected override void Dispose() => _registration.Dispose();
        }

    }
}
