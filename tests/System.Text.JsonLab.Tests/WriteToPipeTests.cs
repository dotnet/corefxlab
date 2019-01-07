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
    public class WriteToPipeTests : IDisposable
    {
        public WriteToPipeTests()
        {
            string jsonString = TestJson.BasicJson;
            _dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
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
        private readonly string _expectedString;

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

        [Fact(Skip = "Need to investigate intermitten test failure: JsonReaderException : '{' is invalid after a value. Expected either ',', '}', or ']'. LineNumber: 0 | BytePositionInLine: 160")]
        public void WriteToPipeUsingSpan()
        {
            string actual = "";
            var taskReader = Task.Run(async () =>
            {
                Json.JsonReaderState state = default;
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
                JsonWriterState state = default;
                long bytesCommitted = 0;
                while (true)
                {
                    Memory<byte> memory = new byte [4_000];
                    (state, bytesCommitted) = WriteData(memory, isFinalBlock: true, state);
                    FlushResult result = await _pipe.Writer.WriteAsync(memory.Slice(0, (int)bytesCommitted));
                    break;
                }
                _pipe.Writer.Complete();
            });

            Task[] tasks = new Task[] { taskReader, taskWriter };
            Task.WaitAll(tasks, 30_000);    // The test shouldn't take more than 30 seconds to finish.

            Assert.Equal(_expectedString, actual);
        }

        [Fact]
        public void TestingPipeWriterGetSpan()
        {
            PipeWriter writer = _pipe.Writer;

            Span<byte> span = writer.GetSpan(0);
            Assert.True(0 < span.Length, $"{span.Length}");

            span = writer.GetSpan(1_000_000);
            Assert.True(1_000_000 <= span.Length, $"{span.Length}");

            span = writer.GetSpan(4_000);
            Assert.True(4_000 <= span.Length, $"{span.Length}");

            span = writer.GetSpan(5_000);
            Assert.True(5_000 <= span.Length, $"{span.Length}");

            span = writer.GetSpan(8_000);
            Assert.True(8_000 <= span.Length, $"{span.Length}");

            span = writer.GetSpan(17_000);
            Assert.True(17_000 <= span.Length, $"{span.Length}");

            span = writer.GetSpan(1_000_000_000);
            Assert.True(1_000_000_000 <= span.Length, $"{span.Length}");

            span = writer.GetSpan(2_000_000_000);
            Assert.True(2_000_000_000 <= span.Length, $"{span.Length}");

            try
            {
                span = writer.GetSpan(-1);
                Assert.True(false, "Expected ArgumentOutOfRangeException not thrown when asking for a negative length span.");
            }
            catch (ArgumentOutOfRangeException)
            { }
        }

        [Fact]
        public void WriteToPipeUsingPipeWriter()
        {
            string actual = "";
            var taskReader = Task.Run(async () =>
            {
                Json.JsonReaderState state = default;
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
                JsonWriterState state = default;
                long bytesWritten = 0;
                while (true)
                {
                    (state, bytesWritten) = WriteData(_pipe.Writer, isFinalBlock: true, state);
                    if (bytesWritten != 0)
                    {
                        FlushResult result = await _pipe.Writer.FlushAsync();
                    }
                    break;
                }
                _pipe.Writer.Complete();
            });

            Task[] tasks = new Task[] { taskReader, taskWriter };
            Task.WaitAll(tasks, 30_000);    // The test shouldn't take more than 30 seconds to finish.

            Assert.Equal(_expectedString, actual);
        }

        private static (Json.JsonReaderState, string) ProcessData(ReadOnlySequence<byte> ros, bool isFinalBlock, Json.JsonReaderState state = default)
        {
            var builder = new StringBuilder();
            var json = new Json.Utf8JsonReader(ros, isFinalBlock, state);
            while (json.Read())
            {
                switch (json.TokenType)
                {
                    case Json.JsonTokenType.PropertyName:
                        builder.Append(json.GetStringValue());
                        break;
                }
            }
            Assert.Equal(json.BytesConsumed, json.CurrentState.BytesConsumed);
            Assert.Equal(json.Position, json.CurrentState.Position);
            return (json.CurrentState, builder.ToString());
        }

        private static (JsonWriterState, long) WriteData(Memory<byte> memory, bool isFinalBlock, JsonWriterState state = default)
        {
            var json = new Utf8JsonWriter2(memory.Span, state);

            json.WriteStartObject();
            json.WriteNumber("age", 30, suppressEscaping: true);
            json.WriteString("first", "John", suppressEscaping: true);
            json.WriteString("last", "Smith", suppressEscaping: true);
            json.WriteStartArray("phoneNumbers", suppressEscaping: true);
            json.WriteStringValue("425-000-1212", suppressEscaping: true);
            json.WriteStringValue("425-000-1213", suppressEscaping: true);
            json.WriteNullValue();
            json.WriteEndArray();
            json.WriteStartObject("address", suppressEscaping: true);
            json.WriteString("street", "1 Microsoft Way", suppressEscaping: true);
            json.WriteString("city", "Redmond", suppressEscaping: true);
            json.WriteNumber("zip", 98052, suppressEscaping: true);
            json.WriteEndObject();
            json.WriteEndObject();
            json.Flush(isFinalBlock);

            Assert.Equal(json.BytesCommitted, json.CurrentState.BytesCommitted);
            Assert.Equal(json.BytesWritten, json.CurrentState.BytesWritten);
            Assert.True(json.BytesCommitted == 160);
            Assert.True(json.BytesWritten == 160);
            return (json.CurrentState, json.BytesCommitted);
        }

        private static (JsonWriterState, long) WriteData(PipeWriter writer, bool isFinalBlock, JsonWriterState state = default)
        {
            var json = new Utf8JsonWriter2(writer, state);

            json.WriteStartObject();
            json.WriteNumber("age", 30, suppressEscaping: true);
            json.WriteString("first", "John", suppressEscaping: true);
            json.WriteString("last", "Smith", suppressEscaping: true);
            json.WriteStartArray("phoneNumbers", suppressEscaping: true);
            json.WriteStringValue("425-000-1212", suppressEscaping: true);
            json.WriteStringValue("425-000-1213", suppressEscaping: true);
            json.WriteNullValue();
            json.WriteEndArray();
            json.WriteStartObject("address", suppressEscaping: true);
            json.WriteString("street", "1 Microsoft Way", suppressEscaping: true);
            json.WriteString("city", "Redmond", suppressEscaping: true);
            json.WriteNumber("zip", 98052, suppressEscaping: true);
            json.WriteEndObject();
            json.WriteEndObject();
            json.Flush(isFinalBlock);

            Assert.Equal(json.BytesCommitted, json.CurrentState.BytesCommitted);
            Assert.Equal(json.BytesWritten, json.CurrentState.BytesWritten);
            Assert.True(json.BytesCommitted == 160);
            Assert.True(json.BytesWritten == 160);
            return (json.CurrentState, json.BytesCommitted);
        }
    }
}
