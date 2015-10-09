// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Buffers;
using System.Diagnostics;
using System.Net.Http.Buffered;
using System.Text.Formatting;

class SampleRestServer : HttpServer
{
    public readonly ApiRoutingTable<Api> Apis = new ApiRoutingTable<Api>();

    public enum Api
    {
        HelloWorld = 0,
        GetTime = 1,
    }

    public SampleRestServer(Log log, ushort port, byte address1, byte address2, byte address3, byte address4) : base(log, port, address1, address2, address3, address4)
    {
        Apis.Add(Api.HelloWorld, HttpMethod.Get, requestUri: "/");
        Apis.Add(Api.GetTime, HttpMethod.Get, requestUri: "/time");
    }

    protected override HttpServerBuffer CreateResponse(HttpRequestLine requestLine, ByteSpan headerAndBody)
    {
        var api = Apis.Map(requestLine);
        switch (api) {
            case Api.HelloWorld:
                return CreateResponseForHelloWorld();
            case Api.GetTime:
                return CreateResponseForGetTime(requestLine);
            default:
                return CreateResponseFor404(requestLine, headerAndBody);
        }
    }

    private HttpServerBuffer CreateResponseForHelloWorld()
    {
        var formatter = new BufferFormatter(1024, FormattingData.InvariantUtf8);
        formatter.Append(@"HTTP/1.1 200 OK");
        formatter.Append(HttpNewline);
        formatter.Append("Content-Length: 12");
        formatter.Append(HttpNewline);
        formatter.Append("Content-Type: text/plain; charset=UTF-8");
        formatter.Append(HttpNewline);
        formatter.Append("Server: .NET Core Sample Server");
        formatter.Append(HttpNewline);
        formatter.Append("Date: ");
        formatter.Append(DateTime.UtcNow, 'R');
        formatter.Append(HttpNewline);
        formatter.Append(HttpNewline);
        formatter.Append("Hello, World");
        return new HttpServerBuffer(formatter.Buffer, formatter.CommitedByteCount, BufferPool.Shared);
    }

    static HttpServerBuffer CreateResponseForGetTime(HttpRequestLine request)
    {
        var formatter = new BufferFormatter(1024, FormattingData.InvariantUtf8);
        WriteCommonHeaders(formatter, @"HTTP/1.1 200 OK", request.IsKeepAlive());
        formatter.Append(HttpNewline);

        formatter.Append(@"<html><head><title>Time</title></head><body>");
        formatter.Append(DateTime.UtcNow, 'O');
        formatter.Append(@"</body></html>");
        return new HttpServerBuffer(formatter.Buffer, formatter.CommitedByteCount, BufferPool.Shared);
    }
}

