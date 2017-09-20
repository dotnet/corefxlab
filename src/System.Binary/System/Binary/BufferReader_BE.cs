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

        #region ReadBigEndianROSpan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReadBigEndianInt16(this ReadOnlySpan<byte> buffer)
        {
            short result = buffer.Read<short>();
            if (BitConverter.IsLittleEndian)
            {
                result = UnsafeUtilities.ReverseEndianness(result);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadBigEndianInt32(this ReadOnlySpan<byte> buffer)
        {
            int result = buffer.Read<int>();
            if (BitConverter.IsLittleEndian)
            {
                result = UnsafeUtilities.ReverseEndianness(result);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadBigEndianInt64(this ReadOnlySpan<byte> buffer)
        {
            long result = buffer.Read<long>();
            if (BitConverter.IsLittleEndian)
            {
                result = UnsafeUtilities.ReverseEndianness(result);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadBigEndianUInt16(this ReadOnlySpan<byte> buffer)
        {
            ushort result = buffer.Read<ushort>();
            if (BitConverter.IsLittleEndian)
            {
                result = UnsafeUtilities.ReverseEndianness(result);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadBigEndianUInt32(this ReadOnlySpan<byte> buffer)
        {
            uint result = buffer.Read<uint>();
            if (BitConverter.IsLittleEndian)
            {
                result = UnsafeUtilities.ReverseEndianness(result);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReadBigEndianUInt64(this ReadOnlySpan<byte> buffer)
        {
            ulong result = buffer.Read<ulong>();
            if (BitConverter.IsLittleEndian)
            {
                result = UnsafeUtilities.ReverseEndianness(result);
            }
            return result;
        }
        #endregion

        #region ReadBigEndianSpan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReadBigEndianInt16(this Span<byte> buffer)
        {
            short result = buffer.Read<short>();
            if (BitConverter.IsLittleEndian)
            {
                result = UnsafeUtilities.ReverseEndianness(result);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadBigEndianInt32(this Span<byte> buffer)
        {
            int result = buffer.Read<int>();
            if (BitConverter.IsLittleEndian)
            {
                result = UnsafeUtilities.ReverseEndianness(result);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadBigEndianInt64(this Span<byte> buffer)
        {
            long result = buffer.Read<long>();
            if (BitConverter.IsLittleEndian)
            {
                result = UnsafeUtilities.ReverseEndianness(result);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadBigEndianUInt16(this Span<byte> buffer)
        {
            ushort result = buffer.Read<ushort>();
            if (BitConverter.IsLittleEndian)
            {
                result = UnsafeUtilities.ReverseEndianness(result);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadBigEndianUInt32(this Span<byte> buffer)
        {
            uint result = buffer.Read<uint>();
            if (BitConverter.IsLittleEndian)
            {
                result = UnsafeUtilities.ReverseEndianness(result);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReadBigEndianUInt64(this Span<byte> buffer)
        {
            ulong result = buffer.Read<ulong>();
            if (BitConverter.IsLittleEndian)
            {
                result = UnsafeUtilities.ReverseEndianness(result);
            }
            return result;
        }
        #endregion

        #region TryReadBigEndianSpan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBigEndianInt16(this Span<byte> buffer, out short value)
        {
            bool success = buffer.TryRead(out value);
            if (BitConverter.IsLittleEndian)
            {
                value = UnsafeUtilities.ReverseEndianness(value);
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBigEndianInt32(this Span<byte> buffer, out int value)
        {
            bool success = buffer.TryRead(out value);
            if (BitConverter.IsLittleEndian)
            {
                value = UnsafeUtilities.ReverseEndianness(value);
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBigEndianInt64(this Span<byte> buffer, out long value)
        {
            bool success = buffer.TryRead(out value);
            if (BitConverter.IsLittleEndian)
            {
                value = UnsafeUtilities.ReverseEndianness(value);
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBigEndianUInt16(this Span<byte> buffer, out ushort value)
        {
            bool success = buffer.TryRead(out value);
            if (BitConverter.IsLittleEndian)
            {
                value = UnsafeUtilities.ReverseEndianness(value);
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBigEndianUInt32(this Span<byte> buffer, out uint value)
        {
            bool success = buffer.TryRead(out value);
            if (BitConverter.IsLittleEndian)
            {
                value = UnsafeUtilities.ReverseEndianness(value);
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBigEndianUInt64(this Span<byte> buffer, out ulong value)
        {
            bool success = buffer.TryRead(out value);
            if (BitConverter.IsLittleEndian)
            {
                value = UnsafeUtilities.ReverseEndianness(value);
            }
            return success;
        }
        #endregion

        #region TryReadBigEndianROSpan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBigEndianInt16(this ReadOnlySpan<byte> buffer, out short value)
        {
            bool success = buffer.TryRead(out value);
            if (BitConverter.IsLittleEndian)
            {
                value = UnsafeUtilities.ReverseEndianness(value);
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBigEndianInt32(this ReadOnlySpan<byte> buffer, out int value)
        {
            bool success = buffer.TryRead(out value);
            if (BitConverter.IsLittleEndian)
            {
                value = UnsafeUtilities.ReverseEndianness(value);
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBigEndianInt64(this ReadOnlySpan<byte> buffer, out long value)
        {
            bool success = buffer.TryRead(out value);
            if (BitConverter.IsLittleEndian)
            {
                value = UnsafeUtilities.ReverseEndianness(value);
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBigEndianUInt16(this ReadOnlySpan<byte> buffer, out ushort value)
        {
            bool success = buffer.TryRead(out value);
            if (BitConverter.IsLittleEndian)
            {
                value = UnsafeUtilities.ReverseEndianness(value);
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBigEndianUInt32(this ReadOnlySpan<byte> buffer, out uint value)
        {
            bool success = buffer.TryRead(out value);
            if (BitConverter.IsLittleEndian)
            {
                value = UnsafeUtilities.ReverseEndianness(value);
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadBigEndianUInt64(this ReadOnlySpan<byte> buffer, out ulong value)
        {
            bool success = buffer.TryRead(out value);
            if (BitConverter.IsLittleEndian)
            {
                value = UnsafeUtilities.ReverseEndianness(value);
            }
            return success;
        }
        #endregion
    }
}
