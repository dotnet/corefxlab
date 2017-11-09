// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net;
using System.Runtime.InteropServices;

namespace System.IO.Pipelines.Networking.Libuv.Interop
{
    public class UvTcpHandle : UvStreamHandle
    {
        public UvTcpHandle() : base()
        {
        }

        public void Init(UvLoopHandle loop, Action<Action<IntPtr>, IntPtr> queueCloseHandle)
        {
            CreateHandle(
                loop.Libuv,
                loop.ThreadId,
                loop.Libuv.handle_size(Uv.HandleType.TCP), queueCloseHandle);

            _uv.tcp_init(loop, this);
        }

        public void Bind(IPEndPoint endpoint)
        {
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

            _uv.tcp_bind(this, ref addr, 0);
        }

        public IPEndPoint GetPeerIPEndPoint()
        {
            int namelen = Marshal.SizeOf<SockAddr>();
            _uv.tcp_getpeername(this, out SockAddr socketAddress, ref namelen);

            return socketAddress.GetIPEndPoint();
        }

        public IPEndPoint GetSockIPEndPoint()
        {
            int namelen = Marshal.SizeOf<SockAddr>();
            _uv.tcp_getsockname(this, out SockAddr socketAddress, ref namelen);

            return socketAddress.GetIPEndPoint();
        }

        public void Open(IntPtr hSocket)
        {
            _uv.tcp_open(this, hSocket);
        }

        public void NoDelay(bool enable)
        {
            _uv.tcp_nodelay(this, enable);
        }
    }
}
