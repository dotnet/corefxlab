using Microsoft.Net.Sockets;
using System;
using System.Buffers;
using System.Diagnostics;
using System.Text.Formatting;
using System.Text.Http;
using System.Text.Utf8;
using System.Threading;

namespace Microsoft.Net.Http
{
    public abstract class HttpServer
    {
        protected static Utf8String HttpNewline = new Utf8String(new byte[] { 13, 10 });

        protected volatile bool _isCancelled = false;
        TcpServer _listener;
        public Log Log { get; protected set; }

        const int RequestBufferSize = 2048;

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
            
            var requestBuffer = ArrayPool<byte>.Shared.Rent(RequestBufferSize);
            var requestByteCount = socket.Receive(requestBuffer);

            if(requestByteCount == 0) {
                socket.Close();
                return;
            }

            var requestBytes = requestBuffer.Slice(0, requestByteCount);
            var request = HttpRequest.Parse(requestBytes);
            Log.LogRequest(request);

            var formatter = new BufferFormatter(1024, FormattingData.InvariantUtf8);
            WriteResponse(formatter, request);

            ArrayPool<byte>.Shared.Return(requestBuffer);

            var response = formatter.Buffer.Slice(0, formatter.CommitedByteCount);

            Console.WriteLine("Response:");
            Console.WriteLine(new Utf8String(response));

            socket.Send(response);
            socket.Close();

            if (Log.IsVerbose)
            {
                Log.LogMessage(Log.Level.Verbose, "Request Processed and Response Sent", DateTime.UtcNow.Ticks);
            }
        }

        protected virtual void WriteResponseFor400(BufferFormatter formatter, Span<byte> receivedBytes) // Bad Request
        {
            Log.LogMessage(Log.Level.Warning, "Request {0}, Response: 400 Bad Request", receivedBytes.Length);
            WriteCommonHeaders(formatter, "1.1", "400", "Bad Request", false);
            formatter.Append(HttpNewline);
        }

        protected virtual void WriteResponseFor404(BufferFormatter formatter, HttpRequestLine requestLine) // Not Found
        {
            Log.LogMessage(Log.Level.Warning, "Request {0}, Response: 404 Not Found", requestLine);
            WriteCommonHeaders(formatter, "1.1", "404", "Not Found", false);
            formatter.Append(HttpNewline);
        }

        // TODO: this should not be here. Also, this should not allocate
        protected static void WriteCommonHeaders(
            BufferFormatter formatter,
            string version,
            string statuCode,
            string reasonCode,
            bool keepAlive)
        {
            var currentTime = DateTime.UtcNow;
            formatter.WriteHttpStatusLine(
                new Utf8String(version), 
                new Utf8String(statuCode), 
                new Utf8String(reasonCode));
            formatter.WriteHttpHeader(new Utf8String("Date"), new Utf8String(currentTime.ToString("R")));
            formatter.WriteHttpHeader(new Utf8String("Server"), new Utf8String(".NET Core Sample Serve"));
            formatter.WriteHttpHeader(new Utf8String("Last-Modified"), new Utf8String(currentTime.ToString("R")));
            formatter.WriteHttpHeader(new Utf8String("Content-Type"), new Utf8String("text/html; charset=UTF-8"));
            
            if (!keepAlive)
            {
                formatter.WriteHttpHeader(new Utf8String("Connection"), new Utf8String("close"));
            }
        }

        protected abstract void WriteResponse(BufferFormatter formatter, HttpRequest request);
    }
}
