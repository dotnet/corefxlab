// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace System.Buffers.Pools
{
    public unsafe sealed class NativeBufferPool : BufferPool
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

        protected override void Dispose(bool disposing)
        {
            lock (_lock)
            {
                if (_disposed) return;
                _disposed = true;
            }
            Marshal.FreeHGlobal(_memory);
        }

        public override OwnedBuffer<byte> Rent(int numberOfBytes)
        {
            if (numberOfBytes < 1) throw new ArgumentOutOfRangeException(nameof(numberOfBytes));
            if (numberOfBytes > _bufferSize) new NotSupportedException();

            int i;
            lock (_lock)
            {
                for (i = 0; i < _rented.Length; i++)
                {
                    if (!_rented[i])
                    {
                        _rented[i] = true;
                        break;
                    }
                }
            }

            if (i >= _rented.Length)
            {
                throw new NotImplementedException("no more buffers to rent");
            }

            return new BufferManager(this, new IntPtr((byte*)(_memory + i * _bufferSize)), _bufferSize);
        }

        internal void Return(BufferManager pooledBuffer)
        {
            var memory = pooledBuffer.Pointer.ToInt64();
            var offset = memory - _memory.ToInt64();
            var index = offset / _bufferSize;

            lock (_lock)
            {
                _rented[index] = false;
            }
        }

        internal sealed class BufferManager : OwnedBuffer<byte>
        {
            private readonly NativeBufferPool _pool;
            IntPtr _pointer;
            int _length;

            public BufferManager(NativeBufferPool pool, IntPtr memory, int length)
            {
                _pointer = memory;
                _length = length;
                _pool = pool;
            }

            public IntPtr Pointer => _pointer;

            public override int Length => _length;

            public override Span<byte> Span => new Span<byte>(_pointer.ToPointer(), _length);

            public override Span<byte> GetSpan(int index, int length)
            {
                if (IsDisposed) ThrowObjectDisposed();
                return Span.Slice(0, length);
            }

            protected override void Dispose(bool disposing)
            {
                _pool.Return(this);
                base.Dispose(disposing);
            }

            protected override bool TryGetArrayInternal(out ArraySegment<byte> buffer)
            {
                buffer = default(ArraySegment<byte>);
                return false;
            }

            protected override bool TryGetPointerInternal(out void* pointer)
            {
                pointer = _pointer.ToPointer();
                return true;
            }
        }
    }
}
