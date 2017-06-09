// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Buffers
{
    [DebuggerTypeProxy(typeof(ReadOnlyBufferDebuggerView<>))]
    public struct ReadOnlyBuffer<T> : IEquatable<ReadOnlyBuffer<T>>, IEquatable<Buffer<T>>
    {
        readonly OwnedBuffer<T> _owner;
        readonly T[] _array;
        readonly int _index;
        readonly int _length;

        internal ReadOnlyBuffer(OwnedBuffer<T> owner,int index, int length)
        {
            _array = null;
            _owner = owner;
            _index = index;
            _length = length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyBuffer(T[] array) : this(array, 0, array.Length)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyBuffer(T[] array, int start) : this(array, start, array.Length - start)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyBuffer(T[] array, int start, int length)
        {
            if (array == null)
                BufferPrimitivesThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            if ((uint)start > (uint)array.Length || (uint)length > (uint)(array.Length - start))
                BufferPrimitivesThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);

            _array = array;
            _owner = null;
            _index = start;
            _length = length;
        }

        private ReadOnlyBuffer(OwnedBuffer<T> owner, T[] array, int index, int length)
        {
            _array = array;
            _owner = owner;
            _index = index;
            _length = length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlyBuffer<T>(T[] array)
        {
            return new ReadOnlyBuffer<T>(array, 0, array.Length);
        }

        public static ReadOnlyBuffer<T> Empty { get; } = OwnedBuffer<T>.EmptyArray;

        public int Length => _length;

        public bool IsEmpty => Length == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyBuffer<T> Slice(int index)
        {
            return new ReadOnlyBuffer<T>(_owner, _array, _index + index, _length - index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyBuffer<T> Slice(int index, int length)
        {
            return new ReadOnlyBuffer<T>(_owner, _array, _index + index, length);
        }

        public ReadOnlySpan<T> Span
        {
            get {
                if (_array != null) return new ReadOnlySpan<T>(_array, _index, _length);
                return _owner.AsSpan(_index, _length);
            }
        }

        public BufferHandle Retain()
        {
            if (_owner != null)
            {
                _owner.Retain();
                return new BufferHandle(_owner);
            }
            return new BufferHandle();
        }

        public BufferHandle Pin()
        {
            if (_owner != null)
            {
                return _owner.Pin(_index);
            }
            var handle = GCHandle.Alloc(_array, GCHandleType.Pinned);
            unsafe
            {
                var pointer = OwnedBuffer<T>.Add((void*)handle.AddrOfPinnedObject(), _index);
                return new BufferHandle(null, pointer, handle);
            }
        }

        public T[] ToArray() => Span.ToArray();

        public void CopyTo(Span<T> span) => Span.CopyTo(span);

        public void CopyTo(Buffer<T> buffer) => Span.CopyTo(buffer.Span);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            if (!(obj is Buffer<T>)) {
                return false;
            }

            var other = (Buffer<T>)obj;
            return Equals(other);
        }

        public bool Equals(Buffer<T> other)
        {
            return Equals((ReadOnlyBuffer<T>)other);
        }

        public bool Equals(ReadOnlyBuffer<T> other)
        {
            return
                _array == other._array &&
                _owner == other._owner &&
                _index == other._index &&
                _length == other._length;
        }

        public static bool operator ==(ReadOnlyBuffer<T> left, Buffer<T> right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(ReadOnlyBuffer<T> left, Buffer<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator ==(ReadOnlyBuffer<T> left, ReadOnlyBuffer<T> right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(ReadOnlyBuffer<T> left, ReadOnlyBuffer<T> right)
        {
            return left.Equals(right);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            if (_owner != null)
            {
                return HashingHelper.CombineHashCodes(_owner.GetHashCode(), _index.GetHashCode(), _length.GetHashCode());
            }
            return HashingHelper.CombineHashCodes(_array.GetHashCode(), _index.GetHashCode(), _length.GetHashCode());
        }
    }
}
