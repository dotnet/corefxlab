// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace System.Devices.Spi
{
    /// <summary>
    /// Represents the info about a SPI bus.
    /// </summary>
    public sealed class SpiBusInfo
    {
        /// <summary>
        /// Gets the number of chip select lines available on the bus.
        /// </summary>
        public int ChipSelectLineCount { get; }

        /// <summary>
        /// Maximum clock cycle frequency of the bus.
        /// </summary>
        public int MaxClockFrequency { get; }

        /// <summary>
        /// Minimum clock cycle frequency of the bus.
        /// </summary>
        public int MinClockFrequency { get; }

        /// <summary>
        /// Gets the bit lengths that can be used on the bus for transmitting data.
        /// </summary>
        public IReadOnlyList<int> SupportedDataBitLengths { get; }
    }
}
