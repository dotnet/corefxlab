// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Net.Http;
using System;
using System.Diagnostics;
using System.Text.Formatting;
using System.Text.Http.SingleSegment;
using System.Text.Json;
using System.Text.Utf8;
using System.Text.Http;

namespace LowAllocationWebServer
{
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
            Apis.Add(HttpMethod.Post, "/json", Api.PostJson, WriteResponseForPostJson); // post body along the lines of: "{ "Count" = 3 }" 
        }

        protected override void WriteResponse(HttpRequestSingleSegment request, HttpResponse response)
        {
            if (!Apis.TryHandle(request, response))
            {
                WriteResponseFor404(request, response);
            }
        }

        void WriteResponseForPostJson(HttpRequestSingleSegment request, HttpResponse response)
        {
            // read request json
            int requestedCount;
            using (var dom = JsonObject.Parse(request.Body)) {
                requestedCount = (int)dom["Count"];
            }

            // write response JSON
            var jsonWriter = new JsonWriter<ResponseFormatter>(new ResponseFormatter(response.Body), prettyPrint: false);
            jsonWriter.WriteObjectStart();
            jsonWriter.WriteArrayStart();
            for (int i = 0; i < requestedCount; i++)
            {
                jsonWriter.WriteString("hello!");
            }
            jsonWriter.WriteArrayEnd();
            jsonWriter.WriteObjectEnd();

            // write headers
            var headers = new ResponseFormatter(response.Headers);
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

        static void WriteResponseForHelloWorld(HttpRequestSingleSegment request, HttpResponse response)
        {
            var body = new ResponseFormatter(response.Body);
            body.Append("Hello, World");

            var headers = new ResponseFormatter(response.Headers);
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

        static void WriteResponseForGetTime(HttpRequestSingleSegment request, HttpResponse response)
        {
            var body = new ResponseFormatter(response.Body);
            body.Format(@"<html><head><title>Time</title></head><body>{0:O}</body></html>", DateTime.UtcNow);

            WriteCommonHeaders(response, HttpVersion.V1_1, 200, "OK", keepAlive: false);
            var headers = new ResponseFormatter(response.Headers);
            headers.Append("Content-Length : ");
            headers.Append(response.Body.CommitedBytes);
            headers.AppendHttpNewLine();
            headers.AppendHttpNewLine();
        }
    }
}
