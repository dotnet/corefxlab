// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Devices.I2c.Provider
{
    /// <summary>
    /// Represents methods common to all I²C device providers.
    /// </summary>
    public interface II2cDeviceProvider : IDisposable
    {
        /// <summary>
        /// Writes data to the inter-integrated circuit (I²C) bus on which the
        /// device is connected.
        /// </summary>
        /// <param name="buffer">
        /// A buffer that contains the data that you want to write to the I²C
        /// device. This data should not include the bus address.
        /// </param>
        void Write(byte[] buffer);

        /// <summary>
        /// Writes data to the inter-integrated circuit (I²C) bus on which the
        /// device is connected, and returns information about the success of the operation
        /// that you can use for error handling.
        /// </summary>
        /// <param name="buffer">
        /// A buffer that contains the data that you want to write to the I²C
        /// device. This data should not include the bus address.
        /// </param>
        /// <returns>
        /// A structure that contains information about the success of the write operation
        /// and the actual number of bytes that the operation wrote into the buffer.
        /// </returns>
        ProviderI2cTransferResult WritePartial(byte[] buffer);

        /// <summary>
        /// Reads data from the inter-integrated circuit (I²C) bus on which the
        /// device is connected into the specified buffer.
        /// </summary>
        /// <param name="buffer">
        /// The buffer to which you want to read the data from the I²C bus. The
        /// length of the buffer determines how much data to request from the device.
        /// </param>
        void Read(byte[] buffer);

        /// <summary>
        /// Reads data from the inter-integrated circuit (I²C) bus on which the
        /// device is connected into the specified buffer, and returns information about
        /// the success of the operation that you can use for error handling.
        /// </summary>
        /// <param name="buffer">
        /// The buffer to which you want to read the data from the I²C bus. The
        /// length of the buffer determines how much data to request from the device.
        /// </param>
        /// <returns>
        /// A structure that contains information about the success of the read operation
        /// and the actual number of bytes that the operation read into the buffer.
        /// </returns>
        ProviderI2cTransferResult ReadPartial(byte[] buffer);

        /// <summary>
        /// Performs an atomic operation to write data to and then read data from the inter-integrated
        /// circuit (I²C) bus on which the device is connected, and sends a restart
        /// condition between the write and read operations.
        /// </summary>
        /// <param name="writeBuffer">
        /// A buffer that contains the data that you want to write to the I²C
        /// device. This data should not include the bus address.
        /// </param>
        /// <param name="readBuffer">
        /// The buffer to which you want to read the data from the I²C bus. The
        /// length of the buffer determines how much data to request from the device.
        /// </param>
        void WriteRead(byte[] writeBuffer, byte[] readBuffer);

        /// <summary>
        /// Performs an atomic operation to write data to and then read data from the inter-integrated
        /// circuit (I²C) bus on which the device is connected, and returns information
        /// about the success of the operation that you can use for error handling.
        /// </summary>
        /// <param name="writeBuffer">
        /// A buffer that contains the data that you want to write to the I²C device.
        /// This data should not include the bus address.
        /// </param>
        /// <param name="readBuffer">
        /// The buffer to which you want to read the data from the I²C bus. The
        /// length of the buffer determines how much data to request from the device.
        /// </param>
        /// <returns>
        /// A structure that contains information about whether both the read and write parts
        /// of the operation succeeded and the sum of the actual number of bytes that the
        /// operation wrote and the actual number of bytes that the operation read.
        /// </returns>
        ProviderI2cTransferResult WriteReadPartial(byte[] writeBuffer, byte[] readBuffer);

        /// <summary>
        /// Gets the plug and play device identifier of the inter-integrated circuit (I²C)
        /// bus controller for the device.
        string DeviceId { get; }
    }
}
