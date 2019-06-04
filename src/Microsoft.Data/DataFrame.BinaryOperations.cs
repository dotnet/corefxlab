
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Generated from DataFrameBinaryOperations.tt. Do not modify directly

using System;
using System.Collections.Generic;

namespace Microsoft.Data
{
    public partial class DataFrame
    {
        #region Binary Operations

        public DataFrame Add<T>(IReadOnlyList<T> values)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Add(values[i]);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame Add<T>(T value)
            where T : unmanaged
        {
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Add(value);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame Subtract<T>(IReadOnlyList<T> values)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Subtract(values[i]);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame Subtract<T>(T value)
            where T : unmanaged
        {
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Subtract(value);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame Multiply<T>(IReadOnlyList<T> values)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Multiply(values[i]);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame Multiply<T>(T value)
            where T : unmanaged
        {
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Multiply(value);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame Divide<T>(IReadOnlyList<T> values)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Divide(values[i]);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame Divide<T>(T value)
            where T : unmanaged
        {
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Divide(value);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame Modulo<T>(IReadOnlyList<T> values)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Modulo(values[i]);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame Modulo<T>(T value)
            where T : unmanaged
        {
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Modulo(value);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame And<T>(IReadOnlyList<T> values)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.And(values[i]);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame And<T>(T value)
            where T : unmanaged
        {
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.And(value);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame Or<T>(IReadOnlyList<T> values)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Or(values[i]);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame Or<T>(T value)
            where T : unmanaged
        {
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Or(value);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame Xor<T>(IReadOnlyList<T> values)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Xor(values[i]);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame Xor<T>(T value)
            where T : unmanaged
        {
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Xor(value);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame LeftShift(int value)
        {
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.LeftShift(value);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame RightShift(int value)
        {
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.RightShift(value);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame Equals<T>(IReadOnlyList<T> values)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Equals(values[i]);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame Equals<T>(T value)
            where T : unmanaged
        {
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Equals(value);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame NotEquals<T>(IReadOnlyList<T> values)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.NotEquals(values[i]);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame NotEquals<T>(T value)
            where T : unmanaged
        {
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.NotEquals(value);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame GreaterThanOrEqual<T>(IReadOnlyList<T> values)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.GreaterThanOrEqual(values[i]);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame GreaterThanOrEqual<T>(T value)
            where T : unmanaged
        {
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.GreaterThanOrEqual(value);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame LessThanOrEqual<T>(IReadOnlyList<T> values)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.LessThanOrEqual(values[i]);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame LessThanOrEqual<T>(T value)
            where T : unmanaged
        {
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.LessThanOrEqual(value);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame GreaterThan<T>(IReadOnlyList<T> values)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.GreaterThan(values[i]);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame GreaterThan<T>(T value)
            where T : unmanaged
        {
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.GreaterThan(value);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame LessThan<T>(IReadOnlyList<T> values)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.LessThan(values[i]);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        public DataFrame LessThan<T>(T value)
            where T : unmanaged
        {
            var newDataFrame = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.LessThan(value);
                newDataFrame.InsertColumn(i, newColumn);
            }
            return newDataFrame;
        }
        #endregion
    }
}
