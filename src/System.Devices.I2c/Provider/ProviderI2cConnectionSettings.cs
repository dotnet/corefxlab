// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Devices.I2c.Provider
{
    /// <summary>
    /// Represents the connection settings you want to use for an inter-integrated circuit
    /// (I²C) device.
    /// </summary>
    public sealed class ProviderI2cConnectionSettings : IProviderI2cConnectionSettings
    {
        /// <summary>
        /// Gets or sets the bus address of the inter-integrated circuit (I²C) device.
        /// </summary> 
        public int SlaveAddress { get; set; }

        /// <summary>
        /// Gets or sets the sharing mode to use to connect to the inter-integrated circuit
        /// (I²C) bus address. This mode determines whether other connections
        /// to the I²C bus address can be opened while you are connect to the
        /// I²C bus address.
        /// </summary> 
        public ProviderI2cSharingMode SharingMode { get; set; }

        /// <summary>
        /// Gets or sets the bus speed to use for connecting to an inter-integrated circuit
        /// (I²C) device. The bus speed is the frequency at which to clock the
        /// I²C bus when accessing the device.
        /// </summary>
        public ProviderI2cBusSpeed BusSpeed { get; set; }
    }
}
