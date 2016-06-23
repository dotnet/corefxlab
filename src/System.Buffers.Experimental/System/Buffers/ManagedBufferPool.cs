// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Buffers
{
    [Obsolete("Use ArrayPool<T> instead. It can be found in CoreFx System.Buffers package.")]
    public sealed class ManagedBufferPool<T> where T : struct
    {
        private const int NUMBER_OF_BUFFERS_IN_BUCKET = 50;
        private int _maxBufferSize;
        private ManagedBufferBucket<T>[] _buckets;

        private static ManagedBufferPool<byte> _sharedInstance = null;

        public static ManagedBufferPool<byte> SharedByteBufferPool
        {
            get
            {
                if (Volatile.Read(ref _sharedInstance) == null)
                {
                    Interlocked.CompareExchange(ref _sharedInstance, new ManagedBufferPool<byte>(), null);
                }

                return _sharedInstance;
            }
        }

        // 2MB taken as the default since the average HTTP page is 1.9MB
        // per http://httparchive.org/interesting.php, as of October 2015
        public ManagedBufferPool(int maxBufferSizeInBytes = 2048000)
        {
            int maxBuckets = BufferUtilities.SelectBucketIndex(maxBufferSizeInBytes);

            _maxBufferSize = maxBufferSizeInBytes;
            _buckets = new ManagedBufferBucket<T>[maxBuckets + 1];
        }

        public T[] RentBuffer(int size, bool clearBuffer = false)
        {
            T[] buffer;
            if (size > _maxBufferSize)
            {
                buffer = new T[size];
            }
            else
            {
                int index = BufferUtilities.SelectBucketIndex(size);
                ManagedBufferBucket<T> bucket = Volatile.Read(ref _buckets[index]);
                if (bucket == null)
                {
                    Interlocked.CompareExchange(ref _buckets[index], new ManagedBufferBucket<T>(BufferUtilities.GetMaxSizeForBucket(index), NUMBER_OF_BUFFERS_IN_BUCKET), null);
                    bucket = _buckets[index];
                }
                buffer = bucket.Rent();
                if (clearBuffer) Array.Clear(buffer, 0, buffer.Length);
            }

            return buffer;
        }

        public void EnlargeBuffer(ref T[] buffer, int newSize, bool clearFreeSpace = false)
        {
            if (newSize <= buffer.Length)
                throw new InvalidOperationException("Cannot make buffer smaller or the same size");

            T[] newBuffer = RentBuffer(newSize, false); // Don't clear the whole buffer since we can optimize below
            Array.Copy(buffer, newBuffer, buffer.Length);

            if (clearFreeSpace) Array.Clear(newBuffer, buffer.Length, newBuffer.Length - buffer.Length);

            ReturnBuffer(ref buffer, true);
            buffer = newBuffer;
        }

        public void ReturnBuffer(ref T[] buffer, bool clearBuffer = false)
        {
            if (clearBuffer) Array.Clear(buffer, 0, buffer.Length);

            if (buffer.Length > _maxBufferSize)
                buffer = null;
            else
                _buckets[BufferUtilities.SelectBucketIndex(buffer.Length)].Return(ref buffer);
        }
    }
}
