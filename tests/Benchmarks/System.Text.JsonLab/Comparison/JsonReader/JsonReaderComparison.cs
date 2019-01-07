// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using Benchmarks;
using Jayrock.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;

using JsonLab = System.Text.JsonLab;

namespace JsonBenchmarks
{
    [MemoryDiagnoser]
    public class JsonReaderComparison
    {
        private string _str;
        private byte[] _data;
        private MemoryStream _stream;
        private StreamReader _reader;

        // Using the string name listed in the resource file instead of the json string directly
        // so that the benchmark output is cleaner
        [ParamsSource(nameof(ValuesForJsonStringName))]
        public string JsonStringName;

        public static IEnumerable<string> ValuesForJsonStringName() => new[] { nameof(JsonStrings.HeavyNestedJson), nameof(JsonStrings.HelloWorld) };

        [GlobalSetup]
        public void Setup()
        {
            _str = JsonStrings.ResourceManager.GetString(JsonStringName);
            _data = Encoding.UTF8.GetBytes(_str);
            _stream = new MemoryStream(_data);
            _reader = new StreamReader(_stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true);
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
            var json = new JsonLab.JsonUtf8Reader(_data);
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
