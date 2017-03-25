// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Xunit.Performance;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Text;
using System.Text.Http;
using System.Text.Http.Parser;

public class HttpParserBench
{
    private static readonly byte[] s_plaintextTechEmpowerRequestBytes = Encoding.UTF8.GetBytes(_plaintextTechEmpowerRequest);

    private const string _plaintextTechEmpowerRequest =
        "GET /plaintext HTTP/1.1\r\n" +
        "Host: localhost\r\n" +
        "Accept: text/plain,text/html;q=0.9,application/xhtml+xml;q=0.9,application/xml;q=0.8,*/*;q=0.7\r\n" +
        "Connection: keep-alive\r\n" +
        "\r\n";

    const int Itterations = 1000;

    [Benchmark(InnerIterationCount = Itterations)]
    static ulong HttpParserRob()
    {
        ReadableBuffer buffer = ReadableBuffer.Create(s_plaintextTechEmpowerRequestBytes);
        var parser = new HttpParser();
        var request = new Request();

        ulong acumulator = 0;
        foreach (var iteration in Benchmark.Iterations)
        {
            using (iteration.StartMeasurement())
            {
                for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                {
                    parser.ParseRequestLine(request, buffer, out var consumed, out var read);
                    acumulator += (ulong)request.Method;
                }
            }
        }

        return acumulator;
    }

    [Benchmark(InnerIterationCount = Itterations)]
    static ulong HttpParserReadableBytes()
    {
        var buffer = new ReadOnlyBytes(s_plaintextTechEmpowerRequestBytes);
        var parser = new HttpParser();
        var request = new Request();

        ulong acumulator = 0;
        foreach (var iteration in Benchmark.Iterations)
        {
            using (iteration.StartMeasurement())
            {
                for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                {
                    parser.ParseRequestLine(request, buffer, out var consumed);
                    acumulator += (ulong)consumed;
                }
            }
        }

        return acumulator;
    }

    [Benchmark(InnerIterationCount = Itterations)]
    static ulong HttpRequestParser()
    {
        var buffer = new ReadOnlyBytes(s_plaintextTechEmpowerRequestBytes);

        ulong acumulator = 0;
        foreach (var iteration in Benchmark.Iterations)
        {
            using (iteration.StartMeasurement())
            {
                for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                {
                    var request = HttpRequest.Parse(buffer);
                    acumulator += (ulong)request.BodyIndex;
                }
            }
        }

        return acumulator;
    }
}

class Request : IHttpHeadersHandler, IHttpRequestLineHandler
{
    public Http.Method Method;
    public Http.Version Version;
    public string Path;
    public string Query;
    public string Target;

    public Dictionary<string, string> Headers = new Dictionary<string, string>();

    public void OnHeader(Span<byte> name, Span<byte> value)
    {
        //var nameString = PrimitiveEncoder.DecodeAscii(name);
        //var valueString = PrimitiveEncoder.DecodeAscii(value);
        //Headers.Add(nameString, valueString);
    }

    public void OnStartLine(Http.Method method, Http.Version version, Span<byte> target, Span<byte> path, Span<byte> query, Span<byte> customMethod, bool pathEncoded)
    {
        //Method = method;
        //Version = version;
        //Path = PrimitiveEncoder.DecodeAscii(path);
        //Query = PrimitiveEncoder.DecodeAscii(query);
        //Target = PrimitiveEncoder.DecodeAscii(target);
    }
}

