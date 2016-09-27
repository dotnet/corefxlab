// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime;

namespace System.Buffers
{
    /// <summary>
    /// Represents a Span<byte> factory</byte>
    /// </summary>
    /// <remarks>
    /// This struct is not safe for multithreaded access, i.e. it's subject to struct tearing that can result in unsafe memory access. 
    /// </remarks>
    public struct Memory<T>
    {
        public static unsafe Memory<T> Empty = new Memory<T>(null, 0, 0);

        private readonly T[] _array;
        private readonly int _offset;
        private readonly unsafe void* _memory;
        private readonly int _memoryLength;

        public unsafe Memory(void* pointer, int length)
        {
            unsafe
            {
                _memory = pointer;
            }

            _array = null;
            _offset = 0;
            _memoryLength = length;
        }

        public unsafe Memory(T[] array) : this(array, 0, array.Length)
        {
        }

        public unsafe Memory(T[] array, int offset, int length, bool isPinned = false)
        {
            unsafe
            {
                if (isPinned)
                {
                    // The caller pinned the array so we can safely get the pointer
                    _memory = (void*)UnsafeUtilities.ComputeAddress(array, new UIntPtr(0));
                }
                else
                {
                    _memory = null;
                }
            }

            _array = array;
            _offset = offset;
            _memoryLength = length;
        }

        public Span<T> Span
        {
            get
            {
                if (Length == 0)
                {
                    return Span<T>.Empty;
                }

                if (_array != null)
                {
                    return _array.Slice(_offset, Length);
                }
                else
                {
                    unsafe
                    {
                        return new Span<T>(UnsafePointer, Length);
                    }
                }
            }
        }

        public bool IsEmpty => Length == 0;

        public static implicit operator Span<T>(Memory<T> memory)
        {
            return memory.Span;
        }

        public static implicit operator ReadOnlySpan<T>(Memory<T> memory)
        {
            return memory.Span;
        }

        public unsafe void* UnsafePointer
        {
            get
            {
                if (_memory == null)
                {
                    throw new InvalidOperationException("The native pointer isn't available because the memory isn't pinned");
                }

                return (byte*)_memory + (UnsafeUtilities.SizeOf<T>() * _offset);
            }
        }

        public int Length => _memoryLength;

        public unsafe Memory<T> Slice(int offset, int length)
        {
            // TODO: Bounds check
            if (_array == null)
            {
                return new Memory<T>((byte*)_memory + (UnsafeUtilities.SizeOf<T>() * offset), length);
            }

            return new Memory<T>(_array, _offset + offset, length, _memory != null);
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
