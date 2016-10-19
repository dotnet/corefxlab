using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Buffers
{
    public sealed class OwnedArray<T> : OwnedMemory<T>
    {
        T[] _array;

        public T[] Array => _array;

        public static implicit operator T[](OwnedArray<T> owner)
        {
            return owner._array;
        }

        public static implicit operator OwnedArray<T>(T[] array)
        {
            return new OwnedArray<T>(array);
        }

        public OwnedArray(int length)
        {
            _array = new T[length];
        }

        public OwnedArray(T[] array)
        {
            Contract.Requires(array != null);
            _array = array;
        }

        protected override bool TryGetArrayCore(out ArraySegment<T> buffer)
        {
            buffer = new ArraySegment<T>(_array);
            return true;
        }

        protected override unsafe bool TryGetPointerCore(out void* pointer)
        {
            pointer = null;
            return false;
        }

        public override int Length => _array.Length;

        protected override void DisposeCore() => _array = null;
        protected override Span<T> GetSpanCore() => _array;
    }

    public class OwnedNativeMemory : OwnedMemory<byte>
    {
        IntPtr _memory;
        int _length;

        public OwnedNativeMemory(int length) : this(length, Marshal.AllocHGlobal(length))
        { }

        protected OwnedNativeMemory(int length, IntPtr address) {
            _memory = address;
            _length = length;
        }

        public static implicit operator IntPtr(OwnedNativeMemory owner)
        {
            return owner._memory;
        }

        ~OwnedNativeMemory()
        {
            DisposeCore();
        }

        protected override void DisposeCore()
        {
            if (_memory != IntPtr.Zero) {
                Free();
                _memory = IntPtr.Zero;
            }
        }

        protected virtual void Free()
        {
            Marshal.FreeHGlobal(_memory);
        }

        protected override Span<byte> GetSpanCore()
        {
            unsafe
            {
                return new Span<byte>(_memory.ToPointer(), _length);
            }
        }

        public override int Length => _length;

        public unsafe byte* Pointer => (byte*)_memory.ToPointer();

        protected override unsafe bool TryGetPointerCore(out void* pointer)
        {
            pointer = _memory.ToPointer();
            return true;
        }

        protected override bool TryGetArrayCore(out ArraySegment<byte> buffer)
        {
            buffer = default(ArraySegment<byte>);
            return false;
        }
    }

    // This is to support secnarios today covered by Memory<T> in corefxlab
    public class OwnedPinnedArray<T> : OwnedMemory<T>
    {
        private T[] _array;
        private unsafe void* _pointer;
        private GCHandle _handle;

        public unsafe OwnedPinnedArray(T[] array, void* pointer, GCHandle handle = default(GCHandle))
        {
            Contract.Requires(array != null);
            Contract.Requires(pointer != null);
            _array = array;
            _pointer = Unsafe.AsPointer(ref array[0]);
            _handle = handle;
            if(_pointer != pointer) {
                throw new InvalidOperationException();
            }
        }

        public unsafe OwnedPinnedArray(T[] array)
        {
            Contract.Requires(array != null);
            _array = array;
            _handle = GCHandle.Alloc(array, GCHandleType.Pinned);
            _pointer = _handle.AddrOfPinnedObject().ToPointer();
        }

        public static implicit operator OwnedPinnedArray<T>(T[] array)
        {
            return new OwnedPinnedArray<T>(array);
        }

        public unsafe byte* Pointer => (byte*)_pointer;
        public T[] Array => _array;

        public unsafe static implicit operator IntPtr(OwnedPinnedArray<T> owner)
        {
            return new IntPtr(owner._pointer);
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
            unsafe {
                _pointer = null;
            }
        }

        protected override Span<T> GetSpanCore()
        {
            return _array;
        }

        protected unsafe override bool TryGetPointerCore(out void* pointer)
        {
            pointer = _pointer;
            return true;
        }

        protected override bool TryGetArrayCore(out ArraySegment<T> buffer)
        {
            buffer = new ArraySegment<T>(_array, 0, _array.Length);
            return true;
        }

        public override int Length => _array.Length;
    }
}