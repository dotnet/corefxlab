using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static Interop.Sys;

namespace System.Buffers.Experimental.System.Buffers
{
    public class EphemeralBufferUnix
    {
        private IntPtr _memory;
        private int _bufferCount;
        private int _bufferSize;
        private ConcurrentQueue<EphemeralMemory> _buffers = new ConcurrentQueue<EphemeralMemory>();
        private long _totalAllocated;
        private byte[] _emptyData;

        public EphemeralBufferUnix(int bufferSize, int bufferCount)
        {
            if (bufferSize < 1) throw new ArgumentOutOfRangeException(nameof(bufferSize));
            if (bufferCount < 1) throw new ArgumentOutOfRangeException(nameof(bufferSize));

            var pageSize = SysConf(SysConfName._SC_PAGESIZE);
            if (pageSize < 0)
            {
                ThrowHelper.ThrowInvalidOperationException();
            }
            _emptyData = new byte[bufferSize];
            var pages = (int)Math.Ceiling((bufferCount * bufferSize) / (double)pageSize);
            _totalAllocated = pages * pageSize;
            _bufferCount = (int)_totalAllocated / bufferSize;
            _bufferSize = bufferSize;
            _memory = MMap(IntPtr.Zero, (ulong)_totalAllocated, MemoryMappedProtections.PROT_READ | MemoryMappedProtections.PROT_WRITE, MemoryMappedFlags.MAP_PRIVATE | MemoryMappedFlags.MAP_ANONYMOUS, new IntPtr(-1), 0);
            if (_memory.ToInt64() < 0)
            {
                ThrowHelper.ThrowInvalidOperationException();
            }
            if (MLock(_memory, (ulong)_totalAllocated) < 0)
            {
                ThrowHelper.ThrowInvalidOperationException();
            }

            for (var i = 0; i < _totalAllocated; i += bufferSize)
            {
                var mem = new EphemeralMemory(IntPtr.Add(_memory, i), bufferSize);
                _buffers.Enqueue(mem);
            }
        }

        sealed class EphemeralMemory : OwnedMemory<byte>
        {
            public EphemeralMemory(IntPtr memory, int length) : base(null, 0, length, memory)
            { }
            internal bool Rented;
            public new IntPtr Pointer => base.Pointer;
        }

        public unsafe void Dispose()
        {
            MemSet((void*)_memory, 0, (UIntPtr)_totalAllocated);
            if (MUnmap(_memory, (ulong)_totalAllocated) < 0)
            {
                Debug.Assert(false,"Didn't let go of the memory");
            }
            GC.SuppressFinalize(this);
        }

        ~EphemeralBufferUnix()
        {
            Dispose();
        }
    }
}
