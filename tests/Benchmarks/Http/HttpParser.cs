// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using System.Text.Http.Parser;

public class HttpParser
{
    private const string _plaintextTechEmpowerRequest =
        "GET /plaintext HTTP/1.1\r\n" +
        "Host: localhost\r\n" +
        "Accept: text/plain,text/html;q=0.9,application/xhtml+xml;q=0.9,application/xml;q=0.8,*/*;q=0.7\r\n" +
        "Connection: keep-alive\r\n" +
        "\r\n";

    private const string _plaintextTechEmpowerHeaders =
    "Host: localhost\r\n" +
    "Accept: text/plain,text/html;q=0.9,application/xhtml+xml;q=0.9,application/xml;q=0.8,*/*;q=0.7\r\n" +
    "Connection: keep-alive\r\n" +
    "\r\n";

    private static readonly byte[] s_plaintextTechEmpowerHeadersArray = Encoding.UTF8.GetBytes(_plaintextTechEmpowerHeaders);
    private static readonly byte[] s_plaintextTechEmpowerRequestArray = Encoding.UTF8.GetBytes(_plaintextTechEmpowerRequest);

    private static readonly ReadOnlySequence<byte> s_plaintextTechEmpowerRequestRos = new ReadOnlySequence<byte>(s_plaintextTechEmpowerRequestArray);
    private static readonly ReadOnlySequence<byte> s_plaintextTechEmpowerHeadersRos = new ReadOnlySequence<byte>(s_plaintextTechEmpowerHeadersArray);

    private static readonly System.Text.Http.Parser.HttpParser s_parser = new System.Text.Http.Parser.HttpParser();

    [Benchmark]
    public void RequestLine()
    {
        var request = new Request();
        s_parser.ParseRequestLine(request, s_plaintextTechEmpowerRequestRos, out _, out _);
    }

    [Benchmark]
    public void Headers()
    {
        var request = new Request();
        s_parser.ParseHeaders(request, s_plaintextTechEmpowerHeadersRos, out _, out _, out _);
    }

    [Benchmark]
    public void FullRequest()
    {
        var request = new Request();
        s_parser.ParseRequestLine(request, s_plaintextTechEmpowerRequestRos, out var consumed, out _);
        s_parser.ParseHeaders(request, s_plaintextTechEmpowerRequestRos.Slice(consumed), out consumed, out _, out _);
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

struct RequestStruct : IHttpHeadersHandler, IHttpRequestLineHandler
{
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

