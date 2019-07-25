// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Data
{
    public abstract partial class BaseColumn
    {
        /// <summary>
        /// Clips values beyond the specified thresholds
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="lower">Minimum value. All values below this threshold will be set to it</param>
        /// <param name="upper">Maximum value. All values above this threshold will be set to it</param>
        public virtual BaseColumn Clip<U>(U lower, U upper) => throw new NotImplementedException();

        /// <summary>
        /// Returns a DataFrame with statistics that describe the column
        /// </summary>
        /// <returns></returns>
        public virtual DataFrame Description() => throw new NotImplementedException();
    }
}
