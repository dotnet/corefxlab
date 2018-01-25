// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Xunit.Performance;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.Sequences;
using System.Text;
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

    private static readonly byte[] s_plaintextTechEmpowerHeadersBytes = Encoding.UTF8.GetBytes(_plaintextTechEmpowerHeaders);

    private const string _plaintextTechEmpowerHeaders =
    "Host: localhost\r\n" +
    "Accept: text/plain,text/html;q=0.9,application/xhtml+xml;q=0.9,application/xml;q=0.8,*/*;q=0.7\r\n" +
    "Connection: keep-alive\r\n" +
    "\r\n";

    const int Itterations = 10000;

    [Benchmark(InnerIterationCount = Itterations)]
    static bool RequestLine()
    {
        ReadOnlyBuffer<byte> buffer = new ReadOnlyBuffer<byte>(s_plaintextTechEmpowerRequestBytes, 0, s_plaintextTechEmpowerHeadersBytes.Length);
        var parser = new HttpParser();
        var request = new Request();
        Position consumed = default;
        Position read;
        bool success = true;

        foreach (var iteration in Benchmark.Iterations)
        {
            using (iteration.StartMeasurement())
            {
                for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                {
                    success = success && parser.ParseRequestLine(request, buffer, out consumed, out read);
                    success = success && parser.ParseRequestLine(request, buffer, out consumed, out read);
                    success = success && parser.ParseRequestLine(request, buffer, out consumed, out read);
                    success = success && parser.ParseRequestLine(request, buffer, out consumed, out read);
                    success = success && parser.ParseRequestLine(request, buffer, out consumed, out read);
                    success = success && parser.ParseRequestLine(request, buffer, out consumed, out read);
                    success = success && parser.ParseRequestLine(request, buffer, out consumed, out read);
                    success = success && parser.ParseRequestLine(request, buffer, out consumed, out read);
                    success = success && parser.ParseRequestLine(request, buffer, out consumed, out read);
                    success = success && parser.ParseRequestLine(request, buffer, out consumed, out read);

                }
            }
        }

        return success;
    }

    [Benchmark(InnerIterationCount = Itterations)]
    static bool Headers()
    {
        ReadOnlyBuffer<byte> buffer = new ReadOnlyBuffer<byte>(s_plaintextTechEmpowerHeadersBytes);
        var parser = new HttpParser();
        var request = new Request();
        Position consumed;
        Position examined;
        int consumedBytes;
        bool success = true;

        foreach (var iteration in Benchmark.Iterations) {
            using (iteration.StartMeasurement()) {
                for (int i = 0; i < Benchmark.InnerIterationCount; i++) {
                    success = success && parser.ParseHeaders(request, buffer, out consumed, out examined, out consumedBytes);
                    success = success && parser.ParseHeaders(request, buffer, out consumed, out examined, out consumedBytes);
                    success = success && parser.ParseHeaders(request, buffer, out consumed, out examined, out consumedBytes);
                    success = success && parser.ParseHeaders(request, buffer, out consumed, out examined, out consumedBytes);
                    success = success && parser.ParseHeaders(request, buffer, out consumed, out examined, out consumedBytes);
                    success = success && parser.ParseHeaders(request, buffer, out consumed, out examined, out consumedBytes);
                    success = success && parser.ParseHeaders(request, buffer, out consumed, out examined, out consumedBytes);
                    success = success && parser.ParseHeaders(request, buffer, out consumed, out examined, out consumedBytes);
                    success = success && parser.ParseHeaders(request, buffer, out consumed, out examined, out consumedBytes);
                    success = success && parser.ParseHeaders(request, buffer, out consumed, out examined, out consumedBytes);
                }
            }
        }

        return success;
    }

    [Benchmark(InnerIterationCount = Itterations)]
    static bool FullRequest()
    {
        ReadOnlyBuffer<byte> buffer = new ReadOnlyBuffer<byte>(s_plaintextTechEmpowerRequestBytes);
        var parser = new HttpParser();
        var request = new Request();
        int consumedBytes  = 0;
        Position examined;
        Position consumed = buffer.Start;
        bool success = true;

        foreach (var iteration in Benchmark.Iterations) {
            using (iteration.StartMeasurement()) {
                for (int i = 0; i < Benchmark.InnerIterationCount; i++) {
                    success = success && parser.ParseRequestLine(request, buffer, out consumed, out examined);
                    success = success && parser.ParseHeaders(request, buffer.Slice(consumed), out consumed, out examined, out consumedBytes);

                    success = success && parser.ParseRequestLine(request, buffer, out consumed, out examined);
                    success = success && parser.ParseHeaders(request, buffer.Slice(consumed), out consumed, out examined, out consumedBytes);

                    success = success && parser.ParseRequestLine(request, buffer, out consumed, out examined);
                    success = success && parser.ParseHeaders(request, buffer.Slice(consumed), out consumed, out examined, out consumedBytes);

                    success = success && parser.ParseRequestLine(request, buffer, out consumed, out examined);
                    success = success && parser.ParseHeaders(request, buffer.Slice(consumed), out consumed, out examined, out consumedBytes);

                    success = success && parser.ParseRequestLine(request, buffer, out consumed, out examined);
                    success = success && parser.ParseHeaders(request, buffer.Slice(consumed), out consumed, out examined, out consumedBytes);
                }
            }
        }

        return success;
    }
}

class Request : IHttpHeadersHandler, IHttpRequestLineHandler
{
    //public Http.Method Method;
    //public Http.Version Version;
    //public string Path;
    //public string Query;
    //public string Target;

    public Dictionary<string, string> Headers = new Dictionary<string, string>();

    public void OnHeader(ReadOnlySpan<byte> name, ReadOnlySpan<byte> value)
    {
        //var nameString = PrimitiveEncoder.DecodeAscii(name);
        //var valueString = PrimitiveEncoder.DecodeAscii(value);
        //Headers.Add(nameString, valueString);
    }

    public void OnStartLine(Http.Method method, Http.Version version, ReadOnlySpan<byte> target, ReadOnlySpan<byte> path, ReadOnlySpan<byte> query, ReadOnlySpan<byte> customMethod, bool pathEncoded)
    {
        //Method = method;
        //Version = version;
        //Path = PrimitiveEncoder.DecodeAscii(path);
        //Query = PrimitiveEncoder.DecodeAscii(query);
        //Target = PrimitiveEncoder.DecodeAscii(target);
    }
}

struct RequestStruct : IHttpHeadersHandler, IHttpRequestLineHandler {
    //public Http.Method Method;
    //public Http.Version Version;
    //public string Path;
    //public string Query;
    //public string Target;

    //public Dictionary<string, string> Headers = new Dictionary<string, string>();

    public void OnHeader(ReadOnlySpan<byte> name, ReadOnlySpan<byte> value)
    {
        //var nameString = PrimitiveEncoder.DecodeAscii(name);
        //var valueString = PrimitiveEncoder.DecodeAscii(value);
        //Headers.Add(nameString, valueString);
    }

    public void OnStartLine(Http.Method method, Http.Version version, ReadOnlySpan<byte> target, ReadOnlySpan<byte> path, ReadOnlySpan<byte> query, ReadOnlySpan<byte> customMethod, bool pathEncoded)
    {
        //Method = method;
        //Version = version;
        //Path = PrimitiveEncoder.DecodeAscii(path);
        //Query = PrimitiveEncoder.DecodeAscii(query);
        //Target = PrimitiveEncoder.DecodeAscii(target);
    }
}

