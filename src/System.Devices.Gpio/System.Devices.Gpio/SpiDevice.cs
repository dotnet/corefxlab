// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    public abstract class SpiDevice : IDisposable
    {
        protected SpiConnectionSettings _settings;

        public SpiDevice(SpiConnectionSettings settings)
        {
            _settings = settings;
        }

        public abstract void Dispose();

        public SpiConnectionSettings GetConnectionSettings() => new SpiConnectionSettings(_settings);

        public abstract void TransferFullDuplex(byte[] writeBuffer, byte[] readBuffer);
        public abstract void Read(byte[] buffer);
        public abstract void Write(byte[] buffer);
    }
}
