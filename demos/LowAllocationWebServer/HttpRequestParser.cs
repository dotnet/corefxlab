using System.Text.Utf8;

namespace System.Net.Http.Buffered
{
    // TODO: I would like to use Utf8String instead of this type.
    // But the issue is that this type "borrows" native bytespan, and I don't want to make Utf8String unsafe
    public struct Utf8Span
    {
        static ByteSpan CreateEmptyByteSpan()
        {
            unsafe { return new ByteSpan(null, 0); }
        }
        static Utf8Span s_empty = new Utf8Span(CreateEmptyByteSpan());
        ByteSpan _bytes;

        public Utf8Span(ByteSpan bytes)
        {
            _bytes = bytes;
        }

        public static Utf8Span Empty { get { return s_empty; } }

        public int Length
        {
            get { return _bytes.Length; }
        }

        public override bool Equals(object obj)
        {
            throw new InvalidOperationException("this should not be called");
        }

        public bool Equals(Utf8String other)
        {
            if (Length != other.Length) return false;
            for(int i=0; i<Length; i++) {
                if (_bytes[i] != other.Bytes[i]) return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            throw new InvalidOperationException("you don't want it in a hashtable, do you?");
        }
    }

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
        public Utf8Span RequestUri;

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

        public Utf8Span ReadRequestUri()
        {
            Utf8Span requestUri;
            int parsedBytes;
            if (!HttpRequestParser.TryParseRequestUri(Buffer, out requestUri, out parsedBytes))
            {
                return Utf8Span.Empty;
            }
            Buffer = Buffer.Slice(parsedBytes);
            return requestUri;
        }

        Utf8Span ReadHttpVersionAsUtf8String()
        {
            Utf8Span httpVersion;
            int parsedBytes;
            if (!HttpRequestParser.TryParseHttpVersion(Buffer, out httpVersion, out parsedBytes))
            {
                return Utf8Span.Empty;
            }
            Buffer = Buffer.Slice(parsedBytes);
            return httpVersion;
        }

        public HttpVersion ReadHttpVersion()
        {
            ByteSpan oldBuffer = Buffer;
            Utf8Span version = ReadHttpVersionAsUtf8String();

            if (version.Equals(s_Http1_1))
            {
                return HttpVersion.V1_1;
            }
            else if (version.Equals(s_Http2_0))
            {
                return HttpVersion.V2_0;
            }
            else if (version.Equals(s_Http2_0))
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
            int parsedBytes;
            var header = SliceTo(Buffer, s_CR, s_LF, out parsedBytes);
            Buffer = Buffer.Slice(parsedBytes);
            return new Utf8Span(header);
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
        static readonly Utf8String s_Get = new Utf8String("GET ");
        static readonly Utf8String s_Post = new Utf8String("POST ");
        static readonly Utf8String s_Put = new Utf8String("PUT ");
        static readonly Utf8String s_Delete = new Utf8String("DELETE ");

        public static bool TryParseRequestLine(ByteSpan buffer, out HttpRequestLine requestLine)
        {
            int parsedBytes;
            return TryParseRequestLine(buffer, out requestLine, out parsedBytes);
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
        public static bool TryParseRequestUri(ByteSpan buffer, out Utf8Span requestUri, out int parsedBytes)
        {
            var uriSpan = HttpRequestReader.SliceTo(buffer, HttpRequestReader.s_SP, out parsedBytes);
            requestUri = new Utf8Span(uriSpan);
            return parsedBytes != 0;
        }
        public static bool TryParseHttpVersion(ByteSpan buffer, out Utf8Span httpVersion, out int parsedBytes)
        {
            var versionSpan = HttpRequestReader.SliceTo(buffer, HttpRequestReader.s_CR, HttpRequestReader.s_LF, out parsedBytes);
            httpVersion = new Utf8Span(versionSpan);
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

    public static class UriParser
    {
        // <heriarchy> [? <query> ] [# <fragment> ]
        // <query> -> <key>=<value> <separator>
        // <separator> -> (not defined but either & or ;)
    }
}
