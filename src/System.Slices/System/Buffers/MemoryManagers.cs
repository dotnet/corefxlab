using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Buffers
{
    public sealed class ArrayManager<T> : MemoryManager<T>
    {
        T[] _array;
        public ArrayManager(int length)
        {
            _array = new T[length];
        }

        public ArrayManager(T[] array)
        {
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

    public sealed class NativeMemoryManager : MemoryManager<byte>
    {
        IntPtr _memory;
        int _length;

        public NativeMemoryManager(int length)
        {
            _length = length;
            _memory = Marshal.AllocHGlobal(length);
        }

        ~NativeMemoryManager()
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

    // THis is to support secnarios today covered by Memory<T> in corefxlab
    public class PinnedArrayManager<T> : MemoryManager<T>
    {
        private T[] _array;
        private unsafe void* _pointer;

        public unsafe PinnedArrayManager(T[] array, void* pointer)
        {
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
            if (_array != null) {
                return _array;
            }
            else {
                unsafe
                {
                    return new Span<T>(_pointer, _array.Length);
                }
            }
        }

        protected unsafe override bool TryGetPointerCore(out void* pointer)
        {
            if (_pointer == null) {
                pointer = null;
                return false;
            }

            pointer = _pointer;
            return true;
        }

        protected override bool TryGetArrayCore(out ArraySegment<T> buffer)
        {
            if (_array == null) {
                buffer = default(ArraySegment<T>);
                return false;
            }
            buffer = new ArraySegment<T>(_array, 0, _array.Length);
            return true;
        }
    }
}