// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Globalization;
using System.Text;
using Xunit;

namespace System.Buffers.Tests
{
    public class Reader_ParseInteger
    {
        private static readonly byte[] s_array;
        private static readonly byte[] s_signedArray;
        private static readonly ReadOnlySequence<byte> s_ros;
        private static readonly ReadOnlySequence<byte> s_rosSplit;
        private static readonly ReadOnlySequence<byte> s_rosSigned;
        private static readonly ReadOnlySequence<byte> s_rosSignedSplit;

        private delegate bool ParseDelegate<T>(ref BufferReader<byte> reader, out T value, char standardFormat = '\0');

        static Reader_ParseInteger()
        {
            s_array = new byte[10_000];
            BufferUtilities.FillIntegerUtf8Array(s_array, 0, 123);
            s_ros = new ReadOnlySequence<byte>(s_array);
            s_rosSplit = BufferUtilities.CreateSplitBuffer(s_array, 1, 11);

            s_signedArray = new byte[10_000];
            BufferUtilities.FillIntegerUtf8Array(s_signedArray, -119, 119);
            s_rosSigned = new ReadOnlySequence<byte>(s_signedArray);
            s_rosSignedSplit = BufferUtilities.CreateSplitBuffer(s_signedArray, 1, 11);
        }

        [Fact]
        public void TryParse_ByteBasic()
        {
            TryParse_Sequence<byte>(ReaderExtensions.TryParse, s_ros, s_rosSplit, 0, 123);
            TryParse_Sequence<sbyte>(ReaderExtensions.TryParse, s_ros, s_rosSplit, 0, 123);
            TryParse_Sequence<sbyte>(ReaderExtensions.TryParse, s_rosSigned, s_rosSignedSplit, -119, 119);
        }

        [Fact]
        public void TryParse_ShortBasic()
        {
            TryParse_Sequence<ushort>(ReaderExtensions.TryParse, s_ros, s_rosSplit, 0, 123);
            TryParse_Sequence<short>(ReaderExtensions.TryParse, s_ros, s_rosSplit, 0, 123);
            TryParse_Sequence<short>(ReaderExtensions.TryParse, s_rosSigned, s_rosSignedSplit, -119, 119);
        }

        [Fact]
        public void TryParse_IntBasic()
        {
            TryParse_Sequence<uint>(ReaderExtensions.TryParse, s_ros, s_rosSplit, 0, 123);
            TryParse_Sequence<int>(ReaderExtensions.TryParse, s_ros, s_rosSplit, 0, 123);
            TryParse_Sequence<int>(ReaderExtensions.TryParse, s_rosSigned, s_rosSignedSplit, -119, 119);
        }

        [Fact]
        public void TryParse_LongBasic()
        {
            TryParse_Sequence<ulong>(ReaderExtensions.TryParse, s_ros, s_rosSplit, 0, 123);
            TryParse_Sequence<long>(ReaderExtensions.TryParse, s_ros, s_rosSplit, 0, 123);
            TryParse_Sequence<long>(ReaderExtensions.TryParse, s_rosSigned, s_rosSignedSplit, -119, 119);
        }

        private void TryParse_Sequence<T>(ParseDelegate<T> parser, ReadOnlySequence<byte> sequence, ReadOnlySequence<byte> splitSequence, T min, T max)
            where T: unmanaged, IComparable<T>
        {
            var reader = new BufferReader<byte>(sequence);

            int count = 0;
            while (parser(ref reader, out T value))
            {
                // advance past the delimiter
                reader.Advance(1);
                Assert.True(value.CompareTo(max) <= 0);
                Assert.True(value.CompareTo(min) >= 0);
                count++;
            }

            reader = new BufferReader<byte>(splitSequence);
            while (parser(ref reader, out T value))
            {
                // advance past the delimiter
                reader.Advance(1);
                Assert.True(value.CompareTo(max) <= 0);
                Assert.True(value.CompareTo(min) >= 0);
                count--;
            }

            // Should find the same number of ints regardless of split position
            Assert.Equal(0, count);
        }

