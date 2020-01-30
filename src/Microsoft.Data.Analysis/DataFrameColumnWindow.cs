using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Data.Analysis
{
    public abstract class DataFrameColumnWindow
    {
        /// <summary>
        /// Counts the number of non-null values inside the window
        /// </summary>
        /// <returns>A DataFrame containing the count</returns>
        public abstract PrimitiveDataFrameColumn<int> Count();

        /// <summary>
        /// Finds the sum of the values in this window
        /// </summary>
        /// <returns>Returns a DataFrame containing the sum in each window</returns>
        public virtual DataFrameColumn Sum() 
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds the mean of the values in this window
        /// </summary>
        /// <returns>Returns a DataFrame containing the mean in each window</returns>
        public abstract PrimitiveDataFrameColumn<double> Mean();

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
