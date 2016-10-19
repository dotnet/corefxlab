// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Buffers
{
    /// <summary>
    /// Represents a Span<byte> factory</byte>
    /// </summary>
    /// <remarks>
    /// This struct is not safe for multithreaded access, i.e. it's subject to struct tearing that can result in unsafe memory access. 
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct Memory<T>
    {
        public static Memory<T> Empty = default(Memory<T>);

        private readonly T[] _array;
        private readonly int _memoryLength;
        private readonly int _offset;

        public Memory(T[] array) : this(array, 0, array?.Length ?? 0)
        {
        }

        public Memory(T[] array, int offset, int length)
        {
            Contract.RequiresNotNull(ExceptionArgument.array, array);

            _array = array;
            _memoryLength = length;
            _offset = offset;
        }

        // Used by UnsafeMemory
        internal Memory(int offset, int length)
        {
            _array = null;
            _memoryLength = length;
            _offset = offset;
        }

        // Used by UnsafeMemory
        internal Memory(int offset, int length, T[] array)
        {
            _array = array;
            _memoryLength = length;
            _offset = offset;
        }

        public bool IsArraySet => _array != null;

        public Span<T> Span => Length == 0 ? Span<T>.Empty : _array.Slice(_offset, Length);

        public bool IsEmpty => Length == 0;

        public static implicit operator Span<T>(Memory<T> memory)
        {
            return memory.Span;
        }

        public static implicit operator ReadOnlySpan<T>(Memory<T> memory)
        {
            return memory.Span;
        }

        public static implicit operator UnsafeMemory<T>(Memory<T> memory)
        {
            return new UnsafeMemory<T>(memory);
        }

        public int Length => _memoryLength;

        public Memory<T> Slice(int offset, int length)
        {
            // TODO: Bounds check
            return new Memory<T>(_array, _offset + offset, length);
        }

        internal Memory<T> SliceUnsafe(int offset, int length)
        {
            return new Memory<T>(_offset + offset, length, _array);
        }

        public Memory<T> Slice(int offset)
        {
            return Slice(offset, Length - offset);
        }

        public bool TryGetArray(out ArraySegment<T> buffer)
        {
            if (_array == null)
            {
                buffer = default(ArraySegment<T>);
                return false;
            }
            
            buffer = new ArraySegment<T>(_array, _offset, Length);
            return true;
        }
    }
}
