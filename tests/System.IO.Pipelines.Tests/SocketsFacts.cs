using System.IO.Pipelines.Networking.Libuv;
using System.IO.Pipelines.Networking.Sockets;
using System.IO.Pipelines.Text.Primitives;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Text.Formatting;

namespace System.IO.Pipelines.Tests
{
    public class SocketsFacts : IDisposable
    {
        public void Dispose()
        {
            // am I leaking small buffers?
            Assert.Equal(0, SocketConnection.SmallBuffersInUse);
        }
        static readonly Span<byte> _ping = new Span<byte>(Encoding.ASCII.GetBytes("PING")), _pong = new Span<byte>(Encoding.ASCII.GetBytes("PING"));

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
            var endpoint = new IPEndPoint(IPAddress.Loopback, 5010);
            const string MessageToSend = "Hello world!";
            string reply = null;

            using (var server = new SocketListener())
            {
                server.OnConnection(Echo);
                server.Start(endpoint);


                using (var client = await SocketConnection.ConnectAsync(endpoint))
                {
                    var output = client.Output.Alloc();
                    output.Append(MessageToSend, EncodingData.InvariantUtf8);
                    await output.FlushAsync();
                    client.Output.Complete();

                    while (true)
                    {
                        var result = await client.Input.ReadAsync();
                        var input = result.Buffer;

                        // wait for the end of the data before processing anything
                        if (result.IsCompleted)
                        {
                            reply = input.GetUtf8String();
                            client.Input.Advance(input.End);
                            break;
                        }
                        else
                        {
                            client.Input.Advance(input.Start, input.End);
                        }
                    }
                }
            }
            Assert.Equal(MessageToSend, reply);
        }

        [Fact]
        public void CanCreateWorkingEchoServer_PipelineSocketServer_NonPipelineClient()
        {
            var endpoint = new IPEndPoint(IPAddress.Loopback, 5010);
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

        [Fact]
        public async Task RunStressPingPongTest_Libuv()
        {
            var endpoint = new IPEndPoint(IPAddress.Loopback, 5020);

            using (var thread = new UvThread())
            using (var server = new UvTcpListener(thread, endpoint))
            {
                server.OnConnection(PongServer);
                await server.StartAsync();

                const int SendCount = 500, ClientCount = 5;
                for (int loop = 0; loop < ClientCount; loop++)
                {
                    using (var client = await new UvTcpClient(thread, endpoint).ConnectAsync())
                    {
                        var tuple = await PingClient(client, SendCount);
                        Assert.Equal(SendCount, tuple.Item1);
                        Assert.Equal(SendCount, tuple.Item2);
                        Console.WriteLine($"Ping: {tuple.Item1}; Pong: {tuple.Item2}; Time: {tuple.Item3}ms");
                    }
                }
            }
        }


        [Fact]
        public async Task RunStressPingPongTest_Socket()
        {
            var endpoint = new IPEndPoint(IPAddress.Loopback, 5020);

            using (var server = new SocketListener())
            {
                server.OnConnection(PongServer);
                server.Start(endpoint);

                const int SendCount = 500, ClientCount = 5;
                for (int loop = 0; loop < ClientCount; loop++)
                {
                    using (var client = await SocketConnection.ConnectAsync(endpoint))
                    {
                        var tuple = await PingClient(client, SendCount);
                        Assert.Equal(SendCount, tuple.Item1);
                        Assert.Equal(SendCount, tuple.Item2);
                        Console.WriteLine($"Ping: {tuple.Item1}; Pong: {tuple.Item2}; Time: {tuple.Item3}ms");
                    }
                }
            }
        }

        static async Task<Tuple<int, int, int>> PingClient(IPipelineConnection connection, int messagesToSend)
        {
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
                        havePong = inputBuffer.Equals(_ping);
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

            return Tuple.Create(sendCount, replyCount, (int)watch.ElapsedMilliseconds);

        }

        private static async Task PongServer(IPipelineConnection connection)
        {
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
                    bool isPing = inputBuffer.Equals(_ping);
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

        private async Task Echo(IPipelineConnection connection)
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

                int len = request.Length;
                var response = connection.Output.Alloc();
                response.Append(request);
                await response.FlushAsync();
                connection.Input.Advance(request.End);
            }
        }
    }
}
