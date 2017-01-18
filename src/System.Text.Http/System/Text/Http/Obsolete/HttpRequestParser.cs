﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Utf8;

namespace System.Text.Http.SingleSegment
{
    [Obsolete("Use System.Text.Http.HttpRequest")]
    public struct HttpRequestSingleSegment
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
            int parsed;
            HttpRequestLine requestLine;
            if (!HttpRequestParser.TryParseRequestLine(bytes, out requestLine, out parsed)){
                throw new NotImplementedException("request line parser");
            }
            bytes = bytes.Slice(parsed);

            HttpHeadersSingleSegment headers;
            if (!HttpRequestParser.TryParseHeaders(bytes, out headers, out parsed))
            {
                throw new NotImplementedException("headers parser");
            }
            var body = bytes.Slice(parsed);

            var request = new HttpRequestSingleSegment(requestLine, headers, body);

            return request;
        }
    }

    public struct HttpRequestReader
    {
        static readonly Utf8String s_Http1_0 = new Utf8String("HTTP/1.0");
        static readonly Utf8String s_Http1_1 = new Utf8String("HTTP/1.1");
        static readonly Utf8String s_Http2_0 = new Utf8String("HTTP/2.0");

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

        internal Utf8String ReadRequestUri()
        {
            Utf8String requestUri;
            int parsedBytes;
            if (!HttpRequestParser.TryParseRequestUri(Buffer, out requestUri, out parsedBytes))
            {
                return Utf8String.Empty;
            }
            Buffer = Buffer.Slice(parsedBytes);
            return requestUri;
        }

        Utf8String ReadHttpVersionAsUtf8String()
        {
            Utf8String httpVersion;
            int parsedBytes;
            if (!HttpRequestParser.TryParseHttpVersion(Buffer, out httpVersion, out parsedBytes))
            {
                return Utf8String.Empty;
            }
            Buffer = Buffer.Slice(parsedBytes + Eol.Length);
            return httpVersion;
        }

        internal HttpVersion ReadHttpVersion()
        {
            ReadOnlySpan<byte> oldBuffer = Buffer;
            Utf8String version = ReadHttpVersionAsUtf8String();

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

        public Utf8String ReadHeader()
        {
            int parsedBytes;
            var header = Buffer.SliceTo(s_CR, s_LF, out parsedBytes);
            if (parsedBytes > Buffer.Length)
            {
                Buffer = Buffer.Slice(parsedBytes);
            }
            else
            {
                Buffer = new Span<byte>();
            }
            return new Utf8String(header);
        }        
    }

    static class HttpRequestParser
    {
        // TODO: these copies should be eliminated
        static readonly Utf8String s_Get = new Utf8String("GET ");
        static readonly Utf8String s_Post = new Utf8String("POST ");
        static readonly Utf8String s_Put = new Utf8String("PUT ");
        static readonly Utf8String s_Delete = new Utf8String("DELETE ");

        internal static bool TryParseRequestLine(ReadOnlySpan<byte> buffer, out HttpRequestLine requestLine, out int totalParsedBytes)
        {
            requestLine = new HttpRequestLine();
            totalParsedBytes = 0;

            var reader = new HttpRequestReader();
            reader.Buffer = buffer;

            requestLine.Method = reader.ReadMethod();
            if(requestLine.Method == HttpMethod.Unknown) { return false; }

            requestLine.RequestUri = reader.ReadRequestUri().Bytes.ToArray();
            if(requestLine.RequestUri.Length == 0) { return false; }
            reader.Buffer = reader.Buffer.Slice(1);

            requestLine.Version = reader.ReadHttpVersion();
            if (requestLine.Version == HttpVersion.Unknown) { return false; }

            totalParsedBytes = buffer.Length - reader.Buffer.Length;
            return true;
        }

        internal static bool TryParseMethod(ReadOnlySpan<byte> buffer, out HttpMethod method, out int parsedBytes)
        {
            var bufferString = new Utf8String(buffer);
            if(bufferString.StartsWith(s_Get))
            {
                method = HttpMethod.Get;
                parsedBytes = s_Get.Length;
                return true;
            }

            if (bufferString.StartsWith(s_Post))
            {
                method = HttpMethod.Post;
                parsedBytes = s_Post.Length;
                return true;
            }

            if (bufferString.StartsWith(s_Put))
            {
                method = HttpMethod.Put;
                parsedBytes = s_Put.Length;
                return true;
            }

            if (bufferString.StartsWith(s_Delete))
            {
                method = HttpMethod.Delete;
                parsedBytes = s_Delete.Length;
                return true;
            }

            method = HttpMethod.Unknown;
            parsedBytes = 0;
            return false;
        }

        internal static bool TryParseRequestUri(ReadOnlySpan<byte> buffer, out Utf8String requestUri, out int parsedBytes)
        {
            var uriSpan = buffer.SliceTo(HttpRequestReader.s_SP, out parsedBytes);
            requestUri = new Utf8String(uriSpan);
            return parsedBytes != 0;
        }

        internal static bool TryParseHttpVersion(ReadOnlySpan<byte> buffer, out Utf8String httpVersion, out int parsedBytes)
        {
            var versionSpan = buffer.SliceTo(HttpRequestReader.s_CR, HttpRequestReader.s_LF, out parsedBytes);
            httpVersion = new Utf8String(versionSpan);
            return parsedBytes != 0;
        }

        internal static bool TryParseHeaders(ReadOnlySpan<byte> bytes, out HttpHeadersSingleSegment headers, out int parsed)
        {
            var index = bytes.IndexOf(HttpRequestReader.Eoh);
            if(index == -1)
            {
                headers = default(HttpHeadersSingleSegment);
                parsed = 0;
                return false;
            }

            headers = new HttpHeadersSingleSegment(bytes.Slice(0, index + HttpRequestReader.Eol.Length));
            parsed = index + HttpRequestReader.Eoh.Length;
            return true;
        }
    }
}
