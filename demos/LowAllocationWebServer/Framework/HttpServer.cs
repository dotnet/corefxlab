﻿using Microsoft.Net.Sockets;
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

            // SharedData is the only per request allocation that I think needs to stay.
            // It allows for reliably returning buffers to the pool (as there are no copies of the object)
            // TODO: one thing I want to optimize here is allocation of the arrays for buffers.
            // The arrays should not be allocated for single header and single body and
            // if arrays are needed, they should be pooled.
            using (var responseData = new SharedData()) { 
                var response = new HttpResponse(responseData);
                WriteResponse(response, request);
                ArrayPool<byte>.Shared.Return(requestBuffer);

                // TODO: this whole thing about segment order is very bad. It needs to be designed.
                for (int index = 0; index < responseData.Count; index++) {
                    var segment = responseData[index];
                    if (segment.Item1 == 0) {
                        socket.Send(segment.Item2);
                    }
                }
                for (int index = 0; index < responseData.Count; index++) {
                    var segment = responseData[index];
                    if (segment.Item1 == 1) {
                        socket.Send(segment.Item2);
                    }
                }

                socket.Close();
            }

            if (Log.IsVerbose)
            {
                Log.LogMessage(Log.Level.Verbose, "Request Processed and Response Sent", DateTime.UtcNow.Ticks);
            }
        }

        protected virtual void WriteResponseFor400(HttpResponse response, Span<byte> receivedBytes) // Bad Request
        {
            Log.LogMessage(Log.Level.Warning, "Request {0}, Response: 400 Bad Request", receivedBytes.Length);
            WriteCommonHeaders(response, HttpVersion.V1_1, 400, "Bad Request", false);
            response.Headers.Append(HttpNewline);
        }

        protected virtual void WriteResponseFor404(HttpResponse response, HttpRequestLine requestLine) // Not Found
        {
            Log.LogMessage(Log.Level.Warning, "Request {0}, Response: 404 Not Found", requestLine);
            WriteCommonHeaders(response, HttpVersion.V1_1, 404, "Not Found", false);
            response.Headers.Append(HttpNewline);
        }

        protected static void WriteCommonHeaders(
            HttpResponse formatter,
            HttpVersion version,
            int statuCode,
            string reasonCode,
            bool keepAlive)
        {
            var currentTime = DateTime.UtcNow;
            formatter.Headers.AppendHttpStatusLine(version, statuCode, new Utf8String(reasonCode));
            formatter.Headers.Append("Date : "); formatter.Headers.Append(currentTime, 'R');
            formatter.Headers.AppendHttpNewLine();
            formatter.Headers.Append("Server : .NET Core Sample Serve");
            formatter.Headers.AppendHttpNewLine();
            formatter.Headers.Append("Content-Type : text/html; charset=UTF-8");
            formatter.Headers.AppendHttpNewLine();

            if (!keepAlive)
            {
                formatter.Headers.Append("Connection : close");
            }
        }

        protected abstract void WriteResponse(HttpResponse response, HttpRequest request);
    }
}
