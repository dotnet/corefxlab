using System.Buffers;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime;
using System.Text;

namespace System
{
    [DebuggerTypeProxy(typeof(ReadOnlyMemoryDebuggerView<>))]
    public struct ReadOnlyMemory<T> : IEquatable<ReadOnlyMemory<T>>, IEquatable<Memory<T>>
    {
        IReadOnlyMemory<T> _owner;
        long _versionId;
        int _reservationId;
        int _index;
        int _length;

        internal ReadOnlyMemory(IReadOnlyMemory<T> owner, long versionId, int reservationId)
            : this(owner, versionId, reservationId, 0, owner.GetSpan(versionId, reservationId).Length)
        { }

        internal ReadOnlyMemory(IReadOnlyMemory<T> owner, long versionId, int reservationId, int index, int length)
        {
            _owner = owner;
            _versionId = versionId;
            _reservationId = reservationId;
            _index = index;
            _length = length;
        }

        internal ReadOnlyMemory(ref ReadOnlyMemory<T> memory, int reservationId)
        {
            _owner = memory._owner;
            _versionId = memory._versionId;
            _reservationId = reservationId;
            _index = memory._index;
            _length = memory._length;
        }

        public static implicit operator ReadOnlyMemory<T>(T[] array)
        {
            var owner = new OwnedArray<T>(array);
            return owner.Memory;
        }

        public bool IsDisposed => _owner.IsDependancyDisposed(_versionId, _reservationId);

        public static ReadOnlyMemory<T> Empty => Memory<T>.Empty;

        public int Length => _length;

        public bool IsEmpty => Length == 0;

        public ReadOnlyMemory<T> Slice(int index)
        {
            _owner.ThrowIfDisposed(_versionId, _reservationId);
            return new ReadOnlyMemory<T>(_owner, _versionId, _reservationId, _index + index, _length - index);
        }
        public ReadOnlyMemory<T> Slice(int index, int length)
        {
            _owner.ThrowIfDisposed(_versionId, _reservationId);
            return new ReadOnlyMemory<T>(_owner, _versionId, _reservationId, _index + index, length);
        }

        public ReadOnlySpan<T> Span => _owner.GetSpan(_versionId, _reservationId).Slice(_index, _length);

        public ReservedReadOnlyMemory<T> Reserve() => _owner.Reserve(ref this, _versionId, _reservationId);

        public T[] ToArray()
        {
            return Span.ToArray();
        }

        public void CopyTo(Span<T> span)
        {
            Span.CopyTo(span);
        }

        public void CopyTo(Memory<T> memory)
        {
            Span.CopyTo(memory.Span);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Memory<T>)) {
                return false;
            }

            var other = (Memory<T>)obj;
            return Equals(other);
        }

        public bool Equals(Memory<T> other)
        {
            return Equals((ReadOnlyMemory<T>)other);
        }

        public bool Equals(ReadOnlyMemory<T> other)
        {
            return
                _owner == other._owner &&
                _reservationId == other._reservationId &&
                _index == other._index &&
                _length == other._length;
        }

        public static bool operator ==(ReadOnlyMemory<T> left, Memory<T> right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(ReadOnlyMemory<T> left, Memory<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator ==(ReadOnlyMemory<T> left, ReadOnlyMemory<T> right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(ReadOnlyMemory<T> left, ReadOnlyMemory<T> right)
        {
            return left.Equals(right);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return HashingHelper.CombineHashCodes(_owner.GetHashCode(), _index.GetHashCode(), _reservationId.GetHashCode(), _length.GetHashCode());
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            var data = Span;
            sb.Append("[");

            bool first = true;
            for (int i = 0; i < Length; i++) {
                if (i > 7) break;
                if (first) first = false;
                else sb.Append(", ");
                sb.Append(data[0].ToString());
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}