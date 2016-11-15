// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace System.IO.Pipelines.Networking.Windows.RIO.Internal
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RioRequestResult
    {
        public int Status;
        public uint BytesTransferred;
        public long ConnectionCorrelation;
        public long RequestCorrelation;
    }
}
