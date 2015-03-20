using Microsoft.Net.Http.Server.Socket;
using System.Diagnostics;
using System.IO;
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
        protected static Utf8String HttpNewline = new Utf8String(13, 10);

        protected volatile bool _isCancelled = false;
        TcpServer _listener;
        public Log Log { get; protected set; }
        
        protected HttpServer(Log log, ushort port, byte address1, byte address2, byte address3, byte address4)
        {
            Log = log;
            _listener = new TcpServer();
            _listener.Start(port, address1, address2, address3, address4);
        }

        public void StartAsync()
        {
            ThreadPool.QueueUserWorkItem((state) => {
                this.Start();
            }, null);
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
            
            var buffer = BufferPool.Shared.RentBuffer(4096);
            var received = socket.Receive(buffer);

            var receivedBytes = buffer.Slice(0, received);
            HttpServerBuffer responseBytes;

            // parse request
            HttpRequestLine requestLine;
            int requestLineBytes;
            if (!HttpRequestParser.TryParseRequestLine(receivedBytes, out requestLine, out requestLineBytes))
            {
                Log.LogError("invalid request line");
                responseBytes = CreateResponseFor400(receivedBytes);
            }
            else
            {
                var restOfRequestBytes = receivedBytes.Slice(requestLineBytes);

                if (Log.IsVerbose)
                {
                    Log.LogMessage(Log.Level.Verbose, "\tMethod:       {0}", requestLine.Method);
                    Log.LogMessage(Log.Level.Verbose, "\tRequest-URI:  {0}", requestLine.RequestUri.ToString());
                    Log.LogMessage(Log.Level.Verbose, "\tHTTP-Version: {0}", requestLine.Version);
                    LogRestOfRequest(restOfRequestBytes);
                }

                responseBytes = CreateResponse(requestLine, restOfRequestBytes);
            }
            BufferPool.Shared.ReturnBuffer(ref buffer);

            // send response
            var segment = responseBytes;        
            
            socket.Send(segment._buffer, segment._count);
            socket.Close();
            responseBytes.Return();
            if (Log.IsVerbose)
            {
                Log.LogMessage(Log.Level.Verbose, "Request Processed", DateTime.UtcNow.Ticks);
            }
        }

        void LogRestOfRequest(Span<byte> buffer)
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

        protected virtual HttpServerBuffer CreateResponseFor400(Span<byte> receivedBytes) // Bad Request
        {
            BufferFormatter formatter = new BufferFormatter(1024, FormattingData.InvariantUtf8);
            WriteCommonHeaders(formatter, @"HTTP/1.1 400 Bad Request");
            formatter.Append(HttpNewline);
            return new HttpServerBuffer(formatter.Buffer, formatter.CommitedByteCount, BufferPool.Shared);
        }

        protected virtual HttpServerBuffer CreateResponseFor404(HttpRequestLine requestLine, Span<byte> headersAndBody) // Not Found
        {
            Log.LogMessage(Log.Level.Warning, "Request {0}, Response: 404 Not Found", requestLine);

            BufferFormatter formatter = new BufferFormatter(1024, FormattingData.InvariantUtf8);
            WriteCommonHeaders(formatter, @"HTTP/1.1 404 Not Found");
            formatter.Append(HttpNewline);
            return new HttpServerBuffer(formatter.Buffer, formatter.CommitedByteCount, BufferPool.Shared);
        }

        protected static void WriteCommonHeaders(BufferFormatter formatter, string responseLine)
        {
            var currentTime = DateTime.UtcNow;
            formatter.Append(responseLine);
            formatter.Append(HttpNewline);
            formatter.Append("Date: ");
            formatter.Append(currentTime, Format.Symbol.R);
            formatter.Append(HttpNewline);
            formatter.Append("Server: .NET Core Sample Server");
            formatter.Append(HttpNewline);
            formatter.Append("Last-Modified: ");
            formatter.Append(currentTime, Format.Symbol.R);
            formatter.Append(HttpNewline);
            formatter.Append("Content-Type: text/html; charset=UTF-8");
            formatter.Append(HttpNewline);
            formatter.Append("Connection: close");
            formatter.Append(HttpNewline);
        }

        protected abstract HttpServerBuffer CreateResponse(HttpRequestLine requestLine, Span<byte> headersAndBody);
    }
}
