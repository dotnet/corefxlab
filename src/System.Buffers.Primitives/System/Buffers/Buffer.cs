// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
    [DebuggerTypeProxy(typeof(BufferDebuggerView<>))]
    public struct Buffer<T> : IEquatable<Buffer<T>>, IEquatable<ReadOnlyBuffer<T>>
    {
        readonly OwnedBuffer<T> _owner;
        readonly T[] _array;
        readonly int _index;
        readonly int _length;

        internal Buffer(OwnedBuffer<T> owner, int index, int length)
        {
            _array = null;
            _owner = owner;
            _index = index;
            _length = length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Buffer(T[] array)
        {
            if (array == null)
                BufferPrimitivesThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            if (default(T) == null && array.GetType() != typeof(T[]))
                BufferPrimitivesThrowHelper.ThrowArrayTypeMismatchException(typeof(T));

            _array = array;
            _owner = null;
            _index = 0;
            _length = array.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Buffer(T[] array, int start)
        {
            if (array == null)
                BufferPrimitivesThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            if (default(T) == null && array.GetType() != typeof(T[]))
                BufferPrimitivesThrowHelper.ThrowArrayTypeMismatchException(typeof(T));

            int arrayLength = array.Length;
            if ((uint)start > (uint)arrayLength)
                BufferPrimitivesThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);

            _array = array;
            _owner = null;
            _index = start;
            _length = arrayLength - start;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Buffer(T[] array, int start, int length)
        {
            if (array == null)
                BufferPrimitivesThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            if (default(T) == null && array.GetType() != typeof(T[]))
                BufferPrimitivesThrowHelper.ThrowArrayTypeMismatchException(typeof(T));
            if ((uint)start > (uint)array.Length || (uint)length > (uint)(array.Length - start))
                BufferPrimitivesThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);

            _array = array;
            _owner = null;
            _index = start;
            _length = length;
        }

        private Buffer(OwnedBuffer<T> owner, T[] array, int index, int length)
        {
            _array = array;
            _owner = owner;
            _index = index;
            _length = length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlyBuffer<T>(Buffer<T> buffer)
        {
            return new ReadOnlyBuffer<T>(buffer._owner, buffer._array, buffer._index, buffer._length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Buffer<T>(T[] array)
        {
            return new Buffer<T>(array, 0, array.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Buffer<T>(ArraySegment<T> arraySegment)
        {
            return new Buffer<T>(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
        }

        public static Buffer<T> Empty { get; } = OwnedBuffer<T>.EmptyArray;

        public int Length => _length;

        public bool IsEmpty => Length == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Buffer<T> Slice(int start)
        {
            if ((uint)start > (uint)_length)
                BufferPrimitivesThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);

            return new Buffer<T>(_owner, _array, _index + start, _length - start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Buffer<T> Slice(int start, int length)
        {
            if ((uint)start > (uint)_length || (uint)length > (uint)(_length - start))
                BufferPrimitivesThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);

            return new Buffer<T>(_owner, _array, _index + start, length);
        }

        public Span<T> Span
        {
            get {
                if (_array != null) return new Span<T>(_array, _index, _length);
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

        public bool TryGetArray(out ArraySegment<T> arraySegment)
        {
            if (_owner != null && _owner.TryGetArray(out var segment))
            {
                arraySegment = new ArraySegment<T>(segment.Array, segment.Offset + _index, _length);
                return true;
            }

            if (_array != null)
            {
                arraySegment = new ArraySegment<T>(_array, _index, _length);
                return true;
            }

            arraySegment = default(ArraySegment<T>);
            return false;
        }

        public T[] ToArray() => Span.ToArray();

        public void CopyTo(Span<T> span) => Span.CopyTo(span);

        public void CopyTo(Buffer<T> buffer) => Span.CopyTo(buffer.Span);

        public bool TryCopyTo(Span<T> span) => Span.TryCopyTo(span);

        public bool TryCopyTo(Buffer<T> buffer) => Span.TryCopyTo(buffer.Span);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            if(!(obj is Buffer<T>)) {
                return false;
            }

            var other = (Buffer<T>)obj;
            return Equals(other);
        }
        public bool Equals(Buffer<T> other)
        {
            return
                _array == other._array &&
                _owner == other._owner &&
                _index == other._index &&
                _length == other._length;
        }
        public bool Equals(ReadOnlyBuffer<T> other)
        {
            return other.Equals(this);
        }
        public static bool operator==(Buffer<T> left, Buffer<T> right)
        {
            return left.Equals(right);
        }
        public static bool operator!=(Buffer<T> left, Buffer<T> right)
        {
            return !left.Equals(right);
        }
        public static bool operator ==(Buffer<T> left, ReadOnlyBuffer<T> right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Buffer<T> left, ReadOnlyBuffer<T> right)
        {
            return !left.Equals(right);
        }

        [EditorBrowsable( EditorBrowsableState.Never)]
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
