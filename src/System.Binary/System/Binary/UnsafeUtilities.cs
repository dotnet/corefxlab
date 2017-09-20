// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Runtime
{
    /// <summary>
    /// A collection of unsafe helper methods that we cannot implement in C#.
    /// NOTE: these can be used for VeryBadThings(tm), so tread with care...
    /// </summary>
    internal static class UnsafeUtilities
    {
        /// <summary>
        /// Reverses a primitive value - performs an endianness swap
        /// </summary> 
        public static unsafe T Reverse<[Primitive]T>(T value) where T : struct
        {
            // note: relying on JIT goodness here!
            if (typeof(T) == typeof(byte) || typeof(T) == typeof(sbyte)) {
                return value;
            }
            else if (typeof(T) == typeof(ushort) || typeof(T) == typeof(short)) {
                ushort val = 0;
                Unsafe.Write(&val, value);
                val = (ushort)((val >> 8) | (val << 8));
                return Unsafe.Read<T>(&val);
            }
            else if (typeof(T) == typeof(uint) || typeof(T) == typeof(int)
                || typeof(T) == typeof(float)) {
                uint val = 0;
                Unsafe.Write(&val, value);
                val = (val << 24)
                    | ((val & 0xFF00) << 8)
                    | ((val & 0xFF0000) >> 8)
                    | (val >> 24);
                return Unsafe.Read<T>(&val);
            }
            else if (typeof(T) == typeof(ulong) || typeof(T) == typeof(long)
                || typeof(T) == typeof(double)) {
                ulong val = 0;
                Unsafe.Write(&val, value);
                val = (val << 56)
                    | ((val & 0xFF00) << 40)
                    | ((val & 0xFF0000) << 24)
                    | ((val & 0xFF000000) << 8)
                    | ((val & 0xFF00000000) >> 8)
                    | ((val & 0xFF0000000000) >> 24)
                    | ((val & 0xFF000000000000) >> 40)
                    | (val >> 56);
                return Unsafe.Read<T>(&val);
            }
            else {
                // default implementation
                int len = Unsafe.SizeOf<T>();
                var val = stackalloc byte[len];
                Unsafe.Write(val, value);
                int to = len >> 1, dest = len - 1;
                for (int i = 0; i < to; i++) {
                    var tmp = val[i];
                    val[i] = val[dest];
                    val[dest--] = tmp;
                }
                return Unsafe.Read<T>(val);
            }
        }

        public static short ReverseEndianness(short value)
        {
            return (short)(value << 8 | value >> 8);
        }

        public static int ReverseEndianness(int value)
        {
            return (value & 0x000000FF) << 24 |
                (value & 0x0000FF00) << 8 |
                (value & 0x00FF0000) >> 8 |
                (int)(((uint)value & 0xFF000000) >> 24);
        }

        public static long ReverseEndianness(long value)
        {
            return (value & 0x00000000000000FFL) << 56 |
                (value & 0x000000000000FF00L) << 40 |
                (value & 0x0000000000FF0000L) << 24 |
                (value & 0x00000000FF000000L) << 8 |
                (value & 0x000000FF00000000L) >> 8 |
                (value & 0x0000FF0000000000L) >> 24 |
                (value & 0x00FF000000000000L) >> 40 |
                (long)(((ulong)value & 0xFF00000000000000L) >> 56);
        }

        public static ushort ReverseEndianness(ushort value)
        {
            return (ushort)(value << 8 | value >> 8);
        }

        public static uint ReverseEndianness(uint value)
        {
            return (value & 0x000000FFU) << 24 |
                (value & 0x0000FF00U) << 8 |
                (value & 0x00FF0000U) >> 8 |
                (value & 0xFF000000U) >> 24;
        }

        public static ulong ReverseEndianness(ulong value)
        {
            return (value & 0x00000000000000FFUL) << 56 |
                (value & 0x000000000000FF00UL) << 40 |
                (value & 0x0000000000FF0000UL) << 24 |
                (value & 0x00000000FF000000UL) << 8 |
                (value & 0x000000FF00000000UL) >> 8 |
                (value & 0x0000FF0000000000UL) >> 24 |
                (value & 0x00FF000000000000UL) >> 40 |
                (value & 0xFF00000000000000UL) >> 56;
        }
    }
}
