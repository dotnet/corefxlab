// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BenchmarkDotNet.Attributes;
using Benchmarks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Buffers;
using System.Collections.Generic;
using System.IO;

namespace System.Text.JsonLab.Benchmarks
{
    // Since there are 90 tests here (6 * 15), setting low values for the warmupCount, targetCount, and invocationCount
    [SimpleJob(-1, 3, 5, 1024)]
    [MemoryDiagnoser]
    public class JsonReaderPerf
    {
        // Keep the JsonStrings resource names in sync with TestCaseType enum values.
        public enum TestCaseType
        {
            HelloWorld,
            BasicJson,
            BasicLargeNum,
            SpecialNumForm,
            ProjectLockJson,
            FullSchema1,
            FullSchema2,
            DeepTree,
            BroadTree,
            LotsOfNumbers,
            LotsOfStrings,
            Json400B,
            Json4KB,
            Json40KB,
            Json400KB
        }

        private string _jsonString;
        private byte[] _dataUtf8;
        private ReadOnlySequence<byte> _sequence;
        private ReadOnlySequence<byte> _sequenceSingle;
        private MemoryStream _stream;
        private StreamReader _reader;

        [ParamsSource(nameof(TestCaseValues))]
        public TestCaseType TestCase;

        [Params(true, false)]
        public bool IsDataCompact;

        public static IEnumerable<TestCaseType> TestCaseValues() => (IEnumerable<TestCaseType>)Enum.GetValues(typeof(TestCaseType));

        [GlobalSetup]
        public void Setup()
        {
            _jsonString = JsonStrings.ResourceManager.GetString(TestCase.ToString());

            // Remove all formatting/indendation
            if (IsDataCompact)
            {
                using (JsonTextReader jsonReader = new JsonTextReader(new StringReader(_jsonString)))
                {
                    JToken obj = JToken.ReadFrom(jsonReader);
                    var stringWriter = new StringWriter();
                    using (JsonTextWriter jsonWriter = new JsonTextWriter(stringWriter))
                    {
                        obj.WriteTo(jsonWriter);
                        _jsonString = stringWriter.ToString();
                    }
                }
            }

            _dataUtf8 = Encoding.UTF8.GetBytes(_jsonString);

            ReadOnlyMemory<byte> dataMemory = _dataUtf8;
            _sequenceSingle = new ReadOnlySequence<byte>(dataMemory);
            var firstSegment = new BufferSegment<byte>(dataMemory.Slice(0, _dataUtf8.Length / 2));
            ReadOnlyMemory<byte> secondMem = dataMemory.Slice(_dataUtf8.Length / 2);
            BufferSegment<byte> secondSegment = firstSegment.Append(secondMem);
            _sequence = new ReadOnlySequence<byte>(firstSegment, 0, secondSegment, secondMem.Length);

            _stream = new MemoryStream(_dataUtf8);
            _reader = new StreamReader(_stream, Encoding.UTF8, false, 1024, true);
        }

        [Benchmark(Baseline = true)]
        public void ReaderNewtonsoftReaderEmptyLoop()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            TextReader reader = _reader;
            var json = new JsonTextReader(reader);
            while (json.Read()) ;
        }

        [Benchmark]
        public string ReaderNewtonsoftReaderReturnString()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            TextReader reader = _reader;

            StringBuilder sb = new StringBuilder();
            var json = new JsonTextReader(reader);
            while (json.Read())
            {
                if (json.Value != null)
                {
                    sb.Append(json.Value).Append(", ");
                }
            }

