// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Threading.Tasks.Channels
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

    internal class ReaderInteractor<T> : Interactor<T>
    {
        internal static ReaderInteractor<T> Create(CancellationToken cancellationToken) =>
            cancellationToken.CanBeCanceled ?
                new CancelableReaderInteractor<T>(cancellationToken) :
                new ReaderInteractor<T>();
    }

    internal class WriterInteractor<T> : Interactor<VoidResult>
    {
        internal T Item { get; private set; }

        internal static WriterInteractor<T> Create(CancellationToken cancellationToken, T item)
        {
            WriterInteractor<T> w = cancellationToken.CanBeCanceled ?
                new CancelableWriter<T>(cancellationToken) :
                new WriterInteractor<T>();
            w.Item = item;
            return w;
        }
    }

    internal sealed class CancelableReaderInteractor<T> : ReaderInteractor<T>
    {
        private CancellationToken _token;
        private CancellationTokenRegistration _registration;

        internal CancelableReaderInteractor(CancellationToken cancellationToken)
        {
            _token = cancellationToken;
            _registration = cancellationToken.Register(s =>
            {
                var thisRef = (CancelableReaderInteractor<T>)s;
                thisRef.TrySetCanceled(thisRef._token);
            }, this);
        }

        protected override void Dispose() => _registration.Dispose();
    }

    internal sealed class CancelableWriter<T> : WriterInteractor<T>
    {
        private CancellationToken _token;
        private CancellationTokenRegistration _registration;

        internal CancelableWriter(CancellationToken cancellationToken)
        {
            _token = cancellationToken;
            _registration = cancellationToken.Register(s =>
            {
                var thisRef = (CancelableWriter<T>)s;
                thisRef.TrySetCanceled(thisRef._token);
            }, this);
        }

        protected override void Dispose() => _registration.Dispose();
    }
}
