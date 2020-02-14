// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Microsoft.Data.Analysis
{
    /// <summary>
    /// A window for <see cref="ArrowStringDataFrameColumn"/> to support rolling window operations
    /// </summary>
    public class ArrowStringDataFrameColumnRollingWindow : DataFrameColumnWindow
    {
        private int _windowSize;
        private ArrowStringDataFrameColumn _currentColumn;

        internal ArrowStringDataFrameColumnRollingWindow(int windowSize, ArrowStringDataFrameColumn currentColumn)
        {
            _windowSize = windowSize;
            _currentColumn = currentColumn;
        }

        public override PrimitiveDataFrameColumn<int> Count() => StringDataFrameColumnRollingWindow.CountImplementation(_currentColumn, _windowSize);

        public override DataFrameColumn Max() => throw new NotSupportedException();

        public override DataFrameColumn Min() => throw new NotSupportedException();

        /// <summary>
        /// Apply <paramref name="func"/> over a rolling window of values. 
        /// </summary>
        /// <param name="func">The delegate to apply for each window. The first parameter is a linked list of values in the current window. The second parameter is the current index in the column. The current window ends at the current index and includes values preceeding it</param>
        /// <returns>The result for each rolling window</returns>
        public ArrowStringDataFrameColumn Apply(Func<LinkedList<string>, long, string> func) => _currentColumn.ApplyRollingFunc(func, _windowSize);
    }
}
