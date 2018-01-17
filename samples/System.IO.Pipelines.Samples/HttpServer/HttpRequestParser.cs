// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Sequences;
using System.IO.Pipelines.Samples.Http;
using System.IO.Pipelines.Text.Primitives;

namespace System.IO.Pipelines.Samples
{
    public class HttpRequestParser
    {
        private ParsingState _state;

        private byte[] _httpVersion;
        private byte[] _path;
        private byte[] _method;

        public ReadOnlyBuffer<byte> HttpVersion => new ReadOnlyBuffer<byte>(_httpVersion);
        public ReadOnlyBuffer<byte> Path => new ReadOnlyBuffer<byte>(_path);
        public ReadOnlyBuffer<byte> Method => new ReadOnlyBuffer<byte>(_method);

        public RequestHeaderDictionary RequestHeaders = new RequestHeaderDictionary();

        public ParseResult ParseRequest(ReadOnlyBuffer<byte> buffer, out SequencePosition consumed, out SequencePosition examined)
        {
            consumed = buffer.Start;
            examined = buffer.Start;

            if (_state == ParsingState.StartLine)
            {
                if (!buffer.TrySliceTo((byte)'\r', (byte)'\n', out ReadOnlyBuffer<byte> startLine, out SequencePosition delim))
                {
                    return ParseResult.Incomplete;
                }

                // Move the buffer to the rest
                buffer = buffer.Slice(delim).Slice(2);

                if (!startLine.TrySliceTo((byte)' ', out ReadOnlyBuffer<byte> method, out delim))
                {
                    return ParseResult.BadRequest;
                }

                _method = method.ToArray();

                // Skip ' '
                startLine = startLine.Slice(delim).Slice(1);

                if (!startLine.TrySliceTo((byte)' ', out ReadOnlyBuffer<byte> path, out delim))
                {
                    return ParseResult.BadRequest;
                }

                _path = path.ToArray();

                // Skip ' '
                startLine = startLine.Slice(delim).Slice(1);

                var httpVersion = startLine;
                if (httpVersion.IsEmpty)
                {
                    return ParseResult.BadRequest;
                }

                _httpVersion = httpVersion.ToArray();

                _state = ParsingState.Headers;
                consumed = buffer.Start;
                examined = buffer.Start;
            }

            // Parse headers
            // key: value\r\n

            while (!buffer.IsEmpty)
            {
                var headerValue = default(ReadOnlyBuffer<byte>);
                if (!buffer.TrySliceTo((byte)'\r', (byte)'\n', out ReadOnlyBuffer<byte> headerPair, out SequencePosition delim))
                {
                    return ParseResult.Incomplete;
                }

                buffer = buffer.Slice(delim).Slice(2);

                consumed = buffer.Start;
                examined = buffer.Start;

                // End of headers
                if (headerPair.IsEmpty)
                {
                    return ParseResult.Complete;
                }

                // :
                if (!headerPair.TrySliceTo((byte)':', out ReadOnlyBuffer<byte> headerName, out delim))
                {
                    return ParseResult.BadRequest;
                }

                headerName = headerName.TrimStart();
                headerPair = headerPair.Slice(delim).Slice(1);

                headerValue = headerPair.TrimStart();
                RequestHeaders.SetHeader(ref headerName, ref headerValue);
            }

            return ParseResult.Incomplete;
        }

        public void Reset()
        {
            _state = ParsingState.StartLine;

            RequestHeaders.Reset();
        }

        public enum ParseResult
        {
            Incomplete,
            Complete,
            BadRequest,
        }

        private enum ParsingState
        {
            StartLine,
            Headers
        }
    }
}
