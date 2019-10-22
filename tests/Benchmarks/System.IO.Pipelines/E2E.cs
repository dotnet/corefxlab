// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.IO.Pipelines.Samples;
using System.Text;
using System.Text.Formatting;
using System.Text.Json;
using BenchmarkDotNet.Attributes;

namespace System.IO.Pipelines.Benchmarks
{
    public class E2E
    {
        private static readonly byte[] s_genericRequest = Encoding.UTF8.GetBytes(_plaintextTechEmpowerRequest);

        private const string _plaintextTechEmpowerRequest =
            "GET /plaintext HTTP/1.1\r\n" +
            "Host: localhost\r\n" +
            "Accept: text/plain,text/html;q=0.9,application/xhtml+xml;q=0.9,application/xml;q=0.8,*/*;q=0.7\r\n" +
            "Connection: keep-alive\r\n" +
            "\r\n";

        [Benchmark]
        [Arguments(10000, 256)]
        public void TechEmpowerHelloWorldNoIO(int numberOfRequests, int concurrentConnections)
        {
            RawInMemoryHttpServer.Run(numberOfRequests, concurrentConnections, s_genericRequest, (request, response) =>
            {
                var formatter = new BufferWriterFormatter<PipeWriter>(response, SymbolTable.InvariantUtf8);
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

        [Benchmark]
        [Arguments(10000, 256)]
        public void TechEmpowerJsonNoIO(int numberOfRequests, int concurrentConnections)
        {
            RawInMemoryHttpServer.Run(numberOfRequests, concurrentConnections, s_genericRequest, (request, response) =>
            {
                var formatter = new BufferWriterFormatter<PipeWriter>(response, SymbolTable.InvariantUtf8);
                formatter.Append("HTTP/1.1 200 OK");
                formatter.Append("\r\nContent-Length: 25");
                formatter.Append("\r\nContent-Type: application/json");
                formatter.Format("\r\nDate: {0:R}", DateTime.UtcNow);
                formatter.Append("Server: System.IO.Pipelines");
                formatter.Append("\r\n\r\n");

                // write body
                var writer = new Utf8JsonWriter(formatter);
                writer.WriteStartObject();
                writer.WriteString("message", "Hello, World!");
                writer.WriteEndObject();
                writer.Flush();
            });
        }
    }
}