            return sb.ToString();
        }

        [Benchmark]
        public void ReaderSystemTextJsonLabSpanEmptyLoop()
        {
            var json = new Utf8JsonReader(_dataUtf8);
            while (json.Read()) ;
        }

        [Benchmark]
        public void ReaderSystemTextJsonLabSingleSpanSequenceEmptyLoop()
        {
            var json = new Utf8JsonReader(_sequenceSingle);
            while (json.Read()) ;
        }

        [Benchmark]
        public void ReaderSystemTextJsonLabMultiSpanSequenceEmptyLoop()
        {
            var json = new Utf8JsonReader(_sequence);
            while (json.Read()) ;
        }

        [Benchmark]
        public byte[] ReaderSystemTextJsonLabReturnBytes()
        {
            byte[] outputArray = new byte[_dataUtf8.Length * 2];

            Span<byte> destination = outputArray;
            var json = new Utf8JsonReader(_dataUtf8);
            while (json.Read())
            {
                JsonTokenType tokenType = json.TokenType;
                ReadOnlySpan<byte> valueSpan = json.Value;
                switch (tokenType)
                {
                    case JsonTokenType.PropertyName:
                        valueSpan.CopyTo(destination);
                        destination[valueSpan.Length] = (byte)',';
                        destination[valueSpan.Length + 1] = (byte)' ';
                        destination = destination.Slice(valueSpan.Length + 2);
                        break;
                    case JsonTokenType.Value:
                        var valueType = json.ValueType;

                        switch (valueType)
                        {
                            case JsonValueType.True:
                                destination[0] = (byte)'T';
                                destination[1] = (byte)'r';
                                destination[2] = (byte)'u';
                                destination[3] = (byte)'e';
                                destination[valueSpan.Length] = (byte)',';
                                destination[valueSpan.Length + 1] = (byte)' ';
                                destination = destination.Slice(valueSpan.Length + 2);
                                break;
                            case JsonValueType.False:
                                destination[0] = (byte)'F';
                                destination[1] = (byte)'a';
                                destination[2] = (byte)'l';
                                destination[3] = (byte)'s';
                                destination[4] = (byte)'e';
                                destination[valueSpan.Length] = (byte)',';
                                destination[valueSpan.Length + 1] = (byte)' ';
                                destination = destination.Slice(valueSpan.Length + 2);
                                break;
                            case JsonValueType.Number:
                            case JsonValueType.String:
                                valueSpan.CopyTo(destination);
                                destination[valueSpan.Length] = (byte)',';
                                destination[valueSpan.Length + 1] = (byte)' ';
                                destination = destination.Slice(valueSpan.Length + 2);
                                break;
                            case JsonValueType.Null:
                                // Special casing  Null so that it matches what JSON.NET does
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
            return outputArray;
        }

        [Benchmark]
        public void ReaderUtf8JsonEmptyLoop()
        {
            Utf8Json.JsonReader json = new Utf8Json.JsonReader(_dataUtf8);

            while (json.GetCurrentJsonToken() != Utf8Json.JsonToken.None)
            {
                json.ReadNext();
            }
        }

        [Benchmark]
        public byte[] ReaderUtf8JsonReturnBytes()
        {
            Utf8Json.JsonReader json = new Utf8Json.JsonReader(_dataUtf8);

            byte[] outputArray = new byte[_dataUtf8.Length * 2];
            Span<byte> destination = outputArray;

            Utf8Json.JsonToken token = json.GetCurrentJsonToken();
            while (token != Utf8Json.JsonToken.None)
            {
                json.ReadNext();
                token = json.GetCurrentJsonToken();

                switch (token)
                {
                    case Utf8Json.JsonToken.String:
                    case Utf8Json.JsonToken.Number:
                    case Utf8Json.JsonToken.True:
                    case Utf8Json.JsonToken.False:
                    case Utf8Json.JsonToken.Null:
                        ReadOnlySpan<byte> valueSpan = json.ReadNextBlockSegment();
                        valueSpan.CopyTo(destination);
                        destination[valueSpan.Length] = (byte)',';
                        destination[valueSpan.Length + 1] = (byte)' ';
                        destination = destination.Slice(valueSpan.Length + 2);
                        break;
                    default:
                        break;
                }
            }

            return outputArray;
        }
    }

    internal class BufferSegment<T> : ReadOnlySequenceSegment<T>
    {
        public BufferSegment(ReadOnlyMemory<T> memory)
        {
            Memory = memory;
        }

        public BufferSegment<T> Append(ReadOnlyMemory<T> memory)
        {
            var segment = new BufferSegment<T>(memory)
            {
                RunningIndex = RunningIndex + Memory.Length
            };
            Next = segment;
            return segment;
        }
    }
}
