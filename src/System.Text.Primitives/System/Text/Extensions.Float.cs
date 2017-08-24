// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text
{
    public static partial class Extensions
    {
        public static bool TryFormat(this double value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default, SymbolTable symbolTable = null)
            => Formatters.Custom.TryFormat(value, buffer, out bytesWritten, format, symbolTable);

        public static bool TryFormat(this float value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default, SymbolTable symbolTable = null)
            => Formatters.Custom.TryFormat(value, buffer, out bytesWritten, format, symbolTable);
    }
}
