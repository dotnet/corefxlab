// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace System.Buffers.Native
{
    public unsafe sealed partial class NativeMemoryPool : MemoryPool<byte>
    {
        const int DefaultSize = 4096;

        static NativeMemoryPool s_shared = new NativeMemoryPool(DefaultSize);
        object _lock = new object();
        bool _disposed;

        IntPtr _memory;
        int _bufferSize;
        int _bufferCount;
        bool[] _rented;

        public static NativeMemoryPool Shared => s_shared;

        public NativeMemoryPool(int bufferSize, int bufferCount = 10)
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

        public override int MaxBufferSize => 1024 * 1024 * 1024;

        public override OwnedMemory<byte> Rent(int numberOfBytes = DefaultSize)
        {
            if (numberOfBytes == AnySize) numberOfBytes = DefaultSize;
            if (numberOfBytes < 1 || numberOfBytes > MaxBufferSize) throw new ArgumentOutOfRangeException(nameof(numberOfBytes));
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

            return new Memory(this, new IntPtr((byte*)(_memory + i * _bufferSize)), _bufferSize);
        }

        internal void Return(Memory pooledBuffer)
        {
            var memory = pooledBuffer.Pointer.ToInt64();
            var offset = memory - _memory.ToInt64();
            var index = offset / _bufferSize;

            lock (_lock)
            {
                _rented[index] = false;
            }
        }
    }
}
