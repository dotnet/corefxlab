// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Data.Analysis
{
    /// <summary>
    /// A window object that can be used for rolling computations
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
