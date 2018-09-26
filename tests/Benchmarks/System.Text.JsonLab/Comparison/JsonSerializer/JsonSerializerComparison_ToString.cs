// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;

namespace JsonBenchmarks
{
    [MemoryDiagnoser]
    public class JsonSerializerComparison_ToString<T> where T : IVerifiable
    {
        private readonly T value;

        public JsonSerializerComparison_ToString() => value = DataGenerator.Generate<T>();

        [Benchmark(Baseline = true, Description = "Newtonsoft")]
        public string Newtonsoft_() => Newtonsoft.Json.JsonConvert.SerializeObject(value);

        [Benchmark(Description = "Jil")]
        public string Jil_() => Jil.JSON.Serialize(value);

        [Benchmark(Description = "Utf8Json")]
        public string Utf8Json_() => Utf8Json.JsonSerializer.ToJsonString(value);

        [Benchmark]
        public string FastJson() => fastJSON.JSON.ToJSON(value);

        // This benchmarks fails to run for IndexViewModel and MyEventsListerViewModel.
        //[Benchmark(Description = "LitJson")]
        public string LitJson_() => LitJson.JsonMapper.ToJson(value);

        [Benchmark(Description = "Manatee")]
        public string Manatee_() => new Manatee.Json.Serialization.JsonSerializer().Serialize(value).ToString();

        [Benchmark]
        public string SpanJsonUtf16() => SpanJson.JsonSerializer.Generic.Utf16.Serialize(value);
    }
}
