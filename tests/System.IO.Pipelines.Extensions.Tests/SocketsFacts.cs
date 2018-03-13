// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.IO.Pipelines.Networking.Libuv;
using System.IO.Pipelines.Networking.Sockets;
using System.IO.Pipelines.Text.Primitives;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Text.Formatting;
using System.Buffers.Text;

namespace System.IO.Pipelines.Tests
{
    public class SocketsFacts : IDisposable
    {
        public void Dispose()
        {
            // am I leaking small buffers?
            Assert.Equal(0, SocketConnection.SmallBuffersInUse);
        }

        [Fact]
        public async Task CanCreateWorkingEchoServer_PipelineLibuvServer_NonPipelineClient()
        {
            var endpoint = new IPEndPoint(IPAddress.Loopback, 5010);
            const string MessageToSend = "Hello world!";
            string reply = null;

            using (var thread = new UvThread())
            using (var server = new UvTcpListener(thread, endpoint))
            {
                server.OnConnection(Echo);
                await server.StartAsync();

                reply = SendBasicSocketMessage(endpoint, MessageToSend);
            }
            Assert.Equal(MessageToSend, reply);
        }

        [Fact]
        public async Task CanCreateWorkingEchoServer_PipelineSocketServer_PipelineSocketClient()
        {
            var endpoint = new IPEndPoint(IPAddress.Loopback, 5020);
            const string MessageToSend = "Hello world!";
            string reply = null;

            using (var server = new SocketListener())
            {
                server.OnConnection(Echo);
                server.Start(endpoint);


                using (var client = await SocketConnection.ConnectAsync(endpoint))
                {
                    try
                    {
                        var output = client.Output;
                        output.Append(MessageToSend, SymbolTable.InvariantUtf8);
                        await output.FlushAsync();
                        client.Output.Complete();

                        while (true)
                        {
                            var result = await client.Input.ReadAsync();

                            var input = result.Buffer;

                            // wait for the end of the data before processing anything
                            if (result.IsCompleted)
                            {
                                reply = input.GetUtf8Span();
                                client.Input.AdvanceTo(input.End);
                                break;
                            }
                            else
                            {
                                client.Input.AdvanceTo(input.Start, input.End);
                            }
                        }
                    }
                    finally
                    {
                        await client.DisposeAsync();
                    }
                }
            }
            Assert.Equal(MessageToSend, reply);
        }

        [Fact]
        public void CanCreateWorkingEchoServer_PipelineSocketServer_NonPipelineClient()
        {
            var endpoint = new IPEndPoint(IPAddress.Loopback, 5030);
            const string MessageToSend = "Hello world!";
            string reply = null;

            using (var server = new SocketListener())
            {
                server.OnConnection(Echo);
                server.Start(endpoint);

                reply = SendBasicSocketMessage(endpoint, MessageToSend);
            }
            Assert.Equal(MessageToSend, reply);
        }

        private static string SendBasicSocketMessage(IPEndPoint endpoint, string message)
        {
            // create the client the old way
            using (var socket = new Socket(SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Connect(endpoint);
                var data = Encoding.UTF8.GetBytes(message);
                socket.Send(data);
                socket.Shutdown(SocketShutdown.Send);

                byte[] buffer = new byte[data.Length];
                int offset = 0, bytesReceived;
                while (offset <= buffer.Length
                    && (bytesReceived = socket.Receive(buffer, offset, buffer.Length - offset, SocketFlags.None)) > 0)
                {
                    offset += bytesReceived;
                }
                socket.Shutdown(SocketShutdown.Receive);
                return Encoding.UTF8.GetString(buffer, 0, offset);
            }
        }

        private async Task Echo(IDuplexPipe connection)
        {
            while (true)
            {
                var result = await connection.Input.ReadAsync();
                var request = result.Buffer;

                if (request.IsEmpty && result.IsCompleted)
                {
                    connection.Input.AdvanceTo(request.End);
                    break;
                }

                var response = connection.Output;
                foreach (var memory in request)
                {
                    response.Write(memory.Span);
                }
                await response.FlushAsync();
                connection.Input.AdvanceTo(request.End);
            }
        }
    }
}
