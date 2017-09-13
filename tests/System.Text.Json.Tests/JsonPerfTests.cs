// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers.Text;
using Xunit;

namespace System.Text.Json.Tests
{
    public partial class JsonPerfTests
    {
        const int BufferSize = 1024 + (ExtraArraySize * 64);

        public enum EncoderTarget
        {
            InvariantUtf8,
            InvariantUtf16,
            SlowUtf8,
            SlowUtf16
        }

        static SymbolTable GetTargetEncoder(EncoderTarget target)
        {
            switch (target)
            {
                case EncoderTarget.InvariantUtf8:
                    return SymbolTable.InvariantUtf8;
                case EncoderTarget.InvariantUtf16:
                    return SymbolTable.InvariantUtf16;
                case EncoderTarget.SlowUtf8:
                    return new FakeUtf8SymbolTable();
                case EncoderTarget.SlowUtf16:
                    return new FakeUtf16SymbolTable();
                default:
                    Assert.True(false, "Invalid encoder targetted in test");
                    return null;
            }
        }

        // Copy of UTF-8 symbol table for creating a "non-invariant" version of it
        // to force the slow path.
        private static readonly byte[][] Utf8DigitsAndSymbols = new byte[][]
        {
            new byte[] { 48, },
            new byte[] { 49, },
            new byte[] { 50, },
            new byte[] { 51, },
            new byte[] { 52, },
            new byte[] { 53, },
            new byte[] { 54, },
            new byte[] { 55, },
            new byte[] { 56, },
            new byte[] { 57, }, // digit 9
            new byte[] { 46, }, // decimal separator
            new byte[] { 44, }, // group separator
            new byte[] { 73, 110, 102, 105, 110, 105, 116, 121, },
            new byte[] { 45, }, // minus sign
            new byte[] { 43, }, // plus sign
            new byte[] { 78, 97, 78, }, // NaN
            new byte[] { 69, }, // E
            new byte[] { 101, }, // e
        };

        class FakeUtf8SymbolTable : SymbolTable
        {
            public FakeUtf8SymbolTable() : base(Utf8DigitsAndSymbols) {}

            public override bool TryEncode(byte utf8, Span<byte> destination, out int bytesWritten)
                => SymbolTable.InvariantUtf8.TryEncode(utf8, destination, out bytesWritten);

            public override bool TryEncode(ReadOnlySpan<byte> utf8, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
                => SymbolTable.InvariantUtf8.TryEncode(utf8, destination, out bytesConsumed, out bytesWritten);

            public override bool TryParse(ReadOnlySpan<byte> source, out byte utf8, out int bytesConsumed)
                => SymbolTable.InvariantUtf8.TryParse(source, out utf8, out bytesConsumed);

            public override bool TryParse(ReadOnlySpan<byte> source, Span<byte> utf8, out int bytesConsumed, out int bytesWritten)
                => SymbolTable.InvariantUtf8.TryParse(source, utf8, out bytesConsumed, out bytesWritten);
        }

        // Copy of UTF-16 symbol table for creating a "non-invariant" version of it
        // to force the slow path.
        private static readonly byte[][] Utf16DigitsAndSymbols = new byte[][]
        {
            new byte[] { 48, 0, }, // digit 0
            new byte[] { 49, 0, },
            new byte[] { 50, 0, },
            new byte[] { 51, 0, },
            new byte[] { 52, 0, },
            new byte[] { 53, 0, },
            new byte[] { 54, 0, },
            new byte[] { 55, 0, },
            new byte[] { 56, 0, },
            new byte[] { 57, 0, }, // digit 9
            new byte[] { 46, 0, }, // decimal separator
            new byte[] { 44, 0, }, // group separator
            new byte[] { 73, 0, 110, 0, 102, 0, 105, 0, 110, 0, 105, 0, 116, 0, 121, 0, }, // Infinity
            new byte[] { 45, 0, }, // minus sign
            new byte[] { 43, 0, }, // plus sign
            new byte[] { 78, 0, 97, 0, 78, 0, }, // NaN
            new byte[] { 69, 0, }, // E
            new byte[] { 101, 0, }, // e
        };

        class FakeUtf16SymbolTable : SymbolTable
        {
            public FakeUtf16SymbolTable() : base(Utf16DigitsAndSymbols) {}

            public override bool TryEncode(byte utf8, Span<byte> destination, out int bytesWritten)
                => SymbolTable.InvariantUtf16.TryEncode(utf8, destination, out bytesWritten);

            public override bool TryEncode(ReadOnlySpan<byte> utf8, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
                => SymbolTable.InvariantUtf16.TryEncode(utf8, destination, out bytesConsumed, out bytesWritten);

            public override bool TryParse(ReadOnlySpan<byte> source, out byte utf8, out int bytesConsumed)
                => SymbolTable.InvariantUtf16.TryParse(source, out utf8, out bytesConsumed);

            public override bool TryParse(ReadOnlySpan<byte> source, Span<byte> utf8, out int bytesConsumed, out int bytesWritten)
                => SymbolTable.InvariantUtf16.TryParse(source, utf8, out bytesConsumed, out bytesWritten);
        }
    }
}
