// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using Xunit;

namespace System.Text.Formatting.Tests
{
    public class ParsedFormatUnitTests
    {
        [Fact]
        public void ParseSimpleFormats()
        {
            Format.Parsed parsed;

            parsed = Format.Parse("B");
            Verify(parsed, Format.NoPrecision, 'B');

            parsed = Format.Parse("b");
            Verify(parsed, Format.NoPrecision, 'b');

            parsed = Format.Parse("D");
            Verify(parsed, Format.NoPrecision, 'D');

            parsed = Format.Parse("d");
            Verify(parsed, Format.NoPrecision, 'd');

            parsed = Format.Parse("E");
            Verify(parsed, Format.NoPrecision, 'E');

            parsed = Format.Parse("e");
            Verify(parsed, Format.NoPrecision, 'e');

            parsed = Format.Parse("F");
            Verify(parsed, Format.NoPrecision, 'F');

            parsed = Format.Parse("f");
            Verify(parsed, Format.NoPrecision, 'f');

            parsed = Format.Parse("G");
            Verify(parsed, Format.NoPrecision, 'G');

            parsed = Format.Parse("g");
            Verify(parsed, Format.NoPrecision, 'g');

            parsed = Format.Parse("N");
            Verify(parsed, Format.NoPrecision, 'N');

            parsed = Format.Parse("n");
            Verify(parsed, Format.NoPrecision, 'n');

            parsed = Format.Parse("O");
            Verify(parsed, Format.NoPrecision, 'O');

            parsed = Format.Parse("o");
            Verify(parsed, Format.NoPrecision, 'o');

            parsed = Format.Parse("P");
            Verify(parsed, Format.NoPrecision, 'P');

            parsed = Format.Parse("p");
            Verify(parsed, Format.NoPrecision, 'p');

            parsed = Format.Parse("R");
            Verify(parsed, Format.NoPrecision, 'R');

            parsed = Format.Parse("r");
            Verify(parsed, Format.NoPrecision, 'r');

            parsed = Format.Parse("t");
            Verify(parsed, Format.NoPrecision, 't');

            parsed = Format.Parse("X");
            Verify(parsed, Format.NoPrecision, 'X');

            parsed = Format.Parse("x");
            Verify(parsed, Format.NoPrecision, 'x');
        }

        [Fact]
        public void ParsePrecisionFormats()
        {
            Format.Parsed parsed;

            parsed = Format.Parse("D1");
            Verify(parsed, 1, 'D');

            parsed = Format.Parse("d2");
            Verify(parsed, 2, 'd');

            parsed = Format.Parse("E3");
            Verify(parsed, 3, 'E');

            parsed = Format.Parse("e4");
            Verify(parsed, 4, 'e');

            parsed = Format.Parse("F5");
            Verify(parsed, 5, 'F');

            parsed = Format.Parse("f6");
            Verify(parsed, 6, 'f');

            parsed = Format.Parse("G7");
            Verify(parsed, 7, 'G');

            parsed = Format.Parse("g99");
            Verify(parsed, 99, 'g');

            parsed = Format.Parse("N98");
            Verify(parsed, 98, 'N');

            parsed = Format.Parse("n10");
            Verify(parsed, 10, 'n');

            parsed = Format.Parse("X11");
            Verify(parsed, 11, 'X');

            parsed = Format.Parse("x10");
            Verify(parsed, 10, 'x');

            
        }

        [Fact]
        public void ParseThrowsExceptionWhenParsedPrecisionExceedsMaxPrecision()
        {
            var ex = Assert.Throws<Exception>(() => Format.Parse($"x{100}"));
        }

        private static void Verify(Format.Parsed format, byte expectedPrecision, char expectedSymbol)
        {
            Assert.Equal(format.Precision, expectedPrecision);
            Assert.Equal(format.Symbol, expectedSymbol.ToString()[0]);
        }
    }
}
