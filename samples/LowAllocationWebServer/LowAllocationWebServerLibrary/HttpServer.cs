// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Net.Sockets;
using System;
using System.Buffers;
using System.Diagnostics;
using System.Text.Formatting;
using System.Text.Http.Formatter;
using System.Text.Utf8;
using System.Threading.Tasks;
using System.Threading;
using System.Text.Http.Parser;

namespace Microsoft.Net
{
    public abstract class HttpServer
    {
        CancellationToken _cancellation;
        TcpServer _listener;
        HttpParser s_parser = new HttpParser();

        public Log Log { get; protected set; }

        public int RequestBufferSize = 1024;
        public int ResponseBufferSize = 1024;

        protected HttpServer(CancellationToken cancellation, Log log, ushort port, byte address1, byte address2, byte address3, byte address4)
        {
            _cancellation = cancellation;
            Log = log;
            _listener = new TcpServer(port, address1, address2, address3, address4);
        }

        public Task StartAsync()
        {
            return Task.Run(() => {
                Start();
            });
        }

        void Start()
        {
            try {
                while (!_cancellation.IsCancellationRequested) {
                    TcpConnection socket = _listener.Accept();
                    ProcessRequest(socket);
                }
                _listener.Stop();
            }
            catch (Exception e) {
                Log.LogError(e.Message);
                Log.LogVerbose(e.ToString());
            }
        }

        protected virtual void ProcessRequest(TcpConnection socket)
        {
            Log.LogVerbose("Processing Request");

            using (BufferSequence rootBuffer = new BufferSequence(RequestBufferSize)) {
                BufferSequence requestBuffer = rootBuffer;
                int totalWritten = 0;
                while (true) {
                    Span<byte> requestSpan = requestBuffer.Free;

                    int requestBytesRead = socket.Receive(requestSpan);
                    if (requestBytesRead == 0) {
                        socket.Close();
                        return;
                    }

                    requestBuffer.Advance(requestBytesRead);
                    totalWritten += requestBytesRead;
                    if (requestBytesRead == requestSpan.Length) {
                        requestBuffer = requestBuffer.Append(RequestBufferSize);
                    }
                    else {
                        break;
                    }
                }

                var requestBytes = new ReadOnlySequence<byte>(rootBuffer, 0, requestBuffer, requestBuffer.Memory.Length);

                var request = new HttpRequest();
                if(!s_parser.ParseRequestLine(request, requestBytes, out int consumed))
                {
                    throw new Exception();
                }
                requestBytes = requestBytes.Slice(consumed);
                if (!s_parser.ParseHeaders(request, requestBytes, out consumed))
                {
                    throw new Exception();
                }

                var requestBody = requestBytes.Slice(consumed);

                Log.LogRequest(request, requestBody);

                using (var response = new TcpConnectionFormatter(socket, ResponseBufferSize)) {
                    WriteResponse(ref request, requestBody, response);
                }

                socket.Close();
            }

            if (Log.IsVerbose) {
                Log.LogMessage(Log.Level.Verbose, "Request Processed and Response Sent", DateTime.UtcNow.Ticks);
            }
        }

        protected virtual void WriteResponseFor400(Span<byte> requestBytes, TcpConnectionFormatter response) // Bad Request
        {
            Log.LogMessage(Log.Level.Warning, "Request {0}, Response: 400 Bad Request", requestBytes.Length);
            WriteCommonHeaders(ref response, Http.Version.Http11, 400, "Bad Request");
            response.AppendEoh();
        }

        protected virtual void WriteResponseFor404(ref HttpRequest request, TcpConnectionFormatter response) // Not Found
        {
            Log.LogMessage(Log.Level.Warning, "Request {0}, Response: 404 Not Found", request.Path);
            WriteCommonHeaders(ref response, Http.Version.Http11, 404, "Not Found");
            response.AppendEoh();
        }

        protected static void WriteCommonHeaders<TFormatter>(
            ref TFormatter formatter,
            Http.Version version,
            int statuCode,
            string reasonCode)
            where TFormatter : ITextBufferWriter
        {
            var currentTime = DateTime.UtcNow;
            formatter.AppendHttpStatusLine(version, statuCode, new Utf8Span(reasonCode));
            formatter.Append("Transfer-Encoding : chunked\r\n");
            formatter.Append("Server : .NET Core Sample Server\r\n");
            formatter.Format("Date : {0:R}\r\n", DateTime.UtcNow);
        }

        protected abstract void WriteResponse(ref HttpRequest request, ReadOnlySequence<byte> body, TcpConnectionFormatter response);
    }

    public abstract class RoutingServer<T> : HttpServer
    {
        protected static readonly ApiRoutingTable<T> Apis = new ApiRoutingTable<T>();

        public RoutingServer(CancellationToken cancellation, Log log, ushort port, byte address1, byte address2, byte address3, byte address4) : base(cancellation, log, port, address1, address2, address3, address4)
        {
        }

        protected override void WriteResponse(ref HttpRequest request, ReadOnlySequence<byte> body, TcpConnectionFormatter response)
        {
            if (!Apis.TryHandle(request, body, response)) {
                WriteResponseFor404(ref request, response);
            }
        }
    }
}
