// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Xunit;
using System.Text.Utf8;
using System.Text.Http.SingleSegment;
using System.Buffers.Text;

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

        //[Fact(Skip = "System.TypeLoadException : The generic type 'System.Collections.Generic.KeyValuePair`2' was used with an invalid instantiation in assembly 'System.Private.CoreLib")]
        public void It_counts_the_number_of_headers_correctly()
        {
            var httpHeader = new HttpHeadersSingleSegment(new Span<byte>(new UTF8Encoding().GetBytes(HeadersString)));
            Assert.Equal(httpHeader.Count, 8);
        }

        //[Fact(Skip = "System.TypeLoadException : The generic type 'System.Collections.Generic.KeyValuePair`2' was used with an invalid instantiation in assembly 'System.Private.CoreLib")]
        public void It_can_get_the_value_of_a_particular_header()
        {
            var httpHeader = new HttpHeadersSingleSegment(new Span<byte>(new UTF8Encoding().GetBytes(HeadersString)));
            Assert.Equal(httpHeader["Host"].ToString(), " localhost:8080");
        }

        //[Fact(Skip = "System.TypeLoadException : The generic type 'System.Collections.Generic.KeyValuePair`2' was used with an invalid instantiation in assembly 'System.Private.CoreLib")]
        public void It_returns_empty_string_when_header_is_not_present()
        {
            var httpHeader = new HttpHeadersSingleSegment(new Span<byte>(new UTF8Encoding().GetBytes(HeadersString)));
            httpHeader["Content-Length"].Length.Should().Be(0);
        }

        //[Fact(Skip = "System.TypeLoadException : The generic type 'System.Collections.Generic.KeyValuePair`2' was used with an invalid instantiation in assembly 'System.Private.CoreLib")]
        public void Its_enumerator_Current_returns_the_same_item_until_MoveNext_gets_called()
        {
            var httpHeader = new HttpHeadersSingleSegment(new Span<byte>(new UTF8Encoding().GetBytes(HeadersString)));
            var enumerator = httpHeader.GetEnumerator();

            enumerator.MoveNext();

            var current = enumerator.Current;
            Assert.True(current.Key == enumerator.Current.Key);
            Assert.True(current.Value == enumerator.Current.Value);

            enumerator.MoveNext();

            current = enumerator.Current;
            Assert.True(current.Key == enumerator.Current.Key);
            Assert.True(current.Value == enumerator.Current.Value);
        }

        //[Fact(Skip = "System.TypeLoadException : The generic type 'System.Collections.Generic.KeyValuePair`2' was used with an invalid instantiation in assembly 'System.Private.CoreLib")]
        public void Its_Enumerator_iterates_through_all_headers()
        {
            var httpHeaders = new HttpHeadersSingleSegment(new Span<byte>(new UTF8Encoding().GetBytes(HeadersString)));
            var count = 0;
            foreach (var httpHeader in httpHeaders)
            {
                count++;
            }

            count.Should().Be(8);
        }

        //[Fact(Skip = "System.TypeLoadException : The generic type 'System.Collections.Generic.KeyValuePair`2' was used with an invalid instantiation in assembly 'System.Private.CoreLib")]
        public void It_parsers_Utf8String_as_well()
        {
            var httpHeader = new HttpHeadersSingleSegment(new Utf8String(new UTF8Encoding().GetBytes(HeadersString)));

            httpHeader.Count.Should().Be(8);
        }

        //[Fact(Skip = "System.TypeLoadException : The generic type 'System.Collections.Generic.KeyValuePair`2' was used with an invalid instantiation in assembly 'System.Private.CoreLib")]
        public void String_without_column_throws_ArgumentException()
        {
            var httpHeader = new HttpHeadersSingleSegment(new Utf8String(new UTF8Encoding().GetBytes(HeaderWithoutColumn)));

            try
            {
                var count = httpHeader.Count;
                Assert.True(false);
            }
            catch(Exception ex)
            {
                Assert.True(ex is ArgumentException);
            }
        }

        //[Fact(Skip = "System.TypeLoadException : The generic type 'System.Collections.Generic.KeyValuePair`2' was used with an invalid instantiation in assembly 'System.Private.CoreLib")]
        public void String_without_carriage_return_and_line_feed_throws_ArgumentException()
        {
            var httpHeader = new HttpHeadersSingleSegment(new Utf8String(new UTF8Encoding().GetBytes(HeaderWithoutCrlf)));

            try
            {
                var count = httpHeader.Count;
                Assert.True(false);
            }
            catch (Exception ex)
            {
                Assert.True(ex is ArgumentException);
            }
        }

        //[Fact(Skip = "System.TypeLoadException : The generic type 'System.Collections.Generic.KeyValuePair`2' was used with an invalid instantiation in assembly 'System.Private.CoreLib")]
        public void CanParseBodylessRequest()
        {
            var request = new Utf8String("GET / HTTP/1.1\r\nConnection: close\r\n\r\n").CopyBytes().AsSpan();
            var parsed = HttpRequestSingleSegment.Parse(request);
            Assert.Equal(HttpMethod.Get, parsed.RequestLine.Method);
            Assert.Equal(HttpVersion.V1_1, parsed.RequestLine.Version);
            Assert.Equal("/", parsed.RequestLine.RequestUri.ToString(SymbolTable.InvariantUtf8));
            Assert.Equal(1, parsed.Headers.Count);
            Assert.Equal(0, parsed.Body.Length);
        }
    }
}
