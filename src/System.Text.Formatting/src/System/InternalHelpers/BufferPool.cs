// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.IO
{
    internal static class BufferPool
    {
        // TODO: implement a real pool
        // Maybe the pool should support renting Span<byte> instead of byte[]. This would make it cheaper to have many small buffers.
        public static byte[] RentBuffer(int minSize)
        {
            Precondition.Require(minSize > 0);
            return new byte[minSize];
        }

        public static void ReturnBuffer(ref byte[] buffer)
        {
            Precondition.Require(buffer != null);
            buffer = null;
        }

        public static void Enlarge(ref byte[] buffer, int minSize)
        {
            Precondition.Require(minSize > buffer.Length);

            byte[] newBuffer = BufferPool.RentBuffer(minSize);
            buffer.CopyTo(newBuffer, 0);
            BufferPool.ReturnBuffer(ref buffer);
            buffer = newBuffer;
        }
    }
}
