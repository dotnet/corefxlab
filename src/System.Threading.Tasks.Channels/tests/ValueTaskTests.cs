// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using Xunit;

namespace System.Threading.Tasks.Channels.Tests
{
    public class ValueTaskTests
    {
        [Fact]
        public void ValidateDebuggerAttributes()
        {
            DebuggerAttributes.ValidateDebuggerDisplayReferences(new ValueTask<int>(42));
            DebuggerAttributes.ValidateDebuggerDisplayReferences(new ValueTask<string>(Task.FromResult("42")));
            DebuggerAttributes.ValidateDebuggerDisplayReferences(new ValueTask<string>(new TaskCompletionSource<string>().Task));
        }

        [Fact]
        public void DefaultValueTask_ValueType_DefaultValue()
        {
            Assert.True(default(ValueTask<int>).IsRanToCompletion);
            Assert.Equal(0, default(ValueTask<int>).Result);

            Assert.True(default(ValueTask<string>).IsRanToCompletion);
            Assert.Equal(null, default(ValueTask<string>).Result);
        }

        [Fact]
        public void CreateFromValue_IsRanToCompletion()
        {
            ValueTask<int> t = new ValueTask<int>(42);
            Assert.True(t.IsRanToCompletion);
            Assert.Equal(42, t.Result);
        }

        [Fact]
        public void CreateFromCompletedTask_IsRanToCompletion()
        {
            ValueTask<int> t = new ValueTask<int>(Task.FromResult(42));
            Assert.True(t.IsRanToCompletion);
            Assert.Equal(42, t.Result);
        }

        [Fact]
        public void CreateFromNotCompletedTask_IsNotRanToCompletion()
        {
            var tcs = new TaskCompletionSource<int>();
            ValueTask<int> t = new ValueTask<int>(tcs.Task);
            Assert.False(t.IsRanToCompletion);
            tcs.SetResult(42);
            Assert.Equal(42, t.Result);
        }

        [Fact]
        public void CastFromValue_IsRanToCompletion()
        {
            ValueTask<int> t = 42;
            Assert.True(t.IsRanToCompletion);
            Assert.Equal(42, t.Result);
        }

        [Fact]
        public void CastFromCompletedTask_IsRanToCompletion()
        {
            ValueTask<int> t = Task.FromResult(42);
            Assert.True(t.IsRanToCompletion);
            Assert.Equal(42, t.Result);
        }

        [Fact]
        public void CastFromFaultedTask_IsNotRanToCompletion()
        {
            ValueTask<int> t = Task.FromException<int>(new FormatException());
            Assert.False(t.IsRanToCompletion);
            Assert.Throws<FormatException>(() => t.Result);
        }

        [Fact]
        public void CreateFromTask_AsTaskIdemptotent()
        {
            Task<int> source = Task.FromResult(42);
            ValueTask<int> t = new ValueTask<int>(source);
            Assert.Same(source, t.AsTask());
            Assert.Same(t.AsTask(), t.AsTask());
        }

        [Fact]
        public void CreateFromValue_AsTaskNotIdemptotent()
        {
            ValueTask<int> t = new ValueTask<int>(42);
            Assert.NotSame(t.AsTask(), t.AsTask());
        }

        [Fact]
        public async Task CreateFromValue_Await()
        {
            ValueTask<int> t = new ValueTask<int>(42);
            Assert.Equal(42, await t);
            Assert.Equal(42, await t.ConfigureAwait(false));
            Assert.Equal(42, await t.ConfigureAwait(true));
        }

        [Fact]
        public async Task CreateFromTask_Await()
        {
            Task<int> source = Task.Delay(1).ContinueWith(_ => 42);
            ValueTask<int> t = new ValueTask<int>(source);
            Assert.Equal(42, await t);
            Assert.Equal(42, await t.ConfigureAwait(false));
            Assert.Equal(42, await t.ConfigureAwait(true));
        }

        [Fact]
        public void Awaiter_OnCompleted()
        {
            // Since ValueTask implementes both OnCompleted and UnsafeOnCompleted,
            // OnCompleted typically won't be used by await, so we add an explicit test
            // for it here.

            ValueTask<int> t = 42;
            var mres = new ManualResetEventSlim();
            t.GetAwaiter().OnCompleted(() => mres.Set());
            Assert.True(mres.Wait(10000));
        }

