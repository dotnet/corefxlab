// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.IO.Buffers
{
    public unsafe class NativeBufferPool : IDisposable
    {
        byte* _memory;
        int[] _freeList;
        int _nextFree = 0;
        readonly int _totalBytes;
        readonly int _bufferSizeInBytes;

        public NativeBufferPool(int bufferSizeInBytes, int numberOfBuffers)
        {
            _bufferSizeInBytes = bufferSizeInBytes;
            _freeList = new int[numberOfBuffers];
            _totalBytes = bufferSizeInBytes * numberOfBuffers;
            _memory = (byte*)Marshal.AllocHGlobal(_totalBytes).ToPointer();
        }

        public ByteSpan Rent()
        {
            var freeIndex = Reserve();
            if (freeIndex == -1) {
                throw new NotImplementedException();
            }
            var start = _bufferSizeInBytes * freeIndex;
            return new ByteSpan(_memory + start, _bufferSizeInBytes, freeIndex);
        }

        public void Return(ref ByteSpan span)
        {
            span._data = null;
            if (Interlocked.CompareExchange(ref _freeList[span._id], 0, 1) != 1) {
                throw new InvalidOperationException("this span has been already returned");
            }
            if (span._id < _nextFree) {
                _nextFree = span._id;
            }
            span._id = -1;
        }

        int Reserve()
        {
            var initialNextFree = _nextFree;
            for (int i = initialNextFree; i < _freeList.Length; i++) {
                if (Interlocked.CompareExchange(ref _freeList[i], 1, 0) == 0) {
                    _nextFree++;
                    return i;
                }
            }
            if (initialNextFree == _nextFree) {
                if (_nextFree == 0) {
                    return -1;
                }
                _nextFree = 0;
            }
            return Reserve();
        }

        public void Dispose()
        {
            for (int i = 0; i < _freeList.Length; i++) {
                _freeList[i] = 1;
            }
            Marshal.FreeHGlobal(new IntPtr(_memory));
            _memory = null;
        }
    }

    public class BufferPool
    {
        static BufferPool s_instance = new BufferPool(1024 * 1024);

        int _maxBufferSize; // larger buffers won't be pooled
        BufferBin[] _bins;

        int _largeAllocationCounter; // performance counter for non-pooled allocations

        private BufferPool(int maxBufferSize)
        {
            Precondition.Require(maxBufferSize > 0);

            _maxBufferSize = maxBufferSize;

            var maxSizeBin = SelectBinIndex(maxBufferSize);
            _bins = new BufferBin[maxSizeBin + 1];

            for (var binIndex = 0; binIndex < _bins.Length; binIndex++)
            {
                int maxSizeForBin = GetMaxSizeForBin(binIndex);
                _bins[binIndex] = new BufferBin(maxSizeForBin, 10);
            }
        }

        public byte[] RentBuffer(int minSize)
        {
            if (minSize > _maxBufferSize)
            {
                Interlocked.Increment(ref _largeAllocationCounter);
                return new byte[minSize];
            }
            int poolIndex = SelectBinIndex(minSize);
            byte[] buffer = _bins[poolIndex].RentBuffer();
            return buffer;
        }

        public void ReturnBuffer(ref byte[] buffer)
        {
            int poolIndex = SelectBinIndex(buffer.Length);
            if (poolIndex >= _bins.Length) // this happend if the returned buffer is a large buffer not managed by the pool
            {
                return;
            }
            _bins[poolIndex].ReturnBuffer(buffer);
            buffer = null;
        }

        public void Enlarge(ref byte[] buffer, int newBufferMinSize)
        {
            var newBuffer = RentBuffer(newBufferMinSize);
            buffer.CopyTo(newBuffer, 0);
            ReturnBuffer(ref buffer);
            buffer = newBuffer;
        }

        public long Allocations
        {
            get
            {
                long sum = 0;
                foreach (var pool in _bins)
                {
                    sum += pool.Allocations;
                }
                return sum + _largeAllocationCounter;
            }
        }

        public static BufferPool Shared
        {
            get
            {
                return s_instance;
            }
        }

        private int SelectBinIndex(int bufferSize)
        {
            Precondition.Require(bufferSize > 0);
            var _poolIndex = MaxBitIndexSet((ulong)bufferSize - 1) - 3;
            if (_poolIndex < 0) return 0;
            return _poolIndex;
        }

        private int GetMaxSizeForBin(int binIndex)
        {
            checked
            {
                int result = 2;
                int shifts = binIndex + 3;
                result <<= shifts;
                return result;
            }
        }

        private int MaxBitIndexSet(ulong value)
        {
            if (value == 0) return -1;
            if (value == 1) return 0;
            int bit = -1;
            while (value > 0)
            {
                value >>= 1;
                bit++;
            }
            return bit;
        }
    }

    // stores buffers of equal size
    public class BufferBin
    {
        int _bufferSize; // the size of buffers in this bin
        int _capacity;
        byte[][] _buffers;
        int _top; // the array above is a stack. This filed is the top of the stack. Top points to the top most buffer

        int _allocationCounter; // performance counter incremented when bin actually allocates a new buffer on the GC heap

        public BufferBin(int bufferSize, int capacity)
        {
            _bufferSize = bufferSize;
            _capacity = capacity;
            _buffers = null;
            _top = -1;
            _allocationCounter = 0;
        }

        public byte[] RentBuffer()
        {
            if (_buffers == null)
            {
                var buffers = new byte[_capacity][];
                Interlocked.CompareExchange(ref _buffers, buffers, null);
            }
            while (true)
            {
                int top = _top;
                if (top == -1) // no buffers in the bin
                {
                    return Allocate();
                }

                var buffer = Interlocked.Exchange(ref _buffers[top], null);
                if (buffer != null) // ok, I got a buffer
                {
                    // need to update _top
                    var originalValue = Interlocked.CompareExchange(ref _top, top - 1, top);
                    if (originalValue != top) // oops, somebody else changed _top
                    {
                        if (_buffers[top] == null) // moreover there is possibly a hole in the stack now
                        {
                            _buffers[top] = Allocate(); // we don't want to leave a hole
                        }
                    }
                    return buffer;
                }
            }
        }

        public void ReturnBuffer(byte[] buffer)
        {
            if(buffer.Length != _bufferSize)
            {
                throw new InvalidOperationException("buffer does not belong to this pool or bin");
            }

            int top = _top;
            if (top == _buffers.Length - 1) return; // no room in the pool; just drop the buffer on the GC; maybe a counter for this would be good

            var candidateIndex = top + 1;
            _buffers[candidateIndex] = buffer;
            var originalValue = Interlocked.CompareExchange(ref _top, candidateIndex, top);
        }

        public int Allocations { get { return _allocationCounter; } }

        byte[] Allocate()
        {
            Interlocked.Increment(ref _allocationCounter);
            return new byte[_bufferSize];
        }
    }
}
