// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime;
using System.Runtime.CompilerServices;

namespace System.Buffers
{
    /// <summary>
    /// Reads bytes as primitives with specific endianness
    /// </summary>
    /// <remarks>
    /// For native formats, SpanExtensions.Read&lt;T&gt; should be used.
    /// Use these helpers when you need to read specific endinanness.
    /// </remarks>
    public static partial class Binary
    {
        /// <summary>
        /// This is a no-op and added only for consistency.
        /// This allows the caller to read a struct of numeric primitives and reverse each field
        /// rather than having to skip byte & sbyte fields.
        /// </summary> 
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte Reverse(this sbyte value)
        {
            return value;
        }

        /// <summary>
        /// Reverses a primitive value - performs an endianness swap
        /// </summary> 
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short Reverse(this short value)
        {
            return (short)((value & 0x00FF) << 8 | (value & 0xFF00) >> 8);
        }

        /// <summary>
        /// Reverses a primitive value - performs an endianness swap
        /// </summary> 
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Reverse(this int value) => (int)Reverse((uint)value);

        /// <summary>
        /// Reverses a primitive value - performs an endianness swap
        /// </summary> 
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Reverse(this long value) => (long)Reverse((ulong)value);

        /// <summary>
        /// This is a no-op and added only for consistency.
        /// This allows the caller to read a struct of numeric primitives and reverse each field
        /// rather than having to skip byte & sbyte fields.
        /// </summary> 
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Reverse(this byte value)
        {
            return value;
        }

        /// <summary>
        /// Reverses a primitive value - performs an endianness swap
        /// </summary> 
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort Reverse(this ushort value)
        {
            return (ushort)((value & 0x00FFU) << 8 | (value & 0xFF00U) >> 8);
        }

        /// <summary>
        /// Reverses a primitive value - performs an endianness swap
        /// </summary> 
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Reverse(this uint value)
        {
            value = (value << 16) | (value >> 16);
            value = (value & 0x00FF00FF) << 8 | (value & 0xFF00FF00) >> 8;
            return value;
        }

        /// <summary>
        /// Reverses a primitive value - performs an endianness swap
        /// </summary> 
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Reverse(this ulong value)
        {
            value = (value << 32) | (value >> 32);
            value = (value & 0x0000FFFF0000FFFF) << 16 | (value & 0xFFFF0000FFFF0000) >> 16;
            value = (value & 0x00FF00FF00FF00FF) << 8 | (value & 0xFF00FF00FF00FF00) >> 8;
            return value;
        }

        /// <summary>
        /// Reads a structure of type T out of a slice of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<[Primitive]T>(this Span<byte> buffer)
            where T : struct
        {
            if (Unsafe.SizeOf<T>() > buffer.Length)
            {
                throw new ArgumentOutOfRangeException();
            }
            return Unsafe.ReadUnaligned<T>(ref buffer.DangerousGetPinnableReference());
        }

        /// <summary>
        /// Reads a structure of type T out of a slice of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<[Primitive]T>(this ReadOnlySpan<byte> buffer)
            where T : struct
        {
            if (Unsafe.SizeOf<T>() > buffer.Length)
            {
                throw new ArgumentOutOfRangeException();
            }
            return Unsafe.ReadUnaligned<T>(ref buffer.DangerousGetPinnableReference());
        }

        /// <summary>
        /// Reads a structure of type T out of a slice of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead<[Primitive]T>(this ReadOnlySpan<byte> buffer, out T value)
            where T : struct
        {
            if (Unsafe.SizeOf<T>() > (uint)buffer.Length)
            {
                value = default;
                return false;
            }
            value = Unsafe.ReadUnaligned<T>(ref buffer.DangerousGetPinnableReference());
            return true;
        }

        /// <summary>
        /// Reads a structure of type T out of a slice of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead<[Primitive]T>(this Span<byte> buffer, out T value)
            where T : struct
        {
            if (Unsafe.SizeOf<T>() > (uint)buffer.Length)
            {
                value = default;
                return false;
            }
            value = Unsafe.ReadUnaligned<T>(ref buffer.DangerousGetPinnableReference());
            return true;
        }
    }
}
