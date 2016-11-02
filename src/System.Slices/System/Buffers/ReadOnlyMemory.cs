using System.Buffers;
using System.Diagnostics;
using System.Runtime;

namespace System
{
    [DebuggerTypeProxy(typeof(ReadOnlyMemoryDebuggerView<>))]
    public struct ReadOnlyMemory<T>
    {
        IReadOnlyMemory<T> _owner;
        long _id;
        int _index;
        int _length;

        public ReadOnlyMemory(IReadOnlyMemory<T> owner, long id)
            : this(owner, id, 0, owner.GetSpan(id).Length)
        { }

        internal ReadOnlyMemory(IReadOnlyMemory<T> owner, long id, int index, int length)
        {
            _owner = owner;
            _id = id;
            _index = index;
            _length = length;
        }

        public static implicit operator ReadOnlyMemory<T>(T[] array)
        {
            var owner = new OwnedArray<T>(array);
            return owner.Memory;
        }

        public static ReadOnlyMemory<T> Empty => Memory<T>.Empty;

        public int Length => _length;

        public bool IsEmpty => Length == 0;

        public ReadOnlyMemory<T> Slice(int index)
        {
            return new ReadOnlyMemory<T>(_owner, _id, _index + index, _length - index);
        }
        public ReadOnlyMemory<T> Slice(int index, int length)
        {
            return new ReadOnlyMemory<T>(_owner, _id, _index + index, length);
        }

        public ReadOnlySpan<T> Span => _owner.GetSpan(_id).Slice(_index, _length);

        public DisposableReservation Reserve()
        {
            return _owner.Reserve(ref this);
        }
   
        public unsafe bool TryGetPointer(out void* pointer)
        {
            if (!_owner.TryGetPointer(_id, out pointer)) {
                return false;
            }
            pointer = Memory<T>.Add(pointer, _index);
            return true;
        }

        public unsafe bool TryGetArray(out ArraySegment<T> buffer)
        {
            buffer = default(ArraySegment<T>);
            return false;
        }

        public T[] ToArray()
        {
            return Span.ToArray();
        }

        public void CopyTo(Span<T> span)
        {
            Span.CopyTo(span);
        }
    }
}