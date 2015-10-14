using System.Text;
using System.Text.Utf8;

namespace System.Net.Http.Buffered
{
    public enum HttpMethod : byte
    {
        Unknown = 0,
        Get,
        Post,
        Put,
        Delete,
    }

    public enum HttpVersion : byte
    {
        Unknown = 0,
        V1_0,
        V1_1,
        V2_0,
    }

    public struct HttpRequestLine
    {
        public HttpMethod Method;
        public HttpVersion Version;
        public Utf8String RequestUri;

        public override string ToString()
        {
            return RequestUri.ToString();
        }
    }

    public struct HttpRequestReader
    {
        static readonly Utf8String s_Http1_0 = new Utf8String("HTTP/1.0");
        static readonly Utf8String s_Http1_1 = new Utf8String("HTTP/1.1");
        static readonly Utf8String s_Http2_0 = new Utf8String("HTTP/2.0");

        internal const byte s_SP = 32; // space
        internal const byte s_CR = 13; // carriage return
        internal const byte s_LF = 10; // line feed
        internal const byte s_HT = 9;   // horizontal TAB

        public ByteSpan Buffer;

        public HttpMethod ReadMethod()
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

        public Utf8String ReadRequestUri()
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
            Buffer = Buffer.Slice(parsedBytes);
            return httpVersion;
        }

        public HttpVersion ReadHttpVersion()
        {
            ByteSpan oldBuffer = Buffer;
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
            var header = SliceTo(Buffer, s_CR, s_LF, out parsedBytes);
            if (parsedBytes > Buffer.Length)
            {
                Buffer = Buffer.Slice(parsedBytes);
            }
            else
            {
                Buffer = new ByteSpan();
            }
            return new Utf8String(header);
        }

        internal static ByteSpan SliceTo(ByteSpan buffer, byte terminator, out int consumedBytes)
        {
            int index = 0;
            while (index < buffer.Length)
            {
                if (buffer[index] == terminator)
                {
                    consumedBytes = index + 1;
                    return buffer.Slice(0, index);
                }
                index++;
            }
            consumedBytes = 0;
            unsafe
            {
                return new ByteSpan(null, 0); //TODO: Empty instance should be used
            }
        }
        internal static ByteSpan SliceTo(ByteSpan buffer, byte terminatorFirst, byte terminatorSecond, out int consumedBytes)
        {
            int index = 0;
            while (index < buffer.Length)
            {
                if (buffer[index] == terminatorFirst && buffer.Length > index + 1 && buffer[index + 1] == terminatorSecond)
                {
                    consumedBytes = index + 2;
                    return buffer.Slice(0, index);
                }
                index++;
            }

            consumedBytes = 0;
            unsafe
            {
                return new ByteSpan(null, 0); // TODO: Empty instance should be used
            }
        }
    }

    public static class HttpRequestParser
    {
        // TODO: these copies should be eliminated
        static readonly Utf8String s_Get = new Utf8String("GET ");
        static readonly Utf8String s_Post = new Utf8String("POST ");
        static readonly Utf8String s_Put = new Utf8String("PUT ");
        static readonly Utf8String s_Delete = new Utf8String("DELETE ");

        public static bool TryParseRequestLine(ByteSpan buffer, out HttpRequestLine requestLine)
        {
            int parsedBytes;
            return TryParseRequestLine(buffer, out requestLine, out parsedBytes);
        }

        // TODO: this needs to be smarter
        public static bool IsKeepAlive(this HttpRequestLine request)
        {
            return (request.Version != HttpVersion.V1_0) && (request.Version != HttpVersion.Unknown);
        }

        public static bool TryParseRequestLine(ByteSpan buffer, out HttpRequestLine requestLine, out int totalParsedBytes)
        {
            requestLine = new HttpRequestLine();
            totalParsedBytes = 0;

            var reader = new HttpRequestReader();
            reader.Buffer = buffer;

            requestLine.Method = reader.ReadMethod();
            if(requestLine.Method == HttpMethod.Unknown) { return false; }

            requestLine.RequestUri = reader.ReadRequestUri();
            if(requestLine.RequestUri.Length == 0) { return false; }

            requestLine.Version = reader.ReadHttpVersion();
            if (requestLine.Version == HttpVersion.Unknown) { return false; }

            totalParsedBytes = buffer.Length - reader.Buffer.Length;
            return true;
        }

        public static bool TryParseMethod(ByteSpan buffer, out HttpMethod method, out int parsedBytes)
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
        public static bool TryParseRequestUri(ByteSpan buffer, out Utf8String requestUri, out int parsedBytes)
        {
            var uriSpan = HttpRequestReader.SliceTo(buffer, HttpRequestReader.s_SP, out parsedBytes);
            requestUri = new Utf8String(uriSpan);
            return parsedBytes != 0;
        }
        public static bool TryParseHttpVersion(ByteSpan buffer, out Utf8String httpVersion, out int parsedBytes)
        {
            var versionSpan = HttpRequestReader.SliceTo(buffer, HttpRequestReader.s_CR, HttpRequestReader.s_LF, out parsedBytes);
            httpVersion = new Utf8String(versionSpan);
            return parsedBytes != 0;
        }

        public static bool StartsWith(this ByteSpan left, ReadOnlySpan<byte> right)
        {
            if (left.Length < right.Length) return false;
            for (int index = 0; index < right.Length; index++) {
                if (left[index] != right[index]) return false;
            }
            return true;
        }
    }
}
