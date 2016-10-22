using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Buffers.Experimental.System.Buffers
{
    public class NativeAllocator<T> : IMemoryAllocator<T> where T : struct
    {
        private static NativeAllocator<T> s_shared = new NativeAllocator<T>();

        public static NativeAllocator<T> Shared => s_shared;

        public MemoryType MemoryType => MemoryType.Native;

        public MemoryAllocatorType AllocatorType => MemoryAllocatorType.SingleUse;

        public OwnedMemory<T> Allocate(int count)
        {
            var memory = Marshal.AllocHGlobal(count * Unsafe.SizeOf<T>());

            return new NativeMemory(memory, count, this);
        }

        unsafe void IMemoryCollector<T>.Deallocate(OwnedMemory<T> buffer)
        {
            if (buffer?.Owner != this) throw new InvalidOperationException("buffer not allocated from this pool");

            void* pointer;
            buffer.Memory.TryGetPointer(out pointer);

            Marshal.FreeHGlobal((IntPtr)pointer);
        }

        public void Dispose()
        {
        }

        internal class NativeMemory : OwnedMemory<T>
        {
            public NativeMemory(IntPtr memory, int length, IMemoryCollector<T> owner) : base(null, 0, memory, length, owner)
            { }

            protected override void DisposeCore()
            { }
        }
    }

}
