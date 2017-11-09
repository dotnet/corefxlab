// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.



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
                ReadOnlySpan<char> textChars = text.NonPortableCast<byte, char>();
                bool result = Utf16Parser.TryParseDecimal(textChars, out value, out int charactersConsumed);
                bytesConsumed = charactersConsumed * sizeof(char);
                return result;
            }

            return false;
        }
    } 
}
