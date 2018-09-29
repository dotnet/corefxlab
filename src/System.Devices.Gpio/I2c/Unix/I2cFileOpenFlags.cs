// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Devices.I2c.Unix
{
    [Flags]
    internal enum I2cFileOpenFlags
    {
        ReadOnly = 0x00,
        NonBlock = 0x800,
        ReadWrite = 0x02,
        Sync = 0x101000
    }
}
