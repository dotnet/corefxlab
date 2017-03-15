// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Text.Formatting;
using System.Text;
using System.Text.Utf8;
using FluentAssertions;
using Xunit;
using System.Text.Http.SingleSegment;

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
            _formatter = new ArrayFormatter(124, TextEncoder.Utf8, ArrayPool<byte>.Shared);
        }

        [Fact]
        public void It_has_an_extension_method_to_write_status_line()
        {            
            _formatter.AppendHttpStatusLine(HttpVersion.V1_1, 200, new Utf8String("OK"));

            var result = _formatter.Formatted;

            result.Slice().SequenceEqual(_statusLineInBytes);
            _formatter.Clear();
        }

        [Fact]
        public void The_http_extension_methods_can_be_composed_to_generate_the_http_message()
        {
            _formatter.AppendHttpStatusLine(HttpVersion.V1_1, 200, new Utf8String("OK"));
            _formatter.Append(new Utf8String("Connection : open"));
            _formatter.AppendHttpNewLine();
            _formatter.AppendHttpNewLine();
            _formatter.Append(HttpBody);
            _formatter.AppendHttpNewLine();
            _formatter.Append(HttpBody);

            var result = _formatter.Formatted;

            result.Slice().SequenceEqual(_httpMessageInBytes);
            _formatter.Clear();
        }
    }
}
