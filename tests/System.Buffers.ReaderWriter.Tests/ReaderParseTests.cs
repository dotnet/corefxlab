// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Reader;
using System.Text;
using Xunit;

namespace System.Buffers.Tests
{
    public class ReaderParseTests
    {
        private static readonly byte[] s_array;
        private static readonly ReadOnlySequence<byte> s_ros;

        static ReaderParseTests()
        {
            var sections = 100000;
            var section = "1234 ";
            var builder = new StringBuilder(sections * section.Length);
            for (int i = 0; i < sections; i++)
            {
                builder.Append(section);
            }
            s_array = Encoding.UTF8.GetBytes(builder.ToString());
            s_ros = new ReadOnlySequence<byte>(s_array);
        }

        [Fact]
        public void TryParseRos()
        {
            var reader = new BufferReader<byte>(s_ros);

            while (reader.TryParse(out int value))
            {
                reader.Advance(1); // advance past the delimiter
                Assert.Equal(1234, value);
            }
        }

        [Fact]
        public void TryParseInt_MultiSegment()
        {
            ReadOnlySequence<byte> bytes = BufferFactory.Create(new byte[][] {
                Encoding.UTF8.GetBytes("-123"),
                Encoding.UTF8.GetBytes("45")
            });

            var reader = new BufferReader<byte>(bytes);

            Assert.True(reader.TryParse(out int value));
            Assert.Equal(-12345, value);

            bytes = BufferFactory.Create(new byte[][] {
                Encoding.UTF8.GetBytes("-1,2,3"),
                Encoding.UTF8.GetBytes("4,5.0000000000NewData")
            });

            reader = new BufferReader<byte>(bytes);

            Assert.True(reader.TryParse(out value));
            Assert.Equal(-1, value);

            reader = new BufferReader<byte>(bytes);
            Assert.True(reader.TryParse(out value, 'N'));
            Assert.Equal(-12345, value);

            reader = new BufferReader<byte>(bytes);
            Assert.False(reader.TryParse(out value, 'X'));
            reader.Advance(1);
            Assert.True(reader.TryParse(out value, 'X'));
            Assert.Equal(1, value);

            bytes = BufferFactory.Create(new byte[][] {
                Encoding.UTF8.GetBytes("FEE"),
                Encoding.UTF8.GetBytes("D")
            });

            reader = new BufferReader<byte>(bytes);
            Assert.True(reader.TryParse(out value, 'X'));
            Assert.Equal(0xFEED, value);

            bytes = BufferFactory.Create(new byte[][] {
                Encoding.UTF8.GetBytes("FE"),
                Encoding.UTF8.GetBytes("ED"),
                Encoding.UTF8.GetBytes("BEEFBEE")
            });

            // Overflow
            reader = new BufferReader<byte>(bytes);
            Assert.False(reader.TryParse(out value, 'X'));

            reader.Advance(3);
            Assert.True(reader.TryParse(out value, 'X'));
            Assert.Equal(unchecked((int)0xDBEEFBEE), value);

            // Heap read
            bytes = BufferFactory.Create(new byte[][] {
                Encoding.UTF8.GetBytes("-0000000000"),
                Encoding.UTF8.GetBytes("0000000000"),
                Encoding.UTF8.GetBytes("0000000000"),
                Encoding.UTF8.GetBytes("0000000000"),
                Encoding.UTF8.GetBytes("0000000000"),
                Encoding.UTF8.GetBytes("0000000000"),
                Encoding.UTF8.GetBytes("0000000000"),
                Encoding.UTF8.GetBytes("0000000000"),
                Encoding.UTF8.GetBytes("0000000000"),
                Encoding.UTF8.GetBytes("0000000000"),
                Encoding.UTF8.GetBytes("0000000000"),
                Encoding.UTF8.GetBytes("0000000000"),
                Encoding.UTF8.GetBytes("0000000000"),
                Encoding.UTF8.GetBytes("0000000000"),
                Encoding.UTF8.GetBytes("123"),
                Encoding.UTF8.GetBytes("45")
            });

            reader = new BufferReader<byte>(bytes);

            Assert.True(reader.TryParse(out value));
            Assert.Equal(-12345, value);

            // Heap overflow
            bytes = BufferFactory.Create(new byte[][] {
                Encoding.UTF8.GetBytes("-0000000000"),
                Encoding.UTF8.GetBytes("0000000000"),
                Encoding.UTF8.GetBytes("0000000000"),
                Encoding.UTF8.GetBytes("0000000000"),
                Encoding.UTF8.GetBytes("0000000000"),
                Encoding.UTF8.GetBytes("0000000000"),
                Encoding.UTF8.GetBytes("0000000000"),
                Encoding.UTF8.GetBytes("0000000000"),
                Encoding.UTF8.GetBytes("0000000000"),
                Encoding.UTF8.GetBytes("0000000000"),
                Encoding.UTF8.GetBytes("0000000000"),
                Encoding.UTF8.GetBytes("0000000000"),
                Encoding.UTF8.GetBytes("0000000000"),
                Encoding.UTF8.GetBytes("0000000000"),
                Encoding.UTF8.GetBytes("1234567891"),
                Encoding.UTF8.GetBytes("2345678901")
            });

            Assert.False(reader.TryParse(out value));
        }

        [Fact]
        public void TryParseInt_SingleNonDigit()
        {
            // Testing single segments that aren't valid on their own, but are with additional segments

            ReadOnlySequence<byte> bytes = BufferFactory.Create(new byte[][] {
                Encoding.UTF8.GetBytes("-"),
                Encoding.UTF8.GetBytes("123")
            });

            var reader = new BufferReader<byte>(bytes);

            Assert.True(reader.TryParse(out int value));
            Assert.Equal(-123, value);

            bytes = BufferFactory.Create(new byte[][] {
                Encoding.UTF8.GetBytes("+"),
                Encoding.UTF8.GetBytes("123")
            });

            reader = new BufferReader<byte>(bytes);

            Assert.True(reader.TryParse(out value));
            Assert.Equal(123, value);

            bytes = BufferFactory.Create(new byte[][] {
                Encoding.UTF8.GetBytes("."),
                Encoding.UTF8.GetBytes("0")
            });

            reader = new BufferReader<byte>(bytes);

            Assert.False(reader.TryParse(out value, 'd'));
            Assert.True(reader.TryParse(out value, 'n'));
            Assert.Equal(0, value);
        }
    }
}
