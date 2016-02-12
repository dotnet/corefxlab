// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace System.Threading.Tasks.Channels.Tests
{
    public class UnbufferedChannelTests : ChannelTestBase
    {
        protected override IChannel<int> CreateChannel()
        {
            return Channel.CreateUnbuffered<int>();
        }

        [Fact]
        public async Task Complete_BeforeEmpty_WaitingWriters_TriggersCompletion()
        {
            IChannel<int> c = Channel.CreateUnbuffered<int>();
            Task write1 = c.WriteAsync(42);
            Task write2 = c.WriteAsync(43);
            c.Complete();
            await c.Completion;
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => write1);
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => write2);
        }

        [Fact]
        public void TryReadWrite_NoPartner_Fail()
        {
            IChannel<int> c = Channel.CreateUnbuffered<int>();
            Assert.False(c.TryWrite(42));
            int result;
            Assert.False(c.TryRead(out result));
            Assert.Equal(result, 0);
        }

        [Fact]
        public void ReadAsync_TryWrite_Success()
        {
            IChannel<int> c = Channel.CreateUnbuffered<int>();
            ValueTask<int> r = c.ReadAsync();
            Assert.False(r.IsCompletedSuccessfully);
            Assert.False(r.AsTask().IsCompleted);
            Assert.True(c.TryWrite(42));
            Assert.Equal(42, r.Result);
        }

        [Fact]
        public void TryRead_WriteAsync_Success()
        {
            IChannel<int> c = Channel.CreateUnbuffered<int>();
            Task w = c.WriteAsync(42);
            Assert.False(w.IsCompleted);
            int result;
            Assert.True(c.TryRead(out result));
            Assert.Equal(42, result);
        }

        [Fact]
        public async Task Cancel_UnpartneredWrite_ThrowsCancellationException()
        {
            IChannel<int> c = Channel.CreateUnbuffered<int>();
            var cts = new CancellationTokenSource();

            Task w = c.WriteAsync(42, cts.Token);
            Assert.False(w.IsCompleted);

            cts.Cancel();
            await AssertCanceled(w, cts.Token);
        }

        [Fact]
        public async Task Cancel_UnpartneredRead_ThrowsCancellationException()
        {
            IChannel<int> c = Channel.CreateUnbuffered<int>();
            var cts = new CancellationTokenSource();

            Task r = c.ReadAsync(cts.Token).AsTask();
            Assert.False(r.IsCompleted);

            cts.Cancel();
            await AssertCanceled(r, cts.Token);
        }

        [Fact]
        public async Task Cancel_PartneredWrite_Success()
        {
            IChannel<int> c = Channel.CreateUnbuffered<int>();
            var cts = new CancellationTokenSource();

            Task w = c.WriteAsync(42, cts.Token);
            Assert.False(w.IsCompleted);

            ValueTask<int> r = c.ReadAsync();
            Assert.True(r.IsCompletedSuccessfully);

            cts.Cancel();
            await w; // no throw
        }

        [Fact]
        public async Task Cancel_PartneredRead_Success()
        {
            IChannel<int> c = Channel.CreateUnbuffered<int>();
            var cts = new CancellationTokenSource();

            ValueTask<int> r = c.ReadAsync(cts.Token);
            Assert.False(r.IsCompletedSuccessfully);

            Task w = c.WriteAsync(42);
            Assert.True(w.IsCompleted);

            cts.Cancel();
            Assert.Equal(42, await r);
        }

    }
}
