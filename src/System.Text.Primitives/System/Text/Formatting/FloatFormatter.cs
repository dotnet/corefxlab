// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace System.Text
{
    internal static class FloatFormatter
    {
        public static bool TryFormatNumber(double value, bool isSingle, Span<byte> buffer, out int bytesWritten, TextFormat format = default, TextEncoder encoder = null)
        {
            Precondition.Require(format.Symbol == 'G' || format.Symbol == 'E' || format.Symbol == 'F');

            encoder = encoder ?? TextEncoder.Utf8;

            bytesWritten = 0;
            int written;

            if (Double.IsNaN(value))
            {
                return encoder.TryEncode(TextEncoder.Symbol.NaN, buffer, out bytesWritten);
            }

            if (Double.IsInfinity(value))
            {
                if (Double.IsNegativeInfinity(value))
                {
                    if (!encoder.TryEncode(TextEncoder.Symbol.MinusSign, buffer, out written))
                    {
                        bytesWritten = 0;
                        return false;
                    }
                    bytesWritten += written;
                }
                if (!encoder.TryEncode(TextEncoder.Symbol.InfinitySign, buffer.Slice(bytesWritten), out written))
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
            return encoder.TryEncode(hack, buffer, out bytesWritten);
        }
    }
}
