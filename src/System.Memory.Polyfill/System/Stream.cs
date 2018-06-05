// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace System
{
    public static partial class MemoryPolyfill
    {
        public static int Read(this Stream stream, Span<byte> buffer)
        {
#if NETCOREAPP2_1
             return stream.Read(buffer);
#else
            byte[] pooled = null;
            try
            {
                pooled = ArrayPool<byte>.Shared.Rent(buffer.Length);
                int read = stream.Read(pooled, 0, pooled.Length);
                pooled.AsSpan(0, read).CopyTo(buffer);
                return read;
            }
            finally
            {
                if(pooled != null) ArrayPool<byte>.Shared.Return(pooled);
            }
#endif
        }

        public static async ValueTask<int> ReadAsync(this Stream stream, Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
#if NETCOREAPP2_1
            return await stream.ReadAsync(buffer, cancellationToken);
#else
            if (MemoryMarshal.TryGetArray(buffer, out ArraySegment<byte> array))
            {
                return await stream.ReadAsync(array.Array, array.Offset, array.Count, cancellationToken);
            }
            else
            {
                byte[] pooled = null;
                try
                {
                    pooled = ArrayPool<byte>.Shared.Rent(buffer.Length);
                    int read = await stream.ReadAsync(pooled, 0, pooled.Length, cancellationToken);
                    pooled.AsSpan(0, read).CopyTo(buffer.Span);
                    return read;
                }
                finally
                {
                    if (pooled != null) ArrayPool<byte>.Shared.Return(pooled);
                }
            }
#endif
        }

        public static void Write(this Stream stream, ReadOnlySpan<byte> buffer)
        {
#if NETCOREAPP2_1
            stream.Write(buffer);
#else
            byte[] pooled = null;
            try
            {
                pooled = ArrayPool<byte>.Shared.Rent(buffer.Length);
                buffer.CopyTo(pooled);
                stream.Write(pooled, 0, pooled.Length);
            }
            finally
            {
                if(pooled != null) ArrayPool<byte>.Shared.Return(pooled);
            }
#endif
        }

        public static Task WriteAsync(this Stream stream, ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
#if NETCOREAPP2_1
            return stream.WriteAsync(buffer, cancellationToken).AsTask();
#else
            if (MemoryMarshal.TryGetArray(buffer, out ArraySegment<byte> array))
            {
                return stream.WriteAsync(array.Array, array.Offset, array.Count, cancellationToken);
            }
            else
            {
                byte[] pooled = null;
                try
                {
                    pooled = ArrayPool<byte>.Shared.Rent(buffer.Length);
                    buffer.CopyTo(pooled);
                    return stream.WriteAsync(pooled, 0, pooled.Length, cancellationToken);
                }
                finally
                {
                    if (pooled != null) ArrayPool<byte>.Shared.Return(pooled);
                }
            }
#endif
        }
    }
}
