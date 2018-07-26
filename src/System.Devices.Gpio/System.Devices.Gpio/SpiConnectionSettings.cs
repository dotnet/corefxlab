﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    /// <summary>
    /// Represents the settings for the connection with an <see cref="SpiDevice"/>.
    /// </summary>
    public sealed class SpiConnectionSettings
    {
        /// <summary>
        /// Gets or sets the Spi bus id for this connection.
        /// </summary>
        public int BusId { get; set; }

        /// <summary>
        /// Gets or sets the chip select line for this connection.
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
        private const int DefaultClockFrequency = 500_000; // 500 KHz

        /// <summary>
        /// Initializes new instance of <see cref="SpiConnectionSettings"/>.
        /// </summary>
        /// <param name="busId">The Spi bus id on which the connection will be made.</param>
        /// <param name="chipSelectLine">The chip select line on which the connection will be made.</param>
        public SpiConnectionSettings(int busId, int chipSelectLine)
        {
            BusId = busId;
            ChipSelectLine = chipSelectLine;
            DataBitLength = DefaultDataBitLength;
            ClockFrequency = DefaultClockFrequency;
            Mode = DefaultMode;
        }

        internal SpiConnectionSettings(SpiConnectionSettings other)
        {
            BusId = other.BusId;
            ChipSelectLine = other.ChipSelectLine;
            DataBitLength = other.DataBitLength;
            ClockFrequency = other.ClockFrequency;
            Mode = other.Mode;
        }
    }
}
