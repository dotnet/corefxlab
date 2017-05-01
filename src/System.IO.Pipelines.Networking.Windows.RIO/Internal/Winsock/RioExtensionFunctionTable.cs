// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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