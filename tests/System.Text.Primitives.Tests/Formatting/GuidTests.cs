// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using Xunit;

namespace System.Text.Primitives.Tests
{
    public class GuidTests
    {
        const int NumberOfRandomSamples = 1000;

        static readonly SymbolTable[] SymbolTables = new SymbolTable[]
        {
            SymbolTable.InvariantUtf8,
            SymbolTable.InvariantUtf16,
        };

        [Theory]
        [InlineData('N')]
        [InlineData('D')]
        [InlineData('B')]
        [InlineData('P')]
        public void SpecificGuidTests(char format)
        {
            foreach (var symbolTable in SymbolTables)
            {
                TestGuidFormat(Guid.Empty, format, symbolTable);
                TestGuidFormat(Guid.Parse("{00000000-0000-0000-0000-000000000001}"), format, symbolTable);
                TestGuidFormat(Guid.Parse("{10000000-0000-0000-0000-000000000000}"), format, symbolTable);
                TestGuidFormat(Guid.Parse("{11111111-1111-1111-1111-111111111111}"), format, symbolTable);
                TestGuidFormat(Guid.Parse("{99999999-9999-9999-9999-999999999999}"), format, symbolTable);
                TestGuidFormat(Guid.Parse("{AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA}"), format, symbolTable);
                TestGuidFormat(Guid.Parse("{FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF}"), format, symbolTable);
                TestGuidFormat(Guid.Parse("{aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa}"), format, symbolTable);
                TestGuidFormat(Guid.Parse("{ffffffff-ffff-ffff-ffff-ffffffffffff}"), format, symbolTable);
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
                foreach (var symbolTable in SymbolTables)
                {
                    TestGuidFormat(guid, format, symbolTable);
                }
            }
        }

        static void TestGuidFormat(Guid guid, char format, SymbolTable symbolTable)
        {
            var expected = guid.ToString(format.ToString());

            var span = new Span<byte>(new byte[128]);
            Assert.True(CustomFormatter.TryFormat(guid, span, out int written, format, symbolTable));

            var actual = TestHelper.SpanToString(span.Slice(0, written), symbolTable);
            Assert.Equal(expected, actual);
        }
    }
}
