﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Xunit;
using System.Text.Utf8;
using System.Text.Http;
using System.Text.Http.SingleSegment;

namespace System.Text.Http.Tests
{
    public class GivenAnHttpHeaders
    {
        private const string HeadersString = "Host: localhost:8080"
            + "\r\nConnection: keep-alive"
            + "\r\nCache-Control: max-age=0"
            + "\r\nAccept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"
            + "\r\nUpgrade-Insecure-Requests: 1"
            + "\r\nUser-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.71 Safari/537.36"
            + "\r\nAccept-Encoding: gzip, deflate, sdch"
            + "\r\nAccept-Language: en-US,en;q=0.8,pt-BR;q=0.6,pt;q=0.4"
            + "\r\n";

        private const string HeaderWithoutColumn = "Connection keep-alive\r\n";

        private const string HeaderWithoutCrlf = "Host: localhost:8080";

        private HttpHeadersSingleSegment _httpHeaders;

        public GivenAnHttpHeaders()
        {
            Span<byte> headers;
            var bytes = new UTF8Encoding().GetBytes(HeadersString);

            unsafe
            {
                fixed (byte* buffer = bytes)
                {
                    headers = new Span<byte>(buffer, bytes.Length);
                }
            }

            _httpHeaders = new HttpHeadersSingleSegment(headers);
        }

        [Fact]
        public void It_counts_the_number_of_headers_correctly()
        {            
            _httpHeaders.Count.Should().Be(8);
        }

        [Fact]
        public void It_can_get_the_value_of_a_particular_header()
        {
            _httpHeaders["Host"].ToString().Should().Be(" localhost:8080");
        }

        [Fact]
        public void It_returns_empty_string_when_header_is_not_present()
        {
            _httpHeaders["Content-Length"].Length.Should().Be(0);
        }

        [Fact]
        public void Its_enumerator_Current_returns_the_same_item_until_MoveNext_gets_called()
        {
            var enumerator = _httpHeaders.GetEnumerator();

            enumerator.MoveNext();

            var current = enumerator.Current;
            current.Should().Be(enumerator.Current);

            enumerator.MoveNext();

            current.Should().NotBe(enumerator.Current);
        }

        [Fact]
        public void Its_Enumerator_iterates_through_all_headers()
        {
            var count = 0;
            foreach (var httpHeader in _httpHeaders)
            {
                count++;
            }

            count.Should().Be(8);
        }

        [Fact]
        public void It_parsers_Utf8String_as_well()
        {
            var httpHeader = new HttpHeadersSingleSegment(new Utf8String(new UTF8Encoding().GetBytes(HeadersString)));

            httpHeader.Count.Should().Be(8);
        }

        [Fact]
        public void String_without_column_throws_ArgumentException()
        {
            var httpHeader = new HttpHeadersSingleSegment(new Utf8String(new UTF8Encoding().GetBytes(HeaderWithoutColumn)));

            Action action = () => { var count = httpHeader.Count; }; 

            action.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void String_without_carriage_return_and_line_feed_throws_ArgumentException()
        {
            var httpHeader = new HttpHeadersSingleSegment(new Utf8String(new UTF8Encoding().GetBytes(HeaderWithoutCrlf)));

            Action action = () => { var count = httpHeader.Count; };

            action.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void CanParseBodylessRequest()
        {
            var request = new Utf8String("GET / HTTP/1.1\r\nConnection: close\r\n\r\n").CopyBytes().Slice();
            var parsed = HttpRequestSingleSegment.Parse(request);
            Assert.Equal(HttpMethod.Get, parsed.RequestLine.Method);
            Assert.Equal(HttpVersion.V1_1, parsed.RequestLine.Version);
            Assert.Equal("/", parsed.RequestLine.RequestUri.ToString(TextEncoder.Utf8));
            Assert.Equal(1, parsed.Headers.Count);
            Assert.Equal(0, parsed.Body.Length);
        } 
    }
}
