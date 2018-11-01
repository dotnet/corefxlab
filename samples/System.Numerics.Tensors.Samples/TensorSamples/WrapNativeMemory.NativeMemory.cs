using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace TensorSamples
{
    partial class WrapNativeMemory
    {
        public class NativeMemory<T> : MemoryManager<T>
        {
            private bool disposed = false;
            private int refCount = 0;
            private IntPtr memory;
            private int length;

            public NativeMemory(IntPtr memory, int length)
            {
                this.memory = memory;
                this.length = length;
            }

            public unsafe NativeMemory(void* memory, int length)
            {
                this.memory = (IntPtr)memory;
                this.length = length;
            }

            ~NativeMemory()
            {
                Dispose(false);
            }

            public static NativeMemory<T> Allocate(int length)
            {
                IntPtr memory = AllocateBuffer(Marshal.SizeOf(typeof(T)) * length);
                return new NativeMemory<T>(memory, length);
            }

            public bool IsDisposed => disposed;

            public unsafe override Span<T> GetSpan() => new Span<T>((void*)memory, length);

            protected bool IsRetained => refCount > 0;

            public override unsafe MemoryHandle Pin(int elementIndex = 0)
            {
                if ((uint)elementIndex > length)
                {
                    throw new ArgumentOutOfRangeException(nameof(elementIndex));
                }

                Retain();
                void* pointer = Unsafe.Add<T>((void*)memory, elementIndex);
                return new MemoryHandle(pointer, default, this);
            }

            public bool Release()
            {
                int newRefCount = Interlocked.Decrement(ref refCount);

                if (newRefCount < 0)
                {
                    throw new InvalidOperationException("Unmatched Release/Retain");
                }

                return newRefCount != 0;
            }

            public void Retain()
            {
                if (disposed)
                {
                    throw new ObjectDisposedException(nameof(NativeMemory<T>));
                }

                Interlocked.Increment(ref refCount);
            }

            public override void Unpin()
            {
                Release();
            }

            protected override void Dispose(bool disposing)
            {
                if (disposed)
                {
                    return;
                }

                FreeBuffer(memory);
                memory = IntPtr.Zero;

                disposed = true;
            }

            protected override bool TryGetArray(out ArraySegment<T> arraySegment)
            {
                // cannot expose managed array
                arraySegment = default;
                return false;
            }
        }

        [DllImport("TensorSamples.native.dll")]
        private static extern unsafe IntPtr AllocateBuffer(int sizeInBytes);

        [DllImport("TensorSamples.native.dll")]
        private static extern unsafe double FreeBuffer(IntPtr buffer);
    }
}
