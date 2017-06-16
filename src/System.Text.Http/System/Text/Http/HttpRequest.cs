// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Collections.Sequences;
using System.Text.Formatting;
using System.Text.Utf8;
using System.Threading;

namespace System.Text.Http
{
    public struct HttpRequestLine
    {
        public HttpMethod Method;
        public HttpVersion Version;
        public Buffer<byte> RequestUri;

        public override string ToString()
        {
            return RequestUri.ToString();
        }
    }

    public struct HttpRequest
    {
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

            request._bodyIndex = headers.Index + headers.Length + 4;
            return request;
        }
    }

    public struct HttpHeader
    {
        public ReadOnlyBytes Name; // TODO (pri 2): this is pretty wasteful; too many references to ReadOnlyBytes; we should store ranges only
        public ReadOnlyBytes Value;

        public void Deconstruct(out string name, out string value)
        {
            name = Name.ToString(TextEncoder.Utf8);
            value = Value.ToString(TextEncoder.Utf8);
        }

        public void Deconstruct(out Utf8String name, out Utf8String value)
        {
            name = Name.ToUtf8String(TextEncoder.Utf8);
            value = Value.ToUtf8String(TextEncoder.Utf8);
        }
    }

    public struct HttpHeaders : ISequence<HttpHeader>
    {
        internal ReadOnlyBytes _headers;

        public bool TryGet(ref Position position, out HttpHeader value, bool advance = true)
        {
            var bytes = _headers.Slice(position.IntegerPosition);
            if (bytes.Length == 0)
            {
                value = default;
                return false;
            }

            int consumed;
            ParseHeader(bytes, out value, out consumed);
            if (advance)
            {
                position.IntegerPosition += consumed;
            }
            return true;
        }

        static void ParseHeader(ReadOnlyBytes bytes, out HttpHeader value, out int consumed)
        {
            int indexOfValue = bytes.IndexOf((byte)':');
            var headerName = bytes.Slice(0, indexOfValue);
            var endOfValue = bytes.IndexOf(HttpRequest.Cr);
            var headerValue = bytes.Slice(indexOfValue + 2, endOfValue - indexOfValue - 2);
            value = new HttpHeader() { Name = headerName, Value = headerValue };
            consumed = endOfValue + 2;
        }

        public override string ToString()
        {
            return _headers.ToString(TextEncoder.Utf8);
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
        public static string ToString(this ReadOnlyBytes? bytes, TextEncoder encoder)
        {
            if (bytes == null) return "";
            return ToString(bytes.Value, encoder);
        }

        public static string ToString(this Buffer<byte> bytes, TextEncoder encoder)
        {
            if (encoder.Encoding == TextEncoder.EncodingName.Utf8)
            {
                return new Utf8String(bytes.Span).ToString();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static string ToString(this ReadOnlyBytes bytes, TextEncoder encoder)
        {
            var sb = new StringBuilder();
            if (encoder.Encoding == TextEncoder.EncodingName.Utf8)
            {
                var position = Position.First;
                ReadOnlyBuffer<byte> segment;
                while (bytes.TryGet(ref position, out segment, true))
                {
                    sb.Append(new Utf8String(segment.Span).ToString());
                }
            }
            else
            {
                throw new NotImplementedException();
            }
            return sb.ToString();
        }

        public static Utf8String ToUtf8String(this ReadOnlyBytes? bytes, TextEncoder encoder)
        {
            if (bytes == null) return Utf8String.Empty;
            return ToUtf8String(bytes.Value, encoder);
        }

        public static Utf8String ToUtf8String(this ReadOnlyBytes bytes, TextEncoder encoder)
        {
            var sb = new ArrayFormatter(bytes.ComputeLength(), TextEncoder.Utf8);
            if (encoder.Encoding == TextEncoder.EncodingName.Utf8)
            {
                var position = Position.First;
                ReadOnlyBuffer<byte> segment;
                while (bytes.TryGet(ref position, out segment, true))
                {
                    sb.Append(new Utf8String(segment.Span));
                }
            }
            else
            {
                throw new NotImplementedException();
            }
            return new Utf8String(sb.Formatted.AsSpan());
        }
    }
}
