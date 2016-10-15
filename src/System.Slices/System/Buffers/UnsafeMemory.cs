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
    public struct UnsafeMemory<T>
    {
        public static UnsafeMemory<T> Empty = default(UnsafeMemory<T>);

        private readonly unsafe void* _ptrMemory;
        private Memory<T> _memory;

        internal unsafe UnsafeMemory(Memory<T> memory)
        {
            _ptrMemory = null;
            _memory = memory;
        }

        public unsafe UnsafeMemory(void* pointer, int length)
        {
            Contract.RequiresNotNull(ExceptionArgument.pointer, pointer);

            _ptrMemory = pointer;
            _memory = new Memory<T>(0, length);
        }

        public UnsafeMemory(T[] array) : this(array, 0, array?.Length ?? 0)
        {
        }

        public UnsafeMemory(T[] array, int offset, int length)
        {
            _memory = new Memory<T>(array, offset, length);

            unsafe
            {
                _ptrMemory = null;
            }
        }

        public unsafe UnsafeMemory(T[] array, int offset, int length, void* pointer = null)
        {
            _memory = new Memory<T>(array, offset, length);

            unsafe
            {
                if (pointer != null)
                {
                    _ptrMemory = Unsafe.AsPointer(ref array[offset]);

                    Contract.RequiresSameReference(_ptrMemory, pointer);
                }
                else
                {
                    _ptrMemory = null;
                }
            }

        }

        internal unsafe UnsafeMemory(Memory<T> memory, void* pointer = null)
        {
            _memory = memory;
            _ptrMemory = pointer;
        }

        public Span<T> Span
        {
            get
            {
                if (Length == 0)
                {
                    return Span<T>.Empty;
                }

                if (_memory.IsArraySet)
                {
                    return _memory.Slice(Length);
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

        public int Length => _memory.Length;

        public unsafe UnsafeMemory<T> Slice(int offset, int length)
        {
            // TODO: Bounds check
            return new UnsafeMemory<T>(_memory.SliceUnsafe(offset, length), _ptrMemory == null ? null : Add(_ptrMemory, offset));
        }

        public UnsafeMemory<T> Slice(int offset)
        {
            return Slice(offset, Length - offset);
        }

        public unsafe bool TryGetPointer(out void* pointer)
        {
            if (_ptrMemory == null)
            {
                pointer = null;
                return false;
            }

            pointer = _ptrMemory;
            return true;
        }

        public bool TryGetArray(out ArraySegment<T> buffer)
        {
            return _memory.TryGetArray(out buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void* Add(void* pointer, int offset)
        {
            return (byte*)pointer + ((ulong)Unsafe.SizeOf<T>() * (ulong)offset);
        }
    }
}
