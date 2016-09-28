// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime;
using System.Runtime.CompilerServices;

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

        public Memory(T[] array) : this(array, 0, array?.Length ?? 0)
        {
        }

        public Memory(T[] array, int offset, int length)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            unsafe
            {
                _memory = null;
            }

            _array = array;
            _offset = offset;
            _memoryLength = length;
        }

        public unsafe Memory(T[] array, int offset, int length, void* pointer = null)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            unsafe
            {
                if (pointer != null)
                {
                    _memory = Unsafe.AsPointer(ref array[offset]);

                    if (_memory != pointer)
                    {
                        throw new ArgumentException(nameof(pointer));
                    }
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
                        void* pointer;
                        // This shouldn't fail
                        TryGetPointer(out pointer);
                        return new Span<T>(pointer, Length);
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

        public int Length => _memoryLength;

        public unsafe Memory<T> Slice(int offset, int length)
        {
            // TODO: Bounds check
            if (_array == null)
            {
                return new Memory<T>(Add(_memory, offset), length);
            }

            return new Memory<T>(_array, _offset + offset, length, _memory == null ? null : Add(_memory, offset));
        }

        public unsafe bool TryGetPointer(out void* pointer)
        {
            if (_memory == null)
            {
                pointer = null;
                return false;
            }

            pointer = _memory;
            return true;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void* Add(void* pointer, int offset)
        {
            return (byte*)pointer + ((ulong)UnsafeUtilities.SizeOf<T>() * (ulong)offset);
        }
    }
}
