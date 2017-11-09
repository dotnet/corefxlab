// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers.Text
{
    public static partial class Extensions
    {
        public static bool TryFormat(this DateTimeOffset value, Span<byte> buffer, out int bytesWritten, StandardFormat format = default, SymbolTable symbolTable = null)
            => CustomFormatter.TryFormat(value, buffer, out bytesWritten, format, symbolTable);

        public static bool TryFormat(this DateTime value, Span<byte> buffer, out int bytesWritten, StandardFormat format = default, SymbolTable symbolTable = null)
            => CustomFormatter.TryFormat(value, buffer, out bytesWritten, format, symbolTable);

        public static bool TryFormat(this TimeSpan value, Span<byte> buffer, out int bytesWritten, StandardFormat format = default, SymbolTable symbolTable = null)
            => CustomFormatter.TryFormat(value, buffer, out bytesWritten, format, symbolTable);
    }
}
