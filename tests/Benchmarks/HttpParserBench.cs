// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Xunit.Performance;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.Sequences;
using System.IO.Pipelines;
using System.Text;
using System.Text.Http;
using System.Text.Http.Parser;
using Position = System.IO.Pipelines.Position;

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
    static bool RequestLineRb()
    {
        ReadOnlyBuffer buffer = ReadOnlyBuffer.Create(s_plaintextTechEmpowerRequestBytes);
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
    static bool RequestLineRobCursors()
    {
        var buffer = new ReadOnlyBytes(s_plaintextTechEmpowerRequestBytes);
        var parser = new HttpParser();
        var request = new Request();
        System.Collections.Sequences.Position consumed = default;
        System.Collections.Sequences.Position read;
        bool success = true;

        foreach (var iteration in Benchmark.Iterations)
        {
            using (iteration.StartMeasurement())
            {
                for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                {
                    success = success && parser.ParseRequestLine(ref request, buffer, out consumed, out read);
                    success = success && parser.ParseRequestLine(ref request, buffer, out consumed, out read);
                    success = success && parser.ParseRequestLine(ref request, buffer, out consumed, out read);
                    success = success && parser.ParseRequestLine(ref request, buffer, out consumed, out read);
                    success = success && parser.ParseRequestLine(ref request, buffer, out consumed, out read);
                    success = success && parser.ParseRequestLine(ref request, buffer, out consumed, out read);
                    success = success && parser.ParseRequestLine(ref request, buffer, out consumed, out read);
                    success = success && parser.ParseRequestLine(ref request, buffer, out consumed, out read);
                    success = success && parser.ParseRequestLine(ref request, buffer, out consumed, out read);
                    success = success && parser.ParseRequestLine(ref request, buffer, out consumed, out read);

                }
            }
        }

        return success;
    }

    [Benchmark(InnerIterationCount = Itterations)]
    static bool RequestLineRob()
    {
        var buffer = new ReadOnlyBytes(s_plaintextTechEmpowerRequestBytes);
        var parser = new HttpParser();
        var request = new RequestStruct();
        int consumed = 0;
        bool success = true;

        foreach (var iteration in Benchmark.Iterations)
        {
            using (iteration.StartMeasurement())
            {
                for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                {
                    success = success && parser.ParseRequestLine(ref request, buffer, out consumed);
                    success = success && parser.ParseRequestLine(ref request, buffer, out consumed);
                    success = success && parser.ParseRequestLine(ref request, buffer, out consumed);
                    success = success && parser.ParseRequestLine(ref request, buffer, out consumed);
                    success = success && parser.ParseRequestLine(ref request, buffer, out consumed);
                    success = success && parser.ParseRequestLine(ref request, buffer, out consumed);
                    success = success && parser.ParseRequestLine(ref request, buffer, out consumed);
                    success = success && parser.ParseRequestLine(ref request, buffer, out consumed);
                    success = success && parser.ParseRequestLine(ref request, buffer, out consumed);
                    success = success && parser.ParseRequestLine(ref request, buffer, out consumed);
                }
            }
        }

        return success;
    }

    [Benchmark(InnerIterationCount = Itterations)]
    static bool HeadersRb()
    {
        ReadOnlyBuffer buffer = ReadOnlyBuffer.Create(s_plaintextTechEmpowerHeadersBytes);
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
    static bool HeadersRobCursors()
    {
        var buffer = new ReadOnlyBytes(s_plaintextTechEmpowerHeadersBytes);
        var parser = new HttpParser();
        var request = new Request();
        System.Collections.Sequences.Position consumed = default;
        System.Collections.Sequences.Position examined;
        int consumedBytes;
        bool success = true;

        foreach (var iteration in Benchmark.Iterations)
        {
            using (iteration.StartMeasurement())
            {
                for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                {
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed, out examined, out consumedBytes);
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed, out examined, out consumedBytes);
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed, out examined, out consumedBytes);
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed, out examined, out consumedBytes);
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed, out examined, out consumedBytes);
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed, out examined, out consumedBytes);
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed, out examined, out consumedBytes);
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed, out examined, out consumedBytes);
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed, out examined, out consumedBytes);
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed, out examined, out consumedBytes);
                }
            }
        }

        return success;
    }

    [Benchmark(InnerIterationCount = Itterations)]
    static bool HeadersRobRef()
    {
        var buffer = new ReadOnlyBytes(s_plaintextTechEmpowerHeadersBytes);
        var parser = new HttpParser();
        var request = new RequestStruct();
        int consumed;
        bool success = true;

        foreach (var iteration in Benchmark.Iterations) {
            using (iteration.StartMeasurement()) {
                for (int i = 0; i < Benchmark.InnerIterationCount; i++) {
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed);
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed);
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed);
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed);
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed);
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed);
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed);
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed);
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed);
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed);
                }
            }
        }

        return success;
    }

    [Benchmark(InnerIterationCount = Itterations)]
    static bool HeadersRob()
    {
        var buffer = new ReadOnlyBytes(s_plaintextTechEmpowerHeadersBytes);
        var parser = new HttpParser();
        var request = new Request();
        int consumed;
        bool success = true;

        foreach (var iteration in Benchmark.Iterations) {
            using (iteration.StartMeasurement()) {
                for (int i = 0; i < Benchmark.InnerIterationCount; i++) {
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed);
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed);
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed);
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed);
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed);
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed);
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed);
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed);
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed);
                    success = success && parser.ParseHeaders(ref request, buffer, out consumed);
                }
            }
        }

        return success;
    }

    [Benchmark(InnerIterationCount = Itterations)]
    static bool FullRequestRb()
    {
        ReadOnlyBuffer buffer = ReadOnlyBuffer.Create(s_plaintextTechEmpowerRequestBytes);
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

    [Benchmark(InnerIterationCount = Itterations)]
    static bool FullRequestRob()
    {
        var buffer = new ReadOnlyBytes(s_plaintextTechEmpowerRequestBytes);
        var parser = new HttpParser();
        var request = new RequestStruct();
        int consumedBytes = 0;
        bool success = true;

        foreach (var iteration in Benchmark.Iterations) {
            using (iteration.StartMeasurement()) {
                for (int i = 0; i < Benchmark.InnerIterationCount; i++) {
                    success = success && parser.ParseRequestLine(ref request, buffer, out consumedBytes);
                    success = success && parser.ParseHeaders(ref request, buffer.Slice(consumedBytes), out consumedBytes);
                    success = success && parser.ParseRequestLine(ref request, buffer, out consumedBytes);
                    success = success && parser.ParseHeaders(ref request, buffer.Slice(consumedBytes), out consumedBytes);
                    success = success && parser.ParseRequestLine(ref request, buffer, out consumedBytes);
                    success = success && parser.ParseHeaders(ref request, buffer.Slice(consumedBytes), out consumedBytes);
                    success = success && parser.ParseRequestLine(ref request, buffer, out consumedBytes);
                    success = success && parser.ParseHeaders(ref request, buffer.Slice(consumedBytes), out consumedBytes);
                    success = success && parser.ParseRequestLine(ref request, buffer, out consumedBytes);
                    success = success && parser.ParseHeaders(ref request, buffer.Slice(consumedBytes), out consumedBytes);
                }
            }
        }

        return success;
    }

    [Benchmark(InnerIterationCount = Itterations)]
    static bool FullRequestRobCursors()
    {
        var buffer = new ReadOnlyBytes(s_plaintextTechEmpowerRequestBytes);
        var parser = new HttpParser();
        var request = new RequestStruct();
        System.Collections.Sequences.Position consumed = default;
        System.Collections.Sequences.Position examined;
        bool success = true;

        foreach (var iteration in Benchmark.Iterations)
        {
            using (iteration.StartMeasurement())
            {
                for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                {
                    success = success && parser.ParseRequest(ref request, buffer, out consumed, out examined);
                    success = success && parser.ParseRequest(ref request, buffer, out consumed, out examined);
                    success = success && parser.ParseRequest(ref request, buffer, out consumed, out examined);
                    success = success && parser.ParseRequest(ref request, buffer, out consumed, out examined);
                    success = success && parser.ParseRequest(ref request, buffer, out consumed, out examined);
                }
            }
        }

        return success;
    }
}