        [Fact]
        public void TryParseInt_MultiSegment()
        {
            ReadOnlySequence<byte> bytes = BufferFactory.CreateUtf8("-123", "45");
            BufferReader<byte> reader = new BufferReader<byte>(bytes);
            Assert.True(reader.TryParse(out int value));
            Assert.Equal(-12345, value);
            Assert.Equal(6, reader.Consumed);

            // With group separators
            bytes = BufferFactory.CreateUtf8("-1,2,3", "4,5.0000000000NewData");
            reader = new BufferReader<byte>(bytes);
            Assert.True(reader.TryParse(out value));
            Assert.Equal(-1, value);
            Assert.Equal(2, reader.Consumed);

            reader = new BufferReader<byte>(bytes);
            Assert.True(reader.TryParse(out value, 'N'));
            Assert.Equal(-12345, value);
            Assert.Equal(20, reader.Consumed);
            Assert.True(reader.IsNext((byte)'N'));

            reader = new BufferReader<byte>(bytes);
            Assert.False(reader.TryParse(out value, 'X'));
            Assert.Equal(0, reader.Consumed);
            reader.Advance(1);
            Assert.True(reader.TryParse(out value));
            Assert.Equal(1, value);
            Assert.Equal(2, reader.Consumed);

            bytes = BufferFactory.CreateUtf8("FEE", "D");
            reader = new BufferReader<byte>(bytes);
            Assert.True(reader.TryParse(out value, 'X'));
            Assert.Equal(0xFEED, value);
            Assert.Equal(4, reader.Consumed);

            // Overflow
            bytes = BufferFactory.CreateUtf8("FE", "ED", "BEEFBEE");
            reader = new BufferReader<byte>(bytes);
            Assert.False(reader.TryParse(out value, 'X'));
            Assert.Equal(0, reader.Consumed);

            reader.Advance(3);
            Assert.True(reader.TryParse(out value, 'X'));
            Assert.Equal(unchecked((int)0xDBEEFBEE), value);
            Assert.Equal(11, reader.Consumed);
        }

