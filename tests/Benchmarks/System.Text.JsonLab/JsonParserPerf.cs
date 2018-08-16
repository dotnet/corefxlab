// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BenchmarkDotNet.Attributes;
using Benchmarks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace System.Text.JsonLab.Benchmarks
{
    // Since there are 30 tests here (2 * 15), setting low values for the warmupCount and targetCount
    [SimpleJob(-1, 3, 5)]
    [MemoryDiagnoser]
    public class JsonParserPerf
    {
        private byte[] _dataUtf8;
        private MemoryStream _stream;
        private StreamReader _reader;

        [ParamsSource(nameof(TestCaseValues))]
        public TestCaseType TestCase;

        public static IEnumerable<TestCaseType> TestCaseValues() => (IEnumerable<TestCaseType>)Enum.GetValues(typeof(TestCaseType));

        [GlobalSetup]
        public void Setup()
        {
            string jsonString = JsonStrings.ResourceManager.GetString(TestCase.ToString());
            _dataUtf8 = Encoding.UTF8.GetBytes(jsonString);

            _stream = new MemoryStream(_dataUtf8);
            _reader = new StreamReader(_stream, Encoding.UTF8, false, 1024, true);
        }

        [Benchmark(Baseline = true)]
        public void ParseNewtonsoft()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            using (JsonTextReader jsonReader = new JsonTextReader(_reader))
            {
                JToken.ReadFrom(jsonReader);
            }
        }

        [Benchmark]
        public void ParseSystemTextJsonLab()
        {
            JsonObject.Parse(_dataUtf8);
        }
    }
}
