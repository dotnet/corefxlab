using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.IO.Pipelines;
using System.Net.Security;
using System.Net.Sockets;
using System.Text.Http.Parser;
using System.Threading.Tasks;

// SocketClient is an experimental low-allocating/low-copy HTTP client API
// TODO (pri 2): need to support cancellations
namespace System.Net.Experimental
{
    public interface IResponse : IHttpHeadersHandler, IHttpResponseLineHandler {
        void OnBody(PipeReader body);
    }

    public abstract class RequestWriter<T> where T : IPipeWritable
    {
        public abstract Text.Http.Parser.Http.Method Verb { get; }

        public async Task WriteAsync(PipeWriter writer, T request)
        {
            WriteRequestLineAndHeaders(writer, ref request);
            await WriteBody(writer, request).ConfigureAwait(false);
            await writer.FlushAsync();
        }

        // TODO (pri 2): writing the request line should not be abstract; writing headers should.
        protected abstract void WriteRequestLineAndHeaders(PipeWriter writer, ref T request);
        protected virtual Task WriteBody(PipeWriter writer, T request) { return Task.CompletedTask; }
    }

    public struct SocketClient : IDisposable
    {
        readonly Pipe _requestPipe;
        readonly Pipe _responsePipe;
        readonly Socket _socket;
        readonly Stream _stream;

        // TODO (pri 3): would be nice to make this whole struct read-only
        Task _responseReader;
        Task _requestWriter;
        public TraceSource Log;

        SocketClient(Socket socket, SslStream stream)
        {
            _socket = socket;
            _stream = stream;
            _requestPipe = new Pipe();
            _responsePipe = new Pipe();
            _responseReader = null;
            _requestWriter = null;
            Log = null;
        }

        public static async Task<SocketClient> ConnectAsync(string host, int port, bool tls = false)
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

            var client = new SocketClient(socket, tlsStream);
            client._responseReader = client.ReceiveAsync();
            client._requestWriter = client.SendAsync();

            return client;
        }

        public async ValueTask<TResponse> SendRequest<TRequest, TResponse>(TRequest request)
            where TRequest : IPipeWritable
            where TResponse : IResponse, new()
        {
            await request.WriteAsync(_requestPipe.Writer).ConfigureAwait(false);

            var reader = _responsePipe.Reader;
            var response = await HttpExtensions.ParseAsync<TResponse>(reader, Log).ConfigureAwait(false);
            response.OnBody(reader);
            return response;
        }

        async Task SendAsync()
        {
            var reader = _requestPipe.Reader;
            try
            {
                while (true)
                {
                    var result = await reader.ReadAsync();
                    var buffer = result.Buffer;

                    try
                    {
                        if (!buffer.IsEmpty)
                        {
                            for (var position = buffer.Start; buffer.TryGet(ref position, out ReadOnlyMemory<byte> segment);)
                            {
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
            catch(Exception e)
            {
                Log.WriteError(e.ToString());
            }
            finally
            {
                reader.Complete();
            }
        }
        
        async Task ReceiveAsync()
        {
            var writer = _responsePipe.Writer;
            try
            {
                while (true)
                {
                    // just wait for data in the socket
                    await ReadFromSocketAsync(Memory<byte>.Empty);

                    while (HasData)
                    {
                        var buffer = writer.GetMemory();
                        var readBytes = await ReadFromSocketAsync(buffer).ConfigureAwait(false);
                        if (readBytes == 0) break;

                        if (Log != null) Log.WriteInformation($"RESPONSE {readBytes}", buffer.Slice(0, readBytes));

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
            get {
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
    }
}



