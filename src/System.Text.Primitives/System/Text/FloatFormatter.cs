// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text 
{
    internal static class FloatFormatter
    {
        public static bool TryFormatNumber(double value, bool isSingle, Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int bytesWritten)
        {
            Precondition.Require(format.Symbol == 'G' || format.Symbol == 'E' || format.Symbol == 'F');

            bytesWritten = 0;
            int written;

            if (Double.IsNaN(value))
            {
                return formattingData.TryWriteSymbol(FormattingData.Symbol.NaN, buffer, out bytesWritten);
            }

            if (Double.IsInfinity(value))
            {
                if (Double.IsNegativeInfinity(value))
                {
                    if (!formattingData.TryWriteSymbol(FormattingData.Symbol.MinusSign, buffer, out written))
                    {
                        bytesWritten = 0;
                        return false;
                    }
                    bytesWritten += written;
                }
                if (!formattingData.TryWriteSymbol(FormattingData.Symbol.InfinitySign, buffer.Slice(bytesWritten), out written))
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
            return hack.TryFormat(buffer, default(Format.Parsed), formattingData, out bytesWritten);
        }
    }
}
