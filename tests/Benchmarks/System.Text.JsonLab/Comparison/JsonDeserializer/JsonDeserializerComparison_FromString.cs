// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;

namespace JsonBenchmarks
{
    [MemoryDiagnoser]
    public class JsonDeserializerComparison_FromString<T> where T : IVerifiable
    {
        private readonly T value;
        private string serialized;
        private Manatee.Json.JsonValue jsonValue;

        public JsonDeserializerComparison_FromString() => value = DataGenerator.Generate<T>();

        [IterationSetup(Target = nameof(Newtonsoft_))]
        public void SerializeJsonNet() => serialized = Newtonsoft.Json.JsonConvert.SerializeObject(value);

        [IterationSetup(Target = nameof(Jil_))]
        public void SerializeJil() => serialized = Jil.JSON.Serialize(value);

        [IterationSetup(Target = nameof(Utf8Json_))]
        public void SerializeUtf8Json() => serialized = Utf8Json.JsonSerializer.ToJsonString(value);

        //[IterationSetup(Target = nameof(YSharp))]
        //public void SerializeYSharp() => serialized = Newtonsoft.Json.JsonConvert.SerializeObject(value);

        [IterationSetup(Target = nameof(FastJson))]
        public void SerializeFastJson() => serialized = fastJSON.JSON.ToJSON(value);

        [IterationSetup(Target = nameof(LitJson_))]
        public void SerializeLitJson() => serialized = LitJson.JsonMapper.ToJson(value);

        [IterationSetup(Target = nameof(Manatee_))]
        public void SerializeManatee() => jsonValue = Manatee.Json.JsonValue.Parse(new Manatee.Json.Serialization.JsonSerializer().Serialize(value).ToString());

        [IterationSetup(Target = nameof(SpanJsonUtf16))]
        public void SerializeSpanJsonUtf16() => serialized = SpanJson.JsonSerializer.Generic.Utf16.Serialize(value);

        [Benchmark(Baseline = true, Description = "Newtonsoft")]
        public T Newtonsoft_()
        {
            T deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(serialized);
            ((IVerifiable)deserialized).TouchEveryProperty();
            return deserialized;
        }

        [Benchmark(Description = "Jil")]
        public T Jil_()
        {
            T deserialized = Jil.JSON.Deserialize<T>(serialized);
            ((IVerifiable)deserialized).TouchEveryProperty();
            return deserialized;
        }

        [Benchmark(Description = "Utf8Json")]
        public T Utf8Json_()
        {
            T deserialized = Utf8Json.JsonSerializer.Deserialize<T>(serialized);
            ((IVerifiable)deserialized).TouchEveryProperty();
            return deserialized;
        }

        //[Benchmark]
        //public T YSharp()
        //{
        //    T deserialized = new System.Text.Json.JsonParser().Parse<T>(serialized);
        //    ((IVerifiable)deserialized).TouchEveryProperty();
        //    return deserialized;
        //}

        [Benchmark]
        public T FastJson()
        {
            T deserialized = fastJSON.JSON.ToObject<T>(serialized);
            ((IVerifiable)deserialized).TouchEveryProperty();
            return deserialized;
        }

        // This benchmarks fails to run for IndexViewModel and MyEventsListerViewModel.
        //[Benchmark(Description = "LitJson")]
        public T LitJson_()
        {
            T deserialized = LitJson.JsonMapper.ToObject<T>(serialized);
            ((IVerifiable)deserialized).TouchEveryProperty();
            return deserialized;
        }

        // This benchmarks fails to run for IndexViewModel and MyEventsListerViewModel.
        //[Benchmark(Description = "Manatee")]
        public T Manatee_()
        {
            T deserialized = new Manatee.Json.Serialization.JsonSerializer().Deserialize<T>(jsonValue);
            ((IVerifiable)deserialized).TouchEveryProperty();
            return deserialized;
        }

        [Benchmark]
        public T SpanJsonUtf16()
        {
            T deserialized = SpanJson.JsonSerializer.Generic.Utf16.Deserialize<T>(serialized);
            ((IVerifiable)deserialized).TouchEveryProperty();
            return deserialized;
        }
    }
}
