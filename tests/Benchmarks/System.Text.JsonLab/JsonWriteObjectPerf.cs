// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BenchmarkDotNet.Attributes;
using Benchmarks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Text.Formatting;

namespace System.Text.JsonLab.Benchmarks
{
    // Since there are 60 tests here (4 * 15), setting low values for the warmupCount and targetCount
    [SimpleJob(-1, 3, 5)]
    [MemoryDiagnoser]
    public class JsonWriteObjectPerf
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

        private byte[] _dataUtf8;
        private MemoryStream _stream;
        private StreamReader _reader;

        private byte[] _output;
        private MemoryStream _outputStream;
        private StreamWriter _writer;

        private ArrayFormatterWrapper _arrayOutput;

        [ParamsSource(nameof(TestCaseValues))]
        public TestCaseType TestCase;

        [Params(true, false)]
        public bool IsDataCompact;

        public static IEnumerable<TestCaseType> TestCaseValues() => (IEnumerable<TestCaseType>)Enum.GetValues(typeof(TestCaseType));

        [GlobalSetup]
        public void Setup()
        {
            string jsonString = JsonStrings.ResourceManager.GetString(TestCase.ToString());

            // Remove all formatting/indendation
            if (IsDataCompact)
            {
                using (var jsonReader = new JsonTextReader(new StringReader(jsonString)))
                {
                    JToken obj = JToken.ReadFrom(jsonReader);
                    var stringWriter = new StringWriter();
                    using (var jsonWriter = new JsonTextWriter(stringWriter))
                    {
                        obj.WriteTo(jsonWriter);
                        jsonString = stringWriter.ToString();
                    }
                }
            }

            _dataUtf8 = Encoding.UTF8.GetBytes(jsonString);

            _stream = new MemoryStream(_dataUtf8);
            _reader = new StreamReader(_stream, Encoding.UTF8, false, 1024, true);

            _output = new byte[_dataUtf8.Length];

            _outputStream = new MemoryStream(_output);
            _writer = new StreamWriter(_outputStream, new UTF8Encoding(), 1024, true); // Do not output the BOM

            _arrayOutput = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
        }

        [Benchmark(Baseline = true)]
        public void ParseAndWriteNewtonsoft()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            using (var jsonReader = new JsonTextReader(_reader))
            {
                JToken obj = JToken.ReadFrom(jsonReader);

                _outputStream.Seek(0, SeekOrigin.Begin);
                TextWriter writer = _writer;
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    obj.WriteTo(jsonWriter);
                }
            }
        }

        [Benchmark]
        public void ParseAndWriteSystemTextJsonLab()
        {
            JsonObject obj = JsonObject.Parse(_dataUtf8);

            _arrayOutput.Clear();
            var jsonUtf8 = new Utf8JsonWriter<ArrayFormatterWrapper>(_arrayOutput);
            jsonUtf8.Write(obj);
            jsonUtf8.Flush();

            obj.Dispose();
        }
    }
}
