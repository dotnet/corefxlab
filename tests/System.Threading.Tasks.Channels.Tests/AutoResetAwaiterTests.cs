// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace System.Runtime.CompilerServices.Tests
{
    public class AutoResetAwaiterTests
    {
        private sealed class AutoResetAwaitable<T>
        {
            private readonly AutoResetAwaiter<T> _awaiter = new AutoResetAwaiter<T>();
            public AutoResetAwaiter<T> GetAwaiter() => _awaiter;
            public void SetResult(T result) => _awaiter.SetResult(result);
            public void SetException(Exception error) => _awaiter.SetException(error);
            public void SetCanceled(CancellationToken token) => _awaiter.SetCanceled(token);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Ctor_Succeeds(bool runContinuationsAsynchronously)
        {
            var ara = new AutoResetAwaiter<int>(runContinuationsAsynchronously);
            Assert.False(ara.IsCompleted);
        }

        [Fact]
        public async Task SetResult_BeforeAwait_CompletesSuccessfully()
        {
            var ara = new AutoResetAwaitable<int>();
            ara.SetResult(42);
            Assert.Equal(42, await ara);
        }

        [Fact]
        public async Task SetException_BeforeAwait_ThrowsAppropriateError()
        {
            var ara = new AutoResetAwaitable<int>();
            ara.SetException(new FormatException());
            await Assert.ThrowsAsync<FormatException>(async () => await ara);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task SetCanceled_BeforeAwait_ThrowsAppropriateError(bool canceledToken)
        {
            var ara = new AutoResetAwaitable<int>();
            var cts = new CancellationTokenSource();
            if (canceledToken)
            {
                cts.Cancel();
            }
            ara.SetCanceled(cts.Token);
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await ara);
        }
    }
}
