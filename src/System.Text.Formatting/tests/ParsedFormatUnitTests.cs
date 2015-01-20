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

            parsed = Format.Parsed.Parse("D");
            Verify(parsed, Format.NoPrecision, Format.Symbol.D);

            parsed = Format.Parsed.Parse("d");
            Verify(parsed, Format.NoPrecision, Format.Symbol.D);

            parsed = Format.Parsed.Parse("E");
            Verify(parsed, Format.NoPrecision, Format.Symbol.E);

            parsed = Format.Parsed.Parse("e");
            Verify(parsed, Format.NoPrecision, Format.Symbol.E);

            parsed = Format.Parsed.Parse("F");
            Verify(parsed, Format.NoPrecision, Format.Symbol.F);

            parsed = Format.Parsed.Parse("f");
            Verify(parsed, Format.NoPrecision, Format.Symbol.F);

            parsed = Format.Parsed.Parse("G");
            Verify(parsed, Format.NoPrecision, Format.Symbol.G);

            parsed = Format.Parsed.Parse("g");
            Verify(parsed, Format.NoPrecision, Format.Symbol.G);

            parsed = Format.Parsed.Parse("N");
            Verify(parsed, Format.NoPrecision, Format.Symbol.N);

            parsed = Format.Parsed.Parse("n");
            Verify(parsed, Format.NoPrecision, Format.Symbol.N);

            parsed = Format.Parsed.Parse("X");
            Verify(parsed, Format.NoPrecision, Format.Symbol.X);

            parsed = Format.Parsed.Parse("x");
            Verify(parsed, Format.NoPrecision, Format.Symbol.XLowercase);
        }

        [Fact]
        public void ParsePrecisionFormats()
        {
            Format.Parsed parsed;

            parsed = Format.Parsed.Parse("D1");
            Verify(parsed, 1, Format.Symbol.D);

            parsed = Format.Parsed.Parse("d2");
            Verify(parsed, 2, Format.Symbol.D);

            parsed = Format.Parsed.Parse("E3");
            Verify(parsed, 3, Format.Symbol.E);

            parsed = Format.Parsed.Parse("e4");
            Verify(parsed, 4, Format.Symbol.E);

            parsed = Format.Parsed.Parse("F5");
            Verify(parsed, 5, Format.Symbol.F);

            parsed = Format.Parsed.Parse("f6");
            Verify(parsed, 6, Format.Symbol.F);

            parsed = Format.Parsed.Parse("G7");
            Verify(parsed, 7, Format.Symbol.G);

            parsed = Format.Parsed.Parse("g99");
            Verify(parsed, 99, Format.Symbol.G);

            parsed = Format.Parsed.Parse("N98");
            Verify(parsed, 98, Format.Symbol.N);

            parsed = Format.Parsed.Parse("n10");
            Verify(parsed, 10, Format.Symbol.N);

            parsed = Format.Parsed.Parse("X11");
            Verify(parsed, 11, Format.Symbol.X);

            parsed = Format.Parsed.Parse("x10");
            Verify(parsed, 10, Format.Symbol.XLowercase);
        }

        private static void Verify(Format.Parsed format, byte expectedPrecision, Format.Symbol expectedSymbol)
        {
            Assert.Equal(format.Precision, expectedPrecision);
            Assert.Equal(format.Symbol, expectedSymbol);
        }
    }
}
