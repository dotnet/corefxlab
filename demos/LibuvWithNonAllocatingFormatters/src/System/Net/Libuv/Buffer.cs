using System;
using System.Runtime.InteropServices;

namespace System.Net.Libuv
{
    class PinnedArray : IDisposable
    {
        public byte[] Array { get; protected set; }
        IntPtr _arrayAddress;
        GCHandle _arrayHandle { get; set; }

        public PinnedArray(byte[] buffer)
        {
            Array = buffer;
            _arrayHandle = GCHandle.Alloc(Array, GCHandleType.Pinned);
            _arrayAddress = _arrayHandle.AddrOfPinnedObject();
        }

        public int Length { get { return Array.Length; } }

        public IntPtr Address
        {
            get
            {
                return _arrayAddress;
            }
        }

        ~PinnedArray()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_arrayHandle.IsAllocated)
            {
                _arrayHandle.Free();
            }
            _arrayAddress = IntPtr.Zero;
            Array = null;
        }
    }

    internal class UVBuffer : IDisposable
    {
        public readonly static UVBuffer Default = new UVBuffer();

        PinnedArray pinned;

        public byte[] Buffer
        {
            get
            {
                return pinned.Array;
            }
        }

        public byte[] Copy(int byteCount)
        {
            byte[] copy = new byte[byteCount];
            Array.Copy(Buffer, 0, copy, 0, byteCount);
            return copy;
        }

        public Span<byte> Borrow(int byteCount)
        {
            return new Span<byte>(Buffer, 0, byteCount);
        }

        public static UVInterop.alloc_callback_unix AllocateUnixBuffer { get; set; }
        public static UVInterop.alloc_callback_win AllocWindowsBuffer { get; set; }

        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing && pinned != null)
            {
                pinned.Dispose();
            }
            pinned = null;
        }

        static UVBuffer()
        {
            AllocateUnixBuffer = OnAllocateUnixBuffer;
            AllocWindowsBuffer = OnAllocateWindowsBuffer;
        }

        IntPtr AllocatePinnedBuffer(uint length)
        {
            if (pinned != null && pinned.Array.Length < length)
            {
                pinned.Dispose();
                pinned = null;
            }

            if (pinned == null)
            {
                var array = new byte[length];
                pinned = new PinnedArray(array);
            }

            return pinned.Address;
        }

        static void OnAllocateUnixBuffer(IntPtr memoryBuffer, uint length, out UnixBufferStruct buffer)
        {
            IntPtr ptr = Default.AllocatePinnedBuffer(length);
            buffer = new UnixBufferStruct(ptr, length);
        }

        static void OnAllocateWindowsBuffer(IntPtr memoryBuffer, uint length, out WindowsBufferStruct buffer)
        {
            IntPtr ptr = Default.AllocatePinnedBuffer(length);
            buffer = new WindowsBufferStruct(ptr, length);
        }
    }
}
