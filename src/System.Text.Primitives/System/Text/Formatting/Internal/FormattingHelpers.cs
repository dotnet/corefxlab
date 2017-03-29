// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text
{
    internal static class FormattingHelpers
    {
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
        public static void WriteTwoDigits(int value, ref byte buffer, int index)
        {
            Precondition.Require(value >= 0 && value <= 99);

            ulong v1 = FormattingHelpers.DivMod10((ulong)value, out ulong v2);
            Unsafe.Add(ref buffer, index) = (byte)('0' + v1);
            Unsafe.Add(ref buffer, index + 1) = (byte)('0' + v2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteFourDigits(int value, ref byte buffer, int index)
        {
            Precondition.Require(value >= 0 && value <= 9999);

            ulong left = DivMod10((ulong)value, out ulong v);
            Unsafe.Add(ref buffer, index + 3) = (byte)('0' + v);

            left = DivMod10(left, out v);
            Unsafe.Add(ref buffer, index + 2) = (byte)('0' + v);

            left = DivMod10(left, out v);
            Unsafe.Add(ref buffer, index + 1) = (byte)('0' + v);

            left = DivMod10(left, out v);
            Unsafe.Add(ref buffer, index) = (byte)('0' + v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteFractionDigits(ulong value, ref byte buffer, int index)
        {
            //Precondition.Require(value >= 0 && value <= 9999);

            ulong left = DivMod10(value, out ulong v);
            Unsafe.Add(ref buffer, index + 6) = (byte)('0' + v);

            left = DivMod10(left, out v);
            Unsafe.Add(ref buffer, index + 5) = (byte)('0' + v);

            left = DivMod10(left, out v);
            Unsafe.Add(ref buffer, index + 4) = (byte)('0' + v);

            left = DivMod10(left, out v);
            Unsafe.Add(ref buffer, index + 3) = (byte)('0' + v);

            left = DivMod10(left, out v);
            Unsafe.Add(ref buffer, index + 2) = (byte)('0' + v);

            left = DivMod10(left, out v);
            Unsafe.Add(ref buffer, index + 1) = (byte)('0' + v);

            left = DivMod10(left, out v);
            Unsafe.Add(ref buffer, index) = (byte)('0' + v);
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
        public static void WriteTwoDigits(int value, ref char buffer, int index)
        {
            Precondition.Require(value >= 0 && value <= 99);

            ulong v1 = DivMod10((ulong)value, out ulong v2);
            Unsafe.Add(ref buffer, index) = (char)('0' + v1);
            Unsafe.Add(ref buffer, index + 1) = (char)('0' + v2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteFourDigits(int value, ref char buffer, int index)
        {
            Precondition.Require(value >= 0 && value <= 9999);

            ulong left = DivMod10((ulong)value, out ulong v);
            Unsafe.Add(ref buffer, index + 3) = (char)('0' + v);

            left = DivMod10(left, out v);
            Unsafe.Add(ref buffer, index + 2) = (char)('0' + v);

            left = DivMod10(left, out v);
            Unsafe.Add(ref buffer, index + 1) = (char)('0' + v);

            left = DivMod10(left, out v);
            Unsafe.Add(ref buffer, index) = (char)('0' + v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteFractionDigits(ulong value, ref char buffer, int index)
        {
            //Precondition.Require(value >= 0 && value <= 9999);

            ulong left = DivMod10(value, out ulong v);
            Unsafe.Add(ref buffer, index + 6) = (char)('0' + v);

            left = DivMod10(left, out v);
            Unsafe.Add(ref buffer, index + 5) = (char)('0' + v);

            left = DivMod10(left, out v);
            Unsafe.Add(ref buffer, index + 4) = (char)('0' + v);

            left = DivMod10(left, out v);
            Unsafe.Add(ref buffer, index + 3) = (char)('0' + v);

            left = DivMod10(left, out v);
            Unsafe.Add(ref buffer, index + 2) = (char)('0' + v);

            left = DivMod10(left, out v);
            Unsafe.Add(ref buffer, index + 1) = (char)('0' + v);

            left = DivMod10(left, out v);
            Unsafe.Add(ref buffer, index) = (char)('0' + v);
        }

        #endregion UTF-16 Helper methods

        /// <summary>
        /// This method for doing division and modulus for integers by 10 is ~5-6 times faster
        /// than the division and multiplication operators.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong DivMod10(ulong n, out ulong modulo)
        {
            ulong d = ((0x1999999A * n) >> 32);
            modulo = n - (d * 10);
            return d;
        }
    }
}
