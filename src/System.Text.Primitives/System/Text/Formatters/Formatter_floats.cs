// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Buffers.Text
{
    public static partial class CustomFormatter
    {
        private static bool TryFormatNumber(double value, bool isSingle, Span<byte> buffer, out int bytesWritten, StandardFormat format = default, SymbolTable symbolTable = null)
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
            var utf16Bytes = MemoryMarshal.AsBytes(hack.AsSpan());
            if (symbolTable == SymbolTable.InvariantUtf8)
            {
                var status = Encodings.Utf16.ToUtf8(utf16Bytes, buffer, out int consumed, out bytesWritten);
                return status == OperationStatus.Done;
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
            {
                // TODO: This is currently pretty expensive. Can this be done more efficiently?
                //       Note: removing the hack might solve this problem a very different way.
                var status = Encodings.Utf16.ToUtf8Length(utf16Bytes, out int needed);
                if (status != OperationStatus.Done)
                {
                    bytesWritten = 0;
                    return false;
                }

                Span<byte> temp = stackalloc byte[needed];

                status = Encodings.Utf16.ToUtf8(utf16Bytes, temp, out int consumed, out written);
                if (status != OperationStatus.Done)
                {
                    bytesWritten = 0;
                    return false;
                }

                return symbolTable.TryEncode(temp, buffer, out consumed, out bytesWritten);
            }
        }
    }

}
