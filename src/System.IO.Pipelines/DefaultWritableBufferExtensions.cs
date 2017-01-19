// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Binary;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Common extension methods against writable buffers
    /// </summary>
    public static class DefaultWritableBufferExtensions
    {
        /// <summary>
        /// Writes the source <see cref="Span{Byte}"/> to the <see cref="WritableBuffer"/>.
        /// </summary>
        /// <param name="buffer">The <see cref="WritableBuffer"/></param>
        /// <param name="source">The <see cref="Span{Byte}"/> to write</param>
        public static void Write(this WritableBuffer buffer, ReadOnlySpan<byte> source)
        {
            if (buffer.Memory.IsEmpty)
            {
                buffer.Ensure();
            }

            // Fast path, try copying to the available memory directly
            if (source.Length <= buffer.Memory.Length)
            {
                source.CopyTo(buffer.Memory.Span);
                buffer.Advance(source.Length);
                return;
            }

            var remaining = source.Length;
            var offset = 0;

            while (remaining > 0)
            {
                var writable = Math.Min(remaining, buffer.Memory.Length);

                buffer.Ensure(writable);

                if (writable == 0)
                {
                    continue;
                }

                source.Slice(offset, writable).CopyTo(buffer.Memory.Span);

                remaining -= writable;
                offset += writable;

                buffer.Advance(writable);
            }
        }

        /// <summary>
        /// Reads a structure of type T out of a buffer of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndian<[Primitive]T>(this WritableBuffer buffer, T value) where T : struct
        {
            int len = Unsafe.SizeOf<T>();
            buffer.Ensure(len);
            buffer.Memory.Span.WriteBigEndian(value);
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
            buffer.Memory.Span.WriteLittleEndian(value);
            buffer.Advance(len);
        }
    }
}
