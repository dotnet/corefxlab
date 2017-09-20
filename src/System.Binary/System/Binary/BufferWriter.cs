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

        #region WriteBigEndianSpan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndianInt16(this Span<byte> buffer, short value)
        {
            buffer.Write(BitConverter.IsLittleEndian ? UnsafeUtilities.ReverseEndianness(value) : value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndianInt32(this Span<byte> buffer, int value)
        {
            buffer.Write(BitConverter.IsLittleEndian ? UnsafeUtilities.ReverseEndianness(value) : value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndianInt64(this Span<byte> buffer, long value)
        {
            buffer.Write(BitConverter.IsLittleEndian ? UnsafeUtilities.ReverseEndianness(value) : value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndianUInt16(this Span<byte> buffer, ushort value)
        {
            buffer.Write(BitConverter.IsLittleEndian ? UnsafeUtilities.ReverseEndianness(value) : value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndianUInt32(this Span<byte> buffer, uint value)
        {
            buffer.Write(BitConverter.IsLittleEndian ? UnsafeUtilities.ReverseEndianness(value) : value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndianUInt64(this Span<byte> buffer, ulong value)
        {
            buffer.Write(BitConverter.IsLittleEndian ? UnsafeUtilities.ReverseEndianness(value) : value);
        }
        #endregion

        #region WriteLittleEndianSpan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteLittleEndianInt16(this Span<byte> buffer, short value)
        {
            buffer.Write(BitConverter.IsLittleEndian ? value : UnsafeUtilities.ReverseEndianness(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteLittleEndianInt32(this Span<byte> buffer, int value)
        {
            buffer.Write(BitConverter.IsLittleEndian ? value : UnsafeUtilities.ReverseEndianness(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteLittleEndianInt64(this Span<byte> buffer, long value)
        {
            buffer.Write(BitConverter.IsLittleEndian ? value : UnsafeUtilities.ReverseEndianness(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteLittleEndianUInt16(this Span<byte> buffer, ushort value)
        {
            buffer.Write(BitConverter.IsLittleEndian ? value : UnsafeUtilities.ReverseEndianness(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteLittleEndianUInt32(this Span<byte> buffer, uint value)
        {
            buffer.Write(BitConverter.IsLittleEndian ? value : UnsafeUtilities.ReverseEndianness(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteLittleEndianUInt64(this Span<byte> buffer, ulong value)
        {
            buffer.Write(BitConverter.IsLittleEndian ? value : UnsafeUtilities.ReverseEndianness(value));
        }
        #endregion

        #region TryWriteBigEndianSpan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteBigEndianInt16(this Span<byte> buffer, short value)
        {
            return buffer.TryWrite(BitConverter.IsLittleEndian ? UnsafeUtilities.ReverseEndianness(value) : value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteBigEndianInt32(this Span<byte> buffer, int value)
        {
            return buffer.TryWrite(BitConverter.IsLittleEndian ? UnsafeUtilities.ReverseEndianness(value) : value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteBigEndianInt64(this Span<byte> buffer, long value)
        {
            return buffer.TryWrite(BitConverter.IsLittleEndian ? UnsafeUtilities.ReverseEndianness(value) : value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteBigEndianUInt16(this Span<byte> buffer, ushort value)
        {
            return buffer.TryWrite(BitConverter.IsLittleEndian ? UnsafeUtilities.ReverseEndianness(value) : value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteBigEndianUInt32(this Span<byte> buffer, uint value)
        {
            return buffer.TryWrite(BitConverter.IsLittleEndian ? UnsafeUtilities.ReverseEndianness(value) : value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteBigEndianUInt64(this Span<byte> buffer, ulong value)
        {
            return buffer.TryWrite(BitConverter.IsLittleEndian ? UnsafeUtilities.ReverseEndianness(value) : value);
        }
        #endregion

        #region TryWriteLittleEndianSpan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteLittleEndianInt16(this Span<byte> buffer, short value)
        {
            return buffer.TryWrite(BitConverter.IsLittleEndian ? value : UnsafeUtilities.ReverseEndianness(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteLittleEndianInt32(this Span<byte> buffer, int value)
        {
            return buffer.TryWrite(BitConverter.IsLittleEndian ? value : UnsafeUtilities.ReverseEndianness(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteLittleEndianInt64(this Span<byte> buffer, long value)
        {
            return buffer.TryWrite(BitConverter.IsLittleEndian ? value : UnsafeUtilities.ReverseEndianness(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteLittleEndianUInt16(this Span<byte> buffer, ushort value)
        {
            return buffer.TryWrite(BitConverter.IsLittleEndian ? value : UnsafeUtilities.ReverseEndianness(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteLittleEndianUInt32(this Span<byte> buffer, uint value)
        {
            return buffer.TryWrite(BitConverter.IsLittleEndian ? value : UnsafeUtilities.ReverseEndianness(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteLittleEndianUInt64(this Span<byte> buffer, ulong value)
        {
            return buffer.TryWrite(BitConverter.IsLittleEndian ? value : UnsafeUtilities.ReverseEndianness(value));
        }
        #endregion
    }
}
