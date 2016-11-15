// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
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
