// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text
{
    public static partial class PrimitiveFormatter
    {
        public static bool TryFormat(this Guid value, Span<byte> buffer, out int bytesWritten, TextFormat format = default(TextFormat), SymbolTable symbolTable = null)
        {
            symbolTable = symbolTable ?? SymbolTable.InvariantUtf8;

            if (symbolTable == SymbolTable.InvariantUtf8)
                return InvariantUtf8GuidFormatter.TryFormat(value, buffer, out bytesWritten, format);
            else if (symbolTable == SymbolTable.InvariantUtf16)
                return InvariantUtf16GuidFormatter.TryFormat(value, buffer, out bytesWritten, format);
            else
                throw new NotImplementedException();
        }
    }
}
