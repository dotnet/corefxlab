// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Net;
using System.Threading;
using System.IO.Pipelines.Networking.Libuv;
using System.Text;

namespace System.IO.Pipelines.Samples
{
    public class RawLibuvHttpServerSample
    {
        public static void Run()
        {
            var ip = IPAddress.Any;
            int port = 5000;
            var thread = new UvThread();
            var listener = new UvTcpListener(thread, new IPEndPoint(ip, port));

            var outputString = "HTTP/1.1 200 OK" +
                               "\r\nContent-Length: 13" +
                               "\r\nContent-Type: text/plain" +
                               "\r\n\r\n" +
                               "Hello, World!";

            var data = Encoding.UTF8.GetBytes(outputString);

            listener.OnConnection(async connection =>
            {
                var frame = new Http11Frame();

                while (true)
                {
                    // Wait for data
                    var result = await connection.Input.ReadAsync();
                    var input = result.Buffer;
                    var consumed = input.Start;
                    var examined = input.Start;

                    try
                    {
                        if (input.IsEmpty && result.IsCompleted)
                        {
                            // No more data
                            break;
                        }

                        if (!frame.ParseRequest(input, out consumed, out examined))
                        {
                            if (result.IsCompleted)
                            {
                                // Didn't get the whole request and the connection ended
                                throw new EndOfStreamException();
                            }

                            continue;
                        }

                        var output = connection.Output.Alloc();
                        output.WriteFast(data);
                        await output.FlushAsync();

                        frame.Reset();
                    }
                    finally
                    {
                        // Consume the input
                        connection.Input.Advance(consumed, examined);
                    }
                }
            });

            listener.StartAsync().GetAwaiter().GetResult();

            Console.WriteLine($"Listening on {ip} on port {port}");
            var wh = new ManualResetEventSlim();
            Console.CancelKeyPress += (sender, e) =>
            {
                wh.Set();
            };

            wh.Wait();

            listener.Dispose();
            thread.Dispose();
        }

        private class Http11Frame : IHttpRequestLineHandler, IHttpHeadersHandler
        {
            private KestrelHttpParser _httpParser = new KestrelHttpParser();
            private RequestProcessingStatus _requestProcessingStatus;

            public bool ParseRequest(ReadableBuffer buffer, out ReadCursor consumed, out ReadCursor examined)
            {
                consumed = buffer.Start;
                examined = buffer.End;

                switch (_requestProcessingStatus)
                {
                    case RequestProcessingStatus.RequestPending:
                        if (buffer.IsEmpty)
                        {
                            break;
                        }

                        _requestProcessingStatus = RequestProcessingStatus.ParsingRequestLine;
                        goto case RequestProcessingStatus.ParsingRequestLine;
                    case RequestProcessingStatus.ParsingRequestLine:
                        if (_httpParser.ParseRequestLine(this, buffer, out consumed, out examined))
                        {
                            buffer = buffer.Slice(consumed, buffer.End);

                            _requestProcessingStatus = RequestProcessingStatus.ParsingHeaders;
                            goto case RequestProcessingStatus.ParsingHeaders;
                        }
                        else
                        {
                            break;
                        }
                    case RequestProcessingStatus.ParsingHeaders:
                        if (_httpParser.ParseHeaders(this, buffer, out consumed, out examined, out int consumedBytes))
                        {
                            _requestProcessingStatus = RequestProcessingStatus.AppStarted;
                        }
                        break;
                }

                return _requestProcessingStatus == RequestProcessingStatus.AppStarted;
            }

            public void OnHeader(Span<byte> name, Span<byte> value)
            {

            }

            public void OnStartLine(HttpMethod method, HttpVersion version, Span<byte> target, Span<byte> path, Span<byte> query, Span<byte> customMethod, bool pathEncoded)
            {

            }

            public void Reset()
            {
                _requestProcessingStatus = RequestProcessingStatus.RequestPending;
                _httpParser.Reset();
            }

            private enum RequestProcessingStatus
            {
                RequestPending,
                ParsingRequestLine,
                ParsingHeaders,
                AppStarted
            }
        }
    }
}
