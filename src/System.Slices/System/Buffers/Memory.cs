using System.Buffers;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Text;

namespace System
{
    [DebuggerTypeProxy(typeof(MemoryDebuggerView<>))]
    public struct Memory<T> : IEquatable<Memory<T>>, IEquatable<ReadOnlyMemory<T>>
    {
        IMemory<T> _owner;
        long _versionId;
        int _reservationId;
        int _index;
        int _length;

        internal Memory(IMemory<T> owner, long versionId, int reservationId)
            : this(owner, versionId, reservationId, 0, owner.GetSpan(versionId, reservationId).Length)
        { }

        private Memory(IMemory<T> owner, long versionId, int reservationId, int index, int length)
        {
            _owner = owner;
            _versionId = versionId;
            _reservationId = reservationId;
            _index = index;
            _length = length;
        }

        internal Memory(ref Memory<T> memory, int reservationId)
        {
            _owner = memory._owner;
            _versionId = memory._versionId;
            _reservationId = reservationId;
            _index = memory._index;
            _length = memory._length;
        }

        public static implicit operator ReadOnlyMemory<T>(Memory<T> memory)
        {
            return new ReadOnlyMemory<T>(memory._owner, memory._versionId, memory._reservationId, memory._index, memory._length);
        }

        public static implicit operator Memory<T>(T[] array)
        {
            var owner = new OwnedArray<T>(array);
            return owner.Memory;
        }

        public bool IsDisposed => _owner.IsDependancyDisposed(_versionId, _reservationId);

        public static Memory<T> Empty => OwnerEmptyMemory<T>.Shared.Memory;

        public int Length => _length;

        public bool IsEmpty => Length == 0;

        public Memory<T> Slice(int index)
        {
            _owner.ThrowIfDisposed(_versionId, _reservationId);
            return new Memory<T>(_owner, _versionId, _reservationId, _index + index, _length - index);
        }
        public Memory<T> Slice(int index, int length)
        {
            _owner.ThrowIfDisposed(_versionId, _reservationId);
            return new Memory<T>(_owner, _versionId, _reservationId, _index + index, length);
        }

        public Span<T> Span => _owner.GetSpan(_versionId, _reservationId).Slice(_index, _length);

        public ReservedMemory<T> Reserve() => _owner.Reserve(ref this, _versionId, _reservationId);

        public unsafe bool TryGetPointer(out void* pointer)
        {
            if (!_owner.TryGetDependancyPointer(_versionId, _reservationId, out pointer)) {
                return false;
            }
            pointer = Add(pointer, _index);
            return true;
        }

        public bool TryGetArray(out ArraySegment<T> buffer)
        {
            if (!_owner.TryGetDependancyArray(_versionId, _reservationId, out buffer)) {
                return false;
            }
            buffer = new ArraySegment<T>(buffer.Array, buffer.Offset + _index, _length);
            return true;
        }

        internal static unsafe void* Add(void* pointer, int offset)
        {
            return (byte*)pointer + ((ulong)Unsafe.SizeOf<T>() * (ulong)offset);
        }

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

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            if(!(obj is Memory<T>)) {
                return false;
            }

            var other = (Memory<T>)obj;
            return Equals(other);
        }
        public bool Equals(Memory<T> other)
        {
            return
                _owner == other._owner &&
                _versionId == other._versionId &&
                _index == other._index &&
                _length == other._length;
        }
        public bool Equals(ReadOnlyMemory<T> other)
        {
            return other.Equals(this);
        }
        public static bool operator==(Memory<T> left, Memory<T> right)
        {
            return left.Equals(right);
        }
        public static bool operator!=(Memory<T> left, Memory<T> right)
        {
            return left.Equals(right);
        }
        public static bool operator ==(Memory<T> left, ReadOnlyMemory<T> right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Memory<T> left, ReadOnlyMemory<T> right)
        {
            return left.Equals(right);
        }

        [EditorBrowsable( EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return HashingHelper.CombineHashCodes(_owner.GetHashCode(), _index.GetHashCode(), _versionId.GetHashCode(), _length.GetHashCode());
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            var data = Span;
            sb.Append("[");

            bool first = true;
            for(int i=0; i<Length; i++) {
                if (i > 7) break;
                if (first) first = false;
                else sb.Append(", ");
                sb.Append(data[i].ToString());
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}