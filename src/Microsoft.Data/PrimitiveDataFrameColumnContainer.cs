// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Data
{
    /// <summary>
    /// PrimitiveDataFrameColumnContainer is just a store for the column data. APIs that want to change the data must be defined in PrimitiveDataFrameColumn
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal partial class PrimitiveDataFrameColumnContainer<T>
        where T : struct
    {
        public IList<DataFrameBuffer<T>> Buffers = new List<DataFrameBuffer<T>>();
        public PrimitiveDataFrameColumnContainer(T[] values)
        {
            values = values ?? throw new ArgumentNullException(nameof(values));
            long length = values.LongLength;
            DataFrameBuffer<T> curBuffer;
            if (Buffers.Count == 0)
            {
                curBuffer = new DataFrameBuffer<T>();
                Buffers.Add(curBuffer);
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
                }
                curBuffer.Append(values[i]);
                Length++;
            }
        }

        public PrimitiveDataFrameColumnContainer(IEnumerable<T> values)
        {
            values = values ?? throw new ArgumentNullException(nameof(values));
            if (Buffers.Count == 0)
            {
                Buffers.Add(new DataFrameBuffer<T>());
            }
            var curBuffer = Buffers[Buffers.Count - 1];
            foreach (var value in values)
            {
                if (curBuffer.Length == curBuffer.MaxCapacity)
                {
                    curBuffer = new DataFrameBuffer<T>();
                    Buffers.Add(curBuffer);
                }
                curBuffer.Append(value);
                Length++;
            }
        }

        public PrimitiveDataFrameColumnContainer() { }

        public long Length;
        //TODO:
        public long NullCount => throw new NotImplementedException();
        private int GetArrayContainingRowIndex(ref long rowIndex)
        {
            if (rowIndex > Length)
            {
                throw new ArgumentException($"Index {rowIndex} cannot be greater than the Column's Length {Length}");
            }
            int curArrayIndex = 0;
            int numBuffers = Buffers.Count;
            while (curArrayIndex < numBuffers && rowIndex > Buffers[curArrayIndex].Length)
            {
                rowIndex -= Buffers[curArrayIndex].Length;
                curArrayIndex++;
            }
            return curArrayIndex;
        }

        public IList<T> this[long startIndex, int length]
        {
            get
            {
                var ret = new List<T>(length);
                long endIndex = startIndex + length;
                int arrayIndex = GetArrayContainingRowIndex(ref startIndex);
                bool temp = Buffers[arrayIndex][(int)startIndex, length, ret];
                while (ret.Count < length)
                {
                    arrayIndex++;
                    temp = Buffers[arrayIndex][0, length - ret.Count, ret];
                }
                return ret;
            }
        }

        public T this[long rowIndex]
        {
            get
            {
                int arrayIndex = GetArrayContainingRowIndex(ref rowIndex);
                return Buffers[arrayIndex][(int)rowIndex];
            }
            set
            {
                int arrayIndex = GetArrayContainingRowIndex(ref rowIndex);
                Buffers[arrayIndex][(int)rowIndex] = value;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var buffer in Buffers)
            {
                sb.Append(buffer.ToString());
                // Can this run out of memory? Just being safe here
                if (sb.Length > 1000)
                {
                    sb.Append("...");
                    break;
                }
            }
            return sb.ToString();
        }
        public PrimitiveDataFrameColumnContainer<T> Clone()
        {
            var ret = new PrimitiveDataFrameColumnContainer<T>();
            foreach (DataFrameBuffer<T> buffer in Buffers)
            {
                DataFrameBuffer<T> newBuffer = new DataFrameBuffer<T>();
                ret.Buffers.Add(newBuffer);
                var span = buffer.Span;
                ret.Length += buffer.Length;
                for (int i = 0; i < buffer.Length; i++)
                {
                    newBuffer.Append(span[i]);
                }
            }
            return ret;
        }

        internal PrimitiveDataFrameColumnContainer<bool> CreateBoolContainerForCompareOps()
        {
            var ret = new PrimitiveDataFrameColumnContainer<bool>();
            foreach (var buffer in Buffers)
            {
                DataFrameBuffer<bool> newBuffer = new DataFrameBuffer<bool>();
                ret.Buffers.Add(newBuffer);
                newBuffer.EnsureCapacity(buffer.Length);
                newBuffer.Span.Fill(false);
                newBuffer.Length = buffer.Length;
                ret.Length += buffer.Length;
            }
            return ret;
        }
    }
}
