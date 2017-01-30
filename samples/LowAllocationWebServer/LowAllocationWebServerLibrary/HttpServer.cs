// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Net.Sockets;
using System;
using System.Buffers;
using System.Diagnostics;
using System.Text.Formatting;
using System.Text.Http.SingleSegment;
using System.Text.Utf8;
using System.Threading;
using System.Text.Http;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Sequences;

namespace Microsoft.Net.Http
{
    public abstract class HttpServer
    {
        readonly static ArrayPool<byte> s_pool = ArrayPool<byte>.Shared;

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

        public Task StartAsync()
        {
            return Task.Run(() => {
                this.Start();
            });
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
            
            var requestBuffer = s_pool.Rent(RequestBufferSize);
            var arrayMemory = new OwnedArray<byte>(requestBuffer);

            var requestByteCount = socket.Receive(requestBuffer);

            if(requestByteCount == 0) {
                socket.Close();
                return;
            }

            var requestMemory = arrayMemory.Memory.Slice(0, requestByteCount);
            var requestBytes = new ReadOnlyBytes(requestMemory);

            var request = HttpRequest.Parse(requestBytes);
            Log.LogRequest(request);

            using (var response = new HttpResponse(1024)) {
                WriteResponse(request, response);
                s_pool.Return(requestBuffer);

                Position position = Position.First;
                while(response.Headers.TryGet(ref position, out var headersSegment, true))
                {
                    socket.Send(headersSegment);
                }

                position = Position.First;
                while (response.Body.TryGet(ref position, out var bodySegment))
                {
                    socket.Send(bodySegment);
                }

                socket.Close();
            }

            if (Log.IsVerbose)
            {
                Log.LogMessage(Log.Level.Verbose, "Request Processed and Response Sent", DateTime.UtcNow.Ticks);
            }
        }

        protected virtual void WriteResponseFor400(Span<byte> requestBytes, HttpResponse response) // Bad Request
        {
            Log.LogMessage(Log.Level.Warning, "Request {0}, Response: 400 Bad Request", requestBytes.Length);
            WriteCommonHeaders(response, HttpVersion.V1_1, 400, "Bad Request", false);
            new ResponseFormatter(response.Headers).Append(HttpNewline);
        }

        // TODO: HttpRequest is a large struct. We cannot pass it around like that
        protected virtual void WriteResponseFor404(HttpRequest request, HttpResponse response) // Not Found
        {
            Log.LogMessage(Log.Level.Warning, "Request {0}, Response: 404 Not Found", request.Path.ToUtf8String(TextEncoder.Utf8));
            WriteCommonHeaders(response, HttpVersion.V1_1, 404, "Not Found", false);
            new ResponseFormatter(response.Headers).Append(HttpNewline);
        }

        // TODO: this is not a very general purpose routine. Maybe should not be in this base class?
        protected static void WriteCommonHeaders(
            HttpResponse response,
            HttpVersion version,
            int statuCode,
            string reasonCode,
            bool keepAlive)
        {
            var currentTime = DateTime.UtcNow;
            var formatter = new ResponseFormatter(response.Headers);
            formatter.AppendHttpStatusLine(version, statuCode, new Utf8String(reasonCode));
            formatter.Append(new Utf8String("Date : ")); formatter.Append(currentTime, 'R');
            formatter.AppendHttpNewLine();
            formatter.Append("Server : .NET Core Sample Server");
            formatter.AppendHttpNewLine();
            formatter.Append("Content-Type : text/html; charset=UTF-8");
            formatter.AppendHttpNewLine();

            if (!keepAlive)
            {
                formatter.Append("Connection : close");
            }
        }

        protected abstract void WriteResponse(HttpRequest request, HttpResponse response);
    }
}
