// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Devices.I2c
{
    // TODO: Document
    public interface II2cDevice : IDisposable
    {
        void Write(byte[] buffer);
        I2cTransferResult WritePartial(byte[] buffer);
        void Read(byte[] buffer);
        I2cTransferResult ReadPartial(byte[] buffer);
        void WriteRead(byte[] writeBuffer, byte[] readBuffer);
        I2cTransferResult WriteReadPartial(byte[] writeBuffer, byte[] readBuffer);

        I2cConnectionSettings ConnectionSettings { get; }
        string DeviceId { get; }
    }
}
