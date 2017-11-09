// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Buffers.Text;
using System.Collections.Sequences;
using System.Text.Formatting;
using System.Text.Utf8;

namespace System.Text.Http
{
    public struct HttpRequestLine
    {
        public HttpMethod Method;
        public HttpVersion Version;
        public Memory<byte> RequestUri;

        public override string ToString()
        {
            return RequestUri.ToString();
        }
    }

    public struct HttpRequest
    {
        const int MaxHeadersLength = 16_384;

        internal static readonly byte[] Cr = new byte[] { (byte)'\r', (byte)'\n' };
        internal static readonly byte[] Cr2 = new byte[] { (byte)'\r', (byte)'\n', (byte)'\r', (byte)'\n' };

        internal ReadOnlyBytes Bytes;
        internal Range _verb;
        internal Range _path;
        internal Range _version;
        internal HttpHeaders _headers;
        internal int _bodyIndex;

        public ReadOnlyBytes Verb => Bytes.Slice(_verb);
        public ReadOnlyBytes Path => Bytes.Slice(_path);
        public ReadOnlyBytes Version => Bytes.Slice(_version);
        public HttpHeaders Headers => _headers;
        public ReadOnlyBytes Body => Bytes.Slice(_bodyIndex);

        public int BodyIndex => _bodyIndex;

        public static HttpRequest Parse(ReadOnlyBytes bytes)
        {
            var request = new HttpRequest();
            request.Bytes = bytes;

            var reader = new BytesReader(bytes);
            Range? verb = reader.ReadRangeUntil((byte)' ');
            request._verb = verb.Value;
            reader.Advance(1);

            Range? path = reader.ReadRangeUntil((byte)' ');
            request._path = path.Value;
            reader.Advance(1);

            Range? version = reader.ReadRangeUntil(Cr);
            request._version = version.Value;
            reader.Advance(2);

            var headers = reader.ReadRangeUntil(Cr2).Value;
            request._headers = new HttpHeaders() { _headers = bytes.Slice(headers.Index, headers.Length + 2) };

            long headersLength = headers.Index + headers.Length + 4;
            if (headersLength > MaxHeadersLength) throw new Exception("headers too long");

            request._bodyIndex = (int)headersLength;
            return request;
        }
    }

    public struct HttpHeader
    {
        public ReadOnlyBytes Name; // TODO (pri 2): this is pretty wasteful; too many references to ReadOnlyBytes; we should store ranges only
        public ReadOnlyBytes Value;

        public void Deconstruct(out string name, out string value)
        {
            name = Name.ToString(SymbolTable.InvariantUtf8);
            value = Value.ToString(SymbolTable.InvariantUtf8);
        }

        public void Deconstruct(out Utf8Span name, out Utf8Span value)
        {
            name = Name.ToUtf8Span(SymbolTable.InvariantUtf8);
            value = Value.ToUtf8Span(SymbolTable.InvariantUtf8);
        }
    }

    public struct HttpHeaders : ISequence<HttpHeader>
    {
        const int MaxHeaderLength = 1024;

        internal ReadOnlyBytes _headers;

        public bool TryGet(ref Position position, out HttpHeader value, bool advance = true)
        {
            var bytes = _headers.Slice(position.IntegerPosition);
            if (bytes.IsEmpty)
            {
                value = default;
                return false;
            }

            ParseHeader(bytes, out value, out int consumed);
            if (advance)
            {
                position.IntegerPosition += consumed;
            }
            return true;
        }

        static void ParseHeader(ReadOnlyBytes bytes, out HttpHeader value, out int consumed)
        {
            long indexOfValue = bytes.IndexOf((byte)':');
            if (indexOfValue > MaxHeaderLength) throw new Exception("header too long");
            var headerName = bytes.Slice(0, indexOfValue);
            long endOfValue = bytes.IndexOf(HttpRequest.Cr);
            if (indexOfValue > MaxHeaderLength) throw new Exception("header too long");
            var headerValue = bytes.Slice(indexOfValue + 2, endOfValue - indexOfValue - 2);
            value = new HttpHeader() { Name = headerName, Value = headerValue };
            consumed = (int)endOfValue + 2;
        }

        public override string ToString()
        {
            return _headers.ToString(SymbolTable.InvariantUtf8);
        }
    }

    public enum HttpMethod : byte
    {
        Unknown = 0,
        Other,
        Get,
        Post,
        Put,
        Delete,
    }

    public enum HttpVersion : byte
    {
        Unknown = 0,
        Other,
        V1_0,
        V1_1,
        V2_0,
    }

    // TODO: these should be improved and moved to System.Text.Primtives
    public static class PrimitiveEncoder
    {
        public static string ToString(this ReadOnlyBytes? bytes, SymbolTable symbolTable)
        {
            if (bytes == null) return "";
            return ToString(bytes.Value, symbolTable);
        }

        public static string ToString(this Memory<byte> bytes, SymbolTable symbolTable)
        {
            if (symbolTable == SymbolTable.InvariantUtf8)
            {
                return new Utf8Span(bytes.Span).ToString();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static string ToString(this ReadOnlyBytes bytes, SymbolTable symbolTable)
        {
            var sb = new StringBuilder();
            if (symbolTable == SymbolTable.InvariantUtf8)
            {
                var position = Position.First;
                while (bytes.TryGet(ref position, out ReadOnlyMemory<byte> segment, true))
                {
                    sb.Append(new Utf8Span(segment.Span).ToString());
                }
            }
            else
            {
                throw new NotImplementedException();
            }
            return sb.ToString();
        }

        public static Utf8Span ToUtf8Span(this ReadOnlyBytes? bytes, SymbolTable symbolTable)
        {
            if (bytes == null) return Utf8Span.Empty;
            return ToUtf8Span(bytes.Value, symbolTable);
        }

        public static Utf8Span ToUtf8Span(this ReadOnlyBytes bytes, SymbolTable symbolTable)
        {
            if (bytes.Length > int.MaxValue) throw new ArgumentOutOfRangeException(nameof(bytes));
            var sb = new ArrayFormatter((int)bytes.Length, SymbolTable.InvariantUtf8);
            if (symbolTable == SymbolTable.InvariantUtf8)
            {
                var position = Position.First;
                while (bytes.TryGet(ref position, out ReadOnlyMemory<byte> segment, true))
                {
                    sb.Append(new Utf8Span(segment.Span));
                }
            }
            else
            {
                throw new NotImplementedException();
            }
            return new Utf8Span(sb.Formatted.AsSpan());
        }
    }
}
