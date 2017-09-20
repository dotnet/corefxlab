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
        public static T ReadLittleEndian<[Primitive]T>(this ReadOnlySpan<byte> buffer) where T : struct
            => BitConverter.IsLittleEndian ? buffer.Read<T>() : UnsafeUtilities.Reverse(buffer.Read<T>());

        /// <summary>
        /// Reads a structure of type <typeparamref name="T"/> out of a span of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ReadLittleEndian<[Primitive]T>(this Span<byte> buffer) where T : struct
            => BitConverter.IsLittleEndian ? buffer.Read<T>() : UnsafeUtilities.Reverse(buffer.Read<T>());

        #region ReadLittleEndianROSpan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReadLittleEndianInt16(this ReadOnlySpan<byte> buffer)
        {
            short result = buffer.Read<short>();
            if (!BitConverter.IsLittleEndian)
            {
                result = UnsafeUtilities.ReverseEndianness(result);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadLittleEndianInt32(this ReadOnlySpan<byte> buffer)
        {
            int result = buffer.Read<int>();
            if (!BitConverter.IsLittleEndian)
            {
                result = UnsafeUtilities.ReverseEndianness(result);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadLittleEndianInt64(this ReadOnlySpan<byte> buffer)
        {
            long result = buffer.Read<long>();
            if (!BitConverter.IsLittleEndian)
            {
                result = UnsafeUtilities.ReverseEndianness(result);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadLittleEndianUInt16(this ReadOnlySpan<byte> buffer)
        {
            ushort result = buffer.Read<ushort>();
            if (!BitConverter.IsLittleEndian)
            {
                result = UnsafeUtilities.ReverseEndianness(result);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadLittleEndianUInt32(this ReadOnlySpan<byte> buffer)
        {
            uint result = buffer.Read<uint>();
            if (!BitConverter.IsLittleEndian)
            {
                result = UnsafeUtilities.ReverseEndianness(result);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReadLittleEndianUInt64(this ReadOnlySpan<byte> buffer)
        {
            ulong result = buffer.Read<ulong>();
            if (!BitConverter.IsLittleEndian)
            {
                result = UnsafeUtilities.ReverseEndianness(result);
            }
            return result;
        }
        #endregion

        #region ReadLittleEndianSpan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReadLittleEndianInt16(this Span<byte> buffer)
        {
            short result = buffer.Read<short>();
            if (!BitConverter.IsLittleEndian)
            {
                result = UnsafeUtilities.ReverseEndianness(result);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadLittleEndianInt32(this Span<byte> buffer)
        {
            int result = buffer.Read<int>();
            if (!BitConverter.IsLittleEndian)
            {
                result = UnsafeUtilities.ReverseEndianness(result);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadLittleEndianInt64(this Span<byte> buffer)
        {
            long result = buffer.Read<long>();
            if (!BitConverter.IsLittleEndian)
            {
                result = UnsafeUtilities.ReverseEndianness(result);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadLittleEndianUInt16(this Span<byte> buffer)
        {
            ushort result = buffer.Read<ushort>();
            if (!BitConverter.IsLittleEndian)
            {
                result = UnsafeUtilities.ReverseEndianness(result);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadLittleEndianUInt32(this Span<byte> buffer)
        {
            uint result = buffer.Read<uint>();
            if (!BitConverter.IsLittleEndian)
            {
                result = UnsafeUtilities.ReverseEndianness(result);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReadLittleEndianUInt64(this Span<byte> buffer)
        {
            ulong result = buffer.Read<ulong>();
            if (!BitConverter.IsLittleEndian)
            {
                result = UnsafeUtilities.ReverseEndianness(result);
            }
            return result;
        }
        #endregion

        #region TryReadLittleEndianSpan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadLittleEndianInt16(this Span<byte> buffer, out short value)
        {
            bool success = buffer.TryRead(out value);
            if (!BitConverter.IsLittleEndian)
            {
                value = UnsafeUtilities.ReverseEndianness(value);
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadLittleEndianInt32(this Span<byte> buffer, out int value)
        {
            bool success = buffer.TryRead(out value);
            if (!BitConverter.IsLittleEndian)
            {
                value = UnsafeUtilities.ReverseEndianness(value);
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadLittleEndianInt64(this Span<byte> buffer, out long value)
        {
            bool success = buffer.TryRead(out value);
            if (!BitConverter.IsLittleEndian)
            {
                value = UnsafeUtilities.ReverseEndianness(value);
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadLittleEndianUInt16(this Span<byte> buffer, out ushort value)
        {
            bool success = buffer.TryRead(out value);
            if (!BitConverter.IsLittleEndian)
            {
                value = UnsafeUtilities.ReverseEndianness(value);
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadLittleEndianUInt32(this Span<byte> buffer, out uint value)
        {
            bool success = buffer.TryRead(out value);
            if (!BitConverter.IsLittleEndian)
            {
                value = UnsafeUtilities.ReverseEndianness(value);
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadLittleEndianUInt64(this Span<byte> buffer, out ulong value)
        {
            bool success = buffer.TryRead(out value);
            if (!BitConverter.IsLittleEndian)
            {
                value = UnsafeUtilities.ReverseEndianness(value);
            }
            return success;
        }
        #endregion

        #region TryReadLittleEndianROSpan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadLittleEndianInt16(this ReadOnlySpan<byte> buffer, out short value)
        {
            bool success = buffer.TryRead(out value);
            if (!BitConverter.IsLittleEndian)
            {
                value = UnsafeUtilities.ReverseEndianness(value);
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadLittleEndianInt32(this ReadOnlySpan<byte> buffer, out int value)
        {
            bool success = buffer.TryRead(out value);
            if (!BitConverter.IsLittleEndian)
            {
                value = UnsafeUtilities.ReverseEndianness(value);
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadLittleEndianInt64(this ReadOnlySpan<byte> buffer, out long value)
        {
            bool success = buffer.TryRead(out value);
            if (!BitConverter.IsLittleEndian)
            {
                value = UnsafeUtilities.ReverseEndianness(value);
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadLittleEndianUInt16(this ReadOnlySpan<byte> buffer, out ushort value)
        {
            bool success = buffer.TryRead(out value);
            if (!BitConverter.IsLittleEndian)
            {
                value = UnsafeUtilities.ReverseEndianness(value);
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadLittleEndianUInt32(this ReadOnlySpan<byte> buffer, out uint value)
        {
            bool success = buffer.TryRead(out value);
            if (!BitConverter.IsLittleEndian)
            {
                value = UnsafeUtilities.ReverseEndianness(value);
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadLittleEndianUInt64(this ReadOnlySpan<byte> buffer, out ulong value)
        {
            bool success = buffer.TryRead(out value);
            if (!BitConverter.IsLittleEndian)
            {
                value = UnsafeUtilities.ReverseEndianness(value);
            }
            return success;
        }
        #endregion
    }
}
