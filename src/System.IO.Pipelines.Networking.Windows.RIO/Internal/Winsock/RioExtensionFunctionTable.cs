// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.InteropServices;

namespace System.IO.Pipelines.Networking.Windows.RIO.Internal.Winsock
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RioExtensionFunctionTable
    {
        public UInt32 Size;

        public IntPtr RIOReceive;
        public IntPtr RIOReceiveEx;
        public IntPtr RIOSend;
        public IntPtr RIOSendEx;
        public IntPtr RIOCloseCompletionQueue;
        public IntPtr RIOCreateCompletionQueue;
        public IntPtr RIOCreateRequestQueue;
        public IntPtr RIODequeueCompletion;
        public IntPtr RIODeregisterBuffer;
        public IntPtr RIONotify;
        public IntPtr RIORegisterBuffer;
        public IntPtr RIOResizeCompletionQueue;
        public IntPtr RIOResizeRequestQueue;
    }
}