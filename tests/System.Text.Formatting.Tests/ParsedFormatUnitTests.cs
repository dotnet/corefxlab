// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using System.Buffers;
using Xunit;

namespace System.Text.Formatting.Tests
{
    public class StandardFormatUnitTests
    {
        [Fact]
        public void ParseSimpleFormats()
        {
            StandardFormat parsed;

            parsed = StandardFormat.Parse("B");
            Verify(parsed, StandardFormat.NoPrecision, 'B');

            parsed = StandardFormat.Parse("b");
            Verify(parsed, StandardFormat.NoPrecision, 'b');

            parsed = StandardFormat.Parse("D");
            Verify(parsed, StandardFormat.NoPrecision, 'D');

            parsed = StandardFormat.Parse("d");
            Verify(parsed, StandardFormat.NoPrecision, 'd');

            parsed = StandardFormat.Parse("E");
            Verify(parsed, StandardFormat.NoPrecision, 'E');

            parsed = StandardFormat.Parse("e");
            Verify(parsed, StandardFormat.NoPrecision, 'e');

            parsed = StandardFormat.Parse("F");
            Verify(parsed, StandardFormat.NoPrecision, 'F');

            parsed = StandardFormat.Parse("f");
            Verify(parsed, StandardFormat.NoPrecision, 'f');

            parsed = StandardFormat.Parse("G");
            Verify(parsed, StandardFormat.NoPrecision, 'G');

            parsed = StandardFormat.Parse("g");
            Verify(parsed, StandardFormat.NoPrecision, 'g');

            parsed = StandardFormat.Parse("N");
            Verify(parsed, StandardFormat.NoPrecision, 'N');

            parsed = StandardFormat.Parse("n");
            Verify(parsed, StandardFormat.NoPrecision, 'n');

            parsed = StandardFormat.Parse("O");
            Verify(parsed, StandardFormat.NoPrecision, 'O');

            parsed = StandardFormat.Parse("o");
            Verify(parsed, StandardFormat.NoPrecision, 'o');

            parsed = StandardFormat.Parse("P");
            Verify(parsed, StandardFormat.NoPrecision, 'P');

            parsed = StandardFormat.Parse("p");
            Verify(parsed, StandardFormat.NoPrecision, 'p');

            parsed = StandardFormat.Parse("R");
            Verify(parsed, StandardFormat.NoPrecision, 'R');

            parsed = StandardFormat.Parse("r");
            Verify(parsed, StandardFormat.NoPrecision, 'r');

            parsed = StandardFormat.Parse("t");
            Verify(parsed, StandardFormat.NoPrecision, 't');

            parsed = StandardFormat.Parse("X");
            Verify(parsed, StandardFormat.NoPrecision, 'X');

            parsed = StandardFormat.Parse("x");
            Verify(parsed, StandardFormat.NoPrecision, 'x');
        }

        [Fact]
        public void ParsePrecisionFormats()
        {
            StandardFormat parsed;

            parsed = StandardFormat.Parse("D1");
            Verify(parsed, 1, 'D');

            parsed = StandardFormat.Parse("d2");
            Verify(parsed, 2, 'd');

            parsed = StandardFormat.Parse("E3");
            Verify(parsed, 3, 'E');

            parsed = StandardFormat.Parse("e4");
            Verify(parsed, 4, 'e');

            parsed = StandardFormat.Parse("F5");
            Verify(parsed, 5, 'F');

            parsed = StandardFormat.Parse("f6");
            Verify(parsed, 6, 'f');

            parsed = StandardFormat.Parse("G7");
            Verify(parsed, 7, 'G');

            parsed = StandardFormat.Parse("g99");
            Verify(parsed, 99, 'g');

            parsed = StandardFormat.Parse("N98");
            Verify(parsed, 98, 'N');

            parsed = StandardFormat.Parse("n10");
            Verify(parsed, 10, 'n');

            parsed = StandardFormat.Parse("X11");
            Verify(parsed, 11, 'X');

            parsed = StandardFormat.Parse("x10");
            Verify(parsed, 10, 'x');

            
        }

        [Fact]
        public void ParseThrowsExceptionWhenParsedPrecisionExceedsMaxPrecision()
        {
            var ex = Assert.Throws<FormatException>(() => StandardFormat.Parse($"x{100}"));
        }

        private static void Verify(StandardFormat format, byte expectedPrecision, char expectedSymbol)
        {
            Assert.Equal(format.Precision, expectedPrecision);
            Assert.Equal(format.Symbol, expectedSymbol.ToString()[0]);
        }
    }
}
