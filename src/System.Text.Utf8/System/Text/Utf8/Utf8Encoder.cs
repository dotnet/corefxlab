// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Text.Utf8
{
    public static class Utf8Encoder
    {
        // To get this to compile with dotnet cli, we need to temporarily un-binary the magic values
        private const byte b0000_0111U = 7;
        private const byte b0000_1111U = 15;
        private const byte b0001_1111U = 31;
        private const byte b0011_1111U = 63;
        private const byte b0111_1111U = 127;
        private const byte b1000_0000U = 128;
        private const byte b1100_0000U = 192;
        private const byte b1110_0000U = 224;
        private const byte b1111_0000U = 240;
        private const byte b1111_1000U = 248;

        #region Decoder
        // Should this be public?
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryGetNumberOfEncodedBytesFromFirstByte(byte first, out int numberOfBytes)
        {
            if ((first & b1000_0000U) == 0)
            {
                numberOfBytes = 1;
                return true;
            }

            if ((first & b1110_0000U) == b1100_0000U)
            {
                numberOfBytes = 2;
                return true;
            }

            if ((first & b1111_0000U) == b1110_0000U)
            {
                numberOfBytes = 3;
                return true;
            }

            if ((first & b1111_1000U) == b1111_0000U)
            {
                numberOfBytes = 4;
                return true;
            }

            numberOfBytes = default(int);
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryGetFirstByteCodePointValue(byte first, out UnicodeCodePoint codePoint, out int encodedBytes)
        {
            if (!TryGetNumberOfEncodedBytesFromFirstByte(first, out encodedBytes))
            {
                codePoint = default(UnicodeCodePoint);
                return false;
            }

            switch (encodedBytes)
            {
                case 1:
                    codePoint = (UnicodeCodePoint)(first & b0111_1111U);
                    return true;
                case 2:
                    codePoint = (UnicodeCodePoint)(first & b0001_1111U);
                    return true;
                case 3:
                    codePoint = (UnicodeCodePoint)(first & b0000_1111U);
                    return true;
                case 4:
                    codePoint = (UnicodeCodePoint)(first & b0000_0111U);
                    return true;
                default:
                    codePoint = default(UnicodeCodePoint);
                    encodedBytes = 0;
                    return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryReadCodePointByte(byte nextByte, ref UnicodeCodePoint codePoint)
        {
            uint current = nextByte;
            if ((current & b1100_0000U) != b1000_0000U)
                return false;

            codePoint = new UnicodeCodePoint((codePoint.Value << 6) | (b0011_1111U & current));
            return true;
        }

        public static bool TryDecodeCodePoint(ReadOnlySpan<byte> buffer, out UnicodeCodePoint codePoint, out int encodedBytes)
        {
            if (buffer.Length == 0)
            {
                codePoint = default(UnicodeCodePoint);
                encodedBytes = default(int);
                return false;
            }

            byte first = buffer[0];
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
        private static bool TryFindEncodedCodePointBytesCountGoingBackwards(ReadOnlySpan<byte> buffer, out int encodedBytes)
        {
            encodedBytes = 1;
            ReadOnlySpan<byte> it = buffer;
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
        public static bool TryDecodeCodePointBackwards(ReadOnlySpan<byte> buffer, out UnicodeCodePoint codePoint, out int encodedBytes)
        {
            if (TryFindEncodedCodePointBytesCountGoingBackwards(buffer, out encodedBytes))
            {
                int realEncodedBytes;
                // TODO: Inline decoding, as the invalid surrogate check can be done faster
                bool ret = TryDecodeCodePoint(buffer.Slice(buffer.Length - encodedBytes), out codePoint, out realEncodedBytes);
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
                    buffer[0] = (byte)(b0111_1111U & codePoint.Value);
                    return true;
                case 2:
                    byte b0 = (byte)(((codePoint.Value >> 6) & b0001_1111U) | b1100_0000U);
                    byte b1 = (byte)(((codePoint.Value >> 0) & b0011_1111U) | b1000_0000U);
                    buffer.Write((ushort)(b0 | b1 << 8));
                    return true;
                case 3:
                    b0 = (byte)(((codePoint.Value >> 12) & b0000_1111U) | b1110_0000U);
                    b1 = (byte)(((codePoint.Value >> 6) & b0011_1111U) | b1000_0000U);
                    buffer.Write((ushort)(b0 | b1 << 8));
                    buffer[2] = (byte)(((codePoint.Value >> 0) & b0011_1111U) | b1000_0000U);
                    return true;
                case 4:
                    b0 = (byte)(((codePoint.Value >> 18) & b0000_0111U) | b1111_0000U);
                    b1 = (byte)(((codePoint.Value >> 12) & b0011_1111U) | b1000_0000U);
                    byte b2 = (byte)(((codePoint.Value >> 6) & b0011_1111U) | b1000_0000U);
                    byte b3 = (byte)(((codePoint.Value >> 0) & b0011_1111U) | b1000_0000U);
                    buffer.Write((uint)(b0 | b1 << 8 | b2 << 16 | b3 << 24));
                    return true;
                default:
                    return false;
            }
        }
        #endregion
    }
}
