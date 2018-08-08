// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.I2c
{
    /// <summary>
    /// Represents the settings for the connection with an <see cref="I2cDevice"/>.
    /// </summary>
    public sealed class I2cConnectionSettings
    {
        /// <summary>
        /// Gets or sets the I2c bus id for this connection.
        /// </summary>
        public uint BusId { get; set; }

        /// <summary>
        /// Gets or sets the I2c connection device address.
        /// </summary>
        public uint DeviceAddress { get; set; }

        /// <summary>
        /// Initializes new instance of <see cref="I2cConnectionSettings"/>.
        /// </summary>
        /// <param name="busId">The I2c bus id on which the connection will be made.</param>
        /// <param name="deviceAddress">The I2c connection device address.</param>
        public I2cConnectionSettings(uint busId, uint deviceAddress)
        {
            BusId = busId;
            DeviceAddress = deviceAddress;
        }

        /// <summary>
        /// Initializes new instance of <see cref="I2cConnectionSettings"/>.
        /// </summary>
        /// <param name="busId">The I2c bus id on which the connection will be made.</param>
        public I2cConnectionSettings(uint busId)
        {
            BusId = busId;
        }

        internal I2cConnectionSettings(I2cConnectionSettings other)
        {
            BusId = other.BusId;
            DeviceAddress = other.DeviceAddress;
        }
    }
}
