﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;

namespace System.Buffers.Text
{
    static class Parsers
    {
        public const int ByteOverflowLength = 3;
        public const int ByteOverflowLengthHex = 2;
        //public const int UInt16OverflowLength = 5;
        //public const int UInt16OverflowLengthHex = 4;
        //public const int UInt32OverflowLength = 10;
        //public const int UInt32OverflowLengthHex = 8;
        //public const int UInt64OverflowLength = 20;
        //public const int UInt64OverflowLengthHex = 16;

        //public const int SByteOverflowLength = 3;
        //public const int SByteOverflowLengthHex = 2;
        public const int Int16OverflowLength = 5;
        public const int Int16OverflowLengthHex = 4;
        public const int Int32OverflowLength = 10;
        public const int Int32OverflowLengthHex = 8;
        public const int Int64OverflowLength = 19;
        public const int Int64OverflowLengthHex = 16;

        //public const sbyte maxValueSbyteDiv10 = sbyte.MaxValue / 10;
        //public const short maxValueShortDiv10 = short.MaxValue / 10;
        public const int maxValueIntDiv10 = int.MaxValue / 10;
        public const long maxValueLongDiv10 = long.MaxValue / 10;

        public static readonly byte[] HexLookup =
        {
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,             // 15
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,             // 31
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,             // 47
            0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,                       // 63
            0xFF, 0xA, 0xB, 0xC, 0xD, 0xE, 0xF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,                   // 79
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,             // 95
            0xFF, 0xa, 0xb, 0xc, 0xd, 0xe, 0xf, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,                   // 111
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,             // 127
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,             // 143
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,             // 159
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,             // 175
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,             // 191
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,             // 207
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,             // 223
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,             // 239
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF              // 255
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsHexFormat(StandardFormat format)
        {
            return format.Symbol == 'X' || format.Symbol == 'x';
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValid(SymbolTable.Symbol symbol)
        {
            return symbol <= SymbolTable.Symbol.D9;
        }

        // If parsedValue > (int.MaxValue / 10), any more appended digits will cause overflow.
        // if parsedValue == (int.MaxValue / 10), any nextDigit greater than 7 or 8 (depending on sign) implies overflow.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WillOverFlow(int value, int nextDigit, int sign)
        {
            bool nextDigitTooLarge = nextDigit > 8 || (sign > 0 && nextDigit > 7);
            return (value > maxValueIntDiv10 || (value == maxValueIntDiv10 && nextDigitTooLarge));
        }
    }
}