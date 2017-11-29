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

            var ab = bytes.Slice(reader.ReadRange((byte)' '));
            Assert.Equal("AB", ab.ToString(SymbolTable.InvariantUtf8));

            var cd = bytes.Slice(reader.ReadRange((byte)'#'));
            Assert.Equal("CD", cd.ToString(SymbolTable.InvariantUtf8));

            var ef = bytes.Slice(reader.ReadRange(new byte[] { (byte)'&', (byte)'&' }));
            Assert.Equal("EF", ef.ToString(SymbolTable.InvariantUtf8));
        }

        [Fact]
        public void MultiSegmentBytesReaderNumbers()
        {
            ReadOnlyBytes bytes = ListHelper.CreateRob(new byte[][] {
                new byte[] { 0          },
                new byte[] { 1, 2       },
                new byte[] { 3, 4       },
                new byte[] { 5, 6, 7, 8 },
                new byte[] { 8          }
            });

            var reader = BytesReader.Create(bytes);

            var value = bytes.Slice(reader.ReadRange(2)).ToSpan();
            Assert.Equal(0, value[0]);
            Assert.Equal(1, value[1]);

            value = bytes.Slice(reader.ReadRange(5)).ToSpan();
            Assert.Equal(3, value[0]);
            Assert.Equal(4, value[1]);

            value = bytes.Slice(reader.ReadRange(new byte[] { 8, 8 })).ToSpan();
            Assert.Equal(6, value[0]);
            Assert.Equal(7, value[1]);
        }

        [Fact]
        public void MultiSegmentBytesReader()
        {
            ReadOnlyBytes bytes = Parse("A|B |CD|#EF&|&");
            var reader = BytesReader.Create(bytes);

            var ab = bytes.Slice(reader.ReadRange((byte)' '));
            Assert.Equal("AB", ab.ToString(SymbolTable.InvariantUtf8));

            var cd = bytes.Slice(reader.ReadRange((byte)'#'));
            Assert.Equal("CD", cd.ToString(SymbolTable.InvariantUtf8));

            var ef = bytes.Slice(reader.ReadRange(new byte[] { (byte)'&', (byte)'&' }));
            Assert.Equal("EF", ef.ToString(SymbolTable.InvariantUtf8));        }

        [Fact]
        public void EmptyBytesReader()
        {
            ReadOnlyBytes bytes = Create("");
            var reader = BytesReader.Create(bytes);
            var range = reader.ReadRange((byte)' ');
            Assert.Equal(Position.End, range.To);

            bytes = Parse("|");
            reader = BytesReader.Create(bytes);
            range = reader.ReadRange((byte)' ');
            Assert.Equal(Position.End, range.To);
        }

        [Fact]
        public void BytesReaderParse()
        {
            ReadOnlyBytes bytes = Parse("12|3Tr|ue|456Tr|ue7|89False|");
            var reader = BytesReader.Create(bytes);

            Assert.True(reader.TryParseUInt64(out ulong u64));
            Assert.Equal(123ul, u64);

            Assert.True(reader.TryParseBoolean(out bool b));
            Assert.Equal(true, b);

            Assert.True(reader.TryParseUInt64(out u64));
            Assert.Equal(456ul, u64);

            Assert.True(reader.TryParseBoolean(out b));
            Assert.Equal(true, b);

            Assert.True(reader.TryParseUInt64(out u64));
            Assert.Equal(789ul, u64);

            Assert.True(reader.TryParseBoolean(out b));
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
                sb.Append("123456789012345678\r\n");
            }
            var data = Encoding.UTF8.GetBytes(sb.ToString());

            var eol = new Span<byte>(s_eol);
            var bytes = new ReadOnlyBytes(data);

            var reader = BytesReader.Create(bytes);

            while (true)
            {
                var range = reader.ReadRange(eol);
                if (range.To == Position.End) break;
            }
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
        public static string ToString(this ReadOnlyBytes? bytes, SymbolTable symbolTable)
        {
            if (!bytes.HasValue) return string.Empty;
            return ToString(bytes.Value, symbolTable);
        }

        public static string ToString(this ReadOnlyBytes bytes, SymbolTable symbolTable)
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
    }
}
