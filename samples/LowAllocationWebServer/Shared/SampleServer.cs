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
using System.Buffers;

namespace LowAllocationWebServer
{
    class SampleRestServer : HttpServer
    {
        public readonly ApiRoutingTable<Api> Apis = new ApiRoutingTable<Api>();

        public enum Api
        {
            HelloWorld,
            GetTime,
            GetJson,
            PostJson,
        }

        public SampleRestServer(Log log, ushort port, byte address1, byte address2, byte address3, byte address4) : base(log, port, address1, address2, address3, address4)
        {
            Apis.Add(HttpMethod.Get, "/plaintext", Api.HelloWorld, WriteResponseForHelloWorld);
            Apis.Add(HttpMethod.Get, "/time", Api.GetTime, WriteResponseForGetTime);
            Apis.Add(HttpMethod.Get, "/json", Api.GetJson, WriteResponseForGetJson); 
            Apis.Add(HttpMethod.Post, "/jsonpost", Api.PostJson, WriteResponseForPostJson); // post body along the lines of: "{ "Count" = 3 }" 
        }

        private void WriteResponseForGetJson(HttpRequest request, HttpResponse response)
        {
            WriteCommonHeaders(response, HttpVersion.V1_1, 200, "OK", keepAlive: false);
            var formatter = new ResponseFormatter(response);
            formatter.Append("Content-Type : application/json; charset=UTF-8\r\n");
            formatter.WriteEoh();

            // write response JSON
            var jsonWriter = new JsonWriter<ResponseFormatter>(formatter, prettyPrint: false);
            jsonWriter.WriteObjectStart();
            jsonWriter.WriteArrayStart();
            for (int i = 0; i < 5; i++)
            {
                jsonWriter.WriteString("hello!");
            }
            jsonWriter.WriteArrayEnd();
            jsonWriter.WriteObjectEnd();
        }

        protected override void WriteResponse(HttpRequest request, HttpResponse response)
        {
            if (!Apis.TryHandle(request, response))
            {
                WriteResponseFor404(request, response);
            }
        }

        void WriteResponseForPostJson(HttpRequest request, HttpResponse response)
        {
            // read request json
            int requestedCount;
            // TODO: this should not conver to span
            using (var dom = JsonObject.Parse(request.Body.ToSpan())) {
                requestedCount = (int)dom["Count"];
            }

            WriteCommonHeaders(response, HttpVersion.V1_1, 200, "OK", keepAlive: false);
            var formatter = new ResponseFormatter(response);
            formatter.Append("Content-Type : application/json; charset=UTF-8\r\n");
            formatter.WriteEoh();

            // write response JSON
            var jsonWriter = new JsonWriter<ResponseFormatter>(formatter, prettyPrint: false);
            jsonWriter.WriteObjectStart();
            jsonWriter.WriteArrayStart();
            for (int i = 0; i < requestedCount; i++)
            {
                jsonWriter.WriteString("hello!");
            }
            jsonWriter.WriteArrayEnd();
            jsonWriter.WriteObjectEnd();      
        }

        static void WriteResponseForHelloWorld(HttpRequest request, HttpResponse response)
        {
            WriteCommonHeaders(response, HttpVersion.V1_1, 200, "OK", keepAlive: false);
            var formatter = new ResponseFormatter(response);
            formatter.Append("Content-Type : text/plain; charset=UTF-8\r\n");
            formatter.WriteEoh();

            formatter.Append("Hello, World");
        }

        static void WriteResponseForGetTime(HttpRequest request, HttpResponse response)
        {
            WriteCommonHeaders(response, HttpVersion.V1_1, 200, "OK", keepAlive: false);
            var formatter = new ResponseFormatter(response);
            formatter.Append("Content-Type : text/html; charset=UTF-8\r\n");
            formatter.WriteEoh();

            formatter.Format(@"<html><head><title>Time</title></head><body>{0:O}</body></html>", DateTime.UtcNow);
        }
    }
}
