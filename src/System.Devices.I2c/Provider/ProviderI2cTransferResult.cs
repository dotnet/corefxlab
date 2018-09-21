// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Devices.I2c.Provider
{
    /// <summary>
    /// Provides information about whether the data transfers that the ReadPartial, WritePartial,
    /// or WriteReadPartial method performed succeeded, and the actual number of bytes
    /// the method transferred.
    /// </summary>
    public struct ProviderI2cTransferResult
    {
        /// <summary>
        /// An enumeration value that indicates if the read or write operation transferred
        /// the full number of bytes that the method requested, or the reason that the full
        /// transfer did not succeed. For WriteReadPartial, the value indicates whether the
        /// data for both the write and the read operations was entirely transferred.
        /// </summary>
        public ProviderI2cTransferStatus Status;

        /// <summary>
        /// The actual number of bytes that the operation actually transferred. The following
        /// table describes what this value represents for each method.
        /// </summary>
        public uint BytesTransferred;
    }
}
