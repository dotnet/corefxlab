using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Buffers
{
    public sealed class OwnedArray<T> : OwnedMemory<T>
    {
        T[] _array;
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

        protected override void DisposeCore() => _array = null;
        protected override Span<T> GetSpanCore() => _array;
    }

    public sealed class OwnedNativeMemory : OwnedMemory<byte>
    {
        IntPtr _memory;
        int _length;

        public OwnedNativeMemory(int length)
        {
            _length = length;
            _memory = Marshal.AllocHGlobal(length);
        }

        ~OwnedNativeMemory()
        {
            DisposeCore();
        }

        protected override void DisposeCore()
        {
            if (_memory != IntPtr.Zero) {
                Marshal.FreeHGlobal(_memory);
                _memory = IntPtr.Zero;
            }
        }

        protected override Span<byte> GetSpanCore()
        {
            unsafe
            {
                return new Span<byte>(_memory.ToPointer(), _length);
            }
        }

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

        public unsafe OwnedPinnedArray(T[] array, void* pointer)
        {
            Contract.Requires(array != null);
            Contract.Requires(pointer != null);
            _array = array;
            _pointer = Unsafe.AsPointer(ref array[0]);
            if(_pointer != pointer) {
                throw new InvalidOperationException();
            }
        }

        protected override void DisposeCore()
        {
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
    }
}