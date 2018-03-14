// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Buffers;
using System.Buffers.Writer;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace System.IO.Pipelines.Performance.Tests
{
    [Config(typeof(CoreConfig))]
    public class PipeThroughput
    {
        private const int WriteLength = 57;
        private const int InnerLoopCount = 512;

        private readonly byte[][] _plaintextWrites =
        {
            new byte[] { 72, 84, 84, 80, 47, 49, 46, 49, 32}, // HTTP/1.1
            new byte[] { 50, 48, 48, 32, 79, 75}, // 200 OK
            new byte[] { 13, 10, 68, 97, 116, 101, 58, 32, 87, 101, 100, 44, 32, 50, 50, 32, 77, 97, 114, 32, 50, 48, 49, 55, 32, 50, 49, 58, 51, 55, 58, 49, 52, 32, 71, 77, 84}, // \r\nDate: Wed, 22 Mar 2017 21:37:14 GMT
            new byte[] { 13, 10, 67, 111, 110, 116, 101, 110, 116, 45, 84, 121, 112, 101, 58, 32}, // \r\nContent-Type:
            new byte[] { 13, 10, 83, 101, 114, 118, 101, 114, 58, 32, 75, 101, 115, 116, 114, 101, 108}, // \r\nServer: Kestrel
            new byte[] { 13, 10, 67, 111, 110, 116, 101, 110, 116, 45, 76, 101, 110, 103, 116, 104, 58, 32}, // \r\nContent-Length:
            new byte[] { 13, 10, 13, 10}, // \r\n\r\n
            new byte[] { 72, 101, 108, 108, 111, 44, 32, 87, 111, 114, 108, 100, 33}, // Hello, World!
        };

        private Pipe _pipe;

        [GlobalSetup]
        public void Setup()
        {
            _pipe = new Pipe();
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void ParseLiveAspNetTwoTasks()
        {
            var writing = Task.Run(async () =>
            {
                for (int i = 0; i < InnerLoopCount; i++)
                {
                    var writableBuffer = _pipe.Writer;
                    writableBuffer.Advance(WriteLength);
                    await writableBuffer.FlushAsync();
                }
            });

            var reading = Task.Run(async () =>
            {
                long remaining = InnerLoopCount * WriteLength;
                while (remaining != 0)
                {
                    var result = await _pipe.Reader.ReadAsync();
                    var buffer = result.Buffer;
                    remaining -= buffer.Length;
                    _pipe.Reader.AdvanceTo(buffer.End, buffer.End);
                }
            });

            Task.WaitAll(writing, reading);
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void ParseLiveAspNetInline()
        {
            for (int i = 0; i < InnerLoopCount; i++)
            {
                var writableBuffer = _pipe.Writer;
                writableBuffer.Advance(WriteLength);
                writableBuffer.FlushAsync().GetAwaiter().GetResult();
                var result = _pipe.Reader.ReadAsync().GetAwaiter().GetResult();
                var buffer = result.Buffer;
                _pipe.Reader.AdvanceTo(buffer.End, buffer.End);
            }
        }


        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void LongWriteInline()
        {
            for (int i = 0; i < InnerLoopCount; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    var writableBuffer = _pipe.Writer;
                    writableBuffer.GetMemory(WriteLength);
                    writableBuffer.Advance(WriteLength);
                    if (j == 14)
                    {
                        writableBuffer.FlushAsync().GetAwaiter().GetResult();
                    }
                }

                var result = _pipe.Reader.ReadAsync().GetAwaiter().GetResult();
                var buffer = result.Buffer;
                _pipe.Reader.AdvanceTo(buffer.End, buffer.End);
            }
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void WritePlaintextResponse()
        {
            for (int i = 0; i < InnerLoopCount; i++)
            {
                var writableBuffer = _pipe.Writer;

                foreach (var write in _plaintextWrites)
                {
                    writableBuffer.Write(write);
                }

                writableBuffer.FlushAsync().GetAwaiter().GetResult();
                var result = _pipe.Reader.ReadAsync().GetAwaiter().GetResult();
                var buffer = result.Buffer;
                _pipe.Reader.AdvanceTo(buffer.End, buffer.End);
            }
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void WriteSpanPlaintextResponse()
        {
            for (int i = 0; i < InnerLoopCount; i++)
            {
                var writableBuffer = _pipe.Writer;

                foreach (var write in _plaintextWrites)
                {
                    writableBuffer.Write((ReadOnlySpan<byte>)write);
                }

                writableBuffer.FlushAsync().GetAwaiter().GetResult();
                var result = _pipe.Reader.ReadAsync().GetAwaiter().GetResult();
                var buffer = result.Buffer;
                _pipe.Reader.AdvanceTo(buffer.End, buffer.End);
            }
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void WriteableBufferWriterWriteFastPlaintextResponse()
        {
            for (int i = 0; i < InnerLoopCount; i++)
            {
                var writableBuffer = _pipe.Writer;
                var writer = BufferWriter.Create(writableBuffer);

                foreach (var write in _plaintextWrites)
                {
                    writer.Write(write);
                }

                writableBuffer.FlushAsync().GetAwaiter().GetResult();
                var result = _pipe.Reader.ReadAsync().GetAwaiter().GetResult();
                var buffer = result.Buffer;
                _pipe.Reader.AdvanceTo(buffer.End, buffer.End);
            }
        }
    }
}
