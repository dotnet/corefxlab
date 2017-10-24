// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Utf8;

namespace System.Text.Http.SingleSegment
{
    [Obsolete("Use System.Text.Http.HttpRequest")]
    public ref struct HttpRequestSingleSegment
    {
        private HttpRequestLine _requestLine;
        private HttpHeadersSingleSegment _headers;
        private ReadOnlySpan<byte> _body;

        public HttpRequestLine RequestLine
        {
            get
            {
                return _requestLine;
            }
        }

        public HttpHeadersSingleSegment Headers
        {
            get
            {
                return _headers;
            }
        }

        public ReadOnlySpan<byte> Body
        {
            get
            {
                return _body;
            }
        }

        public HttpRequestSingleSegment(HttpRequestLine requestLine, HttpHeadersSingleSegment headers, ReadOnlySpan<byte> bytes)
        {
            _requestLine = requestLine;
            _headers = headers;
            _body = bytes;
        }

        public static HttpRequestSingleSegment Parse(ReadOnlySpan<byte> bytes)
        {
            int parsed = HttpRequestParser.TryParseRequestLine(bytes, out var requestLine);
            if (parsed == 0)
            {
                throw new NotImplementedException("request line parser");
            }

            bytes = bytes.Slice(parsed);

            parsed = HttpRequestParser.TryParseHeaders(bytes, out var headers);
            if (parsed == 0)
            {
                throw new NotImplementedException("headers parser");
            }

            var body = bytes.Slice(parsed);
            var request = new HttpRequestSingleSegment(requestLine, headers, body);

            return request;
        }
    }

    public ref struct HttpRequestReader
    {
        static readonly byte[] b_Http1_0 = Encoding.UTF8.GetBytes("HTTP/1.0");
        static readonly byte[] b_Http1_1 = Encoding.UTF8.GetBytes("HTTP/1.1");
        static readonly byte[] b_Http2_0 = Encoding.UTF8.GetBytes("HTTP/2.0");

        static Utf8Span s_Http1_0 => new Utf8Span(b_Http1_0);
        static Utf8Span s_Http1_1 => new Utf8Span(b_Http1_1);
        static Utf8Span s_Http2_0 => new Utf8Span(b_Http2_0);

        internal static readonly byte[] Eol = new byte[] { s_CR, s_LF };
        internal static readonly byte[] Eoh = new byte[] { s_CR, s_LF, s_CR, s_LF }; // End of Headers

        internal const byte s_SP = 32; // space
        internal const byte s_CR = 13; // carriage return
        internal const byte s_LF = 10; // line feed
        internal const byte s_HT = 9;   // horizontal TAB

        private ReadOnlySpan<byte> buffer;

        public ReadOnlySpan<byte> Buffer
        {
            get
            {
                return buffer;
            }

            set
            {
                buffer = value;
            }
        }

        internal HttpMethod ReadMethod()
        {
            HttpMethod method;
            int parsedBytes;
            if (!HttpRequestParser.TryParseMethod(Buffer, out method, out parsedBytes))
            {
                return HttpMethod.Unknown;
            }
            Buffer = Buffer.Slice(parsedBytes);
            return method;
        }

        internal Utf8Span ReadRequestUri()
        {
            Utf8Span requestUri;
            int parsedBytes = HttpRequestParser.TryParseRequestUri(Buffer, out requestUri);
            if (parsedBytes == 0)
            {
                return Utf8Span.Empty;
            }
            Buffer = Buffer.Slice(parsedBytes);
            return requestUri;
        }

        Utf8Span ReadHttpVersionAsUtf8Span()
        {
            Utf8Span httpVersion;
            int parsedBytes = HttpRequestParser.TryParseHttpVersion(Buffer, out httpVersion);
            if (parsedBytes == 0)
            {
                return Utf8Span.Empty;
            }
            Buffer = Buffer.Slice(parsedBytes + Eol.Length);
            return httpVersion;
        }

        internal HttpVersion ReadHttpVersion()
        {
            ReadOnlySpan<byte> oldBuffer = Buffer;
            Utf8Span version = ReadHttpVersionAsUtf8Span();

            if (version.Equals(s_Http1_1))
            {
                return HttpVersion.V1_1;
            }
            else if (version.Equals(s_Http2_0))
            {
                return HttpVersion.V2_0;
            }
            else if (version.Equals(s_Http1_0))
            {
                return HttpVersion.V1_0;
            }
            else
            {
                Buffer = oldBuffer;
                return HttpVersion.Unknown;
            }
        }

        public Utf8Span ReadHeader()
        {
            int parsedBytes = Buffer.SliceTo(s_CR, s_LF, out var header);
            if (parsedBytes > Buffer.Length)
            {
                Buffer = Buffer.Slice(parsedBytes);
            }
            else
            {
                Buffer = new Span<byte>();
            }
            return new Utf8Span(header);
        }        
    }

