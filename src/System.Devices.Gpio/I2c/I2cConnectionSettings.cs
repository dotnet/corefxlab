// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Devices.I2c
{
    /// <summary>
    /// Represents the settings for the connection with an <see cref="I2cDevice"/>.
    /// </summary>
    public sealed class I2cConnectionSettings
    {
        /// <summary>
        /// Gets or sets the I2C bus id for this connection.
        /// </summary>
        public uint BusId { get; set; }

        /// <summary>
        /// Gets or sets the I2C connection device address.
        /// </summary>
        public uint DeviceAddress { get; set; }

        /// <summary>
        /// Initializes new instance of <see cref="I2cConnectionSettings"/>.
        /// </summary>
        /// <param name="busId">The I2C bus id on which the connection will be made.</param>
        /// <param name="deviceAddress">The I2C connection device address.</param>
        public I2cConnectionSettings(uint busId, uint deviceAddress)
        {
            BusId = busId;
            DeviceAddress = deviceAddress;
        }

        /// <summary>
        /// Initializes new instance of <see cref="I2cConnectionSettings"/>.
        /// </summary>
        /// <param name="busId">The I2C bus id on which the connection will be made.</param>
        public I2cConnectionSettings(uint busId)
        {
            BusId = busId;
        }

        internal I2cConnectionSettings(I2cConnectionSettings newConnectionSettings)
        {
            BusId = newConnectionSettings.BusId;
            DeviceAddress = newConnectionSettings.DeviceAddress;
        }
    }
}
