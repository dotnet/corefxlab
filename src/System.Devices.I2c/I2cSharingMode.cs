// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Devices.I2c
{
    /// <summary>
    /// Describes the modes in which you can connect to an inter-integrated circuit (I²C)
    /// bus address. These modes determine whether other connections to the I²C
    /// bus address can be opened while you are connected to the I²C bus address.
    /// </summary>
    public enum I2cSharingMode
    {
        /// <summary>
        /// Connects to the I²C bus address exclusively, so that no other connection
        /// to the I²C bus address can be made while you remain connected. This
        /// mode is the default mode.
        /// </summary>
        Exclusive = 0,

        /// <summary>
        /// Connects to the I²C bus address in shared mode, so that other connections
        /// to the I²C bus address can be made while you remain connected.
        /// </summary>
        Shared = 1
    }
}
