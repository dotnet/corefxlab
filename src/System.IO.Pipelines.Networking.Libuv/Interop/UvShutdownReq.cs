// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace System.IO.Pipelines.Networking.Libuv.Interop
{
    /// <summary>
    /// Summary description for UvShutdownRequest
    /// </summary>
    public class UvShutdownReq : UvRequest
    {
        private readonly static Uv.uv_shutdown_cb _uv_shutdown_cb = UvShutdownCb;

        private Action<UvShutdownReq, int, object> _callback;
        private object _state;

        public UvShutdownReq() : base ()
        {
        }

        public void Init(UvLoopHandle loop)
        {
            CreateMemory(
                loop.Libuv, 
                loop.ThreadId,
                loop.Libuv.req_size(Uv.RequestType.SHUTDOWN));
        }

        public void Shutdown(UvStreamHandle handle, Action<UvShutdownReq, int, object> callback, object state)
        {
            _callback = callback;
            _state = state;
            Pin();
            _uv.shutdown(this, handle, _uv_shutdown_cb);
        }

        private static void UvShutdownCb(IntPtr ptr, int status)
        {
            var req = FromIntPtr<UvShutdownReq>(ptr);
            req.Unpin();
            req._callback(req, status, req._state);
            req._callback = null;
            req._state = null;
        }
    }
}