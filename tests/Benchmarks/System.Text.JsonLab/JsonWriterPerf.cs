// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using System.Buffers;
using System.Buffers.Text;
using System.Buffers.Writer;
using System.IO;
using System.Text.Formatting;

namespace System.Text.JsonLab.Benchmarks
{
    //[SimpleJob(-1, 10, 50, 40960)]
    [SimpleJob(-1, 5, 10, 8192)]
    //[MemoryDiagnoser]
    public class JsonWriterPerf
    {
        private ArrayFormatter _arrayFormatter;
        private int[] _data;

        [Params(true, false)]
        public bool Formatted;

        [Params(1, 2, 10, 100, 500, 5000)]
        public int ExtraArraySize;

        [GlobalSetup]
        public void Setup()
        {
            //int ExtraArraySize = 1;
            _data = new int[ExtraArraySize];
            Random rand = new Random(42);

            for (int i = 0; i < ExtraArraySize; i++)
            {
                _data[i] = rand.Next(-10000, 10000);
            }

            _arrayFormatter = new ArrayFormatter(1024 + (ExtraArraySize * 64), SymbolTable.InvariantUtf8);
        }

        /*[Benchmark]
        public void WriterSystemTextJsonHelloWorld()
        {
            _arrayFormatter.Clear();
            WriterSystemTextJsonHelloWorldUtf8(Formatted, _arrayFormatter);
        }

        [Benchmark]
        public void WriterSystemTextJsonHelloWorldCompact()
        {
            _arrayFormatter.Clear();
            WriterSystemTextJsonHelloWorldCompactUtf8(Formatted, _arrayFormatter);
        }*/

        [Benchmark]
        public void WriterSystemTextJsonArrayOnly()
        {
            _arrayFormatter.Clear();
            WriterSystemTextJsonArrayOnlyUtf8(Formatted, _arrayFormatter, _data);
        }

        [Benchmark]
        public void WriterSystemTextJsonArrayOnlyCompact()
        {
            _arrayFormatter.Clear();
            WriterSystemTextJsonArrayOnlyCompactUtf8(Formatted, _arrayFormatter, _data);
        }

        private static void WriterSystemTextJsonArrayOnlyUtf8(bool formatted, ArrayFormatter output, int[] data)
        {
            var json = JsonWriterFactory.Create(output, true, formatted);

            json.WriteArrayStart("ExtraArray");
            for (var i = 0; i < data.Length; i++)
            {
                json.WriteValue(data[i]);
            }
            json.WriteArrayEnd();
            json.Flush();
        }

        private static void WriterSystemTextJsonArrayOnlyCompactUtf8(bool formatted, ArrayFormatter output, int[] data)
        {
            var json = JsonWriterFactory.Create(output, true, formatted);

            json.WriteArray("ExtraArray", data);
            json.Flush();
        }

        private static void WriterSystemTextJsonHelloWorldUtf8(bool formatted, ArrayFormatter output)
        {
            var json = JsonWriterFactory.Create(output, true, formatted);

            json.WriteObjectStart();
            json.WriteAttribute("message", "Hello, World!");
            json.WriteObjectEnd();
            json.Flush();
        }

        private static void WriterSystemTextJsonHelloWorldCompactUtf8(bool formatted, ArrayFormatter output)
        {
            var json = JsonWriterFactory.Create(output, true, formatted);

            json.WriteObject("message", "Hello, World!");
            json.Flush();
        }
    }
}
