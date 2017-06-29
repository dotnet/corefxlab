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
            ParsedFormat parsed;

            parsed = ParsedFormat.Parse("B");
            Verify(parsed, ParsedFormat.NoPrecision, 'B');

            parsed = ParsedFormat.Parse("b");
            Verify(parsed, ParsedFormat.NoPrecision, 'b');

            parsed = ParsedFormat.Parse("D");
            Verify(parsed, ParsedFormat.NoPrecision, 'D');

            parsed = ParsedFormat.Parse("d");
            Verify(parsed, ParsedFormat.NoPrecision, 'd');

            parsed = ParsedFormat.Parse("E");
            Verify(parsed, ParsedFormat.NoPrecision, 'E');

            parsed = ParsedFormat.Parse("e");
            Verify(parsed, ParsedFormat.NoPrecision, 'e');

            parsed = ParsedFormat.Parse("F");
            Verify(parsed, ParsedFormat.NoPrecision, 'F');

            parsed = ParsedFormat.Parse("f");
            Verify(parsed, ParsedFormat.NoPrecision, 'f');

            parsed = ParsedFormat.Parse("G");
            Verify(parsed, ParsedFormat.NoPrecision, 'G');

            parsed = ParsedFormat.Parse("g");
            Verify(parsed, ParsedFormat.NoPrecision, 'g');

            parsed = ParsedFormat.Parse("N");
            Verify(parsed, ParsedFormat.NoPrecision, 'N');

            parsed = ParsedFormat.Parse("n");
            Verify(parsed, ParsedFormat.NoPrecision, 'n');

            parsed = ParsedFormat.Parse("O");
            Verify(parsed, ParsedFormat.NoPrecision, 'O');

            parsed = ParsedFormat.Parse("o");
            Verify(parsed, ParsedFormat.NoPrecision, 'o');

            parsed = ParsedFormat.Parse("P");
            Verify(parsed, ParsedFormat.NoPrecision, 'P');

            parsed = ParsedFormat.Parse("p");
            Verify(parsed, ParsedFormat.NoPrecision, 'p');

            parsed = ParsedFormat.Parse("R");
            Verify(parsed, ParsedFormat.NoPrecision, 'R');

            parsed = ParsedFormat.Parse("r");
            Verify(parsed, ParsedFormat.NoPrecision, 'r');

            parsed = ParsedFormat.Parse("t");
            Verify(parsed, ParsedFormat.NoPrecision, 't');

            parsed = ParsedFormat.Parse("X");
            Verify(parsed, ParsedFormat.NoPrecision, 'X');

            parsed = ParsedFormat.Parse("x");
            Verify(parsed, ParsedFormat.NoPrecision, 'x');
        }

        [Fact]
        public void ParsePrecisionFormats()
        {
            ParsedFormat parsed;

            parsed = ParsedFormat.Parse("D1");
            Verify(parsed, 1, 'D');

            parsed = ParsedFormat.Parse("d2");
            Verify(parsed, 2, 'd');

            parsed = ParsedFormat.Parse("E3");
            Verify(parsed, 3, 'E');

            parsed = ParsedFormat.Parse("e4");
            Verify(parsed, 4, 'e');

            parsed = ParsedFormat.Parse("F5");
            Verify(parsed, 5, 'F');

            parsed = ParsedFormat.Parse("f6");
            Verify(parsed, 6, 'f');

            parsed = ParsedFormat.Parse("G7");
            Verify(parsed, 7, 'G');

            parsed = ParsedFormat.Parse("g99");
            Verify(parsed, 99, 'g');

            parsed = ParsedFormat.Parse("N98");
            Verify(parsed, 98, 'N');

            parsed = ParsedFormat.Parse("n10");
            Verify(parsed, 10, 'n');

            parsed = ParsedFormat.Parse("X11");
            Verify(parsed, 11, 'X');

            parsed = ParsedFormat.Parse("x10");
            Verify(parsed, 10, 'x');

            
        }

        [Fact]
        public void ParseThrowsExceptionWhenParsedPrecisionExceedsMaxPrecision()
        {
            var ex = Assert.Throws<FormatException>(() => ParsedFormat.Parse($"x{100}"));
        }

        private static void Verify(ParsedFormat format, byte expectedPrecision, char expectedSymbol)
        {
            Assert.Equal(format.Precision, expectedPrecision);
            Assert.Equal(format.Symbol, expectedSymbol.ToString()[0]);
        }
    }
}
