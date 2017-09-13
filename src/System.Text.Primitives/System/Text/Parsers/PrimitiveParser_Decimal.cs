// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using System.Buffers.Text;
using System.Text;

namespace System.Buffers
{
    public static partial class Parsers
    {
        public static partial class Custom
        {
            public static bool TryParseDecimal(ReadOnlySpan<byte> text, out decimal value, out int bytesConsumed, SymbolTable symbolTable = null)
            {
                symbolTable = symbolTable ?? SymbolTable.InvariantUtf8;

                bytesConsumed = 0;
                value = default;

                if (symbolTable == SymbolTable.InvariantUtf8)
                {
                    return Utf8.TryParseDecimal(text, out value, out bytesConsumed);
                }
                else if (symbolTable == SymbolTable.InvariantUtf16)
                {
                    ReadOnlySpan<char> textChars = text.NonPortableCast<byte, char>();
                    int charactersConsumed;
                    bool result = Utf16.TryParseDecimal(textChars, out value, out charactersConsumed);
                    bytesConsumed = charactersConsumed * sizeof(char);
                    return result;
                }

                return false;
            }
        }
    }
}
