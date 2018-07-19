// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    /// <summary>
    /// Defines the SPI communication mode.
    /// The communication mode defines the clock edge on which the master out line toggles,
    /// the master in line samples, and the signal clock's signal steady level (named SCLK).
    /// Each mode is defined with a pair of parameters called clock polarity (CPOL) and clock phase (CPHA).
    /// </summary>
    public enum SpiMode
    {
        /// <summary>CPOL = 0, CPHA = 0</summary>
        Mode0 = 0,
        /// <summary>CPOL = 0, CPHA = 1</summary>
        Mode1 = 1,
        /// <summary>CPOL = 1, CPHA = 0</summary>
        Mode2 = 2,
        /// <summary>CPOL = 1, CPHA = 1</summary>
        Mode3 = 3
    }

    /// <summary>
    /// Represents the settings for the connection with an <see cref="SpiDevice"/>.
    /// </summary>
    public sealed class SpiConnectionSettings
    {
        /// <summary>
        /// Gets or sets the chip select line for the connection to the SPI device.
        /// </summary>
        public int ChipSelectLine { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="SpiMode"/> for this connection.
        /// </summary>
        public SpiMode Mode { get; set; }

        /// <summary>
        /// Gets or sets the bit length for data on this connection.
        /// </summary>
        public int DataBitLength { get; set; }

        /// <summary>
        /// Gets or sets the clock frequency for the connection.
        /// </summary>
        public int ClockFrequency { get; set; }

        private const SpiMode DefaultMode = SpiMode.Mode0;
        private const int DefaultDataBitLength = 8; // 1 byte
        private const int DefaultClockFrequency = 500 * 1000; // 500 KHz

        /// <summary>
        /// Initializes new instance of <see cref="SpiConnectionSettings"/>.
        /// </summary>
        /// <param name="chipSelectLine">The chip select line on which the connection will be made.</param>
        public SpiConnectionSettings(int chipSelectLine)
        {
            ChipSelectLine = chipSelectLine;
            DataBitLength = DefaultDataBitLength;
            ClockFrequency = DefaultClockFrequency;
            Mode = DefaultMode;
        }
    }
}
