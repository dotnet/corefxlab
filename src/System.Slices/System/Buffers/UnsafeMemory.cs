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
    public struct UnsafeMemory<T>
    {
        public static unsafe UnsafeMemory<T> Empty = default(UnsafeMemory<T>);

        private readonly T[] _array;
        private readonly int _offset;
        private readonly unsafe void* _memory;
        private readonly int _memoryLength;

        public unsafe UnsafeMemory(void* pointer, int length)
        {
            Contract.RequiresNotNull(ExceptionArgument.pointer, pointer);

            _memory = pointer;
            _array = null;
            _offset = 0;
            _memoryLength = length;
        }

        public UnsafeMemory(T[] array) : this(array, 0, array?.Length ?? 0)
        {
        }

        public UnsafeMemory(T[] array, int offset, int length)
        {
            Contract.RequiresNotNull(ExceptionArgument.array, array);

            unsafe
            {
                _memory = null;
            }

            _array = array;
            _offset = offset;
            _memoryLength = length;
        }

        public unsafe UnsafeMemory(T[] array, int offset, int length, void* pointer = null)
        {
            Contract.RequiresNotNull(ExceptionArgument.array, array);

            unsafe
            {
                if (pointer != null)
                {
                    _memory = Unsafe.AsPointer(ref array[offset]);

                    Contract.RequiresSameReference(_memory, pointer);
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

        internal unsafe UnsafeMemory(int offset, int length, T[] array, void* pointer = null)
        {
            Contract.RequiresOneNotNull(array, pointer);

            unsafe
            {
                if (array != null && pointer != null)
                {
                    _memory = Unsafe.AsPointer(ref array[offset]);

                    Contract.RequiresSameReference(_memory, pointer);
                }
                else
                {
                    _memory = pointer;
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

        public static implicit operator Span<T>(UnsafeMemory<T> memory)
        {
            return memory.Span;
        }

        public static implicit operator ReadOnlySpan<T>(UnsafeMemory<T> memory)
        {
            return memory.Span;
        }

        public int Length => _memoryLength;

        public unsafe UnsafeMemory<T> Slice(int offset, int length)
        {
            // TODO: Bounds check
            return new UnsafeMemory<T>(_offset + offset, length, _array, _memory == null ? null : Add(_memory, offset));
        }

        public unsafe UnsafeMemory<T> Slice(int offset)
        {
            return Slice(offset, Length - offset);
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
            return (byte*)pointer + ((ulong)Unsafe.SizeOf<T>() * (ulong)offset);
        }
    }
}
