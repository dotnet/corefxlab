using Microsoft.Net.Http.Server.Socket;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Formatting;
using System.Text.Utf8;
using System.Threading;

namespace System.Net.Http.Buffered
{
    // this is a small holder for a buffer and the pool that allocated it
    // it's used to pass buffers around that can be deallocated into the same pool they were taken from
    // if the pool was static, we would not need this type. But I am not sure if a static pool is good enough.
    public struct HttpServerBuffer
    {
        public byte[] _buffer;
        public int _count;
        BufferPool _pool;

        public HttpServerBuffer(byte[] buffer, int count, BufferPool pool = null)
        {
            _buffer = buffer;
            _count = count;
            _pool = pool;
        }

        public void Return()
        {
            if (_pool != null)
            {
                _pool.ReturnBuffer(ref _buffer);
                _count = 0;
            }
        }
    }

    public abstract class HttpServer
    {
        protected static Utf8String HttpNewline = new Utf8String(new byte[] { 13, 10 });

        protected volatile bool _isCancelled = false;
        TcpServer _listener;
        public Log Log { get; protected set; }

        const int RequestBufferSize = 2048;
        public NativeBufferPool _buffers = new NativeBufferPool(RequestBufferSize, numberOfBuffers:1000);

        protected HttpServer(Log log, ushort port, byte address1, byte address2, byte address3, byte address4)
        {
            Log = log;
            _listener = new TcpServer(port, address1, address2, address3, address4);
        }

        public void StartAsync()
        {
            Thread thread = new Thread(new ParameterizedThreadStart((parameter) => {
                var httpServer = parameter as HttpServer;
                httpServer.Start();
            }));
            thread.Start(this);
        }

        public void Stop()
        {
            _isCancelled = true;
            Log.LogMessage(Log.Level.Verbose, "Buffers Allocated: " + BufferPool.Shared.Allocations);
            Log.LogVerbose("Server Terminated");
        }

        void Start()
        {
            try
            {
                while (!_isCancelled)
                {
                    TcpConnection socket = _listener.Accept();
                    ProcessRequest(socket);
                }
                _listener.Stop();
            }
            catch (Exception e)
            {
                Log.LogError(e.Message);
                Log.LogVerbose(e.ToString());
                Stop();
            }
        }

        protected virtual void ProcessRequest(TcpConnection socket)
        {
            Log.LogVerbose("Processing Request");
            
            var buffer = _buffers.Rent();
            var received = socket.Receive(buffer);

            if(received == 0)
            {
                socket.Close();
                return;
            }

            var receivedBytes = buffer.Slice(0, received);

            if (Log.IsVerbose)
            {
                var text = Encoding.UTF8.GetString(receivedBytes.CreateArray());
                Console.WriteLine(text);
            }

            var request = HttpRequest.Parse(receivedBytes);

            if (Log.IsVerbose)
            {
                Log.LogMessage(Log.Level.Verbose, "\tMethod:       {0}", request.RequestLine.Method);
                Log.LogMessage(Log.Level.Verbose, "\tRequest-URI:  {0}", request.RequestLine.RequestUri.ToString());
                Log.LogMessage(Log.Level.Verbose, "\tHTTP-Version: {0}", request.RequestLine.Version);

                Log.LogMessage(Log.Level.Verbose, "\tHttp Headers:");
                foreach (var httpHeader in request.Headers)
                {
                    Log.LogMessage(Log.Level.Verbose, "\t\tName: {0}, Value: {1}", httpHeader.Key, httpHeader.Value);
                }

                LogRestOfRequest(request.Body);
            }

            HttpServerBuffer responseBytes = CreateResponse(request);
                     
            _buffers.Return(buffer);

            // send response
            var segment = responseBytes;        
            
            socket.Send(segment._buffer, segment._count);

            if (!request.RequestLine.IsKeepAlive())
            {
                socket.Close();
            }

            responseBytes.Return();
            if (Log.IsVerbose)
            {
                Log.LogMessage(Log.Level.Verbose, "Request Processed", DateTime.UtcNow.Ticks);
            }
        }

        void LogRestOfRequest(ByteSpan buffer)
        {
            HttpRequestReader reader = new HttpRequestReader();
            reader.Buffer = buffer;
            while (true)
            {
                var header = reader.ReadHeader();
                if (header.Length == 0) break;
                Log.LogMessage(Log.Level.Verbose, "\tHeader: {0}", header.ToString());
            }
            var messageBody = reader.Buffer;
            Log.LogMessage(Log.Level.Verbose, "\tBody bytecount: {0}", messageBody.Length);
        }

        protected virtual HttpServerBuffer CreateResponseFor400(ByteSpan receivedBytes) // Bad Request
        {
            BufferFormatter formatter = new BufferFormatter(1024, FormattingData.InvariantUtf8);
            WriteCommonHeaders(formatter, @"HTTP/1.1 400 Bad Request", false);
            formatter.Append(HttpNewline);
            return new HttpServerBuffer(formatter.Buffer, formatter.CommitedByteCount, BufferPool.Shared);
        }

        protected virtual HttpServerBuffer CreateResponseFor404(HttpRequestLine requestLine) // Not Found
        {
            Log.LogMessage(Log.Level.Warning, "Request {0}, Response: 404 Not Found", requestLine);

            BufferFormatter formatter = new BufferFormatter(1024, FormattingData.InvariantUtf8);
            WriteCommonHeaders(formatter, @"HTTP/1.1 404 Not Found", false);
            formatter.Append(HttpNewline);
            return new HttpServerBuffer(formatter.Buffer, formatter.CommitedByteCount, BufferPool.Shared);
        }

        protected static void WriteCommonHeaders(BufferFormatter formatter, string responseLine, bool keepAlive)
        {
            var currentTime = DateTime.UtcNow;
            formatter.Append(responseLine);
            formatter.Append(HttpNewline);
            formatter.Append("Date: ");
            formatter.Append(currentTime, 'R');
            formatter.Append(HttpNewline);
            formatter.Append("Server: .NET Core Sample Server");
            formatter.Append(HttpNewline);
            formatter.Append("Last-Modified: ");
            formatter.Append(currentTime, 'R');
            formatter.Append(HttpNewline);
            formatter.Append("Content-Type: text/html; charset=UTF-8");
            formatter.Append(HttpNewline);
            if (!keepAlive)
            {
                formatter.Append("Connection: close");
                formatter.Append(HttpNewline);
            }
        }

        protected abstract HttpServerBuffer CreateResponse(HttpRequest request);
    }

    static class FormatterExtensions
    {
        public static void Append<T>(this T formatter, Utf8String text) where T : IFormatter
        {
            var bytes = new byte[text.Length];
            int i = 0;
            foreach (var codeUnit in text)
            {
                bytes[i++] = codeUnit.Value;
            }
            int avaliable;
            do
            {
                avaliable = formatter.FreeBuffer.Length;
                formatter.ResizeBuffer();
            }
            while (avaliable < bytes.Length);

            formatter.FreeBuffer.Set(bytes);
            formatter.CommitBytes(bytes.Length);
        }
    }
}
