// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.IO.Pipelines;
using System.Text.JsonLab.Tests.Resources;
using System.Threading.Tasks;
using Xunit;

namespace System.Text.JsonLab.Tests
{
    public class ReadFromPipeTests : IDisposable
    {
        public ReadFromPipeTests()
        {
            string jsonString = TestJson.Json40KB;
            _dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            _written = 0;
            _pipe = new Pipe();
        }

        public void Dispose()
        {
            _pipe.Writer.Complete();
            _pipe.Reader.Complete();
        }

        private readonly Pipe _pipe;
        private readonly byte[] _dataUtf8;
        private int _written;

        [Fact]
        public void ReadFromPipe()
        {
            var taskReader = Task.Run(async () =>
            {
                SequencePosition position = default;
                JsonReaderState state = default;
                while (true)
                {
                    ReadResult result = await _pipe.Reader.ReadAsync();
                    ReadOnlySequence<byte> data = result.Buffer;
                    bool isFinalBlock = data.Length == 0;   // TODO: Fix isFinalBlock condition, if Writer is done instead, isLastSegment?
                    (state, position) = ProcessData(data, isFinalBlock, state);
                    if (isFinalBlock)
                        break;
                    _pipe.Reader.AdvanceTo(position);
                }
            });

            var taskWriter = Task.Run(async () =>
            {
                while (true)
                {
                    Memory<byte> mem = default;
                    bool isLastSegment = false;
                    if (_written >= _dataUtf8.Length - 4_000)
                    {
                        isLastSegment = true;
                        mem = _dataUtf8.AsMemory(_written);
                    }
                    else
                    {
                        mem = _dataUtf8.AsMemory(_written, 4_000);
                    }
                    FlushResult result = await _pipe.Writer.WriteAsync(mem);
                    _written += 4000;
                    if (isLastSegment)
                        break;
                }
                _pipe.Writer.Complete();
            });

            Task.WaitAll(taskReader, taskWriter);
        }

        private static (JsonReaderState, SequencePosition) ProcessData(ReadOnlySequence<byte> ros, bool isFinalBlock, JsonReaderState state = default)
        {
            var json = new Utf8JsonReader(ros, isFinalBlock, state);
            while (json.Read()) ;
            json.Dispose();
            return (json.State, json.Position);
        }
    }
}
