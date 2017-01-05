// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using Xunit;

namespace System.Threading.Tasks.Channels.Tests
{
    public class ChannelTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public void Create_InvalidBufferSizes_ThrowArgumentExceptions(int bufferedCapacity)
        {
            Assert.Throws<ArgumentOutOfRangeException>("bufferedCapacity", () => Channel.CreateBounded<int>(bufferedCapacity));
        }

        [Theory]
        [InlineData(1)]
        public void Create_ValidBufferSizes_Success(int bufferedCapacity)
        {
            Assert.NotNull(Channel.CreateBounded<int>(bufferedCapacity));
        }

        [Fact]
        public void AsObservable_InvalidSource_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>("source", () => ChannelExtensions.AsObservable<int>(null));
        }

        [Fact]
        public void AsObserver_InvalidTarget_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>("target", () => ChannelExtensions.AsObserver<int>(null));
        }

        [Fact]
        public void AsObservable_SameSource_Idempotent()
        {
            IChannel<int> c = Channel.CreateUnbounded<int>();
            Assert.Same(c.AsObservable(), c.AsObservable());
        }

        [Fact]
        public void GetAsyncEnumerator_InvalidSource_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>("channel", () => ChannelExtensions.GetAsyncEnumerator<int>(null));
            Assert.Throws<ArgumentNullException>("channel", () => ChannelExtensions.GetAsyncEnumerator<int>(null, CancellationToken.None));
        }

        [Fact]
        public void CaseRead_Sync_InvalidArguments_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>("channel", () => Channel.CaseRead<int>(null, (Action<int>)null));
            Assert.Throws<ArgumentNullException>("channel", () => Channel.CaseRead<int>(null, i => { }));
            Assert.Throws<ArgumentNullException>("action", () => Channel.CaseRead<int>(Channel.CreateUnbounded<int>(), (Action<int>)null));
        }

        [Fact]
        public void CaseRead_Async_InvalidArguments_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>("channel", () => Channel.CaseRead<int>(null, (Func<int, Task>)null));
            Assert.Throws<ArgumentNullException>("channel", () => Channel.CaseRead<int>(null, i => Task.CompletedTask));
            Assert.Throws<ArgumentNullException>("func", () => Channel.CaseRead<int>(Channel.CreateUnbounded<int>(), (Func<int, Task>)null));
        }

        [Fact]
        public void CaseWrite_Sync_InvalidArguments_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>("channel", () => Channel.CaseWrite<int>(null, 0, (Action)null));
            Assert.Throws<ArgumentNullException>("channel", () => Channel.CaseWrite<int>(null, 0, (Action)delegate { }));
            Assert.Throws<ArgumentNullException>("action", () => Channel.CaseWrite<int>(Channel.CreateUnbounded<int>(), 0, (Action)null));
        }

        [Fact]
        public void CaseWrite_Async_InvalidArguments_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>("channel", () => Channel.CaseWrite<int>(null, 0, (Func<Task>)null));
            Assert.Throws<ArgumentNullException>("channel", () => Channel.CaseWrite<int>(null, 0, delegate { return Task.CompletedTask; }));
            Assert.Throws<ArgumentNullException>("func", () => Channel.CaseWrite<int>(Channel.CreateUnbounded<int>(), 0, (Func<Task>)null));
        }

        [Fact]
        public void CaseRead_Sync_DifferentResultsEachCall()
        {
            Assert.NotSame(
                Channel.CaseRead(Channel.CreateUnbounded<int>(), i => { }),
                Channel.CaseRead(Channel.CreateUnbounded<int>(), i => { }));
        }

        [Fact]
        public void CaseRead_Async_DifferentResultsEachCall()
        {
            Assert.NotSame(
                Channel.CaseRead(Channel.CreateUnbounded<int>(), i => Task.CompletedTask),
                Channel.CaseRead(Channel.CreateUnbounded<int>(), i => Task.CompletedTask));
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

        [Fact]
        public void Complete_InvalidArgument_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => ChannelExtensions.Complete<int>(null));
        }

        private sealed class CanReadFalseStream : MemoryStream
        {
            public override bool CanRead { get { return false; } }
        }
    }
}
