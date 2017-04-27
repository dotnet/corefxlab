// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace System.IO.Pipelines.Networking.Windows.RIO.Internal
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RioBufferSegment
    {
        public RioBufferSegment(IntPtr bufferId, uint offset, uint length)
        {
            BufferId = bufferId;
            Offset = offset;
            Length = length;
        }

        IntPtr BufferId;
        public readonly uint Offset;
        public uint Length;
    }
}
