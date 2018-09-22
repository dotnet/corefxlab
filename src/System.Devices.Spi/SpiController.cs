// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Devices.Spi
{
    /// <summary>
    /// Represents the SPI controller on the system.
    /// </summary>
    public abstract class SpiController
    {
        /// <summary>
        /// Gets the SPI device with the specified settings.
        /// </summary>
        /// <param name="settings">
        /// The desired SPI connection settings.
        /// </param>
        /// <returns>
        /// The SPI device.
        /// </returns>
        public abstract SpiDevice GetDevice(SpiConnectionSettings settings);
    }
}
