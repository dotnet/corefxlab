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

        public ReadOnlySpan<byte> Buffer;

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
            ReadOnlySpan<byte> oldBuffer = Buffer;
            Utf8String version = ReadHttpVersionAsUtf8String();

            if (s_Http1_1.Equals(version))
            {
                return HttpVersion.V1_1;
            }
            else if (s_Http2_0.Equals(version))
            {
                return HttpVersion.V2_0;
            }
            else if (s_Http1_0.Equals((object)version))
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
            Buffer = Buffer.Slice(parsedBytes);
            return new Utf8String(header);
        }

        internal static ReadOnlySpan<byte> SliceTo(ReadOnlySpan<byte> buffer, byte terminator, out int consumedBytes)
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
            return new Span<byte>();
        }
        internal static ReadOnlySpan<byte> SliceTo(ReadOnlySpan<byte> buffer, byte terminatorFirst, byte terminatorSecond, out int consumedBytes)
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
            return new Span<byte>();
        }
    }

    public static class HttpRequestParser
    {
        static readonly Utf8String s_Get = new Utf8String("GET ");
        static readonly Utf8String s_Post = new Utf8String("POST ");
        static readonly Utf8String s_Put = new Utf8String("PUT ");
        static readonly Utf8String s_Delete = new Utf8String("DELETE ");

        public static bool TryParseRequestLine(ReadOnlySpan<byte> buffer, out HttpRequestLine requestLine)
        {
            int parsedBytes;
            return TryParseRequestLine(buffer, out requestLine, out parsedBytes);
        }

        public static bool TryParseRequestLine(ReadOnlySpan<byte> buffer, out HttpRequestLine requestLine, out int totalParsedBytes)
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

        public static bool TryParseMethod(ReadOnlySpan<byte> buffer, out HttpMethod method, out int parsedBytes)
        {
            if(buffer.StartsWith(s_Get.Bytes))
            {
                method = HttpMethod.Get;
                parsedBytes = s_Get.Length;
                return true;
            }

            if (buffer.StartsWith(s_Post.Bytes))
            {
                method = HttpMethod.Post;
                parsedBytes = s_Post.Length;
                return true;
            }

            if (buffer.StartsWith(s_Put.Bytes))
            {
                method = HttpMethod.Put;
                parsedBytes = s_Put.Length;
                return true;
            }

            if (buffer.StartsWith(s_Delete.Bytes))
            {
                method = HttpMethod.Delete;
                parsedBytes = s_Delete.Length;
                return true;
            }

            method = HttpMethod.Unknown;
            parsedBytes = 0;
            return false;
        }
        public static bool TryParseRequestUri(ReadOnlySpan<byte> buffer, out Utf8String requestUri, out int parsedBytes)
        {
            requestUri = new Utf8String(HttpRequestReader.SliceTo(buffer, HttpRequestReader.s_SP, out parsedBytes));
            return parsedBytes != 0;
        }
        public static bool TryParseHttpVersion(ReadOnlySpan<byte> buffer, out Utf8String httpVersion, out int parsedBytes)
        {
            httpVersion = new Utf8String(HttpRequestReader.SliceTo(buffer, HttpRequestReader.s_CR, HttpRequestReader.s_LF, out parsedBytes));
            return parsedBytes != 0;
        }
    }

    public static class UriParser
    {
        // <heriarchy> [? <query> ] [# <fragment> ]
        // <query> -> <key>=<value> <separator>
        // <separator> -> (not defined but either & or ;)
    }
}
