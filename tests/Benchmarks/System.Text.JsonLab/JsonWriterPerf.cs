// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Code;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Text.Formatting;

using static System.Text.JsonLab.Benchmarks.Helper;

namespace System.Text.JsonLab.Benchmarks
{
    [MemoryDiagnoser]
    public class JsonWriterPerf
    {
        private const int ExtraArraySize = 500;

        public const int BufferSize = 1024 + (ExtraArraySize * 64);

        private ArrayFormatter _arrayFormatter;
        private SymbolTable _symbolTable;
        private MemoryStream _stream;
        private StreamWriter _writer;

        [Params(EncoderTarget.InvariantUtf8, EncoderTarget.InvariantUtf16)]
        public EncoderTarget Target;

        [Params(true, false)]
        public bool Formatted;

        [GlobalSetup]
        public void Setup()
        {
            var enc = Target == EncoderTarget.InvariantUtf8 ? Encoding.UTF8 : Encoding.Unicode;
            var buffer = new byte[BufferSize];
            _stream = new MemoryStream(buffer);
            _writer = new StreamWriter(_stream, enc, BufferSize, true);
            _symbolTable = GetTargetEncoder(Target);
            _arrayFormatter = new ArrayFormatter(BufferSize, _symbolTable);
        }

        [Benchmark]
        public void WriterSystemTextJsonBasic()
        {
            _arrayFormatter.Clear();
            WriterSystemTextJsonBasic(Formatted, _arrayFormatter);
        }

        [Benchmark]
        public void WriterNewtonsoftBasic()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            WriterNewtonsoftBasic(Formatted, _writer);
        }

        [Benchmark]
        public void WriterSystemTextJsonHelloWorld()
        {
            _arrayFormatter.Clear();
            WriterSystemTextJsonHelloWorld(Formatted, _arrayFormatter);
        }

        [Benchmark]
        public void WriterNewtonsoftHelloWorld()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            WriterNewtonsoftHelloWorld(Formatted, _writer);
        }

        // Skipping this test for now since it results in too many extra permutations
        // The Target Param is redundant here so we end up running this test twice.
        //[Benchmark]
        [ArgumentsSource(nameof(GetWriterSystemTextJsonParameters))]
        public void WriterSlowSystemTextJsonHelloWorld(ArrayFormatter formatter)
        {
            formatter.Clear();
            WriterSystemTextJsonHelloWorld(Formatted, formatter);
        }

        // Skipping this test for now since it results in too many extra permutations
        // The Target Param is redundant here so we end up running this test twice.
        //[Benchmark]
        [ArgumentsSource(nameof(GetWriterSystemTextJsonParameters))]
        public void WriterSlowSystemTextJsonBasic(ArrayFormatter formatter)
        {
            formatter.Clear();
            WriterSystemTextJsonBasic(Formatted, formatter);
        }

        public IEnumerable<object[]> GetWriterSystemTextJsonParameters()
        {
            yield return new object[] { new ArrayFormatterParam(EncoderTarget.SlowUtf8) };
            yield return new object[] { new ArrayFormatterParam(EncoderTarget.SlowUtf16) };
        }

        public class ArrayFormatterParam : IParam
        {
            private readonly EncoderTarget _target;

            public ArrayFormatterParam(EncoderTarget target)
            {
                _target = target;
            }

            public object Value => new ArrayFormatter(BufferSize, GetTargetEncoder(_target));

            public string DisplayText => $"({_target})";

            public string ToSourceCode()
                => $"new System.Text.Formatting.ArrayFormatter(BufferSize, System.Text.Json.Benchmarks.Helper.GetTargetEncoder(System.Text.Json.Benchmarks.Helper.EncoderTarget.{_target}))";
        }

        private static void WriterSystemTextJsonBasic(bool formatted, ArrayFormatter output)
        {
            var json = new JsonWriter(output, formatted);

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
                json.WriteValue(i);
            }
            json.WriteArrayEnd();

            json.WriteObjectEnd();
        }

        private static void WriterNewtonsoftBasic(bool formatted, StreamWriter writer)
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
                    json.WriteValue(i);
                }
                json.WriteEnd();

                json.WriteEnd();
            }
        }

        private static void WriterSystemTextJsonHelloWorld(bool formatted, ArrayFormatter output)
        {
            var json = new JsonWriter(output, formatted);

            json.WriteObjectStart();
            json.WriteAttribute("message", "Hello, World!");
            json.WriteObjectEnd();
        }

        private static void WriterNewtonsoftHelloWorld(bool formatted, StreamWriter writer)
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
