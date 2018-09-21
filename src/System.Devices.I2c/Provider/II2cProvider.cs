// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Devices.I2c.Provider
{
    /// <summary>
    /// Represents actions common to all I²C providers.
    /// </summary>
    public interface II2cProvider
    {
        /// <summary>
        /// Gets all the I²C controllers that are on the system.
        /// </summary>
        /// <returns>
        /// When the method completes successfully, it returns a list of values that represent
        /// the available I²C controllers on the system.
        /// </returns>
        Task<IReadOnlyList<II2cControllerProvider>> GetControllersAsync();
    }
}