    static class HttpRequestParser
    {
        // TODO: these copies should be eliminated
        static readonly byte[] b_Get = Encoding.UTF8.GetBytes("GET ");
        static readonly byte[] b_Post = Encoding.UTF8.GetBytes("POST ");
        static readonly byte[] b_Put = Encoding.UTF8.GetBytes("PUT ");
        static readonly byte[] b_Delete = Encoding.UTF8.GetBytes("DELETE ");

        static Utf8Span s_Get => new Utf8Span(b_Get);
        static Utf8Span s_Post => new Utf8Span(b_Post);
        static Utf8Span s_Put => new Utf8Span(b_Put);
        static Utf8Span s_Delete => new Utf8Span(b_Delete);


        internal static int TryParseRequestLine(ReadOnlySpan<byte> buffer, out HttpRequestLine requestLine)
        {
            requestLine = new HttpRequestLine();

            var reader = new HttpRequestReader();
            reader.Buffer = buffer;

            requestLine.Method = reader.ReadMethod();
            if(requestLine.Method == HttpMethod.Unknown) { return 0; }

            requestLine.RequestUri = reader.ReadRequestUri().Bytes.ToArray();
            if(requestLine.RequestUri.Length == 0) { return 0; }
            reader.Buffer = reader.Buffer.Slice(1);

            requestLine.Version = reader.ReadHttpVersion();
            if (requestLine.Version == HttpVersion.Unknown) { return 0; }

            return buffer.Length - reader.Buffer.Length;
        }

        internal static bool TryParseMethod(ReadOnlySpan<byte> buffer, out HttpMethod method, out int parsedBytes)
        {
            var bufferString = new Utf8Span(buffer);
            if(bufferString.StartsWith(s_Get))
            {
                method = HttpMethod.Get;
                parsedBytes = s_Get.Bytes.Length;
                return true;
            }

            if (bufferString.StartsWith(s_Post))
            {
                method = HttpMethod.Post;
                parsedBytes = s_Post.Bytes.Length;
                return true;
            }

            if (bufferString.StartsWith(s_Put))
            {
                method = HttpMethod.Put;
                parsedBytes = s_Put.Bytes.Length;
                return true;
            }

            if (bufferString.StartsWith(s_Delete))
            {
                method = HttpMethod.Delete;
                parsedBytes = s_Delete.Bytes.Length;
                return true;
            }

            method = HttpMethod.Unknown;
            parsedBytes = 0;
            return false;
        }

        internal static int TryParseRequestUri(ReadOnlySpan<byte> buffer, out Utf8Span requestUri)
        {
            var parsedBytes = buffer.SliceTo(HttpRequestReader.s_SP, out var uriSpan);
            requestUri = new Utf8Span(uriSpan);
            return parsedBytes;
        }

        internal static int TryParseHttpVersion(ReadOnlySpan<byte> buffer, out Utf8Span httpVersion)
        {
            var parsedBytes = buffer.SliceTo(HttpRequestReader.s_CR, HttpRequestReader.s_LF, out var versionSpan);
            httpVersion = new Utf8Span(versionSpan);
            return parsedBytes;
        }

        internal static int TryParseHeaders(ReadOnlySpan<byte> bytes, out HttpHeadersSingleSegment headers)
        {
            var index = bytes.IndexOf(HttpRequestReader.Eoh);
            if(index == -1)
            {
                headers = default;
                return 0;
            }

            headers = new HttpHeadersSingleSegment(bytes.Slice(0, index + HttpRequestReader.Eol.Length));
            return index + HttpRequestReader.Eoh.Length;
        }
    }
}
