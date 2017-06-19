// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace System.Text
{
    internal static class FloatFormatter
    {
        public static bool TryFormatNumber(double value, bool isSingle, Span<byte> buffer, out int bytesWritten, TextFormat format = default(TextFormat), SymbolTable symbolTable = null)
        {
            Precondition.Require(format.Symbol == 'G' || format.Symbol == 'E' || format.Symbol == 'F');

            symbolTable = symbolTable ?? SymbolTable.InvariantUtf8;

            bytesWritten = 0;
            int written;

            if (Double.IsNaN(value))
            {
                return symbolTable.TryEncode(SymbolTable.Symbol.NaN, buffer, out bytesWritten);
            }

            if (Double.IsInfinity(value))
            {
                if (Double.IsNegativeInfinity(value))
                {
                    if (!symbolTable.TryEncode(SymbolTable.Symbol.MinusSign, buffer, out written))
                    {
                        bytesWritten = 0;
                        return false;
                    }
                    bytesWritten += written;
                }
                if (!symbolTable.TryEncode(SymbolTable.Symbol.InfinitySign, buffer.Slice(bytesWritten), out written))
                {
                    bytesWritten = 0;
                    return false;
                }
                bytesWritten += written;
                return true;
            }

            // TODO: the lines below need to be replaced with properly implemented algorithm
            // the problem is the algorithm is complex, so I am commiting a stub for now
            var hack = value.ToString(format.Symbol.ToString());
            var utf16Bytes = hack.AsSpan().AsBytes();
            if (symbolTable == SymbolTable.InvariantUtf8)
            {
                var status = Encoders.Utf8.ConvertFromUtf16(utf16Bytes, buffer, out int consumed, out bytesWritten);
                return status == Buffers.TransformationStatus.Done;
            }
            else if (symbolTable == SymbolTable.InvariantUtf16)
            {
                bytesWritten = utf16Bytes.Length;
                if (utf16Bytes.TryCopyTo(buffer))
                    return true;

                bytesWritten = 0;
                return false;
            }
            else
                throw new NotSupportedException();
        }
    }
}
