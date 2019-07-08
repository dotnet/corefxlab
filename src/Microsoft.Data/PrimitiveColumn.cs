// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Apache.Arrow;
using Microsoft.Collections.Extensions;

namespace Microsoft.Data
{
    /// <summary>
    /// A column to hold primitive types such as int, float etc.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class PrimitiveColumn<T> : BaseColumn
        where T : unmanaged
    {
        private PrimitiveColumnContainer<T> _columnContainer;

        internal int NumberOfBuffers => _columnContainer.Buffers.Count;
        internal Memory<byte> Buffer(int i) => _columnContainer.Buffers[i].Memory;
        internal Memory<byte> NullBuffer(int i) => _columnContainer.NullBitMapBuffers[i].Memory;

        internal Apache.Arrow.Array AsArrowArray(int i)
        {
            Debug.Assert(i < _columnContainer.Buffers.Count);
            DataFrameBuffer<T> values = _columnContainer.Buffers[i];
            DataFrameBuffer<byte> nulls = _columnContainer.NullBitMapBuffers[i];
            ArrowBuffer valueBuffer = new ArrowBuffer(values.Memory);
            // The following line is wrong, but C# Arrow does not have good null support yet
            ArrowBuffer nullBuffer = nulls.Length == 0 ? ArrowBuffer.Empty : new ArrowBuffer(nulls.Memory);
            switch (this)
            {
                case PrimitiveColumn<bool> boolColumn:
                    return new BooleanArray(valueBuffer, nullBuffer, values.Length, -1, 0);
                case PrimitiveColumn<double> doubleColumn:
                    return new DoubleArray(valueBuffer, nullBuffer, values.Length, -1, 0);
                case PrimitiveColumn<float> floatColumn:
                    return new FloatArray(valueBuffer, nullBuffer, values.Length, -1, 0);
                case PrimitiveColumn<int> intColumn:
                    return new Int32Array(valueBuffer, nullBuffer, values.Length, -1, 0);
                case PrimitiveColumn<long> longColumn:
                    return new Int64Array(valueBuffer, nullBuffer, values.Length, -1, 0);
                case PrimitiveColumn<short> shortColumn:
                    return new Int16Array(valueBuffer, nullBuffer, values.Length, -1, 0);
                case PrimitiveColumn<uint> uintColumn:
                    return new UInt32Array(valueBuffer, nullBuffer, values.Length, -1, 0);
                case PrimitiveColumn<ulong> ulongColumn:
                    return new UInt64Array(valueBuffer, nullBuffer, values.Length, -1, 0);
                case PrimitiveColumn<ushort> ushortColumn:
                    return new UInt16Array(valueBuffer, nullBuffer, values.Length, -1, 0);
                case PrimitiveColumn<byte> byteColumn:
                case PrimitiveColumn<char> charColumn:
                case PrimitiveColumn<decimal> decimalColumn:
                case PrimitiveColumn<sbyte> sbyteColumn:
                default:
                    throw new NotImplementedException(nameof(byteColumn.DataType));
            }
        }

        internal PrimitiveColumn(string name, PrimitiveColumnContainer<T> column) : base(name, column.Length, typeof(T))
        {
            _columnContainer = column;
        }

        public PrimitiveColumn(string name, IEnumerable<T> values) : base(name, 0, typeof(T))
        {
            _columnContainer = new PrimitiveColumnContainer<T>(values);
            Length = _columnContainer.Length;
        }

        public PrimitiveColumn(string name, long length = 0) : base(name, length, typeof(T))
        {
            _columnContainer = new PrimitiveColumnContainer<T>(length);
        }

        public PrimitiveColumn(string name, Memory<byte> memory, Memory<byte> nullBitMap, int length = 0) : base(name, length, typeof(T))
        {
            _columnContainer = new PrimitiveColumnContainer<T>(memory, nullBitMap, length);
        }

        public new IList<T?> this[long startIndex, int length]
        {
            get
            {
                if (startIndex > Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(startIndex));
                }
                return _columnContainer[startIndex, length];
            }
        }

        protected override object GetValue(long startIndex, int length)
        {
            if (startIndex > Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }
            return _columnContainer[startIndex, length];
        }

        protected override object GetValue(long rowIndex)
        {
            return _columnContainer[rowIndex];
        }

        protected override void SetValue(long rowIndex, object value)
        {
            if (value == null || value.GetType() == typeof(T))
            {
                _columnContainer[rowIndex] = (T?)value;
            }
            else
            {
                throw new ArgumentException(Strings.MismatchedValueType + $" {DataType.ToString()}", nameof(value));
            }
        }

