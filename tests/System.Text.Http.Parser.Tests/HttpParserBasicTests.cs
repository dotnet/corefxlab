// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;
using System.IO.Pipelines;
using System.Collections.Generic;
using System.Buffers;
using System.Buffers.Text;

namespace System.Text.Http.Parser.Tests
{
    public class HttpParserBasicTests
    {
        [Theory]
        [InlineData("GET /plaintext HTTP/1.1\r\nN: V\r\n\r\n")]
        public void HttpParserBasicsRob(string requestText)
        {
            var parser = new HttpParser();
            var request = new Request();
            ReadOnlyBytes buffer = new ReadOnlyBytes(Encoding.ASCII.GetBytes(requestText));

            Assert.True(parser.ParseRequestLine(ref request, buffer, out var consumed));
            Assert.Equal(25, consumed);

            Assert.True(parser.ParseHeaders(ref request, buffer.Slice(consumed), out consumed));
            Assert.Equal(8, consumed);

            // request line
            Assert.Equal(Http.Method.Get, request.Method);
            Assert.Equal(Http.Version.Http11, request.Version);
            Assert.Equal("/plaintext", request.Path);

            // headers
            Assert.Equal(1, request.Headers.Count);
            Assert.True(request.Headers.ContainsKey("N"));
            Assert.Equal("V", request.Headers["N"]);
        }

        [Theory]
        [InlineData("GET /plaintext HTTP/1.1\r\nN: V\r\n\r\n")]
        public void HttpParserSegmentedRob(string requestText)
        {
            var parser = new HttpParser();

            for (int pivot = 26; pivot < requestText.Length; pivot++) {
                var front = requestText.Substring(0, pivot);
                var back = requestText.Substring(pivot);

                var frontBytes = Encoding.ASCII.GetBytes(front);
                var endBytes = Encoding.ASCII.GetBytes(back);

                var (first, last) = MemoryList.Create(frontBytes, endBytes);
                ReadOnlyBytes buffer = new ReadOnlyBytes(first, last);

                var request = new Request();

                try {
                    Assert.True(parser.ParseRequestLine(ref request, buffer, out var consumed));
                    Assert.Equal(25, consumed);

                    var unconsumed = buffer.Slice(consumed);
                    Assert.True(parser.ParseHeaders(ref request, unconsumed, out consumed));
                    Assert.Equal(8, consumed);
                }
                catch {
                    throw;
                }

                // request line
                Assert.Equal(Http.Method.Get, request.Method);
                Assert.Equal(Http.Version.Http11, request.Version);
                Assert.Equal("/plaintext", request.Path);

                // headers
                Assert.Equal(1, request.Headers.Count);
                Assert.True(request.Headers.ContainsKey("N"));
                Assert.Equal("V", request.Headers["N"]);
            }
        }

        [Fact]
        public void TechEmpowerRob()
        {
            var parser = new HttpParser();
            var request = new Request();
            ReadOnlyBytes buffer = new ReadOnlyBytes(_plaintextTechEmpowerRequestBytes);

            Assert.True(parser.ParseRequestLine(ref request, buffer, out var consumed));
            Assert.Equal(25, consumed);

            Assert.True(parser.ParseHeaders(ref request, buffer.Slice(consumed), out consumed));
            Assert.Equal(139, consumed);

            // request line
            Assert.Equal(Http.Method.Get, request.Method);
            Assert.Equal(Http.Version.Http11, request.Version);
            Assert.Equal("/plaintext", request.Path);

            // headers
            Assert.Equal(3, request.Headers.Count);
            Assert.True(request.Headers.ContainsKey("Host"));
            Assert.True(request.Headers.ContainsKey("Accept"));
            Assert.True(request.Headers.ContainsKey("Connection"));
            Assert.Equal("localhost", request.Headers["Host"]);
            Assert.Equal("text/plain,text/html;q=0.9,application/xhtml+xml;q=0.9,application/xml;q=0.8,*/*;q=0.7", request.Headers["Accept"]);
            Assert.Equal("keep-alive", request.Headers["Connection"]);
        }

        [Fact]
        public void TechEmpowerRobRb()
        {
            var parser = new HttpParser();
            var request = new Request();
            ReadableBuffer buffer = ReadableBuffer.Create(_plaintextTechEmpowerRequestBytes);

            Assert.True(parser.ParseRequestLine(request, buffer, out var consumed, out var read));
            Assert.True(parser.ParseHeaders(request, buffer.Slice(consumed), out consumed, out var examined, out var consumedBytes));
            Assert.Equal(139, consumedBytes);

            // request line
            Assert.Equal(Http.Method.Get, request.Method);
            Assert.Equal(Http.Version.Http11, request.Version);
            Assert.Equal("/plaintext", request.Path);

            // headers
            Assert.Equal(3, request.Headers.Count);
            Assert.True(request.Headers.ContainsKey("Host"));
            Assert.True(request.Headers.ContainsKey("Accept"));
            Assert.True(request.Headers.ContainsKey("Connection"));
            Assert.Equal("localhost", request.Headers["Host"]);
            Assert.Equal("text/plain,text/html;q=0.9,application/xhtml+xml;q=0.9,application/xml;q=0.8,*/*;q=0.7", request.Headers["Accept"]);
            Assert.Equal("keep-alive", request.Headers["Connection"]);
        }

        private const string _plaintextTechEmpowerRequest =
"GET /plaintext HTTP/1.1\r\n" +
"Host: localhost\r\n" +
"Accept: text/plain,text/html;q=0.9,application/xhtml+xml;q=0.9,application/xml;q=0.8,*/*;q=0.7\r\n" +
"Connection: keep-alive\r\n" +
"\r\n";
        byte[] _plaintextTechEmpowerRequestBytes = Encoding.ASCII.GetBytes(_plaintextTechEmpowerRequest);
    }

    class Request : IHttpHeadersHandler, IHttpRequestLineHandler
    {
        public Http.Method Method;
        public Http.Version Version;
        public string Path;
        public string Query;
        public string Target;

        public Dictionary<string, string> Headers = new Dictionary<string, string>();

        public void OnHeader(ReadOnlySpan<byte> name, ReadOnlySpan<byte> value)
        {
            var nameString = Encodings.Ascii.ToUtf16String(name);
            var valueString = Encodings.Ascii.ToUtf16String(value);
            Headers.Add(nameString, valueString);
        }

        public void OnStartLine(Http.Method method, Http.Version version, ReadOnlySpan<byte> target, ReadOnlySpan<byte> path, ReadOnlySpan<byte> query, ReadOnlySpan<byte> customMethod, bool pathEncoded)
        {
            Method = method;
            Version = version;
            Path = Encodings.Ascii.ToUtf16String(path);
            Query = Encodings.Ascii.ToUtf16String(query);
            Target = Encodings.Ascii.ToUtf16String(target);
        }
    }
}
