// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

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

        public override BaseColumn Clone(BaseColumn mapIndices = null, bool invertMapIndices = false)
        {
            if (!(mapIndices is null))
            {
                if (mapIndices.DataType != typeof(long))
                    throw new ArgumentException(Strings.MismatchedValueType + " PrimitiveColumn<long>", nameof(mapIndices));
                if (mapIndices.Length > Length)
                    throw new ArgumentException(Strings.MapIndicesExceedsColumnLenth, nameof(mapIndices));
                return Clone(mapIndices as PrimitiveColumn<long>, invertMapIndices);
            }
            return Clone();
        }

        public override BaseColumn CloneAndAppendNulls(BaseColumn mapIndices = null, bool invertMapIndices = false)
        {
            PrimitiveColumn<T> ret = Clone(mapIndices, invertMapIndices) as PrimitiveColumn<T>;
            ret.AppendMany(null, NullCount);
            return ret;
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
                {
                    throw new ArgumentException(Strings.MapIndicesExceedsColumnLenth, nameof(mapIndices));
                }
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
