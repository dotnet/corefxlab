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
    [DebuggerTypeProxy(typeof(ReadOnlyBufferDebuggerView<>))]
    public struct ReadOnlyBuffer<T>
    {
        readonly object _arrayOrOwnedBuffer;
        readonly int _index;
        readonly int _length;

        internal ReadOnlyBuffer(OwnedBuffer<T> owner, int index, int length)
        {
            _arrayOrOwnedBuffer = owner;
            _index = index | (1 << 31);
            _length = length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyBuffer(T[] array)
        {
            if (array == null)
                BufferPrimitivesThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);

            _arrayOrOwnedBuffer = array;
            _index = 0;
            _length = array.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyBuffer(T[] array, int start)
        {
            if (array == null)
                BufferPrimitivesThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);

            int arrayLength = array.Length;
            if ((uint)start > (uint)arrayLength)
                BufferPrimitivesThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);

            _arrayOrOwnedBuffer = array;
            _index = start;
            _length = arrayLength - start;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyBuffer(T[] array, int start, int length)
        {
            if (array == null)
                BufferPrimitivesThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            if ((uint)start > (uint)array.Length || (uint)length > (uint)(array.Length - start))
                BufferPrimitivesThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);

            _arrayOrOwnedBuffer = array;
            _index = start;
            _length = length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlyBuffer<T>(T[] array)
        {
            return new ReadOnlyBuffer<T>(array, 0, array.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlyBuffer<T>(ArraySegment<T> arraySegment)
        {
            return new ReadOnlyBuffer<T>(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
        }

        public static ReadOnlyBuffer<T> Empty { get; } = OwnedBuffer<T>.EmptyArray;

        public int Length => _length;

        public bool IsEmpty => Length == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyBuffer<T> Slice(int start)
        {
            if ((uint)start > (uint)_length)
                BufferPrimitivesThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);

            if (_index < 0)
                return new ReadOnlyBuffer<T>(Unsafe.As<OwnedBuffer<T>>(_arrayOrOwnedBuffer), _index + start, _length - start);
            return new ReadOnlyBuffer<T>(Unsafe.As<T[]>(_arrayOrOwnedBuffer), _index + start, _length - start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyBuffer<T> Slice(int start, int length)
        {
            if ((uint)start > (uint)_length || (uint)length > (uint)(_length - start))
                BufferPrimitivesThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);

            if (_index < 0)
                return new ReadOnlyBuffer<T>(Unsafe.As<OwnedBuffer<T>>(_arrayOrOwnedBuffer), _index + start, length);
            return new ReadOnlyBuffer<T>(Unsafe.As<T[]>(_arrayOrOwnedBuffer), _index + start, length);
        }

        public ReadOnlySpan<T> Span
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (_index < 0)
                    return Unsafe.As<OwnedBuffer<T>>(_arrayOrOwnedBuffer).AsSpan(_index & 0x7FFFFFFF, _length);
                return new ReadOnlySpan<T>(Unsafe.As<T[]>(_arrayOrOwnedBuffer), _index, _length);
            }
        }

        public BufferHandle Retain(bool pin = false)
        {
            BufferHandle bufferHandle;
            if (pin)
            {
                if (_index < 0)
                {
                    bufferHandle = Unsafe.As<OwnedBuffer<T>>(_arrayOrOwnedBuffer).Pin(_index & 0x7FFFFFFF);
                }
                else
                {
                    var handle = GCHandle.Alloc(Unsafe.As<T[]>(_arrayOrOwnedBuffer), GCHandleType.Pinned);
                    unsafe
                    {
                        var pointer = OwnedBuffer<T>.Add((void*)handle.AddrOfPinnedObject(), _index);
                        bufferHandle = new BufferHandle(null, pointer, handle);
                    }
                }
            }
            else
            {
                if (_index < 0)
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

        public T[] ToArray() => Span.ToArray();

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool DangerousTryGetArray(out ArraySegment<T> arraySegment)
        {
            if (_index < 0)
            {
                if (Unsafe.As<OwnedBuffer<T>>(_arrayOrOwnedBuffer).TryGetArray(out var segment))
                {
                    arraySegment = new ArraySegment<T>(segment.Array, segment.Offset + (_index & 0x7FFFFFFF), _length);
                    return true;
                }
            }
            else
            {
                arraySegment = new ArraySegment<T>(Unsafe.As<T[]>(_arrayOrOwnedBuffer), _index, _length);
                return true;
            }

            arraySegment = default;
            return false;
        }

        public void CopyTo(Span<T> span) => Span.CopyTo(span);

        public void CopyTo(Buffer<T> buffer) => Span.CopyTo(buffer.Span);

        public bool TryCopyTo(Span<T> span) => Span.TryCopyTo(span);

        public bool TryCopyTo(Buffer<T> buffer) => Span.TryCopyTo(buffer.Span);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            if (obj is ReadOnlyBuffer<T>)
            {
                var other = (ReadOnlyBuffer<T>)obj;
                return Equals(other);
            }
            else if (obj is Buffer<T>)
            {
                var other = (Buffer<T>)obj;
                return Equals(other);
            }
            else
            {
                return false;
            }
        }
        
        public bool Equals(ReadOnlyBuffer<T> other)
        {
            return
                _arrayOrOwnedBuffer == other._arrayOrOwnedBuffer &&
                (_index & 0x7FFFFFFF) == (other._index & 0x7FFFFFFF) &&
                _length == other._length;
        }

        public static bool operator ==(ReadOnlyBuffer<T> left, ReadOnlyBuffer<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ReadOnlyBuffer<T> left, ReadOnlyBuffer<T> right)
        {
            return !left.Equals(right);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return HashingHelper.CombineHashCodes(_arrayOrOwnedBuffer.GetHashCode(), (_index & 0x7FFFFFFF).GetHashCode(), _length.GetHashCode());
        }
    }
}
