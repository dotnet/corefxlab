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
    /// A basic immutable store to hold values in a DataFrame column. Supports wrapping with an ArrowBuffer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataFrameBuffer<T>
        where T : struct
    {
        // TODO: Change this to Memory<T>

        protected Memory<byte> _memory;
        public ReadOnlyMemory<byte> Memory => _memory;

        protected int _size;

        protected int Capacity => Memory.Length / _size;

        public int MaxCapacity => Int32.MaxValue / _size;

        public ReadOnlySpan<T> ReadOnlySpan
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => MemoryMarshal.Cast<byte, T>(Memory.Span);
        }

        public int Length { get; internal set; }

        public DataFrameBuffer(int numberOfValues = 8)
        {
            _size = Unsafe.SizeOf<T>();
            if ((long)numberOfValues * _size > MaxCapacity)
            {
                throw new ArgumentException($"{numberOfValues} exceeds buffer capacity", nameof(numberOfValues));
            }
            _memory = new byte[numberOfValues * _size];
        }

        internal virtual T this[int index]
        {
            get
            {
                if (index > Length)
                    throw new ArgumentOutOfRangeException(nameof(index));
                return ReadOnlySpan[index];
            }
            set => throw new NotSupportedException();
        }

        internal bool this[int startIndex, int length, IList<T> returnList]
        {
            get
            {
                if (startIndex > Length)
                    throw new ArgumentOutOfRangeException(nameof(startIndex));
                long endIndex = Math.Min(Length, startIndex + length);
                for (int i = startIndex; i < endIndex; i++)
                {
                    returnList.Add(ReadOnlySpan[i]);
                }
                return true;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            ReadOnlySpan<T> span = ReadOnlySpan;
            for (int i = 0; i < Length; i++)
            {
                sb.Append(span[i]).Append(" ");
            }
            return sb.ToString();
        }

    }
}