        public new T? this[long rowIndex]
        {
            get
            {
                return _columnContainer[rowIndex];
            }
            set
            {
                if (value == null || value.GetType() == typeof(T))
                {
                    _columnContainer[rowIndex] = value;
                }
                else
                {
                    throw new ArgumentException(Strings.MismatchedValueType + $" {DataType.ToString()}", nameof(value));
                }
            }
        }

        public void Append(T? value)
        {
            _columnContainer.Append(value);
            Length++;
        }

        public void AppendMany(T? value, long count)
        {
            _columnContainer.AppendMany(value, count);
            Length += count;
        }

        public override long NullCount
        {
            get
            {
                Debug.Assert(_columnContainer.NullCount >= 0);
                return _columnContainer.NullCount;
            }
        }

        public bool IsValid(long index) => _columnContainer.IsValid(index);

        public override string ToString()
        {
            return $"{Name}: {_columnContainer.ToString()}";
        }

        public override BaseColumn Clone(BaseColumn mapIndices = null, bool invertMapIndices = false, long numberOfNullsToAppend = 0)
        {
            PrimitiveColumn<T> clone;
            if (!(mapIndices is null))
            {
                if (mapIndices.DataType != typeof(long))
                    throw new ArgumentException(Strings.MismatchedValueType + " PrimitiveColumn<long>", nameof(mapIndices));
                if (mapIndices.Length > Length)
                    throw new ArgumentException(Strings.MapIndicesExceedsColumnLenth, nameof(mapIndices));
                clone = Clone(mapIndices as PrimitiveColumn<long>, invertMapIndices);
            }
            else
            {
                clone = Clone();
            }
            Debug.Assert(!ReferenceEquals(clone, null));
            clone.AppendMany(null, numberOfNullsToAppend);
            return clone;
        }

        public PrimitiveColumn<T> Clone(PrimitiveColumn<long> mapIndices = null, bool invertMapIndices = false)
        {
            if (mapIndices is null)
            {
                PrimitiveColumnContainer<T> newColumnContainer = _columnContainer.Clone();
                return new PrimitiveColumn<T>(Name, newColumnContainer);
            }
            else
            {
                if (mapIndices.Length > Length)
                    throw new ArgumentException(Strings.MapIndicesExceedsColumnLenth, nameof(mapIndices));

                PrimitiveColumn<T> ret = new PrimitiveColumn<T>(Name, mapIndices.Length);
                ret._columnContainer._modifyNullCountWhileIndexing = false;
                if (invertMapIndices == false)
                {
                    for (long i = 0; i < mapIndices.Length; i++)
                    {
                        T? value = _columnContainer[mapIndices._columnContainer[i].Value];
                        ret[i] = value;
                        if (!value.HasValue)
                            ret._columnContainer.NullCount++;
                    }
                }
                else
                {
                    long mapIndicesIndex = mapIndices.Length - 1;
                    for (long i = 0; i < mapIndices.Length; i++)
                    {
                        T? value = _columnContainer[mapIndices._columnContainer[mapIndicesIndex - i].Value];
                        ret[i] = value;
                        if (!value.HasValue)
                            ret._columnContainer.NullCount++;
                    }
                }
                ret._columnContainer._modifyNullCountWhileIndexing = true;
                return ret;
            }
        }

        internal PrimitiveColumn<bool> CloneAsBoolColumn()
        {
            PrimitiveColumnContainer<bool> newColumnContainer = _columnContainer.CloneAsBoolContainer();
            return new PrimitiveColumn<bool>(Name, newColumnContainer);
        }

        internal PrimitiveColumn<double> CloneAsDoubleColumn()
        {
            PrimitiveColumnContainer<double> newColumnContainer = _columnContainer.CloneAsDoubleContainer();
            return new PrimitiveColumn<double>(Name, newColumnContainer);
        }

        internal PrimitiveColumn<decimal> CloneAsDecimalColumn()
        {
            PrimitiveColumnContainer<decimal> newColumnContainer = _columnContainer.CloneAsDecimalContainer();
            return new PrimitiveColumn<decimal>(Name, newColumnContainer);
        }

        public void ApplyElementwise(Func<T, T> func)
        {
            foreach (DataFrameBuffer<T> buffer in _columnContainer.Buffers)
            {
                Span<T> span = buffer.Span;
                for (int i = 0; i < buffer.Length; i++)
                {
                    span[i] = func(span[i]);
                }
            }
        }
    }
}
