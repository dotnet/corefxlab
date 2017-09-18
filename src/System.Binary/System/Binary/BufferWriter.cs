// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime;
using System.Runtime.CompilerServices;

namespace System.Buffers
{
    /// <summary>
    /// Writes endian-specific primitives into spans.
    /// </summary>
    /// <remarks>
    /// Use these helpers when you need to write specific endinaness.
    /// </remarks>
    public static partial class Binary
    {
        /// <summary>
        /// Writes a structure of type T into a slice of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<[Primitive]T>(this Span<byte> buffer, T value)
            where T : struct
        {
            if ((uint)Unsafe.SizeOf<T>() > (uint)buffer.Length)
            {
                throw new ArgumentOutOfRangeException();
            }
            Unsafe.WriteUnaligned<T>(ref buffer.DangerousGetPinnableReference(), value);
        }

        /// <summary>
        /// Writes a structure of type T into a slice of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWrite<[Primitive]T>(this Span<byte> buffer, T value)
            where T : struct
        {
            if (Unsafe.SizeOf<T>() > (uint)buffer.Length)
            {
                return false;
            }
            Unsafe.WriteUnaligned<T>(ref buffer.DangerousGetPinnableReference(), value);
            return true;
        }

        /// <summary>
        /// Writes a structure of type T to a span of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndian<[Primitive]T>(this Span<byte> buffer, T value) where T : struct
            => buffer.Write(BitConverter.IsLittleEndian ? UnsafeUtilities.Reverse(value) : value);

        /// <summary>
        /// Writes a structure of type T to a span of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteLittleEndian<[Primitive]T>(this Span<byte> buffer, T value) where T : struct
            => buffer.Write(BitConverter.IsLittleEndian ? value : UnsafeUtilities.Reverse(value));


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteLittleEndian<[Primitive]T>(this Span<byte> buffer, T value) where T : struct
        {
            return BitConverter.IsLittleEndian ? TryWrite(buffer, value) : TryWrite(buffer, UnsafeUtilities.Reverse(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteBigEndian<[Primitive]T>(this Span<byte> buffer, T value) where T : struct
        {
            return BitConverter.IsLittleEndian ? TryWrite(buffer, UnsafeUtilities.Reverse(value)) : TryWrite(buffer, value);
        }
    }
}
