// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;
using System.Text.Utf8;

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
        #region Integers
        public static bool TryFormat(this byte value, Span<byte> buffer, out int bytesWritten, TextFormat format, EncodingData encoding)
        {
            return IntegerFormatter.TryFormatUInt64(value, 1, buffer, format, encoding, out bytesWritten);
        }

        public static bool TryFormat(this sbyte value, Span<byte> buffer, out int bytesWritten, TextFormat format, EncodingData encoding)
        {
            return IntegerFormatter.TryFormatInt64(value, 1, buffer, format, encoding, out bytesWritten);
        }

        public static bool TryFormat(this ushort value, Span<byte> buffer, out int bytesWritten, TextFormat format, EncodingData encoding)
        {
            return IntegerFormatter.TryFormatUInt64(value, 2, buffer, format, encoding, out bytesWritten);
        }
        public static bool TryFormat(this short value, Span<byte> buffer, out int bytesWritten, TextFormat format, EncodingData encoding)
        {
            return IntegerFormatter.TryFormatInt64(value, 2, buffer, format, encoding, out bytesWritten);
        }

        public static bool TryFormat(this uint value, Span<byte> buffer, out int bytesWritten, TextFormat format, EncodingData encoding)
        {
            return IntegerFormatter.TryFormatUInt64(value, 4, buffer, format, encoding, out bytesWritten);
        }
        public static bool TryFormat(this int value, Span<byte> buffer, out int bytesWritten, TextFormat format, EncodingData encoding)
        {
            return IntegerFormatter.TryFormatInt64(value, 4, buffer, format, encoding, out bytesWritten);
        }

        public static bool TryFormat(this ulong value, Span<byte> buffer, out int bytesWritten, TextFormat format, EncodingData encoding)
        {
            return IntegerFormatter.TryFormatUInt64(value, 8, buffer, format, encoding, out bytesWritten);
        }
        public static bool TryFormat(this long value, Span<byte> buffer, out int bytesWritten, TextFormat format, EncodingData encoding)
        {
            return IntegerFormatter.TryFormatInt64(value, 8, buffer, format, encoding, out bytesWritten);
        }
        #endregion
    }
}
