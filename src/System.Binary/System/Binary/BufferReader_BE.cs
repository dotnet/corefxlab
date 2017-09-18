// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime;
using System.Runtime.CompilerServices;

namespace System.Buffers
{
    public static partial class Binary
    {
        /// <summary>
        /// Reads a structure of type <typeparamref name="T"/> out of a span of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ReadBigEndian<[Primitive]T>(this ReadOnlySpan<byte> buffer) where T : struct
            => BitConverter.IsLittleEndian ? UnsafeUtilities.Reverse(buffer.Read<T>()) : buffer.Read<T>();

        /// <summary>
        /// Reads a structure of type <typeparamref name="T"/> out of a span of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ReadBigEndian<[Primitive]T>(this Span<byte> buffer) where T : struct
            => BitConverter.IsLittleEndian ? UnsafeUtilities.Reverse(buffer.Read<T>()) : buffer.Read<T>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBigEndiann<[Primitive]T>(this ReadOnlySpan<byte> buffer, out T value) where T : struct
        {
            if (!BitConverter.IsLittleEndian) return buffer.TryRead(out value);
            if (!buffer.TryRead(out value)) return false;
            value = UnsafeUtilities.Reverse(value);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBigEndian<[Primitive]T>(this Span<byte> buffer, out T value) where T : struct
        {
            if (!BitConverter.IsLittleEndian) return buffer.TryRead(out value);
            if (!buffer.TryRead(out value)) return false;
            value = UnsafeUtilities.Reverse(value);
            return true;
        }
    }
}
