// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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

        static readonly byte[] s_Eol = Encoding.UTF8.GetBytes("\r\n");
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

        public bool ParseRequestLine<T>(T handler, in ReadOnlySequence<byte> buffer, out SequencePosition consumed, out SequencePosition examined) where T : IHttpRequestLineHandler
        {
            consumed = buffer.Start;
            examined = buffer.End;

            // Prepare the first span
            var span = buffer.First.Span;
            var lineIndex = span.IndexOf(ByteLF);
            if (lineIndex >= 0)
            {
                consumed = buffer.GetPosition(lineIndex + 1, consumed);
                span = span.Slice(0, lineIndex + 1);
            }
            else if (buffer.IsSingleSegment)
            {
                return false;
            }
            else
            {
                if (TryGetNewLineSpan(buffer, out SequencePosition found))
                {
                    span = PipelineExtensions.ToSpan(buffer.Slice(consumed, found));
                    consumed = found;
                }
                else
                {
                    // No request line end
                    return false;
                }
            }

            // Parse the span
            ParseRequestLine(ref handler, span);

            examined = consumed;
            return true;
        }

        public bool ParseRequestLine<T>(ref T handler, in ReadOnlySequence<byte> buffer, out int consumed) where T : IHttpRequestLineHandler
        {
            // Prepare the first span
            var span = buffer.First.Span;
            var lineIndex = span.IndexOf(ByteLF);
            if (lineIndex >= 0)
            {
                consumed = lineIndex + 1;
                span = span.Slice(0, lineIndex + 1);
            }
            else
            {
                var eol = Sequence.IndexOf(buffer, s_Eol[0], s_Eol[1]);
                if (eol == -1)
                {
                    consumed = 0;
                    return false;
                }

                span = PipelineExtensions.ToSpan(buffer.Slice(0, eol + s_Eol.Length));
            }

            // Parse the span
            ParseRequestLine(ref handler, span);

            consumed = span.Length;
            return true;
        }

        private void ParseRequestLine<T>(ref T handler, ReadOnlySpan<byte> data) where T : IHttpRequestLineHandler
        {
            ReadOnlySpan<byte> customMethod = default;

            // Get Method and set the offset
            if (!HttpUtilities.GetKnownMethod(data, out Http.Method method, out int offset))
            {
                customMethod = GetUnknownMethod(data, out offset);
            }

            // Skip the space after the method
            offset++;

            if (offset >= data.Length)
            {
                // Need some path data
                RejectRequestLine(data);
            }

            bool pathEncoded = false;
            ReadOnlySpan<byte> target = data.Slice(offset);
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
                if (version[0] == ByteCR || data[data.Length - 2] != ByteCR)
                {
                    // If missing delimiter or CR before LF, reject and log entire line
                    RejectRequestLine(data);
                }
                else
                {
                    // else inform HTTP version is unsupported.
                    RejectUnknownVersion(data.Slice(offset, data.Length - offset - 2));
                }
            }

            // After version's 8 bytes and CR, expect LF
            if (version[8 + 1] != ByteLF)
            {
                RejectRequestLine(data);
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
            consumed = buffer.Start;
            examined = buffer.End;
            consumedBytes = 0;

            BufferReader<byte> reader = new BufferReader<byte>(buffer);
            ReadOnlySpan<byte> eol = s_Eol;

            bool success = false;

            while (!reader.End)
            {
                if (!reader.TryReadToAny(out ReadOnlySpan<byte> headerLine, eol, advancePastDelimiter: false))
                {
                    // Couldn't find another delimiter
                    break;
                }

                int headerLength = headerLine.Length;
                if (!reader.IsNext(eol, advancePast: true))
                {
                    // Not a good CR/LF pair
                    if (headerLength == 0)
                    {
                        RejectRequest(RequestRejectionReason.InvalidRequestHeadersNoCRLF);
                    }
                    else
                    {
                        RejectRequestHeader(headerLine);
                    }
                }

                if (headerLength == 0)
                {
                    // Consider an empty line to be the end
                    success = true;
                    break;
                }

                TakeSingleHeader(headerLine, handler);
            }

            consumed = reader.Position;
            consumedBytes = (int)reader.Consumed;
            if (success)
                examined = consumed;
            return success;
        }

        public unsafe bool ParseHeaders<T>(ref T handler, ReadOnlySequence<byte> buffer, out int consumedBytes) where T : IHttpHeadersHandler
        {
            var index = 0;
            consumedBytes = 0;
            SequencePosition position = buffer.Start;

            if (!buffer.TryGet(ref position, out ReadOnlyMemory<byte> currentMemory))
            {
                consumedBytes = 0;
                return false;
            }
            ReadOnlySpan<byte> currentSpan = currentMemory.Span;
            var remaining = currentSpan.Length;

            while (true)
            {
                fixed (byte* pBuffer = &MemoryMarshal.GetReference(currentSpan))
                {
                    while (remaining > 0)
                    {
                        int ch1;
                        int ch2;

                        // Fast path, we're still looking at the same span
                        if (remaining >= 2)
                        {
                            ch1 = pBuffer[index];
                            ch2 = pBuffer[index + 1];
                        }
                        // Slow Path
                        else
                        {
                            ReadTwoChars(buffer, consumedBytes, out ch1, out ch2);
                            // I think the above is fast enough. If we don't like it, we can do the code below after some modifications
                            // to ensure that one next.Span is enough
                            //if(hasNext) ReadNextTwoChars(pBuffer, remaining, index, out ch1, out ch2, next.Span);
                            //else
                            //{
                            //    return false;
                            //}
                        }

                        if (ch1 == ByteCR)
                        {
                            if (ch2 == ByteLF)
                            {
                                consumedBytes += 2;
                                return true;
                            }

                            if (ch2 == -1)
                            {
                                consumedBytes = 0;
                                return false;
                            }

                            // Headers don't end in CRLF line.
                            RejectRequest(RequestRejectionReason.InvalidRequestHeadersNoCRLF);
                        }

                        var endIndex = new ReadOnlySpan<byte>(pBuffer + index, remaining).IndexOf(ByteLF);
                        var length = 0;

                        if (endIndex != -1)
                        {
                            length = endIndex + 1;
                            var pHeader = pBuffer + index;

                            TakeSingleHeader(pHeader, length, ref handler);
                        }
                        else
                        {
                            // Split buffers
                            var end = Sequence.IndexOf(buffer.Slice(index), ByteLF);
                            if (end == -1)
                            {
                                // Not there
                                consumedBytes = 0;
                                return false;
                            }

                            var headerSpan = PipelineExtensions.ToSpan(buffer.Slice(index, end - index + 1));
                            length = headerSpan.Length;

                            fixed (byte* pHeader = &MemoryMarshal.GetReference(headerSpan))
                            {
                                TakeSingleHeader(pHeader, length, ref handler);
                            }
                        }

                        // Skip the reader forward past the header line
                        index += length;
                        consumedBytes += length;
                        remaining -= length;
                    }
                }

                if (buffer.TryGet(ref position, out var nextSegment))
                {
                    currentSpan = nextSegment.Span;
                    remaining = currentSpan.Length + remaining;
                    index = currentSpan.Length - remaining;
                }
                else
                {
                    consumedBytes = 0;
                    return false;
                }
            }
        }

        public static bool ParseResponseLine<T>(ref T handler, ref ReadOnlySequence<byte> buffer, out int consumedBytes) where T : IHttpResponseLineHandler
        {
            var line = buffer.First.Span;
            var lf = line.IndexOf(ByteLF);
            if (lf >= 0)
            {
                line = line.Slice(0, lf + 1);
            }
            else if (buffer.IsSingleSegment)
            {
                consumedBytes = 0;
                return false;
            }
            else
            {
                long index = Sequence.IndexOf(buffer, ByteLF);
                if (index < 0)
                {
                    consumedBytes = 0;
                    return false;
                }
                if (index > maxRequestLineLength)
                {
                    throw new Exception("invalid response (LF too far)");
                }
                line = line.Slice(0, lf + 1);
            }

            if (line[lf - 1] != ByteCR)
            {
                throw new Exception("invalid response (no CR)");
            }

            Http.Version version;
            if (line.StartsWith(s_http11)) { version = Http.Version.Http11; }
            else if (line.StartsWith(s_http20)) { version = Http.Version.Http20; }
            else if (line.StartsWith(s_http10)) { version = Http.Version.Http10; }
            else
            {
                throw new Exception("invalid response (version)");
            }

            int codeStart = line.IndexOf((byte)' ') + 1;
            var codeSlice = line.Slice(codeStart);
            if (!Utf8Parser.TryParse(codeSlice, out ushort code, out consumedBytes))
            {
                throw new Exception("invalid response (status code)");
            }

            var reasonStart = consumedBytes + 1;
            var reason = codeSlice.Slice(reasonStart, codeSlice.Length - reasonStart - 2);
            consumedBytes = lf + s_Eol.Length;

            handler.OnStatusLine(version, code, reason);

            return true;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ReadTwoChars(ReadOnlySequence<byte> buffer, int consumedBytes, out int ch1, out int ch2)
        {
            var first = buffer.First.Span;
            if (first.Length - consumedBytes > 1)
            {
                ch1 = first[consumedBytes];
                ch2 = first[consumedBytes + 1];
            }

            if (buffer.Length < consumedBytes + 2)
            {
                ch1 = -1;
                ch2 = -1;
            }
            else
            {
                buffer = buffer.Slice(consumedBytes, 2);
                Span<byte> temp = stackalloc byte[2];
                buffer.CopyTo(temp);
                ch1 = temp[0];
                ch2 = temp[1];
            }
        }

        // This is not needed, but I will leave it here for now, as we might need it later when optimizing
        //private static unsafe void ReadNextTwoChars(byte* pBuffer, int remaining, int index, out int ch1, out int ch2, ReadOnlySpan<byte> next)
        //{
        //    Debug.Assert(next.Length > 1);
        //    Debug.Assert(remaining == 0 || remaining == 1);

        //    if (remaining == 1)
        //    {
        //        ch1 = pBuffer[index];
        //        ch2 = next.IsEmpty ? -1 : next[0];
        //    }
        //    else
        //    {
        //        ch1 = -1;
        //        ch2 = -1;

        //        if (next.Length > 0)
        //        {
        //            ch1 = next[0];
        //            if (next.Length > 1)
        //            {
        //                ch2 = next[1];
        //            }
        //        }
        //    }
        //}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe int FindEndOfName(byte* headerLine, int length)
        {
            var index = 0;
            var sawWhitespace = false;
            for (; index < length; index++)
            {
                var ch = headerLine[index];
                if (ch == ByteColon)
                {
                    break;
                }
                if (ch == ByteTab || ch == ByteSpace || ch == ByteCR)
                {
                    sawWhitespace = true;
                }
            }

            if (index == length || sawWhitespace)
            {
                RejectRequestHeader(new ReadOnlySpan<byte>(headerLine, length));
            }

            return index;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe void TakeSingleHeader<T>(byte* headerLine, int length, ref T handler) where T : IHttpHeadersHandler
        {
            // Skip CR, LF from end position
            var valueEnd = length - 3;
            var nameEnd = FindEndOfName(headerLine, length);

            if (headerLine[valueEnd + 2] != ByteLF)
            {
                RejectRequestHeader(new ReadOnlySpan<byte>(headerLine, length));
            }
            if (headerLine[valueEnd + 1] != ByteCR)
            {
                RejectRequestHeader(new ReadOnlySpan<byte>(headerLine, length));
            }

            // Skip colon from value start
            var valueStart = nameEnd + 1;
            // Ignore start whitespace
            for (; valueStart < valueEnd; valueStart++)
            {
                var ch = headerLine[valueStart];
                if (ch != ByteTab && ch != ByteSpace && ch != ByteCR)
                {
                    break;
                }
                else if (ch == ByteCR)
                {
                    RejectRequestHeader(new ReadOnlySpan<byte>(headerLine, length));
                }
            }

            // Check for CR in value
            var i = valueStart + 1;
            if (Contains(headerLine + i, valueEnd - i, ByteCR))
            {
                RejectRequestHeader(new ReadOnlySpan<byte>(headerLine, length));
            }

            // Ignore end whitespace
            for (; valueEnd >= valueStart; valueEnd--)
            {
                var ch = headerLine[valueEnd];
                if (ch != ByteTab && ch != ByteSpace)
                {
                    break;
                }
            }

            var nameBuffer = new ReadOnlySpan<byte>(headerLine, nameEnd);
            var valueBuffer = new ReadOnlySpan<byte>(headerLine + valueStart, valueEnd - valueStart + 1);

            handler.OnHeader(nameBuffer, valueBuffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe bool Contains(byte* searchSpace, int length, byte value)
        {
            var i = 0;
            if (Vector.IsHardwareAccelerated)
            {
                // Check Vector lengths
                if (length - Vector<byte>.Count >= i)
                {
                    var vValue = GetVector(value);
                    do
                    {
                        if (!Vector<byte>.Zero.Equals(Vector.Equals(vValue, Unsafe.Read<Vector<byte>>(searchSpace + i))))
                        {
                            goto found;
                        }

                        i += Vector<byte>.Count;
                    } while (length - Vector<byte>.Count >= i);
                }
            }

            // Check remaining for CR
            for (; i <= length; i++)
            {
                var ch = searchSpace[i];
                if (ch == value)
                {
                    goto found;
                }
            }
            return false;
            found:
            return true;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool TryGetNewLineSpan(in ReadOnlySequence<byte> buffer, out SequencePosition found)
        {
            var position = BuffersExtensions.PositionOf(buffer, ByteLF);
            if (position.HasValue)
            {
                // Move 1 byte past the \n
                found = buffer.GetPosition(1, position.Value);
                return true;
            }

            found = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private ReadOnlySpan<byte> GetUnknownMethod(ReadOnlySpan<byte> data, out int methodLength)
        {
            methodLength = 0;
            for (var i = 0; i < data.Length; i++)
            {
                var ch = data[i];

                if (ch == ByteSpace)
                {
                    if (i == 0)
                    {
                        RejectRequestLine(data);
                    }

                    methodLength = i;
                    break;
                }
                else if (!IsValidTokenChar((char)ch))
                {
                    RejectRequestLine(data);
                }
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Vector<byte> GetVector(byte vectorByte)
        {
            // Vector<byte> .ctor doesn't become an intrinsic due to detection issue
            // However this does cause it to become an intrinsic (with additional multiply and reg->reg copy)
            // https://github.com/dotnet/coreclr/issues/7459#issuecomment-253965670
            return Vector.AsVectorByte(new Vector<uint>(vectorByte * 0x01010101u));
        }

        private enum State : byte
        {
            ParsingName,
            BeforValue,
            ParsingValue,
            ArferCR,
        }
    }
}
