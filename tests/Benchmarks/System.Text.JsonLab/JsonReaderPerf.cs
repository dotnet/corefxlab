// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BenchmarkDotNet.Attributes;
using Benchmarks;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;

using static System.Text.JsonLab.Benchmarks.Helper;

namespace System.Text.JsonLab.Benchmarks
{
    [MemoryDiagnoser]
    public class JsonReaderPerf
    {
        private byte[] _data;
        private SymbolTable _symbolTable;
        private MemoryStream _stream;
        private StreamReader _reader;

        [Params(EncoderTarget.InvariantUtf8, EncoderTarget.InvariantUtf16)]
        public EncoderTarget Target;

        // Using the string name listed in the resource file instead of the json string directly
        // so that the benchmark output is cleaner
        [ParamsSource(nameof(ValuesForJsonStringName))]
        public string JsonStringName;

        public static IEnumerable<string> ValuesForJsonStringName() => new[] { nameof(JsonStrings.HeavyNestedJson), nameof(JsonStrings.HelloWorld) };

        [GlobalSetup]
        public void Setup()
        {
            _symbolTable = GetTargetEncoder(Target);
            _data = EncodeTestData(Target, JsonStrings.ResourceManager.GetString(JsonStringName));

            _stream = new MemoryStream(_data);
            var enc = Target == EncoderTarget.InvariantUtf8 ? Encoding.UTF8 : Encoding.Unicode;
            _reader = new StreamReader(_stream, enc, false, 1024, true);
        }

        [Benchmark]
        public void ReaderSystemTextJsonLab() => TestReaderSystemTextJsonLab(_data, _symbolTable);

        [Benchmark(Baseline = true)]
        public void ReaderNewtonsoft()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            TestReaderNewtonsoft(_reader);
        }

        private static void TestReaderSystemTextJsonLab(ReadOnlySpan<byte> data, SymbolTable symbolTable)
        {
            var json = new JsonReader(data, symbolTable);
            while (json.Read()) ;
        }

        private static void TestReaderNewtonsoft(StreamReader reader)
        {
            using (var json = new Newtonsoft.Json.JsonTextReader(reader))
                while (json.Read()) ;
        }

        private static byte[] EncodeTestData(EncoderTarget encoderTarget, string data)
        {
            if (encoderTarget == EncoderTarget.InvariantUtf16 || encoderTarget == EncoderTarget.SlowUtf16)
                return Encoding.Unicode.GetBytes(data);
            else
                return Encoding.UTF8.GetBytes(data);
        }
    }
}
