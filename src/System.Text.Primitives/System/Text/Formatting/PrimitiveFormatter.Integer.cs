// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text
{
    /// <summary>
    /// Pseudo-implementations of IBufferFormattable interface for primitive types
    /// </summary>
    /// <remarks>
    /// Holds extension methods for formatting types that cannot implement IBufferFormattable for layering reasons.
    /// </remarks>
    public static partial class PrimitiveFormatter
    {
        public static bool TryFormat(this byte value, Span<byte> buffer, out int bytesWritten, TextFormat format = default, TextEncoder encoder = null)
        {
            return TryFormatCore(value, buffer, out bytesWritten, format, encoder);
        }

        public static bool TryFormat(this sbyte value, Span<byte> buffer, out int bytesWritten, TextFormat format = default, TextEncoder encoder = null)
        {
            return TryFormatCore(value, 0xff, buffer, out bytesWritten, format, encoder);
        }

        public static bool TryFormat(this ushort value, Span<byte> buffer, out int bytesWritten, TextFormat format = default, TextEncoder encoder = null)
        {
            return TryFormatCore(value, buffer, out bytesWritten, format, encoder);
        }

        public static bool TryFormat(this short value, Span<byte> buffer, out int bytesWritten, TextFormat format = default, TextEncoder encoder = null)
        {
            return TryFormatCore(value, 0xffff, buffer, out bytesWritten, format, encoder);
        }

        public static bool TryFormat(this uint value, Span<byte> buffer, out int bytesWritten, TextFormat format = default, TextEncoder encoder = null)
        {
            return TryFormatCore(value, buffer, out bytesWritten, format, encoder);
        }

        public static bool TryFormat(this int value, Span<byte> buffer, out int bytesWritten, TextFormat format = default, TextEncoder encoder = null)
        {
            return TryFormatCore(value, 0xffffffff, buffer, out bytesWritten, format, encoder);
        }

        public static bool TryFormat(this ulong value, Span<byte> buffer, out int bytesWritten, TextFormat format = default, TextEncoder encoder = null)
        {
            return TryFormatCore(value, buffer, out bytesWritten, format, encoder);
        }

        public static bool TryFormat(this long value, Span<byte> buffer, out int bytesWritten, TextFormat format = default, TextEncoder encoder = null)
        {
            return TryFormatCore(value, 0xffffffffffffffff, buffer, out bytesWritten, format, encoder);
        }

        static bool TryFormatCore(long value, ulong mask, Span<byte> buffer, out int bytesWritten, TextFormat format, TextEncoder encoder)
        {
            encoder = encoder ?? TextEncoder.Utf8;
            if (format.IsDefault || format.Symbol == 'g')
            {
                format.Symbol = 'G';
            }

            if (encoder.IsInvariantUtf8)
                return TryFormatInvariantUtf8(value, mask, buffer, out bytesWritten, format);
            else if (encoder.IsInvariantUtf16)
                return TryFormatInvariantUtf16(value, mask, buffer, out bytesWritten, format);
            else
                return IntegerFormatter.TryFormatInt64(value, mask, buffer, out bytesWritten, format, encoder);
        }

        static bool TryFormatCore(ulong value, Span<byte> buffer, out int bytesWritten, TextFormat format, TextEncoder encoder)
        {
            encoder = encoder ?? TextEncoder.Utf8;

            if (encoder.IsInvariantUtf8)
                return TryFormatInvariantUtf8(value, buffer, out bytesWritten, format);
            else if (encoder.IsInvariantUtf16)
                return TryFormatInvariantUtf16(value, buffer, out bytesWritten, format);
            else
                return IntegerFormatter.TryFormatUInt64(value, buffer, out bytesWritten, format, encoder);
        }

        static bool TryFormatInvariantUtf8(long value, ulong mask, Span<byte> buffer, out int bytesWritten, TextFormat format)
        {
            switch (format.Symbol)
            {
                case (char)0:
                case 'd':
                case 'D':
                case 'G':
                case 'g':
                    return InvariantUtf8IntegerFormatter.TryFormatDecimalInt64(value, format.Precision, buffer, out bytesWritten);

                case 'n':
                case 'N':
                    return InvariantUtf8IntegerFormatter.TryFormatNumericInt64(value, format.Precision, buffer, out bytesWritten);

                case 'x':
                    return InvariantUtf8IntegerFormatter.TryFormatHexUInt64((ulong)value & mask, format.Precision, true, buffer, out bytesWritten);

                case 'X':
                    return InvariantUtf8IntegerFormatter.TryFormatHexUInt64((ulong)value & mask, format.Precision, false, buffer, out bytesWritten);

                default:
                    throw new NotSupportedException();
            }
        }

        static bool TryFormatInvariantUtf8(ulong value, Span<byte> buffer, out int bytesWritten, TextFormat format)
        {
            switch (format.Symbol)
            {
                case (char)0:
                case 'd':
                case 'D':
                case 'G':
                case 'g':
                    return InvariantUtf8IntegerFormatter.TryFormatDecimalUInt64(value, format.Precision, buffer, out bytesWritten);

                case 'n':
                case 'N':
                    return InvariantUtf8IntegerFormatter.TryFormatNumericUInt64(value, format.Precision, buffer, out bytesWritten);

                case 'x':
                    return InvariantUtf8IntegerFormatter.TryFormatHexUInt64(value, format.Precision, true, buffer, out bytesWritten);

                case 'X':
                    return InvariantUtf8IntegerFormatter.TryFormatHexUInt64(value, format.Precision, false, buffer, out bytesWritten);

                default:
                    throw new NotSupportedException();
            }
        }

        static bool TryFormatInvariantUtf16(long value, ulong mask, Span<byte> buffer, out int bytesWritten, TextFormat format)
        {
            switch (format.Symbol)
            {
                case (char)0:
                case 'd':
                case 'D':
                case 'G':
                case 'g':
                    return InvariantUtf16IntegerFormatter.TryFormatDecimalInt64(value, format.Precision, buffer, out bytesWritten);

                case 'n':
                case 'N':
                    return InvariantUtf16IntegerFormatter.TryFormatNumericInt64(value, format.Precision, buffer, out bytesWritten);

                case 'x':
                    return InvariantUtf16IntegerFormatter.TryFormatHexUInt64((ulong)value & mask, format.Precision, true, buffer, out bytesWritten);

                case 'X':
                    return InvariantUtf16IntegerFormatter.TryFormatHexUInt64((ulong)value & mask, format.Precision, false, buffer, out bytesWritten);

                default:
                    throw new NotSupportedException();
            }
        }

        static bool TryFormatInvariantUtf16(ulong value, Span<byte> buffer, out int bytesWritten, TextFormat format)
        {
            switch (format.Symbol)
            {
                case (char)0:
                case 'd':
                case 'D':
                case 'G':
                case 'g':
                    return InvariantUtf16IntegerFormatter.TryFormatDecimalUInt64(value, format.Precision, buffer, out bytesWritten);

                case 'n':
                case 'N':
                    return InvariantUtf16IntegerFormatter.TryFormatNumericUInt64(value, format.Precision, buffer, out bytesWritten);

                case 'x':
                    return InvariantUtf16IntegerFormatter.TryFormatHexUInt64(value, format.Precision, true, buffer, out bytesWritten);

                case 'X':
                    return InvariantUtf16IntegerFormatter.TryFormatHexUInt64(value, format.Precision, false, buffer, out bytesWritten);

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
