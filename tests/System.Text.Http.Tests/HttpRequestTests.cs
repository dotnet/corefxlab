// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Collections.Generic;
using System.Collections.Sequences;
using System.Text;
using System.Text.Http;
using System.Text.Utf8;
using Xunit;

namespace System.Slices.Tests
{
    public partial class HttpRequestTests
    {
        [Fact]
        public void HttpReaderSingleSegment()
        {
            var bytes = new ReadOnlyBytes(s_requestBytes);
            HttpRequest request = HttpRequest.Parse(bytes);

            Assert.Equal("GET", request.Verb.ToString(TextEncoder.InvariantUtf8));
            Assert.Equal("/developer/documentation/data-insertion/r-sample-http-get", request.Path.ToString(TextEncoder.InvariantUtf8));
            Assert.Equal("HTTP/1.1", request.Version.ToString(TextEncoder.InvariantUtf8));
            var headers = request.Headers.ToString();
            var body = bytes.Slice(request.BodyIndex).ToString(TextEncoder.InvariantUtf8);
            Assert.Equal("Hello World", body);

            HttpHeader header;
            var position = Position.First;
            while (request.Headers.TryGet(ref position, out header, true))
            {
                string name;
                string value;
                header.Deconstruct(out name, out value);
                if (name == "Connection") Assert.Equal("keep-alive", value);
            }
        }

        [Fact]
        public void HttpReaderMultipleSegments()
        {
            ReadOnlyBytes bytes = s_segmentedRequest;
            HttpRequest request = HttpRequest.Parse(bytes);

            Assert.Equal("GET", request.Verb.ToString(TextEncoder.InvariantUtf8));
            Assert.Equal("/developer/documentation/data-insertion/r-sample-http-get", request.Path.ToString(TextEncoder.InvariantUtf8));
            Assert.Equal("HTTP/1.1", request.Version.ToString(TextEncoder.InvariantUtf8));
            var headers = request.Headers.ToString();
            var body = bytes.Slice(request.BodyIndex).ToString(TextEncoder.InvariantUtf8);
            Assert.Equal("Hello World", body);

            HttpHeader header;
            var position = new Position();
            while (request.Headers.TryGet(ref position, out header, true))
            {
                string name;
                string value;
                header.Deconstruct(out name, out value);
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

        static byte[] s_requestBytes = Encoding.UTF8.GetBytes(@"GET /developer/documentation/data-insertion/r-sample-http-get HTTP/1.1
Host: marketing.adobe.com
Connection: keep-alive
Cache-Control: max-age=0
Upgrade-Insecure-Requests: 1
User-Agent: corfxlab_pipleline

Hello World");

        static string s_requestStringSegmented = @"GE|T /developer/documen|tation/data-insertion/r-sample-http-get HT|TP/1.1
Host: marketing.adobe.com
Connection: keep-alive
Cache-Control: max-age=0
Upgrade-Insecure-Requests: 1
User-Agent: corfxlab_pipleline

Hello World";
        #endregion
    }
}