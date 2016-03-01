// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using Xunit;

namespace System.Threading.Tasks.Channels.Tests
{
    public class ChannelTests
    {
        [Fact]
        public void Unbounded_CorrectValue()
        {
            Assert.Equal(-1, Channel.Unbounded);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public void Create_InvalidBufferSizes_ThrowArgumentExceptions(int bufferedCapacity)
        {
            Assert.Throws<ArgumentOutOfRangeException>("bufferedCapacity", () => Channel.Create<int>(bufferedCapacity));
            Assert.Throws<ArgumentOutOfRangeException>("bufferedCapacity", () => Channel.Create<int>(bufferedCapacity, false));
            Assert.Throws<ArgumentOutOfRangeException>("bufferedCapacity", () => Channel.Create<int>(bufferedCapacity, true));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1)]
        public void Create_ValidBufferSizes_Success(int bufferedCapacity)
        {
            Assert.NotNull(Channel.Create<int>(bufferedCapacity));
            Assert.NotNull(Channel.Create<int>(bufferedCapacity, false));
            Assert.NotNull(Channel.Create<int>(bufferedCapacity, true));
        }

        [Fact]
        public void CreateUnbuffered_Success()
        {
            Assert.NotNull(Channel.CreateUnbuffered<int>());
            Assert.NotNull(Channel.CreateUnbuffered<string>());
        }

        [Fact]
        public void ReadFromStream_InvalidStream_ThrowsArgumentExceptions()
        {
            Assert.Throws<ArgumentNullException>("source", () => Channel.ReadFromStream<int>(null));
            Assert.Throws<ArgumentException>("source", () => Channel.ReadFromStream<int>(new CanReadFalseStream()));
        }

        [Fact]
        public void WriteToStream_InvalidStream_ThrowsArgumentExceptions()
        {
            Assert.Throws<ArgumentNullException>("destination", () => Channel.WriteToStream<int>(null));
            Assert.Throws<ArgumentException>("destination", () => Channel.WriteToStream<int>(new MemoryStream(Array.Empty<byte>(), writable: false)));
        }

        [Fact]
        public void CreateFromEnumerable_InvalidSource_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>("source", () => Channel.CreateFromEnumerable<int>(null));
        }

        [Fact]
        public void CreateFromTask_InvalidSource_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>("source", () => Channel.CreateFromTask<int>(null));
        }

        [Fact]
        public void AsObservable_InvalidSource_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>("source", () => Channel.AsObservable<int>(null));
        }

        [Fact]
        public void AsObserver_InvalidTarget_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>("target", () => Channel.AsObserver<int>(null));
        }

        [Fact]
        public void AsObservable_SameSource_Idempotent()
        {
            IChannel<int> c = Channel.Create<int>();
            Assert.Same(c.AsObservable(), c.AsObservable());
        }

        [Fact]
        public void GetAwaiter_InvalidSource_ThrowsArgumentException()
        {
            Assert.Throws<NullReferenceException>(() => Channel.GetAwaiter<int>(null));
        }

        [Fact]
        public void GetAsyncEnumerator_InvalidSource_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>("channel", () => Channel.GetAsyncEnumerator<int>(null));
            Assert.Throws<ArgumentNullException>("channel", () => Channel.GetAsyncEnumerator<int>(null, CancellationToken.None));
        }

        [Fact]
        public void CaseRead_Sync_InvalidArguments_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>("channel", () => Channel.CaseRead<int>(null, (Action<int>)null));
            Assert.Throws<ArgumentNullException>("channel", () => Channel.CaseRead<int>(null, i => { }));
            Assert.Throws<ArgumentNullException>("action", () => Channel.CaseRead<int>(Channel.Create<int>(), (Action<int>)null));
        }

        [Fact]
        public void CaseRead_Async_InvalidArguments_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>("channel", () => Channel.CaseRead<int>(null, (Func<int, Task>)null));
            Assert.Throws<ArgumentNullException>("channel", () => Channel.CaseRead<int>(null, i => Task.CompletedTask));
            Assert.Throws<ArgumentNullException>("func", () => Channel.CaseRead<int>(Channel.Create<int>(), (Func<int, Task>)null));
        }

        [Fact]
        public void CaseWrite_Sync_InvalidArguments_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>("channel", () => Channel.CaseWrite<int>(null, 0, (Action)null));
            Assert.Throws<ArgumentNullException>("channel", () => Channel.CaseWrite<int>(null, 0, (Action)delegate { }));
            Assert.Throws<ArgumentNullException>("action", () => Channel.CaseWrite<int>(Channel.Create<int>(), 0, (Action)null));
        }

        [Fact]
        public void CaseWrite_Async_InvalidArguments_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>("channel", () => Channel.CaseWrite<int>(null, 0, (Func<Task>)null));
            Assert.Throws<ArgumentNullException>("channel", () => Channel.CaseWrite<int>(null, 0, delegate { return Task.CompletedTask; }));
            Assert.Throws<ArgumentNullException>("func", () => Channel.CaseWrite<int>(Channel.Create<int>(), 0, (Func<Task>)null));
        }

        [Fact]
        public void CaseRead_Sync_DifferentResultsEachCall()
        {
            Assert.NotSame(
                Channel.CaseRead(Channel.Create<int>(), i => { }),
                Channel.CaseRead(Channel.Create<int>(), i => { }));
        }

        [Fact]
        public void CaseRead_Async_DifferentResultsEachCall()
        {
            Assert.NotSame(
                Channel.CaseRead(Channel.Create<int>(), i => Task.CompletedTask),
                Channel.CaseRead(Channel.Create<int>(), i => Task.CompletedTask));
        }

        [Fact]
        public void CaseWrite_Sync_DifferentResultsEachCall()
        {
            Assert.NotSame(
                Channel.CaseWrite(Channel.Create<int>(), 0, () => { }),
                Channel.CaseWrite(Channel.Create<int>(), 0, () => { }));
        }

        [Fact]
        public void CaseWrite_Async_DifferentResultsEachCall()
        {
            Assert.NotSame(
                Channel.CaseWrite(Channel.Create<int>(), 0, () => Task.CompletedTask),
                Channel.CaseWrite(Channel.Create<int>(), 0, () => Task.CompletedTask));
        }

        [Fact]
        public void Complete_InvalidArgument_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => Channel.Complete<int>(null));
        }

        private sealed class CanReadFalseStream : MemoryStream
        {
            public override bool CanRead { get { return false; } }
        }
    }
}
