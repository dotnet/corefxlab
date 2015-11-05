// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Buffers
{
    public sealed class BufferPool<T> where T : struct
    {
        private const int NUMBER_OF_BUFFERS_IN_BUCKET = 50;
        private int _maxBufferSize;
        private BufferBucket<T>[] _buckets;

        // 2MB taken as the default since the average HTTP page is 1.9MB
        // per http://httparchive.org/interesting.php, as of October 2015
        public BufferPool(int maxBufferSizeInBytes = 2048000)
        {
            int maxBuckets = SelectBucketIndex(maxBufferSizeInBytes);

            _maxBufferSize = maxBufferSizeInBytes;
            _buckets = new BufferBucket<T>[maxBuckets + 1];
        }

        public T[] RentBuffer(int size, bool clearBuffer = false)
        {
            T[] buffer;
            if (size > _maxBufferSize)
            {
#if DEBUG
                System.Diagnostics.Debugger.Break();
#endif
                buffer = new T[size];
            }
            else
            {
                int index = SelectBucketIndex(size);
                BufferBucket<T> bucket = Volatile.Read(ref _buckets[index]);
                if (bucket == null)
                {
                    Interlocked.CompareExchange(ref _buckets[index], new BufferBucket<T>(GetMaxSizeForBucket(index), NUMBER_OF_BUFFERS_IN_BUCKET), null);
                    bucket = _buckets[index];
                }
                buffer = bucket.Rent();
                if (clearBuffer) Array.Clear(buffer, 0, buffer.Length);
            }

            return buffer;
        }

        public void ReturnBuffer(ref T[] buffer, bool clearBuffer = false)
        {
            if (clearBuffer) Array.Clear(buffer, 0, buffer.Length);

            if (buffer.Length > _maxBufferSize)
                buffer = null;
            else
                _buckets[SelectBucketIndex(buffer.Length)].Return(ref buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int SelectBucketIndex(int bufferSize)
        {
            uint bitsRemaining = ((uint)bufferSize - 1) >> 4;
            int poolIndex = 0;

            while (bitsRemaining > 0)
            {
                bitsRemaining >>= 1;
                poolIndex++;
            }

            return poolIndex;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetMaxSizeForBucket(int binIndex)
        {
            checked
            {
                int result = 2;
                int shifts = binIndex + 3;
                result <<= shifts;
                return result;
            }
        }
    }
}
