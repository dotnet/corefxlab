// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Buffers.Text;
using System.Collections.Sequences;
using System.Text;
using System.Text.Utf8;
using Xunit;

namespace System.Buffers.Tests
{
    public partial class BytesReaderTests
    {
        [Fact]
        public void SingleSegmentBytesReader()
        {
            ReadOnlyBytes bytes = Create("AB CD#EF&&");
            var reader = BytesReader.Create(bytes);

            Assert.True(reader.TryReadBytes(out var ab, (byte)' '));
            Assert.Equal("AB", ab.ToString(SymbolTable.InvariantUtf8));

            Assert.True(reader.TryReadBytes(out var cd, (byte)'#'));
            Assert.Equal("CD", cd.ToString(SymbolTable.InvariantUtf8));

            //Assert.True(reader.TryReadBytes(out var ef, new byte[] { (byte)'&', (byte)'&' }));
            //Assert.Equal("EF", ef.ToString(SymbolTable.InvariantUtf8));
        }

        [Fact]
        public void MultiSegmentBytesReaderNumbers()
        {
            ReadOnlyBytes bytes = ListHelper.CreateRob(new byte[][] {
                new byte[] { 0          },
                new byte[] { 1, 2       },
                new byte[] { 3, 4       },
                new byte[] { 5, 6, 7, 8 },
                new byte[] { 8, 0       },
                new byte[] { 1,         },
                new byte[] { 0, 2,      },
                new byte[] { 1, 2, 3, 4 },
            });

            var reader = BytesReader.Create(bytes);

            Assert.True(reader.TryReadBytes(out var bytesValue, 2));
            var span = bytesValue.ToSpan();
            Assert.Equal(0, span[0]);
            Assert.Equal(1, span[1]);

            Assert.True(reader.TryReadBytes(out bytesValue, 5));
            span = bytesValue.ToSpan();
            Assert.Equal(3, span[0]);
            Assert.Equal(4, span[1]);

            Assert.True(reader.TryReadBytes(out bytesValue, new byte[] { 8, 8 }));
            span = bytesValue.ToSpan();
            Assert.Equal(6, span[0]);
            Assert.Equal(7, span[1]);

            Assert.True(reader.TryRead(out int value, true));
            Assert.Equal(BitConverter.ToInt32(new byte[] { 0, 1, 0, 2 }), value);

            Assert.True(reader.TryRead(out value));
            Assert.Equal(BitConverter.ToInt32(new byte[] { 4, 3, 2, 1 }), value);
        }

        [Fact]
        public void MultiSegmentBytesReader()
        {
            ReadOnlyBytes bytes = Parse("A|B |CD|#EF&|&");
            var reader = BytesReader.Create(bytes);

            Assert.True(reader.TryReadBytes(out var ab, (byte)' '));
            Assert.Equal("AB", ab.ToString(SymbolTable.InvariantUtf8));

            Assert.True(reader.TryReadBytes(out var cd, (byte)'#'));
            Assert.Equal("CD", cd.Utf8ToString());

            //Assert.True(reader.TryReadBytes(out var ef, new byte[] { (byte)'&', (byte)'&' }));
            //Assert.Equal("EF", ef.ToString(SymbolTable.InvariantUtf8));
        }

        [Fact]
        public void EmptyBytesReader()
        {
            ReadOnlyBytes bytes = Create("");
            var reader = BytesReader.Create(bytes);
            Assert.False(reader.TryReadBytes(out var range, (byte)' '));

            bytes = Parse("|");
            reader = BytesReader.Create(bytes);
            Assert.False(reader.TryReadBytes(out range, (byte)' '));
        }

        [Fact]
        public void BytesReaderParse()
        {
            ReadOnlyBytes bytes = Parse("12|3Tr|ue|456Tr|ue7|89False|");
            var reader = BytesReader.Create(bytes);

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

        static byte[] s_eol = new byte[] { (byte)'\r', (byte)'\n' };

        [Fact]
        static void BytesReaderBenchmarkBaseline()
        {
            int sections = 10;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < sections; i++)
            {
                sb.Append("1234 ");
            }
            var data = Encoding.UTF8.GetBytes(sb.ToString());

            var readOnlyBytes = new ReadOnlyBytes(data);
            var bytesRange = new ReadOnlyBytes(data);

            var robReader = BytesReader.Create(readOnlyBytes);

            long robSum = 0;
            while (robReader.TryParse(out int value))
            {
                robSum += value;
                robReader.Advance(1);
            }

            var brReader = BytesReader.Create(bytesRange);
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

    static class ListHelper
    {
        public static ReadOnlyBytes CreateRob(params byte[][] buffers)
        {
            if (buffers.Length == 1) return new ReadOnlyBytes(buffers[0]);
            var (list, length) = MemoryList.Create(buffers);
            return new ReadOnlyBytes(list, length);
        }
    }

    public static class ReadOnlyBytesTextExtensions
    {
        public static string ToString<TSequence>(this TSequence bytes, SymbolTable symbolTable) where TSequence: ISequence<ReadOnlyMemory<byte>>
        {
            var sb = new StringBuilder();
            if (symbolTable == SymbolTable.InvariantUtf8)
            {
                Position position = default;
                while (bytes.TryGet(ref position, out ReadOnlyMemory<byte> segment))
                {
                    sb.Append(new Utf8Span(segment.Span).ToString());
                }
            }
            else
            {
                throw new NotImplementedException();
            }
            return sb.ToString();
        }

        public static string Utf8ToString<TSequence>(this TSequence bytes) where TSequence : ISequence<ReadOnlyMemory<byte>>
        {
            var sb = new StringBuilder();

            Position position = default;
            while (bytes.TryGet(ref position, out ReadOnlyMemory<byte> segment))
            {
                sb.Append(new Utf8Span(segment.Span).ToString());
            }
            return sb.ToString();
        }
    }
}
