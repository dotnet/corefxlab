// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Globalization;
using Xunit;

namespace System.Buffers.Tests
{
    public class Reader_ParseFloatingPoint
    {
        private delegate bool ParseDelegate<T>(ref BufferReader<byte> reader, out T value,  char standardFormat = '\0');

        [Fact]
        public void TryParse_FloatSpecial()
        {
            TryParse_Special(ReaderExtensions.TryParse, float.PositiveInfinity, float.NegativeInfinity, float.NaN);
        }

        [Fact]
        public void TryParse_DoubleSpecial()
        {
            TryParse_Special(ReaderExtensions.TryParse, double.PositiveInfinity, double.NegativeInfinity, double.NaN);
        }

        private void TryParse_Special<T>(ParseDelegate<T> parser, T positiveInfinity, T negativeInfinity, T nan)
            where T : unmanaged, IComparable<T>
        {
            ReadOnlySequence<byte> bytes = BufferFactory.CreateUtf8("Infinity");
            BufferReader<byte> reader = new BufferReader<byte>(bytes);
            Assert.True(parser(ref reader, out T value));
            Assert.Equal(positiveInfinity, value);
            Assert.Equal(8, reader.Consumed);

            bytes = BufferFactory.CreateUtf8("I", "n", "finity", "-");
            reader = new BufferReader<byte>(bytes);
            Assert.True(parser(ref reader, out value));
            Assert.Equal(positiveInfinity, value);
            Assert.Equal(8, reader.Consumed);

            bytes = BufferFactory.CreateUtf8("-Infinity");
            reader = new BufferReader<byte>(bytes);
            Assert.True(parser(ref reader, out value));
            Assert.Equal(negativeInfinity, value);
            Assert.Equal(9, reader.Consumed);

            bytes = BufferFactory.CreateUtf8("-", "Infinit", "y");
            reader = new BufferReader<byte>(bytes);
            Assert.True(parser(ref reader, out value));
            Assert.Equal(negativeInfinity, value);
            Assert.Equal(9, reader.Consumed);

            bytes = BufferFactory.CreateUtf8("NaN");
            reader = new BufferReader<byte>(bytes);
            Assert.True(parser(ref reader, out value));
            Assert.Equal(nan, value);
            Assert.Equal(3, reader.Consumed);
        }

        [Theory,
            InlineData("F", "f"),       // Fixed point
            InlineData("F10", "f"),     // Fixed point (10 decimal digits)
            InlineData("F20", "f"),     // Fixed point (20 decimal digits)
            InlineData("E", "e"),       // Exponential
            InlineData("E12", "e"),     // Exponential (12 decimal digits)
            InlineData("E24", "e"),     // Exponential (24 decimal digits)
            InlineData("e", "e"),       // Exponential (lower case)

            // Fixed point with a bunch of leading and trailing zeroes
            InlineData("'00000000000000000000'000000000000000000.00000000000000000000'00000000000'", 'f'),
            // Exponential with a bunch of leading zeroes
            InlineData("'00000000000000000000'0000000.0000000000000E0", 'e')
            ]
        private void TryParse_Float(string formatString, char standardFormat)
        {
            TryParse(ReaderExtensions.TryParse, float.MaxValue, formatString, standardFormat);
            TryParse(ReaderExtensions.TryParse, float.MinValue, formatString, standardFormat);
            TryParse(ReaderExtensions.TryParse, (float)Math.PI, formatString, standardFormat);
        }

        [Theory,
            // The Utf8Parser only handles up to 50 significant digits. As double goes to 10^308 fixed point
            // isn't possible for most of the double range.
            //InlineData("F", "f"),       // Fixed point
            //InlineData("F10", "f"),     // Fixed point (10 decimal digits)
            //InlineData("F20", "f"),     // Fixed point (20 decimal digits)
            InlineData("E", "e"),       // Exponential
            InlineData("E12", "e"),     // Exponential (12 decimal digits)
            InlineData("E24", "e"),     // Exponential (24 decimal digits)
            InlineData("e", "e"),       // Exponential (lower case)

            // Exponential with a bunch of leading zeroes
            InlineData("'00000000000000000000'#########.#####E0", 'e')
            ]
        private void TryParse_DoubleMinMax(string formatString, char standardFormat)
        {
            TryParse(ReaderExtensions.TryParse, double.MaxValue, formatString, standardFormat);
            TryParse(ReaderExtensions.TryParse, double.MinValue, formatString, standardFormat);
        }

        [Theory,
            InlineData("F", "f"),       // Fixed point
            InlineData("F10", "f"),     // Fixed point (10 decimal digits)
            InlineData("F20", "f"),     // Fixed point (20 decimal digits)
            InlineData("E", "e"),       // Exponential
            InlineData("E12", "e"),     // Exponential (12 decimal digits)
            InlineData("E24", "e"),     // Exponential (24 decimal digits)
            InlineData("e", "e"),       // Exponential (lower case)

            // Fixed point with a bunch of leading and trailing zeroes
            InlineData("'00000000000000000000'000000000000000000.00000000000000000000'00000000000'", 'f'),
            // Exponential with a bunch of leading zeroes
            InlineData("'00000000000000000000'0000000.0000000000000E0", 'e')
            ]
        private void TryParse_DoublePi(string formatString, char standardFormat)
        {
            TryParse(ReaderExtensions.TryParse, Math.PI, formatString, standardFormat);
        }

        [Theory,
            InlineData("F", "f"),       // Fixed point
            InlineData("F10", "f"),     // Fixed point (10 decimal digits)
            InlineData("F20", "f"),     // Fixed point (20 decimal digits)
            InlineData("E", "e"),       // Exponential
            InlineData("E12", "e"),     // Exponential (12 decimal digits)
            InlineData("E24", "e"),     // Exponential (24 decimal digits)
            InlineData("e", "e"),       // Exponential (lower case)

            // Fixed point with a bunch of leading and trailing zeroes
            InlineData("'00000000000000000000'000000000000000000.00000000000000000000'00000000000'", 'f')
            ]
        private void TryParse_Decimal(string formatString, char standardFormat)
        {
            TryParse(ReaderExtensions.TryParse, decimal.MaxValue, formatString, standardFormat);
            TryParse(ReaderExtensions.TryParse, decimal.MinValue, formatString, standardFormat);
            TryParse(ReaderExtensions.TryParse, (decimal)Math.PI, formatString, standardFormat);
        }

        private void TryParse<T>(ParseDelegate<T> parser, T expected, string formatString, char standardFormat)
            where T : unmanaged, IEquatable<T>, IFormattable, IConvertible
        {
            // Note that there is no support in Utf8Parser for localized separators
            string text = expected.ToString(formatString, CultureInfo.InvariantCulture);
            ReadOnlySequence<byte> bytes = BufferFactory.CreateUtf8(text);
            BufferReader<byte> reader = new BufferReader<byte>(bytes);
            Assert.True(parser(ref reader, out T value, standardFormat));
            Assert.Equal(text, value.ToString(formatString, CultureInfo.InvariantCulture));
        }
    }
}
