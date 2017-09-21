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
        /// Reverses a primitive value - performs an endianness swap
        /// </summary> 
        public static unsafe T Reverse<[Primitive]T>(T value) where T : struct
        {
            // note: relying on JIT goodness here!
            if (typeof(T) == typeof(byte) || typeof(T) == typeof(sbyte))
            {
                return value;
            }
            else if (typeof(T) == typeof(ushort) || typeof(T) == typeof(short))
            {
                ushort val = 0;
                Unsafe.Write(&val, value);
                val = (ushort)((val >> 8) | (val << 8));
                return Unsafe.Read<T>(&val);
            }
            else if (typeof(T) == typeof(uint) || typeof(T) == typeof(int)
                || typeof(T) == typeof(float))
            {
                uint val = 0;
                Unsafe.Write(&val, value);
                val = (val << 24)
                    | ((val & 0xFF00) << 8)
                    | ((val & 0xFF0000) >> 8)
                    | (val >> 24);
                return Unsafe.Read<T>(&val);
            }
            else if (typeof(T) == typeof(ulong) || typeof(T) == typeof(long)
                || typeof(T) == typeof(double))
            {
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
            else
            {
                // default implementation
                int len = Unsafe.SizeOf<T>();
                var val = stackalloc byte[len];
                Unsafe.Write(val, value);
                int to = len >> 1, dest = len - 1;
                for (int i = 0; i < to; i++)
                {
                    var tmp = val[i];
                    val[i] = val[dest];
                    val[dest--] = tmp;
                }
                return Unsafe.Read<T>(val);
            }
        }

        /// <summary>
        /// Reads a structure of type T out of a slice of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<[Primitive]T>(this Span<byte> buffer)
            where T : struct
        {
            RequiresInInclusiveRange(Unsafe.SizeOf<T>(), (uint)buffer.Length);
            return Unsafe.ReadUnaligned<T>(ref buffer.DangerousGetPinnableReference());
        }

        /// <summary>
        /// Reads a structure of type T out of a slice of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<[Primitive]T>(this ReadOnlySpan<byte> buffer)
            where T : struct
        {
            RequiresInInclusiveRange(Unsafe.SizeOf<T>(), (uint)buffer.Length);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void RequiresInInclusiveRange(int start, uint length)
        {
            if ((uint)start > length)
            {
                throw new ArgumentOutOfRangeException();
            }
        }
    }
}
