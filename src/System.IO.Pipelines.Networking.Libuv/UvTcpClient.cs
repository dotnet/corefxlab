// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Threading.Tasks;
using System.IO.Pipelines.Networking.Libuv.Interop;

namespace System.IO.Pipelines.Networking.Libuv
{
    public class UvTcpClient
    {
        private static readonly Action<UvConnectRequest, int, Exception, object> _connectCallback = OnConnection;
        private static readonly Action<object> _startConnect = state => ((UvTcpClient)state).DoConnect();

        private readonly TaskCompletionSource<UvTcpConnection> _connectTcs = new TaskCompletionSource<UvTcpConnection>();
        private readonly IPEndPoint _ipEndPoint;
        private readonly UvThread _thread;

        private UvTcpHandle _connectSocket;

        public UvTcpClient(UvThread thread, IPEndPoint endPoint)
        {
            _thread = thread;
            _ipEndPoint = endPoint;
        }

        public async Task<UvTcpConnection> ConnectAsync()
        {
            _thread.Post(_startConnect, this);

            var connection = await _connectTcs.Task;

            return connection;
        }

        private void DoConnect()
        {
            _connectSocket = new UvTcpHandle();
            _connectSocket.Init(_thread.Loop, null);

            var connectReq = new UvConnectRequest();
            connectReq.Init(_thread.Loop);
            connectReq.Connect(_connectSocket, _ipEndPoint, _connectCallback, this);
        }

        private static void OnConnection(UvConnectRequest req, int status, Exception exception, object state)
        {
            var client = (UvTcpClient)state;

            var connection = new UvTcpConnection(client._thread, client._connectSocket);

            Task.Run(() => client._connectTcs.TrySetResult(connection));
        }
    }
}
