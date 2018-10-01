// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Devices.I2c.Unix
{
    internal unsafe ref struct I2cMessage
    {
        public ushort Address;
        public I2cMessageFlags Flags;
        public ushort Length;
        public byte* Buffer;

        // TODO: These were not in original code (below), but used in other I2C implementations.  Possibly review/add later.
        //public int Error;
        //public short Done;
    }
}
