using System;
using System.Buffers;

namespace System.Net.Libuv
{
    internal class UVBuffer : IDisposable
    {
        static NativeBufferPool _pool = new NativeBufferPool(1024, 10);
        public readonly static UVBuffer Default = new UVBuffer();

        public static UVInterop.alloc_callback_unix AllocateUnixBuffer { get; set; }
        public static UVInterop.alloc_callback_win AllocWindowsBuffer { get; set; }

        private UVBuffer() { }

        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                _pool.Dispose();
            }
        }

        static UVBuffer()
        {
            AllocateUnixBuffer = OnAllocateUnixBuffer;
            AllocWindowsBuffer = OnAllocateWindowsBuffer;
        }

        internal static void FreeBuffer(IntPtr buffer)
        {
            unsafe
            {
                var span = new ByteSpan((byte*)buffer, 1); // TODO: this is a hack
                _pool.Return(ref span);
            }
        }

        static void OnAllocateUnixBuffer(IntPtr memoryBuffer, uint length, out UnixBufferStruct buffer)
        {
            var memory = _pool.Rent();
            unsafe
            {
                buffer = new UnixBufferStruct((IntPtr)memory.UnsafeBuffer, (uint)memory.Length);
            }
        }

        static void OnAllocateWindowsBuffer(IntPtr memoryBuffer, uint length, out WindowsBufferStruct buffer)
        {
            var memory = _pool.Rent();
            unsafe
            {
                buffer = new WindowsBufferStruct((IntPtr)memory.UnsafeBuffer, (uint)memory.Length);
            }
        }
    }
}
