// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using Xunit;

namespace System.Threading.Tasks.Channels.Tests
{
    public class ChannelTests
    {
        [Fact]
        public void ChannelOptimizations_Properties_Roundtrip()
        {
            var co = new ChannelOptimizations();

            Assert.False(co.SingleReader);
            Assert.False(co.SingleWriter);

            co.SingleReader = true;
            Assert.True(co.SingleReader);
            Assert.False(co.SingleWriter);
            co.SingleReader = false;
            Assert.False(co.SingleReader);

            co.SingleWriter = true;
            Assert.False(co.SingleReader);
            Assert.True(co.SingleWriter);
            co.SingleWriter = false;
            Assert.False(co.SingleWriter);

            co.SingleReader = true;
            co.SingleWriter = true;
            Assert.True(co.SingleReader);
            Assert.True(co.SingleWriter);

            Assert.False(co.AllowSynchronousContinuations);
            co.AllowSynchronousContinuations = true;
            Assert.True(co.AllowSynchronousContinuations);
            co.AllowSynchronousContinuations = false;
            Assert.False(co.AllowSynchronousContinuations);
        }

        [Fact]
        public void CreateUnbounded_InvalidChannelOptimizations_ThrowArgumentException()
        {
            Assert.Throws<ArgumentNullException>("optimizations", () => Channel.CreateUnbounded<int>(null));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public void CreateBounded_InvalidBufferSizes_ThrowArgumentExceptions(int bufferedCapacity)
        {
            Assert.Throws<ArgumentOutOfRangeException>("bufferedCapacity", () => Channel.CreateBounded<int>(bufferedCapacity));
        }

        [Theory]
        [InlineData((BoundedChannelFullMode)(-1))]
        [InlineData((BoundedChannelFullMode)(3))]
        public void CreateBounded_InvalidModes_ThrowArgumentExceptions(BoundedChannelFullMode mode)
        {
            Assert.Throws<ArgumentOutOfRangeException>("mode", () => Channel.CreateBounded<int>(1, mode));
        }

        [Theory]
        [InlineData(1)]
        public void CreateBounded_ValidBufferSizes_Success(int bufferedCapacity)
        {
            Assert.NotNull(Channel.CreateBounded<int>(bufferedCapacity));
        }

        [Fact]
        public void AsObservable_SameSource_Idempotent()
        {
            Channel<int> c = Channel.CreateUnbounded<int>();
            Assert.Same(c.In.AsObservable(), c.In.AsObservable());
        }

        [Fact]
        public void CaseRead_Sync_InvalidArguments_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>("channel", () => Channel.CaseRead<int>(null, (Action<int>)null));
            Assert.Throws<ArgumentNullException>("channel", () => Channel.CaseRead<int>(null, i => { }));
            Assert.Throws<ArgumentNullException>("action", () => Channel.CaseRead<int>(Channel.CreateUnbounded<int>().In, (Action<int>)null));
        }

        [Fact]
        public void CaseRead_Async_InvalidArguments_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>("channel", () => Channel.CaseRead<int>(null, (Func<int, Task>)null));
            Assert.Throws<ArgumentNullException>("channel", () => Channel.CaseRead<int>(null, i => Task.CompletedTask));
            Assert.Throws<ArgumentNullException>("func", () => Channel.CaseRead<int>(Channel.CreateUnbounded<int>().In, (Func<int, Task>)null));
        }

        [Fact]
        public void CaseWrite_Sync_InvalidArguments_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>("channel", () => Channel.CaseWrite<int>(null, 0, (Action)null));
            Assert.Throws<ArgumentNullException>("channel", () => Channel.CaseWrite<int>(null, 0, (Action)delegate { }));
            Assert.Throws<ArgumentNullException>("action", () => Channel.CaseWrite<int>(Channel.CreateUnbounded<int>().Out, 0, (Action)null));
        }

        [Fact]
        public void CaseWrite_Async_InvalidArguments_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>("channel", () => Channel.CaseWrite<int>(null, 0, (Func<Task>)null));
            Assert.Throws<ArgumentNullException>("channel", () => Channel.CaseWrite<int>(null, 0, delegate { return Task.CompletedTask; }));
            Assert.Throws<ArgumentNullException>("func", () => Channel.CaseWrite<int>(Channel.CreateUnbounded<int>().Out, 0, (Func<Task>)null));
        }

        [Fact]
        public void CaseRead_Sync_DifferentResultsEachCall()
        {
            Assert.NotSame(
                Channel.CaseRead(Channel.CreateUnbounded<int>().In, i => { }),
                Channel.CaseRead(Channel.CreateUnbounded<int>().In, i => { }));
        }

        [Fact]
        public void CaseRead_Async_DifferentResultsEachCall()
        {
            Assert.NotSame(
                Channel.CaseRead(Channel.CreateUnbounded<int>().In, i => Task.CompletedTask),
                Channel.CaseRead(Channel.CreateUnbounded<int>().In, i => Task.CompletedTask));
        }

        [Fact]
        public void CaseWrite_Sync_DifferentResultsEachCall()
        {
            Assert.NotSame(
                Channel.CaseWrite(Channel.CreateUnbounded<int>(), 0, () => { }),
                Channel.CaseWrite(Channel.CreateUnbounded<int>(), 0, () => { }));
        }

        [Fact]
        public void CaseWrite_Async_DifferentResultsEachCall()
        {
            Assert.NotSame(
                Channel.CaseWrite(Channel.CreateUnbounded<int>(), 0, () => Task.CompletedTask),
                Channel.CaseWrite(Channel.CreateUnbounded<int>(), 0, () => Task.CompletedTask));
        }

        private sealed class CanReadFalseStream : MemoryStream
        {
            public override bool CanRead { get { return false; } }
        }
    }
}
