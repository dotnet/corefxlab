// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;

namespace System.Text.Formatting.Tests
{
    public class CustomUtf16SymbolTable : SymbolTable
    {
        public CustomUtf16SymbolTable(byte[][] symbols) : base(symbols) {}

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
