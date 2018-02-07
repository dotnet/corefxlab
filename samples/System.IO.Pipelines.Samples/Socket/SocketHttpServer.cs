// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace System.IO.Pipelines.Samples.Http
{
    public class SocketHttpServer : HttpServerBase
    {
        private Socket _listenSocket;

        public SocketHttpServer()
        {
            Features.Set<IServerAddressesFeature>(new ServerAddressesFeature());
        }

        protected override async void StartAccepting<TContext>(IHttpApplication<TContext> application, IPAddress ip, int port)
        {
            Thread.CurrentThread.Name = "Socket Accept Thread";
            _listenSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _listenSocket.Bind(new IPEndPoint(ip, port));
            _listenSocket.Listen(10);
            while (true)
            {
                try
                {
                    var clientSocket = await _listenSocket.AcceptAsync();
                    clientSocket.NoDelay = true;
                    var task = ProcessConnection(application, MemoryPool<byte>.Shared, clientSocket);
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (Exception)
                {
                    /* Ignored */
                }
            }
        }

        public override void Dispose()
        {
            _listenSocket?.Dispose();
            _listenSocket = null;
        }

        private static async Task ProcessConnection<TContext>(IHttpApplication<TContext> application, MemoryPool<byte> memoryPool, Socket socket)
        {
            using (var ns = new NetworkStream(socket))
            {
                using (var connection = new StreamDuplexPipe(new PipeOptions(memoryPool), ns))
                {
                    await ProcessClient(application, connection);
                }
            }
        }

        private static async Task ProcessClient<TContext>(IHttpApplication<TContext> application, IDuplexPipe duplexPipe)
        {
            var connection = new HttpConnection<TContext>(application, duplexPipe.Input, duplexPipe.Output);

            await connection.ProcessAllRequests();
        }
    }
}
