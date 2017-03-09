// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
        readonly OwnedMemory<T> _owner;
        readonly int _index;
        readonly int _length;

        internal ReadOnlyMemory(OwnedMemory<T> owner, int length)
        {
            _owner = owner;
            _index = 0;
            _length = length;
        }

        internal ReadOnlyMemory(OwnedMemory<T> owner,int index, int length)
        {
            _owner = owner;
            _index = index;
            _length = length;
        }

        public static implicit operator ReadOnlyMemory<T>(T[] array)
        {
            var owner = new OwnedArray<T>(array);
            return owner.Memory;
        }

        public static ReadOnlyMemory<T> Empty { get; } = OwnerEmptyMemory<T>.Shared.Memory;

        public int Length => _length;

        public bool IsEmpty => Length == 0;

        public ReadOnlyMemory<T> Slice(int index)
        {
            return new ReadOnlyMemory<T>(_owner, _index + index, _length - index);
        }
        public ReadOnlyMemory<T> Slice(int index, int length)
        {
            return new ReadOnlyMemory<T>(_owner, _index + index, length);
        }

        public ReadOnlySpan<T> Span => _owner.GetSpanInternal(_index, _length);

        public DisposableReservation<T> Reserve()
        {
            return _owner.Memory.Reserve();
        }

        public unsafe MemoryHandle Pin() => MemoryHandle.Create(_owner, _index);
   
        public unsafe bool TryGetPointer(out void* pointer)
        {
            if (!_owner.TryGetPointerInternal(out pointer)) {
                return false;
            }
            pointer = Memory<T>.Add(pointer, _index);
            return true;
        }

        public unsafe bool TryGetArray(out ArraySegment<T> buffer)
        {
            if (!_owner.TryGetArrayInternal(out buffer)) {
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
            return HashingHelper.CombineHashCodes(_owner.GetHashCode(), _index.GetHashCode(), _length.GetHashCode());
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            var data = Span;
            sb.Append("[");

            bool first = true;
            int i;
            for (i = 0; i < Length; i++)
            {
                if (i > 7) break;
                if (first) first = false;
                else sb.Append(", ");
                sb.Append(data[i].ToString());
            }
            if (i < Span.Length)
            {
                sb.Append(",...");
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}