// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Operations;
using System.Buffers.Text;
using System.Buffers.Writer;
using System.Diagnostics;
using System.Text;
using System.Text.Utf8;
using Xunit;

namespace System.Buffers.Tests
{
    public class BasicUnitTests
    {
        private static readonly Utf8String _crlf = (Utf8String)"\r\n";
        private static readonly Utf8String _eoh = (Utf8String)"\r\n\r\n"; // End Of Headers
        private static readonly Utf8String _http11OK = (Utf8String)"HTTP/1.1 200 OK\r\n";
        private static readonly Utf8String _headerServer = (Utf8String)"Server: Custom";
        private static readonly Utf8String _headerContentLength = (Utf8String)"Content-Length: ";
        private static readonly Utf8String _headerContentLengthZero = (Utf8String)"Content-Length: 0\r\n";
        private static readonly Utf8String _headerContentTypeText = (Utf8String)"Content-Type: text/plain\r\n";

        private static Utf8String _plainTextBody = (Utf8String)"Hello, World!";
        private static Sink _sink = new Sink(4096);
        private static readonly string s_response = "HTTP/1.1 200 OK\r\nServer: Custom\r\nDate: Fri, 16 Mar 2018 10:22:15 GMT\r\nContent-Type: text/plain\r\nContent-Length: 13\r\n\r\nHello, World!";

        [Fact]
        public void WritePlainText()
        {
            DateHeader.SetDateValues(new DateTimeOffset(2018, 3, 16, 10, 22, 15, 10, TimeSpan.FromMilliseconds(0)));

            _sink.Reset();
            var writer = BufferWriter.Create(_sink);

            // HTTP 1.1 OK
            writer.Write(_http11OK);

            // Server headers
            writer.Write(_headerServer);

            // Date header
            writer.Write(DateHeader.HeaderBytes);

            // Content-Type header
            writer.Write(_headerContentTypeText);

            // Content-Length header
            writer.Write(_headerContentLength);
            writer.Write((ulong)_plainTextBody.Bytes.Length);

            // End of headers
            writer.Write(_eoh);

            // Body
            writer.Write(_plainTextBody);
            writer.Flush();

            var result = _sink.ToString();
            Assert.Equal(s_response, _sink.ToString());
        }

        [Fact]
        public void BufferWriterTransform()
        {
            byte[] buffer = new byte[10];
            var writer = BufferWriter.Create(buffer.AsSpan());
            var transformation = new TransformationFormat(new RemoveTransformation(2));
            ReadOnlyMemory<byte> value = new byte[] { 1, 2, 3 };
            writer.WriteBytes(value, transformation);
            Assert.Equal(-1, buffer.AsSpan().IndexOf((byte)2));
        }

        [Fact]
        public void WriteLineString()
        {
            _sink.Reset();
            var writer = BufferWriter.Create(_sink);
            var newLine = new byte[] { (byte)'X', (byte)'Y' };
            writer.WriteLine("hello world", newLine);
            writer.WriteLine("!", newLine);

            writer.Flush();
            var result = _sink.ToString();
            Assert.Equal("hello worldXY!XY", result);
        }

        [Fact]
        public void WriteLineUtf8String()
        {
            _sink.Reset();
            var writer = BufferWriter.Create(_sink);
            var newLine = new byte[] { (byte)'X', (byte)'Y' };
            writer.WriteLine((Utf8String)"hello world", newLine);
            writer.WriteLine((Utf8String)"!", newLine);

            writer.Flush();
            var result = _sink.ToString();
            Assert.Equal("hello worldXY!XY", result);
        }

        [Fact]
        public void WriteInt32Transformed()
        {
            TransformationFormat widen = new TransformationFormat(new AsciiToUtf16());
            _sink.Reset();
            var writer = BufferWriter.Create(_sink);
            writer.Write(255, widen);
            writer.Flush();

            var result = _sink.ToStringAssumingUtf16Buffer();
            Assert.Equal("255", result);
        }
    }

    internal class Sink : IBufferWriter<byte>
    {
        private byte[] _buffer;
        private int _written;

        public Sink(int size)
        {
            _buffer = new byte[4096];
            _written = 0;
        }
        public void Reset() => _written = 0;

        public void Advance(int count)
        {
            _written += count;
            if (_written > _buffer.Length) throw new ArgumentOutOfRangeException(nameof(count));
        }

        public Memory<byte> GetMemory(int sizeHint = 0)
            => _buffer.AsMemory(_written, _buffer.Length - _written);

        public Span<byte> GetSpan(int sizeHint = 0)
            => _buffer.AsSpan(_written, _buffer.Length - _written);

        public Span<byte> WrittenBytes => _buffer.AsSpan(0, _written);

        public override string ToString()
        {
            return Encoding.UTF8.GetString(_buffer, 0, _written);
        }

        public string ToStringAssumingUtf16Buffer()
        {
            return Encoding.Unicode.GetString(_buffer, 0, _written);
        }
    }

    internal static class DateHeader
    {
        private const int prefixLength = 8; // "\r\nDate: ".Length
        private const int dateTimeRLength = 29; // Wed, 14 Mar 2018 14:20:00 GMT
        private const int suffixLength = 2; // crlf
        private const int suffixIndex = dateTimeRLength + prefixLength;

        private static byte[] s_headerBytesMaster = new byte[prefixLength + dateTimeRLength + suffixLength];
        private static byte[] s_headerBytesScratch = new byte[prefixLength + dateTimeRLength + suffixLength];

        static DateHeader()
        {
            var utf8 = Encoding.ASCII.GetBytes("\r\nDate: ").AsSpan();
            utf8.CopyTo(s_headerBytesMaster);
            utf8.CopyTo(s_headerBytesScratch);
            s_headerBytesMaster[suffixIndex] = (byte)'\r';
            s_headerBytesMaster[suffixIndex + 1] = (byte)'\n';
            s_headerBytesScratch[suffixIndex] = (byte)'\r';
            s_headerBytesScratch[suffixIndex + 1] = (byte)'\n';
            SetDateValues(DateTimeOffset.UtcNow);
        }

        public static ReadOnlySpan<byte> HeaderBytes => s_headerBytesMaster;

        public static void SetDateValues(DateTimeOffset value)
        {
            lock (s_headerBytesScratch)
            {
                if (!Utf8Formatter.TryFormat(value, s_headerBytesScratch.AsSpan(prefixLength), out int written, 'R'))
                {
                    throw new Exception("date time format failed");
                }
                Debug.Assert(written == dateTimeRLength);
                var temp = s_headerBytesMaster;
                s_headerBytesMaster = s_headerBytesScratch;
                s_headerBytesScratch = temp;
            }
        }
    }

    internal class AsciiToUtf16 : IBufferTransformation
    {
        public OperationStatus Execute(ReadOnlySpan<byte> input, Span<byte> output, out int consumed, out int written)
        {
            throw new NotImplementedException();
        }

        public OperationStatus Transform(Span<byte> buffer, int dataLength, out int written)
        {
            written = dataLength * 2;
            if (buffer.Length < written) return OperationStatus.DestinationTooSmall;
            for(int i = written - 1; i > 0; i-=2)
            {
                buffer[i] = 0;
                buffer[i - 1] = buffer[i / 2];
            }
            return OperationStatus.Done;
        }
    }
}
