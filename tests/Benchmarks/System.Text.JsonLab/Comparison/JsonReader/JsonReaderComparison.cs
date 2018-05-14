// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BenchmarkDotNet.Attributes;
using Benchmarks;
using Jayrock.Json;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Text;

using JsonLab = System.Text.JsonLab;

using static System.Text.JsonLab.Benchmarks.Helper;

namespace JsonBenchmarks
{
    [MemoryDiagnoser]
    public class JsonReaderComparison
    {
        private string _str;
        private byte[] _data;
        private SymbolTable _symbolTable;
        private MemoryStream _stream;
        private StreamReader _reader;

        [Params(EncoderTarget.InvariantUtf8)]
        public EncoderTarget Target;

        // Using the string name listed in the resource file instead of the json string directly
        // so that the benchmark output is cleaner
        [ParamsSource(nameof(ValuesForJsonStringName))]
        public string JsonStringName;

        public static IEnumerable<string> ValuesForJsonStringName() => new[] { nameof(JsonStrings.HeavyNestedJson), nameof(JsonStrings.HelloWorld) };

        [GlobalSetup]
        public void Setup()
        {
            _symbolTable = Target == EncoderTarget.InvariantUtf16 ? SymbolTable.InvariantUtf16 : SymbolTable.InvariantUtf8;
            
            _str = JsonStrings.ResourceManager.GetString(JsonStringName);
            _data = Target == EncoderTarget.InvariantUtf16 ? Encoding.Unicode.GetBytes(_str) : Encoding.UTF8.GetBytes(_str);

            _stream = new MemoryStream(_data);
            Encoding encoding = Target == EncoderTarget.InvariantUtf16 ? Encoding.Unicode : Encoding.UTF8;
            _reader = new StreamReader(_stream, encoding, detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true);
        }

        [Benchmark(Baseline = true)]
        public void ReaderNewtonsoft()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            using (var json = new Newtonsoft.Json.JsonTextReader(_reader))
                while (json.Read()) ;
        }

        [Benchmark]
        public void ReaderCorefxlab()
        {
            var json = new JsonLab.JsonReader(_data, _symbolTable);
            while (json.Read()) ;
        }

        [Benchmark]
        public void ReaderUtf8Json()
        {
            Utf8Json.JsonReader json = new Utf8Json.JsonReader(_data);

            while (json.GetCurrentJsonToken() != Utf8Json.JsonToken.None)
            {
                json.ReadNext();
            }
        }

        [Benchmark]
        public void ReaderJayrock()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            using (var json = new JsonTextReader(_reader))
                while (json.Read()) ;
        }

        [Benchmark]
        public void ReaderLitJson()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            var json = new LitJson.JsonReader(_reader);
            while (json.Read()) ;
        }

        [Benchmark]
        public void ReaderSpanJsonUtf8()
        {
            SpanJson.JsonReader<byte> json = new SpanJson.JsonReader<byte>(_data);
            SpanJson.JsonToken token;
            while ((token = json.ReadUtf8NextToken()) != SpanJson.JsonToken.None)
            {
                json.SkipNextUtf8Value(token);
            }
        }
    }
}
