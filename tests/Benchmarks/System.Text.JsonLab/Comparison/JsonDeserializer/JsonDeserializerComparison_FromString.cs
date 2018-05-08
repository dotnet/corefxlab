// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BenchmarkDotNet.Attributes;

namespace JsonBenchmarks
{
    [MemoryDiagnoser]
    public class JsonDeserializerComparison_FromString<T>
    {
        private readonly T value;
        private string serialized;
        private Manatee.Json.JsonValue jsonValue;
        private byte[] utf8data;

        public JsonDeserializerComparison_FromString() => value = DataGenerator.Generate<T>();

        [IterationSetup(Target = nameof(Jil_))]
        public void SerializeJil() => serialized = Jil.JSON.Serialize(value);

        [IterationSetup(Target = nameof(Newtonsoft_))]
        public void SerializeJsonNet() => serialized = Newtonsoft.Json.JsonConvert.SerializeObject(value);

        [IterationSetup(Target = nameof(Utf8Json_))]
        public void SerializeUtf8Json() => serialized = Utf8Json.JsonSerializer.ToJsonString(value);

        [IterationSetup(Target = nameof(YSharp))]
        public void SerializeYSharp() => serialized = Newtonsoft.Json.JsonConvert.SerializeObject(value);

        [IterationSetup(Target = nameof(JsonLab))]
        public void SerializeJsonLab()
        {
            string str = "{\"Email\":\"name.familyname@not.com\",\"Email1\":\"1name.familyname@not.com\",\"Email2\":\"2name.familyname@not.com\",\"Email3\":\"3name.familyname@not.com\",\"Email4\":\"4name.familyname@not.com\",\"Email5\":\"5name.familyname@not.com\",\"Email6\":\"6name.familyname@not.com\",\"Email7\":\"7name.familyname@not.com\",\"Email8\":\"8name.familyname@not.com\",\"Email9\":\"9name.familyname@not.com\",\"Email10\":\"10name.familyname@not.com\",\"Email11\":\"11name.familyname@not.com\",\"Email12\":\"12name.familyname@not.com\",\"Email13\":\"13name.familyname@not.com\",\"Email14\":\"14name.familyname@not.com\",\"Email15\":\"15name.familyname@not.com\",\"Email16\":\"16name.familyname@not.com\",\"Email17\":\"17name.familyname@not.com\",\"Email18\":\"18name.familyname@not.com\",\"Email19\":\"19name.familyname@not.com\",\"Email20\":\"20name.familyname@not.com\",\"Email21\":\"21name.familyname@not.com\",\"Password\":\"abcdefgh123456!@\",\"RememberMe\":true}";

            utf8data = System.Text.Encoding.UTF8.GetBytes(str);
        }

        [IterationSetup(Target = nameof(FastJson))]
        public void SerializeFastJson() => serialized = fastJSON.JSON.ToJSON(value);

        [IterationSetup(Target = nameof(LitJson_))]
        public void SerializeLitJson() => serialized = LitJson.JsonMapper.ToJson(value);

        [IterationSetup(Target = nameof(Manatee_))]
        public void SerializeManatee() => jsonValue = Manatee.Json.JsonValue.Parse(new Manatee.Json.Serialization.JsonSerializer().Serialize(value).ToString());

        //[Benchmark(Description = "Jil")]
        public T Jil_() => Jil.JSON.Deserialize<T>(serialized);

        //[Benchmark(Baseline = true, Description = "Newtonsoft")]
        public T Newtonsoft_() => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(serialized);

        //[Benchmark(Description = "Utf8Json")]
        public T Utf8Json_() => Utf8Json.JsonSerializer.Deserialize<T>(serialized);

        //[Benchmark(Description = "YSharp")]
        public T YSharp() => new System.Text.Json.JsonParser().Parse<T>(serialized);

        //[Benchmark(Description = "FastJson")]
        public T FastJson() => fastJSON.JSON.ToObject<T>(serialized);

        // This benchmarks fails to run for IndexViewModel and MyEventsListerViewModel.
        //[Benchmark(Description = "LitJson")]
        public T LitJson_() => LitJson.JsonMapper.ToObject<T>(serialized);

        // This benchmarks fails to run for IndexViewModel and MyEventsListerViewModel.
        //[Benchmark(Description = "Manatee")]
        public T Manatee_() => new Manatee.Json.Serialization.JsonSerializer().Deserialize<T>(jsonValue);

        [Benchmark(Description = "JsonLab")]
        public T JsonLab() => System.Text.JsonLab.JsonDynamicObject.Deserialize<T>(utf8data);
    }
}
