using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Buffers
{
    public sealed class OwnedArray<T> : OwnedMemory<T>
    {
        public OwnedArray(int length, IMemoryDisposer<T> owner = null) : this(new T[length], owner) { }

        public OwnedArray(T[] array, IMemoryDisposer<T> owner = null) : base(array, 0, IntPtr.Zero, array.Length, owner) { }

        public unsafe T[] Array
        {
            get
            {
                ArraySegment<T> ownedArray;
                Memory.TryGetArray(out ownedArray, null);
                return ownedArray.Array;
            }
        }

        public static implicit operator T[](OwnedArray<T> owner) => owner.Array;

        public static implicit operator OwnedArray<T>(T[] array)
        {
            return new OwnedArray<T>(array);
        }

        protected override void DisposeCore() {}
    }

    public class OwnedNativeMemory : OwnedMemory<byte>
    {
        public OwnedNativeMemory(int length, IMemoryDisposer<byte> owner = null) : this(length, Marshal.AllocHGlobal(length), owner)
        { }

        protected OwnedNativeMemory(int length, IntPtr address, IMemoryDisposer<byte> owner = null) : base(null, 0, address, length, owner) { }

        public static unsafe implicit operator IntPtr(OwnedNativeMemory owner)
        {
            return new IntPtr(owner.Pointer);
        }

        ~OwnedNativeMemory()
        {
            DisposeCore();
        }

        protected override void DisposeCore()
        {
            Marshal.FreeHGlobal(this);
        }

        public unsafe byte* Pointer
        {
            get
            {
                void* pointer;
                Memory.TryGetPointer(out pointer);
                return (byte*)pointer;
            }
        }
    }

    // This is to support secnarios today covered by Memory<T> in corefxlab
    public class OwnedPinnedArray<T> : OwnedMemory<T>
    {
        private GCHandle _handle;

        public unsafe OwnedPinnedArray(T[] array, void* pointer, GCHandle handle = default(GCHandle), IMemoryDisposer<T> owner = null) :
            base(array, 0, new IntPtr(pointer), array.Length, owner)
        {
            Contract.RequiresSameReference(pointer, Unsafe.AsPointer(ref array[0]));

            _handle = handle;
        }

        public OwnedPinnedArray(T[] array, IMemoryDisposer<T> owner = null) : base(owner)
        {
            _handle = GCHandle.Alloc(array, GCHandleType.Pinned);

            ReInitialize(array, 0, _handle.AddrOfPinnedObject(), array.Length);
        }

        public static implicit operator OwnedPinnedArray<T>(T[] array)
        {
            return new OwnedPinnedArray<T>(array);
        }

        public unsafe byte* Pointer
        {
            get
            {
                void* pointer;
                Memory.TryGetPointer(out pointer);
                return (byte*)pointer;
            }
        }

        public unsafe T[] Array
        {
            get
            {
                ArraySegment<T> ownedArray;
                Memory.TryGetArray(out ownedArray, null);
                return ownedArray.Array;
            }
        }

        public static unsafe implicit operator IntPtr(OwnedPinnedArray<T> owner)
        {
            return new IntPtr(owner.Pointer);
        }

        public static implicit operator T[] (OwnedPinnedArray<T> owner)
        {
            return owner.Array;
        }

        protected override void DisposeCore()
        {
            if (_handle.IsAllocated) {
                _handle.Free();
            }
        }
    }
}