// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net;

namespace System.IO.Pipelines.Networking.Libuv.Interop
{
    /// <summary>
    /// Summary description for UvWriteRequest
    /// </summary>
    public class UvConnectRequest : UvRequest
    {
        private readonly static Uv.uv_connect_cb _uv_connect_cb = (req, status) => UvConnectCb(req, status);

        private Action<UvConnectRequest, int, Exception, object> _callback;
        private object _state;

        public UvConnectRequest() : base()
        {
        }

        public void Init(UvLoopHandle loop)
        {
            var requestSize = loop.Libuv.req_size(Uv.RequestType.CONNECT);
            CreateMemory(
                loop.Libuv,
                loop.ThreadId,
                requestSize);
        }

        public void Connect(
            UvTcpHandle socket,
            IPEndPoint endpoint,
            Action<UvConnectRequest, int, Exception, object> callback,
            object state)
        {
            _callback = callback;
            _state = state;

            var addressText = endpoint.Address.ToString();

            _uv.ip4_addr(addressText, endpoint.Port, out SockAddr addr, out Exception error1);

            if (error1 != null)
            {
                _uv.ip6_addr(addressText, endpoint.Port, out addr, out Exception error2);
                if (error2 != null)
                {
                    throw error1;
                }
            }

            Pin();
            Libuv.tcp_connect(this, socket, ref addr, _uv_connect_cb);
        }

        public void Connect(
            UvPipeHandle pipe,
            string name,
            Action<UvConnectRequest, int, Exception, object> callback,
            object state)
        {
            _callback = callback;
            _state = state;

            Pin();
            Libuv.pipe_connect(this, pipe, name, _uv_connect_cb);
        }

        private static void UvConnectCb(IntPtr ptr, int status)
        {
            var req = FromIntPtr<UvConnectRequest>(ptr);
            req.Unpin();

            var callback = req._callback;
            req._callback = null;

            var state = req._state;
            req._state = null;

            Exception error = null;
            if (status < 0)
            {
                req.Libuv.Check(status, out error);
            }

            callback(req, status, error, state);
        }
    }
}