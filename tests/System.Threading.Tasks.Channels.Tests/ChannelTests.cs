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

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public void CreateBounded_InvalidBufferSizes_ThrowArgumentExceptions(int bufferedCapacity) =>
            Assert.Throws<ArgumentOutOfRangeException>("bufferedCapacity", () => Channel.CreateBounded<int>(bufferedCapacity));

        [Theory]
        [InlineData((BoundedChannelFullMode)(-1))]
        [InlineData((BoundedChannelFullMode)(4))]
        public void CreateBounded_InvalidModes_ThrowArgumentExceptions(BoundedChannelFullMode mode) =>
            Assert.Throws<ArgumentOutOfRangeException>("mode", () => Channel.CreateBounded<int>(1, mode));

        [Theory]
        [InlineData(1)]
        public void CreateBounded_ValidBufferSizes_Success(int bufferedCapacity) =>
            Assert.NotNull(Channel.CreateBounded<int>(bufferedCapacity));

        [Fact]
        public void AsObservable_SameSource_Idempotent()
        {
            var c = Channel.CreateUnbounded<int>();
            Assert.Same(c.In.AsObservable(), c.In.AsObservable());
        }

        private sealed class CanReadFalseStream : MemoryStream
        {
            public override bool CanRead => false;
        }
    }
}
