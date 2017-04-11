// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using Xunit;

namespace System.Text.Primitives.Tests
{
    public class UuidTests
    {
        const int NumberOfRandomSamples = 1000;

        static readonly TextEncoder[] Encoders = new TextEncoder[]
        {
            TextEncoder.Utf8,
            TextEncoder.Utf16,
        };

        [Theory]
        [InlineData('N')]
        [InlineData('D')]
        [InlineData('B')]
        [InlineData('P')]
        public void SpecificGuidTests(char format)
        {
            foreach (var encoder in Encoders)
            {
                TestUuidFormat(Guid.Empty, format, encoder);
                TestUuidFormat(Guid.Parse("{00000000-0000-0000-0000-000000000001}"), format, encoder);
                TestUuidFormat(Guid.Parse("{10000000-0000-0000-0000-000000000000}"), format, encoder);
                TestUuidFormat(Guid.Parse("{11111111-1111-1111-1111-111111111111}"), format, encoder);
                TestUuidFormat(Guid.Parse("{99999999-9999-9999-9999-999999999999}"), format, encoder);
                TestUuidFormat(Guid.Parse("{AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA}"), format, encoder);
                TestUuidFormat(Guid.Parse("{FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF}"), format, encoder);
                TestUuidFormat(Guid.Parse("{aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa}"), format, encoder);
                TestUuidFormat(Guid.Parse("{ffffffff-ffff-ffff-ffff-ffffffffffff}"), format, encoder);
            }
        }

        [Theory]
        [InlineData('N')]
        [InlineData('D')]
        [InlineData('B')]
        [InlineData('P')]
        public void RandomGuidTests(char format)
        {
            for (var i = 0; i < NumberOfRandomSamples; i++)
            {
                Guid guid = Guid.NewGuid();
                foreach (var encoder in Encoders)
                {
                    TestUuidFormat(guid, format, encoder);
                }
            }
        }

        static void TestUuidFormat(Guid guid, char format, TextEncoder encoder)
        {
            var expected = guid.ToString(format.ToString(), CultureInfo.InvariantCulture);

            var span = new Span<byte>(new byte[128]);
            Assert.True(PrimitiveFormatter.TryFormat(guid, span, out int written, format, encoder));

            var actual = TestHelper.SpanToString(span.Slice(0, written), encoder);
            Assert.Equal(expected, actual);
        }
    }
}
