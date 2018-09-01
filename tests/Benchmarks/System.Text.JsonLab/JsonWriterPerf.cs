// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BenchmarkDotNet.Attributes;
using System.Buffers.Text;
using System.IO;
using System.Text.Formatting;

namespace System.Text.JsonLab.Benchmarks
{
    [SimpleJob(-1, 3, 5)]
    [MemoryDiagnoser]
    public class JsonWriterPerf
    {
        private static readonly byte[] Message = Encoding.UTF8.GetBytes("message");
        private static readonly byte[] HelloWorld = Encoding.UTF8.GetBytes("Hello, World!");
        private static readonly byte[] ExtraArray = Encoding.UTF8.GetBytes("ExtraArray");

        private const int ExtraArraySize = 10000000;
        private const int BufferSize = 1024 + (ExtraArraySize * 64);

        private ArrayFormatterWrapper _arrayFormatterWrapper;
        private MemoryStream _memoryStream;
        private StreamWriter _streamWriter;

        private int[] _data;
        private byte[] _output;

        [Params(true, false)]
        public bool Formatted;

        [GlobalSetup]
        public void Setup()
        {
            _data = new int[ExtraArraySize];
            Random rand = new Random(42);

            for (int i = 0; i < ExtraArraySize; i++)
            {
                _data[i] = rand.Next(-10000, 10000);
            }

            var buffer = new byte[BufferSize];
            _memoryStream = new MemoryStream(buffer);
            _streamWriter = new StreamWriter(_memoryStream, new UTF8Encoding(false), BufferSize, true);
            _arrayFormatterWrapper = new ArrayFormatterWrapper(BufferSize, SymbolTable.InvariantUtf8);

            // To pass an initialBuffer to Utf8Json:
            // _output = new byte[BufferSize];
            _output = null;
        }

        [Benchmark]
        public void WriterSystemTextJsonBasic()
        {
            _arrayFormatterWrapper.Clear();
            WriterSystemTextJsonBasicUtf8(Formatted, _arrayFormatterWrapper, _data.AsSpan(0, 10));
        }

        [Benchmark]
        public void WriterNewtonsoftBasic()
        {
            WriterNewtonsoftBasic(Formatted, GetWriter(), _data.AsSpan(0, 10));
        }

        [Benchmark]
        public void WriterUtf8JsonBasic()
        {
            WriterUtf8JsonBasic(_data.AsSpan(0, 10));
        }

        [Benchmark]
        public void WriterSystemTextJsonHelloWorld()
        {
            _arrayFormatterWrapper.Clear();
            WriterSystemTextJsonHelloWorldUtf8(Formatted, _arrayFormatterWrapper);
        }

        [Benchmark]
        public void WriterNewtonsoftHelloWorld()
        {
            WriterNewtonsoftHelloWorld(Formatted, GetWriter());
        }

        [Benchmark]
        public void WriterUtf8JsonHelloWorld()
        {
            WriterUtf8JsonHelloWorldHelper(_output);
        }

        [Benchmark]
        [Arguments(1)]
        [Arguments(2)]
        [Arguments(5)]
        [Arguments(10)]
        [Arguments(100)]
        [Arguments(1000)]
        [Arguments(10000)]
        [Arguments(100000)]
        [Arguments(1000000)]
        [Arguments(10000000)]
        public void WriterSystemTextJsonArrayOnly(int size)
        {
            _arrayFormatterWrapper.Clear();
            WriterSystemTextJsonArrayOnlyUtf8(Formatted, _arrayFormatterWrapper, _data.AsSpan(0, size));
        }

        [Benchmark]
        [Arguments(1)]
        [Arguments(2)]
        [Arguments(5)]
        [Arguments(10)]
        [Arguments(100)]
        [Arguments(1000)]
        [Arguments(10000)]
        [Arguments(100000)]
        [Arguments(1000000)]
        [Arguments(10000000)]
        public void WriterUtf8JsonArrayOnly(int size)
        {
            WriterUtf8JsonArrayOnly(_data.AsSpan(0, size), _output);
        }

        private TextWriter GetWriter()
        {
            _memoryStream.Seek(0, SeekOrigin.Begin);
            return _streamWriter;
        }

        private static void WriterSystemTextJsonBasicUtf8(bool formatted, ArrayFormatterWrapper output, ReadOnlySpan<int> data)
        {
            Utf8JsonWriter<ArrayFormatterWrapper> json = Utf8JsonWriter.Create(output, formatted);

            json.WriteObjectStart();
            json.WriteAttribute("age", 42);
            json.WriteAttribute("first", "John");
            json.WriteAttribute("last", "Smith");
            json.WriteArrayStart("phoneNumbers");
            json.WriteValue("425-000-1212");
            json.WriteValue("425-000-1213");
            json.WriteArrayEnd();
            json.WriteObjectStart("address");
            json.WriteAttribute("street", "1 Microsoft Way");
            json.WriteAttribute("city", "Redmond");
            json.WriteAttribute("zip", 98052);
            json.WriteObjectEnd();
            json.WriteArrayUtf8(ExtraArray, data);
            json.WriteObjectEnd();
            json.Flush();
        }

