// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Networking.Sockets
{
    /// <summary>
    /// Allows a managed socket to be used as a server, listening on a designated address and accepting connections from clients
    /// </summary>
    public class SocketListener : IDisposable
    {
        private readonly bool _ownsFactory;
        private Socket _socket;
        private Socket Socket => _socket;
        private PipelineFactory _factory;
        private PipelineFactory PipelineFactory => _factory;
        private Func<SocketConnection, Task> Callback { get; set; }
        static readonly EventHandler<SocketAsyncEventArgs> _asyncCompleted = OnAsyncCompleted;

        /// <summary>
        /// Creates a new SocketListener instance
        /// </summary>
        /// <param name="factory">Optionally allows the underlying <see cref="PipelineFactory"/> (and hence memory pool) to be specified; if one is not provided, a <see cref="PipelineFactory"/> will be instantiated and owned by the listener</param>
        public SocketListener(PipelineFactory factory = null)
        {
            _ownsFactory = factory == null;
            _factory = factory ?? new PipelineFactory();
        }

        /// <summary>
        /// Releases all resources owned by the listener
        /// </summary>
        public void Dispose() => Dispose(true);
        /// <summary>
        /// Releases all resources owned by the listener
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                GC.SuppressFinalize(this);
                _socket?.Dispose();
                _socket = null;
                if (_ownsFactory) { _factory?.Dispose(); }
                _factory = null;
            }
        }

        /// <summary>
        /// Commences listening for and accepting connection requests from clients
        /// </summary>
        /// <param name="endpoint">The endpoint on which to listen</param>
        public void Start(IPEndPoint endpoint)
        {
            if (_socket == null)
            {
                _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                _socket.Bind(endpoint);
                _socket.Listen(10);
                var args = new SocketAsyncEventArgs(); // note: we keep re-using same args over for successive accepts
                                                       // so usefulness of pooling here minimal; this is per listener
                args.Completed += _asyncCompleted;
                args.UserToken = this;
                BeginAccept(args);
            }
        }

        /// <summary>
        /// Stops the server from listening; no further connections will be accepted
        /// </summary>
        public void Stop()
        {
            if (_socket != null)
            {
                try
                {
                    _socket.Shutdown(SocketShutdown.Both);
                }
                catch { /* nom nom */ }
                _socket.Dispose();
                _socket = null;
            }
        }

        public Socket ListeningSocket => _socket;

        private Socket GetReusableSocket() => null; // TODO: socket pooling / re-use

        private void BeginAccept(SocketAsyncEventArgs args)
        {
            // keep trying to take sync; break when it goes async
            args.AcceptSocket = GetReusableSocket();
            while (!Socket.AcceptAsync(args))
            {
                OnAccept(args);
                args.AcceptSocket = GetReusableSocket();
            }
        }
        /// <summary>
        /// Specifies a callback to be invoked whenever a connection is accepted
        /// </summary>
        public void OnConnection(Func<SocketConnection, Task> callback)
        {
            Callback = callback;
        }

        private static void OnAsyncCompleted(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                switch (e.LastOperation)
                {
                    case SocketAsyncOperation.Accept:
                        var obj = (SocketListener)e.UserToken;
                        obj.OnAccept(e);
                        obj.BeginAccept(e);
                        break;
                }
            }
            catch { }
        }

        private void OnAccept(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                var conn = new SocketConnection(e.AcceptSocket, PipelineFactory);
                e.AcceptSocket = null;
                ExecuteConnection(conn);
            }

            // note that we don't want to call BeginAccept at the end of OnAccept, as that
            // will cause a stack-dive in the sync (backlog) case
        }

        private async void ExecuteConnection(SocketConnection conn)
        {
            try
            {
                await Callback?.Invoke(conn);
            }
            catch
            {

            }
            finally
            {
                conn.Dispose();
            }
        }
    }
}
