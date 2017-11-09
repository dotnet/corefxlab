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
            var reader = new BytesReader(bytes);

            var ab = reader.ReadBytesUntil((byte)' ');
            Assert.True(ab.HasValue);
            Assert.Equal("AB", ab.ToString(SymbolTable.InvariantUtf8));

            reader.Advance(1);
            var cd = reader.ReadBytesUntil((byte)'#');
            Assert.Equal("CD", cd.ToString(SymbolTable.InvariantUtf8));

            reader.Advance(1);
            var ef = reader.ReadBytesUntil(new byte[] { (byte)'&', (byte)'&' });
            Assert.Equal("EF", ef.ToString(SymbolTable.InvariantUtf8));

            reader.Advance(2);

            //Assert.True(reader.IsEmpty);
        }

        [Fact]
        public void MultiSegmentBytesReaderNumbers()
        {
            ReadOnlyBytes bytes = ReadOnlyBytes.Create(new byte[][] {
                new byte[] { 0          },
                new byte[] { 1, 2       },
                new byte[] { 3, 4       },
                new byte[] { 5, 6, 7, 8 },
                new byte[] { 8          }
            });

            var reader = new BytesReader(bytes);

            var value = reader.ReadBytesUntil(2).Value.ToSpan();
            Assert.Equal(0, value[0]);
            Assert.Equal(1, value[1]);
            reader.Advance(1);

            value = reader.ReadBytesUntil(5).Value.ToSpan();
            Assert.Equal(3, value[0]);
            Assert.Equal(4, value[1]);
            reader.Advance(1);

            value = reader.ReadBytesUntil(new byte[] { 8, 8 }).Value.ToSpan();
            Assert.Equal(6, value[0]);
            Assert.Equal(7, value[1]);
            reader.Advance(2);

            //Assert.True(reader.IsEmpty);
        }

        [Fact]
        public void MultiSegmentBytesReader()
        {
            ReadOnlyBytes bytes = Parse("A|B |CD|#EF&|&");
            var reader = new BytesReader(bytes);

            var ab = reader.ReadBytesUntil((byte)' ');
            Assert.Equal("AB", ab.ToString(SymbolTable.InvariantUtf8));
            Assert.Equal(2, reader.Index);

            reader.Advance(1);
            Assert.Equal(3, reader.Index);

            var cd = reader.ReadBytesUntil((byte)'#');
            Assert.Equal("CD", cd.ToString(SymbolTable.InvariantUtf8));
            Assert.Equal(5, reader.Index);

            reader.Advance(1);
            Assert.Equal(6, reader.Index);

            var ef = reader.ReadBytesUntil(new byte[] { (byte)'&', (byte)'&' });
            Assert.Equal("EF", ef.ToString(SymbolTable.InvariantUtf8));
            Assert.Equal(8, reader.Index);

            reader.Advance(2);
            Assert.Equal(10, reader.Index);
        }

        [Fact]
        public void EmptyBytesReader()
        {
            ReadOnlyBytes bytes = Create("");
            var reader = new BytesReader(bytes);
            var found = reader.ReadBytesUntil((byte)' ');
            Assert.Equal("", found.ToString(SymbolTable.InvariantUtf8));

            bytes = Parse("|");
            reader = new BytesReader(bytes);
            found = reader.ReadBytesUntil((byte)' ');
            Assert.Equal("", found.ToString(SymbolTable.InvariantUtf8));

            //Assert.True(reader.IsEmpty);
        }

        [Fact]
        public void BytesReaderParse()
        {

            ReadOnlyBytes bytes = Parse("12|3Tr|ue|456Tr|ue7|89False|");
            var reader = new BytesReader(bytes);

            Assert.True(reader.TryParseUInt64(out ulong u64));
            Assert.Equal(123ul, u64);
            Assert.Equal(3, reader.Index);

            Assert.True(reader.TryParseBoolean(out bool b));
            Assert.Equal(true, b);
            Assert.Equal(7, reader.Index);

            Assert.True(reader.TryParseUInt64(out u64));
            Assert.Equal(456ul, u64);
            Assert.Equal(10, reader.Index);

            Assert.True(reader.TryParseBoolean(out b));
            Assert.Equal(true, b);
            Assert.Equal(14, reader.Index);

            Assert.True(reader.TryParseUInt64(out u64));
            Assert.Equal(789ul, u64);
            Assert.Equal(17, reader.Index);

            Assert.True(reader.TryParseBoolean(out b));
            Assert.Equal(false, b);
            Assert.Equal(22, reader.Index);

            //Assert.True(reader.IsEmpty);
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

            var reader = new BytesReader(bytes, SymbolTable.InvariantUtf8);

            while (true)
            {
                var result = reader.ReadBytesUntil(eol);
                if (result == null) break;
                reader.Advance(2);
            }
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
                var position = Position.First;
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
