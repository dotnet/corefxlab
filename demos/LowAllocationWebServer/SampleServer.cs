// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Net.Http.Buffered;
using System.Text.Formatting;
using System.Text.Http;
using System.Text.Utf8;

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

    protected override void WriteResponse(BufferFormatter formatter, HttpRequest request)
    {
        var api = Apis.Map(request.RequestLine);
        switch (api) {
            case Api.HelloWorld:
                WriteResponseForHelloWorld(formatter);
                break;
            case Api.GetTime:
                WriteResponseForGetTime(formatter, request.RequestLine);
                break;
            default:
                // TODO: this should be built into the base class
                WriteResponseFor404(formatter, request.RequestLine);
                break;
        }
    }

    private static void WriteResponseForHelloWorld(BufferFormatter formatter)
    {
        formatter.WriteHttpStatusLine(new Utf8String("1.1"), new Utf8String("200"), new Utf8String("Ok"));
        formatter.WriteHttpHeader(new Utf8String("Content-Length"), new Utf8String("12"));
        formatter.WriteHttpHeader(new Utf8String("Content-Type"), new Utf8String("text/plain; charset=UTF-8"));
        formatter.WriteHttpHeader(new Utf8String("Server"), new Utf8String(".NET Core Sample Serve"));
        // TODO: this needs to not allocate
        formatter.WriteHttpHeader(new Utf8String("Date"), new Utf8String(DateTime.UtcNow.ToString("R")));
        formatter.EndHttpHeaderSection();
        formatter.WriteHttpBody(new Utf8String("Hello, World"));
    }

    static void WriteResponseForGetTime(BufferFormatter formatter, HttpRequestLine request)
    {
        // TODO: this needs to not allocate
        var body = string.Format(@"<html><head><title>Time</title></head><body>{0}</body></html>", DateTime.UtcNow.ToString("O"));
        WriteCommonHeaders(formatter, "1.1", "200", "Ok", keepAlive: false);
        // TOOD: this needs to not allocate
        formatter.WriteHttpHeader(new Utf8String("Content-Length"), new Utf8String(body.Length.ToString()));
        formatter.EndHttpHeaderSection();
        formatter.WriteHttpBody(new Utf8String(body));
    }
}

