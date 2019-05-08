// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

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

        public PrimitiveColumn(string name, long length = 0, bool isNullable = true) : base(name, length, typeof(T))
        {
            _columnContainer = new PrimitiveColumnContainer<T>(length);
        }

        public override object this[long startIndex, int length] {
            get
            {
                if (startIndex > Length )
                {
                    throw new ArgumentOutOfRangeException(nameof(startIndex));
                }
                return _columnContainer[startIndex, length];
            }
        }

        protected override object GetValue(long rowIndex)
        {
            return _columnContainer[rowIndex];
        }

        protected override void SetValue(long rowIndex, object value)
        {
            if (value.GetType() == typeof(T))
            {
                _columnContainer[rowIndex] = (T)value;
            }
            else
            {
                throw new ArgumentException(nameof(value));
            }
        }

        // This method involves boxing
        public new T this[long rowIndex]
        {
            get
            {
                return _columnContainer[rowIndex];
            }
            set => SetValue(rowIndex, value);
        }

        public void Append(T value)
        {
            _columnContainer.Append(value);
            Length++;
        }

        public override string ToString()
        {
            return $"{Name}: {_columnContainer.ToString()}";
        }

        public override BaseColumn Clone(BaseColumn mapIndices = null, bool invertMapIndices = false)
        {
            if (!(mapIndices is null))
            {
                if (mapIndices.DataType != typeof(long)) throw new ArgumentException($"Expected sortIndices to be a PrimitiveColumn<long>");
                if (mapIndices.Length != Length) throw new ArgumentException(strings.MismatchedColumnLengths, nameof(mapIndices));
                return _Clone(mapIndices as PrimitiveColumn<long>, invertMapIndices);
            }
            return _Clone();
        }

        public PrimitiveColumn<T> _Clone(PrimitiveColumn<long> mapIndices = null, bool invertMapIndices = false)
        {
            if (mapIndices is null)
            {
                PrimitiveColumnContainer<T> newColumnContainer = _columnContainer.Clone();
                return new PrimitiveColumn<T>(Name, newColumnContainer);
            }
            else
            {
                PrimitiveColumn<T> ret = new PrimitiveColumn<T>(Name);
                if (invertMapIndices == false)
                {
                    for (long i = 0; i < mapIndices.Length; i++)
                    {
                        ret.Append(_columnContainer[mapIndices._columnContainer[i]]);
                    }
                }
                else
                {
                    for (long i = Length - 1; i >= 0; i--)
                    {
                        ret.Append(_columnContainer[mapIndices._columnContainer[i]]);
                    }
                }
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
            foreach (var buffer in _columnContainer.Buffers)
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
