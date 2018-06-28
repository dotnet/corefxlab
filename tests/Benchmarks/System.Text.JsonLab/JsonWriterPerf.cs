// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using System.Buffers.Text;
using System.IO;
using System.Text.Formatting;

namespace System.Text.JsonLab.Benchmarks
{
    //[SimpleJob(-1, 5, 10, 32768)]
    //[DisassemblyDiagnoser(printAsm: true, printSource: true)]
    //[InliningDiagnoser(filterByNamespace: false)]
    [MemoryDiagnoser]
    public class JsonWriterPerf
    {
        private const int ExtraArraySize = 1000;
        private const int BufferSize = 1024 + (ExtraArraySize * 64);

        private ArrayFormatterWrapper _arrayFormatterWrapper;
        private MemoryStream _memoryStream;
        private StreamWriter _streamWriter;

        private int[] _data;

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
        }

        [Benchmark]
        public void WriterSystemTextJsonBasic()
        {
            _arrayFormatterWrapper.Clear();
            WriterSystemTextJsonBasicUtf8(Formatted, _arrayFormatterWrapper, _data.AsSpan(0, 10));
        }

        //[Benchmark]
        public void WriterNewtonsoftBasic()
        {
            WriterNewtonsoftBasic(Formatted, GetWriter(), _data.AsSpan(0, 10));
        }

        [Benchmark]
        public void WriterSystemTextJsonHelloWorld()
        {
            _arrayFormatterWrapper.Clear();
            WriterSystemTextJsonHelloWorldUtf8(Formatted, _arrayFormatterWrapper);
        }

        //[Benchmark]
        public void WriterNewtonsoftHelloWorld()
        {
            WriterNewtonsoftHelloWorld(Formatted, GetWriter());
        }

        [Benchmark]
        [Arguments(1)]
        //[Arguments(2)]
        //[Arguments(5)]
        [Arguments(10)]
        //[Arguments(100)]
        [Arguments(1000)]
        public void WriterSystemTextJsonArrayOnly(int size)
        {
            _arrayFormatterWrapper.Clear();
            WriterSystemTextJsonArrayOnlyUtf8(Formatted, _arrayFormatterWrapper, _data.AsSpan(0, size));
        }

        private TextWriter GetWriter()
        {
            _memoryStream.Seek(0, SeekOrigin.Begin);
            return _streamWriter;
        }

        private static void WriterSystemTextJsonBasicUtf8(bool formatted, ArrayFormatterWrapper output, ReadOnlySpan<int> data)
        {
            var json = new JsonWriter<ArrayFormatterWrapper>(output, formatted);

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

            json.WriteArrayStart("ExtraArray");
            for (var i = 0; i < data.Length; i++)
            {
                json.WriteValue(data[i]);
            }
            json.WriteArrayEnd();

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

        private static void WriterSystemTextJsonHelloWorldUtf8(bool formatted, ArrayFormatterWrapper output)
        {
            var json = new JsonWriter<ArrayFormatterWrapper>(output, formatted);

            json.WriteObjectStart();
            json.WriteAttribute("message", "Hello, World!");
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

        private static void WriterSystemTextJsonArrayOnlyUtf8(bool formatted, ArrayFormatterWrapper output, ReadOnlySpan<int> data)
        {
            var json = new JsonWriter<ArrayFormatterWrapper>(output, formatted);

            json.WriteArrayStart("ExtraArray");
            for (var i = 0; i < data.Length; i++)
            {
                json.WriteValue(data[i]);
            }
            json.WriteArrayEnd();
            json.Flush();
        }
    }
}
