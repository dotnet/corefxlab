// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Http.Parser.Internal;

namespace System.Text.Http.Parser
{
    public class HttpParser : IHttpParser
    {
        // TODO: commented out;
        //public ILogger Log { get; set; }

        // byte types don't have a data type annotation so we pre-cast them; to avoid in-place casts
        private const byte ByteCR = (byte)'\r';
        private const byte ByteLF = (byte)'\n';
        private const byte ByteColon = (byte)':';
        private const byte ByteSpace = (byte)' ';
        private const byte ByteTab = (byte)'\t';
        private const byte ByteQuestionMark = (byte)'?';
        private const byte BytePercentage = (byte)'%';
        private const long maxRequestLineLength = 1024;

        private static ReadOnlySpan<byte> Eol => new byte[] { ByteCR, ByteLF };
        static readonly byte[] s_http11 = Encoding.UTF8.GetBytes("HTTP/1.1");
        static readonly byte[] s_http10 = Encoding.UTF8.GetBytes("HTTP/1.0");
        static readonly byte[] s_http20 = Encoding.UTF8.GetBytes("HTTP/2.0");

        private readonly bool _showErrorDetails;

        public HttpParser()
            : this(showErrorDetails: true)
        {
        }

        public HttpParser(bool showErrorDetails)
        {
            _showErrorDetails = showErrorDetails;
        }

        public bool ParseRequestLine<T>(T handler, ref BufferReader<byte> reader) where T : IHttpRequestLineHandler
        {
            // Look for CR/LF
            if (!reader.TryReadToAny(out ReadOnlySpan<byte> requestLine, Eol, advancePastDelimiter: false))
            {
                // Couldn't find a delimiter
                return false;
            }

            if (!reader.IsNext(Eol, advancePast: true))
            {
                // Not CR/LF
                RejectRequestLine(requestLine);
            }

            // Parse the span
            ParseRequestLine(handler, requestLine);
            return true;
        }

        public bool ParseRequestLine<T>(T handler, in ReadOnlySequence<byte> buffer, out SequencePosition consumed, out SequencePosition examined) where T : IHttpRequestLineHandler
        {
            consumed = buffer.Start;
            examined = buffer.End;
            var reader = new BufferReader<byte>(buffer);
            bool success = ParseRequestLine(handler, ref reader);
            consumed = reader.Position;
            if (success) examined = reader.Position;
            return success;
        }

