// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Devices.I2c.Provider
{
    /// <summary>
    /// Represents properties and methods common to all I²C controllers.
    /// </summary>
    public interface II2cControllerProvider
    {
        /// <summary>
        /// Gets the I²C device provider with the specified settings.
        /// </summary>
        /// <param name="settings">The desired settings.</param>
        /// <returns>The I²C device provider.</returns>
        II2cDeviceProvider GetDeviceProvider(ProviderI2cConnectionSettings settings);
    }
}
