// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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