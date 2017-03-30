// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text
{
    internal static class FormattingHelpers
    {
        private const int FractionDigits = 7;
        private const string HexTable = "0123456789abcdef";

        #region UTF-8 Helper methods

        // This method assumes the buffer passed starting at index has space for at least 2 more chars.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void WriteHexByte(byte value, ref byte buffer, int index)
        {
            Unsafe.Add(ref buffer, index) = (byte)HexTable[value >> 4];
            Unsafe.Add(ref buffer, index + 1) = (byte)HexTable[value & 0xF];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFractionDigits(long value, int digitCount, ref byte buffer, int index)
        {
            for (var i = FractionDigits; i > digitCount; i--)
                value = DivMod10(value, out long m);

            return WriteDigits(value, digitCount, ref buffer, index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteDigits(long value, ref byte buffer, int index)
        {
            return WriteDigits(value, CountDigits(value), ref buffer, index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteDigits(long value, int digitCount, ref byte buffer, int index)
        {
            long left = value;

            for (var i = digitCount - 1; i >= 0; i--)
            {
                left = DivMod10(left, out long num);
                Unsafe.Add(ref buffer, index + i) = (byte)('0' + num);
            }

            return digitCount;
        }

        #endregion UTF-8 Helper methods

        #region UTF-16 Helper methods

        // This method assumes the buffer passed starting at index has space for at least 2 more chars.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void WriteHexByte(byte value, ref char buffer, int index)
        {
            Unsafe.Add(ref buffer, index) = HexTable[value >> 4];
            Unsafe.Add(ref buffer, index + 1) = HexTable[value & 0xF];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFractionDigits(long value, int digitCount, ref char buffer, int index)
        {
            for (var i = FractionDigits; i > digitCount; i--)
                value = DivMod10(value, out long m);

            return WriteDigits(value, digitCount, ref buffer, index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteDigits(long value, ref char buffer, int index)
        {
            return WriteDigits(value, CountDigits(value), ref buffer, index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteDigits(long value, int digitCount, ref char buffer, int index)
        {
            long left = value;

            for (var i = digitCount - 1; i >= 0; i--)
            {
                left = DivMod10(left, out long num);
                Unsafe.Add(ref buffer, index + i) = (char)('0' + num);
            }

            return digitCount;
        }

        #endregion UTF-16 Helper methods

        #region Fast Math Helper methods

        /// <summary>
        /// This method for doing division and modulus for integers by 10 is ~5-6 times faster
        /// than the division and multiplication operators.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long DivMod10(long n, out long modulo)
        {
            // This commented implementation is faster, but only works for positive integer values 
            // between 0 and 1073741828.
            //      long d = ((0x1999999A * n) >> 32);
            //      modulo = (n - (d * 10));
            //      return d;

            return DivMod(n, 10, out modulo);
        }

        /// <summary>
        /// We don't have access to Math.DivRem, so this is a copy of the implementation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long DivMod(long numerator, long denominator, out long modulo)
        {
            long div = numerator / denominator;
            modulo = numerator - (div * denominator);
            return div;
        }

        #endregion Fast Math Helper methods

        #region Character counting helper methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CountDigits(long n)
        {
            Precondition.Require(n >= 0);

            if (n == 0) return 1;
            return (int)Math.Floor(Math.Log10(n)) + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CountFractionDigits(long n)
        {
            Precondition.Require(n >= 0);

            long left = n;
            long m = 0;
            int count = FractionDigits;

            // Remove all the 0 (zero) values from the right.
            while (left > 0 && m == 0 && count > 0)
            {
                left = DivMod10(left, out m);
                count--;
            }

            return count + 1;
        }

        #endregion Character counting helper methods
    }
}
