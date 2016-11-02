using System.Buffers;
using System.Diagnostics;
using System.Runtime;

namespace System
{
    [DebuggerTypeProxy(typeof(ReadOnlyMemoryDebuggerView<>))]
    public struct ReadOnlyMemory<T>
    {
        OwnedMemory<T> _owner;
        long _id;
        int _index;
        int _length;

        internal ReadOnlyMemory(OwnedMemory<T> owner, long id)
            : this(owner, id, 0, owner.GetSpanInternal(id).Length)
        { }

        internal ReadOnlyMemory(OwnedMemory<T> owner, long id, int index, int length)
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

        public ReadOnlySpan<T> Span => _owner.GetSpanInternal(_id).Slice(_index, _length);

        public DisposableReservation Reserve()
        {
            return _owner.Reserve(ref this);
        }
   
        public unsafe bool TryGetPointer(out void* pointer)
        {
            if (!_owner.TryGetPointerInternal(_id, out pointer)) {
                return false;
            }
            pointer = Memory<T>.Add(pointer, _index);
            return true;
        }

        public unsafe bool TryGetArray(out ArraySegment<T> buffer)
        {
            if (!_owner.TryGetArrayInternal(_id, out buffer)) {
                return false;
            }
            buffer = new ArraySegment<T>(buffer.Array, buffer.Offset + _index, _length);
            return true;
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