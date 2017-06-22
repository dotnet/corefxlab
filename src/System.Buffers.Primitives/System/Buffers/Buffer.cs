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
    public unsafe struct Buffer<T>
    {
        readonly void* _native;
        readonly object _arrayOrOwnedBuffer;
        readonly int _index;
        readonly int _length;

        internal Buffer(OwnedBuffer<T> owner, int index, int length)
        {
            _arrayOrOwnedBuffer = owner;
            _native = null;
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

            _arrayOrOwnedBuffer = array;
            _native = null;
            _index = 0;
            _length = array.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Buffer(object owner, void* pointer, int length)
        {
            _arrayOrOwnedBuffer = owner;
            _native = pointer;
            _index = 0;
            _length = length;
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

            _arrayOrOwnedBuffer = array;
            _native = null;
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

            _arrayOrOwnedBuffer = array;
            _native = null;
            _index = start;
            _length = length;
        }

        private Buffer(object owner, void* pointer, int index, int length)
        {
            _arrayOrOwnedBuffer = owner;
            _native = pointer;
            _index = index;
            _length = length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlyBuffer<T>(Buffer<T> buffer)
        {
            return new ReadOnlyBuffer<T>(buffer._arrayOrOwnedBuffer, buffer._native, buffer._index, buffer._length);
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

            return new Buffer<T>(_arrayOrOwnedBuffer, _native, _index + start, _length - start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Buffer<T> Slice(int start, int length)
        {
            if ((uint)start > (uint)_length || (uint)length > (uint)(_length - start))
                BufferPrimitivesThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);

            return new Buffer<T>(_arrayOrOwnedBuffer, _native, _index + start, length);
        }

        public Span<T> Span
        {
            get
            {
                if (_native == null) return new Span<T>(Unsafe.As<T[]>(_arrayOrOwnedBuffer), _index, _length);
                if (_arrayOrOwnedBuffer != null) return Unsafe.As<OwnedBuffer<T>>(_arrayOrOwnedBuffer).AsSpan(_index, _length);
                return new Span<T>(_native, _length); 
            }
        }

        public BufferHandle Retain(bool pin = false)
        {
            BufferHandle bufferHandle;
            if (pin)
            {
                if (_native == null)
                {
                    var handle = GCHandle.Alloc(Unsafe.As<T[]>(_arrayOrOwnedBuffer), GCHandleType.Pinned);
                    unsafe
                    {
                        var pointer = OwnedBuffer<T>.Add((void*)handle.AddrOfPinnedObject(), _index);
                        bufferHandle = new BufferHandle(null, pointer, handle);
                    }
                }
                else if (_arrayOrOwnedBuffer != null)
                {
                    bufferHandle = Unsafe.As<OwnedBuffer<T>>(_arrayOrOwnedBuffer).Pin(_index);
                }
                else
                {
                    bufferHandle = new BufferHandle(null);
                }
            }
            else
            {
                if (_native == null)
                {
                    bufferHandle = new BufferHandle((IRetainable)_arrayOrOwnedBuffer);
                }
                else if (_arrayOrOwnedBuffer != null)
                {
                    Unsafe.As<OwnedBuffer<T>>(_arrayOrOwnedBuffer).Retain();
                    bufferHandle = new BufferHandle(Unsafe.As<OwnedBuffer<T>>(_arrayOrOwnedBuffer));
                }
                else
                {
                    bufferHandle = new BufferHandle(null);
                }
            }
            return bufferHandle;
        }

        public bool TryGetArray(out ArraySegment<T> arraySegment)
        {
            if (_native != null)
            {
                var owner = Unsafe.As<OwnedBuffer<T>>(_arrayOrOwnedBuffer);
                if (owner.TryGetArray(out var segment))
                {
                    arraySegment = new ArraySegment<T>(segment.Array, segment.Offset + _index, _length);
                    return true;
                }
            }

            arraySegment = new ArraySegment<T>(Unsafe.As<T[]>(_arrayOrOwnedBuffer), _index, _length);
            return true;
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
                _arrayOrOwnedBuffer == other._arrayOrOwnedBuffer &&
                _native == other._native &&
                _index == other._index &&
                _length == other._length;
        }

        public static bool operator ==(Buffer<T> left, Buffer<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Buffer<T> left, Buffer<T> right)
        {
            return !left.Equals(right);
        }

        [EditorBrowsable( EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return HashingHelper.CombineHashCodes(_arrayOrOwnedBuffer.GetHashCode(), _index.GetHashCode(), _length.GetHashCode());
        }
    }
}
