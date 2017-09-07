// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using Xunit;

#pragma warning disable 0649 // unused fields there for future testing needs

namespace System.Threading.Tasks.Channels.Tests
{
    public abstract class TestBase
    {
        protected void AssertSynchronouslyCanceled(Task task, CancellationToken token)
        {
            Assert.Equal(TaskStatus.Canceled, task.Status);
            OperationCanceledException oce = Assert.ThrowsAny<OperationCanceledException>(() => task.GetAwaiter().GetResult());
            Assert.Equal(token, oce.CancellationToken);
        }

        protected async Task AssertCanceled(Task task, CancellationToken token)
        {
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => task);
            AssertSynchronouslyCanceled(task, token);
        }

        protected void AssertSynchronousSuccess(Task task) => Assert.Equal(TaskStatus.RanToCompletion, task.Status);

        protected void AssertSynchronousTrue(Task<bool> task)
        {
            AssertSynchronousSuccess(task);
            Assert.True(task.Result);
        }

        protected void AssertSynchronousFalse(Task<bool> task)
        {
            AssertSynchronousSuccess(task);
            Assert.False(task.Result);
        }

        internal sealed class DelegateEnumerable<T> : IEnumerable<T>
        {
            public Func<IEnumerator<T>> GetEnumeratorDelegate;

            public IEnumerator<T> GetEnumerator() =>
                GetEnumeratorDelegate != null ? 
                    GetEnumeratorDelegate() :
                    null;

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        internal sealed class DelegateEnumerator<T> : IEnumerator<T>
        {
            public Func<T> CurrentDelegate;
            public Action DisposeDelegate;
            public Func<bool> MoveNextDelegate;
            public Action ResetDelegate;

            public T Current => CurrentDelegate != null ? CurrentDelegate() : default(T);

            object IEnumerator.Current => Current;

            public void Dispose() => DisposeDelegate?.Invoke();

            public bool MoveNext() => MoveNextDelegate?.Invoke() ?? false;

            public void Reset() => ResetDelegate?.Invoke();
        }

        internal sealed class DelegateObserver<T> : IObserver<T>
        {
            public Action<T> OnNextDelegate = null;
            public Action<Exception> OnErrorDelegate = null;
            public Action OnCompletedDelegate = null;

            void IObserver<T>.OnNext(T value) => OnNextDelegate?.Invoke(value);

            void IObserver<T>.OnError(Exception error) => OnErrorDelegate?.Invoke(error);

            void IObserver<T>.OnCompleted() => OnCompletedDelegate?.Invoke();
        }
    }
}
