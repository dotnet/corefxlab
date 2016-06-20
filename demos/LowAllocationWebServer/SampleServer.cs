// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Net.Http;
using System;
using System.Buffers;
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
            case Api.PostJson:
                WriteResponseForPostJson(formatter, request.RequestLine, request.Body);
                break;
            default:
                // TODO: this should be built into the base class
                WriteResponseFor404(formatter, request.RequestLine);
                break;
        }
    }

    // This method is a bit of a mess. We need to fix many Http and Json APIs
    void WriteResponseForPostJson(BufferFormatter formatter, HttpRequestLine requestLine, ReadOnlySpan<byte> body)
    {
        Console.WriteLine(new Utf8String(body));

        uint requestedCount = ReadCountUsingReader(body).GetValueOrDefault(1);
        //uint requestedCount = ReadCountUsingNonAllocatingDom(body).GetValueOrDefault(1);

        // TODO: this needs to be written directly to the buffer after content length reservation is implemented.
        var buffer = ArrayPool<byte>.Shared.Rent(2048);
        var spanFormatter = new SpanFormatter(buffer.Slice(), FormattingData.InvariantUtf8);
        var json = new JsonWriter<SpanFormatter>(spanFormatter,  prettyPrint: true);
        json.WriteObjectStart();
        json.WriteArrayStart();
        for (int i = 0; i < requestedCount; i++) {
            json.WriteString(DateTime.UtcNow.ToString()); // TODO: this needs to not allocate.
        }
        json.WriteArrayEnd(); ;
        json.WriteObjectEnd();
        var responseBodyText = new Utf8String(buffer, 0, spanFormatter.CommitedByteCount);

        formatter.AppendHttpStatusLine(HttpVersion.V1_1, 200, new Utf8String("OK"));
        formatter.Append(new Utf8String("Content-Length : "));
        formatter.Append(responseBodyText.Length);
        formatter.AppendHttpNewLine();
        formatter.Append("Content-Type : text/plain; charset=UTF-8");
        formatter.AppendHttpNewLine();
        formatter.Append("Server : .NET Core Sample Serve");
        formatter.AppendHttpNewLine();
        formatter.Append(new Utf8String("Date : "));
        formatter.Append(DateTime.UtcNow.ToString("R"));
        formatter.AppendHttpNewLine();
        formatter.AppendHttpNewLine();
        formatter.Append(responseBodyText);

        ArrayPool<byte>.Shared.Return(buffer);
    }

    static void WriteResponseForHelloWorld(BufferFormatter formatter)
    {
        var responseBodyText = new Utf8String("Hello, World");

        formatter.AppendHttpStatusLine(HttpVersion.V1_1, 200, new Utf8String("OK"));
        formatter.Append(new Utf8String("Content-Length : "));
        formatter.Append(responseBodyText.Length);
        formatter.AppendHttpNewLine();
        formatter.Append("Content-Type : text/plain; charset=UTF-8");
        formatter.AppendHttpNewLine();
        formatter.Append("Server : .NET Core Sample Serve");
        formatter.AppendHttpNewLine();
        formatter.Append(new Utf8String("Date : "));
        formatter.Append(DateTime.UtcNow.ToString("R"));
        formatter.AppendHttpNewLine();
        formatter.AppendHttpNewLine();
        formatter.Append(responseBodyText);
    }

    static void WriteResponseForGetTime(BufferFormatter formatter, HttpRequestLine request)
    {
        // TODO: this needs to not allocate.
        var body = string.Format(@"<html><head><title>Time</title></head><body>{0}</body></html>", DateTime.UtcNow.ToString("O"));
        WriteCommonHeaders(formatter, HttpVersion.V1_1, 200, "OK", keepAlive: false);
        formatter.Append(new Utf8String("Content-Length : "));
        formatter.Append(body.Length);
        formatter.AppendHttpNewLine();
        formatter.AppendHttpNewLine();
        formatter.Append(body);
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

