// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Devices.I2c
{
    /// <summary>
    /// Describes the bus speeds that are available for connecting to an inter-integrated
    /// circuit (I²C) device. The bus speed is the frequency at which to clock the I²C
    /// bus when accessing the device.
    /// </summary>
    public enum I2cBusSpeed
    {
        /// <summary>
        /// The standard speed of 100 kilohertz (kHz). This speed is the default.
        /// </summary>
        StandardMode = 0,

        /// <summary>
        /// A fast speed of 400 kilohertz (kHz).
        /// </summary>
        FastMode = 1
    }
}
