// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Devices.I2c.Provider
{
    internal interface IProviderI2cConnectionSettings
    {
        ProviderI2cBusSpeed BusSpeed { get; set; }
        ProviderI2cSharingMode SharingMode { get; set; }
        int SlaveAddress { get; set; }
    }
}
