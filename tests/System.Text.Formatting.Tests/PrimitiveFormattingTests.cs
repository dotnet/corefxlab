// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Utf8;
using Xunit;

namespace System.Text.Formatting.Tests
{
    public partial class SystemTextFormattingTests
    {
        ArrayPool<byte> pool = ArrayPool<byte>.Shared;

        public SystemTextFormattingTests()
        {
            var culture = (CultureInfo)CultureInfo.InvariantCulture.Clone();

            culture.DateTimeFormat.LongTimePattern = "h:mm:ss tt";
            culture.DateTimeFormat.ShortTimePattern = "h:mm tt";
            culture.DateTimeFormat.LongDatePattern = "dddd, d MMMM yyyy";
            culture.DateTimeFormat.ShortDatePattern = "M/d/yyyy";

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }

        private void CheckByte(byte value, string format, string expected)
        {
            var formatter = new StringFormatter(pool);
            var parsed = Format.Parse(format);
            formatter.Clear();
            formatter.Append(value, parsed);
            var result = formatter.ToString();
            Assert.Equal(expected, result);

            var clrResult = value.ToString(format, CultureInfo.InvariantCulture);
            Assert.Equal(clrResult, result);
        }

        [Fact]
        public void ByteBasicTests()
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

        private void CheckInt64(long value, string format, string expected, StringFormatter formatter)
        {
            var parsed = Format.Parse(format);
            formatter.Clear();
            formatter.Append(value, parsed);
            var result = formatter.ToString();
            Assert.Equal(expected, result);

            var clrResult = value.ToString(format, CultureInfo.InvariantCulture);
            Assert.Equal(clrResult, result);
        }

        [Fact]
        public void Int64BasicTests()
        {
            var formatter = new StringFormatter(pool);
            CheckInt64(long.MinValue, null, "-9223372036854775808", formatter);
            CheckInt64(-10, null, "-10", formatter);
            CheckInt64(-1, null, "-1", formatter);
            CheckInt64(0, null, "0", formatter);
            CheckInt64(1, null, "1", formatter);
            CheckInt64(10, null, "10", formatter);
            CheckInt64(long.MaxValue, null, "9223372036854775807", formatter);

            CheckInt64(long.MinValue, "d", "-9223372036854775808", formatter);
            CheckInt64(-10, "d", "-10", formatter);
            CheckInt64(-1, "d", "-1", formatter);
            CheckInt64(0, "d", "0", formatter);
            CheckInt64(1, "d", "1", formatter);
            CheckInt64(10, "d", "10", formatter);
            CheckInt64(long.MaxValue, "d", "9223372036854775807", formatter);

            CheckInt64(long.MinValue, "x", "8000000000000000", formatter);
            CheckInt64(-10, "x", "fffffffffffffff6", formatter);
            CheckInt64(-1, "x", "ffffffffffffffff", formatter);
            CheckInt64(0, "x", "0", formatter);
            CheckInt64(1, "x", "1", formatter);
            CheckInt64(10, "x", "a", formatter);
            CheckInt64(long.MaxValue, "x", "7fffffffffffffff", formatter);

            CheckInt64(long.MinValue, "X", "8000000000000000", formatter);
            CheckInt64(-10,  "X", "FFFFFFFFFFFFFFF6", formatter);
            CheckInt64(-1,  "X", "FFFFFFFFFFFFFFFF", formatter);
            CheckInt64(0,  "X", "0", formatter);
            CheckInt64(1, "X", "1", formatter);
            CheckInt64(10, "X", "A", formatter);
            CheckInt64(long.MaxValue, "X", "7FFFFFFFFFFFFFFF", formatter);

            CheckInt64(0, "d0", "0", formatter);
            CheckInt64(0, "d1", "0", formatter);
            CheckInt64(0, "d2", "00", formatter);
            CheckInt64(0, "d10", "0000000000", formatter);

            CheckInt64(1, "d0", "1", formatter);
            CheckInt64(1, "d1", "1", formatter);
            CheckInt64(1, "d2", "01", formatter);
            CheckInt64(1, "d10", "0000000001", formatter);

            CheckInt64(21, "d0", "21", formatter);
            CheckInt64(21, "d1", "21", formatter);
            CheckInt64(21, "d2", "21", formatter);
            CheckInt64(21, "d10", "0000000021", formatter);

            CheckInt64(-1, "d0", "-1", formatter);
            CheckInt64(-1, "d1", "-1", formatter);
            CheckInt64(-1, "d2", "-01", formatter);
            CheckInt64(-1, "d10", "-0000000001", formatter);
        }

        [Fact]
        public void FloatFormatting()
        {
            var sb = new StringFormatter(pool);

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
            var sb = new StringFormatter(pool);
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
            var sb = new StringFormatter(pool);
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
            var sb = new StringFormatter(pool);
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
            var sb = new StringFormatter(pool);
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
            var sb = new StringFormatter(pool);
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

            var sb = new StringFormatter(pool);
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

            var sb = new StringFormatter(pool);
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

            using(var writer = new StreamFormatter(stream, FormattingData.InvariantUtf8, pool)) {
                writer.Append(100);
                writer.Append(-100);
                writer.Append('h');
                writer.Append("i!");
                AssertUtf8Equal(buffer.Slice(0, (int)stream.Position), "100-100hi!");
            }
        }

        [Fact] // TODO: this should test more than ascii
        public void FormatString()
        {
            var buffer = new byte[1024];
            MemoryStream stream = new MemoryStream(buffer);

            using(var utf8Writer = new StreamFormatter(stream, FormattingData.InvariantUtf8, pool)) {
                utf8Writer.Append("Hello");
                utf8Writer.Append(" ");
                utf8Writer.Append("World!");
                utf8Writer.Append("\u0391"); // greek alpha
                utf8Writer.Append("\uD950\uDF21");
                utf8Writer.Append(new Utf8String("Hello"));
                AssertUtf8Equal(buffer.Slice(0, (int)stream.Position), "Hello World!\u0391\uD950\uDF21Hello");
            }

            stream.Position = 0;
            using(var utf16Writer = new StreamFormatter(stream, FormattingData.InvariantUtf16, pool)) {
                utf16Writer.Append("Hello");
                utf16Writer.Append(" ");
                utf16Writer.Append("World!");
                utf16Writer.Append("\u0391");
                utf16Writer.Append("\uD950\uDF21"); 
                AssertUtf16Equal(buffer.Slice(0, (int)stream.Position), "Hello World!\u0391\uD950\uDF21");
            }
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

        internal static void AssertUtf16Equal(Span<byte> formatted, string expected)
        {
            var expectedBytes = Encoding.Unicode.GetBytes(expected);
            Assert.Equal(expectedBytes.Length, formatted.Length);
            for (int index = 0; index < expected.Length; index++)
            {
                Assert.Equal(formatted[index], expectedBytes[index]);
            }
        }
    }
}
