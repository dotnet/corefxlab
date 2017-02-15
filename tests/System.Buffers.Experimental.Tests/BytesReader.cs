// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Buffers;
using System.Collections.Sequences;
using System.Text;
using System.Text.Utf8;
using Xunit;

namespace System.Slices.Tests
{
    public partial class ReadOnlyBytesTests
    {
        [Fact]
        public void SingleSegmentBytesReader()
        {
            ReadOnlyBytes bytes = Create("AB CD#EF&&");
            var reader = new BytesReader(bytes);

            var ab = reader.ReadBytesUntil((byte)' ');
            Assert.Equal("AB", ab.ToString(TextEncoder.Utf8));

            reader.Advance(1);
            var cd = reader.ReadBytesUntil((byte)'#');
            Assert.Equal("CD", cd.ToString(TextEncoder.Utf8));

            reader.Advance(1);
            var ef = reader.ReadBytesUntil(new byte[] { (byte)'&', (byte)'&' });
            Assert.Equal("EF", ef.ToString(TextEncoder.Utf8));

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
            Assert.Equal("AB", ab.ToString(TextEncoder.Utf8));

            reader.Advance(1);
            var cd = reader.ReadBytesUntil((byte)'#');
            Assert.Equal("CD", cd.ToString(TextEncoder.Utf8));

            reader.Advance(1);
            var ef = reader.ReadBytesUntil(new byte[] { (byte)'&', (byte)'&' });
            Assert.Equal("EF", ef.ToString(TextEncoder.Utf8));

            reader.Advance(2);

            //Assert.True(reader.IsEmpty);
        }

        [Fact]
        public void EmptyBytesReader()
        {
            ReadOnlyBytes bytes = Create("");
            var reader = new BytesReader(bytes);
            var found = reader.ReadBytesUntil((byte)' ');
            Assert.Equal("", found.ToString(TextEncoder.Utf8));

            bytes = Parse("|");
            reader = new BytesReader(bytes);
            found = reader.ReadBytesUntil((byte)' ');
            Assert.Equal("", found.ToString(TextEncoder.Utf8));

            //Assert.True(reader.IsEmpty);
        }

        [Fact]
        public void BytesReaderParse()
        {
            ulong u64;
            bool b;

            ReadOnlyBytes bytes = Parse("12|3Tr|ue|456Tr|ue7|89False|");
            var reader = new BytesReader(bytes);

            Assert.True(reader.TryParseUInt64(out u64));
            Assert.Equal(123ul, u64);

            Assert.True(reader.TryParseBoolean(out b));
            Assert.Equal(true, b);

            Assert.True(reader.TryParseUInt64(out u64));
            Assert.Equal(456ul, u64);

            Assert.True(reader.TryParseBoolean(out b));
            Assert.Equal(true, b);

            Assert.True(reader.TryParseUInt64(out u64));
            Assert.Equal(789ul, u64);

            Assert.True(reader.TryParseBoolean(out b));
            Assert.Equal(false, b);

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

            var reader = new BytesReader(bytes, TextEncoder.Utf8);

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

        public static string ToString(this ReadOnlyBytes? bytes, TextEncoder encoder)
        {
            if (bytes == null) return "";
            return ToString(bytes.Value, encoder);
        }

        public static string ToString(this ReadOnlyBytes bytes, TextEncoder encoder)
        {
            var sb = new StringBuilder();
            if (encoder.Encoding == TextEncoder.EncodingName.Utf8)
            {
                var position = Position.First;
                ReadOnlyMemory<byte> segment;
                while (bytes.TryGet(ref position, out segment))
                {
                    sb.Append(new Utf8String(segment.Span));
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