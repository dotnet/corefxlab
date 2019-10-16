
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

        public DataFrame Add<T>(IReadOnlyList<T> values, bool inPlace = false)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Add(values[i], inPlace);
                if (inPlace)
                    retDataFrame.SetColumn(i, newColumn);
                else
                    retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame Add<T>(T value, bool inPlace = false)
            where T : unmanaged
        {
            return Add(value, inPlace, reverseOrderOfOperations: false);
        }
        private DataFrame Add<T>(T value, bool inPlace = false, bool reverseOrderOfOperations = false)
            where T : unmanaged
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Add(value, inPlace, reverseOrderOfOperations);
                if (inPlace)
                    retDataFrame.SetColumn(i, newColumn);
                else
                    retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame Subtract<T>(IReadOnlyList<T> values, bool inPlace = false)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Subtract(values[i], inPlace);
                if (inPlace)
                    retDataFrame.SetColumn(i, newColumn);
                else
                    retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame Subtract<T>(T value, bool inPlace = false)
            where T : unmanaged
        {
            return Subtract(value, inPlace, reverseOrderOfOperations: false);
        }
        private DataFrame Subtract<T>(T value, bool inPlace = false, bool reverseOrderOfOperations = false)
            where T : unmanaged
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Subtract(value, inPlace, reverseOrderOfOperations);
                if (inPlace)
                    retDataFrame.SetColumn(i, newColumn);
                else
                    retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame Multiply<T>(IReadOnlyList<T> values, bool inPlace = false)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Multiply(values[i], inPlace);
                if (inPlace)
                    retDataFrame.SetColumn(i, newColumn);
                else
                    retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame Multiply<T>(T value, bool inPlace = false)
            where T : unmanaged
        {
            return Multiply(value, inPlace, reverseOrderOfOperations: false);
        }
        private DataFrame Multiply<T>(T value, bool inPlace = false, bool reverseOrderOfOperations = false)
            where T : unmanaged
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Multiply(value, inPlace, reverseOrderOfOperations);
                if (inPlace)
                    retDataFrame.SetColumn(i, newColumn);
                else
                    retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame Divide<T>(IReadOnlyList<T> values, bool inPlace = false)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Divide(values[i], inPlace);
                if (inPlace)
                    retDataFrame.SetColumn(i, newColumn);
                else
                    retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame Divide<T>(T value, bool inPlace = false)
            where T : unmanaged
        {
            return Divide(value, inPlace, reverseOrderOfOperations: false);
        }
        private DataFrame Divide<T>(T value, bool inPlace = false, bool reverseOrderOfOperations = false)
            where T : unmanaged
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Divide(value, inPlace, reverseOrderOfOperations);
                if (inPlace)
                    retDataFrame.SetColumn(i, newColumn);
                else
                    retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame Modulo<T>(IReadOnlyList<T> values, bool inPlace = false)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Modulo(values[i], inPlace);
                if (inPlace)
                    retDataFrame.SetColumn(i, newColumn);
                else
                    retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame Modulo<T>(T value, bool inPlace = false)
            where T : unmanaged
        {
            return Modulo(value, inPlace, reverseOrderOfOperations: false);
        }
        private DataFrame Modulo<T>(T value, bool inPlace = false, bool reverseOrderOfOperations = false)
            where T : unmanaged
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Modulo(value, inPlace, reverseOrderOfOperations);
                if (inPlace)
                    retDataFrame.SetColumn(i, newColumn);
                else
                    retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame And<T>(IReadOnlyList<T> values, bool inPlace = false)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.And(values[i], inPlace);
                if (inPlace)
                    retDataFrame.SetColumn(i, newColumn);
                else
                    retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame And<T>(T value, bool inPlace = false)
            where T : unmanaged
        {
            return And(value, inPlace, reverseOrderOfOperations: false);
        }
        private DataFrame And<T>(T value, bool inPlace = false, bool reverseOrderOfOperations = false)
            where T : unmanaged
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.And(value, inPlace, reverseOrderOfOperations);
                if (inPlace)
                    retDataFrame.SetColumn(i, newColumn);
                else
                    retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame Or<T>(IReadOnlyList<T> values, bool inPlace = false)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Or(values[i], inPlace);
                if (inPlace)
                    retDataFrame.SetColumn(i, newColumn);
                else
                    retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame Or<T>(T value, bool inPlace = false)
            where T : unmanaged
        {
            return Or(value, inPlace, reverseOrderOfOperations: false);
        }
        private DataFrame Or<T>(T value, bool inPlace = false, bool reverseOrderOfOperations = false)
            where T : unmanaged
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Or(value, inPlace, reverseOrderOfOperations);
                if (inPlace)
                    retDataFrame.SetColumn(i, newColumn);
                else
                    retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame Xor<T>(IReadOnlyList<T> values, bool inPlace = false)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Xor(values[i], inPlace);
                if (inPlace)
                    retDataFrame.SetColumn(i, newColumn);
                else
                    retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame Xor<T>(T value, bool inPlace = false)
            where T : unmanaged
        {
            return Xor(value, inPlace, reverseOrderOfOperations: false);
        }
        private DataFrame Xor<T>(T value, bool inPlace = false, bool reverseOrderOfOperations = false)
            where T : unmanaged
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Xor(value, inPlace, reverseOrderOfOperations);
                if (inPlace)
                    retDataFrame.SetColumn(i, newColumn);
                else
                    retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame LeftShift(int value, bool inPlace = false)
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.LeftShift(value, inPlace);
                if (inPlace)
                    retDataFrame.SetColumn(i, newColumn);
                else
                    retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame RightShift(int value, bool inPlace = false)
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.RightShift(value, inPlace);
                if (inPlace)
                    retDataFrame.SetColumn(i, newColumn);
                else
                    retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame Equals<T>(IReadOnlyList<T> values)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = new DataFrame();

            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Equals(values[i]);
                retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame Equals<T>(T value)
            where T : unmanaged
        {
            DataFrame retDataFrame = new DataFrame();

            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.Equals(value);
                retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame NotEquals<T>(IReadOnlyList<T> values)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = new DataFrame();

            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.NotEquals(values[i]);
                retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame NotEquals<T>(T value)
            where T : unmanaged
        {
            DataFrame retDataFrame = new DataFrame();

            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.NotEquals(value);
                retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame GreaterThanOrEqual<T>(IReadOnlyList<T> values)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = new DataFrame();

            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.GreaterThanOrEqual(values[i]);
                retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame GreaterThanOrEqual<T>(T value)
            where T : unmanaged
        {
            DataFrame retDataFrame = new DataFrame();

            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.GreaterThanOrEqual(value);
                retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame LessThanOrEqual<T>(IReadOnlyList<T> values)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = new DataFrame();

            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.LessThanOrEqual(values[i]);
                retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame LessThanOrEqual<T>(T value)
            where T : unmanaged
        {
            DataFrame retDataFrame = new DataFrame();

            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.LessThanOrEqual(value);
                retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame GreaterThan<T>(IReadOnlyList<T> values)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = new DataFrame();

            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.GreaterThan(values[i]);
                retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame GreaterThan<T>(T value)
            where T : unmanaged
        {
            DataFrame retDataFrame = new DataFrame();

            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.GreaterThan(value);
                retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame LessThan<T>(IReadOnlyList<T> values)
            where T : unmanaged
        {
            if (values.Count != ColumnCount)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = new DataFrame();

            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.LessThan(values[i]);
                retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame LessThan<T>(T value)
            where T : unmanaged
        {
            DataFrame retDataFrame = new DataFrame();

            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn baseColumn = _table.Column(i);
                BaseColumn newColumn = baseColumn.LessThan(value);
                retDataFrame.InsertColumn(i, newColumn);
            }
            return retDataFrame;
        }
        #endregion
    }
}
