// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.IO.Pipelines.Networking.Windows.RIO.Internal.Winsock
{
    public struct Version
    {
        public ushort Raw;

        public Version(byte major, byte minor)
        {
            Raw = major;
            Raw <<= 8;
            Raw += minor;
        }

        public byte Major
        {
            get
            {
                ushort result = Raw;
                result >>= 8;
                return (byte)result;
            }
        }

        public byte Minor
        {
            get
            {
                ushort result = Raw;
                result &= 0x00FF;
                return (byte)result;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}", Major, Minor);
        }
    }
}