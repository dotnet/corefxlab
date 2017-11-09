// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using static System.Buffers.Binary.BinaryPrimitives;

namespace System.IO.Pipelines
{
    public static class ReadWriteExtensions
    {
        /// <summary>
        /// Reverses a primitive value - performs an endianness swap
        /// </summary> 
        private static unsafe T Reverse<[Primitive]T>(T value) where T : struct
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static T ReadBigEndian<[Primitive]T>(this Span<byte> buffer) where T : struct
            => BitConverter.IsLittleEndian ? Reverse(ReadMachineEndian<T>(buffer)) : ReadMachineEndian<T>(buffer);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static T ReadLittleEndian<[Primitive]T>(this Span<byte> buffer) where T : struct
            => BitConverter.IsLittleEndian ? ReadMachineEndian<T>(buffer) : Reverse(ReadMachineEndian<T>(buffer));


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void WriteBigEndian<[Primitive]T>(this Span<byte> buffer, T value) where T : struct
        {
            if (BitConverter.IsLittleEndian)
                value = Reverse(value);
            WriteMachineEndian(buffer, ref value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void WriteLittleEndian<[Primitive]T>(this Span<byte> buffer, T value) where T : struct
        {
            if (!BitConverter.IsLittleEndian)
                value = Reverse(value);
            WriteMachineEndian(buffer, ref value);
        }

        public static async Task<ReadableBuffer> ReadToEndAsync(this IPipeReader input)
        {
            while (true)
            {
                // Wait for more data
                var result = await input.ReadAsync();

                if (result.IsCompleted)
                {
                    // Read all the data, return it
                    return result.Buffer;
                }

                // Don't advance the buffer so remains in buffer
                input.Advance(result.Buffer.Start, result.Buffer.End);
            }
        }

        /// <summary>
        /// Reads a structure of type <typeparamref name="T"/> out of a buffer of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ReadBigEndian<[Primitive]T>(this ReadableBuffer buffer) where T : struct
        {
            var memory = buffer.First;
            int len = Unsafe.SizeOf<T>();
            var value = memory.Length >= len ? memory.Span.ReadBigEndian<T>() : ReadMultiBig<T>(buffer, len);
            return value;
        }

        /// <summary>
        /// Reads a structure of type <typeparamref name="T"/> out of a buffer of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ReadLittleEndian<[Primitive]T>(this ReadableBuffer buffer) where T : struct
        {
            var memory = buffer.First;
            int len = Unsafe.SizeOf<T>();
            var value = memory.Length >= len ? memory.Span.ReadLittleEndian<T>() : ReadMultiLittle<T>(buffer, len);
            return value;
        }

        private static T ReadMultiBig<[Primitive]T>(ReadableBuffer buffer, int len) where T : struct
        {
            Span<byte> localSpan = stackalloc byte[len];
            buffer.Slice(0, len).CopyTo(localSpan);
            return localSpan.ReadBigEndian<T>();
        }

        private static T ReadMultiLittle<[Primitive]T>(ReadableBuffer buffer, int len) where T : struct
        {
            Span<byte> localSpan = stackalloc byte[len];
            buffer.Slice(0, len).CopyTo(localSpan);
            return localSpan.ReadLittleEndian<T>();
        }

        /// <summary>
        /// Reads a structure of type T out of a buffer of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndian<[Primitive]T>(this WritableBuffer buffer, T value) where T : struct
        {
            int len = Unsafe.SizeOf<T>();
            buffer.Ensure(len);
            buffer.Buffer.Span.WriteBigEndian(value);
            buffer.Advance(len);
        }

        /// <summary>
        /// Reads a structure of type T out of a buffer of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteLittleEndian<[Primitive]T>(this WritableBuffer buffer, T value) where T : struct
        {
            int len = Unsafe.SizeOf<T>();
            buffer.Ensure(len);
            buffer.Buffer.Span.WriteLittleEndian(value);
            buffer.Advance(len);
        }
    }
}
