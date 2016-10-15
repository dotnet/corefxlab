using System;
using System.Buffers;
using System.Runtime.InteropServices;

namespace System.Net.Libuv
{
    internal class UVBuffer
    {
        static NativeBufferPool _pool = new NativeBufferPool(1024);
        public readonly static UVBuffer Default = new UVBuffer();

        public static UVInterop.alloc_callback_unix AllocateUnixBuffer { get; set; }
        public static UVInterop.alloc_callback_win AllocWindowsBuffer { get; set; }

        private UVBuffer() { }

        static UVBuffer()
        {
            AllocateUnixBuffer = OnAllocateUnixBuffer;
            AllocWindowsBuffer = OnAllocateWindowsBuffer;
        }

        internal static void FreeBuffer(UnsafeMemory<byte> buffer)
        {
            _pool.Return(buffer);
        }

        static void OnAllocateUnixBuffer(IntPtr memoryBuffer, uint length, out Unix buffer)
        {
            var rented = _pool.Rent((int)length);
            unsafe
            {
                void* pointer;
                if (!rented.TryGetPointer(out pointer))
                {
                    throw new InvalidOperationException("The native pointer isn't available because the memory isn't pinned");
                }
                buffer = new Unix(new IntPtr(pointer), (uint)rented.Length);
            }
        }

        static void OnAllocateWindowsBuffer(IntPtr memoryBuffer, uint length, out Windows buffer)
        {
            var rented = _pool.Rent((int)length);

            unsafe
            {
                void* pointer;
                if (!rented.TryGetPointer(out pointer))
                {
                    throw new InvalidOperationException("The native pointer isn't available because the memory isn't pinned");
                }
                buffer = new Windows(new IntPtr(pointer), (uint)rented.Length);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Windows
        {
            internal uint Length;
            internal IntPtr Buffer;

            internal Windows(IntPtr buffer, uint length)
            {
                Buffer = buffer;
                Length = length;
            }

            internal void Dispose()
            {
                unsafe
                {
                    FreeBuffer(new UnsafeMemory<byte>(Buffer.ToPointer(), (int)Length));
                    Length = 0;
                    Buffer = IntPtr.Zero;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Unix
        {
            internal IntPtr Buffer;
            internal IntPtr Length;

            internal Unix(IntPtr buffer, uint length)
            {
                Buffer = buffer;
                Length = (IntPtr)length;
            }

            internal void Dispose()
            {
                unsafe
                {
                    FreeBuffer(new UnsafeMemory<byte>((byte*)Buffer.ToPointer(), Length.ToInt32()));
                    Length = IntPtr.Zero;
                    Buffer = IntPtr.Zero;
                }
            }
        }
    }
}