        public bool ParseRequestLine<T>(T handler, in ReadOnlySequence<byte> buffer, out int consumed) where T : IHttpRequestLineHandler
        {
            // Look for CR/LF

            BufferReader<byte> reader = new BufferReader<byte>(buffer);
            if (!reader.TryReadToAny(out ReadOnlySpan<byte> requestLine, Eol, advancePastDelimiter: false))
            {
                // Couldn't find a delimiter
                consumed = 0;
                return false;
            }

            if (!reader.IsNext(Eol, advancePast: true))
            {
                if (reader.TryRead(out byte value) && value == ByteCR && !reader.TryRead(out value))
                {
                    // Incomplete if ends in CR
                    consumed = 0;
                    return false;
                }

                // Not CR/LF
                RejectRequestLine(requestLine);
            }

            ParseRequestLine(handler, requestLine);

            consumed = (int)reader.Consumed;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ParseRequestLine<T>(T handler, in ReadOnlySpan<byte> data) where T : IHttpRequestLineHandler
        {
            // The absolute smallest request line is something like "Z * HTTP/1.1"
            // See https://www.w3.org/Protocols/rfc2616/rfc2616-sec5.html#sec5.1.2

            // Note that GetKnownMethod depends on the buffer having at least 9 bytes.
            if (data.Length < 12)
            {
                RejectRequestLine(data);
            }

            ReadOnlySpan<byte> customMethod = default;

            // Get Method and set the offset
            if (!HttpUtilities.GetKnownMethod(data, out Http.Method method, out int offset))
            {
                customMethod = GetUnknownMethod(data, out offset);
            }

            // Skip the space after the method
            ReadOnlySpan<byte> target = data.Slice(++offset);

            bool pathEncoded = false;
            int pathEnd = target.IndexOfAny(ByteSpace, ByteQuestionMark);
            if (pathEnd < 1 || pathEnd > target.Length - 1)
            {
                // Cant start or end with space/? or eat the entire target
                RejectRequestLine(data);
            }

            ReadOnlySpan<byte> path = target.Slice(0, pathEnd);

            int escapeIndex = path.IndexOf(BytePercentage);
            if (escapeIndex == 0)
            {
                // Can't start with %
                RejectRequestLine(data);
            }
            else if (escapeIndex > 0)
            {
                pathEncoded = true;
            }

            ReadOnlySpan<byte> query = default;
            if (target[pathEnd] == ByteQuestionMark)
            {
                // Query string
                query = target.Slice(path.Length);
                int spaceIndex = query.IndexOf(ByteSpace);
                if (spaceIndex < 1)
                {
                    // End of query string not found
                    RejectRequestLine(data);
                }
                query = query.Slice(0, spaceIndex);
            }

            target = target.Slice(0, path.Length + query.Length);

            // Version

            // Skip space
            ReadOnlySpan<byte> version = data.Slice(offset + target.Length + 1);

            if (!HttpUtilities.GetKnownVersion(version, out Http.Version httpVersion))
            {
                RejectUnknownVersion(version);
            }

            handler.OnStartLine(method, httpVersion, target, path, query, customMethod, pathEncoded);
        }

        public bool ParseHeaders<T>(
            T handler,
            in ReadOnlySequence<byte> buffer,
            out SequencePosition consumed,
            out SequencePosition examined,
            out int consumedBytes) where T : IHttpHeadersHandler
        {
            var reader = new BufferReader<byte>(buffer);

            consumed = reader.Sequence.Start;
            examined = reader.Sequence.End;
            consumedBytes = 0;

            bool success = ParseHeaders(handler, ref reader);
            consumed = reader.Position;
            consumedBytes = (int)reader.Consumed;
            if (success)
                examined = consumed;
            return success;
        }

        public bool ParseHeaders<T>(
            T handler,
            ref BufferReader<byte> reader) where T : IHttpHeadersHandler
        {
            bool success = false;

            while (!reader.End)
            {
                if (!reader.TryReadToAny(out ReadOnlySpan<byte> headerLine, Eol, advancePastDelimiter: false))
                {
                    // Couldn't find another delimiter
                    break;
                }

                int headerLength = headerLine.Length;
                if (!reader.IsNext(Eol, advancePast: true))
                {
                    // Not a good CR/LF pair
                    RejectCRLF(ref reader, headerLine);
                    break;
                }

                if (headerLength == 0)
                {
                    // Consider an empty line to be the end
                    success = true;
                    break;
                }

                TakeSingleHeader(headerLine, handler);
            }

            return success;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void RejectCRLF(ref BufferReader<byte> reader, ReadOnlySpan<byte> headerLine)
        {
            if (reader.TryRead(out byte value) && value == ByteCR && !reader.TryRead(out value))
            {
                // Incomplete if ends in CR
                return;
            }

            if (headerLine.Length == 0)
            {
                RejectRequest(RequestRejectionReason.InvalidRequestHeadersNoCRLF);
            }
            else
            {
                RejectRequestHeader(headerLine);
            }
        }

        public bool ParseHeaders<T>(T handler, ReadOnlySequence<byte> buffer, out int consumedBytes) where T : IHttpHeadersHandler
        {
            var reader = new BufferReader<byte>(buffer);
            consumedBytes = 0;

            bool success = ParseHeaders(handler, ref reader);
            if (success)
                consumedBytes = (int)reader.Consumed;
            return success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void TakeSingleHeader<T>(ReadOnlySpan<byte> headerLine, T handler) where T : IHttpHeadersHandler
        {
            Debug.Assert(headerLine.IndexOf(ByteCR) == -1);

            // Find the end of the name (name:value)
            int nameEnd = 0;
            for (; nameEnd < headerLine.Length; nameEnd++)
            {
                byte ch = headerLine[nameEnd];
                if (ch == ByteColon)
                {
                    break;
                }
                else if (ch == ByteTab || ch == ByteSpace)
                {
                    RejectRequestHeader(headerLine);
                }
            }

            if (nameEnd == headerLine.Length)
            {
                // Couldn't find the colon
                RejectRequestHeader(headerLine);
            }

            // Move past the colon
            int valueStart = nameEnd + 1;

            // Trim any whitespace from the start and end of the value
            for (; valueStart < headerLine.Length; valueStart++)
            {
                byte ch = headerLine[valueStart];
                if (ch != ByteSpace && ch != ByteTab)
                {
                    break;
                }
            }

            int valueEnd = headerLine.Length - 1;
            for (; valueEnd > valueStart; valueEnd--)
            {
                byte ch = headerLine[valueEnd];
                if (ch != ByteSpace && ch != ByteTab)
                {
                    break;
                }
            }

            handler.OnHeader(
                headerLine.Slice(0, nameEnd),
                headerLine.Slice(valueStart, valueEnd - valueStart + 1));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private ReadOnlySpan<byte> GetUnknownMethod(in ReadOnlySpan<byte> data, out int methodLength)
        {
            methodLength = 0;
            for (; methodLength < data.Length; methodLength++)
            {
                byte ch = data[methodLength];

                if (ch == ByteSpace)
                {
                    if (methodLength == 0)
                    {
                        RejectRequestLine(data);
                    }

                    break;
                }
                else if (!IsValidTokenChar((char)ch))
                {
                    RejectRequestLine(data);
                }
            }

            if (methodLength == data.Length)
            {
                // Didn't find a space
                RejectRequestLine(data);
            }

            return data.Slice(0, methodLength);
        }

        // TODO: this could be optimized by using a table
        private static bool IsValidTokenChar(char c)
        {
            // Determines if a character is valid as a 'token' as defined in the
            // HTTP spec: https://tools.ietf.org/html/rfc7230#section-3.2.6
            return
                (c >= '0' && c <= '9') ||
                (c >= 'A' && c <= 'Z') ||
                (c >= 'a' && c <= 'z') ||
                c == '!' ||
                c == '#' ||
                c == '$' ||
                c == '%' ||
                c == '&' ||
                c == '\'' ||
                c == '*' ||
                c == '+' ||
                c == '-' ||
                c == '.' ||
                c == '^' ||
                c == '_' ||
                c == '`' ||
                c == '|' ||
                c == '~';
        }

        private void RejectRequest(RequestRejectionReason reason)
            => throw BadHttpRequestException.GetException(reason);

        private void RejectRequestLine(ReadOnlySpan<byte> requestLine)
            => throw GetInvalidRequestException(RequestRejectionReason.InvalidRequestLine, requestLine);

        private void RejectRequestHeader(ReadOnlySpan<byte> headerLine)
            => throw GetInvalidRequestException(RequestRejectionReason.InvalidRequestHeader, headerLine);

        private void RejectUnknownVersion(ReadOnlySpan<byte> version)
            => throw GetInvalidRequestException(RequestRejectionReason.UnrecognizedHTTPVersion, version);

        private BadHttpRequestException GetInvalidRequestException(RequestRejectionReason reason, ReadOnlySpan<byte> detail)
        {
            string errorDetails = _showErrorDetails ?
                detail.GetAsciiStringEscaped(Constants.MaxExceptionDetailSize) :
                string.Empty;
            return BadHttpRequestException.GetException(reason, errorDetails);
        }

        public void Reset()
        {
        }
    }
}
