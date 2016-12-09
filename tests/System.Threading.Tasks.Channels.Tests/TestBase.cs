// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
            Assert.True(task.IsCanceled);
            OperationCanceledException oce = Assert.ThrowsAny<OperationCanceledException>(() => task.GetAwaiter().GetResult());
            Assert.Equal(token, oce.CancellationToken);
        }

        protected async Task AssertCanceled(Task task, CancellationToken token)
        {
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => task);
            AssertSynchronouslyCanceled(task, token);
        }

        protected void AssertSynchronousTrue(Task<bool> task)
        {
            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.True(task.Result);
        }

        protected void AssertSynchronousFalse(Task<bool> task)
        {
            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.False(task.Result);
        }

        internal sealed class DelegateEnumerable<T> : IEnumerable<T>
        {
            public Func<IEnumerator<T>> GetEnumeratorDelegate;

            public IEnumerator<T> GetEnumerator()
            {
                return GetEnumeratorDelegate != null ? 
                    GetEnumeratorDelegate() :
                    null;
            }

            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        }

        internal sealed class DelegateEnumerator<T> : IEnumerator<T>
        {
            public Func<T> CurrentDelegate;
            public Action DisposeDelegate;
            public Func<bool> MoveNextDelegate;
            public Action ResetDelegate;

            public T Current
            {
                get
                {
                    return CurrentDelegate != null ? 
                        CurrentDelegate() :
                        default(T); }
            }

            object IEnumerator.Current { get { return Current; } }

            public void Dispose()
            {
                if (DisposeDelegate != null)
                    DisposeDelegate();
            }

            public bool MoveNext()
            {
                return MoveNextDelegate != null ? 
                    MoveNextDelegate() :
                    false;
            }

            public void Reset()
            {
                if (ResetDelegate != null)
                    ResetDelegate();
            }
        }

        internal sealed class DelegateObserver<T> : IObserver<T>
        {
            public Action<T> OnNextDelegate = null;
            public Action<Exception> OnErrorDelegate = null;
            public Action OnCompletedDelegate = null;

            void IObserver<T>.OnNext(T value)
            {
                if (OnNextDelegate != null)
                    OnNextDelegate(value);
            }

            void IObserver<T>.OnError(Exception error)
            {
                if (OnErrorDelegate != null)
                    OnErrorDelegate(error);
            }

            void IObserver<T>.OnCompleted()
            {
                if (OnCompletedDelegate != null)
                    OnCompletedDelegate();
            }
        }
    }
}
