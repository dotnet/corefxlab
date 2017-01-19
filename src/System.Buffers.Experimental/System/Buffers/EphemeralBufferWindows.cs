using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static Interop.Kernel32;

namespace System.Buffers.Experimental.System.Buffers
{
    public class EphemeralBufferWindows
    {
        private IntPtr _memory;
        private int _bufferCount;
        private int _bufferSize;
        private ConcurrentQueue<SecureMemory> _buffers = new ConcurrentQueue<SecureMemory>();
        private UIntPtr _totalAllocated;
        private byte[] _emptyData;

        public EphemeralBufferWindows(int bufferSize, int bufferCount)
        {
            if (bufferSize < 1) throw new ArgumentOutOfRangeException(nameof(bufferSize));
            if (bufferCount < 1) throw new ArgumentOutOfRangeException(nameof(bufferSize));

            _emptyData = new byte[bufferSize];
            SYSTEM_INFO sysInfo;
            GetSystemInfo(out sysInfo);
            var pages = (int)Math.Ceiling((bufferCount * bufferSize) / (double)sysInfo.dwPageSize);
            var totalAllocated = pages * sysInfo.dwPageSize;
            _bufferCount = totalAllocated / bufferSize;
            _bufferSize = bufferSize;
            _totalAllocated = new UIntPtr((uint)totalAllocated);

            _memory = VirtualAlloc(IntPtr.Zero, _totalAllocated, MemOptions.MEM_COMMIT | MemOptions.MEM_RESERVE, PageOptions.PAGE_READWRITE);
            VirtualLock(_memory, _totalAllocated);
            for (var i = 0; i < totalAllocated; i += bufferSize)
            {
                var mem = new SecureMemory(IntPtr.Add(_memory, i), bufferSize);
                _buffers.Enqueue(mem);
            }
        }

        public OwnedMemory<byte> Rent()
        {
            SecureMemory returnValue;
            if (!_buffers.TryDequeue(out returnValue))
            {
                ThrowHelper.ThrowInvalidOperationException();
            }
            returnValue.Rented = true;
            return returnValue;
        }

        public void Return(OwnedMemory<byte> buffer)
        {
            var buffer2 = buffer as SecureMemory;
            if (buffer2 == null)
            {
                Debug.Assert(false,"Buffer was empty");
                return;
            }
            Debug.Assert(buffer2.Rented, "Returning a buffer that isn't rented!");
            if (!buffer2.Rented)
            {
                return;
            }
            buffer2.Rented = false;
            _emptyData.CopyTo(buffer.Span);
            _buffers.Enqueue(buffer2);
        }

        sealed class SecureMemory : OwnedMemory<byte>
        {
            public SecureMemory(IntPtr memory, int length) : base(null, 0, length, memory)
            { }
            internal bool Rented;
            public new IntPtr Pointer => base.Pointer;
        }

        public unsafe void Dispose()
        {
            RtlZeroMemory(_memory, _totalAllocated);
            VirtualFree(_memory, _totalAllocated, 0x8000);
            GC.SuppressFinalize(this);
        }

        ~EphemeralBufferWindows()
        {
            Dispose();
        }
    }
}
