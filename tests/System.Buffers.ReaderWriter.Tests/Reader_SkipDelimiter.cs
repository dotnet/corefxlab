// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Reader;
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

            bytes = BufferFactory.CreateUtf8("abc\\\"de\"");
            reader = new BufferReader<byte>(bytes);
            Assert.True(reader.TryReadTo(out span, (byte)'"', (byte)'\\', advancePastDelimiter: true));
            Assert.Equal(Encoding.UTF8.GetBytes("abc\\\"de"), span.ToArray());
            Assert.True(reader.End);
            Assert.Equal(8, reader.Consumed);
        }

        [Fact]
        public void TryReadTo_SkipDelimiter_Runs()
        {
            ReadOnlySequence<byte> bytes = BufferFactory.CreateUtf8("abc\\\\\"def");
            BufferReader<byte> reader = new BufferReader<byte>(bytes);
            Assert.True(reader.TryReadTo(out ReadOnlySpan<byte> span, (byte)'"', (byte)'\\', advancePastDelimiter: false));
            Assert.Equal(Encoding.UTF8.GetBytes("abc\\\\"), span.ToArray());
            Assert.True(reader.IsNext((byte)'"'));
            Assert.Equal(5, reader.Consumed);

            // Split after escape char
            bytes = BufferFactory.CreateUtf8("abc\\\\", "\"def");
            reader = new BufferReader<byte>(bytes);
            Assert.True(reader.TryReadTo(out span, (byte)'"', (byte)'\\', advancePastDelimiter: false));
            Assert.Equal(Encoding.UTF8.GetBytes("abc\\\\"), span.ToArray());
            Assert.True(reader.IsNext((byte)'"'));
            Assert.Equal(5, reader.Consumed);

            // Split before and after escape char
            bytes = BufferFactory.CreateUtf8("abc\\", "\\", "\"def");
            reader = new BufferReader<byte>(bytes);
            Assert.True(reader.TryReadTo(out span, (byte)'"', (byte)'\\', advancePastDelimiter: false));
            Assert.Equal(Encoding.UTF8.GetBytes("abc\\\\"), span.ToArray());
            Assert.True(reader.IsNext((byte)'"'));
            Assert.Equal(5, reader.Consumed);

            // Check advance past delimiter
            reader = new BufferReader<byte>(bytes);
            Assert.True(reader.TryReadTo(out span, (byte)'"', (byte)'\\', advancePastDelimiter: true));
            Assert.Equal(Encoding.UTF8.GetBytes("abc\\\\"), span.ToArray());
            Assert.True(reader.IsNext((byte)'d'));
            Assert.Equal(6, reader.Consumed);
        }
    }
}
