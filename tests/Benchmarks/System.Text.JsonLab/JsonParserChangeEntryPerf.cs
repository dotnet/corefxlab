// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using Benchmarks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Formatting;

namespace System.Text.JsonLab.Benchmarks
{
    [MemoryDiagnoser]
    public class JsonParserChangeEntryPerf
    {
        // Keep the JsonStrings resource names in sync with TestCaseType enum values.
        public enum TestCaseType
        {
            DepsJsonSignalR,
            DepsJsonWeather,
            DepsJsonWebSockets
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

        public static IEnumerable<TestCaseType> TestCaseValues() => (IEnumerable<TestCaseType>)Enum.GetValues(typeof(TestCaseType));

        [GlobalSetup]
        public void Setup()
        {
            string jsonString = JsonStrings.ResourceManager.GetString(TestCase.ToString());
            _dataUtf8 = Encoding.UTF8.GetBytes(jsonString);

            _stream = new MemoryStream(_dataUtf8);
            _reader = new StreamReader(_stream, Encoding.UTF8, false, 1024, true);

            _output = new byte[_dataUtf8.Length];
            _outputStream = new MemoryStream(_output);
            _writer = new StreamWriter(_outputStream, Encoding.UTF8, 1024, true);

            _arrayOutput = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
        }

        [Benchmark(Baseline = true)]
        public void ChangeEntryPointLibraryNameNewtonsoft()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            TextReader reader = _reader;
            JToken deps;
            using (var jsonReader = new JsonTextReader(reader))
            {
                deps = JToken.ReadFrom(jsonReader);
            }

            foreach (JProperty target in deps["targets"])
            {
                JProperty targetLibrary = target.Value.Children<JProperty>().FirstOrDefault();
                if (targetLibrary == null)
                {
                    continue;
                }
                targetLibrary.Remove();
            }

            JProperty library = deps["libraries"].Children<JProperty>().First();
            library.Remove();

            _outputStream.Seek(0, SeekOrigin.Begin);
            TextWriter writer = _writer;
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                deps.WriteTo(jsonWriter);
            }
        }

        [Benchmark]
        public void ChangeEntryPointLibraryNameJsonLab()
        {
            JsonObject obj = JsonObject.Parse(_dataUtf8);

            foreach (JsonObject target in obj["targets"])
            {
                if (target.TryGetChild(out JsonObject firstChild))
                    obj.Remove(firstChild);
            }

            JsonObject libraries = obj["libraries"];
            if (libraries.TryGetChild(out JsonObject child))
                obj.Remove(child);

            _arrayOutput.Clear();
            var jsonUtf8 = new Utf8JsonWriter<ArrayFormatterWrapper>(_arrayOutput);
            jsonUtf8.Write(obj);
            jsonUtf8.Flush();

            obj.Dispose();
        }
    }
}
