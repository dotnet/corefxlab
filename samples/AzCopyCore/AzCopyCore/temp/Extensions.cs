using System.Buffers.Text;
using System.Diagnostics;
using System.IO;
using System.IO.Pipelines;
using System.Text.Http.Parser;
using System.Threading.Tasks;

namespace System.Buffers
{
    // TODO (pri 3): Add to the platform (but NetStandard does not support the stream APIs)
    static class GeneralExtensions
    {
        /// <summary>
        /// Copies bytes from ReadOnlySequence to a Stream
        /// </summary>
        public static async Task WriteAsync(this Stream stream, ReadOnlySequence<byte> buffer)
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
                ReadOnlySequence<byte> bodyBuffer = result.Buffer;
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

    class ConsoleTraceListener : TraceListener
    {
        public override void Write(string message)
            => Console.Write(message);

        public override void WriteLine(string message)
            => Console.WriteLine(message);

        public override void Fail(string message)
        {
            base.Fail(message);
        }
        public override void Fail(string message, string detailMessage)
        {
            base.Fail(message, detailMessage);
        }
        public override bool IsThreadSafe => false;

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            ConsoleColor color = default;
            if (eventType == TraceEventType.Error || eventType == TraceEventType.Critical)
            {
                color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.WriteLine(eventType.ToString());

            if (eventType == TraceEventType.Error || eventType == TraceEventType.Critical) { 
                Console.ForegroundColor = color;
            }
            
            foreach(var item in data)
            {
                Console.Write("\t");
                Console.WriteLine(data);
            }
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            ConsoleColor color = default;
            if (eventType == TraceEventType.Error || eventType == TraceEventType.Critical)
            {
                color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.WriteLine(format, args);

            if (eventType == TraceEventType.Error || eventType == TraceEventType.Critical)
            {
                Console.ForegroundColor = color;
            }
        }
    }
}
