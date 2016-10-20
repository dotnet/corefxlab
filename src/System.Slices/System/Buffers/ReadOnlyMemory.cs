using System.Buffers;

namespace System
{
    public struct ReadOnlyMemory<T>
    {
        OwnedMemory<T> _owner;
        long _id;
        int _index;
        int _length;

        internal ReadOnlyMemory(OwnedMemory<T> owner, long id)
            : this(owner, id, 0, owner.GetSpan(id).Length)
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
            var owner = new OwnedMemory<T>(array);
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

        public DisposableReservation Reserve() => new DisposableReservation(_owner, _id);

        public unsafe bool TryGetPointer(out void* pointer)
        {
            if (!_owner.TryGetPointer(_id, out pointer)) {
                return false;
            }
            pointer = Memory<T>.Add(pointer, _index);
            return true;
        }

        /// <summary>
        /// Array segment for the memory instance.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="dummy">This parameter is here just to make the API unsafe. Feel free to pass null.</param>
        /// <returns></returns>
        public unsafe bool TryGetArray(out ArraySegment<T> buffer, void* dummy)
        {
            if (!_owner.TryGetArray(_id, out buffer)) {
                return false;
            }
            buffer = new ArraySegment<T>(buffer.Array, buffer.Offset + _index, _length);
            return true;
        }
    }
}