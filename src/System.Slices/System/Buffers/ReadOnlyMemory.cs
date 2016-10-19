using System.Buffers;
using System.Runtime.CompilerServices;
using System.Threading;

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

        public DisposableReservation Reserve() => new DisposableReservation(_owner, _id);

        public struct DisposableReservation : IDisposable
        {
            OwnedMemory<T> _owner;
            long _id;

            internal DisposableReservation(OwnedMemory<T> owner, long id)
            {
                _id = id;
                _owner = owner;
                _owner.AddReference(_id);
            }

            public void Dispose()
            {
                _owner.ReleaseReference(_id);
                _owner = null;
            }
        }
    }
}