static class HttpParserExtensions
{
    public static bool ParseRequestLine<T>(this HttpParser parser, ref T handler, in ReadOnlyBytes buffer, out System.Collections.Sequences.Position consumed, out System.Collections.Sequences.Position examined) where T : IHttpRequestLineHandler
    {
        if(parser.ParseRequestLine(ref handler, buffer, out int consumedBytes))
        {
            consumed = buffer.PositionAt(consumedBytes).GetValueOrDefault();
            examined = consumed;
            return true;
        }
        consumed = buffer.PositionAt(0).GetValueOrDefault();
        examined = default;
        return false;
    }

    public static bool ParseHeaders<T>(this HttpParser parser, ref T handler, in ReadOnlyBytes buffer, out System.Collections.Sequences.Position consumed, out System.Collections.Sequences.Position examined, out int consumedBytes) where T : IHttpHeadersHandler
    {
        if (parser.ParseHeaders(ref handler, buffer, out consumedBytes))
        {
            consumed = buffer.PositionAt(consumedBytes).GetValueOrDefault();
            examined = consumed;
            return true;
        }
        consumed = buffer.PositionAt(0).GetValueOrDefault();
        examined = default;
        return false;
    }

    public static bool ParseRequest<T>(this HttpParser parser, ref T handler, in ReadOnlyBytes buffer, out System.Collections.Sequences.Position consumed, out System.Collections.Sequences.Position examined) where T : IHttpRequestLineHandler, IHttpHeadersHandler
    {
        if (
            parser.ParseRequestLine(ref handler, buffer, out var consumedRLBytes) &&
            parser.ParseHeaders(ref handler, buffer.Slice(consumedRLBytes), out var consumedHDBytes)
        )
        {
            consumed = buffer.PositionAt(consumedRLBytes + consumedHDBytes).GetValueOrDefault();
            examined = consumed;
            return true;
        }
        else
        {
            consumed = buffer.PositionAt(0).GetValueOrDefault();
            examined = default;
            return false;
        }
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

