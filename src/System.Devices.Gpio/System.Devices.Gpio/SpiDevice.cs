// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    public abstract class SpiDevice : IDisposable
    {
        public SpiConnectionSettings ConnectionSettings { get; }

        public SpiDevice(SpiConnectionSettings settings)
        {
            ConnectionSettings = settings;
        }

        public abstract void Dispose();

        public abstract void TransferFullDuplex(byte[] writeBuffer, byte[] readBuffer);
        public abstract void Read(byte[] buffer);
        public abstract void Write(byte[] buffer);
    }
}
