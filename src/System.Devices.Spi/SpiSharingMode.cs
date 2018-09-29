// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Devices.Spi
{
    /// <summary>
    /// Describes the modes in which you can connect to an SPI bus address.
    /// These modes determine whether other connections to the SPI
    /// bus address can be opened while you are connected to the SPI bus address.
    /// </summary>
    public enum SpiSharingMode
    {
        /// <summary>
        /// Connects to the SPI bus address exclusively, so that no other connection
        /// to the SPI bus address can be made while you remain connected.
        /// This mode is the default mode.
        /// </summary>
        Exclusive = 0,

        /// <summary>
        /// Connects to the SPI bus address in shared mode, so that other connections
        /// to the SPI bus address can be made while you remain connected.
        /// </summary>
        Shared = 1
    }
}
