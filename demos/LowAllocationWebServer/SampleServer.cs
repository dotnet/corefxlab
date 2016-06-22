// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Net.Http;
using System;
using System.Diagnostics;
using System.Text.Formatting;
using System.Text.Http;
using System.Text.Json;
using System.Text.Parsing;
using System.Text.Utf8;

class SampleRestServer : HttpServer
{
    public readonly ApiRoutingTable<Api> Apis = new ApiRoutingTable<Api>();

    public enum Api
    {
        HelloWorld,
        GetTime,
        PostJson,
    }

    public SampleRestServer(Log log, ushort port, byte address1, byte address2, byte address3, byte address4) : base(log, port, address1, address2, address3, address4)
    {
        Apis.Add(Api.HelloWorld, HttpMethod.Get, requestUri: "/plaintext");
        Apis.Add(Api.GetTime, HttpMethod.Get, requestUri: "/time");
        Apis.Add(Api.PostJson, HttpMethod.Post, requestUri: "/json"); // post body along the lines of: "{ "Count" = "3" }" 
    }

    protected override void WriteResponse(HttpResponse response, HttpRequest request)
    {
        var api = Apis.Map(request.RequestLine);
        switch (api) {
            case Api.HelloWorld:
                WriteResponseForHelloWorld(response);
                break;
            case Api.GetTime:
                WriteResponseForGetTime(response, request.RequestLine);
                break;
            case Api.PostJson:
                WriteResponseForPostJson(response, request.RequestLine, request.Body);
                break;
            default:
                WriteResponseFor404(response, request.RequestLine);
                break;
        }
    }

    void WriteResponseForPostJson(HttpResponse response, HttpRequestLine requestLine, ReadOnlySpan<byte> body)
    {
        uint requestedCount = ReadCountUsingReader(body).GetValueOrDefault(1);

        var json = new JsonWriter<ResponseFormatter>(response.Body, prettyPrint: false);
        json.WriteObjectStart();
        json.WriteArrayStart();
        for (int i = 0; i < requestedCount; i++) {
            json.WriteString(DateTime.UtcNow.ToString()); // TODO: this needs to not allocate.
        }
        json.WriteArrayEnd();
        json.WriteObjectEnd();

        var headers = response.Headers;
        headers.AppendHttpStatusLine(HttpVersion.V1_1, 200, new Utf8String("OK"));
        headers.Append("Content-Length : ");
        headers.Append(response.Body.CommitedBytes);
        headers.AppendHttpNewLine();
        headers.Append("Content-Type : text/plain; charset=UTF-8");
        headers.AppendHttpNewLine();
        headers.Append("Server : .NET Core Sample Serve");
        headers.AppendHttpNewLine();
        headers.Append("Date : ");
        headers.Append(DateTime.UtcNow, 'R');
        headers.AppendHttpNewLine();
        headers.AppendHttpNewLine();
    }

    static void WriteResponseForHelloWorld(HttpResponse formatter)
    {
        formatter.Body.Append("Hello, World");

        formatter.Headers.AppendHttpStatusLine(HttpVersion.V1_1, 200, new Utf8String("OK"));
        formatter.Headers.Append("Content-Length : ");
        formatter.Headers.Append(formatter.Body.CommitedBytes);
        formatter.Headers.AppendHttpNewLine();
        formatter.Headers.Append("Content-Type : text/plain; charset=UTF-8");
        formatter.Headers.AppendHttpNewLine();
        formatter.Headers.Append("Server : .NET Core Sample Serve");
        formatter.Headers.AppendHttpNewLine();
        formatter.Headers.Append("Date : ");
        formatter.Headers.Append(DateTime.UtcNow, 'R');
        formatter.Headers.AppendHttpNewLine();
        formatter.Headers.AppendHttpNewLine();
    }

    static void WriteResponseForGetTime(HttpResponse formatter, HttpRequestLine request)
    {
        formatter.Body.Format(@"<html><head><title>Time</title></head><body>{0:O}</body></html>", DateTime.UtcNow);

        WriteCommonHeaders(formatter, HttpVersion.V1_1, 200, "OK", keepAlive: false);
        formatter.Headers.Append("Content-Length : ");
        formatter.Headers.Append(formatter.Body.CommitedBytes);
        formatter.Headers.AppendHttpNewLine();
        formatter.Headers.AppendHttpNewLine();

    }

    uint? ReadCountUsingReader(ReadOnlySpan<byte> json)
    {
        uint count;
        var reader = new JsonReader(new Utf8String(json));
        while (reader.Read()) {
            switch (reader.TokenType) {
                case JsonReader.JsonTokenType.Property:
                    var name = reader.GetName();
                    var value = reader.GetValue();
                    Console.WriteLine("Property {0} = {1}", name, value);
                    if (name == "Count") {
                        if (!InvariantParser.TryParse(value, out count)) {
                            return null;
                        }
                        return count;
                    }
                    break;
            }
        }
        return null;
    }

    uint? ReadCountUsingNonAllocatingDom(ReadOnlySpan<byte> json)
    {
        var parser = new JsonParser(json.CreateArray(), json.Length); // TODO: eliminate allocation
        JsonParseObject jsonObject = parser.Parse();
        uint count = (uint)jsonObject["Count"];
        return count;
    }
}


