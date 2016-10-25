using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Buffers
{
    public sealed class OwnedArray<T> : OwnedMemory<T>
    {
        public T[] Array => _array;

        public static implicit operator T[](OwnedArray<T> owner) {
            return owner._array;
        }

        public static implicit operator OwnedArray<T>(T[] array) {
            return new OwnedArray<T>(array);
        }

        public OwnedArray(int length) : base(new T[length], 0, length)
        {}

        public OwnedArray(T[] array) : base(array, 0, array.Length)
        {}

        protected override void DisposeCore() => _array = null;
    }

    public class OwnedNativeMemory : OwnedMemory<byte>
    {
        public OwnedNativeMemory(int length) : this(length, Marshal.AllocHGlobal(length))
        { }

        protected OwnedNativeMemory(int length, IntPtr address) : base(null, 0, length, address) { }

        public static implicit operator IntPtr(OwnedNativeMemory owner)
        {
            return owner._pointer;
        }

        ~OwnedNativeMemory()
        {
            DisposeCore();
        }

        protected override void DisposeCore()
        {
            if (_pointer != IntPtr.Zero) {
                Free();
                _pointer = IntPtr.Zero;
            }
        }

        protected virtual void Free()
        {
            Marshal.FreeHGlobal(_pointer);
        }

        public unsafe byte* Pointer => (byte*)_pointer.ToPointer();
    }

    // This is to support secnarios today covered by Memory<T> in corefxlab
    public class OwnedPinnedArray<T> : OwnedMemory<T>
    {
        private GCHandle _handle;

        public unsafe OwnedPinnedArray(T[] array, void* pointer, GCHandle handle = default(GCHandle)) :
            base(array, 0, array.Length, new IntPtr(pointer))
        {
            var computedPointer = new IntPtr(Unsafe.AsPointer(ref _array[0]));
            if (computedPointer != new IntPtr(pointer)) {
                throw new InvalidOperationException();
            }
            _handle = handle;
        }

        public unsafe OwnedPinnedArray(T[] array) : base(array, 0, array.Length, IntPtr.Zero)
        {
            _handle = GCHandle.Alloc(array, GCHandleType.Pinned);
            _pointer = _handle.AddrOfPinnedObject();
        }

        public static implicit operator OwnedPinnedArray<T>(T[] array)
        {
            return new OwnedPinnedArray<T>(array);
        }

        public unsafe byte* Pointer => (byte*)_pointer;
        public T[] Array => _array;

        public unsafe static implicit operator IntPtr(OwnedPinnedArray<T> owner)
        {
            return owner._pointer;
        }

        public static implicit operator T[] (OwnedPinnedArray<T> owner)
        {
            return owner._array;
        }

        protected override void DisposeCore()
        {
            if (_handle.IsAllocated) {
                _handle.Free();
            }
            _array = null;
            unsafe
            {
                _pointer = IntPtr.Zero;
            }
        }
    }

    internal class OwnerEmptyMemory<T> : OwnedMemory<T>
    {
        public readonly static OwnedMemory<T> Shared = new OwnerEmptyMemory<T>();
        readonly static T[] s_empty = new T[0];

        public OwnerEmptyMemory() : base(s_empty, 0, 0) { }

        protected override void DisposeCore()
        { }
    }
}