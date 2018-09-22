// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Devices.I2c
{
    /// <summary>
    /// Represents the I²C controller for the system.
    /// </summary>
    public abstract class I2cController
    {
        /// <summary>
        /// Gets the I²C device with the specified settings.
        /// </summary>
        /// <param name="settings">
        /// The desired I²C connection settings.
        /// </param>
        /// <returns>
        /// The I²C device.
        /// </returns>
        public abstract I2cDevice GetDevice(I2cConnectionSettings settings);
    }
}
