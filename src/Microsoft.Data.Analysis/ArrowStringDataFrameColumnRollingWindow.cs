using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;

namespace Microsoft.Data.Analysis
{
    public class ArrowStringDataFrameColumnRollingWindow : DataFrameColumnWindow
    {
        private int _windowSize;
        private ArrowStringDataFrameColumn _currentColumn;
        private long _currentIndex;

        internal ArrowStringDataFrameColumnRollingWindow(int windowSize, ArrowStringDataFrameColumn currentColumn)
        {
            _windowSize = windowSize;
            _currentColumn = currentColumn;
        }

        public override PrimitiveDataFrameColumn<int> Count() => StringDataFrameColumnRollingWindow.CountImplementation(_currentIndex, _currentColumn, _windowSize);

        public override DataFrameColumn Max() => throw new NotSupportedException();

        public override DataFrameColumn Min() => throw new NotSupportedException();

        // ArrowStringDataFrameColumnRollingWindow does not get an Apply function because ArrowStringDataFrameColumn is immutable
    }
}
