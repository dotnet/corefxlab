// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
        readonly OwnedMemory<T> _owner;
        readonly int _index;
        readonly int _length;

        internal Memory(OwnedMemory<T> owner, int length)
        {
            _owner = owner;
            _index = 0;
            _length = length;
        }

        private Memory(OwnedMemory<T> owner, int index, int length)
        {
            _owner = owner;
            _index = index;
            _length = length;
        }

        public static implicit operator ReadOnlyMemory<T>(Memory<T> memory)
        {
            return new ReadOnlyMemory<T>(memory._owner, memory._index, memory._length);
        }

        public static implicit operator Memory<T>(T[] array)
        {
            var owner = new OwnedArray<T>(array);
            return owner.Memory;
        }

        public static Memory<T> Empty { get; } = OwnerEmptyMemory<T>.Shared.Memory;

        public int Length => _length;

        public bool IsEmpty => Length == 0;

        public Memory<T> Slice(int index)
        {
            return new Memory<T>(_owner, _index + index, _length - index);
        }
        public Memory<T> Slice(int index, int length)
        {
            return new Memory<T>(_owner, _index + index, length);
        }

        public Span<T> Span => _owner.GetSpanInternal(_index, _length);

        public DisposableReservation<T> Reserve() => new DisposableReservation<T>(_owner);

        public unsafe MemoryHandle GetPinnedMemoryHandle() => MemoryHandle.Create(_owner, _index);

        public unsafe bool TryGetPointer(out void* pointer)
        {
            if (!_owner.TryGetPointerInternal(out pointer)) {
                return false;
            }
            pointer = Add(pointer, _index);
            return true;
        }

        public bool TryGetArray(out ArraySegment<T> buffer)
        {
            if (!_owner.TryGetArrayInternal(out buffer)) {
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
            return !left.Equals(right);
        }
        public static bool operator ==(Memory<T> left, ReadOnlyMemory<T> right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Memory<T> left, ReadOnlyMemory<T> right)
        {
            return !left.Equals(right);
        }

        [EditorBrowsable( EditorBrowsableState.Never)]
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
            for(i=0; i<Length; i++) {
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
