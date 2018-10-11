// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Reader;
using Xunit;

namespace System.Buffers.Tests
{
    public class Reader_ReadTo
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void TryReadTo_Span(bool advancePastDelimiter)
        {
            ReadOnlySequence<byte> bytes = BufferFactory.Create(new byte[][] {
                new byte[] { 0 },
                new byte[] { 1, 2 },
                new byte[] { },
                new byte[] { 3, 4, 5, 6 }
            });

            BufferReader<byte> reader = new BufferReader<byte>(bytes);
            for (byte i = 0; i < bytes.Length - 1; i++)
            {
                BufferReader<byte> copy = reader;
                Assert.True(copy.TryReadTo(out ReadOnlySpan<byte> span, i, 255, advancePastDelimiter));
                Assert.True(copy.TryReadTo(out span, 6, 255, advancePastDelimiter));
                Assert.Equal(!advancePastDelimiter, copy.TryReadTo(out span, 6, 255, advancePastDelimiter));
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void TryReadTo_Sequence(bool advancePastDelimiter)
        {
            ReadOnlySequence<byte> bytes = BufferFactory.Create(new byte[][] {
                new byte[] { 0 },
                new byte[] { 1, 2 },
                new byte[] { },
                new byte[] { 3, 4, 5, 6 }
            });

            BufferReader<byte> reader = new BufferReader<byte>(bytes);
            for (byte i = 0; i < bytes.Length - 1; i++)
            {
                BufferReader<byte> copy = reader;
                Assert.True(copy.TryReadTo(out ReadOnlySequence<byte> span, i, 255, advancePastDelimiter));
                Assert.True(copy.TryReadTo(out span, 6, 255, advancePastDelimiter));
                Assert.Equal(!advancePastDelimiter, copy.TryReadTo(out span, 6, 255, advancePastDelimiter));
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void TryReadToSpan_Sequence(bool advancePastDelimiter)
        {
            ReadOnlySequence<byte> bytes = BufferFactory.Create(new byte[][] {
                new byte[] { 0, 0 },
                new byte[] { 1, 1, 2, 2 },
                new byte[] { },
                new byte[] { 3, 3, 4, 4, 5, 5, 6, 6 }
            });

            BufferReader<byte> reader = new BufferReader<byte>(bytes);
            for (byte i = 0; i < bytes.Length / 2 - 1; i++)
            {
                byte[] expected = new byte[i * 2 + 1];
                for (int j = 0; j < expected.Length - 1; j++)
                {
                    expected[j] = (byte)(j / 2);
                }
                expected[i * 2] = i;
                ReadOnlySpan<byte> searchFor = new byte []{ i, (byte)(i + 1) };
                BufferReader<byte> copy = reader;
                Assert.True(copy.TryReadTo(out ReadOnlySequence<byte> seq, searchFor, advancePastDelimiter));
                Assert.True(seq.ToArray().AsSpan().SequenceEqual(expected));
            }

            bytes = BufferFactory.Create(new byte[][] {
                new byte[] { 47, 42, 66, 32, 42, 32, 66, 42, 47 }   // /*b * b*/
            });

            reader = new BufferReader<byte>(bytes);
            Assert.True(reader.TryReadTo(out ReadOnlySequence<byte> sequence, new byte[] { 42, 47 }, advancePastDelimiter));    //  */
            Assert.True(sequence.ToArray().AsSpan().SequenceEqual(new byte[] { 47, 42, 66, 32, 42, 32, 66 }));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void TryReadTo_NotFound_Span(bool advancePastDelimiter)
        {
            ReadOnlySequence<byte> bytes = BufferFactory.Create(new byte[][] {
                new byte[] { 1 },
                new byte[] { 2, 3, 255 }
            });

            BufferReader<byte> reader = new BufferReader<byte>(bytes);
            reader.Advance(4);
            Assert.False(reader.TryReadTo(out ReadOnlySpan<byte> span, 255, 0, advancePastDelimiter));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void TryReadTo_NotFound_Sequence(bool advancePastDelimiter)
        {
            ReadOnlySequence<byte> bytes = BufferFactory.Create(new byte[][] {
                new byte[] { 1 },
                new byte[] { 2, 3, 255 }
            });

            BufferReader<byte> reader = new BufferReader<byte>(bytes);
            reader.Advance(4);
            Assert.False(reader.TryReadTo(out ReadOnlySequence<byte> span, 255, 0, advancePastDelimiter));
        }
    }
}
