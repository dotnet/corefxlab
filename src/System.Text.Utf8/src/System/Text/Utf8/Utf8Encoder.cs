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
        private static bool TryGetNumberOfCodeUnitsInCodePointFromFirstCodeUnit(Utf8CodeUnit first, out int numberOfCodeUnits)
        {
            if (((byte)first & mask_1000_0000) == 0)
            {
                numberOfCodeUnits = 1;
                return true;
            }

            if (((byte)first & mask_1110_0000) == mask_1100_0000)
            {
                numberOfCodeUnits = 2;
                return true;
            }

            if (((byte)first & mask_1111_0000) == mask_1110_0000)
            {
                numberOfCodeUnits = 3;
                return true;
            }

            if (((byte)first & mask_1111_1000) == mask_1111_0000)
            {
                numberOfCodeUnits = 4;
                return true;
            }

            numberOfCodeUnits = default(int);
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryGetValueOfFirstCodeUnitInEncodedCodePoint(Utf8CodeUnit first, out UnicodeCodePoint codePoint, out int numberOfCodeUnits)
        {
            if (!TryGetNumberOfCodeUnitsInCodePointFromFirstCodeUnit(first, out numberOfCodeUnits))
            {
                codePoint = default(UnicodeCodePoint);
                return false;
            }

            switch (numberOfCodeUnits)
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
                    numberOfCodeUnits = 0;
                    return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryReadValueOfNextCodeUnitOfEncodedCodePoint(Utf8CodeUnit nextCodeUnit, ref UnicodeCodePoint codePoint)
        {
            uint current = (byte)nextCodeUnit;
            if ((current & mask_1100_0000) != mask_1000_0000)
                return false;

            codePoint = new UnicodeCodePoint((codePoint.Value << 6) | (mask_0011_1111 & current));
            return true;
        }

        public static bool TryDecodeCodePoint(Span<Utf8CodeUnit> buffer, out UnicodeCodePoint codePoint, out int numberOfCodeUnits)
        {
            if (buffer.Length == 0)
            {
                codePoint = default(UnicodeCodePoint);
                numberOfCodeUnits = default(int);
                return false;
            }

            Utf8CodeUnit first = buffer[0];
            if (!TryGetValueOfFirstCodeUnitInEncodedCodePoint(first, out codePoint, out numberOfCodeUnits))
                return false;

            if (buffer.Length < numberOfCodeUnits)
                return false;

            // TODO: Should we manually inline this for values 1-4 or will compiler do this for us?
            for (int i = 1; i < numberOfCodeUnits; i++)
            {
                if (!TryReadValueOfNextCodeUnitOfEncodedCodePoint(buffer[i], ref codePoint))
                    return false;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryFindNumberOfCodeUnitsInCodePointGoingBackwards(Span<Utf8CodeUnit> buffer, out int numberOfCodeUnits)
        {
            numberOfCodeUnits = 1;
            Span<Utf8CodeUnit> it = buffer;
            // TODO: Should we have something like: Span<byte>.(Slice from the back)
            for (; numberOfCodeUnits <= UnicodeConstants.Utf8MaxCodeUnitsPerCodePoint; numberOfCodeUnits++, it = it.Slice(0, it.Length - 1))
            {
                if (it.Length == 0)
                {
                    numberOfCodeUnits = default(int);
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
            numberOfCodeUnits = default(int);
            return false;
        }

        // TODO: Name TBD
        // TODO: optimize?
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryDecodeCodePointBackwards(Span<Utf8CodeUnit> buffer, out UnicodeCodePoint codePoint, out int numberOfCodeUnits)
        {
            if (TryFindNumberOfCodeUnitsInCodePointGoingBackwards(buffer, out numberOfCodeUnits))
            {
                int realNumberOfCodeUnits;
                // TODO: Inline decoding, as the invalid surrogate check can be done faster
                bool ret = TryDecodeCodePoint(buffer.Slice(buffer.Length - numberOfCodeUnits, numberOfCodeUnits), out codePoint, out realNumberOfCodeUnits);
                if (ret && numberOfCodeUnits != realNumberOfCodeUnits)
                {
                    // invalid surrogate character
                    // we know the character length by iterating on surrogate characters from the end
                    // but the first byte of the character has also encoded length
                    // seems like the lengths don't match
                    return false;
                }
                return ret;
            }

            codePoint = default(UnicodeCodePoint);
            numberOfCodeUnits = default(int);
            return false;
        }
        #endregion

        #region Encoder
        // Should this be public?
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int GetNumberOfCodeUnitsInCodePoint(UnicodeCodePoint codePoint)
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

        public static bool TryEncodeCodePoint(UnicodeCodePoint codePoint, Span<byte> buffer, out int numberOfCodeUnits)
        {
            if (!UnicodeCodePoint.IsSupportedCodePoint(codePoint))
            {
                numberOfCodeUnits = 0;
                return false;
            }

            numberOfCodeUnits = GetNumberOfCodeUnitsInCodePoint(codePoint);
            if (numberOfCodeUnits > buffer.Length)
            {
                numberOfCodeUnits = 0;
                return false;
            }

            switch (numberOfCodeUnits)
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
