// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Text.Utf8
{
    public static class Utf8Encoder
    {
        const uint mask_0111_1111 = 0x7F;
        const uint mask_0011_1111 = 0x3F;
        const uint mask_0001_1111 = 0x1F;
        const uint mask_0000_1111 = 0x0F;
        const uint mask_0000_0111 = 0x07;
        const uint mask_1000_0000 = 0x80;
        const uint mask_1100_0000 = 0xC0;
        const uint mask_1110_0000 = 0xE0;
        const uint mask_1111_0000 = 0xF0;
        const uint mask_1111_1000 = 0xF8;

        #region Decoder
        // Should this be public?
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryGetNumberOfEncodedBytesFromFirstByte(Utf8CodeUnit first, out int numberOfBytes)
        {
            if (((byte)first & mask_1000_0000) == 0)
            {
                numberOfBytes = 1;
                return true;
            }

            if (((byte)first & mask_1110_0000) == mask_1100_0000)
            {
                numberOfBytes = 2;
                return true;
            }

            if (((byte)first & mask_1111_0000) == mask_1110_0000)
            {
                numberOfBytes = 3;
                return true;
            }

            if (((byte)first & mask_1111_1000) == mask_1111_0000)
            {
                numberOfBytes = 4;
                return true;
            }

            numberOfBytes = default(int);
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryGetFirstByteCodePointValue(Utf8CodeUnit first, out UnicodeCodePoint codePoint, out int encodedBytes)
        {
            if (!TryGetNumberOfEncodedBytesFromFirstByte(first, out encodedBytes))
            {
                codePoint = default(UnicodeCodePoint);
                return false;
            }

            switch (encodedBytes)
            {
                case 1:
                    codePoint = (UnicodeCodePoint)((byte)first & mask_0111_1111);
                    return true;
                case 2:
                    codePoint = (UnicodeCodePoint)((byte)first & mask_0001_1111);
                    return true;
                case 3:
                    codePoint = (UnicodeCodePoint)((byte)first & mask_0000_1111);
                    return true;
                case 4:
                    codePoint = (UnicodeCodePoint)((byte)first & mask_0000_0111);
                    return true;
                default:
                    codePoint = default(UnicodeCodePoint);
                    encodedBytes = 0;
                    return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryReadCodePointByte(Utf8CodeUnit nextByte, ref UnicodeCodePoint codePoint)
        {
            uint current = (byte)nextByte;
            if ((current & mask_1100_0000) != mask_1000_0000)
                return false;

            codePoint = new UnicodeCodePoint((codePoint.Value << 6) | (mask_0011_1111 & current));
            return true;
        }

        public static bool TryDecodeCodePoint(Span<Utf8CodeUnit> buffer, out UnicodeCodePoint codePoint, out int encodedBytes)
        {
            if (buffer.Length == 0)
            {
                codePoint = default(UnicodeCodePoint);
                encodedBytes = default(int);
                return false;
            }

            Utf8CodeUnit first = buffer[0];
            if (!TryGetFirstByteCodePointValue(first, out codePoint, out encodedBytes))
                return false;

            if (buffer.Length < encodedBytes)
                return false;

            // TODO: Should we manually inline this for values 1-4 or will compiler do this for us?
            for (int i = 1; i < encodedBytes; i++)
            {
                if (!TryReadCodePointByte(buffer[i], ref codePoint))
                    return false;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryFindEncodedCodePointBytesCountGoingBackwards(Span<Utf8CodeUnit> buffer, out int encodedBytes)
        {
            encodedBytes = 1;
            Span<Utf8CodeUnit> it = buffer;
            // TODO: Should we have something like: Span<byte>.(Slice from the back)
            for (; encodedBytes <= UnicodeConstants.Utf8MaxCodeUnitsPerCodePoint; encodedBytes++, it = it.Slice(0, it.Length - 1))
            {
                if (it.Length == 0)
                {
                    encodedBytes = default(int);
                    return false;
                }

                // TODO: Should we have Span<byte>.Last?
                if (Utf8CodeUnit.IsFirstCodeUnitInEncodedCodePoint((Utf8CodeUnit)it[it.Length - 1]))
                {
                    // output: encodedBytes
                    return true;
                }
            }

            // Invalid unicode character or stream prematurely ended (which is still invalid character in that stream)
            encodedBytes = default(int);
            return false;
        }

        // TODO: Name TBD
        // TODO: optimize?
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryDecodeCodePointBackwards(Span<Utf8CodeUnit> buffer, out UnicodeCodePoint codePoint, out int encodedBytes)
        {
            if (TryFindEncodedCodePointBytesCountGoingBackwards(buffer, out encodedBytes))
            {
                int realEncodedBytes;
                // TODO: Inline decoding, as the invalid surrogate check can be done faster
                bool ret = TryDecodeCodePoint(buffer.Slice(buffer.Length - encodedBytes, encodedBytes), out codePoint, out realEncodedBytes);
                if (ret && encodedBytes != realEncodedBytes)
                {
                    // invalid surrogate character
                    // we know the character length by iterating on surrogate characters from the end
                    // but the first byte of the character has also encoded length
                    // seems like the lengths don't match
                    return false;
                }
                return true;
            }

            codePoint = default(UnicodeCodePoint);
            encodedBytes = default(int);
            return false;
        }
        #endregion

        #region Encoder
        // Should this be public?
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int GetNumberOfEncodedBytes(UnicodeCodePoint codePoint)
        {
            if (codePoint.Value <= 0x7F)
            {
                return 1;
            }

            if (codePoint.Value <= 0x7FF)
            {
                return 2;
            }

            if (codePoint.Value <= 0xFFFF)
            {
                return 3;
            }

            if (codePoint.Value <= 0x1FFFFF)
            {
                return 4;
            }

            return 0;
        }

        public static bool TryEncodeCodePoint(UnicodeCodePoint codePoint, Span<byte> buffer, out int encodedBytes)
        {
            if (!UnicodeCodePoint.IsSupportedCodePoint(codePoint))
            {
                encodedBytes = 0;
                return false;
            }

            encodedBytes = GetNumberOfEncodedBytes(codePoint);
            if (encodedBytes > buffer.Length)
            {
                encodedBytes = 0;
                return false;
            }

            switch (encodedBytes)
            {
                case 1:
                    buffer[0] = (byte)(mask_0111_1111 & codePoint.Value);
                    return true;
                case 2:
                    buffer[0] = (byte)(((codePoint.Value >> 6) & mask_0001_1111) | mask_1100_0000);
                    buffer[1] = (byte)(((codePoint.Value >> 0) & mask_0011_1111) | mask_1000_0000);
                    return true;
                case 3:
                    buffer[0] = (byte)(((codePoint.Value >> 12) & mask_0000_1111) | mask_1110_0000);
                    buffer[1] = (byte)(((codePoint.Value >> 6) & mask_0011_1111) | mask_1000_0000);
                    buffer[2] = (byte)(((codePoint.Value >> 0) & mask_0011_1111) | mask_1000_0000);
                    return true;
                case 4:
                    buffer[0] = (byte)(((codePoint.Value >> 18) & mask_0000_0111) | mask_1111_0000);
                    buffer[1] = (byte)(((codePoint.Value >> 12) & mask_0011_1111) | mask_1000_0000);
                    buffer[2] = (byte)(((codePoint.Value >> 6) & mask_0011_1111) | mask_1000_0000);
                    buffer[3] = (byte)(((codePoint.Value >> 0) & mask_0011_1111) | mask_1000_0000);
                    return true;
                default:
                    return false;
            }
        }
        #endregion
    }
}
