// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Reader;
using System.Globalization;
using System.Text;
using Xunit;

namespace System.Buffers.Tests
{
    public class Reader_SkipDelimiter
    {
        [Fact]
        public void TryReadTo_SkipDelimiter()
        {
            byte[] expected = Encoding.UTF8.GetBytes("This is our \\\"understanding\\\"");
            ReadOnlySequence<byte> bytes = BufferFactory.CreateUtf8("This is our \\\"understanding\\\"\" you see.");
            BufferReader<byte> reader = new BufferReader<byte>(bytes);
            Assert.True(reader.TryReadTo(out ReadOnlySpan<byte> span, (byte)'"', (byte)'\\', advancePastDelimiter: true));
            Assert.Equal(expected, span.ToArray());
            Assert.True(reader.IsNext((byte)' '));
            Assert.Equal(30, reader.Consumed);

            reader = new BufferReader<byte>(bytes);
            Assert.True(reader.TryReadTo(out span, (byte)'"', (byte)'\\', advancePastDelimiter: false));
            Assert.Equal(expected, span.ToArray());
            Assert.True(reader.IsNext((byte)'"'));
            Assert.Equal(29, reader.Consumed);

            // Put the skip delimiter in another segment
            bytes = BufferFactory.CreateUtf8("This is our \\\"understanding", "\\\"\" you see.");
            reader = new BufferReader<byte>(bytes);
            Assert.True(reader.TryReadTo(out span, (byte)'"', (byte)'\\', advancePastDelimiter: true));
            Assert.Equal(expected, span.ToArray());
            Assert.True(reader.IsNext((byte)' '));
            Assert.Equal(30, reader.Consumed);

            reader = new BufferReader<byte>(bytes);
            Assert.True(reader.TryReadTo(out span, (byte)'"', (byte)'\\', advancePastDelimiter: false));
            Assert.Equal(expected, span.ToArray());
            Assert.True(reader.IsNext((byte)'"'));
            Assert.Equal(29, reader.Consumed);

            // Put the skip delimiter at the end of the segment
            bytes = BufferFactory.CreateUtf8("This is our \\\"understanding\\", "\"\" you see.");
            reader = new BufferReader<byte>(bytes);
            Assert.True(reader.TryReadTo(out span, (byte)'"', (byte)'\\', advancePastDelimiter: true));
            Assert.Equal(expected, span.ToArray());
            Assert.True(reader.IsNext((byte)' '));
            Assert.Equal(30, reader.Consumed);

            reader = new BufferReader<byte>(bytes);
            Assert.True(reader.TryReadTo(out span, (byte)'"', (byte)'\\', advancePastDelimiter: false));
            Assert.Equal(expected, span.ToArray());
            Assert.True(reader.IsNext((byte)'"'));
            Assert.Equal(29, reader.Consumed);

            // No trailing data
            bytes = BufferFactory.CreateUtf8("This is our \\\"understanding\\\"\"");
            reader = new BufferReader<byte>(bytes);
            Assert.True(reader.TryReadTo(out span, (byte)'"', (byte)'\\', advancePastDelimiter: false));
            Assert.Equal(expected, span.ToArray());
            Assert.True(reader.IsNext((byte)'"'));
            Assert.Equal(29, reader.Consumed);

            reader = new BufferReader<byte>(bytes);
            Assert.True(reader.TryReadTo(out span, (byte)'"', (byte)'\\', advancePastDelimiter: true));
            Assert.Equal(expected, span.ToArray());
            Assert.True(reader.End);
            Assert.Equal(30, reader.Consumed);

            // All delimiters skipped
            bytes = BufferFactory.CreateUtf8("This is our \\\"understanding\\\"");
            reader = new BufferReader<byte>(bytes);
            Assert.False(reader.TryReadTo(out span, (byte)'"', (byte)'\\', advancePastDelimiter: false));
            Assert.Equal(0, reader.Consumed);

            reader = new BufferReader<byte>(bytes);
            Assert.False(reader.TryReadTo(out span, (byte)'"', (byte)'\\', advancePastDelimiter: true));
            Assert.Equal(0, reader.Consumed);
        }
    }
}
