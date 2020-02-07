﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.Data.Analysis
{
    /// <summary>
    /// A window for <see cref="PrimitiveDataFrameColumn{T}"/> to support rolling window operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PrimitiveDataFrameColumnRollingWindow<T> : DataFrameColumnWindow
        where T : unmanaged
    {
        private int _windowSize;
        private PrimitiveDataFrameColumn<T> _currentColumn;
        private long _currentIndex;

        internal PrimitiveDataFrameColumnRollingWindow(int windowSize, PrimitiveDataFrameColumn<T> currentColumn)
        {
            _windowSize = windowSize;
            _currentColumn = currentColumn;
        }

        /// <summary>
        /// Apply <paramref name="func"/> over a rolling window of values. 
        /// </summary>
        /// <typeparam name="U">The returned result for each rolling window</typeparam>
        /// <param name="func">The delegate to apply for each window. The first parameter is a linked list of values in the current window. The second parameter is the current index in the column. The current window ends at the current index and include values preceeding it</param>
        /// <returns>The result computed for each rolling window</returns>
        public PrimitiveDataFrameColumn<U> Apply<U>(Func<LinkedList<T?>, long, U?> func)
            where U : unmanaged
        {
            return _currentColumn.ApplyRollingFunc(func, _windowSize);
        }

        /// <summary>
        /// Finds the min of the values in this window
        /// </summary>
        /// <returns>Returns a DataFrame containing the min in each window</returns>
        public new PrimitiveDataFrameColumn<T> Min()
        {
            _currentIndex = 0;

            LinkedList<KeyValuePair<double, long>> sortedList = new LinkedList<KeyValuePair<double, long>>();
            LinkedList<T?> windowValues = new LinkedList<T?>();
            IDoubleConverter<T> doubleConverter = DoubleConverter<T>.Instance;
            return _currentColumn.Apply<T>((T? value) =>
            {
                double doubleValue = value.HasValue ? doubleConverter.GetDouble(value.Value) : double.MaxValue;
                // This algorithm builds a sorted list. Time complexity is O(N) here since each value is added/removed to sortedList only once.
                // If the values at the end of the sortedList are greater than value, delete them since they can never be the min in this window
                // If value is null, treat it as double.MaxValue
                while (sortedList.Count != 0 && sortedList.Last.Value.Key >= doubleValue)
                {
                    sortedList.RemoveLast();
                    windowValues.RemoveLast();
                }

                sortedList.AddLast(new KeyValuePair<double, long>(doubleValue, _currentIndex));
                windowValues.AddLast(value);

                while (sortedList.First.Value.Value <= _currentIndex - _windowSize)
                {
                    sortedList.RemoveFirst();
                    windowValues.RemoveFirst();
                }

                // For the edge case of (6, null, null, null) where (null, null, null) is in the current window, we will have (null) in sortedList at this point. Check if value is null and sortedList.First is double.Max. If it is, remove it
                if (!value.HasValue && sortedList.Count == 1 && sortedList.Last.Value.Key == double.MaxValue)
                {
                    Debug.Assert(windowValues.Count == 1);
                    sortedList.RemoveFirst();
                    windowValues.RemoveFirst();
                }

                _currentIndex++;
                if (_currentIndex < _windowSize)
                {
                    return null;
                }

                return windowValues.Count > 0 ? windowValues.First.Value : null;
            });

        }

        /// <summary>
        /// Finds the max of the values in this window
        /// </summary>
        /// <returns>Returns a DataFrame containing the max in each window</returns>
        public new PrimitiveDataFrameColumn<T> Max()
        {
            _currentIndex = 0;

            IDoubleConverter<T> doubleConverter = DoubleConverter<T>.Instance;
            return _currentColumn.Apply<T>((T? value) =>
            {
                LinkedList<T?> windowValues = new LinkedList<T?>();
                LinkedList<KeyValuePair<double, long>> sortedList = new LinkedList<KeyValuePair<double, long>>();
                double doubleValue = value.HasValue ? doubleConverter.GetDouble(value.Value) : double.MinValue;
                // This algorithm builds a sorted list. Time complexity is O(N) here since each value is added/removed to sortedList only once.
                // If the values at the end of the sortedList are lesser than value, delete them since they can never be the max in this window
                // If value is null, treat it as double.MinValue
                while (sortedList.Count != 0 && sortedList.Last.Value.Key <= doubleValue)
                {
                    sortedList.RemoveLast();
                    windowValues.RemoveLast();
                }

                sortedList.AddLast(new KeyValuePair<double, long>(doubleValue, _currentIndex));
                windowValues.AddLast(value);

                while (sortedList.First.Value.Value <= _currentIndex - _windowSize)
                {
                    sortedList.RemoveFirst();
                    windowValues.RemoveFirst();
                }

                // For the edge case of (6, null, null, null) where (null, null, null) is in the current window, we will have (null) in sortedList at this point. Check if value is null and sortedList.First is double.Max. If it is, remove it
                if (!value.HasValue && sortedList.Count == 1 && sortedList.Last.Value.Key == double.MinValue)
                {
                    Debug.Assert(windowValues.Count == 1);
                    sortedList.RemoveFirst();
                    windowValues.RemoveFirst();
                }

                _currentIndex++;
                if (_currentIndex < _windowSize)
                {
                    return null;
                }

                return windowValues.Count > 0 ? windowValues.First.Value : null;
            });

        }

        public override PrimitiveDataFrameColumn<int> Count()
        {
            _currentIndex = 0;
            int count = 0;
            return _currentColumn.Apply<int>((T? value) =>
            {
                if (value.HasValue && count < _windowSize)
                {
                    count++;
                }
                if (!value.HasValue && count > 0)
                {
                    count--;
                }
                if (_currentIndex < _windowSize - 1)
                {
                    _currentIndex++;
                    return null;
                }
                _currentIndex++;
                return count;
            });
        }
    }

    public partial class PrimitiveDataFrameColumn<T> : DataFrameColumn
        where T : unmanaged
    {
        internal PrimitiveDataFrameColumn<U> ApplyRollingFunc<U>(Func<LinkedList<T?>, long, U?> func, int windowSize)
            where U : unmanaged
        {
            PrimitiveDataFrameColumn<U> ret = new PrimitiveDataFrameColumn<U>(Name, Length);
            _columnContainer.Apply(func, ret._columnContainer, windowSize);
            return ret;
        }
    }
}
