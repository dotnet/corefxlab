// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using Xunit;

namespace System.Text.Primitives.Tests
{
    public partial class PrimitiveParserTests
    {
        [Theory]
        [InlineData("308.2", 5, 308.2, 5)]
        [InlineData("-51.5909", 8, -51.5909, 8)]
        [InlineData("-51.5909M", 9, -51.5909, 8)]
        [InlineData("-51.500", 7, -51.5, 7)]
        public unsafe void DecimalPositiveTests(string text, int length, decimal expectedValue, int expectedConsumed)
        {
            byte[] byteBuffer = Text.Encoding.UTF8.GetBytes(text);
            ReadOnlySpan<byte> byteSpan = new ReadOnlySpan<byte>(byteBuffer);

            char[] charBuffer = text.ToCharArray();
            ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(charBuffer);

            bool result;
            decimal actualValue;
            int actualConsumed;

            result = CustomParser.TryParseDecimal(byteSpan, out actualValue, out actualConsumed, SymbolTable.InvariantUtf8);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);

            //fixed (byte* bytePointer = byteBuffer)
            //{
            //    result = Parsers.Utf8.TryParseDecimal(bytePointer, length, out actualValue);

            //    Assert.True(result);
            //    Assert.Equal(expectedValue, actualValue);

            //    result = Parsers.Utf8.TryParseDecimal(bytePointer, length, out actualValue, out actualConsumed);

            //    Assert.True(result);
            //    Assert.Equal(expectedValue, actualValue);
            //    Assert.Equal(expectedConsumed, actualConsumed);
            //}

            result = Utf8Parser.TryParseDecimal(byteSpan, out actualValue);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);

            result = Utf8Parser.TryParseDecimal(byteSpan, out actualValue, out actualConsumed);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);

            ReadOnlySpan<byte> utf16ByteSpan = charSpan.AsBytes();
            result = CustomParser.TryParseDecimal(utf16ByteSpan, out actualValue, out actualConsumed, SymbolTable.InvariantUtf16);
            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed / 2);

            //fixed (char* charPointer = charBuffer)
            //{
            //    result = Parsers.Utf16.TryParseDecimal(charPointer, length, out actualValue);

            //    Assert.True(result);
            //    Assert.Equal(expectedValue, actualValue);

            //    result = Parsers.Utf16.TryParseDecimal(charPointer, length, out actualValue, out actualConsumed);

            //    Assert.True(result);
            //    Assert.Equal(expectedValue, actualValue);
            //    Assert.Equal(expectedConsumed, actualConsumed);
            //}

            result = Utf16Parser.TryParseDecimal(charSpan, out actualValue);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);

            result = Utf16Parser.TryParseDecimal(charSpan, out actualValue, out actualConsumed);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);
        }
    }
}
