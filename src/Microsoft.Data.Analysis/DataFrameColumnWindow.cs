using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Data.Analysis
{
    /// <summary>
    /// A window object that can be used for expanding and rolling computations
    /// </summary>
    public abstract class DataFrameColumnWindow
    {
        /// <summary>
        /// Counts the number of non-null values inside the window
        /// </summary>
        /// <returns>A DataFrame containing the count in each window</returns>
        public abstract PrimitiveDataFrameColumn<int> Count();

        /// <summary>
        /// Finds the min of the values in this window
        /// </summary>
        /// <returns>Returns a DataFrame containing the min in each window</returns>
        public virtual DataFrameColumn Min() => throw new NotImplementedException();

        /// <summary>
        /// Finds the max of the values in this window
        /// </summary>
        /// <returns>Returns a DataFrame containing the max in each window</returns>
        public virtual DataFrameColumn Max() => throw new NotImplementedException();
    }

}
