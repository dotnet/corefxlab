using System;
using System.Collections.Generic;
using Xunit;
using Microsoft.Xunit.Performance;
using System.Text;
using System.IO.Pipelines.Samples;
using System.Text.Http;
using System.IO.Pipelines;
using System.Text.Formatting;

public partial class E2EPipelineTests
{
    private static readonly byte[] s_genericRequest = Encoding.UTF8.GetBytes(@"GET /developer/documentation/data-insertion/r-sample-http-get HTTP/1.1
Host: marketing.adobe.com
Connection: keep-alive
Cache-Control: max-age=0
Upgrade-Insecure-Requests: 1
User-Agent: Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.98 Safari/537.36
Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8
Accept-Encoding: gzip, deflate, sdch, br
Accept-Language: en-US,en;q=0.8,it;q=0.6,ms;q=0.4

");

    [Benchmark]
    [InlineData(1000, 250)]
    private static void TechEmpowerHelloWorldNoIO(int numberOfRequests, int concurrentConnections)
    {
        foreach (var iteration in Benchmark.Iterations)
        {
            using (iteration.StartMeasurement())
            {
                RawInMemoryHttpServer.Run(numberOfRequests, concurrentConnections, s_genericRequest, WriteResponse);
            }
        }
    }

    static void WriteResponse(HttpRequest request, WritableBuffer output)
    {
        var formatter = new OutputFormatter<WritableBuffer>(output, EncodingData.InvariantUtf8);
        formatter.Append("HTTP/1.1 200 OK");
        formatter.Append("\r\nContent-Length: 13");
        formatter.Append("\r\nContent-Type: text/plain");
        formatter.Append("\r\n\r\n");
        formatter.Append("Hello, World!");
    }
}

