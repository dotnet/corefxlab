// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Buffers;
using System.Net;
using System.Net.Http.Buffered;
using System.Text.Formatting;

class SampleRestServer : HttpServer
{
    public readonly ApiRoutingTable<Api> Apis = new ApiRoutingTable<Api>();

    public enum Api
    {
        GetTime = 1,
    }

    public SampleRestServer(Log log, ushort port, byte address1, byte address2, byte address3, byte address4) : base(log, port, address1, address2, address3, address4)
    {
        Apis.Add(Api.GetTime, HttpMethod.Get, requestUri: "/time");
    }

    protected override HttpServerBuffer CreateResponse(HttpRequestLine requestLine, Span<byte> headerAndBody)
    {
        var api = Apis.Map(requestLine);
        switch (api) {
            case Api.GetTime:
                return CreateResponseForGetTime();
            default:
                return CreateResponseFor404(requestLine, headerAndBody);
        }
    }

    static HttpServerBuffer CreateResponseForGetTime()
    {
        var formatter = new BufferFormatter(1024, FormattingData.InvariantUtf8);
        WriteCommonHeaders(formatter, @"HTTP/1.1 200 OK");
        formatter.Append(HttpNewline);

        formatter.Append(@"<html><head><title>Time</title></head><body>");
        formatter.Append(DateTime.UtcNow, 'O');
        formatter.Append(@"</body></html>");
        return new HttpServerBuffer(formatter.Buffer, formatter.CommitedByteCount, BufferPool.Shared);
    }
}

