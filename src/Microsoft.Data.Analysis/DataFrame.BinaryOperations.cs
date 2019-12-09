
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Generated from DataFrameBinaryOperations.tt. Do not modify directly

using System;
using System.Collections.Generic;

namespace Microsoft.Data.Analysis
{
    public partial class DataFrame
    {
        public DataFrame Add<T>(IReadOnlyList<T> values, bool inPlace = false)
            where T : unmanaged
        {
            if (values.Count != Columns.Count)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.AddValue(values[i], inPlace);
                if (inPlace)
                    retDataFrame.Columns[i] = newColumn;
                else
                    retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        /// <summary>
        /// Performs an element wise addition on each column
        /// </summary>
        public DataFrame AddValue<T>(T value, bool inPlace = false)
            where T : unmanaged
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.AddValue(value, inPlace);
                if (inPlace)
                    retDataFrame.Columns[i] = newColumn;
                else
                    retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame Subtract<T>(IReadOnlyList<T> values, bool inPlace = false)
            where T : unmanaged
        {
            if (values.Count != Columns.Count)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.SubtractValue(values[i], inPlace);
                if (inPlace)
                    retDataFrame.Columns[i] = newColumn;
                else
                    retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        /// <summary>
        /// Performs an element wise subtraction on each column
        /// </summary>
        public DataFrame SubtractValue<T>(T value, bool inPlace = false)
            where T : unmanaged
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.SubtractValue(value, inPlace);
                if (inPlace)
                    retDataFrame.Columns[i] = newColumn;
                else
                    retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame Multiply<T>(IReadOnlyList<T> values, bool inPlace = false)
            where T : unmanaged
        {
            if (values.Count != Columns.Count)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.MultiplyValue(values[i], inPlace);
                if (inPlace)
                    retDataFrame.Columns[i] = newColumn;
                else
                    retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        /// <summary>
        /// Performs an element wise multiplication on each column
        /// </summary>
        public DataFrame MultiplyValue<T>(T value, bool inPlace = false)
            where T : unmanaged
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.MultiplyValue(value, inPlace);
                if (inPlace)
                    retDataFrame.Columns[i] = newColumn;
                else
                    retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame Divide<T>(IReadOnlyList<T> values, bool inPlace = false)
            where T : unmanaged
        {
            if (values.Count != Columns.Count)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.DivideValue(values[i], inPlace);
                if (inPlace)
                    retDataFrame.Columns[i] = newColumn;
                else
                    retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        /// <summary>
        /// Performs an element wise division on each column
        /// </summary>
        public DataFrame DivideValue<T>(T value, bool inPlace = false)
            where T : unmanaged
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.DivideValue(value, inPlace);
                if (inPlace)
                    retDataFrame.Columns[i] = newColumn;
                else
                    retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame Modulo<T>(IReadOnlyList<T> values, bool inPlace = false)
            where T : unmanaged
        {
            if (values.Count != Columns.Count)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.ModuloValue(values[i], inPlace);
                if (inPlace)
                    retDataFrame.Columns[i] = newColumn;
                else
                    retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        /// <summary>
        /// Performs an element wise modulus operation on each column
        /// </summary>
        public DataFrame ModuloValue<T>(T value, bool inPlace = false)
            where T : unmanaged
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.ModuloValue(value, inPlace);
                if (inPlace)
                    retDataFrame.Columns[i] = newColumn;
                else
                    retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame And(IReadOnlyList<bool> values, bool inPlace = false)
        {
            if (values.Count != Columns.Count)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.AndValue(values[i], inPlace);
                if (inPlace)
                    retDataFrame.Columns[i] = newColumn;
                else
                    retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        /// <summary>
        /// Performs an element wise boolean And on each column
        /// </summary>
        public DataFrame AndValue(bool value, bool inPlace = false)
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.AndValue(value, inPlace);
                if (inPlace)
                    retDataFrame.Columns[i] = newColumn;
                else
                    retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame Or(IReadOnlyList<bool> values, bool inPlace = false)
        {
            if (values.Count != Columns.Count)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.OrValue(values[i], inPlace);
                if (inPlace)
                    retDataFrame.Columns[i] = newColumn;
                else
                    retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        /// <summary>
        /// Performs an element wise boolean Or on each column
        /// </summary>
        public DataFrame OrValue(bool value, bool inPlace = false)
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.OrValue(value, inPlace);
                if (inPlace)
                    retDataFrame.Columns[i] = newColumn;
                else
                    retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame Xor(IReadOnlyList<bool> values, bool inPlace = false)
        {
            if (values.Count != Columns.Count)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.XorValue(values[i], inPlace);
                if (inPlace)
                    retDataFrame.Columns[i] = newColumn;
                else
                    retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        /// <summary>
        /// Performs an element wise boolean Xor on each column
        /// </summary>
        public DataFrame XorValue(bool value, bool inPlace = false)
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.XorValue(value, inPlace);
                if (inPlace)
                    retDataFrame.Columns[i] = newColumn;
                else
                    retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        /// <summary>
        /// Performs an element wise left shift on each column
        /// </summary>
        public DataFrame LeftShift(int value, bool inPlace = false)
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.LeftShift(value, inPlace);
                if (inPlace)
                    retDataFrame.Columns[i] = newColumn;
                else
                    retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        /// <summary>
        /// Performs an element wise right shift on each column
        /// </summary>
        public DataFrame RightShift(int value, bool inPlace = false)
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.RightShift(value, inPlace);
                if (inPlace)
                    retDataFrame.Columns[i] = newColumn;
                else
                    retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame ElementwiseEquals<T>(IReadOnlyList<T> values)
            where T : unmanaged
        {
            if (values.Count != Columns.Count)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.ElementwiseValueEquals(values[i]);
                retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        /// <summary>
        /// Performs an element wise equals on each column
        /// </summary>
        public DataFrame ElementwiseValueEquals<T>(T value)
            where T : unmanaged
        {
            DataFrame retDataFrame = new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.ElementwiseValueEquals(value);
                retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame ElementwiseNotEquals<T>(IReadOnlyList<T> values)
            where T : unmanaged
        {
            if (values.Count != Columns.Count)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.ElementwiseValueNotEquals(values[i]);
                retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        /// <summary>
        /// Performs an element wise not-equals on each column
        /// </summary>
        public DataFrame ElementwiseValueNotEquals<T>(T value)
            where T : unmanaged
        {
            DataFrame retDataFrame = new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.ElementwiseValueNotEquals(value);
                retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame ElementwiseGreaterThanOrEqual<T>(IReadOnlyList<T> values)
            where T : unmanaged
        {
            if (values.Count != Columns.Count)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.ElementwiseValueGreaterThanOrEqual(values[i]);
                retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        /// <summary>
        /// Performs an element wise greater than or equal on each column
        /// </summary>
        public DataFrame ElementwiseValueGreaterThanOrEqual<T>(T value)
            where T : unmanaged
        {
            DataFrame retDataFrame = new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.ElementwiseValueGreaterThanOrEqual(value);
                retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame ElementwiseLessThanOrEqual<T>(IReadOnlyList<T> values)
            where T : unmanaged
        {
            if (values.Count != Columns.Count)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.ElementwiseValueLessThanOrEqual(values[i]);
                retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        /// <summary>
        /// Performs an element wise less than or equal on each column
        /// </summary>
        public DataFrame ElementwiseValueLessThanOrEqual<T>(T value)
            where T : unmanaged
        {
            DataFrame retDataFrame = new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.ElementwiseValueLessThanOrEqual(value);
                retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame ElementwiseGreaterThan<T>(IReadOnlyList<T> values)
            where T : unmanaged
        {
            if (values.Count != Columns.Count)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.ElementwiseValueGreaterThan(values[i]);
                retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        /// <summary>
        /// Performs an element wise greater than on each column
        /// </summary>
        public DataFrame ElementwiseValueGreaterThan<T>(T value)
            where T : unmanaged
        {
            DataFrame retDataFrame = new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.ElementwiseValueGreaterThan(value);
                retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        public DataFrame ElementwiseLessThan<T>(IReadOnlyList<T> values)
            where T : unmanaged
        {
            if (values.Count != Columns.Count)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));
            }
            DataFrame retDataFrame = new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.ElementwiseValueLessThan(values[i]);
                retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        /// <summary>
        /// Performs an element wise less than on each column
        /// </summary>
        public DataFrame ElementwiseValueLessThan<T>(T value)
            where T : unmanaged
        {
            DataFrame retDataFrame = new DataFrame();

            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.ElementwiseValueLessThan(value);
                retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }

        /// <summary>
        /// Performs a reversed element wise addition on each column
        /// </summary>
        public DataFrame ReverseAddValue<T>(T value, bool inPlace = false)
            where T : unmanaged
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();
            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.ReverseAddValue(value, inPlace);
                if (inPlace)
                    retDataFrame.Columns[i] = newColumn;
                else
                    retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        /// <summary>
        /// Performs a reversed element wise subtraction on each column
        /// </summary>
        public DataFrame ReverseSubtractValue<T>(T value, bool inPlace = false)
            where T : unmanaged
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();
            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.ReverseSubtractValue(value, inPlace);
                if (inPlace)
                    retDataFrame.Columns[i] = newColumn;
                else
                    retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        /// <summary>
        /// Performs a reversed element wise multiplication on each column
        /// </summary>
        public DataFrame ReverseMultiplyValue<T>(T value, bool inPlace = false)
            where T : unmanaged
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();
            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.ReverseMultiplyValue(value, inPlace);
                if (inPlace)
                    retDataFrame.Columns[i] = newColumn;
                else
                    retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        /// <summary>
        /// Performs a reversed element wise division on each column
        /// </summary>
        public DataFrame ReverseDivideValue<T>(T value, bool inPlace = false)
            where T : unmanaged
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();
            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.ReverseDivideValue(value, inPlace);
                if (inPlace)
                    retDataFrame.Columns[i] = newColumn;
                else
                    retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        /// <summary>
        /// Performs a reversed element wise modulus operation on each column
        /// </summary>
        public DataFrame ReverseModuloValue<T>(T value, bool inPlace = false)
            where T : unmanaged
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();
            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.ReverseModuloValue(value, inPlace);
                if (inPlace)
                    retDataFrame.Columns[i] = newColumn;
                else
                    retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        /// <summary>
        /// Performs a reversed element wise boolean And on each column
        /// </summary>
        public DataFrame ReverseAndValue(bool value, bool inPlace = false)
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();
            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.ReverseAndValue(value, inPlace);
                if (inPlace)
                    retDataFrame.Columns[i] = newColumn;
                else
                    retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        /// <summary>
        /// Performs a reversed element wise boolean Or on each column
        /// </summary>
        public DataFrame ReverseOrValue(bool value, bool inPlace = false)
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();
            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.ReverseOrValue(value, inPlace);
                if (inPlace)
                    retDataFrame.Columns[i] = newColumn;
                else
                    retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
        /// <summary>
        /// Performs a reversed element wise boolean Xor on each column
        /// </summary>
        public DataFrame ReverseXorValue(bool value, bool inPlace = false)
        {
            DataFrame retDataFrame = inPlace ? this : new DataFrame();
            for (int i = 0; i < Columns.Count; i++)
            {
                DataFrameColumn baseColumn = _columnCollection[i];
                DataFrameColumn newColumn = baseColumn.ReverseXorValue(value, inPlace);
                if (inPlace)
                    retDataFrame.Columns[i] = newColumn;
                else
                    retDataFrame.Columns.Insert(i, newColumn);
            }
            return retDataFrame;
        }
    }
}
