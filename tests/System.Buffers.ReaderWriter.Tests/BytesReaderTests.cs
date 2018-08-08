// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers.Reader;
using System.Text;
using Xunit;

namespace System.Buffers.Tests
{
    public partial class BytesReaderTests
    {
        [Fact]
        public void SingleSegmentBytesReader()
        {
            var bytes = new ReadOnlySequence<byte>(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            var reader = BufferReader.Create(bytes);

            Assert.True(reader.TryReadUntil(out ReadOnlySequence<byte> ab, 3));
            Assert.True(ab.First.SequenceEqual(new byte[] { 1, 2 }));

            Assert.True(reader.TryReadUntil(out ReadOnlySequence<byte> cd, 6));
            Assert.True(cd.First.SequenceEqual(new byte[] { 4, 5 }));

            Assert.True(reader.TryReadUntil(out ReadOnlySequence<byte> ef, new byte[] { 8, 9 }));
            Assert.True(ef.First.SequenceEqual(new byte[] { 7 }));
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

            var reader = BufferReader.Create(bytes);

            Assert.True(reader.TryReadUntil(out ReadOnlySequence<byte> bytesValue, 2));
            var span = bytesValue.ToSpan();
            Assert.Equal(0, span[0]);
            Assert.Equal(1, span[1]);

            Assert.True(reader.TryReadUntil(out bytesValue, 5));
            span = bytesValue.ToSpan();
            Assert.Equal(3, span[0]);
            Assert.Equal(4, span[1]);

            Assert.True(reader.TryReadUntil(out bytesValue, new byte[] { 8, 8 }));
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
            var reader = BufferReader.Create(bytes);
            Assert.False(reader.TryReadUntil(out ReadOnlySequence<byte> range, (byte)' '));
        }

        [Fact]
        public void BytesReaderParse()
        {
            ReadOnlySequence<byte> bytes = BufferFactory.Parse("12|3Tr|ue|456Tr|ue7|89False|");
            var reader = BufferReader.Create(bytes);

            Assert.True(reader.TryParse(out ulong u64));
            Assert.Equal(123ul, u64);

            Assert.True(reader.TryParse(out bool b));
            Assert.Equal(true, b);

            Assert.True(reader.TryParse(out u64));
            Assert.Equal(456ul, u64);

            Assert.True(reader.TryParse(out b));
            Assert.Equal(true, b);

            Assert.True(reader.TryParse(out u64));
            Assert.Equal(789ul, u64);

            Assert.True(reader.TryParse(out b));
            Assert.Equal(false, b);
        }

        private static byte[] s_eol = new byte[] { (byte)'\r', (byte)'\n' };

        [Fact(Skip = "this needs to be redone; given we are unifying ROBs and readers")]
        private static void BytesReaderBenchmarkBaseline()
        {
            int sections = 10;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < sections; i++)
            {
                sb.Append("1234 ");
            }
            var data = Encoding.UTF8.GetBytes(sb.ToString());

            var readOnlyBytes = new ReadOnlySequence<byte>(data);
            var bytesRange = new ReadOnlySequence<byte>(data);

            var robReader = BufferReader.Create(readOnlyBytes);

            long robSum = 0;
            while (robReader.TryParse(out int value))
            {
                robSum += value;
                robReader.Advance(1);
            }

            var brReader = BufferReader.Create(bytesRange);
            long brSum = 0;
            while (brReader.TryParse(out int value))
            {
                brSum += value;
                brReader.Advance(1);
            }

            Assert.Equal(robSum, brSum);
            Assert.NotEqual(brSum, 0);
        }
    }
}