        private static void WriterNewtonsoftBasic(bool formatted, TextWriter writer, ReadOnlySpan<int> data)
        {
            using (var json = new Newtonsoft.Json.JsonTextWriter(writer))
            {
                json.Formatting = formatted ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;

                json.WriteStartObject();
                json.WritePropertyName("age");
                json.WriteValue(42);
                json.WritePropertyName("first");
                json.WriteValue("John");
                json.WritePropertyName("last");
                json.WriteValue("Smith");
                json.WritePropertyName("phoneNumbers");
                json.WriteStartArray();
                json.WriteValue("425-000-1212");
                json.WriteValue("425-000-1213");
                json.WriteEnd();
                json.WritePropertyName("address");
                json.WriteStartObject();
                json.WritePropertyName("street");
                json.WriteValue("1 Microsoft Way");
                json.WritePropertyName("city");
                json.WriteValue("Redmond");
                json.WritePropertyName("zip");
                json.WriteValue(98052);
                json.WriteEnd();

                json.WritePropertyName("ExtraArray");
                json.WriteStartArray();
                for (var i = 0; i < data.Length; i++)
                {
                    json.WriteValue(data[i]);
                }
                json.WriteEnd();

                json.WriteEnd();
            }
        }

        private static void WriterUtf8JsonBasic(ReadOnlySpan<int> data)
        {
            Utf8Json.JsonWriter json = new Utf8Json.JsonWriter();

            json.WriteBeginObject();
            json.WritePropertyName("age");
            json.WriteInt32(42);
            json.WriteValueSeparator();
            json.WritePropertyName("first");
            json.WriteString("John");
            json.WriteValueSeparator();
            json.WritePropertyName("last");
            json.WriteString("Smith");
            json.WriteValueSeparator();
            json.WritePropertyName("phoneNumbers");
            json.WriteBeginArray();
            json.WriteString("425-000-1212");
            json.WriteValueSeparator();
            json.WriteString("425-000-1213");
            json.WriteEndArray();
            json.WriteValueSeparator();
            json.WritePropertyName("address");
            json.WriteBeginObject();
            json.WritePropertyName("street");
            json.WriteString("1 Microsoft Way");
            json.WriteValueSeparator();
            json.WritePropertyName("city");
            json.WriteString("Redmond");
            json.WriteValueSeparator();
            json.WritePropertyName("zip");
            json.WriteInt32(98052);
            json.WriteEndObject();
            json.WriteValueSeparator();

            json.WritePropertyName("ExtraArray");
            json.WriteBeginArray();
            for (var i = 0; i < data.Length - 1; i++)
            {
                json.WriteInt32(data[i]);
                json.WriteValueSeparator();
            }
            if (data.Length > 0)
                json.WriteInt32(data[data.Length - 1]);
            json.WriteEndArray();

            json.WriteEndObject();
        }

        private static void WriterSystemTextJsonHelloWorldUtf8(bool formatted, ArrayFormatterWrapper output)
        {
            Utf8JsonWriter<ArrayFormatterWrapper> json = Utf8JsonWriter.Create(output, formatted);

            json.WriteObjectStart();
            json.WriteAttributeUtf8(Message, HelloWorld);
            json.WriteObjectEnd();
            json.Flush();
        }

        private static void WriterNewtonsoftHelloWorld(bool formatted, TextWriter writer)
        {
            using (var json = new Newtonsoft.Json.JsonTextWriter(writer))
            {
                json.Formatting = formatted ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;

                json.WriteStartObject();
                json.WritePropertyName("message");
                json.WriteValue("Hello, World!");
                json.WriteEnd();
            }
        }

        private static void WriterUtf8JsonHelloWorldHelper(byte[] output)
        {
            Utf8Json.JsonWriter json = new Utf8Json.JsonWriter(output);

            json.WriteBeginObject();
            json.WritePropertyName("message");
            json.WriteString("Hello, World!");
            json.WriteEndObject();
        }

        private static void WriterSystemTextJsonArrayOnlyUtf8(bool formatted, ArrayFormatterWrapper output, ReadOnlySpan<int> data)
        {
            Utf8JsonWriter<ArrayFormatterWrapper> json = Utf8JsonWriter.Create(output, formatted);

            json.WriteArrayUtf8(ExtraArray, data);
            json.Flush();
        }

        private static void WriterUtf8JsonArrayOnly(ReadOnlySpan<int> data, byte[] output)
        {
            Utf8Json.JsonWriter json = new Utf8Json.JsonWriter(output);

            json.WriteBeginArray();
            json.WritePropertyName("ExtraArray");
            for (var i = 0; i < data.Length - 1; i++)
            {
                json.WriteInt32(data[i]);
                json.WriteValueSeparator();
            }
            if (data.Length > 0)
                json.WriteInt32(data[data.Length - 1]);
            json.WriteEndArray();
        }
    }
}
