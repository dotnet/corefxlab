using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace TensorSamples
{
    partial class WrapNativeMemory
    {
        public class NativeMemory<T> : OwnedMemory<T>
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

            public override bool IsDisposed => disposed;

            public override int Length => length;

            public unsafe override Span<T> Span => new Span<T>((void*)memory, length);

            protected override bool IsRetained => refCount > 0;

            public override MemoryHandle Pin()
            {
                unsafe
                {
                    Retain();
                    return new MemoryHandle(this, (void*)memory);
                }
            }

            public override bool Release()
            {
                int newRefCount = Interlocked.Decrement(ref refCount);

                if (newRefCount < 0)
                {
                    throw new InvalidOperationException("Unmatched Release/Retain");
                }

                return newRefCount != 0;
            }

            public override void Retain()
            {
                if (disposed)
                {
                    throw new ObjectDisposedException(nameof(NativeMemory<T>));
                }

                Interlocked.Increment(ref refCount);
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
        extern private static unsafe double FreeBuffer(IntPtr buffer);

        [DllImport("TensorSamples.native.dll")]
        extern private static unsafe IntPtr AllocateBuffer(int sizeInBytes);
    }
}
