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
            string jsonString = TestJson.Json400KB;
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

        const string expectedStartsWith = "_idindexguidisActivebalancepictureageeyeColornamegendercompanyemailphoneaddressaboutregisteredlatitudelongitudetagsfriendsidnameidnameidnamegreetingfavoriteFruit_idindexguidisActivebalancepictureageeyeColornamegendercompanyemailphoneaddressaboutregisteredlatitudelongitudetagsfriendsidnameidnameidnamegreetingfavoriteFruit_idindexguidisActivebalancepictureageeyeColornamegendercompanyemailphoneaddressaboutregisteredlatitudelongitudetagsfriendsidnameidnameidnamegreetingfavoriteFruit_idindexguidisActivebalancepictureageeyeColornamegendercompanyemailphoneaddressaboutregisteredlatitudelongitudetagsfriendsidnameidnameidnamegreetingfavoriteFruit_idindexguidisActivebalancepictureageeyeColornamegendercompanyemailphoneaddressaboutregisteredlatitudelongitudetagsfriendsidnameidnameidnamegreetingfavoriteFruit_idindexguidisActivebalancepictureageeyeColornamegendercompanyemailphoneaddressaboutregisteredlatitudelongitudetagsfriendsidnameidnameidnamegreetingfavoriteFruit_idindexguidisActivebalancepictureageeyeColornamegendercompanyemailphoneaddressaboutregisteredlatitudelongitudetagsfriendsidnameidnameidnamegreetingfavoriteFruit_idindexguidisActivebalancepictureageeyeColornamegendercompanyemailphoneaddressaboutregiste";

        [Fact]
        public void ReadFromPipe()
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
                    bool isFinalBlock = data.Length == 0;   // TODO: Fix isFinalBlock condition, if Writer is done instead, isLastSegment?
                    (state, position, str) = ProcessData(data, isFinalBlock, state);
                    if (isFinalBlock)
                        break;
                    _pipe.Reader.AdvanceTo(position);
                    actual += str;
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
            Task.WaitAll(tasks, 5_000);    // The test shouldn't take more than 5 seconds to finish.

            Assert.True(actual.StartsWith(expectedStartsWith));
        }

        private static (JsonReaderState, SequencePosition, string) ProcessData(ReadOnlySequence<byte> ros, bool isFinalBlock, JsonReaderState state = default)
        {
            var builder = new StringBuilder();
            var json = new Utf8JsonReader(ros, isFinalBlock, state);
            while (json.Read())
            {
                switch (json.TokenType)
                {
                    case JsonTokenType.PropertyName:
                        builder.Append(json.GetValueAsString());
                        break;
                }
            }
            json.Dispose();
            return (json.State, json.Position, builder.ToString());
        }

        [Fact]
        public void ReadSpanFromPipe()
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
                    bool isFinalBlock = data.Length == 0;   // TODO: Fix isFinalBlock condition, if Writer is done instead, isLastSegment?
                    (state, position, str) = ProcessDataSpan(data, isFinalBlock, state);
                    if (isFinalBlock)
                        break;
                    _pipe.Reader.AdvanceTo(position);
                    actual += str;
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
            Task.WaitAll(tasks, 5_000);    // The test shouldn't take more than 5 seconds to finish.

            Assert.True(actual.StartsWith(expectedStartsWith));
        }

        private static (JsonReaderState, SequencePosition, string) ProcessDataSpan(ReadOnlySequence<byte> ros, bool isFinalBlock, JsonReaderState state = default)
        {
            var builder = new StringBuilder();
            ReadOnlySpan<byte> leftOver = default;
            byte[] pooledArray = null;
            Utf8JsonReader json = default;
            long totalConsumed = 0;
            foreach (ReadOnlyMemory<byte> mem in ros)
            {
                ReadOnlySpan<byte> span = mem.Span;

                if (leftOver.Length > int.MaxValue - span.Length)
                    throw new ArgumentOutOfRangeException("Current sequence segment size is too large to fit left over data from the previous segment into a 2 GB buffer.");

                pooledArray = ArrayPool<byte>.Shared.Rent(span.Length + leftOver.Length);    // This is gauranteed to not overflow
                Span<byte> bufferSpan = pooledArray.AsSpan(0, leftOver.Length + span.Length);
                leftOver.CopyTo(bufferSpan);
                span.CopyTo(bufferSpan.Slice(leftOver.Length));

                json = new Utf8JsonReader(bufferSpan, isFinalBlock, state);

                while (json.Read())
                {
                    switch (json.TokenType)
                    {
                        case JsonTokenType.PropertyName:
                            builder.Append(json.GetValueAsString());
                            break;
                    }
                }

                if (json.Consumed < bufferSpan.Length)
                {
                    leftOver = bufferSpan.Slice((int)json.Consumed);
                }
                else
                {
                    leftOver = default;
                }
                totalConsumed += json.Consumed;
                if (pooledArray != null)    // TODO: Will this work if data spans more than two segments?
                    ArrayPool<byte>.Shared.Return(pooledArray);

                state = json.State;
            }
            return (json.State, ros.GetPosition(totalConsumed), builder.ToString());
        }
    }
}
