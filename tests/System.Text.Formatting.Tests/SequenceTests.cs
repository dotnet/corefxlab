// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Sequences;
using System.Text.Utf8;
using Xunit;

namespace System.Text.Parsing.Tests
{
    public class SequenceParsingTests {

        [Theory]
        [InlineData(123, 3, new string[] { "123" })]
        [InlineData(123, 3, new string[] { "1", "23" })]
        [InlineData(123, 3, new string[] { "1", "2" , "3" })]
        [InlineData(1, 1, new string[] { "1_", "2", "3" })]
        [InlineData(1, 1, new string[] { "1", "_2", "3" })]
        [InlineData(12, 2, new string[] { "1", "2_", "3" })]
        [InlineData(123, 3, new string[] { "1", "2", "3_" })]
        public void ParseUInt32(uint expectedValue, int expectedConsumed, string[] segments) {
            var buffers = ToUtf8Buffers(segments);
            Assert.True(buffers.TryParseUInt32(out uint value, out int consumed));
            Assert.Equal(expectedValue, value);
            Assert.Equal(expectedConsumed, consumed);
        }

        [Theory]
        [InlineData(123, 3, new string[] { "123" })]
        [InlineData(123, 3, new string[] { "1", "23" })]
        [InlineData(123, 3, new string[] { "1", "2", "3" })]
        [InlineData(1, 1, new string[] { "1_", "2", "3" })]
        [InlineData(1, 1, new string[] { "1", "_2", "3" })]
        [InlineData(12, 2, new string[] { "1", "2_", "3" })]
        [InlineData(123, 3, new string[] { "1", "2", "3_" })]
        public void ParseUInt64(ulong expectedValue, int expectedConsumed, string[] segments)
        {
            var buffers = ToUtf8Buffers(segments);
            Assert.True(buffers.TryParseUInt64(out ulong value, out int consumed));
            Assert.Equal(expectedValue, value);
            Assert.Equal(expectedConsumed, consumed);
        }

        static ArrayList<ReadOnlyMemory<byte>> ToUtf8Buffers(params string[] segments)
        {
            var buffers = new ArrayList<ReadOnlyMemory<byte>>();
            foreach (var segment in segments) {
                buffers.Add(new Utf8Span(segment).Bytes.ToArray());
            }
            return buffers;
        }
    }
}
