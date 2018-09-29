// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Devices.I2c.Unix
{
    // TODO: See i2c_rdwr_ioctl_data.
    internal unsafe struct I2cRdWrIoctlData
    {
        public I2cMessage* Messages { get; set; }
        public uint NumberOfMessages { get; set; }
    }
}
