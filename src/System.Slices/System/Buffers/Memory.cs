﻿using System.Buffers;
using System.Runtime.CompilerServices;

namespace System
{
    public struct Memory<T> 
    {
        OwnedMemory<T> _owner;
        long _id;
        int _index;
        int _length;

        internal Memory(OwnedMemory<T> owner, long id)
            : this(owner, id, 0, owner.GetSpan(id).Length)
        { }

        private Memory(OwnedMemory<T> owner, long id, int index, int length)
        {
            _owner = owner;
            _id = id;
            _index = index;
            _length = length;
        }

        public static implicit operator ReadOnlyMemory<T>(Memory<T> memory)
        {
            return new ReadOnlyMemory<T>(memory._owner, memory._id, memory._index, memory._length);
        }

        public static implicit operator Memory<T>(T[] array)
        {
            var owner = new OwnedMemory<T>(array);
            return owner.Memory;
        }

        public static Memory<T> Empty => OwnedMemory<T>.Empty.Memory;

        public int Length => _length;

        public bool IsEmpty => Length == 0;

        public Memory<T> Slice(int index)
        {
            return new Memory<T>(_owner, _id, _index + index, _length - index);
        }
        public Memory<T> Slice(int index, int length)
        {
            return new Memory<T>(_owner, _id, _index + index, length);
        }

        public Span<T> Span => _owner.GetSpan(_id).Slice(_index, _length);

        public DisposableReservation Reserve() => new DisposableReservation(_owner, _id);

        public unsafe bool TryGetPointer(out void* pointer)
        {
            if (!_owner.TryGetPointer(_id, out pointer)) {
                return false;
            }
            pointer = Add(pointer, _index);
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

        internal static unsafe void* Add(void* pointer, int offset)
        {
            return (byte*)pointer + ((ulong)Unsafe.SizeOf<T>() * (ulong)offset);
        }
    }

    public struct DisposableReservation : IDisposable
    {
        IKnown _owner;
        long _id;

        internal DisposableReservation(IKnown owner, long id)
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