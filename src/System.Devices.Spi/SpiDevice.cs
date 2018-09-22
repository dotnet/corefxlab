// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Devices.Spi
{
    /// <summary>
    /// Represents a device connected through the SPI bus.
    /// </summary>
    public abstract class SpiDevice : IDisposable
    {
        /// <summary>
        /// Gets the connection settings used for communication with the SPI device.
        /// </summary>
        public SpiConnectionSettings ConnectionSettings { get; }

        /// <summary>
        /// Gets the unique ID associated with the SPI device.
        /// </summary>
        public string DeviceId { get; }

        /// <summary>
        /// Writes to the connected device.
        /// </summary>
        /// <param name="buffer">
        /// The buffer containing the data to write to the device.
        /// </param>
        public abstract void Write(Span<byte> buffer);

        /// <summary>
        /// Reads from the connected device.
        /// </summary>
        /// <param name="buffer">
        /// Array containing data read from the device.
        /// </param>
        public abstract void Read(Span<byte> buffer);

        /// <summary>
        /// Transfer data sequentially to the device.
        /// </summary>
        /// <param name="writeBuffer">
        /// Array containing data to write to the device.
        /// </param>
        /// <param name="readBuffer">
        /// Array containing data read from the device.
        /// </param>
        public abstract void TransferSequential(Span<byte> writeBuffer, Span<byte> readBuffer);

        /// <summary>
        /// Transfer data using a full duplex communication system. Full duplex allows both
        /// the master and the slave to communicate simultaneously.
        /// </summary>
        /// <param name="writeBuffer">
        /// Array containing data to write to the device.
        /// </param>
        /// <param name="readBuffer">
        /// Array containing data read from the device.
        /// </param>
        public abstract void TransferFullDuplex(Span<byte> writeBuffer, Span<byte> readBuffer);

        public abstract void Dispose();
        public abstract void Dispose(bool disposing);


        // TODO: Are the following really needed?

        ///// <summary>
        ///// Gets all the SPI buses found on the system.
        ///// </summary>
        ///// <returns>String containing all the buses found on the system.</returns>
        //public static string GetDeviceSelector();

        ///// <summary>
        ///// Gets all the SPI buses found on the system that match the input parameter.
        ///// </summary>
        ///// <param name="friendlyName">
        ///// Input parameter specifying an identifying name for the desired bus. This usually
        ///// corresponds to a name on the schematic.
        ///// </param>
        ///// <returns>
        ///// String containing all the buses that have the input in the name.
        ///// </returns>
        //public static string GetDeviceSelector(string friendlyName);

        ///// <summary>
        ///// Retrieves the info about a certain bus.
        ///// </summary>
        ///// <param name="busId">The id of the bus.</param>
        ///// <returns>The bus info requested.</returns>
        //public static SpiBusInfo GetBusInfo(string busId);

        ///// <summary>
        ///// Opens a device with the connection settings provided.
        ///// </summary>
        ///// <param name="busId">The id of the bus.</param>
        ///// <param name="settings">The SPI device requested.</param>
        ///// <returns></returns>
        //public static Task<SpiDevice> FromIdAsync(string busId, SpiConnectionSettings settings);
    }
}
