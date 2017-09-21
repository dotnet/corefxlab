// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime;
using System.Runtime.CompilerServices;

namespace System.Buffers
{
    public static partial class Binary
    {
        #region ReadBigEndianROSpan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReadInt16BigEndian(this ReadOnlySpan<byte> buffer)
        {
            short result = buffer.Read<short>();
            if (BitConverter.IsLittleEndian)
            {
                result = result.Reverse();
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadInt32BigEndian(this ReadOnlySpan<byte> buffer)
        {
            int result = buffer.Read<int>();
            if (BitConverter.IsLittleEndian)
            {
                result = result.Reverse();
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadInt64BigEndian(this ReadOnlySpan<byte> buffer)
        {
            long result = buffer.Read<long>();
            if (BitConverter.IsLittleEndian)
            {
                result = result.Reverse();
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadUInt16BigEndian(this ReadOnlySpan<byte> buffer)
        {
            ushort result = buffer.Read<ushort>();
            if (BitConverter.IsLittleEndian)
            {
                result = result.Reverse();
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadUInt32BigEndian(this ReadOnlySpan<byte> buffer)
        {
            uint result = buffer.Read<uint>();
            if (BitConverter.IsLittleEndian)
            {
                result = result.Reverse();
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReadUInt64BigEndian(this ReadOnlySpan<byte> buffer)
        {
            ulong result = buffer.Read<ulong>();
            if (BitConverter.IsLittleEndian)
            {
                result = result.Reverse();
            }
            return result;
        }
        #endregion

        #region ReadBigEndianSpan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReadInt16BigEndian(this Span<byte> buffer)
        {
            short result = buffer.Read<short>();
            if (BitConverter.IsLittleEndian)
            {
                result = result.Reverse();
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadInt32BigEndian(this Span<byte> buffer)
        {
            int result = buffer.Read<int>();
            if (BitConverter.IsLittleEndian)
            {
                result = result.Reverse();
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadInt64BigEndian(this Span<byte> buffer)
        {
            long result = buffer.Read<long>();
            if (BitConverter.IsLittleEndian)
            {
                result = result.Reverse();
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadUInt16BigEndian(this Span<byte> buffer)
        {
            ushort result = buffer.Read<ushort>();
            if (BitConverter.IsLittleEndian)
            {
                result = result.Reverse();
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadUInt32BigEndian(this Span<byte> buffer)
        {
            uint result = buffer.Read<uint>();
            if (BitConverter.IsLittleEndian)
            {
                result = result.Reverse();
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReadUInt64BigEndian(this Span<byte> buffer)
        {
            ulong result = buffer.Read<ulong>();
            if (BitConverter.IsLittleEndian)
            {
                result = result.Reverse();
            }
            return result;
        }
        #endregion

        #region TryReadBigEndianSpan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadInt16BigEndian(this Span<byte> buffer, out short value)
        {
            bool success = buffer.TryRead(out value);
            if (BitConverter.IsLittleEndian)
            {
                value = value.Reverse();
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadInt32BigEndian(this Span<byte> buffer, out int value)
        {
            bool success = buffer.TryRead(out value);
            if (BitConverter.IsLittleEndian)
            {
                value = value.Reverse();
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadInt64BigEndian(this Span<byte> buffer, out long value)
        {
            bool success = buffer.TryRead(out value);
            if (BitConverter.IsLittleEndian)
            {
                value = value.Reverse();
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadUInt16BigEndian(this Span<byte> buffer, out ushort value)
        {
            bool success = buffer.TryRead(out value);
            if (BitConverter.IsLittleEndian)
            {
                value = value.Reverse();
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadUInt32BigEndian(this Span<byte> buffer, out uint value)
        {
            bool success = buffer.TryRead(out value);
            if (BitConverter.IsLittleEndian)
            {
                value = value.Reverse();
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadUInt64BigEndian(this Span<byte> buffer, out ulong value)
        {
            bool success = buffer.TryRead(out value);
            if (BitConverter.IsLittleEndian)
            {
                value = value.Reverse();
            }
            return success;
        }
        #endregion

        #region TryReadBigEndianROSpan
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadInt16BigEndian(this ReadOnlySpan<byte> buffer, out short value)
        {
            bool success = buffer.TryRead(out value);
            if (BitConverter.IsLittleEndian)
            {
                value = value.Reverse();
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadInt32BigEndian(this ReadOnlySpan<byte> buffer, out int value)
        {
            bool success = buffer.TryRead(out value);
            if (BitConverter.IsLittleEndian)
            {
                value = value.Reverse();
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadInt64BigEndian(this ReadOnlySpan<byte> buffer, out long value)
        {
            bool success = buffer.TryRead(out value);
            if (BitConverter.IsLittleEndian)
            {
                value = value.Reverse();
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadUInt16BigEndian(this ReadOnlySpan<byte> buffer, out ushort value)
        {
            bool success = buffer.TryRead(out value);
            if (BitConverter.IsLittleEndian)
            {
                value = value.Reverse();
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadUInt32BigEndian(this ReadOnlySpan<byte> buffer, out uint value)
        {
            bool success = buffer.TryRead(out value);
            if (BitConverter.IsLittleEndian)
            {
                value = value.Reverse();
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadUInt64BigEndian(this ReadOnlySpan<byte> buffer, out ulong value)
        {
            bool success = buffer.TryRead(out value);
            if (BitConverter.IsLittleEndian)
            {
                value = value.Reverse();
            }
            return success;
        }
        #endregion
    }
}
