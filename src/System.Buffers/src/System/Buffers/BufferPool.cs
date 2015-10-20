// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;
using System.Threading;

namespace System.Buffers
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
                throw new NotSupportedException("buffer resizing not supported.");
            }
            var start = _bufferSizeInBytes * freeIndex;
            return new ByteSpan(_memory + start, _bufferSizeInBytes);
        }

        int BufferIndexFromSpanAddress(ref ByteSpan span)
        {
            var buffer = (ulong)span._data;
            var firstBuffer = (ulong)_memory;
            var offset = buffer - firstBuffer;
            var index = offset / (ulong)_bufferSizeInBytes;
            return (int)index;
        }

        public void Return(ByteSpan buffer)
        {
            int spanIndex = BufferIndexFromSpanAddress(ref buffer);

            if (spanIndex < 0 || spanIndex > _freeList.Length)
            {
                throw new InvalidOperationException("This buffer is not from this pool.");
            }

            buffer._data = null;
            if (Interlocked.CompareExchange(ref _freeList[spanIndex], 0, 1) != 1) {
                throw new InvalidOperationException("this buffer has been already returned.");
            }
            if (spanIndex < _nextFree) {
                _nextFree = spanIndex;
            }
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
}
