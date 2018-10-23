// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using Xunit;

namespace System.Buffers.Tests
{
    public partial class BytesReaderTests
    {
        [Fact]
        public void SingleSegmentBytesReader()
        {
            byte[] buffer = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var bytes = new ReadOnlySequence<byte>(buffer);
            var reader = new BufferReader<byte>(bytes);

            Assert.True(reader.TryReadTo(out ReadOnlySequence<byte> ab, 3));
            Assert.True(ab.First.SequenceEqual(new byte[] { 1, 2 }));

            Assert.True(reader.TryReadTo(out ReadOnlySequence<byte> cd, 6));
            Assert.True(cd.First.SequenceEqual(new byte[] { 4, 5 }));

            Assert.True(reader.TryReadTo(out ReadOnlySequence<byte> ef, new byte[] { 8, 9 }));
            Assert.True(ef.First.SequenceEqual(new byte[] { 7 }));
        }

        [Theory,
            InlineData(true),
            InlineData(false)]
        public void SkipTests(bool singleSegment)
        {
            byte[] buffer = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var bytes = singleSegment
                ? new ReadOnlySequence<byte>(buffer)
                : BufferUtilities.CreateSplitBuffer(buffer, 2, 4);

            var skipReader = new BufferReader<byte>(bytes);
            Assert.False(skipReader.TryAdvanceTo(10));
            Assert.True(skipReader.TryAdvanceTo(4, advancePastDelimiter: false));
            Assert.True(skipReader.TryRead(out byte value));
            Assert.Equal(4, value);

            Assert.True(skipReader.TryAdvanceToAny(new byte[] { 3, 12, 7 }, advancePastDelimiter: false));
            Assert.True(skipReader.TryRead(out value));
            Assert.Equal(7, value);
            Assert.Equal(1, skipReader.AdvancePast(8));
            Assert.True(skipReader.TryRead(out value));
            Assert.Equal(9, value);

            skipReader = new BufferReader<byte>(bytes);
            Assert.Equal(0, skipReader.AdvancePast(2));
            Assert.Equal(3, skipReader.AdvancePastAny(new byte[] { 2, 3, 1 }));
            Assert.True(skipReader.TryRead(out value));
            Assert.Equal(4, value);
        }

        [Fact]
        public void AdvancePastEmptySegments()
        {
            ReadOnlySequence<byte> bytes = BufferFactory.Create(new byte[][] {
                new byte[] { 0          },
                new byte[] { },
                new byte[] { },
                new byte[] { }
            });

            var reader = new BufferReader<byte>(bytes);
            reader.Advance(1);
            Assert.Equal(0, reader.CurrentSpanIndex);
            Assert.Equal(0, reader.CurrentSpan.Length);
            Assert.False(reader.TryPeek(out byte value));
            ReadOnlySequence<byte> sequence = reader.Sequence.Slice(reader.Position);
            Assert.Equal(0, sequence.Length);
        }

        [Fact]
        public void MultiSegmentBytesReaderNumbers()
        {
            var bytes = BufferFactory.Create(new byte[][] {
                new byte[] { 0          },
                new byte[] { 1, 2       },
                new byte[] { 3, 4       },
                new byte[] { 5, 6, 7, 8 },
                new byte[] { 8, 0       },
                new byte[] { 1,         },
                new byte[] { 0, 2,      },
                new byte[] { 1, 2, 3, 4 },
                new byte[] { 5, 6       },
                new byte[] { 7, 8, 9,   },
                new byte[] { 0, 1, 2, 3 },
                new byte[] { 4, 5       },
                new byte[] { 6, 7, 8, 9 },
                new byte[] { 0, 1, 2, 3 },
                new byte[] { 4          },
            });

            var reader = new BufferReader<byte>(bytes);

            Assert.True(reader.TryReadTo(out ReadOnlySequence<byte> bytesValue, 2));
            var span = bytesValue.ToSpan();
            Assert.Equal(0, span[0]);
            Assert.Equal(1, span[1]);

            Assert.True(reader.TryReadTo(out bytesValue, 5));
            span = bytesValue.ToSpan();
            Assert.Equal(3, span[0]);
            Assert.Equal(4, span[1]);

            Assert.True(reader.TryReadTo(out bytesValue, new byte[] { 8, 8 }));
            span = bytesValue.ToSpan();
            Assert.Equal(6, span[0]);
            Assert.Equal(7, span[1]);

            Assert.True(reader.TryRead(out int intValue));
            Assert.Equal(BitConverter.ToInt32(new byte[] { 0, 1, 0, 2 }), intValue);

            Assert.True(reader.TryReadInt32BigEndian(out intValue));
            Assert.Equal(BitConverter.ToInt32(new byte[] { 4, 3, 2, 1 }), intValue);

            Assert.True(reader.TryReadInt64LittleEndian(out long longValue));
            Assert.Equal(BitConverter.ToInt64(new byte[] { 5, 6, 7, 8, 9, 0, 1, 2 }), longValue);

            Assert.True(reader.TryReadInt64BigEndian(out longValue));
            Assert.Equal(BitConverter.ToInt64(new byte[] { 0, 9, 8, 7, 6, 5, 4, 3 }), longValue);

            Assert.True(reader.TryReadInt16LittleEndian(out short shortValue));
            Assert.Equal(BitConverter.ToInt16(new byte[] { 1, 2 }), shortValue);

            Assert.True(reader.TryReadInt16BigEndian(out shortValue));
            Assert.Equal(BitConverter.ToInt16(new byte[] { 4, 3 }), shortValue);
        }

        [Fact]
        public void EmptyBytesReader()
        {
            var bytes = ReadOnlySequence<byte>.Empty;
            var reader = new BufferReader<byte>(bytes);
            Assert.False(reader.TryReadTo(out ReadOnlySequence<byte> range, (byte)' '));
        }

        [Fact]
        public void BytesReaderParse()
        {
            ReadOnlySequence<byte> bytes = BufferFactory.Parse("12|3Tr|ue|456Tr|ue7|89False|");
            var reader = new BufferReader<byte>(bytes);

            Assert.True(reader.TryParse(out long l64));
            Assert.Equal(123, l64);

            Assert.True(reader.TryParse(out bool b));
            Assert.Equal(true, b);

            Assert.True(reader.TryParse(out l64));
            Assert.Equal(456, l64);

            Assert.True(reader.TryParse(out b));
            Assert.Equal(true, b);

            Assert.True(reader.TryParse(out l64));
            Assert.Equal(789, l64);

            Assert.True(reader.TryParse(out b));
            Assert.Equal(false, b);
        }
    }
}
