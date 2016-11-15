// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.IO.Pipelines.Networking.Windows.RIO.Internal.Winsock
{
    public sealed class RegisteredIO
    {
        public RioRegisterBuffer RioRegisterBuffer;

        public RioCreateCompletionQueue RioCreateCompletionQueue;
        public RioCreateRequestQueue RioCreateRequestQueue;


        public RioReceive RioReceive;
        public RioSend Send;

        public RioNotify Notify;

        public RioCloseCompletionQueue CloseCompletionQueue;
        public RioDequeueCompletion DequeueCompletion;
        public RioDeregisterBuffer DeregisterBuffer;
        public RioResizeCompletionQueue ResizeCompletionQueue;
        public RioResizeRequestQueue ResizeRequestQueue;


        public const long CachedValue = long.MinValue;

        public RegisteredIO()
        {
        }
    }
}