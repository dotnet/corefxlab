// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Text 
{
    public static partial class PrimitiveFormatter
    {
        public static unsafe bool TryFormat(this Guid value, Span<byte> buffer, out int bytesWritten, TextFormat format = default(TextFormat), TextEncoder encoder = null)
        {
            encoder = encoder == null ? TextEncoder.Utf8 : encoder;

            if (encoder.IsInvariantUtf8)
                return InvariantUtf8UuidFormatter.TryFormat(value, buffer, out bytesWritten, format);
            else if (encoder.IsInvariantUtf16)
                return InvariantUtf16UuidFormatter.TryFormat(value, buffer, out bytesWritten, format);
            else
                throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryWriteInt32(int i, Span<byte> buffer, ref int bytesWritten, TextFormat byteFormat, TextEncoder encoder)
        {
            int written;
            if (!i.TryFormat(buffer.Slice(bytesWritten), out written, byteFormat, encoder))
            {
                bytesWritten = 0;
                return false;
            }
            bytesWritten += written;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryWriteInt64(long i, Span<byte> buffer, ref int bytesWritten, TextFormat byteFormat, TextEncoder encoder)
        {
            int written;
            if (!i.TryFormat(buffer.Slice(bytesWritten), out written, byteFormat, encoder))
            {
                bytesWritten = 0;
                return false;
            }
            bytesWritten += written;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryWriteChar(char character, Span<byte> buffer, ref int bytesWritten, TextEncoder encoder)
        {
            int consumed;
            int written;

            unsafe
            {
                ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(&character, 1);

                if (!encoder.TryEncode(charSpan, buffer.Slice(bytesWritten), out consumed, out written))
                {
                    bytesWritten = 0;
                    return false;
                }
            }

            bytesWritten += written;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryWriteString(string text, Span<byte> buffer, ref int bytesWritten, TextEncoder encoder)
        {
            int written;
            if (!encoder.TryEncode(text, buffer.Slice(bytesWritten), out written))
            {
                bytesWritten = 0;
                return false;
            }
            bytesWritten += written;
            return true;
        }
    }
}
