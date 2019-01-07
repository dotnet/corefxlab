// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;
using System.Buffers.Text;
using System.IO;
using System.Text.Formatting;

namespace System.Text.JsonLab.Benchmarks
{
    // Since there are 24 tests here (2 * 12), setting low values for the warmupCount and targetCount
    [SimpleJob(warmupCount: 3, targetCount: 5)]
    [MemoryDiagnoser]
    public class JsonReaderMaxDepthPerf
    {
        private byte[] _dataUtf8;
        private MemoryStream _stream;
        private StreamReader _reader;

        [Params(1, 2, 4, 8, 16, 32, 64, 65, 66, 128, 256, 512)]
        public int Depth;

        [GlobalSetup]
        public void Setup()
        {
            var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
            var jsonUtf8 = new Utf8JsonWriter<ArrayFormatterWrapper>(output);

            WriteDepth(ref jsonUtf8, Depth - 1);

            ArraySegment<byte> formatted = output.Formatted;
            string actualStr = Encoding.UTF8.GetString(formatted.Array, formatted.Offset, formatted.Count);

            _dataUtf8 = Encoding.UTF8.GetBytes(actualStr);

            _stream = new MemoryStream(_dataUtf8);
            _reader = new StreamReader(_stream, Encoding.UTF8, false, 1024, true);
        }

        [Benchmark(Baseline = true)]
        public void ReaderNewtonsoftReaderEmptyLoop()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            TextReader reader = _reader;
            var json = new JsonTextReader(reader)
            {
                MaxDepth = Depth
            };
            while (json.Read()) ;
        }

        [Benchmark]
        public void ReaderSystemTextJsonLabSpanEmptyLoop()
        {
            var json = new JsonUtf8Reader(_dataUtf8)
            {
                MaxDepth = Depth
            };
            while (json.Read()) ;
        }

        private static void WriteDepth(ref Utf8JsonWriter<ArrayFormatterWrapper> jsonUtf8, int depth)
        {
            jsonUtf8.WriteObjectStart();
            for (int i = 0; i < depth; i++)
            {
                jsonUtf8.WriteObjectStart("message" + i);
            }
            jsonUtf8.WriteAttribute("message" + depth, "Hello, World!");
            for (int i = 0; i < depth; i++)
            {
                jsonUtf8.WriteObjectEnd();
            }
            jsonUtf8.WriteObjectEnd();
            jsonUtf8.Flush();
        }
    }
}
