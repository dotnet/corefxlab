// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace System.Buffers
{
    public unsafe sealed class NativeBufferPool : IBufferPool<byte>
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
            lock (_lock) {
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

            return new BufferManager<byte>(new IntPtr((byte*)(_memory + i * _bufferSize)), _bufferSize);
        }

        public void Return(OwnedMemory<byte> buffer)
        {
            var pooledBuffer = buffer as BufferManager<byte>;
            if (pooledBuffer == null) throw new Exception();

            var memory = pooledBuffer.Pointer.ToInt64();
            if(memory < _memory.ToInt64() || memory > _memory.ToInt64() + _bufferSize * _bufferCount) {
                throw new Exception("not rented from this pool");
            }

            var offset = memory - _memory.ToInt64();
            var index = offset / _bufferSize;

            lock (_lock) {
                buffer.Dispose();
                if (_rented[index] == false) throw new Exception("this buffer is not rented");
                _rented[index] = false;
            }
        }

        sealed class BufferManager<T> : OwnedMemory<T>
        {
            IntPtr _memory;
            int _length;

            public BufferManager(IntPtr memory, int length)
            {
                _length = length;
                _memory = memory;
            }

            public IntPtr Pointer => _memory;

            protected override void DisposeCore()
            {
                _memory = IntPtr.Zero;
            }

            protected override Span<T> GetSpanCore()
            {
                unsafe
                {
                    return new Span<T>(_memory.ToPointer(), _length);
                }
            }

            protected override unsafe bool TryGetPointerCore(out void* pointer)
            {
                pointer = _memory.ToPointer();
                return true;
            }

            protected override bool TryGetArrayCore(out ArraySegment<T> buffer)
            {
                buffer = default(ArraySegment<T>);
                return false;
            }
        }
    }
}
