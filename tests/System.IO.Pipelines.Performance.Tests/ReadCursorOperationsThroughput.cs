using System.Buffers;
using System.Buffers.Reader;
using System.Collections.Generic;
using System.IO.Pipelines.Testing;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace System.IO.Pipelines.Performance.Tests
{
    [Config(typeof(CoreConfig))]
    public class ReadCursorOperationsThroughput
    {
        private const int InnerLoopCount = 1024;

        private const string plaintextRequest = "GET /plaintext HTTP/1.1\r\nHost: www.example.com\r\n\r\n";

        private const string liveaspnetRequest = "GET https://live.asp.net/ HTTP/1.1\r\n" +
            "Host: live.asp.net\r\n" +
            "Connection: keep-alive\r\n" +
            "Upgrade-Insecure-Requests: 1\r\n" +
            "User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.99 Safari/537.36\r\n" +
            "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8\r\n" +
            "DNT: 1\r\n" +
            "Accept-Encoding: gzip, deflate, sdch, br\r\n" +
            "Accept-Language: en-US,en;q=0.8\r\n" +
            "Cookie: __unam=7a67379-1s65dc575c4-6d778abe-1; omniID=9519gfde_3347_4762_8762_df51458c8ec2\r\n\r\n";

        private ReadOnlySequence<byte> _plainTextBuffer;
        private ReadOnlySequence<byte> _plainTextPipelinedBuffer;
        private ReadOnlySequence<byte> _liveAspNetBuffer;
        private ReadOnlySequence<byte> _liveAspNetMultiBuffer;

        [GlobalSetup]
        public void Setup()
        {
            var liveaspnetRequestBytes = Encoding.UTF8.GetBytes(liveaspnetRequest);
            var pipelinedRequests = string.Concat(Enumerable.Repeat(plaintextRequest, 16));
            _plainTextPipelinedBuffer = new ReadOnlySequence<byte>(Encoding.UTF8.GetBytes(pipelinedRequests));
            _plainTextBuffer = new ReadOnlySequence<byte>(Encoding.UTF8.GetBytes(plaintextRequest));
            _liveAspNetBuffer = new ReadOnlySequence<byte>(liveaspnetRequestBytes);

            // Split the liveaspnetRequest across 3 byte[]
            var remaining = liveaspnetRequestBytes.Length;
            var consumed = 0;
            var liveAspNetBuffers = new List<byte[]>();
            var chunk = remaining / 3;

            while (remaining > 0)
            {
                var bytes = new byte[Math.Min(chunk, remaining)];
                Buffer.BlockCopy(liveaspnetRequestBytes, consumed, bytes, 0, bytes.Length);
                consumed += bytes.Length;
                remaining -= bytes.Length;

                liveAspNetBuffers.Add(bytes);
            }

            _liveAspNetMultiBuffer = BufferUtilities.CreateBuffer(liveAspNetBuffers.ToArray());
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void SeekPlainText()
        {
            FindAllNewLines(_plainTextBuffer);
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void SeekPlainTextReadableBufferReader()
        {
            FindAllNewLinesReadableBufferReader(_plainTextBuffer);
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void SeekPlainTextPipelined()
        {
            FindAllNewLines(_plainTextPipelinedBuffer);
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void SeekPlainTextPipelinedReadableBufferReader()
        {
            FindAllNewLinesReadableBufferReader(_plainTextPipelinedBuffer);
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void SeekLiveAspNet()
        {
            FindAllNewLines(_liveAspNetBuffer);
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void SeekLiveAspNetMultiBuffer()
        {
            FindAllNewLines(_liveAspNetMultiBuffer);
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void SeekLiveAspNetReadableBufferReader()
        {
            FindAllNewLinesReadableBufferReader(_liveAspNetBuffer);
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void SeekLiveAspNetMultiBufferReadableBufferReader()
        {
            FindAllNewLinesReadableBufferReader(_liveAspNetMultiBuffer);
        }

        private static void FindAllNewLinesReadableBufferReader(ReadOnlySequence<byte> buffer)
        {
            var reader = BufferReader.Create(buffer);
            var end = buffer.End;

            while (!reader.End)
            {
                var span = reader.CurrentSegment;

                // Trim the start if we have an index
                if (reader.CurrentSegmentIndex > 0)
                {
                    span = span.Slice(reader.CurrentSegmentIndex);
                }

                while (span.Length > 0)
                {
                    var length = span.IndexOf((byte)'\n');
                    var skip = length;

                    if (length == -1)
                    {
                        var subBuffer = buffer.Slice(reader.Position);
                        var position = subBuffer.PositionOf((byte)'\n');
                        if (position == null)
                        {
                            // We're done
                            return;
                        }

                        length = span.Length;
                        skip = (int)subBuffer.Slice(0, position.Value).Length + 1;
                    }
                    else
                    {
                        length += 1;
                        skip = length;
                    }

                    span = span.Slice(length);
                    reader.Advance(skip);
                }
            }
        }

        private static void FindAllNewLines(ReadOnlySequence<byte> buffer)
        {
            while (true)
            {
                var position = buffer.PositionOf((byte)'\n');
                if (position == null)
                {
                    break;
                }

                buffer = buffer.Slice(position.Value).Slice(1);
            }
        }
    }
}
