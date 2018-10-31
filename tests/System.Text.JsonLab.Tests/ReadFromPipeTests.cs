// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;
using System.Buffers;
using System.IO;
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
            string jsonString = TestJson.Json400KB;
            _dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            _written = 0;
            _pipe = new Pipe();

            Stream stream = new MemoryStream(_dataUtf8);
            TextReader reader = new StreamReader(stream, Encoding.UTF8, false, 1024, true);
            _expectedString = NewtonsoftReturnStringHelper(reader);
        }

        public void Dispose()
        {
            _pipe.Writer.Complete();
            _pipe.Reader.Complete();
        }

        private readonly Pipe _pipe;
        private readonly byte[] _dataUtf8;
        private int _written;
        private string _expectedString;

        public static string NewtonsoftReturnStringHelper(TextReader reader)
        {
            var sb = new StringBuilder();
            var json = new JsonTextReader(reader);
            while (json.Read())
            {
                if (json.TokenType == JsonToken.PropertyName)
                {
                    if (json.Value != null)
                    {
                        sb.Append(json.Value);
                    }
                }
            }
            return sb.ToString();
        }

        [Fact]
        public void ReadFromPipeUsingSequence()
        {
            string actual = "";
            var taskReader = Task.Run(async () =>
            {
                JsonReaderState state = default;
                string str = "";
                while (true)
                {
                    ReadResult result = await _pipe.Reader.ReadAsync();
                    ReadOnlySequence<byte> data = result.Buffer;
                    bool isFinalBlock = result.IsCompleted;
                    (state, str) = ProcessData(data, isFinalBlock, state);
                    _pipe.Reader.AdvanceTo(state.Position);
                    actual += str;
                    if (isFinalBlock)
                        break;

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

            Task[] tasks = new Task[] { taskReader, taskWriter };
            Task.WaitAll(tasks, 30_000);    // The test shouldn't take more than 30 seconds to finish.

            Assert.Equal(_expectedString, actual);
        }

        private static (JsonReaderState, string) ProcessData(ReadOnlySequence<byte> ros, bool isFinalBlock, JsonReaderState state = default)
        {
            var builder = new StringBuilder();
            var json = new JsonUtf8Reader(ros, isFinalBlock, state);
            while (json.Read())
            {
                switch (json.TokenType)
                {
                    case JsonTokenType.PropertyName:
                        json.TryGetValueAsString(out string value);
                        builder.Append(value);
                        break;
                }
            }
            Assert.Equal(json.BytesConsumed, json.CurrentState.BytesConsumed);
            Assert.Equal(json.Position, json.CurrentState.Position);
            return (json.CurrentState, builder.ToString());
        }

        // (Expected a '"' to indicate end of string, but instead reached end of data. LineNumber: 12940 | BytePosition: 6.)
        [Fact(Skip = "This needs to be fixed and re-enabled or removed. Fails specficially on Unix.")]
        public void ReadFromPipeUsingSpan()
        {
            string actual = "";
            var taskReader = Task.Run(async () =>
            {
                SequencePosition position = default;
                JsonReaderState state = default;
                string str = "";
                while (true)
                {
                    ReadResult result = await _pipe.Reader.ReadAsync();
                    ReadOnlySequence<byte> data = result.Buffer;
                    bool isFinalBlock = result.IsCompleted;
                    (state, position, str) = ProcessDataSpan(data, isFinalBlock, state);
                    _pipe.Reader.AdvanceTo(position);
                    actual += str;
                    if (isFinalBlock)
                        break;
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

            Task[] tasks = new Task[] { taskReader, taskWriter };
            Task.WaitAll(tasks, 30_000);    // The test shouldn't take more than 30 seconds to finish.

            Assert.Equal(_expectedString, actual);
        }

        private static (JsonReaderState, SequencePosition, string) ProcessDataSpan(ReadOnlySequence<byte> ros, bool isFinalBlock, JsonReaderState state = default)
        {
            var builder = new StringBuilder();
            ReadOnlySpan<byte> leftOver = default;
            byte[] pooledArray = null;
            JsonUtf8Reader json = default;
            long totalConsumed = 0;
            foreach (ReadOnlyMemory<byte> mem in ros)
            {
                ReadOnlySpan<byte> span = mem.Span;

                if (leftOver.Length > int.MaxValue - span.Length)
                    throw new ArgumentOutOfRangeException("Current sequence segment size is too large to fit left over data from the previous segment into a 2 GB buffer.");

                pooledArray = ArrayPool<byte>.Shared.Rent(span.Length + leftOver.Length);    // This is guaranteed to not overflow
                Span<byte> bufferSpan = pooledArray.AsSpan(0, leftOver.Length + span.Length);
                leftOver.CopyTo(bufferSpan);
                span.CopyTo(bufferSpan.Slice(leftOver.Length));

                json = new JsonUtf8Reader(bufferSpan, isFinalBlock, state);

                while (json.Read())
                {
                    switch (json.TokenType)
                    {
                        case JsonTokenType.PropertyName:
                            json.TryGetValueAsString(out string value);
                            builder.Append(value);
                            break;
                    }
                }

                if (json.BytesConsumed < bufferSpan.Length)
                {
                    leftOver = bufferSpan.Slice((int)json.BytesConsumed);
                }
                else
                {
                    leftOver = default;
                }
                totalConsumed += json.BytesConsumed;
                Assert.Equal(json.BytesConsumed, json.CurrentState.BytesConsumed);
                Assert.Equal(json.Position, json.CurrentState.Position);
                if (pooledArray != null)    // TODO: Will this work if data spans more than two segments?
                    ArrayPool<byte>.Shared.Return(pooledArray);

                state = json.CurrentState;
            }
            return (json.CurrentState, ros.GetPosition(totalConsumed), builder.ToString());
        }
    }
}
