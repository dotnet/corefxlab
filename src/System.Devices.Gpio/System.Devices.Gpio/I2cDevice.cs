// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    public abstract class I2cDevice : IDisposable
    {
        protected I2cConnectionSettings _settings;

        public I2cDevice(I2cConnectionSettings settings)
        {
            _settings = settings;
        }

        public abstract void Dispose();

        public I2cConnectionSettings GetConnectionSettings() => new I2cConnectionSettings(_settings);

        public abstract void WriteRead(byte[] writeBuffer, byte[] readBuffer);
        public abstract void Read(byte[] buffer);
        public abstract void Write(byte[] buffer);
    }
}
