// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BenchmarkDotNet.Attributes;
using System.Buffers.Text;
using System.IO;
using System.Text.Formatting;

namespace System.Text.JsonLab.Benchmarks
{
    [MemoryDiagnoser]
    public class JsonWriterPerf
    {
        private const int ExtraArraySize = 500;
        private const int BufferSize = 1024 + (ExtraArraySize * 64);

        private ArrayFormatter _arrayFormatter;
        private MemoryStream _memoryStream;
        private StreamWriter _streamWriter;
        private StringBuilder _stringBuilder;

        private int[] _data;

        [Params(true, false)]
        public bool IsUTF8Encoded;

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

            if (IsUTF8Encoded)
            {
                var buffer = new byte[BufferSize];
                _memoryStream = new MemoryStream(buffer);
                _streamWriter = new StreamWriter(_memoryStream, new UTF8Encoding(false), BufferSize, true);
                _arrayFormatter = new ArrayFormatter(BufferSize, SymbolTable.InvariantUtf8);
            }
            else
            {
                _stringBuilder = new StringBuilder();
                _arrayFormatter = new ArrayFormatter(BufferSize, SymbolTable.InvariantUtf16);
            }
        }

        [Benchmark]
        public void WriterSystemTextJsonBasic()
        {
            _arrayFormatter.Clear();
            if (IsUTF8Encoded)
                WriterSystemTextJsonBasicUtf8(Formatted, _arrayFormatter, _data);
            else
                WriterSystemTextJsonBasicUtf16(Formatted, _arrayFormatter, _data);
        }

        [Benchmark]
        public void WriterNewtonsoftBasic()
        {
            WriterNewtonsoftBasic(Formatted, GetWriter(), _data);
        }

        [Benchmark]
        public void WriterSystemTextJsonHelloWorld()
        {
            _arrayFormatter.Clear();
            if (IsUTF8Encoded)
                WriterSystemTextJsonHelloWorldUtf8(Formatted, _arrayFormatter);
            else
                WriterSystemTextJsonHelloWorldUtf16(Formatted, _arrayFormatter);
        }

        [Benchmark]
        public void WriterNewtonsoftHelloWorld()
        {
            WriterNewtonsoftHelloWorld(Formatted, GetWriter());
        }

        private TextWriter GetWriter()
        {
            TextWriter writer;
            if (IsUTF8Encoded)
            {
                _memoryStream.Seek(0, SeekOrigin.Begin);
                writer = _streamWriter;
            }
            else
            {
                _stringBuilder.Clear();
                writer = new StringWriter(_stringBuilder);
            }
            return writer;
        }

        private static void WriterSystemTextJsonBasicUtf8(bool formatted, ArrayFormatter output, int[] data)
        {
            var json = new JsonWriterUtf8(output, formatted);

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

            // Add a large array of values
            json.WriteArrayStart("ExtraArray");
            for (var i = 0; i < ExtraArraySize; i++)
            {
                json.WriteValue(data[i]);
            }
            json.WriteArrayEnd();

            json.WriteObjectEnd();
        }

        private static void WriterSystemTextJsonBasicUtf16(bool formatted, ArrayFormatter output, int[] data)
        {
            var json = new JsonWriterUtf16(output, formatted);

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

            // Add a large array of values
            json.WriteArrayStart("ExtraArray");
            for (var i = 0; i < ExtraArraySize; i++)
            {
                json.WriteValue(data[i]);
            }
            json.WriteArrayEnd();

            json.WriteObjectEnd();
        }

        private static void WriterNewtonsoftBasic(bool formatted, TextWriter writer, int[] data)
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

                // Add a large array of values
                json.WritePropertyName("ExtraArray");
                json.WriteStartArray();
                for (var i = 0; i < ExtraArraySize; i++)
                {
                    json.WriteValue(data[i]);
                }
                json.WriteEnd();

                json.WriteEnd();
            }
        }

        private static void WriterSystemTextJsonHelloWorldUtf8(bool formatted, ArrayFormatter output)
        {
            var json = new JsonWriterUtf8(output, formatted);

            json.WriteObjectStart();
            json.WriteAttribute("message", "Hello, World!");
            json.WriteObjectEnd();
        }

        private static void WriterSystemTextJsonHelloWorldUtf16(bool formatted, ArrayFormatter output)
        {
            var json = new JsonWriterUtf16(output, formatted);

            json.WriteObjectStart();
            json.WriteAttribute("message", "Hello, World!");
            json.WriteObjectEnd();
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
    }
}
