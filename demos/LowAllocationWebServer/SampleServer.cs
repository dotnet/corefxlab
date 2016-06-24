// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Net.Http;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.Formatting;
using System.Text.Http;
using System.Text.Json;
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
        Apis.Add(HttpMethod.Get, "/plaintext", Api.HelloWorld, WriteResponseForHelloWorld);
        Apis.Add(HttpMethod.Get, "/time", Api.GetTime, WriteResponseForGetTime);
        Apis.Add(HttpMethod.Post, "/json",  Api.PostJson, WriteResponseForPostJson); // post body along the lines of: "{ "Count" = "3" }" 
    }

    protected override void WriteResponse(HttpRequest request, HttpResponse response)
    {
        if (!Apis.TryHandle(request, response)) {
            WriteResponseFor404(request, response);
        }
    }

    void WriteResponseForPostJson(HttpRequest request, HttpResponse response)
    {
        // read request JSON
        uint requestedCount = ReadCount(request.Body);

        // write response JSON
        var json = new JsonWriter<ResponseFormatter>(response.Body, prettyPrint: false);
        json.WriteObjectStart();
        json.WriteArrayStart();
        for (int i = 0; i < requestedCount; i++) {
            json.WriteString("hello!"); 
        }
        json.WriteArrayEnd();
        json.WriteObjectEnd();

        // write headers
        var headers = response.Headers;
        headers.AppendHttpStatusLine(HttpVersion.V1_1, 200, new Utf8String("OK"));
        headers.Append("Content-Length : ");
        headers.Append(response.Body.CommitedBytes);
        headers.AppendHttpNewLine();
        headers.Append("Content-Type : text/plain; charset=UTF-8");
        headers.AppendHttpNewLine();
        headers.Append("Server : .NET Core Sample Server");
        headers.AppendHttpNewLine();
        headers.Append("Date : ");
        headers.Append(DateTime.UtcNow, 'R');
        headers.AppendHttpNewLine();
        headers.AppendHttpNewLine();
    }

    static void WriteResponseForHelloWorld(HttpRequest request, HttpResponse response)
    {
        response.Body.Append("Hello, World");

        response.Headers.AppendHttpStatusLine(HttpVersion.V1_1, 200, new Utf8String("OK"));
        response.Headers.Append("Content-Length : ");
        response.Headers.Append(response.Body.CommitedBytes);
        response.Headers.AppendHttpNewLine();
        response.Headers.Append("Content-Type : text/plain; charset=UTF-8");
        response.Headers.AppendHttpNewLine();
        response.Headers.Append("Server : .NET Core Sample Server");
        response.Headers.AppendHttpNewLine();
        response.Headers.Append("Date : ");
        response.Headers.Append(DateTime.UtcNow, 'R');
        response.Headers.AppendHttpNewLine();
        response.Headers.AppendHttpNewLine();
    }

    static void WriteResponseForGetTime(HttpRequest request, HttpResponse response)
    {
        response.Body.Format(@"<html><head><title>Time</title></head><body>{0:O}</body></html>", DateTime.UtcNow);

        WriteCommonHeaders(response, HttpVersion.V1_1, 200, "OK", keepAlive: false);
        response.Headers.Append("Content-Length : ");
        response.Headers.Append(response.Body.CommitedBytes);
        response.Headers.AppendHttpNewLine();
        response.Headers.AppendHttpNewLine();
    }

    uint ReadCount(ReadOnlySpan<byte> json)
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
                            return 1;
                        }
                        return count;
                    }
                    break;
            }
        }
        return 1;
    }
}


