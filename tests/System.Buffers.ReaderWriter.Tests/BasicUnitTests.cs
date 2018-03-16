// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Buffers.Text;
using System.Buffers.Writer;
using System.Diagnostics;
using System.Text;
using System.Text.Utf8;
using System.Threading;
using Xunit;

namespace System.Buffers.Tests
{
    public class BasicUnitTests
    {
        private static Utf8String _crlf = (Utf8String)"\r\n";
        private static Utf8String _eoh = (Utf8String)"\r\n\r\n"; // End Of Headers
        private static Utf8String _http11OK = (Utf8String)"HTTP/1.1 200 OK\r\n";
        private static Utf8String _headerServer = (Utf8String)"Server: Custom";
        private static Utf8String _headerContentLength = (Utf8String)"Content-Length: ";
        private static Utf8String _headerContentLengthZero = (Utf8String)"Content-Length: 0\r\n";
        private static Utf8String _headerContentTypeText = (Utf8String)"Content-Type: text/plain\r\n";

        private static Utf8String _plainTextBody = (Utf8String)"Hello, World!";

        static Sink _sink = new Sink(4096);
        static string s_response = "HTTP/1.1 200 OK\r\nServer: Custom\r\nDate: Fri, 16 Mar 2018 10:22:15 GMT\r\nContent-Type: text/plain\r\nContent-Length: 13\r\n\r\nHello, World!";

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
    }

    class Sink : IBufferWriter<byte>
    {
        byte[] _buffer;
        int _written;

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

        public override string ToString()
        {
            return Encoding.UTF8.GetString(_buffer, 0, _written);
        }
    }

    static class DateHeader
    {
        const int prefixLength = 8; // "\r\nDate: ".Length
        const int dateTimeRLength = 29; // Wed, 14 Mar 2018 14:20:00 GMT
        const int suffixLength = 2; // crlf
        const int suffixIndex = dateTimeRLength + prefixLength;

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
                if (!Utf8Formatter.TryFormat(value, s_headerBytesScratch.AsSpan().Slice(prefixLength), out int written, 'R'))
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
}
