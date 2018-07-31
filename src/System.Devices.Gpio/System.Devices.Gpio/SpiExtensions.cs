// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Devices.Gpio
{
    public static class SpiExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this SpiDevice device, params byte[] values)
        {
            device.Write(values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteInt8(this SpiDevice device, byte value)
        {
            device.Write(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteInt16(this SpiDevice device, ushort value)
        {
            device.Write(BitConverter.GetBytes(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteInt32(this SpiDevice device, uint value)
        {
            device.Write(BitConverter.GetBytes(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteInt64(this SpiDevice device, ulong value)
        {
            device.Write(BitConverter.GetBytes(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadInt8(this SpiDevice device)
        {
            return (byte)device.Read(1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadInt16(this SpiDevice device)
        {
            return (ushort)device.Read(2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadInt32(this SpiDevice device)
        {
            return (uint)device.Read(4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReadInt64(this SpiDevice device)
        {
            return device.Read(8);
        }

        public static ulong Read(this SpiDevice device, uint bytes)
        {
            if (bytes > sizeof(ulong))
            {
                throw new ArgumentOutOfRangeException($"Value of {bytes} parameter cannot be greater than 8");
            }

            var buffer = new byte[bytes];
            device.Read(buffer);
            return Utils.ValueFromBuffer(buffer);
        }
    }
}
