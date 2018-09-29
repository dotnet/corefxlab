// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Devices.Spi
{
    /// <summary>
    /// Represents the settings for the connection with an SpiDevice.
    /// </summary>
    public sealed class SpiConnectionSettings
    {
        /// <summary>
        /// Initializes new instance of <see cref="SpiConnectionSettings"/>.
        /// </summary>
        /// <param name="chipSelectLine">
        /// The chip select line to the SPI device.
        /// </param>
        /// <param name="sharingMode">
        /// The sharing mode for the SPI connection.
        /// </param>
        /// <param name="mode">
        /// The SpiMode for the SPI connection.
        /// </param>
        /// <param name="dataBitLength">
        /// The bit length for data on the SPI connection.
        /// </param>
        /// <param name="clockFrequency">
        /// The clock frequency for the SPI connection.
        /// </param>
        public SpiConnectionSettings(
            int chipSelectLine,
            SpiSharingMode sharingMode = SpiSharingMode.Exclusive,
            SpiMode mode = SpiMode.Mode0,
            int dataBitLength = 8,
            int clockFrequency = 500_000)
        {
            ChipSelectLine = chipSelectLine;
            SharingMode = sharingMode;
            Mode = mode;
            DataBitLength = dataBitLength;
            ClockFrequency = clockFrequency;
        }

        /// <summary>
        /// Gets or sets the chip select line to the SPI device.
        /// </summary>
        public int ChipSelectLine { get; set; }

        /// <summary>
        /// Gets or sets the sharing mode for the SPI connection.
        /// </summary>
        public SpiSharingMode SharingMode { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="SpiMode"/> for the SPI connection.
        /// </summary>
        public SpiMode Mode { get; set; }

        /// <summary>
        /// Gets or sets the bit length for data on the SPI connection.
        /// </summary>
        public int DataBitLength { get; set; }

        /// <summary>
        /// Gets or sets the clock frequency for the SPI connection.
        /// </summary>
        public int ClockFrequency { get; set; }
    }
}
