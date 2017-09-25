// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Net.Sockets;
using System;
using System.Buffers;
using System.Diagnostics;
using System.Text.Formatting;
using System.Text.Utf8;
using System.Text.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Buffers.Text;

namespace Microsoft.Net.Http
{
    public abstract class HttpServer
    {
        CancellationToken _cancellation;
        TcpServer _listener;

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

            using (OwnedBuffer rootBuffer = MemoryPool.Rent(RequestBufferSize)) {
                OwnedBuffer requestBuffer = rootBuffer;
                int totalWritten = 0;
                while (true) {
                    Span<byte> requestSpan = requestBuffer.AsSpan();

                    int requestBytesRead = socket.Receive(requestSpan);
                    if (requestBytesRead == 0) {
                        socket.Close();
                        return;
                    }

                    requestBuffer.Advance(requestBytesRead);
                    totalWritten += requestBytesRead;
                    if (requestBytesRead == requestSpan.Length) {
                        requestBuffer = requestBuffer.Enlarge(RequestBufferSize);
                    }
                    else {
                        break;
                    }
                }

                var requestBytes = new ReadOnlyBytes(rootBuffer, totalWritten);

                HttpRequest parsedRequest = HttpRequest.Parse(requestBytes);
                Log.LogRequest(parsedRequest);

                using (var response = new TcpConnectionFormatter(socket, ResponseBufferSize)) {
                    WriteResponse(parsedRequest, response);
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
            WriteCommonHeaders(ref response, HttpVersion.V1_1, 400, "Bad Request");
            response.AppendEoh();
        }

        // TODO: HttpRequest is a large struct. We cannot pass it around like that
        protected virtual void WriteResponseFor404(HttpRequest request, TcpConnectionFormatter response) // Not Found
        {
            Log.LogMessage(Log.Level.Warning, "Request {0}, Response: 404 Not Found", request.Path.ToUtf8String(SymbolTable.InvariantUtf8).ToString());
            WriteCommonHeaders(ref response, HttpVersion.V1_1, 404, "Not Found");
            response.AppendEoh();
        }

        // TODO: this is not a very general purpose routine. Maybe should not be in this base class?
        protected static void WriteCommonHeaders<TFormatter>(
            ref TFormatter formatter,
            HttpVersion version,
            int statuCode,
            string reasonCode)
            where TFormatter : ITextOutput
        {
            var currentTime = DateTime.UtcNow;
            formatter.AppendHttpStatusLine(version, statuCode, new Utf8String(reasonCode));
            formatter.Append("Transfer-Encoding : chunked\r\n");
            formatter.Append("Server : .NET Core Sample Server\r\n");
            formatter.Format("Date : {0:R}\r\n", DateTime.UtcNow);
        }

        protected abstract void WriteResponse(HttpRequest request, TcpConnectionFormatter response);
    }

    public abstract class RoutingServer<T> : HttpServer
    {
        protected static readonly ApiRoutingTable<T> Apis = new ApiRoutingTable<T>();

        public RoutingServer(CancellationToken cancellation, Log log, ushort port, byte address1, byte address2, byte address3, byte address4) : base(cancellation, log, port, address1, address2, address3, address4)
        {
        }

        protected override void WriteResponse(HttpRequest request, TcpConnectionFormatter response)
        {
            if (!Apis.TryHandle(request, response)) {
                WriteResponseFor404(request, response);
            }
        }
    }
}
