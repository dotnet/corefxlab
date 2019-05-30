// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Microsoft.Data
{
    /// <summary>
    /// PrimitiveDataFrameColumnContainer is just a store for the column data. APIs that want to change the data must be defined in PrimitiveDataFrameColumn
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal partial class PrimitiveColumnContainer<T>
        where T : struct
    {
        public IList<DataFrameBuffer<T>> Buffers = new List<DataFrameBuffer<T>>();

        // To keep the mapping simple, each buffer is mapped 1v1 to a nullBitMapBuffer
        // A set bit implies a valid value. An unset bit => null value
        public IList<DataFrameBuffer<byte>> NullBitMapBuffers = new List<DataFrameBuffer<byte>>();

        // Need a way to differentiate between columns initialized with default values and those with null values in SetValidityBit
        internal bool _modifyNullCountWhileIndexing = true;

        public PrimitiveColumnContainer(T[] values)
        {
            values = values ?? throw new ArgumentNullException(nameof(values));
            long length = values.LongLength;
            DataFrameBuffer<T> curBuffer;
            if (Buffers.Count == 0)
            {
                curBuffer = new DataFrameBuffer<T>();
                Buffers.Add(curBuffer);
                NullBitMapBuffers.Add(new DataFrameBuffer<byte>());
            }
            else
            {
                curBuffer = Buffers[Buffers.Count - 1];
            }
            for (long i = 0; i < length; i++)
            {
                if (curBuffer.Length == curBuffer.MaxCapacity)
                {
                    curBuffer = new DataFrameBuffer<T>();
                    Buffers.Add(curBuffer);
                    NullBitMapBuffers.Add(new DataFrameBuffer<byte>());
                }
                curBuffer.Append(values[i]);
                SetValidityBit(Length, true);
                Length++;
            }
        }

        public PrimitiveColumnContainer(IEnumerable<T> values)
        {
            values = values ?? throw new ArgumentNullException(nameof(values));
            if (Buffers.Count == 0)
            {
                Buffers.Add(new DataFrameBuffer<T>());
                NullBitMapBuffers.Add(new DataFrameBuffer<byte>());
            }
            DataFrameBuffer<T> curBuffer = Buffers[Buffers.Count - 1];
            foreach (T value in values)
            {
                if (curBuffer.Length == curBuffer.MaxCapacity)
                {
                    curBuffer = new DataFrameBuffer<T>();
                    Buffers.Add(curBuffer);
                    NullBitMapBuffers.Add(new DataFrameBuffer<byte>());
                }
                curBuffer.Append(value);
                SetValidityBit(Length, true);
                Length++;
            }
        }

        public PrimitiveColumnContainer(long length = 0)
        {
            while (length > 0)
            {
                if (Buffers.Count == 0)
                {
                    Buffers.Add(new DataFrameBuffer<T>());
                    NullBitMapBuffers.Add(new DataFrameBuffer<byte>());
                }
                DataFrameBuffer<T> lastBuffer = Buffers[Buffers.Count - 1];
                if (lastBuffer.Length == lastBuffer.MaxCapacity)
                {
                    lastBuffer = new DataFrameBuffer<T>();
                    Buffers.Add(lastBuffer);
                    NullBitMapBuffers.Add(new DataFrameBuffer<byte>());
                }
                int allocatable = (int)Math.Min(length, lastBuffer.MaxCapacity);
                lastBuffer.EnsureCapacity(allocatable);
                DataFrameBuffer<byte> lastNullBitMapBuffer = NullBitMapBuffers[NullBitMapBuffers.Count - 1];
                lastNullBitMapBuffer.EnsureCapacity((int)Math.Ceiling(allocatable / 8.0));
                lastBuffer.Length = allocatable;
                lastNullBitMapBuffer.Length = allocatable;
                length -= allocatable;
                Length += lastBuffer.Length;
            }
        }

        public void Append(T? value)
        {
            if (Buffers.Count == 0)
            {
                Buffers.Add(new DataFrameBuffer<T>());
                NullBitMapBuffers.Add(new DataFrameBuffer<byte>());
            }
            DataFrameBuffer<T> lastBuffer = Buffers[Buffers.Count - 1];
            if (lastBuffer.Length == lastBuffer.MaxCapacity)
            {
                lastBuffer = new DataFrameBuffer<T>();
                Buffers.Add(lastBuffer);
                NullBitMapBuffers.Add(new DataFrameBuffer<byte>());
            }
            lastBuffer.Append(value ?? default);
            SetValidityBit(Length, value.HasValue ? true : false);
            Length++;
        }

        public void AppendMany(T? value, long count)
        {
            if (!value.HasValue)
            {
                NullCount += count;
            }
            while (count > 0)
            {
                if (Buffers.Count == 0)
                {
                    Buffers.Add(new DataFrameBuffer<T>());
                    NullBitMapBuffers.Add(new DataFrameBuffer<byte>());
                }
                DataFrameBuffer<T> lastBuffer = Buffers[Buffers.Count - 1];
                if (lastBuffer.Length == lastBuffer.MaxCapacity)
                {
                    lastBuffer = new DataFrameBuffer<T>();
                    Buffers.Add(lastBuffer);
                    NullBitMapBuffers.Add(new DataFrameBuffer<byte>());
                }
                int allocatable = (int)Math.Min(count, lastBuffer.MaxCapacity);
                lastBuffer.EnsureCapacity(allocatable);
                lastBuffer.Span.Slice(lastBuffer.Length, allocatable).Fill(value ?? default);
                lastBuffer.Length += allocatable;
                Length += allocatable;

                DataFrameBuffer<byte> lastNullBitMapBuffer = NullBitMapBuffers[NullBitMapBuffers.Count - 1];
                int nullBitMapAllocatable = (int)Math.Ceiling(allocatable / 8.0);
                lastNullBitMapBuffer.EnsureCapacity(nullBitMapAllocatable);
                _modifyNullCountWhileIndexing = false;
                for (long i = Length - count; i < Length; i++)
                {
                    SetValidityBit(i, value.HasValue ? true : false);
                }
                _modifyNullCountWhileIndexing = true;
                lastNullBitMapBuffer.Length += nullBitMapAllocatable;
                count -= allocatable;
            }
        }

        public bool IsValid(long index) => NullCount == 0 || GetValidityBit(index);

        /// <summary>
        /// A null value has an unset bit
        /// A NON-null value has a set bit
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        private void SetValidityBit(long index, bool value)
        {
            if ((uint)index > Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            // First find the right bitMapBuffer
            int bitMapIndex = (int)(index / Buffers[0].MaxCapacity);
            Debug.Assert(NullBitMapBuffers.Count > bitMapIndex);
            DataFrameBuffer<byte> bitMapBuffer = NullBitMapBuffers[bitMapIndex];

            // Set the bit
            index -= bitMapIndex * Buffers[0].MaxCapacity;
            int bitMapBufferIndex = (int)((uint)index / 8);
            Debug.Assert(bitMapBuffer.Length >= bitMapBufferIndex);
            if (bitMapBuffer.Length == bitMapBufferIndex)
                bitMapBuffer.Append(0);
            byte curBitMap = bitMapBuffer[bitMapBufferIndex];
            byte newBitMap;
            if (value)
            {
                newBitMap = (byte)(curBitMap | (byte)(1 << (int)(index % 8)));
                if (_modifyNullCountWhileIndexing && (curBitMap >> ((int)(index % 8)) & 1) == 0 && index < Length && NullCount > 0)
                {
                    // Old value was null.
                    NullCount--;
                }
            }
            else
            {
                if (_modifyNullCountWhileIndexing && (curBitMap >> ((int)(index % 8)) & 1) == 1 && index < Length)
                {
                    // old value was NOT null and new value is null
                    NullCount++;
                }
                else if (_modifyNullCountWhileIndexing && index == Length)
                {
                    // New entry from an append
                    NullCount++;
                }
                newBitMap = (byte)(curBitMap & (byte)~(1 << (int)((uint)index % 8)));
            }
            bitMapBuffer[bitMapBufferIndex] = newBitMap;
        }

        private bool GetValidityBit(long index)
        {
            if ((uint)index >= Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            // First find the right bitMapBuffer
            int bitMapIndex = (int)(index / Buffers[0].MaxCapacity);
            Debug.Assert(NullBitMapBuffers.Count > bitMapIndex);
            DataFrameBuffer<byte> bitMapBuffer = NullBitMapBuffers[bitMapIndex];

            // Get the bit
            index -= bitMapIndex * Buffers[0].MaxCapacity;
            int bitMapBufferIndex = (int)((uint)index / 8);
            Debug.Assert(bitMapBuffer.Length > bitMapBufferIndex);
            byte curBitMap = bitMapBuffer[bitMapBufferIndex];
            return ((curBitMap >> ((int)index % 8)) & 1) != 0;
        }

        public long Length;

        public long NullCount;
        private int GetArrayContainingRowIndex(ref long rowIndex)
        {
            if (rowIndex > Length)
            {
                throw new ArgumentOutOfRangeException(Strings.ColumnIndexOutOfRange, nameof(rowIndex));
            }
            return (int)(rowIndex / Buffers[0].MaxCapacity);
        }

        public IList<T?> this[long startIndex, int length]
        {
            get
            {
                var ret = new List<T?>(length);
                long endIndex = Math.Min(Length, startIndex + length);
                for (long i = startIndex; i < endIndex; i++)
                {
                    ret.Add(this[i]);
                }
                return ret;
            }
        }

        public T? this[long rowIndex]
        {
            get
            {
                if (!IsValid(rowIndex))
                {
                    return null;
                }
                int arrayIndex = GetArrayContainingRowIndex(ref rowIndex);
                return Buffers[arrayIndex][(int)rowIndex];
            }
            set
            {
                int arrayIndex = GetArrayContainingRowIndex(ref rowIndex);
                if (value.HasValue)
                {
                    Buffers[arrayIndex][(int)rowIndex] = value.Value;
                    SetValidityBit(rowIndex, true);
                }
                else
                {
                    Buffers[arrayIndex][(int)rowIndex] = default;
                    SetValidityBit(rowIndex, false);
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Length; i++)
            {
                T? value = this[i];
                if (value.HasValue)
                {
                    sb.Append(this[i]).Append(" ");
                }
                else
                {
                    sb.Append("null").Append(" ");
                }
                // Can this run out of memory? Just being safe here
                if (sb.Length > 1000)
                {
                    sb.Append("...");
                    break;
                }
            }
            return sb.ToString();
        }

        private List<DataFrameBuffer<byte>> CloneNullBitMapBuffers()
        {
            List<DataFrameBuffer<byte>> ret = new List<DataFrameBuffer<byte>>();
            foreach (DataFrameBuffer<byte> buffer in NullBitMapBuffers)
            {
                DataFrameBuffer<byte> newBuffer = new DataFrameBuffer<byte>();
                ret.Add(newBuffer);
                Span<byte> span = buffer.Span;
                for (int i = 0; i < buffer.Length; i++)
                {
                    newBuffer.Append(span[i]);
                }
            }
            return ret;
        }

        public PrimitiveColumnContainer<T> Clone()
        {
            var ret = new PrimitiveColumnContainer<T>();
            foreach (DataFrameBuffer<T> buffer in Buffers)
            {
                DataFrameBuffer<T> newBuffer = new DataFrameBuffer<T>();
                ret.Buffers.Add(newBuffer);
                Span<T> span = buffer.Span;
                ret.Length += buffer.Length;
                for (int i = 0; i < buffer.Length; i++)
                {
                    newBuffer.Append(span[i]);
                }
            }
            ret.NullBitMapBuffers = CloneNullBitMapBuffers();
            ret.NullCount = NullCount;
            return ret;
        }

        internal PrimitiveColumnContainer<bool> CloneAsBoolContainer()
        {
            var ret = new PrimitiveColumnContainer<bool>();
            foreach (DataFrameBuffer<T> buffer in Buffers)
            {
                DataFrameBuffer<bool> newBuffer = new DataFrameBuffer<bool>();
                ret.Buffers.Add(newBuffer);
                newBuffer.EnsureCapacity(buffer.Length);
                newBuffer.Span.Fill(false);
                newBuffer.Length = buffer.Length;
                ret.Length += buffer.Length;
            }
            ret.NullBitMapBuffers = CloneNullBitMapBuffers();
            ret.NullCount = NullCount;
            return ret;
        }

        internal PrimitiveColumnContainer<double> CloneAsDoubleContainer()
        {
            var ret = new PrimitiveColumnContainer<double>();
            foreach (DataFrameBuffer<T> buffer in Buffers)
            {
                ret.Length += buffer.Length;
                DataFrameBuffer<double> newBuffer = new DataFrameBuffer<double>();
                ret.Buffers.Add(newBuffer);
                newBuffer.EnsureCapacity(buffer.Length);
                Span<T> span = buffer.Span;
                for (int i = 0; i < buffer.Length; i++)
                {
                    newBuffer.Append(DoubleConverter<T>.Instance.GetDouble(span[i]));
                }
            }
            ret.NullBitMapBuffers = CloneNullBitMapBuffers();
            ret.NullCount = NullCount;
            return ret;
        }

        internal PrimitiveColumnContainer<decimal> CloneAsDecimalContainer()
        {
            var ret = new PrimitiveColumnContainer<decimal>();
            foreach (DataFrameBuffer<T> buffer in Buffers)
            {
                ret.Length += buffer.Length;
                DataFrameBuffer<decimal> newBuffer = new DataFrameBuffer<decimal>();
                ret.Buffers.Add(newBuffer);
                newBuffer.EnsureCapacity(buffer.Length);
                Span<T> span = buffer.Span;
                for (int i = 0; i < buffer.Length; i++)
                {
                    newBuffer.Append(DecimalConverter<T>.Instance.GetDecimal(span[i]));
                }
            }
            ret.NullBitMapBuffers = CloneNullBitMapBuffers();
            ret.NullCount = NullCount;
            return ret;
        }
    }
}
