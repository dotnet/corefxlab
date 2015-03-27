// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Formatting;
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
            Verify(parsed, Format.NoPrecision, Format.Symbol.B);

            parsed = Format.Parse("b");
            Verify(parsed, Format.NoPrecision, Format.Symbol.B);

            parsed = Format.Parse("D");
            Verify(parsed, Format.NoPrecision, Format.Symbol.D);

            parsed = Format.Parse("d");
            Verify(parsed, Format.NoPrecision, Format.Symbol.D);

            parsed = Format.Parse("E");
            Verify(parsed, Format.NoPrecision, Format.Symbol.E);

            parsed = Format.Parse("e");
            Verify(parsed, Format.NoPrecision, Format.Symbol.E);

            parsed = Format.Parse("F");
            Verify(parsed, Format.NoPrecision, Format.Symbol.F);

            parsed = Format.Parse("f");
            Verify(parsed, Format.NoPrecision, Format.Symbol.F);

            parsed = Format.Parse("G");
            Verify(parsed, Format.NoPrecision, Format.Symbol.G);

            parsed = Format.Parse("g");
            Verify(parsed, Format.NoPrecision, Format.Symbol.G);

            parsed = Format.Parse("N");
            Verify(parsed, Format.NoPrecision, Format.Symbol.N);

            parsed = Format.Parse("n");
            Verify(parsed, Format.NoPrecision, Format.Symbol.N);

            parsed = Format.Parse("O");
            Verify(parsed, Format.NoPrecision, Format.Symbol.O);

            parsed = Format.Parse("o");
            Verify(parsed, Format.NoPrecision, Format.Symbol.O);

            parsed = Format.Parse("P");
            Verify(parsed, Format.NoPrecision, Format.Symbol.P);

            parsed = Format.Parse("p");
            Verify(parsed, Format.NoPrecision, Format.Symbol.P);

            parsed = Format.Parse("R");
            Verify(parsed, Format.NoPrecision, Format.Symbol.R);

            parsed = Format.Parse("r");
            Verify(parsed, Format.NoPrecision, Format.Symbol.R);

            parsed = Format.Parse("X");
            Verify(parsed, Format.NoPrecision, Format.Symbol.X);

            parsed = Format.Parse("x");
            Verify(parsed, Format.NoPrecision, Format.Symbol.x);
        }

        [Fact]
        public void ParsePrecisionFormats()
        {
            Format.Parsed parsed;

            parsed = Format.Parse("D1");
            Verify(parsed, 1, Format.Symbol.D);

            parsed = Format.Parse("d2");
            Verify(parsed, 2, Format.Symbol.D);

            parsed = Format.Parse("E3");
            Verify(parsed, 3, Format.Symbol.E);

            parsed = Format.Parse("e4");
            Verify(parsed, 4, Format.Symbol.E);

            parsed = Format.Parse("F5");
            Verify(parsed, 5, Format.Symbol.F);

            parsed = Format.Parse("f6");
            Verify(parsed, 6, Format.Symbol.F);

            parsed = Format.Parse("G7");
            Verify(parsed, 7, Format.Symbol.G);

            parsed = Format.Parse("g99");
            Verify(parsed, 99, Format.Symbol.G);

            parsed = Format.Parse("N98");
            Verify(parsed, 98, Format.Symbol.N);

            parsed = Format.Parse("n10");
            Verify(parsed, 10, Format.Symbol.N);

            parsed = Format.Parse("X11");
            Verify(parsed, 11, Format.Symbol.X);

            parsed = Format.Parse("x10");
            Verify(parsed, 10, Format.Symbol.x);
        }

        private static void Verify(Format.Parsed format, byte expectedPrecision, Format.Symbol expectedSymbol)
        {
            Assert.Equal(format.Precision, expectedPrecision);
            Assert.Equal(format.Symbol, expectedSymbol);
        }
    }
}
