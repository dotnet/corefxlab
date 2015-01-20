// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Text.Formatting;
using Xunit;

namespace System.Text.Formatting.Tests
{
    public partial class SystemTextFormattingTests
    {
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
            var format = Format.Parsed.Parse("D");
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
            var format = Format.Parsed.Parse("D3");
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
            var format = Format.Parsed.Parse("G");
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
            var format = Format.Parsed.Parse("N1");
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
            var x = Format.Parsed.Parse("x");
            var X = Format.Parsed.Parse("X");

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
            var x = Format.Parsed.Parse("x10");
            var X = Format.Parsed.Parse("X10");

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
