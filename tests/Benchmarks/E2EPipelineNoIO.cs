// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Xunit;
using Microsoft.Xunit.Performance;
using System.Text;
using System.IO.Pipelines.Samples;
using System.Text.Http;
using System.IO.Pipelines;
using System.Text.Formatting;
using System.Text.Json;

public partial class E2EPipelineTests
{
    private static readonly byte[] s_genericRequest = Encoding.UTF8.GetBytes(_plaintextTechEmpowerRequest);

    private const string _plaintextTechEmpowerRequest =
        "GET /plaintext HTTP/1.1\r\n" +
        "Host: localhost\r\n" +
        "Accept: text/plain,text/html;q=0.9,application/xhtml+xml;q=0.9,application/xml;q=0.8,*/*;q=0.7\r\n" +
        "Connection: keep-alive\r\n" +
        "\r\n";

    [Benchmark]
    [InlineData(1000, 256)]
    [InlineData(1000, 1024)]
    [InlineData(1000, 4096)]
    private static void TechEmpowerHelloWorldNoIO(int numberOfRequests, int concurrentConnections)
    {
        foreach (var iteration in Benchmark.Iterations)
        {
            using (iteration.StartMeasurement())
            {
                RawInMemoryHttpServer.Run(numberOfRequests, concurrentConnections, s_genericRequest, (request, response) => {
                    var formatter = new OutputFormatter<WritableBuffer>(response, TextEncoder.Utf8);
                    formatter.Append("HTTP/1.1 200 OK");
                    formatter.Append("\r\nContent-Length: 13");
                    formatter.Append("\r\nContent-Type: text/plain");
                    formatter.Format("\r\nDate: {0:R}", DateTime.UtcNow);
                    formatter.Append("Server: System.IO.Pipelines");
                    formatter.Append("\r\n\r\n");

                    // write body
                    formatter.Append("Hello, World!");
                });
            }
        }
    }

    [Benchmark(Skip = "The generic type 'System.Collections.Generic.KeyValuePair`2' was used with an invalid instantiation in assembly 'System.Private.CoreLib")]
    [InlineData(1000, 256)]
    [InlineData(1000, 1024)]
    [InlineData(1000, 4096)]
    private static void TechEmpowerHelloWorldNoIOSingleSegmentParser(int numberOfRequests, int concurrentConnections)
    {
        foreach (var iteration in Benchmark.Iterations)
        {
            using (iteration.StartMeasurement())
            {
                RawInMemoryHttpServer.RunSingleSegmentParser(numberOfRequests, concurrentConnections, s_genericRequest, (request, response)=> {
                    var formatter = new OutputFormatter<WritableBuffer>(response, TextEncoder.Utf8);
                    formatter.Append("HTTP/1.1 200 OK");
                    formatter.Append("\r\nContent-Length: 13");
                    formatter.Append("\r\nContent-Type: text/plain");
                    formatter.Format("\r\nDate: {0:R}", DateTime.UtcNow);
                    formatter.Append("Server: System.IO.Pipelines");
                    formatter.Append("\r\n\r\n");

                    // write body
                    formatter.Append("Hello, World!");
                });
            }
        }
    }

    [Benchmark]
    [InlineData(1000, 256)]
    [InlineData(1000, 1024)]
    [InlineData(1000, 4096)]
    private static void TechEmpowerJsonNoIO(int numberOfRequests, int concurrentConnections)
    {
        foreach (var iteration in Benchmark.Iterations)
        {
            using (iteration.StartMeasurement())
            {
                RawInMemoryHttpServer.Run(numberOfRequests, concurrentConnections, s_genericRequest, (request, response) => {
                    var formatter = new OutputFormatter<WritableBuffer>(response, TextEncoder.Utf8);
                    formatter.Append("HTTP/1.1 200 OK");
                    formatter.Append("\r\nContent-Length: 25");
                    formatter.Append("\r\nContent-Type: application/json");
                    formatter.Format("\r\nDate: {0:R}", DateTime.UtcNow);
                    formatter.Append("Server: System.IO.Pipelines");
                    formatter.Append("\r\n\r\n");

                    // write body
                    var writer = new JsonWriter<OutputFormatter<WritableBuffer>>(formatter);
                    writer.WriteObjectStart();
                    writer.WriteAttribute("message", "Hello, World!");
                    writer.WriteObjectEnd();                  
                });
            }
        }
    }
}
