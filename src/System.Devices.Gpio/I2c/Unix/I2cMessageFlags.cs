// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Devices.I2c.Unix
{
    [Flags]
    internal enum I2cMessageFlags : ushort
    {
        Write = 0x0000,
        Read = 0x0001,

        Retries = 0x0701,
        Timout = 0x0702,
        ChangeSlaveAddress = 0x0703,
        TenBitAddressing = 0x0704,
        Functionality = 0x0705,
        ForceChangeSlaveAddress = 0x0706,
        ReadWrite = 0x0707,
        SmBusPec = 0x0708,

        AckTest = 0x0710,

        SmBusAccess = 0x0720,

        IgnoreNak = 0x1000,
        RevDirAddress = 0x2000,
        NoStart = 0x4000,
        NoReadAck = 0x0800
    }
}
