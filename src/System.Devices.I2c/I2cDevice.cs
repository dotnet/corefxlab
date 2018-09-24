// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Devices.I2c
{
    /// <summary>
    /// Represents a communications channel to a device on an inter-integrated circuit
    /// (I²C) bus.
    /// </summary>
    public abstract class I2cDevice : IDisposable
    {
        /// <summary>
        /// Gets the connection settings used for communication with the inter-integrated
        /// circuit (I²C) device.
        /// </summary>
        I2cConnectionSettings ConnectionSettings { get; }

        /// <summary>
        /// Gets the unique ID associated with the inter-integrated circuit (I²C)
        /// bus controller for the device.
        /// </summary>
        string DeviceId { get; }

        /// <summary>
        /// Writes data to the inter-integrated circuit (I²C) bus on which the
        /// device is connected, based on the bus address specified in the
        /// <see cref="I2cConnectionSettings"/> object used to create the <see cref="I2cDevice"/> object.
        /// </summary>
        /// <param name="buffer">
        /// A buffer that contains the data to write to the I²C device.
        /// This data should not include the bus address.
        /// </param>
        public abstract void Write(ReadOnlySpan<byte> buffer);

        /// <summary>
        /// Writes data to the inter-integrated circuit (I²C) bus on which the
        /// device is connected, and returns information about the success of
        /// the operation that can be used for error handling.
        /// </summary>
        /// <param name="buffer">
        /// A buffer that contains the data to write to the I²C device.
        /// This data should not include the bus address.
        /// </param>
        /// <returns>
        /// Information about the success of the write operation and the actual
        /// number of bytes that the operation wrote into the buffer.
        /// </returns>
        public abstract I2cTransferResult WritePartial(ReadOnlySpan<byte> buffer);

        /// <summary>
        /// Reads data from the inter-integrated circuit (I²C) bus on which the
        /// device is connected into the specified buffer.
        /// </summary>
        /// <param name="buffer">
        /// The buffer to read the data from the I²C bus. The
        /// length of the buffer determines how much data to request from the device.
        /// </param>
        public abstract void Read(Span<byte> buffer);

        /// <summary>
        /// Reads data from the inter-integrated circuit (I²C) bus on which the
        /// device is connected into the specified buffer, and returns information about
        /// the success of the operation that can be used for error handling.
        /// </summary>
        /// <param name="buffer">
        /// The buffer to read the data from the I²C bus. The length of the
        /// buffer determines how much data to request from the device.
        /// </param>
        /// <returns>
        /// Information about the success of the read operation and the actual number
        /// of bytes that the operation read into the buffer.
        /// </returns>
        public abstract I2cTransferResult ReadPartial(Span<byte> buffer);

        /// <summary>
        /// Performs an atomic operation to write data to and then read data from the
        /// inter-integrated circuit (I²C) bus on which the device is connected, and
        /// sends a restart condition between the write and read operations.
        /// </summary>
        /// <param name="writeBuffer">
        /// A buffer that contains the data to write to the I²C device.
        /// This data should not include the bus address.
        /// </param>
        /// <param name="readBuffer">
        /// The buffer to read the data from the I²C bus.
        /// The length of the buffer determines how much data to request from the device.
        /// </param>
        public abstract void WriteRead(ReadOnlySpan<byte> writeBuffer, Span<byte> readBuffer);

        /// <summary>
        /// Performs an atomic operation to write data to and then read data from the
        /// inter-integrated circuit (I²C) bus on which the device is connected, and returns
        /// information about the success of the operation that can be used for error handling.
        /// </summary>
        /// <param name="writeBuffer">
        /// A buffer that contains the data to write to the I²C device.
        /// This data should not include the bus address.
        /// </param>
        /// <param name="readBuffer">
        /// The buffer to read the data from the I²C bus.
        /// The length of the buffer determines how much data to request from the device.
        /// </param>
        /// <returns>
        /// Information about whether both the read and write parts of the operation
        /// succeeded and the sum of the actual number of bytes that the operation
        /// wrote and the actual number of bytes that the operation read.
        /// </returns>
        public abstract I2cTransferResult WriteReadPartial(ReadOnlySpan<byte> writeBuffer, Span<byte> readBuffer);

        public abstract void Dispose();
        public abstract void Dispose(bool disposing);
    }
}
