using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Data.Analysis
{
    //public abstract class Window<T>
    //{
    //    /// <summary>
    //    /// Counts the number of non-null values inside the window
    //    /// </summary>
    //    /// <returns>A DataFrame or a DataFrameColumn containing the count</returns>
    //    public abstract T Count();

    //    /// <summary>
    //    /// Finds the sum of the values in this window
    //    /// </summary>
    //    /// <returns>Returns a DataFrame containing the sum in each window</returns>
    //    public abstract T Sum();

    //    /// <summary>
    //    /// Finds the mean of the values in this window
    //    /// </summary>
    //    /// <returns>Returns a DataFrame containing the mean in each window</returns>
    //    public abstract DataFrame Mean();

    //    /// <summary>
    //    /// Finds the min of the values in this window
    //    /// </summary>
    //    /// <returns>Returns a DataFrame containing the min in each window</returns>
    //    public abstract DataFrame Min();

    //    /// <summary>
    //    /// Finds the max of the values in this window
    //    /// </summary>
    //    /// <returns>Returns a DataFrame containing the max in each window</returns>
    //    public abstract DataFrame Max();
    //}

    public abstract class DataFrameWindow
    {
        /// <summary>
        /// Counts the number of non-null values inside the window
        /// </summary>
        /// <returns>A DataFrame containing the count</returns>
        public abstract DataFrame Count();

        /// <summary>
        /// Finds the sum of the values in this window
        /// </summary>
        /// <returns>Returns a DataFrame containing the sum in each window</returns>
        public abstract DataFrame Sum();

        /// <summary>
        /// Finds the mean of the values in this window
        /// </summary>
        /// <returns>Returns a DataFrame containing the mean in each window</returns>
        public abstract DataFrame Mean();

        /// <summary>
        /// Finds the min of the values in this window
        /// </summary>
        /// <returns>Returns a DataFrame containing the min in each window</returns>
        public abstract DataFrame Min();

        /// <summary>
        /// Finds the max of the values in this window
        /// </summary>
        /// <returns>Returns a DataFrame containing the max in each window</returns>
        public abstract DataFrame Max();
    }

    public class Rolling : DataFrameWindow
    {
        private int _windowSize;
        public Rolling(int windowSize)
        {
            _windowSize = windowSize;
        }

        public override DataFrame Count()
        {
            throw new NotImplementedException();
        }

        public override DataFrame Max()
        {
            throw new NotImplementedException();
        }

        public override DataFrame Mean()
        {
            throw new NotImplementedException();
        }

        public override DataFrame Min()
        {
            throw new NotImplementedException();
        }

        public override DataFrame Sum()
        {
            throw new NotImplementedException();
        }
    }
}
