// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.IO;

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
    }
}
