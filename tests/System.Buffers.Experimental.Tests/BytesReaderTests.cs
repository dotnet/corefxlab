// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.Sequences;
using System.Text;
using Xunit;

namespace System.Buffers.Tests
{
    public partial class BytesReaderTests
    {
        [Fact]
        public void SingleSegmentBytesReader()
        {
            var bytes = new ReadOnlyBuffer(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }).AsSlicable();
            var reader = BufferReader.Create(bytes);

            Assert.True(BufferReaderExtensions.TryReadUntill(ref reader, out var ab, 3));
            Assert.True(ab.First.SequenceEqual(new byte[] { 1, 2 }));

            Assert.True(BufferReaderExtensions.TryReadUntill(ref reader, out var cd, 6));
            Assert.True(cd.First.SequenceEqual(new byte[] { 4, 5 }));

            Assert.True(BufferReaderExtensions.TryReadUntill(ref reader, out var ef, new byte[] { 8, 9 }));
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
            }).AsSlicable();

            var reader = BufferReader.Create(bytes);

            Assert.True(BufferReaderExtensions.TryReadUntill(ref reader, out var bytesValue, 2));
            var span = bytesValue.ToSpan();
            Assert.Equal(0, span[0]);
            Assert.Equal(1, span[1]);

            Assert.True(BufferReaderExtensions.TryReadUntill(ref reader, out bytesValue, 5));
            span = bytesValue.ToSpan();
            Assert.Equal(3, span[0]);
            Assert.Equal(4, span[1]);

            Assert.True(BufferReaderExtensions.TryReadUntill(ref reader, out bytesValue, new byte[] { 8, 8 }));
            span = bytesValue.ToSpan();
            Assert.Equal(6, span[0]);
            Assert.Equal(7, span[1]);

            Assert.True(BufferReaderExtensions.TryRead(ref reader, out int value, true));
            Assert.Equal(BitConverter.ToInt32(new byte[] { 0, 1, 0, 2 }), value);

            Assert.True(BufferReaderExtensions.TryRead(ref reader, out value));
            Assert.Equal(BitConverter.ToInt32(new byte[] { 4, 3, 2, 1 }), value);
        }

        [Fact]
        public void EmptyBytesReader()
        {
            var bytes = ReadOnlyBuffer.Empty.AsSlicable();
            var reader = BufferReader.Create(bytes);
            Assert.False(BufferReaderExtensions.TryReadUntill(ref reader, out var range, (byte)' '));
        }

        [Fact]
        public void BytesReaderParse()
        {
            ReadOnlyBuffer bytes = BufferFactory.Parse("12|3Tr|ue|456Tr|ue7|89False|");
            var reader = BufferReader.Create(bytes);

            Assert.True(BufferReaderExtensions.TryParse(ref reader, out ulong u64));
            Assert.Equal(123ul, u64);

            Assert.True(BufferReaderExtensions.TryParse(ref reader, out bool b));
            Assert.Equal(true, b);

            Assert.True(BufferReaderExtensions.TryParse(ref reader, out u64));
            Assert.Equal(456ul, u64);

            Assert.True(BufferReaderExtensions.TryParse(ref reader, out b));
            Assert.Equal(true, b);

            Assert.True(BufferReaderExtensions.TryParse(ref reader, out u64));
            Assert.Equal(789ul, u64);

            Assert.True(BufferReaderExtensions.TryParse(ref reader, out b));
            Assert.Equal(false, b);
        }

        static byte[] s_eol = new byte[] { (byte)'\r', (byte)'\n' };

        [Fact(Skip = "this needs to be redone; given we are unifying ROBs and readers")]
        static void BytesReaderBenchmarkBaseline()
        {
            int sections = 10;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < sections; i++)
            {
                sb.Append("1234 ");
            }
            var data = Encoding.UTF8.GetBytes(sb.ToString());

            var readOnlyBytes = new ReadOnlyBuffer(data);
            var bytesRange = new ReadOnlyBuffer(data);

            var robReader = BufferReader.Create(readOnlyBytes);

            long robSum = 0;
            while (BufferReaderExtensions.TryParse(ref robReader, out int value))
            {
                robSum += value;
                robReader.Advance(1);
            }

            var brReader = BufferReader.Create(bytesRange);
            long brSum = 0;
            while (BufferReaderExtensions.TryParse(ref brReader, out int value))
            {
                brSum += value;
                brReader.Advance(1);
            }

            Assert.Equal(robSum, brSum);
            Assert.NotEqual(brSum, 0);
        }
    }

    static class BufferFactory
    {
        public static ReadOnlyBuffer Create(params byte[][] buffers)
        {
            if (buffers.Length == 1) return new ReadOnlyBuffer(buffers[0]);
            var list = new List<Memory<byte>>();
            foreach (var b in buffers) list.Add(b);
            return new ReadOnlyBuffer(list);
        }

        public static ReadOnlyBuffer Parse(string text)
        {
            var segments = text.Split('|');
            var buffers = new List<Memory<byte>>();
            foreach (var segment in segments)
            {
                buffers.Add(Encoding.UTF8.GetBytes(segment));
            }
            return new ReadOnlyBuffer(buffers.ToArray());
        }

        public static ReadOnlyBufferToSlicableSequenceAdapter AsSlicable(this ReadOnlyBuffer buffer)
            => new ReadOnlyBufferToSlicableSequenceAdapter(buffer);
    }

    struct ReadOnlyBufferToSlicableSequenceAdapter : ISequence<ReadOnlyMemory<byte>>, ISlicable
    {
        ReadOnlyBuffer _buffer;
        public ReadOnlyBufferToSlicableSequenceAdapter(ReadOnlyBuffer buffer)
        {
            _buffer = buffer;
        }

        public Position Start => _buffer.Start;

        public Position Seek(Position origin, long offset)
            => _buffer.Seek(origin, offset);

        public ReadOnlyBuffer Slice(Position start, Position end)
            => _buffer.Slice(start, end);

        public bool TryGet(ref Position position, out ReadOnlyMemory<byte> item, bool advance = true)
            => _buffer.TryGet(ref position, out item, advance);
    }
}
