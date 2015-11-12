// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Buffers
{
    public unsafe sealed class NativeBufferPool<T> where T : struct
    {
        private const int NUMBER_OF_BUFFERS_IN_BUCKET = 50;
        private int _maxElements;
        private NativeBufferBucket<T>[] _buckets;

        public NativeBufferPool(int maxElementsInBuffer)
        {
            int maxBuckets = BufferUtilities.SelectBucketIndex(maxElementsInBuffer);
            _maxElements = maxElementsInBuffer;
            _buckets = new NativeBufferBucket<T>[maxBuckets + 1];
        }

        public Span<T> RentBuffer(int numberOfElements, bool clearBuffer = false)
        {
            Span<T> buffer;
            if (numberOfElements > _maxElements)
            {
#if DEBUG
                System.Diagnostics.Debugger.Break();
#endif
                buffer = new Span<T>(Marshal.AllocHGlobal(numberOfElements * Marshal.SizeOf(typeof(T))).ToPointer(), numberOfElements);
            }
            else
            {
                int index = BufferUtilities.SelectBucketIndex(numberOfElements);
                NativeBufferBucket<T> bucket = Volatile.Read(ref _buckets[index]);
                if (bucket == null)
                {
                    Interlocked.CompareExchange(ref _buckets[index], new NativeBufferBucket<T>(BufferUtilities.GetMaxSizeForBucket(index), NUMBER_OF_BUFFERS_IN_BUCKET), null);
                    bucket = _buckets[index];
                }

                buffer = bucket.Rent();
            }

            if (clearBuffer) BufferUtilities.ClearSpan<T>(buffer);
            return buffer;
        }

        public void EnlargeBuffer(ref Span<T> buffer, int newSize, bool clearFreeSpace = false)
        {
            // Still need to figure out how to handle the resizing without allocating more
            // spans to put into the resized-span's original bucket
            throw new NotImplementedException();
        }

        public void ReturnBuffer(ref Span<T> buffer, bool clearBuffer = false)
        {
            if (clearBuffer) BufferUtilities.ClearSpan<T>(buffer);

            if (buffer.Length > _maxElements)
            {
                Marshal.FreeHGlobal(new IntPtr(buffer.UnsafePointer));
                buffer = default(Span<T>);
            }
            else
            {
                _buckets[BufferUtilities.SelectBucketIndex(buffer.Length)].Return(ref buffer);
            }
        }
    }
}
