// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Buffers
{
    [Obsolete("Use ArrayPool<T> instead. It can be found in CoreFx System.Buffers package.")]
    public sealed class ManagedBufferPool : BufferPool
    {
        static ManagedBufferPool s_shared = new ManagedBufferPool();

        public static ManagedBufferPool Shared
        {
            get
            {
                return s_shared;
            }
        }

        public override Bytes Rent(int minimumBufferSize)
        {
            var array = ArrayPool<byte>.Shared.Rent(minimumBufferSize);
            return new Bytes(new ArraySegment<byte>(array, 0, array.Length));
        }

        public override void Return(Bytes buffer)
        {
            var span = (Span<byte>)buffer;
            ArraySegment<byte> segment;
            unsafe
            {
                void* p;          
                if(!span.TryGetArrayElseGetPointer(out segment, out p)) {
                    throw new Exception("this buffer was not rented from this pool.");
                }
            }
            ArrayPool<byte>.Shared.Return(segment.Array);
        }

        protected override void Dispose(bool disposing)
        {
            throw new NotImplementedException();
        }
    }
}
