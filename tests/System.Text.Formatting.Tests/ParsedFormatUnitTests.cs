// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace System.Text.Formatting.Tests
{
    public class ParsedFormatUnitTests
    {
        [Fact]
        public void ParseSimpleFormats()
        {
            TextFormat parsed;

            parsed = TextFormat.Parse("B");
            Verify(parsed, TextFormat.NoPrecision, 'B');

            parsed = TextFormat.Parse("b");
            Verify(parsed, TextFormat.NoPrecision, 'b');

            parsed = TextFormat.Parse("D");
            Verify(parsed, TextFormat.NoPrecision, 'D');

            parsed = TextFormat.Parse("d");
            Verify(parsed, TextFormat.NoPrecision, 'd');

            parsed = TextFormat.Parse("E");
            Verify(parsed, TextFormat.NoPrecision, 'E');

            parsed = TextFormat.Parse("e");
            Verify(parsed, TextFormat.NoPrecision, 'e');

            parsed = TextFormat.Parse("F");
            Verify(parsed, TextFormat.NoPrecision, 'F');

            parsed = TextFormat.Parse("f");
            Verify(parsed, TextFormat.NoPrecision, 'f');

            parsed = TextFormat.Parse("G");
            Verify(parsed, TextFormat.NoPrecision, 'G');

            parsed = TextFormat.Parse("g");
            Verify(parsed, TextFormat.NoPrecision, 'g');

            parsed = TextFormat.Parse("N");
            Verify(parsed, TextFormat.NoPrecision, 'N');

            parsed = TextFormat.Parse("n");
            Verify(parsed, TextFormat.NoPrecision, 'n');

            parsed = TextFormat.Parse("O");
            Verify(parsed, TextFormat.NoPrecision, 'O');

            parsed = TextFormat.Parse("o");
            Verify(parsed, TextFormat.NoPrecision, 'o');

            parsed = TextFormat.Parse("P");
            Verify(parsed, TextFormat.NoPrecision, 'P');

            parsed = TextFormat.Parse("p");
            Verify(parsed, TextFormat.NoPrecision, 'p');

            parsed = TextFormat.Parse("R");
            Verify(parsed, TextFormat.NoPrecision, 'R');

            parsed = TextFormat.Parse("r");
            Verify(parsed, TextFormat.NoPrecision, 'r');

            parsed = TextFormat.Parse("t");
            Verify(parsed, TextFormat.NoPrecision, 't');

            parsed = TextFormat.Parse("X");
            Verify(parsed, TextFormat.NoPrecision, 'X');

            parsed = TextFormat.Parse("x");
            Verify(parsed, TextFormat.NoPrecision, 'x');
        }

        [Fact]
        public void ParsePrecisionFormats()
        {
            TextFormat parsed;

            parsed = TextFormat.Parse("D1");
            Verify(parsed, 1, 'D');

            parsed = TextFormat.Parse("d2");
            Verify(parsed, 2, 'd');

            parsed = TextFormat.Parse("E3");
            Verify(parsed, 3, 'E');

            parsed = TextFormat.Parse("e4");
            Verify(parsed, 4, 'e');

            parsed = TextFormat.Parse("F5");
            Verify(parsed, 5, 'F');

            parsed = TextFormat.Parse("f6");
            Verify(parsed, 6, 'f');

            parsed = TextFormat.Parse("G7");
            Verify(parsed, 7, 'G');

            parsed = TextFormat.Parse("g99");
            Verify(parsed, 99, 'g');

            parsed = TextFormat.Parse("N98");
            Verify(parsed, 98, 'N');

            parsed = TextFormat.Parse("n10");
            Verify(parsed, 10, 'n');

            parsed = TextFormat.Parse("X11");
            Verify(parsed, 11, 'X');

            parsed = TextFormat.Parse("x10");
            Verify(parsed, 10, 'x');

            
        }

        [Fact]
        public void ParseThrowsExceptionWhenParsedPrecisionExceedsMaxPrecision()
        {
            var ex = Assert.Throws<Exception>(() => TextFormat.Parse($"x{100}"));
        }

        private static void Verify(TextFormat format, byte expectedPrecision, char expectedSymbol)
        {
            Assert.Equal(format.Precision, expectedPrecision);
            Assert.Equal(format.Symbol, expectedSymbol.ToString()[0]);
        }
    }
}
