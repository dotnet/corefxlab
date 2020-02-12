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

        public override PrimitiveDataFrameColumn<int> Count()
        {
            _currentIndex = 0;
            int count = 0;
            PrimitiveDataFrameColumn<int> ret = new PrimitiveDataFrameColumn<int>("Count", _currentColumn.Length);
            for (long i = 0; i < _currentColumn.Length; i++)
            {
                string value = _currentColumn[i];
                if (value != null && count < _windowSize)
                {
                    count++;
                }
                if (value == null && count > 0)
                {
                    count--;
                }
                if (_currentIndex < _windowSize - 1)
                {
                    ret[_currentIndex] = null;
                    _currentIndex++;
                    continue;
                }
                ret[_currentIndex] = count;
                _currentIndex++;
            }
            return ret;
        }

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
