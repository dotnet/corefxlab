// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Devices.I2c.Provider
{
    /// <summary>
    /// Describes whether the data transfers that the ReadPartial, WritePartial, or WriteReadPartial
    /// method performed succeeded, or provides the reason that the transfers did not
    /// succeed.
    /// </summary>
    public enum ProviderI2cTransferStatus
    {
        /// <summary>
        /// The data was entirely transferred. For WriteReadPartial, the data for both the
        /// write and the read operations was entirely transferred.
        /// </summary>
        FullTransfer = 0,

        /// <summary>
        /// The I²C device negatively acknowledged the data transfer before all
        /// of the data was transferred.
        /// </summary>
        PartialTransfer = 1,

        /// <summary>
        /// The bus address was not acknowledged.
        /// </summary>
        SlaveAddressNotAcknowledged = 2
    }
}
