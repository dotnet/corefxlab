// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using System.Text.Formatting;
using System.Text.Http.Formatter;
using System.Text.Utf8;
using Xunit;

namespace System.Text.Http.Tests
{
    public class GivenIFormatterExtensionsForHttp
    {
        private const string HttpBody = "Body Part1";

        private const string HttpMessage =
            "HTTP/1.1 200 OK\r\nConnection : open\r\n\r\nBody Part1\r\nBody Part1";

        private readonly byte[] _statusLineInBytes = Utf8Encoding.GetBytes("HTTP/1.1 200 OK\r\n");
        private readonly byte[] _headerInBytes = Utf8Encoding.GetBytes("Connection : close\r\n");
        private readonly byte[] _updatedHeaderInBytes = Utf8Encoding.GetBytes("Connection : open \r\n");
        private readonly byte[] _httpHeaderSectionDelineatorInBytes = Utf8Encoding.GetBytes("\r\n");
        private readonly byte[] _httpBodyInBytes = Utf8Encoding.GetBytes(HttpBody);
        private readonly byte[] _httpMessageInBytes = Utf8Encoding.GetBytes(HttpMessage);

        private ArrayFormatter _formatter;
        private static readonly UTF8Encoding Utf8Encoding = new UTF8Encoding();

        public GivenIFormatterExtensionsForHttp()
        {
            _formatter = new ArrayFormatter(124, SymbolTable.InvariantUtf8, ArrayPool<byte>.Shared);
        }

        [Fact]
        public void It_has_an_extension_method_to_write_status_line()
        {
            _formatter.AppendHttpStatusLine(Parser.Http.Version.Http11, 200, new Utf8Span("OK"));

            var result = _formatter.Formatted;

            result.AsSpan().SequenceEqual(_statusLineInBytes);
            _formatter.Clear();
        }

        [Fact]
        public void The_http_extension_methods_can_be_composed_to_generate_the_http_message()
        {
            _formatter.AppendHttpStatusLine(Parser.Http.Version.Http11, 200, new Utf8Span("OK"));
            _formatter.Append(new Utf8Span("Connection : open"));
            _formatter.AppendHttpNewLine();
            _formatter.AppendHttpNewLine();
            _formatter.Append(HttpBody);
            _formatter.AppendHttpNewLine();
            _formatter.Append(HttpBody);

            var result = _formatter.Formatted;

            result.AsSpan().SequenceEqual(_httpMessageInBytes);
            _formatter.Clear();
        }

        static string s_expectedRequest = "GET /path HTTP/1.1\r\nheader1:header_value1\r\nheader2:header_value2\r\nx-ms-date:Tue, 15 Aug 2017 01:02:03 GMT\r\nheader3:3456\r\n\r\n";
        [Fact]
        public void ComposeHttpRequest()
        {
            _formatter.AppendHttpRequestLine(Parser.Http.Method.Get, Parser.Http.Version.Http11, "/path");

            _formatter.AppendHttpHeader("header1:", "header_value1");
            _formatter.AppendHttpHeader("header2:", "header_value2");
            _formatter.AppendHttpHeader("x-ms-date:", new DateTime(2017, 8, 15, 1, 2, 3, 4));
            _formatter.AppendHttpHeader("header3:", 3456);
            _formatter.AppendHttpNewLine();

            var result = _formatter.Formatted;
            var text = Encoding.UTF8.GetString(result.ToArray());
            Assert.Equal(s_expectedRequest, text);
            _formatter.Clear();
        }
    }
}
