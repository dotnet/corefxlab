// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

namespace System.Buffers.Text
{
    public static partial class CustomParser
    {
        public static bool TryParseDecimal(ReadOnlySpan<byte> text, out decimal value, out int bytesConsumed, SymbolTable symbolTable = null)
        {
            symbolTable = symbolTable ?? SymbolTable.InvariantUtf8;

            bytesConsumed = 0;
            value = default;

            if (symbolTable == SymbolTable.InvariantUtf8)
            {
                return Utf8Parser.TryParse(text, out value, out bytesConsumed);
            }
            else if (symbolTable == SymbolTable.InvariantUtf16)
            {
                ReadOnlySpan<char> textChars = MemoryMarshal.Cast<byte, char>(text);
                bool result = Utf16Parser.TryParseDecimal(textChars, out value, out int charactersConsumed);
                bytesConsumed = charactersConsumed * sizeof(char);
                return result;
            }

            return false;
        }
    }
}
