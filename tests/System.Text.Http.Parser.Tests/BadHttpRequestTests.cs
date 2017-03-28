// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Xunit;
using System.Text.Http.Parser;
using System.Buffers;

namespace System.Text.Http.Parser.Tests
{
    public class BadHttpRequestTests
    {
        [Theory]
        [MemberData(nameof(InvalidRequestLineData))]
        public void TestInvalidRequestLines(string request, string expectedExceptionMessage)
        {
            TestBadRequest(
                request,
                "400 Bad Request",
                expectedExceptionMessage);
        }

        [Theory]
        [MemberData(nameof(UnrecognizedHttpVersionData))]
        public void TestInvalidRequestLinesWithUnrecognizedVersion(string httpVersion)
        {
            TestBadRequest(
                $"GET / {httpVersion}\r\n",
                "505 HTTP Version Not Supported",
                $"Unrecognized HTTP version: '{httpVersion}'");
        }

        [Theory]
        [MemberData(nameof(InvalidRequestHeaderData))]
        public void TestInvalidHeaders(string rawHeaders, string expectedExceptionMessage)
        {
            TestBadRequest(
                $"GET / HTTP/1.1\r\n{rawHeaders}",
                "400 Bad Request",
                expectedExceptionMessage);
        }

        [Theory(Skip = "I think this validation is done above the HTTP parser, but it needs to be verified")]
        [InlineData("Hea\0der: value", "Invalid characters in header name.")]
        [InlineData("Header: va\0lue", "Malformed request: invalid headers.")]
        [InlineData("Head\x80r: value", "Invalid characters in header name.")]
        [InlineData("Header: valu\x80", "Malformed request: invalid headers.")]
        public void BadRequestWhenHeaderNameContainsNonASCIIOrNullCharacters(string header, string expectedExceptionMessage)
        {
            TestBadRequest(
                $"GET / HTTP/1.1\r\n{header}\r\n\r\n",
                "400 Bad Request",
                expectedExceptionMessage);
        }

        private void TestBadRequest(string request, string expectedResponseStatusCode, string expectedExceptionMessage, string expectedAllowHeader = null)
        {
            HttpParser parser = new HttpParser();
            var parsed = new Request();
            var rob = new ReadOnlyBytes(Encoding.ASCII.GetBytes(request));
            try {
                parser.ParseRequestLine(parsed, rob, out var consumed);
                parser.ParseHeaders(parsed, rob.Slice(consumed), out consumed);
            }
            catch(BadHttpRequestException e) {
                //Assert.Equal(expectedExceptionMessage, e.Message);
                return;
            }

            Assert.True(false); // should never get here
        }

        //private async Task ReceiveBadRequestResponse(TestConnection connection, string expectedResponseStatusCode, string expectedDateHeaderValue, string expectedAllowHeader = null)
        //{
        //    var lines = new[]
        //    {
        //        $"HTTP/1.1 {expectedResponseStatusCode}",
        //        "Connection: close",
        //        $"Date: {expectedDateHeaderValue}",
        //        "Content-Length: 0",
        //        expectedAllowHeader,
        //        "",
        //        ""
        //    };

        //    await connection.ReceiveForcedEnd(lines.Where(f => f != null).ToArray());
        //}

        public static TheoryData<string, string> InvalidRequestLineData
        {
            get {
                var data = new TheoryData<string, string>();

                foreach (var requestLine in HttpParsingData.RequestLineInvalidData) {
                    data.Add(requestLine, $"Invalid request line: '{requestLine.EscapeNonPrintable()}'");
                }

                foreach (var target in HttpParsingData.TargetWithEncodedNullCharData) {
                    data.Add($"GET {target} HTTP/1.1\r\n", $"Invalid request target: '{target.EscapeNonPrintable()}'");
                }

                foreach (var target in HttpParsingData.TargetWithNullCharData) {
                    data.Add($"GET {target} HTTP/1.1\r\n", $"Invalid request target: '{target.EscapeNonPrintable()}'");
                }

                return data;
            }
        }

        public static TheoryData<string> UnrecognizedHttpVersionData => HttpParsingData.UnrecognizedHttpVersionData;

        public static IEnumerable<object[]> InvalidRequestHeaderData => HttpParsingData.RequestHeaderInvalidData;
    }
}