        [Fact]
        public void GetHashCode_ContainsResult()
        {
            ValueTask<int> t = 42;
            Assert.Equal(42, t.GetHashCode());
        }

        [Fact]
        public void GetHashCode_ContainsTask()
        {
            ValueTask<string> t = Task.FromResult("42");
            Assert.NotEqual(0, t.GetHashCode());
        }

        [Fact]
        public void GetHashCode_ContainsNull()
        {
            ValueTask<string> t = (string)null;
            Assert.Equal(0, t.GetHashCode());
        }

        [Fact]
        public void OperatorEquals()
        {
            Assert.True((ValueTask<int>)42 == (ValueTask<int>)42);
            Assert.False((ValueTask<int>)42 == (ValueTask<int>)43);

            Assert.True((ValueTask<string>)"42" == (ValueTask<string>)"42");
            Assert.True((ValueTask<string>)(string)null == (ValueTask<string>)(string)null);

            Assert.False((ValueTask<string>)"42" == (ValueTask<string>)(string)null);
            Assert.False((ValueTask<string>)(string)null == (ValueTask<string>)"42");

            Assert.False((ValueTask<int>)42 == (ValueTask<int>)Task.FromResult(42));
            Assert.False((ValueTask<int>)Task.FromResult(42) == (ValueTask<int>)42);
        }

        [Fact]
        public void OperatorNotEquals()
        {
            Assert.False((ValueTask<int>)42 != (ValueTask<int>)42);
            Assert.True((ValueTask<int>)42 != (ValueTask<int>)43);

            Assert.False((ValueTask<string>)"42" != (ValueTask<string>)"42");
            Assert.False((ValueTask<string>)(string)null != (ValueTask<string>)(string)null);

            Assert.True((ValueTask<string>)"42" != (ValueTask<string>)(string)null);
            Assert.True((ValueTask<string>)(string)null != (ValueTask<string>)"42");

            Assert.True((ValueTask<int>)42 != (ValueTask<int>)Task.FromResult(42));
            Assert.True((ValueTask<int>)Task.FromResult(42) != (ValueTask<int>)42);
        }

        [Fact]
        public void Equals_ValueTask()
        {
            Assert.True(((ValueTask<int>)42).Equals((ValueTask<int>)42));
            Assert.False(((ValueTask<int>)42).Equals((ValueTask<int>)43));

            Assert.True(((ValueTask<string>)"42").Equals((ValueTask<string>)"42"));
            Assert.True(((ValueTask<string>)(string)null).Equals((ValueTask<string>)(string)null));

            Assert.False(((ValueTask<string>)"42").Equals((ValueTask<string>)(string)null));
            Assert.False(((ValueTask<string>)(string)null).Equals((ValueTask<string>)"42"));

            Assert.False(((ValueTask<int>)42).Equals((ValueTask<int>)Task.FromResult(42)));
            Assert.False(((ValueTask<int>)Task.FromResult(42)).Equals((ValueTask<int>)42));
        }

        [Fact]
        public void Equals_Object()
        {
            Assert.True(((ValueTask<int>)42).Equals((object)(ValueTask<int>)42));
            Assert.False(((ValueTask<int>)42).Equals((object)(ValueTask<int>)43));

            Assert.True(((ValueTask<string>)"42").Equals((object)(ValueTask<string>)"42"));
            Assert.True(((ValueTask<string>)(string)null).Equals((object)(ValueTask<string>)(string)null));

            Assert.False(((ValueTask<string>)"42").Equals((object)(ValueTask<string>)(string)null));
            Assert.False(((ValueTask<string>)(string)null).Equals((object)(ValueTask<string>)"42"));

            Assert.False(((ValueTask<int>)42).Equals((object)(ValueTask<int>)Task.FromResult(42)));
            Assert.False(((ValueTask<int>)Task.FromResult(42)).Equals((object)(ValueTask<int>)42));

            Assert.False(((ValueTask<int>)42).Equals((object)null));
            Assert.False(((ValueTask<int>)42).Equals(new object()));
            Assert.False(((ValueTask<int>)42).Equals((object)42));
        }
    }
}
