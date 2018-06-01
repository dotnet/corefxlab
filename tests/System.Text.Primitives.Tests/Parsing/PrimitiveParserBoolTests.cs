// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using Xunit;
using System.Runtime.InteropServices;

namespace System.Text.Primitives.Tests
{
    public partial class PrimitiveParserTests
    {
        [Theory]
        [InlineData("True", 4, true, 4)]
        [InlineData("False", 5, false, 5)]
        [InlineData("True1234", 4, true, 4)]
        [InlineData("False1234", 5, false, 5)]
        [InlineData("True1234", 6, true, 4)]
        [InlineData("False1234", 7, false, 5)]
        public unsafe void BooleanPositiveTests(string text, int length, bool expectedValue, int expectedConsumed)
        {
            byte[] byteBuffer = Text.Encoding.UTF8.GetBytes(text);
            ReadOnlySpan<byte> byteSpan = new ReadOnlySpan<byte>(byteBuffer);

            char[] charBuffer = text.ToCharArray();
            ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(charBuffer);

            bool result;

            result = CustomParser.TryParseBoolean(byteSpan, out bool actualValue, out int actualConsumed, SymbolTable.InvariantUtf8);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);

            result = Utf8Parser.TryParse(byteSpan, out actualValue, out actualConsumed);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);

            ReadOnlySpan<byte> utf16ByteSpan = MemoryMarshal.AsBytes(charSpan);
            result = CustomParser.TryParseBoolean(utf16ByteSpan, out actualValue, out actualConsumed, SymbolTable.InvariantUtf16);
            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed / 2);

            result = Utf16Parser.TryParseBoolean(charSpan, out actualValue);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);

            result = Utf16Parser.TryParseBoolean(charSpan, out actualValue, out actualConsumed);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);
        }
    }
}
