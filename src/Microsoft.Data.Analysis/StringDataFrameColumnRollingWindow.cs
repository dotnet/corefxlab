// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Data.Analysis
{
    /// <summary>
    /// A window for <see cref="StringDataFrameColumn"/> to support rolling window operations
    /// </summary>
    public class StringDataFrameColumnRollingWindow : DataFrameColumnWindow
    {
        private int _windowSize;
        private StringDataFrameColumn _currentColumn;
        private long _currentIndex;

        internal StringDataFrameColumnRollingWindow(int windowSize, StringDataFrameColumn currentColumn)
        {
            _windowSize = windowSize;
            _currentColumn = currentColumn;
        }

        internal static PrimitiveDataFrameColumn<int> CountImplementation(long currentIndex, DataFrameColumn stringOrArrowStringDataFrameColumn, int windowSize)
        {
            currentIndex = 0;
            int count = 0;
            PrimitiveDataFrameColumn<int> ret = new PrimitiveDataFrameColumn<int>("Count", stringOrArrowStringDataFrameColumn.Length);
            for (long i = 0; i < stringOrArrowStringDataFrameColumn.Length; i++)
            {
                string value = (string)stringOrArrowStringDataFrameColumn[i];
                if (value != null && count < windowSize)
                {
                    count++;
                }
                if (value == null && count > 0)
                {
                    count--;
                }
                if (currentIndex < windowSize - 1)
                {
                    ret[currentIndex] = null;
                    currentIndex++;
                    continue;
                }
                ret[currentIndex] = count;
                currentIndex++;
            }
            return ret;
        }

        public override PrimitiveDataFrameColumn<int> Count() => CountImplementation(_currentIndex, _currentColumn, _windowSize);

        public override DataFrameColumn Max()
        {
            throw new NotSupportedException();
        }

        public override DataFrameColumn Min()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Apply <paramref name="func"/> over a rolling window of values. 
        /// </summary>
        /// <param name="func">The delegate to apply for each window. The first parameter is a linked list of values in the current window. The second parameter is the current index in the column. The current window ends at the current index and includes values preceeding it</param>
        /// <returns>The result for each rolling window</returns>
        public StringDataFrameColumn Apply(Func<LinkedList<string>, long, string> func) => _currentColumn.ApplyRollingFunc(func, _windowSize);
    }
}
