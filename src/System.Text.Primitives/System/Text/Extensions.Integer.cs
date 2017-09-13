// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;

namespace System.Buffers.Text
{
    /// <summary>
    /// Pseudo-implementations of IBufferFormattable interface for primitive types
    /// </summary>
    /// <remarks>
    /// Holds extension methods for formatting types that cannot implement IBufferFormattable for layering reasons.
    /// </remarks>
    public static partial class Extensions
    {
        public static bool TryFormat(this byte value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default, SymbolTable symbolTable = null)
            => Formatters.Custom.TryFormat(value, buffer, out bytesWritten, format, symbolTable);

        public static bool TryFormat(this sbyte value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default, SymbolTable symbolTable = null)
            => Formatters.Custom.TryFormat(value, buffer, out bytesWritten, format, symbolTable);

        public static bool TryFormat(this ushort value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default, SymbolTable symbolTable = null)
            => Formatters.Custom.TryFormat(value, buffer, out bytesWritten, format, symbolTable);

        public static bool TryFormat(this short value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default, SymbolTable symbolTable = null)
            => Formatters.Custom.TryFormat(value, buffer, out bytesWritten, format, symbolTable);

        public static bool TryFormat(this uint value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default, SymbolTable symbolTable = null)
            => Formatters.Custom.TryFormat(value, buffer, out bytesWritten, format, symbolTable);

        public static bool TryFormat(this int value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default, SymbolTable symbolTable = null)
            => Formatters.Custom.TryFormat(value, buffer, out bytesWritten, format, symbolTable);

        public static bool TryFormat(this ulong value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default, SymbolTable symbolTable = null)
            => Formatters.Custom.TryFormat(value, buffer, out bytesWritten, format, symbolTable);

        public static bool TryFormat(this long value, Span<byte> buffer, out int bytesWritten, ParsedFormat format = default, SymbolTable symbolTable = null)
            => Formatters.Custom.TryFormat(value, buffer, out bytesWritten, format, symbolTable);
    }
}
