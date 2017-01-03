// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text 
{
    public static partial class PrimitiveFormatter
    {
        public static bool TryFormat(this double value, Span<byte> buffer, TextFormat format, EncodingData encoding, out int bytesWritten)
        {
            if (format.IsDefault)
            {
                format.Symbol = 'G';
            }
            Precondition.Require(format.Symbol == 'G');
            return FloatFormatter.TryFormatNumber(value, false, buffer, format, encoding, out bytesWritten);
        }

        public static bool TryFormat(this float value, Span<byte> buffer, TextFormat format, EncodingData encoding, out int bytesWritten)
        {
            if (format.IsDefault)
            {
                format.Symbol = 'G';
            }
            Precondition.Require(format.Symbol == 'G');
            return FloatFormatter.TryFormatNumber(value, true, buffer, format, encoding, out bytesWritten);
        }
    }
}
