// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.IO.Pipelines;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading.Tasks;
using static System.Buffers.Text.Encodings;

namespace System.Net.Experimental
{
    public class SocketPipe : IDisposable, IDuplexPipe
    {
        readonly Pipe _requestPipe;
        readonly Pipe _responsePipe;
        readonly Socket _socket;
        readonly Stream _stream;

        // TODO (pri 3): would be nice to make this whole struct read-only
        Task _responseReader;
        Task _requestWriter;
        public TraceSource Log;

        SocketPipe(Socket socket, SslStream stream)
        {
            _socket = socket;
            _stream = stream;
            _requestPipe = new Pipe();
            _responsePipe = new Pipe();
            _responseReader = null;
            _requestWriter = null;
            Log = null;
        }

        public static async Task<SocketPipe> ConnectAsync(string host, int port, bool tls = false)
        {
            var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            // TODO (pri 3): all this TLS code is not tested
            // TODO (pri 3): would be great to get flat APIs for TLS
            SslStream tlsStream = null;
            if (tls)
            {
                var networkStream = new NetworkStream(socket);
                tlsStream = new SslStream(networkStream);
                await tlsStream.AuthenticateAsClientAsync(host).ConfigureAwait(false);
            }
            else
            {
                await socket.ConnectAsync(host, port).ConfigureAwait(false);
            }

            var client = new SocketPipe(socket, tlsStream);
            client._responseReader = client.ReceiveAsync();
            client._requestWriter = client.SendAsync();

            return client;
        }

        async Task SendAsync()
        {
            PipeReader reader = _requestPipe.Reader;
            try
            {
                while (true)
                {
                    ReadResult result = await reader.ReadAsync();
                    ReadOnlySequence<byte> buffer = result.Buffer;

                    try
                    {
                        if (!buffer.IsEmpty)
                        {
                            for (SequencePosition position = buffer.Start; buffer.TryGet(ref position, out ReadOnlyMemory<byte> segment);)
                            {
                                if (Log != null && Log.Switch.ShouldTrace(TraceEventType.Verbose))
                                {
                                    string data = Utf8.ToString(segment.Span);
                                    if (!string.IsNullOrWhiteSpace(data))
                                        Log.TraceInformation(data);
                                }

                                await WriteToSocketAsync(segment).ConfigureAwait(false);
                            }
                        }
                        else if (result.IsCompleted)
                        {
                            break;
                        }
                    }
                    finally
                    {
                        reader.AdvanceTo(buffer.End);
                    }
                }
            }
            catch (Exception e)
            {
                Log.TraceEvent(TraceEventType.Error, 0, e.ToString());
            }
            finally
            {
                reader.Complete();
            }
        }

        async Task ReceiveAsync()
        {
            PipeWriter writer = _responsePipe.Writer;
            try
            {
                while (true)
                {
                    // just wait for data in the socket
                    await ReadFromSocketAsync(Memory<byte>.Empty);

                    while (HasData)
                    {
                        Memory<byte> buffer = writer.GetMemory();
                        int readBytes = await ReadFromSocketAsync(buffer).ConfigureAwait(false);
                        if (readBytes == 0) break;

                        if (Log != null && Log.Switch.ShouldTrace(TraceEventType.Verbose))
                        {
                            string data = Utf8.ToString(buffer.Span.Slice(0, readBytes));
                            if (!string.IsNullOrWhiteSpace(data))
                                Log.TraceInformation(data);
                        }

                        writer.Advance(readBytes);
                        await writer.FlushAsync();
                    }
                }
            }
            finally
            {
                writer.Complete();
            }
        }

        async Task WriteToSocketAsync(ReadOnlyMemory<byte> buffer)
        {
            if (_stream != null)
            {
                await _stream.WriteAsync(buffer).ConfigureAwait(false);
                await _stream.FlushAsync().ConfigureAwait(false);
            }
            else
            {
                await _socket.SendAsync(buffer, SocketFlags.None).ConfigureAwait(false);
            }
        }

        async ValueTask<int> ReadFromSocketAsync(Memory<byte> buffer)
        {
            if (_stream != null)
            {
                return await _stream.ReadAsync(buffer).ConfigureAwait(false);
            }
            else
            {
                return await _socket.ReceiveAsync(buffer, SocketFlags.None).ConfigureAwait(false);
            }
        }

        bool HasData
        {
            get
            {
                if (_stream != null) return _stream.Length != 0;
                return _socket.Available != 0;
            }
        }

        public void Dispose()
        {
            _stream?.Dispose();
            _socket.Dispose();
        }

        public bool IsConnected => _socket != null;

        public PipeReader Input => _responsePipe.Reader;

        public PipeWriter Output => _requestPipe.Writer;
    }
}



