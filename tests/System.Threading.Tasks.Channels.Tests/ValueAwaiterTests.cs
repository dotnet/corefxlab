// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xunit;

namespace System.Threading.Tasks.Channels.Tests
{
    public class ValueAwaiterTests
    {
        public static IEnumerable<object[]> Ctor_AlreadyCompletedInput_MemberData()
        {
            yield return new object[] { new ValueAwaiter<string>("hello") };
            yield return new object[] { new ValueAwaiter<string>(new ValueTask<string>("hello")) };
            yield return new object[] { new ValueAwaiter<string>(Task.FromResult("hello")) };
            yield return new object[] { new ValueAwaiter<string>(new DelegateAwaiter<string>
            {
                IsCompletedDelegate = () => true,
                GetResultDelegate = () => "hello",
                OnCompletedDelegate = a => Task.Run(a),
                UnsafeOnCompletedDelegate = a => Task.Run(a)
            }) };
        }

        [Theory]
        [MemberData(nameof(Ctor_AlreadyCompletedInput_MemberData))]
        public void Ctor_AlreadyCompletedInput(ValueAwaiter<string> va)
        {
            Assert.True(va.IsCompleted);
            Assert.Equal("hello", va.GetResult());

            int threadId = Environment.CurrentManagedThreadId;
            var are = new AutoResetEvent(false);

            va.OnCompleted(() =>
            {
                Assert.NotEqual(threadId, Environment.CurrentManagedThreadId);
                are.Set();
            });
            are.WaitOne();

            va.UnsafeOnCompleted(() =>
            {
                Assert.NotEqual(threadId, Environment.CurrentManagedThreadId);
                are.Set();
            });
            are.WaitOne();
        }

        public static IEnumerable<object[]> Ctor_NotYetCompleted_MemberData()
        {
            for (int completionMode = 0; completionMode < 2; completionMode++)
            {
                {
                    var tcs = new TaskCompletionSource<int>();
                    yield return new object[] { completionMode, tcs, new ValueAwaiter<int>(tcs.Task) };
                }

                {
                    var tcs = new TaskCompletionSource<int>();
                    yield return new object[] { completionMode, tcs, new ValueAwaiter<int>(new ValueTask<int>(tcs.Task)) };
                }

                {
                    var tcs = new TaskCompletionSource<int>();
                    yield return new object[] { completionMode, tcs, new ValueAwaiter<int>(new DelegateAwaiter<int>
                    {
                        IsCompletedDelegate = () => tcs.Task.GetAwaiter().IsCompleted,
                        GetResultDelegate = () => tcs.Task.GetAwaiter().GetResult(),
                        OnCompletedDelegate = a => tcs.Task.GetAwaiter().OnCompleted(a),
                        UnsafeOnCompletedDelegate = a => tcs.Task.GetAwaiter().UnsafeOnCompleted(a),
                    }) };
                }
            }
        }

        [Theory]
        [MemberData(nameof(Ctor_NotYetCompleted_MemberData))]
        public void Ctor_NotYetCompleted(int completionMode, TaskCompletionSource<int> tcs, ValueAwaiter<int> va)
        {
            Assert.False(tcs.Task.IsCompleted);
            Assert.False(va.IsCompleted);

            var are = new AutoResetEvent(false);
            Action action = () => are.Set();
            switch (completionMode)
            {
                case 0: va.OnCompleted(action); break;
                case 1: va.UnsafeOnCompleted(action); break;
            }

            tcs.SetResult(42);
            are.WaitOne();

            Assert.True(va.IsCompleted);
            Assert.Equal(42, va.GetResult());
        }

        private sealed class DelegateAwaiter<T> : IAwaiter<T>
        {
            public Func<bool> IsCompletedDelegate;
            public Func<T> GetResultDelegate;
            public Action<Action> OnCompletedDelegate;
            public Action<Action> UnsafeOnCompletedDelegate;

            public bool IsCompleted => IsCompletedDelegate?.Invoke() ?? true;
            public T GetResult() => GetResultDelegate != null ? GetResultDelegate() : default(T);
            public void OnCompleted(Action action) => OnCompletedDelegate?.Invoke(action);
            public void UnsafeOnCompleted(Action action) => UnsafeOnCompletedDelegate?.Invoke(action);
        }
    }
}
