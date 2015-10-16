// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

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
