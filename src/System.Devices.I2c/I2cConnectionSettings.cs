// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Devices.I2c
{
    public sealed class I2cConnectionSettings
    {
        /// <summary>
        /// Creates and initializes a new instance of the <see cref="I2cConnectionSettings"/> class for
        /// inter-integrated circuit (I²C) device with specified bus address, using the
        /// default settings of the standard mode for the bus speed and exclusive sharing mode.
        /// </summary>
        /// <param name="slaveAddress">
        /// The bus address of the inter-integrated circuit (I²C) device to which
        /// the settings of the <see cref="I2cConnectionSettings"/> should apply. Only 7-bit addressing
        /// is supported, so the range of values that are valid is from 8 to 119.
        /// </param>
        /// <param name="sharingMode">
        /// The sharing mode to use to connect to the inter-integrated circuit (I²C) bus address.
        /// </param>
        /// <param name="busSpeed">
        /// The bus speed to use for connecting to an inter-integrated circuit (I²C) device.
        /// </param>
        public I2cConnectionSettings(
            int slaveAddress,
            I2cSharingMode sharingMode = I2cSharingMode.Exclusive,
            I2cBusSpeed busSpeed = I2cBusSpeed.StandardMode)
        {
            SlaveAddress = slaveAddress;
            SharingMode = sharingMode;
            BusSpeed = busSpeed;
        }

        /// <summary>
        /// Gets or sets the bus address of the inter-integrated circuit (I²C) slave device.
        /// </summary>
        public int SlaveAddress { get; set; }

        /// <summary>
        /// Gets or sets the sharing mode to use to connect to the inter-integrated circuit
        /// (I²C) bus address. This mode determines whether other connections to
        /// the I²C bus address can be opened while connected to the I²C bus address.
        /// </summary>
        public I2cSharingMode SharingMode { get; set; }

        /// <summary>
        /// Gets or sets the bus speed to use for connecting to an inter-integrated circuit
        /// (I²C) device. The bus speed is the frequency at which to clock the
        /// I²C bus when accessing the device.
        /// </summary>
        public I2cBusSpeed BusSpeed { get; set; }
    }
}
