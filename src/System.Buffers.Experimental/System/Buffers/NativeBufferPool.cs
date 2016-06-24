// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Buffers
{
    public unsafe sealed class NativeBufferPool : IDisposable
    {
        private const int NUMBER_OF_BUFFERS_IN_BUCKET = 50;
        private int _maxElements;
        private NativeBufferBucket[] _buckets;

        private bool _disposed;

        private static NativeBufferPool _sharedInstance = null;

        public static NativeBufferPool Shared
        {
            get
            {
                if (Volatile.Read(ref _sharedInstance) == null)
                {
                    // 2MB taken as the default since the average HTTP page is 1.9MB
                    // per http://httparchive.org/interesting.php, as of October 2015
                    NativeBufferPool newPool = new NativeBufferPool(2048000);
                    if (Interlocked.CompareExchange(ref _sharedInstance, newPool, null) != null)
                        newPool.Dispose();
                }

                return _sharedInstance;
            }
        }

        public NativeBufferPool(int maxElementsInBuffer)
        {
            int maxBuckets = BufferUtilities.SelectBucketIndex(maxElementsInBuffer);
            _maxElements = maxElementsInBuffer;
            _buckets = new NativeBufferBucket[maxBuckets + 1];
        }

        ~NativeBufferPool()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (NativeBufferBucket bucket in _buckets)
                    if (bucket != null)
                        bucket.Dispose();

                _disposed = true;
            }
        }

        public Span<byte> Rent(int numberOfElements, bool clearBuffer = false)
        {
            if (_disposed)
                throw new ObjectDisposedException("NativeBufferPool");

            Span<byte> buffer;
            if (numberOfElements > _maxElements)
            {
#if DEBUG
                System.Diagnostics.Debugger.Break();
#endif
                buffer = new Span<byte>(Marshal.AllocHGlobal(numberOfElements * Marshal.SizeOf(typeof(byte))).ToPointer(), numberOfElements);
            }
            else
            {
                int index = BufferUtilities.SelectBucketIndex(numberOfElements);
                NativeBufferBucket bucket = Volatile.Read(ref _buckets[index]);
                if (bucket == null)
                {
                    NativeBufferBucket newBucket = new NativeBufferBucket(BufferUtilities.GetMaxSizeForBucket(index), NUMBER_OF_BUFFERS_IN_BUCKET);
                    if (Interlocked.CompareExchange(ref _buckets[index], newBucket, null) != null)
                        newBucket.Dispose();

                    bucket = _buckets[index];
                }

                var rented = bucket.Rent();
                buffer = new Span<byte>(rented._memory, rented._length);
            }

            if (clearBuffer) BufferUtilities.ClearSpan(buffer);
            return buffer;
        }

        public void Return(Span<byte> buffer, bool clearBuffer = false)
        {
            if (_disposed)
                throw new ObjectDisposedException("NativeBufferPool");

            if (clearBuffer) BufferUtilities.ClearSpan(buffer);

            if (buffer.Length > _maxElements)
            {
                Marshal.FreeHGlobal(new IntPtr(buffer.UnsafePointer));
            }
            else
            {
                _buckets[BufferUtilities.SelectBucketIndex(buffer.Length)].Return(new NativeBuffer(buffer.UnsafePointer, 0));
            }
        }
    }
}
