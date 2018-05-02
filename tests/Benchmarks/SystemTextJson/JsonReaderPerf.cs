// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BenchmarkDotNet.Attributes;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Text;

using static Benchmarks.SystemTextJson.Helper;

namespace Benchmarks.SystemTextJson
{
    [MemoryDiagnoser]
    public class JsonReaderPerf
    {
        private byte[] data;
        private SymbolTable symbolTable;
        private MemoryStream mem;
        private StreamReader reader;

#pragma warning disable CS0649 // Field '' is never assigned to, and will always have its default value
        [Params(EncoderTarget.InvariantUtf8, EncoderTarget.InvariantUtf16)]
        public EncoderTarget Target;

        // Using the string name listed in the resource file instead of the json string directly
        // so that the benchmark output is cleaner
        [ParamsSource(nameof(ValuesForJsonStringName))]
        public string JsonStringName;
#pragma warning restore CS0649

        public static IEnumerable<string> ValuesForJsonStringName() => new[] { nameof(JsonStrings.HeavyNestedJson), nameof(JsonStrings.HelloWorld) };

        [GlobalSetup]
        public void Setup()
        {
            symbolTable = GetTargetEncoder(Target);
            data = EncodeTestData(Target, JsonStrings.ResourceManager.GetString(JsonStringName));

            mem = new MemoryStream(data);
            var enc = Target == EncoderTarget.InvariantUtf8 ? Encoding.UTF8 : Encoding.Unicode;
            reader = new StreamReader(mem, enc, false, 1024, true);
        }

        [Benchmark]
        public void ReaderSystemTextJson() => TestReaderSystemTextJson(data, symbolTable);

        [Benchmark(Baseline = true)]
        public void ReaderNewtonsoft()
        {
            mem.Seek(0, SeekOrigin.Begin);
            TestReaderNewtonsoft(reader);
        }

        private static void TestReaderSystemTextJson(ReadOnlySpan<byte> data, SymbolTable symbolTable)
        {
            var json = new System.Text.Json.JsonReader(data, symbolTable);
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
