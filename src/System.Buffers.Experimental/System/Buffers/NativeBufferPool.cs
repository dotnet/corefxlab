// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace System.Buffers
{
    public unsafe class NativeBufferPool : IBufferPool<byte>
    {
        static NativeBufferPool s_shared = new NativeBufferPool(4096);
        object _lock = new object();
        bool _disposed;

        IntPtr _memory;
        int _bufferSize;
        int _bufferCount;
        bool[] _rented;

        public static NativeBufferPool Shared => s_shared;

        public NativeBufferPool(int bufferSize, int bufferCount = 10)
        {
            if (bufferSize < 1) throw new ArgumentOutOfRangeException(nameof(bufferSize));
            if (bufferCount < 1) throw new ArgumentOutOfRangeException(nameof(bufferSize));

            _rented = new bool[bufferCount];
            _bufferSize = bufferSize;
            _bufferCount = bufferCount;
            _memory = Marshal.AllocHGlobal(_bufferSize * _bufferCount);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        ~NativeBufferPool()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            lock (_lock)
            {
                if (_disposed) return;
                _disposed = true;
            }

            if (disposing)
            {
                GC.SuppressFinalize(this);
            }

            Marshal.FreeHGlobal(_memory);
        }

        public OwnedMemory<byte> Rent(int numberOfBytes)
        {
            if (numberOfBytes < 1) throw new ArgumentOutOfRangeException(nameof(numberOfBytes));
            if (numberOfBytes > _bufferSize) new NotSupportedException();

            int i;
            lock (_lock) {
                for(i = 0; i < _rented.Length; i++) {
                    if (!_rented[i]) {
                        _rented[i] = true;
                        break;
                    }
                }
            }

            if (i >= _rented.Length) {
                    throw new NotImplementedException("no more buffers to rent");
            }

            return new BufferManager(new IntPtr((byte*)(_memory + i * _bufferSize)), _bufferSize, this);
        }

        void IMemoryDisposer<byte>.Return(OwnedMemory<byte> buffer)
        {
            if (buffer?.Owner != this) throw new InvalidOperationException("buffer not rented from this pool");

            void* pointer;
            buffer.Memory.TryGetPointer(out pointer);

            var memory = (long)pointer;
            var offset = memory - _memory.ToInt64();
            var index = offset / _bufferSize;

            lock (_lock)
            {
                if (_rented[index] == false) throw new Exception("this buffer is not rented");
                _rented[index] = false;
            }
        }

        sealed class BufferManager : OwnedMemory<byte>
        {
            public BufferManager(IntPtr memory, int length, IMemoryDisposer<byte> owner) : base(null, 0, memory, length, owner)
            {}

            protected override void DisposeCore()
            {
            }
        }
    }
}
