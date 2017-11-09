// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO.Pipelines.Samples.Http;
using System.IO.Pipelines.Text.Primitives;

namespace System.IO.Pipelines.Samples
{
    public class HttpRequestParser
    {
        private ParsingState _state;

        private PreservedBuffer _httpVersion;
        private PreservedBuffer _path;
        private PreservedBuffer _method;

        public ReadableBuffer HttpVersion => _httpVersion.Buffer;
        public ReadableBuffer Path => _path.Buffer;
        public ReadableBuffer Method => _method.Buffer;

        public RequestHeaderDictionary RequestHeaders = new RequestHeaderDictionary();

        public ParseResult ParseRequest(ReadableBuffer buffer, out ReadCursor consumed, out ReadCursor examined)
        {
            consumed = buffer.Start;
            examined = buffer.Start;

            if (_state == ParsingState.StartLine)
            {
                if (!buffer.TrySliceTo((byte)'\r', (byte)'\n', out ReadableBuffer startLine, out ReadCursor delim))
                {
                    return ParseResult.Incomplete;
                }

                // Move the buffer to the rest
                buffer = buffer.Slice(delim).Slice(2);

                if (!startLine.TrySliceTo((byte)' ', out ReadableBuffer method, out delim))
                {
                    return ParseResult.BadRequest;
                }

                _method = method.Preserve();

                // Skip ' '
                startLine = startLine.Slice(delim).Slice(1);

                if (!startLine.TrySliceTo((byte)' ', out ReadableBuffer path, out delim))
                {
                    return ParseResult.BadRequest;
                }

                _path = path.Preserve();

                // Skip ' '
                startLine = startLine.Slice(delim).Slice(1);

                var httpVersion = startLine;
                if (httpVersion.IsEmpty)
                {
                    return ParseResult.BadRequest;
                }

                _httpVersion = httpVersion.Preserve();

                _state = ParsingState.Headers;
                consumed = buffer.Start;
                examined = buffer.Start;
            }

            // Parse headers
            // key: value\r\n

            while (!buffer.IsEmpty)
            {
                var headerValue = default(ReadableBuffer);
                if (!buffer.TrySliceTo((byte)'\r', (byte)'\n', out ReadableBuffer headerPair, out ReadCursor delim))
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
                if (!headerPair.TrySliceTo((byte)':', out ReadableBuffer headerName, out delim))
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

            _method.Dispose();
            _path.Dispose();
            _httpVersion.Dispose();

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
