using System.Buffers.Text;
using System.Diagnostics;
using System.IO;
using System.IO.Pipelines;
using System.Text;
using System.Text.Http.Parser;
using System.Threading.Tasks;

namespace System.Buffers
{
    // TODO (pri 3): Add to the platform (but NetStandard does not support the stream APIs)
    static class GeneralExtensions
    {
        /// <summary>
        /// Copies bytes from ReadOnlyBuffer to a Stream
        /// </summary>
        public static async Task WriteAsync(this Stream stream, ReadOnlyBuffer<byte> buffer)
        {
            for (var position = buffer.Start; buffer.TryGet(ref position, out var memory);)
            {
                await stream.WriteAsync(memory).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Copies bytes from PipeReader to a Stream
        /// </summary>
        public static async Task WriteAsync(this Stream stream, PipeReader reader, ulong bytes)
        {
            while (bytes > 0)
            {
                var result = await reader.ReadAsync();
                ReadOnlyBuffer<byte> bodyBuffer = result.Buffer;
                if (bytes < (ulong)bodyBuffer.Length)
                {
                    throw new NotImplementedException();
                }
                bytes -= (ulong)bodyBuffer.Length;
                await stream.WriteAsync(bodyBuffer).ConfigureAwait(false);
                await stream.FlushAsync().ConfigureAwait(false);
                reader.AdvanceTo(bodyBuffer.End);
            }
        }

        /// <summary>
        /// Copies bytes from Stream to PipeWriter 
        /// </summary>
        public static async Task WriteAsync(this PipeWriter writer, Stream stream)
        {
            if (!stream.CanRead) throw new ArgumentException("Stream.CanRead returned false", nameof(stream));
            while (true)
            {
                var buffer = writer.GetMemory();
                if (buffer.Length == 0) throw new NotSupportedException("PipeWriter.GetMemory returned an empty buffer.");
                int read = await stream.ReadAsync(buffer).ConfigureAwait(false);
                if (read == 0) return;
                writer.Advance(read);
                await writer.FlushAsync();
            }
        }
    }

    // TODO (pri 3): Is TraceSource the right logger here? 
    public static class TraceListenerExtensions
    {
        public static void WriteInformation(this TraceSource source, string tag, ReadOnlyMemory<byte> utf8Text)
        {
            if (source.Switch.ShouldTrace(TraceEventType.Information))
            {
                var message = Encodings.Utf8.ToString(utf8Text.Span);
                source.TraceInformation($"{tag}:\n{message}\n");
            }
        }

        public static void WriteError(this TraceSource source, string message)
        {
            if (source.Switch.ShouldTrace(TraceEventType.Error))
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                source.TraceEvent(TraceEventType.Error, 0, message);
                Console.ForegroundColor = color;
            }
        }
    }

    public static class HttpExtensions
    {
        static HttpParser s_headersParser = new HttpParser();
        private const byte ByteLF = (byte)'\n';
        private const byte ByteCR = (byte)'\r';
        private const long maxRequestLineLength = 1024;
        static readonly byte[] s_Eol = Encoding.ASCII.GetBytes("\r\n");
        static readonly byte[] s_http11 = Encoding.ASCII.GetBytes("HTTP/1.1");
        static readonly byte[] s_http10 = Encoding.ASCII.GetBytes("HTTP/1.0");
        static readonly byte[] s_http20 = Encoding.ASCII.GetBytes("HTTP/2.0");

        // TODO (pri 2): move to corfxlab
        public static bool ParseResponseLine<T>(ref T handler, ref ReadOnlyBuffer<byte> buffer, out int consumedBytes) where T : IHttpResponseLineHandler
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
                if(index < 0)
                {
                    consumedBytes = 0;
                    return false;
                }
                if(index > maxRequestLineLength)
                {
                    throw new Exception("invalid response (LF too far)");
                }
                line = line.Slice(0, lf + 1);
            }

            if(line[lf - 1] != ByteCR)
            {
                throw new Exception("invalid response (no CR)");
            }

            Http.Version version;
            if (line.StartsWith(s_http11)) { version = Http.Version.Http11; }
            // TODO (pri 2): add HTTP2 to HTTP.Version
            else if (line.StartsWith(s_http20)) { version = Http.Version.Unknown; }
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

        // TODO (pri 3): Add to the platform, but this would require common logging API
        public static async ValueTask<T> ParseAsync<T>(PipeReader reader, TraceSource log = null)
            where T : IHttpResponseLineHandler, IHttpHeadersHandler, new()
        {
            var result = await reader.ReadAsync();
            ReadOnlyBuffer<byte> buffer = result.Buffer;

            if (log != null) log.WriteInformation("RESPONSE: ", buffer.First);

            var handler = new T();
            if (!ParseResponseLine(ref handler, ref buffer, out int rlConsumed))
            {
                throw new NotImplementedException("could not parse the response");
            }

            buffer = buffer.Slice(rlConsumed);
            if (!s_headersParser.ParseHeaders(ref handler, buffer, out int hdConsumed))
            {
                throw new NotImplementedException("could not parse the response");
            }

            reader.AdvanceTo(buffer.GetPosition(buffer.Start, hdConsumed));

            return handler;
        }
    }

    // TODO (pri 3): Should I use the command line library?
    class CommandOptions
    {
        readonly string[] _options;

        public CommandOptions(string[] options)
        {
            _options = options;
        }

        public bool Contains(string optionName)
        {
            for (int i = 0; i < _options.Length; i++)
            {
                var candidate = _options[i];
                if (candidate.StartsWith(optionName)) return true;
            }
            return false;
        }

        public ReadOnlyMemory<char> Get(string optionName)
        {
            if (optionName.Length < 1) throw new ArgumentOutOfRangeException(nameof(optionName));

            for (int i = 0; i < _options.Length; i++)
            {
                var candidate = _options[i];
                if (candidate.StartsWith(optionName))
                {
                    var option = candidate.AsReadOnlyMemory();
                    return option.Slice(optionName.Length);
                }
            }
            return ReadOnlyMemory<char>.Empty;
        }
    }
}
