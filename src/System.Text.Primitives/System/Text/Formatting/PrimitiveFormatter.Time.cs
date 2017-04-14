// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text
{
    public static partial class PrimitiveFormatter
    {
        internal static readonly TimeSpan NullOffset = TimeSpan.MinValue;

        public static bool TryFormat(this DateTimeOffset value, Span<byte> buffer, out int bytesWritten, TextFormat format = default(TextFormat), TextEncoder encoder = null)
        {
            TimeSpan offset = NullOffset;
            if (format.IsDefault)
            {
                format.Symbol = 'G';
                offset = value.Offset;
            }

            Precondition.Require(format.Symbol == 'R' || format.Symbol == 'O' || format.Symbol == 'G');

            encoder = encoder == null ? TextEncoder.Utf8 : encoder;

            switch (format.Symbol)
            {
                case 'R':
                    return TryFormatDateTimeRfc1123(value.UtcDateTime, buffer, out bytesWritten, encoder);

                case 'O':
                    return TryFormatDateTimeFormatO(value.DateTime, value.Offset, buffer, out bytesWritten, encoder);

                case 'G':
                    return TryFormatDateTimeFormatG(value.DateTime, offset, buffer, out bytesWritten, encoder);

                default:
                    throw new NotImplementedException();
            }
        }

        public static bool TryFormat(this DateTime value, Span<byte> buffer, out int bytesWritten, TextFormat format = default(TextFormat), TextEncoder encoder = null)
        {
            if (format.IsDefault)
            {
                format.Symbol = 'G';
            }

            Precondition.Require(format.Symbol == 'R' || format.Symbol == 'O' || format.Symbol == 'G');

            encoder = encoder == null ? TextEncoder.Utf8 : encoder;

            switch (format.Symbol)
            {
                case 'R':
                    return TryFormatDateTimeRfc1123(value, buffer, out bytesWritten, encoder);

                case 'O':
                    return TryFormatDateTimeFormatO(value, NullOffset, buffer, out bytesWritten, encoder);

                case 'G':
                    return TryFormatDateTimeFormatG(value, NullOffset, buffer, out bytesWritten, encoder);

                default:
                    throw new NotImplementedException();
            }
        }

        public static bool TryFormat(this TimeSpan value, Span<byte> buffer, out int bytesWritten, TextFormat format = default(TextFormat), TextEncoder encoder = null)
        {
            if (format.IsDefault)
            {
                format.Symbol = 'c';
            }

            Precondition.Require(format.Symbol == 'G' || format.Symbol == 'g' || format.Symbol == 'c' || format.Symbol == 't' || format.Symbol == 'T');

            encoder = encoder == null ? TextEncoder.Utf8 : encoder;

            return TryFormatTimeSpan(value, format.Symbol, buffer, out bytesWritten, encoder);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryFormatDateTimeFormatG(DateTime value, TimeSpan offset, Span<byte> buffer, out int bytesWritten, TextEncoder encoder)
        {
            // for now it only works for invariant culture
            if (encoder.IsInvariantUtf8)
                return InvariantUtf8TimeFormatter.TryFormatG(value, offset, buffer, out bytesWritten);
            else if (encoder.IsInvariantUtf16)
                return InvariantUtf16TimeFormatter.TryFormatG(value, offset, buffer, out bytesWritten);
            else
                throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryFormatDateTimeFormatO(DateTime value, TimeSpan offset, Span<byte> buffer, out int bytesWritten, TextEncoder encoder)
        {
            // for now it only works for invariant culture
            if (encoder.IsInvariantUtf8)
                return InvariantUtf8TimeFormatter.TryFormatO(value, offset, buffer, out bytesWritten);
            else if (encoder.IsInvariantUtf16)
                return InvariantUtf16TimeFormatter.TryFormatO(value, offset, buffer, out bytesWritten);
            else
                throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryFormatDateTimeRfc1123(DateTime value, Span<byte> buffer, out int bytesWritten, TextEncoder encoder)
        {
            // for now it only works for invariant culture
            if (encoder.IsInvariantUtf8)
                return InvariantUtf8TimeFormatter.TryFormatRfc1123(value, buffer, out bytesWritten);
            else if (encoder.IsInvariantUtf16)
                return InvariantUtf16TimeFormatter.TryFormatRfc1123(value, buffer, out bytesWritten);
            else
                throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryFormatTimeSpan(TimeSpan value, char format, Span<byte> buffer, out int bytesWritten, TextEncoder encoder)
        {
            // for now it only works for invariant culture
            if (encoder.IsInvariantUtf8)
                return InvariantUtf8TimeFormatter.TryFormat(value, format, buffer, out bytesWritten);
            else if (encoder.IsInvariantUtf16)
                return InvariantUtf16TimeFormatter.TryFormat(value, format, buffer, out bytesWritten);
            else
                throw new NotImplementedException();
        }
    }
}
