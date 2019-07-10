// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.Data
{
    /// <summary>
    /// A basic mutable store to hold values in a DataFrame column. Supports wrapping with an ArrowBuffer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MutableDataFrameBuffer<T> : DataFrameBuffer<T>
        where T : struct
    {
        public Memory<byte> MutableMemory
        {
            get => _memory;
        }

        public Span<T> Span
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => MemoryMarshal.Cast<byte, T>(MutableMemory.Span);
        }

        public MutableDataFrameBuffer(int numberOfValues = 8) : base(numberOfValues) { }

        internal MutableDataFrameBuffer(ReadOnlyMemory<byte> buffer)
        {
            _size = Unsafe.SizeOf<T>();
            int capacity = buffer.Length / _size;
            _memory = new byte[capacity];
            buffer.CopyTo(_memory);

        }

        public void Append(T value)
        {
            if (Length == MaxCapacity)
            {
                throw new ArgumentException("Current buffer is full", nameof(value));
            }
            EnsureCapacity(1);
            Span[Length] = value;
            if (Length < MaxCapacity)
                ++Length;
        }

        public void EnsureCapacity(int numberOfValues)
        {
            long newLength = Length + (long)numberOfValues;
            if (newLength > MaxCapacity)
            {
                throw new ArgumentException("Current buffer is full", nameof(numberOfValues));
            }

            if (newLength > Capacity)
            {
                var newCapacity = Math.Max(newLength * _size, Memory.Length * 2);
                var memory = new Memory<byte>(new byte[newCapacity]);
                Memory.CopyTo(memory);
                _memory = memory;
            }
        }

        internal override T this[int index]
        {
            set
            {
                if (index > Length)
                    throw new ArgumentOutOfRangeException(nameof(index));
                Span[index] = value;
            }
        }

        internal static MutableDataFrameBuffer<T> GetMutableBuffer(DataFrameBuffer<T> buffer)
        {
            MutableDataFrameBuffer<T> mutableBuffer = buffer as MutableDataFrameBuffer<T>;
            if (ReferenceEquals(mutableBuffer, null))
            {
                mutableBuffer = new MutableDataFrameBuffer<T>(buffer.Memory);
                mutableBuffer.Length = buffer.Length;
            }
            return mutableBuffer;
        }
    }
}
