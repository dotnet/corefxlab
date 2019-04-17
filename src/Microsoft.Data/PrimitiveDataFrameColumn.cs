// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Microsoft.Data
{
    /// <summary>
    /// A column to hold primitive values such as int, float etc. Other value types are not really supported
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PrimitiveDataFrameColumn<T> : BaseDataFrameColumn
        where T : struct
    {
        internal PrimitiveDataFrameColumnContainer<T> _columnContainer;

        public Type DataType = typeof(T);

        internal PrimitiveDataFrameColumn(string name, PrimitiveDataFrameColumnContainer<T> column) : base(name, column.Length)
        {
            _columnContainer = column;
        }

        public PrimitiveDataFrameColumn(string name, IEnumerable<T> values) : base(name)
        {
            _columnContainer = new PrimitiveDataFrameColumnContainer<T>(values);
            Length = _columnContainer.Length;
        }

        public PrimitiveDataFrameColumn(string name, bool isNullable = true) : base(name)
        {
            _columnContainer = new PrimitiveDataFrameColumnContainer<T>();
        }

        public override object this[long startIndex, int length] {
            get
            {
                if (startIndex > Length )
                {
                    throw new ArgumentException($"Indexer arguments exceed Length {Length} of the Column");
                }
                return _columnContainer[startIndex, length];
            }
        }

        // This method involves boxing
        public override object this[long rowIndex]
        {
            get
            {
                return _columnContainer[rowIndex];
            }
            set
            {
                _columnContainer[rowIndex] = (T)value;
            }
        }

        public void Add(T value) => _columnContainer.Add(value);

        public override string ToString()
        {
            return $"{Name}: {_columnContainer.ToString()}";
        }

        public PrimitiveDataFrameColumn<T> Clone()
        {
            PrimitiveDataFrameColumnContainer<T> newColumnContainer = _columnContainer.Clone();
            return new PrimitiveDataFrameColumn<T>(Name, newColumnContainer);
        }

        internal PrimitiveDataFrameColumn<bool> CreateBoolColumnForCompareOps()
        {
            PrimitiveDataFrameColumnContainer<bool> newColumnContainer = _columnContainer.CreateBoolContainerForCompareOps();
            return new PrimitiveDataFrameColumn<bool>(Name, newColumnContainer);
        }

    }
}
