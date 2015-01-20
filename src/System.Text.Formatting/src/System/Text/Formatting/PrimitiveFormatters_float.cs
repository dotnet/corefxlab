// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text.Formatting 
{
    public static partial class PrimitiveFormatters
    {
        public static bool TryFormat(this double value, Span<byte> buffer, ReadOnlySpan<char> format, FormattingData formattingData, out int bytesWritten)
        {
            Format.Parsed parsedFormat = Format.Parsed.Parse(format);
            return TryFormat(value, buffer, parsedFormat, formattingData, out bytesWritten);
        }

        public static bool TryFormat(this double value, Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int bytesWritten)
        {
            Precondition.Require(format.Symbol == Format.Symbol.G);
            return FloatFormatter.TryFormatNumber(value, false, buffer, format, formattingData, out bytesWritten);
        }

        public static bool TryFormat(this float value, Span<byte> buffer, ReadOnlySpan<char> format, FormattingData formattingData, out int bytesWritten)
        {
            Format.Parsed parsedFormat = Format.Parsed.Parse(format);
            return TryFormat(value, buffer, parsedFormat, formattingData, out bytesWritten);
        }

        public static bool TryFormat(this float value, Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int bytesWritten)
        {
            Precondition.Require(format.Symbol == Format.Symbol.G);
            return FloatFormatter.TryFormatNumber(value, true, buffer, format, formattingData, out bytesWritten);
        }
    }
}
