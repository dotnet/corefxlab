// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Globalization;
using Xunit;

namespace System.Buffers.Tests
{
    public class Reader_ParseStruct
    {
        private delegate bool ParseDelegate<T>(ref BufferReader<byte> reader, out T value, char standardFormat = '\0');

        [Fact]
        public void TryParseGuid_MultiSegment()
        {
            Guid expected = new Guid("9f21bcb9-f5c2-4b54-9b1c-9e0869bf9c16");
            ReadOnlySequence<byte> bytes = BufferFactory.CreateUtf8("9f21bcb9-f5c2-4b54-9b1c-9e0869bf9c16");
            BufferReader<byte> reader = new BufferReader<byte>(bytes);
            Assert.True(reader.TryParse(out Guid value));
            Assert.Equal(expected, value);
            Assert.Equal(36, reader.Consumed);

            // Leading zero fails
            bytes = BufferFactory.CreateUtf8("09f21bcb9-f5c2-4b54-9b1c-9e0869bf9c16");
            reader = new BufferReader<byte>(bytes);
            Assert.False(reader.TryParse(out value));
            Assert.Equal(default, value);
            Assert.Equal(0, reader.Consumed);

            // Extra digit on the end fails
            bytes = BufferFactory.CreateUtf8("9f21bcb9-f5c2-4b54-9b1c-9e0869bf9c160");
            reader = new BufferReader<byte>(bytes);
            Assert.False(reader.TryParse(out value));
            Assert.Equal(default, value);
            Assert.Equal(0, reader.Consumed);

            // Non digit OK
            bytes = BufferFactory.CreateUtf8("9f21bcb9-f5c2-4b54-9b1c-9e0869bf9c16}");
            reader = new BufferReader<byte>(bytes);
            Assert.True(reader.TryParse(out value));
            Assert.Equal(expected, value);
            Assert.Equal(36, reader.Consumed);

            // Split
            bytes = BufferFactory.CreateUtf8("9f21bcb9-f5c2-4b54-9b1c-", "9e0869bf9c16");
            reader = new BufferReader<byte>(bytes);
            Assert.True(reader.TryParse(out value));
            Assert.Equal(expected, value);
            Assert.Equal(36, reader.Consumed);

            // Extra digit on the end fails
            bytes = BufferFactory.CreateUtf8("9f21bcb9-f5c2-4b54-", "9b1c-9e0869bf9c160A");
            reader = new BufferReader<byte>(bytes);
            Assert.False(reader.TryParse(out value));
            Assert.Equal(default, value);
            Assert.Equal(0, reader.Consumed);

            // Bracket match
            bytes = BufferFactory.CreateUtf8("{9f21bcb9-f5c2-4b54-9b1c-9e0869bf9c16}");
            reader = new BufferReader<byte>(bytes);
            Assert.True(reader.TryParse(out value, 'B'));
            Assert.Equal(expected, value);
            Assert.Equal(38, reader.Consumed);

            // Bracket mismatch
            bytes = BufferFactory.CreateUtf8("{9f21bcb9-f5c2-4b54-9b1c-9e0869bf9c16{");
            reader = new BufferReader<byte>(bytes);
            Assert.False(reader.TryParse(out value, 'B'));
            Assert.Equal(default, value);
            Assert.Equal(0, reader.Consumed);

            bytes = BufferFactory.CreateUtf8("{9f21bcb9-f5c2-4b54-9b1c-9e0869bf9c16)");
            reader = new BufferReader<byte>(bytes);
            Assert.False(reader.TryParse(out value, 'B'));
            Assert.Equal(default, value);
            Assert.Equal(0, reader.Consumed);
        }

        [Theory,
            InlineData("R", "R"),       // RFC 1123
            InlineData("O", "O"),       // Round tripable (lower)
            ]
        private void TryParse_DateTime(string formatString, char standardFormat)
        {
            TryParse(ReaderExtensions.TryParse, DateTime.MaxValue.ToLocalTime(), formatString, standardFormat);
            TryParse(ReaderExtensions.TryParse, DateTime.MinValue.ToLocalTime(), formatString, standardFormat);
            TryParse(ReaderExtensions.TryParse, DateTime.MaxValue.ToUniversalTime(), formatString, standardFormat);
            TryParse(ReaderExtensions.TryParse, DateTime.MinValue.ToUniversalTime(), formatString, standardFormat);
        }

        [Theory,
            InlineData("R", "R"),       // RFC 1123
            InlineData("O", "O"),       // Round tripable (lower)
            ]
        private void TryParse_DateTimeOffset(string formatString, char standardFormat)
        {
            TryParse(ReaderExtensions.TryParse, DateTimeOffset.MaxValue, formatString, standardFormat);
            TryParse(ReaderExtensions.TryParse, DateTimeOffset.MinValue, formatString, standardFormat);
        }

        private void TryParse<T>(ParseDelegate<T> parser, T expected, string formatString, char standardFormat)
            where T : unmanaged, IEquatable<T>, IFormattable
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