        [Theory,
            InlineData("D", 'd'),       // No group separators
            InlineData("N", 'n'),       // Group separators
            InlineData("X", 'x'),       // Hex
            InlineData("x", 'x'),       // Hex (lower case)
            InlineData("F1", "d"),      // No group separators with .0
            InlineData("N1", "n")       // Group separators with .0
            ]
        public void TryParse_ByteLeadingZeroes(string formatString, char standardFormat)
        {
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, byte.MaxValue, formatString, standardFormat);
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, byte.MaxValue / 2, formatString, standardFormat);
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, byte.MinValue, formatString, standardFormat);
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, sbyte.MaxValue, formatString, standardFormat);
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, sbyte.MaxValue / 2, formatString, standardFormat);
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, sbyte.MinValue, formatString, standardFormat);
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, sbyte.MinValue / 2, formatString, standardFormat);
        }

        [Theory,
            InlineData("D", 'd'),       // No group separators
            InlineData("N", 'n'),       // Group separators
            InlineData("X", 'x'),       // Hex
            InlineData("x", 'x'),       // Hex (lower case)
            InlineData("F1", "d"),      // No group separators with .0
            InlineData("N1", "n")       // Group separators with .0
            ]
        public void TryParse_ShortLeadingZeroes(string formatString, char standardFormat)
        {
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, ushort.MaxValue, formatString, standardFormat);
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, ushort.MaxValue / 2, formatString, standardFormat);
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, ushort.MinValue, formatString, standardFormat);
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, short.MaxValue, formatString, standardFormat);
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, short.MaxValue / 2, formatString, standardFormat);
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, short.MinValue, formatString, standardFormat);
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, short.MinValue / 2, formatString, standardFormat);
        }

        [Theory,
            InlineData("D", 'd'),       // No group separators
            InlineData("N", 'n'),       // Group separators
            InlineData("X", 'x'),       // Hex
            InlineData("x", 'x'),       // Hex (lower case)
            InlineData("F1", "d"),      // No group separators with .0
            InlineData("N1", "n")       // Group separators with .0
           ]
        public void TryParse_IntLeadingZeroes(string formatString, char standardFormat)
        {
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, uint.MaxValue, formatString, standardFormat);
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, uint.MaxValue / 2, formatString, standardFormat);
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, uint.MinValue, formatString, standardFormat);
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, int.MaxValue, formatString, standardFormat);
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, int.MaxValue / 2, formatString, standardFormat);
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, int.MinValue, formatString, standardFormat);
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, int.MinValue / 2, formatString, standardFormat);
        }

        [Theory,
            InlineData("D", 'd'),       // No group separators
            InlineData("N", 'n'),       // Group separators
            InlineData("X", 'x'),       // Hex
            InlineData("x", 'x'),       // Hex (lower case)
            InlineData("F1", "d"),      // No group separators with .0
            InlineData("N1", "n")       // Group separators with .0
            ]
        public void TryParse_LongLeadingZeroes(string formatString, char standardFormat)
        {
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, ulong.MaxValue, formatString, standardFormat);
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, ulong.MaxValue / 2, formatString, standardFormat);
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, ulong.MinValue, formatString, standardFormat);
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, long.MaxValue, formatString, standardFormat);
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, long.MaxValue / 2, formatString, standardFormat);
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, long.MinValue, formatString, standardFormat);
            TryParse_LeadingZeroes(ReaderExtensions.TryParse, long.MinValue / 2, formatString, standardFormat);
        }

        private void TryParse_LeadingZeroes<T>(ParseDelegate<T> parser, T expected, string formatString, char standardFormat)
            where T : unmanaged, IComparable<T>, IFormattable
        {
            // Note that there is no support in Utf8Parser for localized separators
            string text = expected.ToString(formatString, CultureInfo.InvariantCulture);
            bool negative = text.StartsWith('-');
            text = text.TrimStart('-');

            ReadOnlySequence<byte> bytes = BufferFactory.CreateUtf8(
                "-000000000",
                "0000000000",
                "0000000000",
                "0000000000",
                "0000000000",
                "0000000000",
                "0000000000",
                "0000000000",
                "0000000000",
                "0000000000",
                "0000000000",
                "0000000000",
                "0000000000",
                "0000000000",
                negative
                    ? "-000000000"
                    : "0000000000",
                "0000000000",
                text.Substring(0, 1),
                text.Substring(1)
            );

            BufferReader<byte> reader = new BufferReader<byte>(bytes);

            // Too many bytes
            Assert.False(parser(ref reader, out T value, standardFormat));
            reader.Advance(140);
            Assert.Equal(140, reader.Consumed);
            Assert.True(parser(ref reader, out value, standardFormat));
            Assert.Equal(expected, value);
            Assert.True(reader.Consumed > 20, "should have consumed all of the leading zeroes");

            long priorConsumed = reader.Consumed;

            // Overflow
            bytes = BufferFactory.CreateUtf8(
                negative ? "-0000000000" : "0000000000",
                "0000000000",
                "0000000000",
                "0000000000",
                "0000000000",
                "0000000000",
                "1234567891",
                "2345678901",
                "1234567891"
            );

            Assert.False(parser(ref reader, out value, standardFormat));
            Assert.Equal(priorConsumed, reader.Consumed);
        }

        [Fact]
        public void TryParseInt_SingleNonDigit()
        {
            // Testing single segments that aren't valid on their own, but are with additional segments

            ReadOnlySequence<byte> bytes = BufferFactory.CreateUtf8("-", "123");
            var reader = new BufferReader<byte>(bytes);
            Assert.True(reader.TryParse(out int value));
            Assert.Equal(-123, value);
            Assert.Equal(4, reader.Consumed);
            Assert.True(reader.End);

            bytes = BufferFactory.CreateUtf8("+", "123");
            reader = new BufferReader<byte>(bytes);
            Assert.True(reader.TryParse(out value));
            Assert.Equal(123, value);
            Assert.Equal(4, reader.Consumed);
            Assert.True(reader.End);

            bytes = BufferFactory.CreateUtf8(".", "0");
            reader = new BufferReader<byte>(bytes);
            Assert.False(reader.TryParse(out value, 'd'));
            Assert.Equal(0, reader.Consumed);
            Assert.True(reader.TryParse(out value, 'n'));
            Assert.Equal(2, reader.Consumed);
            Assert.Equal(0, value);
            Assert.True(reader.End);
        }
    }
}
