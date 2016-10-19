using System.Buffers;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System
{
    public struct Memory2<T> 
    {
        MemoryManager<T> _manager;
        long _id;
        int _index;
        int _length;

        internal Memory2(MemoryManager<T> manager, long id)
            : this(manager, id, 0, manager.GetSpan(id).Length)
        { }

        private Memory2(MemoryManager<T> manager, long id, int index, int length)
        {
            _manager = manager;
            _id = id;
            _index = index;
            _length = length;
        }

        public static Memory2<T> Empty => EmptyManager.Shared.Memory;

        public int Length => _length;

        public bool IsEmpty => Length == 0;

        public Memory2<T> Slice(int index)
        {
            return new Memory2<T>(_manager, _id, _index + index, _length - index);
        }
        public Memory2<T> Slice(int index, int length)
        {
            return new Memory2<T>(_manager, _id, _index + index, length);
        }

        public Span<T> Span => _manager.GetSpan(_id).Slice(_index, _length);

        public DisposableReservation Reserve() => new DisposableReservation(_manager, _id);

        public unsafe bool TryGetPointer(out void* pointer)
        {
            if (!_manager.TryGetPointer(_id, out pointer)) {
                return false;
            }
            pointer = Add(pointer, _index);
            return true;
        }

        public bool TryGetArray(out ArraySegment<T> buffer)
        {
            if (!_manager.TryGetArray(_id, out buffer)) {
                return false;
            }
            buffer = new ArraySegment<T>(buffer.Array, buffer.Offset + _index, _length);
            return true;
        }

        public struct DisposableReservation : IDisposable
        {
            MemoryManager<T> _manager;
            long _id;

            internal DisposableReservation(MemoryManager<T> manager, long id)
            {
                _id = id;
                _manager = manager;
                _manager.AddReference(_id);
            }

            public void Dispose()
            {
                _manager.ReleaseReference(_id);
                _manager = null;
            }
        }

        class EmptyManager : MemoryManager<T>
        {
            public readonly static MemoryManager<T> Shared = new EmptyManager();
            readonly static ArraySegment<T> s_empty = new ArraySegment<T>(new T[0], 0, 0);
              
            protected override bool TryGetArrayCore(out ArraySegment<T> buffer)
            {
                buffer = s_empty;
                return true;
            }

            protected override unsafe bool TryGetPointerCore(out void* pointer)
            {
                pointer = null;
                return false;
            }

            protected override void DisposeCore()
            { }

            protected override Span<T> GetSpanCore()
            {
                return Span<T>.Empty;
            }
        }

        static unsafe void* Add(void* pointer, int offset)
        {
            return (byte*)pointer + ((ulong)Unsafe.SizeOf<T>() * (ulong)offset);
        }
    }
}