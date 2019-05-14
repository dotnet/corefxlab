// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Tests;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace System.Text.Http.Parser.Tests
{
    public class HttpParserTests
    {
        [Theory]
        [MemberData(nameof(RequestLineValidData))]
        public void ParsesRequestLine(
            string requestLine,
            string expectedMethod,
            string expectedRawTarget,
            string expectedRawPath,
            string expectedDecodedPath,
            string expectedQueryString,
            string expectedVersion)
        {
            var parser = new HttpParser();
            var buffer = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes(requestLine));
            var requestHandler = new RequestHandler();

            Assert.True(parser.ParseRequestLine(requestHandler, buffer, out var consumed));

            Assert.Equal(requestHandler.Method, expectedMethod);
            Assert.Equal(requestHandler.Version, expectedVersion);
            Assert.Equal(requestHandler.RawTarget, expectedRawTarget);
            Assert.Equal(requestHandler.RawPath, expectedRawPath);
            Assert.Equal(requestHandler.Version, expectedVersion);
        }

        [Theory]
        [MemberData(nameof(RequestLineIncompleteData))]
        public void ParseRequestLineReturnsFalseWhenGivenIncompleteRequestLines(string requestLine)
        {
            var parser = new HttpParser();
            var buffer = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes(requestLine));
            var requestHandler = new RequestHandler();

            Assert.False(parser.ParseRequestLine(requestHandler, buffer, out var consumed));
        }

        [Theory]
        [MemberData(nameof(RequestLineIncompleteData))]
        public void ParseRequestLineDoesNotConsumeIncompleteRequestLine(string requestLine)
        {
            var parser = new HttpParser();
            var buffer = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes(requestLine));
            var requestHandler = new RequestHandler();

            Assert.False(parser.ParseRequestLine(requestHandler, buffer, out var consumed));
        }

        [Theory]
        [MemberData(nameof(RequestLineInvalidData))]
        public void ParseRequestLineThrowsOnInvalidRequestLine(string requestLine)
        {
            var parser = new HttpParser();
            var buffer = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes(requestLine));
            var requestHandler = new RequestHandler();

            var exception = Assert.Throws<BadHttpRequestException>(() =>
                parser.ParseRequestLine(requestHandler, buffer, out var consumed));

            //Assert.Equal($"Invalid request line: '{requestLine.EscapeNonPrintable()}'", exception.Message);
            //Assert.Equal(StatusCodes.Status400BadRequest, (exception as BadHttpRequestException).StatusCode);
        }

        [Theory]
        [MemberData(nameof(MethodWithNonTokenCharData))]
        public void ParseRequestLineThrowsOnNonTokenCharsInCustomMethod(string method)
        {
            var requestLine = $"{method} / HTTP/1.1\r\n";

            var parser = new HttpParser();
            var buffer = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes(requestLine));
            var requestHandler = new RequestHandler();

            var exception = Assert.Throws<BadHttpRequestException>(() =>
                parser.ParseRequestLine(requestHandler, buffer, out var consumed));

            //Assert.Equal($"Invalid request line: '{method.EscapeNonPrintable()} / HTTP/1.1\\x0D\\x0A'", exception.Message);
            //Assert.Equal(StatusCodes.Status400BadRequest, (exception as BadHttpRequestException).StatusCode);
        }

        [Theory]
        [MemberData(nameof(UnrecognizedHttpVersionData))]
        public void ParseRequestLineThrowsOnUnrecognizedHttpVersion(string httpVersion)
        {
            var requestLine = $"GET / {httpVersion}\r\n";

            var parser = new HttpParser();
            var buffer = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes(requestLine));
            var requestHandler = new RequestHandler();

            var exception = Assert.Throws<BadHttpRequestException>(() =>
                parser.ParseRequestLine(requestHandler, buffer, out var consumed));

            //Assert.Equal($"Unrecognized HTTP version: '{httpVersion}'", exception.Message);
            //Assert.Equal(StatusCodes.Status505HttpVersionNotsupported, (exception as BadHttpRequestException).StatusCode);
        }

        [Theory]
        [InlineData("\r")]
        [InlineData("H")]
        [InlineData("He")]
        [InlineData("Hea")]
        [InlineData("Head")]
        [InlineData("Heade")]
        [InlineData("Header")]
        [InlineData("Header:")]
        [InlineData("Header: ")]
        [InlineData("Header: v")]
        [InlineData("Header: va")]
        [InlineData("Header: val")]
        [InlineData("Header: valu")]
        [InlineData("Header: value")]
        [InlineData("Header: value\r")]
        [InlineData("Header: value\r\n")]
        [InlineData("Header: value\r\n\r")]
        [InlineData("Header-1: value1\r\nH")]
        [InlineData("Header-1: value1\r\nHe")]
        [InlineData("Header-1: value1\r\nHea")]
        [InlineData("Header-1: value1\r\nHead")]
        [InlineData("Header-1: value1\r\nHeade")]
        [InlineData("Header-1: value1\r\nHeader")]
        [InlineData("Header-1: value1\r\nHeader-")]
        [InlineData("Header-1: value1\r\nHeader-2")]
        [InlineData("Header-1: value1\r\nHeader-2:")]
        [InlineData("Header-1: value1\r\nHeader-2: ")]
        [InlineData("Header-1: value1\r\nHeader-2: v")]
        [InlineData("Header-1: value1\r\nHeader-2: va")]
        [InlineData("Header-1: value1\r\nHeader-2: val")]
        [InlineData("Header-1: value1\r\nHeader-2: valu")]
        [InlineData("Header-1: value1\r\nHeader-2: value")]
        [InlineData("Header-1: value1\r\nHeader-2: value2")]
        [InlineData("Header-1: value1\r\nHeader-2: value2\r")]
        [InlineData("Header-1: value1\r\nHeader-2: value2\r\n")]
        [InlineData("Header-1: value1\r\nHeader-2: value2\r\n\r")]
        public void ParseHeadersReturnsFalseWhenGivenIncompleteHeaders(string rawHeaders)
        {
            var parser = new HttpParser();

            var buffer = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes(rawHeaders));
            var requestHandler = new RequestHandler();
            Assert.False(parser.ParseHeaders(requestHandler, buffer, out var consumed));
        }

        [Theory]
        [InlineData("\r")]
        [InlineData("H")]
        [InlineData("He")]
        [InlineData("Hea")]
        [InlineData("Head")]
        [InlineData("Heade")]
        [InlineData("Header")]
        [InlineData("Header:")]
        [InlineData("Header: ")]
        [InlineData("Header: v")]
        [InlineData("Header: va")]
        [InlineData("Header: val")]
        [InlineData("Header: valu")]
        [InlineData("Header: value")]
        [InlineData("Header: value\r")]
        public void ParseHeadersDoesNotConsumeIncompleteHeader(string rawHeaders)
        {
            var parser = new HttpParser();

            var buffer = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes(rawHeaders));
            var requestHandler = new RequestHandler();
            parser.ParseHeaders(requestHandler, buffer, out var consumed);

            Assert.Equal(0, consumed);
        }

        [Fact]
        public void ParseHeadersCanReadHeaderValueWithoutLeadingWhitespace()
        {
            VerifyHeader("Header", "value", "value");
        }

        [Theory]
        [InlineData("Cookie: \r\n\r\n", "Cookie", "", null, null)]
        [InlineData("Cookie:\r\n\r\n", "Cookie", "", null, null)]
        [InlineData("Cookie: \r\nConnection: close\r\n\r\n", "Cookie", "", "Connection", "close")]
        [InlineData("Cookie:\r\nConnection: close\r\n\r\n", "Cookie", "", "Connection", "close")]
        [InlineData("Connection: close\r\nCookie: \r\n\r\n", "Connection", "close", "Cookie", "")]
        [InlineData("Connection: close\r\nCookie:\r\n\r\n", "Connection", "close", "Cookie", "")]
        public void ParseHeadersCanParseEmptyHeaderValues(
            string rawHeaders,
            string expectedHeaderName1,
            string expectedHeaderValue1,
            string expectedHeaderName2,
            string expectedHeaderValue2)
        {
            var expectedHeaderNames = expectedHeaderName2 == null
                ? new[] { expectedHeaderName1 }
                : new[] { expectedHeaderName1, expectedHeaderName2 };
            var expectedHeaderValues = expectedHeaderValue2 == null
                ? new[] { expectedHeaderValue1 }
                : new[] { expectedHeaderValue1, expectedHeaderValue2 };

            VerifyRawHeaders(rawHeaders, expectedHeaderNames, expectedHeaderValues);
        }

        [Theory]
        [InlineData(" value")]
        [InlineData("  value")]
        [InlineData("\tvalue")]
        [InlineData(" \tvalue")]
        [InlineData("\t value")]
        [InlineData("\t\tvalue")]
        [InlineData("\t\t value")]
        [InlineData(" \t\tvalue")]
        [InlineData(" \t\t value")]
        [InlineData(" \t \t value")]
        public void ParseHeadersDoesNotIncludeLeadingWhitespaceInHeaderValue(string rawHeaderValue)
        {
            VerifyHeader("Header", rawHeaderValue, "value");
        }

        [Theory]
        [InlineData("value ")]
        [InlineData("value\t")]
        [InlineData("value \t")]
        [InlineData("value\t ")]
        [InlineData("value\t\t")]
        [InlineData("value\t\t ")]
        [InlineData("value \t\t")]
        [InlineData("value \t\t ")]
        [InlineData("value \t \t ")]
        public void ParseHeadersDoesNotIncludeTrailingWhitespaceInHeaderValue(string rawHeaderValue)
        {
            VerifyHeader("Header", rawHeaderValue, "value");
        }

        [Theory]
        [InlineData("one two three")]
        [InlineData("one  two  three")]
        [InlineData("one\ttwo\tthree")]
        [InlineData("one two\tthree")]
        [InlineData("one\ttwo three")]
        [InlineData("one \ttwo \tthree")]
        [InlineData("one\t two\t three")]
        [InlineData("one \ttwo\t three")]
        public void ParseHeadersPreservesWhitespaceWithinHeaderValue(string headerValue)
        {
            VerifyHeader("Header", headerValue, headerValue);
        }

        [Fact(Skip = "Why would parser return false and non-zero consumed?")]
        public void ParseHeadersConsumesBytesCorrectlyAtEnd()
        {
            var parser = new HttpParser();

            const string headerLine = "Header: value\r\n\r";
            var buffer = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes(headerLine));
            var requestHandler = new RequestHandler();
            Assert.False(parser.ParseHeaders(requestHandler, buffer, out var consumed));

            Assert.Equal(headerLine.Length - 1, consumed);

            var buffer2 = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("\r\n"));
            Assert.True(parser.ParseHeaders(requestHandler, buffer2, out consumed));

            Assert.Equal(2, consumed);
        }

        [Theory]
        [MemberData(nameof(RequestHeaderInvalidData))]
        public void ParseHeadersThrowsOnInvalidRequestHeadersRb(string rawHeaders, string expectedExceptionMessage)
        {
            var parser = new HttpParser();
            var buffer = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes(rawHeaders));
            var requestHandler = new RequestHandler();

            var exception = Assert.Throws<BadHttpRequestException>(() =>
                parser.ParseHeaders(requestHandler, buffer, out var consumed, out var examined, out var consumedBytes));

            //Assert.Equal(expectedExceptionMessage, exception.Message);
            //Assert.Equal(StatusCodes.Status400BadRequest, exception.StatusCode);
        }

        [Theory]
        [MemberData(nameof(RequestHeaderInvalidData))]
        public void ParseHeadersThrowsOnInvalidRequestHeaders(string rawHeaders, string expectedExceptionMessage)
        {
            var parser = new HttpParser();
            var buffer = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes(rawHeaders));
            var requestHandler = new RequestHandler();

            var exception = Assert.Throws<BadHttpRequestException>(() =>
                parser.ParseHeaders(requestHandler, buffer, out var consumed));

            //Assert.Equal(expectedExceptionMessage, exception.Message);
            //Assert.Equal(StatusCodes.Status400BadRequest, exception.StatusCode);
        }

        [Fact]
        public void ParseRequestLineSplitBufferWithoutNewLineDoesNotUpdateConsumed()
        {
            HttpParser parser = new HttpParser();

            ReadOnlySequence<byte> buffer = BufferUtilities.CreateUtf8Buffer("GET ", "/");
            RequestHandler requestHandler = new RequestHandler();

            bool result = parser.ParseRequestLine(requestHandler, buffer, out SequencePosition consumed, out SequencePosition examined);
            Assert.False(result);
            Assert.Equal(buffer.Slice(consumed).Length, buffer.Length);
            Assert.True(buffer.Slice(examined).IsEmpty);
        }

        //[Fact]
        //public void ExceptionDetailNotIncludedWhenLogLevelInformationNotEnabled()
        //{
        //    var mockTrace = new HttpParser();
        //    mockTrace
        //        .Setup(trace => trace.IsEnabled(LogLevel.Information))
        //        .Returns(false);

        //    var parser = new HttpParser();

        //    // Invalid request line
        //    var buffer = ReadableBuffer.Create(Encoding.ASCII.GetBytes("GET % HTTP/1.1\r\n"));
        //    var requestHandler = new RequestHandler();

        //    var exception = Assert.Throws<BadHttpRequestException>(() =>
        //        parser.ParseRequestLine(requestHandler, buffer, out var consumed, out var examined));

        //    Assert.Equal("Invalid request line: ''", exception.Message);
        //    Assert.Equal(StatusCodes.Status400BadRequest, (exception as BadHttpRequestException).StatusCode);

        //    // Unrecognized HTTP version
        //    buffer = ReadableBuffer.Create(Encoding.ASCII.GetBytes("GET / HTTP/1.2\r\n"));

        //    exception = Assert.Throws<BadHttpRequestException>(() =>
        //        parser.ParseRequestLine(requestHandler, buffer, out var consumed, out var examined));

        //    Assert.Equal("Unrecognized HTTP version: ''", exception.Message);
        //    Assert.Equal(StatusCodes.Status505HttpVersionNotsupported, (exception as BadHttpRequestException).StatusCode);

        //    // Invalid request header
        //    buffer = ReadableBuffer.Create(Encoding.ASCII.GetBytes("Header: value\n\r\n"));

        //    exception = Assert.Throws<BadHttpRequestException>(() =>
        //        parser.ParseHeaders(requestHandler, buffer, out var consumed, out var examined, out var consumedBytes));

        //    Assert.Equal("Invalid request header: ''", exception.Message);
        //    Assert.Equal(StatusCodes.Status400BadRequest, exception.StatusCode);
        //}

        private void VerifyHeader(
            string headerName,
            string rawHeaderValue,
            string expectedHeaderValue)
        {
            var parser = new HttpParser();
            var buffer = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes($"{headerName}:{rawHeaderValue}\r\n"));

            var requestHandler = new RequestHandler();
            parser.ParseHeaders(requestHandler, buffer, out var consumed, out var examined, out var consumedBytes);

            var pairs = requestHandler.Headers.ToArray();
            Assert.Equal(1, pairs.Length);
            Assert.Equal(headerName, pairs[0].Key);
            Assert.Equal(expectedHeaderValue, pairs[0].Value);
            Assert.True(buffer.Slice(consumed).IsEmpty);
            Assert.True(buffer.Slice(examined).IsEmpty);
        }

        private void VerifyRawHeaders(string rawHeaders, IEnumerable<string> expectedHeaderNames, IEnumerable<string> expectedHeaderValues)
        {
            Assert.True(expectedHeaderNames.Count() == expectedHeaderValues.Count(), $"{nameof(expectedHeaderNames)} and {nameof(expectedHeaderValues)} sizes must match");

            var parser = new HttpParser();
            var buffer = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes(rawHeaders));

            var requestHandler = new RequestHandler();
            parser.ParseHeaders(requestHandler, buffer, out var consumed, out var examined, out var consumedBytes);

            var parsedHeaders = requestHandler.Headers.ToArray();

            Assert.Equal(expectedHeaderNames.Count(), parsedHeaders.Length);
            Assert.Equal(expectedHeaderNames, parsedHeaders.Select(t => t.Key));
            Assert.Equal(expectedHeaderValues, parsedHeaders.Select(t => t.Value));
            Assert.True(buffer.Slice(consumed).IsEmpty);
            Assert.True(buffer.Slice(examined).IsEmpty);
        }

        public static IEnumerable<string[]> RequestLineValidData => HttpParsingData.RequestLineValidData;

        public static IEnumerable<object[]> RequestLineIncompleteData => HttpParsingData.RequestLineIncompleteData.Select(requestLine => new[] { requestLine });

        public static IEnumerable<object[]> RequestLineInvalidData => HttpParsingData.RequestLineInvalidData.Select(requestLine => new[] { requestLine });

        public static IEnumerable<object[]> MethodWithNonTokenCharData => HttpParsingData.MethodWithNonTokenCharData.Select(method => new[] { method });

        public static TheoryData<string> UnrecognizedHttpVersionData => HttpParsingData.UnrecognizedHttpVersionData;

        public static IEnumerable<object[]> RequestHeaderInvalidData => HttpParsingData.RequestHeaderInvalidData;

        private class RequestHandler : IHttpRequestLineHandler, IHttpHeadersHandler
        {
            public string Method { get; set; }

            public string Version { get; set; }

            public string RawTarget { get; set; }

            public string RawPath { get; set; }

            public string Query { get; set; }

            public bool PathEncoded { get; set; }

            public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();

            public void OnHeader(ReadOnlySpan<byte> name, ReadOnlySpan<byte> value)
            {
                Headers[TextEncodings.Ascii.ToUtf16String(name)] = TextEncodings.Ascii.ToUtf16String(value);
            }

            public void OnStartLine(Http.Method method, Http.Version version, ReadOnlySpan<byte> target, ReadOnlySpan<byte> path, ReadOnlySpan<byte> query, ReadOnlySpan<byte> customMethod, bool pathEncoded)
            {
                Method = method != Http.Method.Custom ? method.ToString().ToUpper() : TextEncodings.Ascii.ToUtf16String(customMethod);
                Version = ToString(version);
                RawTarget = TextEncodings.Ascii.ToUtf16String(target);
                RawPath = TextEncodings.Ascii.ToUtf16String(path);
                Query = TextEncodings.Ascii.ToUtf16String(query);
                PathEncoded = pathEncoded;
            }

            private string ToString(Http.Version version)
            {
                switch (version)
                {
                    case Http.Version.Http10: return "HTTP/1.0";
                    case Http.Version.Http11: return "HTTP/1.1";
                    case Http.Version.Unknown: return "Unknown";
                    default: throw new NotImplementedException();
                }
            }
        }
    }
}
