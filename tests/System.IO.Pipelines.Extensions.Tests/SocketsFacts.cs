// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

#if (Windows || OSX) // "The test hangs on linux"
        [Fact]
#endif
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
                        var output = client.Output.Alloc();
                        output.AsOutput().Append(MessageToSend, SymbolTable.InvariantUtf8);
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
                                client.Input.Advance(input.End);
                                break;
                            }
                            else
                            {
                                client.Input.Advance(input.Start, input.End);
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

#if (Windows || OSX) // "The test hangs on linux"
        [Fact]
#endif
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

        // Issue #1687 - The test intermittently fails on linux -
        // Reason: Unhandled Exception: System.IO.Pipelines.Networking.Libuv.Interop.UvException: Error -16 EBUSY resource busy or locked
#if (Windows || OSX)
        [Fact]
#endif
        public async Task RunStressPingPongTest_Libuv()
        {
            var endpoint = new IPEndPoint(IPAddress.Loopback, 5040);

            using (var thread = new UvThread())
            using (var server = new UvTcpListener(thread, endpoint))
            {
                server.OnConnection(PongServer);
                await server.StartAsync();

                const int SendCount = 500, ClientCount = 5;
                for (int loop = 0; loop < ClientCount; loop++)
                {
                    using (var connection = await new UvTcpClient(thread, endpoint).ConnectAsync())
                    {
                        try
                        {
                            var tuple = await PingClient(connection, SendCount);
                            Assert.Equal(SendCount, tuple.Item1);
                            Assert.Equal(SendCount, tuple.Item2);
                            Console.WriteLine($"Ping: {tuple.Item1}; Pong: {tuple.Item2}; Time: {tuple.Item3}ms");
                        }
                        finally
                        {
                            await connection.DisposeAsync();
                        }
                    }
                }
            }
        }


        [Fact]
        public async Task RunStressPingPongTest_Socket()
        {
            var endpoint = new IPEndPoint(IPAddress.Loopback, 5050);

            using (var server = new SocketListener())
            {
                server.OnConnection(PongServer);
                server.Start(endpoint);

                const int SendCount = 500, ClientCount = 5;
                for (int loop = 0; loop < ClientCount; loop++)
                {
                    using (var connection = await SocketConnection.ConnectAsync(endpoint))
                    {
                        try
                        {
                            var tuple = await PingClient(connection, SendCount);
                            Assert.Equal(SendCount, tuple.Item1);
                            Assert.Equal(SendCount, tuple.Item2);
                            Console.WriteLine($"Ping: {tuple.Item1}; Pong: {tuple.Item2}; Time: {tuple.Item3}ms");
                        }
                        finally
                        {
                            await connection.DisposeAsync();
                        }
                    }
                }
            }
        }

        static async Task<Tuple<int, int, int>> PingClient(IPipeConnection connection, int messagesToSend)
        {
            ArraySegment<byte> _ping = new ArraySegment<byte>(Encoding.ASCII.GetBytes("PING"));
            ArraySegment<byte> _pong = new ArraySegment<byte>(Encoding.ASCII.GetBytes("PING"));

            int count = 0;
            var watch = Stopwatch.StartNew();
            int sendCount = 0, replyCount = 0;
            for (int i = 0; i < messagesToSend; i++)
            {
                await connection.Output.WriteAsync(_ping);
                sendCount++;

                bool havePong = false;
                while (true)
                {
                    var result = await connection.Input.ReadAsync();
                    var inputBuffer = result.Buffer;

                    if (inputBuffer.IsEmpty && result.IsCompleted)
                    {
                        connection.Input.Advance(inputBuffer.End);
                        break;
                    }
                    if (inputBuffer.Length < 4)
                    {
                        connection.Input.Advance(inputBuffer.Start, inputBuffer.End);
                    }
                    else
                    {
                        havePong = inputBuffer.EqualsTo(_ping);
                        if (havePong)
                        {
                            count++;
                        }
                        connection.Input.Advance(inputBuffer.End);
                        break;
                    }
                }

                if (havePong)
                {
                    replyCount++;
                }
                else
                {
                    break;
                }
            }
            connection.Input.Complete();
            connection.Output.Complete();
            watch.Stop();

            // Task.Run here so that we're not on the UV thread when we complete
            return await Task.Run(() => Tuple.Create(sendCount, replyCount, (int)watch.ElapsedMilliseconds));

        }

        private static async Task PongServer(IPipeConnection connection)
        {
            ArraySegment<byte> _ping = new ArraySegment<byte>(Encoding.ASCII.GetBytes("PING"));
            ArraySegment<byte> _pong = new ArraySegment<byte>(Encoding.ASCII.GetBytes("PING"));

            while (true)
            {
                var result = await connection.Input.ReadAsync();
                var inputBuffer = result.Buffer;

                if (inputBuffer.IsEmpty && result.IsCompleted)
                {
                    connection.Input.Advance(inputBuffer.End);
                    break;
                }

                if (inputBuffer.Length < 4)
                {
                    connection.Input.Advance(inputBuffer.Start, inputBuffer.End);
                }
                else
                {
                    bool isPing = inputBuffer.EqualsTo(_ping);
                    if (isPing)
                    {
                        await connection.Output.WriteAsync(_pong);
                    }
                    else
                    {
                        break;
                    }

                    connection.Input.Advance(inputBuffer.End);
                }
            }
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

        private async Task Echo(IPipeConnection connection)
        {
            while (true)
            {
                var result = await connection.Input.ReadAsync();
                var request = result.Buffer;

                if (request.IsEmpty && result.IsCompleted)
                {
                    connection.Input.Advance(request.End);
                    break;
                }

                var response = connection.Output.Alloc();
                foreach (var memory in request)
                {
                    response.Write(memory.Span);
                }
                await response.FlushAsync();
                connection.Input.Advance(request.End);
            }
        }
    }
}
