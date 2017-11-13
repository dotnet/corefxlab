// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Text.Formatting;
using System.Text.Json;
using System.Buffers;
using System.Threading;
using System.Text.Http.Parser;
using Microsoft.Net;

namespace LowAllocationWebServer
{
    class SampleRestServer : RoutingServer<SampleRestServer.Api>
    {
        public enum Api
        {
            HelloWorld,
            GetTime,
            GetJson,
            PostJson,
        }

        static SampleRestServer()
        {
            Apis.Add(Http.Method.Get, "/plaintext", Api.HelloWorld, WriteResponseForHelloWorld);
            Apis.Add(Http.Method.Get, "/time", Api.GetTime, WriteResponseForGetTime);
            Apis.Add(Http.Method.Get, "/json", Api.GetJson, WriteResponseForGetJson);
            Apis.Add(Http.Method.Post, "/jsonpost", Api.PostJson, WriteResponseForPostJson); // post body along the lines of: "{ "Count" = 3 }"
        }

        public SampleRestServer(CancellationToken cancellation, Log log, ushort port, byte address1, byte address2, byte address3, byte address4)
            : base(cancellation, log, port, address1, address2, address3, address4)
        { }

        static void WriteResponseForHelloWorld(HttpRequest request, TcpConnectionFormatter response)
        {
            WriteCommonHeaders(ref response, Http.Version.Http11, 200, "OK");

            response.Append("Content-Type : text/plain; charset=UTF-8\r\n");
            response.AppendEoh();

            response.Append("Hello, World");
        }

        static void WriteResponseForGetTime(HttpRequest request, TcpConnectionFormatter response)
        {
            WriteCommonHeaders(ref response, Http.Version.Http11, 200, "OK");

            response.Append("Content-Type : text/html; charset=UTF-8\r\n");
            response.AppendEoh();

            response.Format(@"<html><head><title>Time</title></head><body>{0:O}</body></html>", DateTime.UtcNow);
        }

        static void WriteResponseForGetJson(HttpRequest request, TcpConnectionFormatter response)
        {
            WriteCommonHeaders(ref response, Http.Version.Http11, 200, "OK");

            response.Append("Content-Type : application/json; charset=UTF-8\r\n");
            response.AppendEoh();

            // write response JSON
            var jsonWriter = new JsonWriter(response, prettyPrint: false);
            jsonWriter.WriteObjectStart();
            jsonWriter.WriteArrayStart("values");
            for (int i = 0; i < 5; i++)
            {
                jsonWriter.WriteValue("hello!");
            }
            jsonWriter.WriteArrayEnd();
            jsonWriter.WriteObjectEnd();
        }

        static void WriteResponseForPostJson(HttpRequest request, TcpConnectionFormatter response)
        {
            // read request json
            int requestedCount;

            // TODO: this should not convert to span
            var dom = JsonObject.Parse(request.Body);
            try
            {
                requestedCount = (int)dom["Count"];
            }
            finally
            {
                dom.Dispose();
            }

            WriteCommonHeaders(ref response, Http.Version.Http11, 200, "OK");
            response.Append("Content-Type : application/json; charset=UTF-8\r\n");
            response.AppendEoh();

            // write response JSON
            var jsonWriter = new JsonWriter(response, prettyPrint: false);
            jsonWriter.WriteObjectStart();
            jsonWriter.WriteArrayStart("values");
            for (int i = 0; i < requestedCount; i++)
            {
                jsonWriter.WriteValue("hello!");
            }
            jsonWriter.WriteArrayEnd();
            jsonWriter.WriteObjectEnd();
        }
    }
}
