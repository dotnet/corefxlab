// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net;
using System.Threading.Tasks;
using System.IO.Pipelines.Networking.Libuv.Interop;

namespace System.IO.Pipelines.Networking.Libuv
{
    public class UvTcpListener : IDisposable
    {
        private static Action<UvStreamHandle, int, Exception, object> _onConnectionCallback = OnConnectionCallback;
        private static Action<object> _startListeningCallback = state => ((UvTcpListener)state).Listen();
        private static Action<object> _stopListeningCallback = state => ((UvTcpListener)state).Shutdown();

        private readonly IPEndPoint _endpoint;
        private readonly UvThread _thread;

        private UvTcpHandle _listenSocket;
        private Func<UvTcpConnection, Task> _callback;

        private TaskCompletionSource<object> _startedTcs = new TaskCompletionSource<object>();

        public UvTcpListener(UvThread thread, IPEndPoint endpoint)
        {
            _thread = thread;
            _endpoint = endpoint;
        }

        public void OnConnection(Func<UvTcpConnection, Task> callback)
        {
            _callback = callback;
        }

        public Task StartAsync()
        {
            // TODO: Make idempotent
            _thread.Post(_startListeningCallback, this);

            return _startedTcs.Task;
        }

        public void Dispose()
        {
            // TODO: Make idempotent
            _thread.Post(_stopListeningCallback, this);
        }

        private void Shutdown()
        {
            _listenSocket.Dispose();
        }

        private void Listen()
        {
            // TODO: Error handling
            _listenSocket = new UvTcpHandle();
            _listenSocket.Init(_thread.Loop, null);
            _listenSocket.NoDelay(true);
            _listenSocket.Bind(_endpoint);
            _listenSocket.Listen(10, _onConnectionCallback, this);

            // Don't complete the task on the UV thread
            Task.Run(() => _startedTcs.TrySetResult(null));
        }

        private static void OnConnectionCallback(UvStreamHandle listenSocket, int status, Exception error, object state)
        {
            var listener = (UvTcpListener)state;

            var acceptSocket = new UvTcpHandle();

            try
            {
                acceptSocket.Init(listener._thread.Loop, null);
                acceptSocket.NoDelay(true);
                listenSocket.Accept(acceptSocket);
                var connection = new UvTcpConnection(listener._thread, acceptSocket);
                ExecuteCallback(listener, connection);
            }
            catch (UvException)
            {
                acceptSocket.Dispose();
            }
        }

        private static async void ExecuteCallback(UvTcpListener listener, UvTcpConnection connection)
        {
            try
            {
                await listener._callback?.Invoke(connection);
            }
            catch
            {
                // Swallow exceptions
            }
            finally
            {
                await connection.DisposeAsync();
            }
        }
    }
}