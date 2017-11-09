// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Collections.Sequences;
using Xunit;

namespace System.Text.Http.Tests
{
    public partial class HttpRequestTests
    {
        [Fact]
        public void HttpReaderSingleSegment()
        {
            var bytes = new ReadOnlyBytes(s_requestBytes);
            HttpRequest request = HttpRequest.Parse(bytes);

            Assert.Equal("GET", request.Verb.ToString(SymbolTable.InvariantUtf8));
            Assert.Equal("/developer/documentation/data-insertion/r-sample-http-get", request.Path.ToString(SymbolTable.InvariantUtf8));
            Assert.Equal("HTTP/1.1", request.Version.ToString(SymbolTable.InvariantUtf8));
            var headers = request.Headers.ToString();
            var body = bytes.Slice(request.BodyIndex).ToString(SymbolTable.InvariantUtf8);
            Assert.Equal("Hello World", body);

            var position = Position.First;
            while (request.Headers.TryGet(ref position, out HttpHeader header, true))
            {
                header.Deconstruct(out string name, out string value);
                if (name == "Connection") Assert.Equal("keep-alive", value);
            }
        }

        [Fact]
        public void HttpReaderMultipleSegments()
        {
            ReadOnlyBytes bytes = s_segmentedRequest;
            HttpRequest request = HttpRequest.Parse(bytes);

            Assert.Equal("GET", request.Verb.ToString(SymbolTable.InvariantUtf8));
            Assert.Equal("/developer/documentation/data-insertion/r-sample-http-get", request.Path.ToString(SymbolTable.InvariantUtf8));
            Assert.Equal("HTTP/1.1", request.Version.ToString(SymbolTable.InvariantUtf8));
            var headers = request.Headers.ToString();
            var body = bytes.Slice(request.BodyIndex).ToString(SymbolTable.InvariantUtf8);
            Assert.Equal("Hello World", body);

            var position = new Position();
            while (request.Headers.TryGet(ref position, out HttpHeader header, true))
            {
                header.Deconstruct(out string name, out string value);
                if (name == "Connection") Assert.Equal("keep-alive", value);
            }
        }

        #region Test Data
        ReadOnlyBytes s_segmentedRequest = Parse(s_requestStringSegmented);
        private static ReadOnlyBytes Parse(string text)
        {
            var segments = text.Split('|');
            var buffers = new List<byte[]>();
            foreach (var segment in segments)
            {
                buffers.Add(Encoding.UTF8.GetBytes(segment));
            }
            return ReadOnlyBytes.Create(buffers.ToArray());
        }

        static string s_requestString = "GET /developer/documentation/data-insertion/r-sample-http-get HTTP/1.1" +
        "\r\nHost: marketing.adobe.com" +
        "\r\nConnection: keep-alive" +
        "\r\nCache-Control: max-age=0" +
        "\r\nUpgrade-Insecure-Requests: 1" +
        "\r\nUser-Agent: corfxlab_pipleline" +
        "\r\n" +
        "\r\nHello World";

        static string s_requestStringSegmented = "GE|T /developer/documen|tation/data-insertion/r-sample-http-get HT|TP/1.1" +
        "\r\nHost: marketing.adobe.com" +
        "\r\nConnection: keep-alive" +
        "\r\nCache-Control: max-age=0" +
        "\r\nUpgrade-Insecure-Requests: 1" +
        "\r\nUser-Agent: corfxlab_pipleline" +
        "\r\n" +
        "\r\nHello World";

        static byte[] s_requestBytes = Encoding.UTF8.GetBytes(s_requestString);
        #endregion
    }
}
