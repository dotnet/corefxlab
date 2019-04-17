
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Microsoft.Data
{
    public partial class DataFrame
    {
        #region Binary Operations

        public DataFrame Add<T>(IReadOnlyList<T> values)
            where T : struct
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    var newColumn = column.Clone();
                    newColumn._columnContainer.Add(values[ii]);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame Add<T>(T value)
            where T : struct
        {
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    var newColumn = column.Clone();
                    newColumn._columnContainer.Add(value);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame Subtract<T>(IReadOnlyList<T> values)
            where T : struct
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    var newColumn = column.Clone();
                    newColumn._columnContainer.Subtract(values[ii]);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame Subtract<T>(T value)
            where T : struct
        {
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    var newColumn = column.Clone();
                    newColumn._columnContainer.Subtract(value);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame Multiply<T>(IReadOnlyList<T> values)
            where T : struct
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    var newColumn = column.Clone();
                    newColumn._columnContainer.Multiply(values[ii]);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame Multiply<T>(T value)
            where T : struct
        {
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    var newColumn = column.Clone();
                    newColumn._columnContainer.Multiply(value);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame Divide<T>(IReadOnlyList<T> values)
            where T : struct
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    var newColumn = column.Clone();
                    newColumn._columnContainer.Divide(values[ii]);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame Divide<T>(T value)
            where T : struct
        {
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    var newColumn = column.Clone();
                    newColumn._columnContainer.Divide(value);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame Modulo<T>(IReadOnlyList<T> values)
            where T : struct
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    var newColumn = column.Clone();
                    newColumn._columnContainer.Modulo(values[ii]);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame Modulo<T>(T value)
            where T : struct
        {
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    var newColumn = column.Clone();
                    newColumn._columnContainer.Modulo(value);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame And<T>(IReadOnlyList<T> values)
            where T : struct
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    var newColumn = column.Clone();
                    newColumn._columnContainer.And(values[ii]);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame And<T>(T value)
            where T : struct
        {
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    var newColumn = column.Clone();
                    newColumn._columnContainer.And(value);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame Or<T>(IReadOnlyList<T> values)
            where T : struct
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    var newColumn = column.Clone();
                    newColumn._columnContainer.Or(values[ii]);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame Or<T>(T value)
            where T : struct
        {
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    var newColumn = column.Clone();
                    newColumn._columnContainer.Or(value);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame Xor<T>(IReadOnlyList<T> values)
            where T : struct
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    var newColumn = column.Clone();
                    newColumn._columnContainer.Xor(values[ii]);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame Xor<T>(T value)
            where T : struct
        {
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    var newColumn = column.Clone();
                    newColumn._columnContainer.Xor(value);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame LeftShift<T>(int value)
            where T : struct
        {
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    var newColumn = column.Clone();
                    newColumn._columnContainer.LeftShift(value);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame RightShift<T>(int value)
            where T : struct
        {
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    var newColumn = column.Clone();
                    newColumn._columnContainer.RightShift(value);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame Equals<T>(IReadOnlyList<T> values)
            where T : struct
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    PrimitiveDataFrameColumn<bool> newColumn = column.CreateBoolColumnForCompareOps();
                    column._columnContainer.Equals(values[ii], newColumn._columnContainer);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame Equals<T>(T value)
            where T : struct
        {
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    PrimitiveDataFrameColumn<bool> newColumn = column.CreateBoolColumnForCompareOps();
                    column._columnContainer.Equals(value, newColumn._columnContainer);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame NotEquals<T>(IReadOnlyList<T> values)
            where T : struct
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    PrimitiveDataFrameColumn<bool> newColumn = column.CreateBoolColumnForCompareOps();
                    column._columnContainer.NotEquals(values[ii], newColumn._columnContainer);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame NotEquals<T>(T value)
            where T : struct
        {
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    PrimitiveDataFrameColumn<bool> newColumn = column.CreateBoolColumnForCompareOps();
                    column._columnContainer.NotEquals(value, newColumn._columnContainer);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame GreaterThanOrEqual<T>(IReadOnlyList<T> values)
            where T : struct
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    PrimitiveDataFrameColumn<bool> newColumn = column.CreateBoolColumnForCompareOps();
                    column._columnContainer.GreaterThanOrEqual(values[ii], newColumn._columnContainer);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame GreaterThanOrEqual<T>(T value)
            where T : struct
        {
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    PrimitiveDataFrameColumn<bool> newColumn = column.CreateBoolColumnForCompareOps();
                    column._columnContainer.GreaterThanOrEqual(value, newColumn._columnContainer);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame LessThanOrEqual<T>(IReadOnlyList<T> values)
            where T : struct
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    PrimitiveDataFrameColumn<bool> newColumn = column.CreateBoolColumnForCompareOps();
                    column._columnContainer.LessThanOrEqual(values[ii], newColumn._columnContainer);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame LessThanOrEqual<T>(T value)
            where T : struct
        {
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    PrimitiveDataFrameColumn<bool> newColumn = column.CreateBoolColumnForCompareOps();
                    column._columnContainer.LessThanOrEqual(value, newColumn._columnContainer);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame GreaterThan<T>(IReadOnlyList<T> values)
            where T : struct
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    PrimitiveDataFrameColumn<bool> newColumn = column.CreateBoolColumnForCompareOps();
                    column._columnContainer.GreaterThan(values[ii], newColumn._columnContainer);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame GreaterThan<T>(T value)
            where T : struct
        {
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    PrimitiveDataFrameColumn<bool> newColumn = column.CreateBoolColumnForCompareOps();
                    column._columnContainer.GreaterThan(value, newColumn._columnContainer);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame LessThan<T>(IReadOnlyList<T> values)
            where T : struct
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException($"values.Count {values.Count} must match the number of columns in the table", nameof(values));
            }
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    PrimitiveDataFrameColumn<bool> newColumn = column.CreateBoolColumnForCompareOps();
                    column._columnContainer.LessThan(values[ii], newColumn._columnContainer);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }

        public DataFrame LessThan<T>(T value)
            where T : struct
        {
            var newDataFrame = new DataFrame();
            for (int ii = 0; ii < ColumnCount; ii++)
            {
                PrimitiveDataFrameColumn<T> column = _table.Column(ii) as PrimitiveDataFrameColumn<T>;
                if (column != null)
                {
                    PrimitiveDataFrameColumn<bool> newColumn = column.CreateBoolColumnForCompareOps();
                    column._columnContainer.LessThan(value, newColumn._columnContainer);
                    newDataFrame.InsertColumn(ii, newColumn);
                }
            }
            return newDataFrame;
        }


        #endregion
    }
}
