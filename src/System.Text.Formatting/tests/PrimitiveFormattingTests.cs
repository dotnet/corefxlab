// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Text.Formatting;
using Xunit;

namespace System.Text.Formatting.Tests
{
    public partial class SystemTextFormattingTests
    {
        StringFormatter formatter = new StringFormatter();

        private void CheckByte(byte value, string format, string expected)
        {
            var parsed = Format.Parse(format);
            formatter.Clear();
            formatter.Append(value, parsed);
            var result = formatter.ToString();
            Assert.Equal(expected, result);

            var clrResult = value.ToString(format);
            Assert.Equal(clrResult, result);
        }

        [Fact]
        public void Byte()
        {
            CheckByte(0, null, "0");
            CheckByte(1, null, "1");
            CheckByte(10, null, "10");
            CheckByte(byte.MaxValue, null, "255");

            CheckByte(0, "", "0");
            CheckByte(1, "", "1");
            CheckByte(10, "", "10");
            CheckByte(byte.MaxValue, "", "255");

            CheckByte(0, "g", "0");
            CheckByte(1, "g", "1");
            CheckByte(10, "g", "10");
            CheckByte(byte.MaxValue, "g", "255");

            CheckByte(0, "d", "0");
            CheckByte(1, "d", "1");
            CheckByte(10, "d", "10");
            CheckByte(byte.MaxValue, "d", "255");

            CheckByte(0, "d0", "0");
            CheckByte(0, "d1", "0");
            CheckByte(0, "d2", "00");
            CheckByte(0, "d10", "0000000000");

            CheckByte(1, "d0", "1");
            CheckByte(1, "d1", "1");
            CheckByte(1, "d2", "01");
            CheckByte(1, "d10", "0000000001");

            CheckByte(21, "d0", "21");
            CheckByte(21, "d1", "21");
            CheckByte(21, "d2", "21");
            CheckByte(21, "d10", "0000000021");

            CheckByte(0, "x", "0");
            CheckByte(1, "x", "1");
            CheckByte(10, "x", "a");
            CheckByte(byte.MaxValue, "x", "ff");

            CheckByte(0, "X", "0");
            CheckByte(1, "X", "1");
            CheckByte(10, "X", "A");
            CheckByte(byte.MaxValue, "X", "FF");
        }

        private void CheckInt64(long value, string format, string expected)
        {
            var parsed = Format.Parse(format);
            formatter.Clear();
            formatter.Append(value, parsed);
            var result = formatter.ToString();
            Assert.Equal(expected, result);

            var clrResult = value.ToString(format);
            Assert.Equal(clrResult, result);
        }

        [Fact]
        public void Int64()
        {
            CheckInt64(long.MinValue, null, "-9223372036854775808");
            CheckInt64(-10, null, "-10");
            CheckInt64(-1, null, "-1");
            CheckInt64(0, null, "0");
            CheckInt64(1, null, "1");
            CheckInt64(10, null, "10");
            CheckInt64(long.MaxValue, null, "9223372036854775807");

            CheckInt64(long.MinValue, "d", "-9223372036854775808");
            CheckInt64(-10, "d", "-10");
            CheckInt64(-1, "d", "-1");
            CheckInt64(0, "d", "0");
            CheckInt64(1, "d", "1");
            CheckInt64(10, "d", "10");
            CheckInt64(long.MaxValue, "d", "9223372036854775807");

            CheckInt64(long.MinValue, "x", "8000000000000000");
            CheckInt64(-10, "x", "fffffffffffffff6");
            CheckInt64(-1, "x", "ffffffffffffffff");
            CheckInt64(0, "x", "0");
            CheckInt64(1, "x", "1");
            CheckInt64(10, "x", "a");
            CheckInt64(long.MaxValue, "x", "7fffffffffffffff");

            CheckInt64(long.MinValue, "X", "8000000000000000");
            CheckInt64(-10,  "X", "FFFFFFFFFFFFFFF6");
            CheckInt64(-1,  "X", "FFFFFFFFFFFFFFFF");
            CheckInt64(0,  "X", "0");
            CheckInt64(1, "X", "1");
            CheckInt64(10, "X", "A");
            CheckInt64(long.MaxValue, "X", "7FFFFFFFFFFFFFFF");

            CheckInt64(0, "d0", "0");
            CheckInt64(0, "d1", "0");
            CheckInt64(0, "d2", "00");
            CheckInt64(0, "d10", "0000000000");

            CheckInt64(1, "d0", "1");
            CheckInt64(1, "d1", "1");
            CheckInt64(1, "d2", "01");
            CheckInt64(1, "d10", "0000000001");

            CheckInt64(21, "d0", "21");
            CheckInt64(21, "d1", "21");
            CheckInt64(21, "d2", "21");
            CheckInt64(21, "d10", "0000000021");

            CheckInt64(-1, "d0", "-1");
            CheckInt64(-1, "d1", "-1");
            CheckInt64(-1, "d2", "-01");
            CheckInt64(-1, "d10", "-0000000001");
        }

        [Fact]
        public void FloatFormatting()
        {
            var sb = new StringFormatter();

            sb.Append(Double.NaN);
            var result = sb.ToString();
            Assert.Equal("NaN", result);
            sb.Clear();

            sb.Append(Double.PositiveInfinity);
            result = sb.ToString();
            Assert.Equal("Infinity", result);
            sb.Clear();

            sb.Append(Double.NegativeInfinity);
            result = sb.ToString();
            Assert.Equal("-Infinity", result);
            sb.Clear();

            sb.Append(1.2);
            result = sb.ToString();
            Assert.Equal("1.2", result);
        }

        [Fact]
        public void FormatDefault()
        {
            var sb = new StringFormatter();
            sb.Append('C');
            sb.Append((sbyte)-10);
            sb.Append((byte)99);
            sb.Append((short)-10);
            sb.Append((ushort)99);
            sb.Append((int)-10);
            sb.Append((uint)99);
            sb.Append((long)-10);
            sb.Append((ulong)99);
            var result = sb.ToString();
            Assert.Equal("C-1099-1099-1099-1099", result);
        }

        [Fact]
        public void FormatD()
        {
            var format = Format.Parse("D");
            var sb = new StringFormatter();
            sb.Append((sbyte)-10, format);
            sb.Append((byte)99, format);
            sb.Append((short)-10, format);
            sb.Append((ushort)99, format);
            sb.Append((int)-10, format);
            sb.Append((uint)99, format);
            sb.Append((long)-10, format);
            sb.Append((ulong)99, format);
            var result = sb.ToString();
            Assert.Equal("-1099-1099-1099-1099", result);
        }

        [Fact]
        public void FormatDPrecision()
        {
            var format = Format.Parse("D3");
            var sb = new StringFormatter();
            sb.Append((sbyte)-10, format);
            sb.Append((byte)99, format);
            sb.Append((short)-10, format);
            sb.Append((ushort)99, format);
            sb.Append((int)-10, format);
            sb.Append((uint)99, format);
            sb.Append((long)-10, format);
            sb.Append((ulong)99, format);
            var result = sb.ToString();
            Assert.Equal("-010099-010099-010099-010099", result);
        }

        [Fact]
        public void FormatG()
        {
            var format = Format.Parse("G");
            var sb = new StringFormatter();
            sb.Append((sbyte)-10, format);
            sb.Append((byte)99, format);
            sb.Append((short)-10, format);
            sb.Append((ushort)99, format);
            sb.Append((int)-10, format);
            sb.Append((uint)99, format);
            sb.Append((long)-10, format);
            sb.Append((ulong)99, format);
            var result = sb.ToString();
            Assert.Equal("-1099-1099-1099-1099", result);
        }

        [Fact]
        public void FormatNPrecision()
        {
            var format = Format.Parse("N1");
            var sb = new StringFormatter();
            sb.Append((sbyte)-10, format);
            sb.Append((byte)99, format);
            sb.Append((short)-10, format);
            sb.Append((ushort)99, format);
            sb.Append((int)-10, format);
            sb.Append((uint)99, format);
            sb.Append((long)-10, format);
            sb.Append((ulong)99, format);
            var result = sb.ToString();
            Assert.Equal("-10.099.0-10.099.0-10.099.0-10.099.0", result);
        }

        [Fact]
        public void FormatX()
        {
            var x = Format.Parse("x");
            var X = Format.Parse("X");

            var sb = new StringFormatter();
            sb.Append((ulong)255, x);
            sb.Append((uint)255, X);
            Assert.Equal("ffFF", sb.ToString());

            sb.Clear();
            sb.Append((int)-1, X);
            Assert.Equal("FFFFFFFF", sb.ToString());

            sb.Clear();
            sb.Append((int)-2, X);
            Assert.Equal("FFFFFFFE", sb.ToString());
        }

        [Fact]
        public void FormatXPrecision()
        {
            var x = Format.Parse("x10");
            var X = Format.Parse("X10");

            var sb = new StringFormatter();
            sb.Append((ulong)255, x);
            sb.Append((uint)255, X);
            Assert.Equal("00000000ff00000000FF", sb.ToString());

            sb.Clear();
            sb.Append((int)-1, X);
            Assert.Equal("00FFFFFFFF", sb.ToString());

            sb.Clear();
            sb.Append((int)-2, X);
            Assert.Equal("00FFFFFFFE", sb.ToString());
        }

        [Fact]
        public void Int32ToStreamUtf8()
        {
            var buffer = new byte[1024];
            MemoryStream stream = new MemoryStream(buffer);

            var writer = new StreamFormatter(stream, FormattingData.InvariantUtf8);
            writer.Append(100);
            writer.Append(-100);
            writer.Append('h');
            writer.Append("i!");
            AssertUtf8Equal(buffer.Slice(0, (int)stream.Position), "100-100hi!");
        }

        internal static void AssertUtf8Equal(Span<byte> formatted, string expected)
        {
            var expectedBytes = Encoding.UTF8.GetBytes(expected);
            Assert.Equal(expectedBytes.Length, formatted.Length);
            for (int index = 0; index < expected.Length; index++)
            {
                Assert.Equal(formatted[index], expectedBytes[index]);
            }
        }
    }
}
