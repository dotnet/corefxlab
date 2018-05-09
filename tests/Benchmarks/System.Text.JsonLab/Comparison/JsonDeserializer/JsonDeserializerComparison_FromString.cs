// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BenchmarkDotNet.Attributes;

namespace JsonBenchmarks
{
    [MemoryDiagnoser]
    public class JsonDeserializerComparison_FromString<T> where T : IVerifiable
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
            //string str = "{\"Email\":\"name.familyname@not.com\",\"Email1\":\"1name.familyname@not.com\",\"Email2\":\"2name.familyname@not.com\",\"Email3\":\"3name.familyname@not.com\",\"Email4\":\"4name.familyname@not.com\",\"Email5\":\"5name.familyname@not.com\",\"Email6\":\"6name.familyname@not.com\",\"Email7\":\"7name.familyname@not.com\",\"Email8\":\"8name.familyname@not.com\",\"Email9\":\"9name.familyname@not.com\",\"Email10\":\"10name.familyname@not.com\",\"Email11\":\"11name.familyname@not.com\",\"Email12\":\"12name.familyname@not.com\",\"Email13\":\"13name.familyname@not.com\",\"Email14\":\"14name.familyname@not.com\",\"Email15\":\"15name.familyname@not.com\",\"Email16\":\"16name.familyname@not.com\",\"Email17\":\"17name.familyname@not.com\",\"Email18\":\"18name.familyname@not.com\",\"Email19\":\"19name.familyname@not.com\",\"Email20\":\"20name.familyname@not.com\",\"Email21\":\"21name.familyname@not.com\",\"Password\":\"abcdefgh123456!@\",\"RememberMe\":true}";
            string str = "{\"Email\":1,\"Email1\":2,\"Email2\":3,\"Email3\":4,\"Email4\":5,\"Email5\":6,\"Email6\":7,\"Email7\":8,\"Email8\":9,\"Email9\":10,\"Email10\":11,\"Email11\":12,\"Email12\":13,\"Email13\":14,\"Email14\":15,\"Email15\":16,\"Email16\":17,\"Email17\":18,\"Email18\":19,\"Email19\":20,\"Email20\":21,\"Email21\":22,\"RememberMe\":true}";
            utf8data = System.Text.Encoding.UTF8.GetBytes(str);
        }

        [IterationSetup(Target = nameof(FastJson))]
        public void SerializeFastJson() => serialized = fastJSON.JSON.ToJSON(value);

        [IterationSetup(Target = nameof(LitJson_))]
        public void SerializeLitJson() => serialized = LitJson.JsonMapper.ToJson(value);

        [IterationSetup(Target = nameof(Manatee_))]
        public void SerializeManatee() => jsonValue = Manatee.Json.JsonValue.Parse(new Manatee.Json.Serialization.JsonSerializer().Serialize(value).ToString());

        //[Benchmark(Description = "Jil")]
        public T Jil_()
        {
            T deserialized = Jil.JSON.Deserialize<T>(serialized);
            ((IVerifiable)deserialized).TouchEveryProperty();
            return deserialized;
        }

        //[Benchmark(Baseline = true, Description = "Newtonsoft")]
        public T Newtonsoft_()
        {
            T deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(serialized);
            ((IVerifiable)deserialized).TouchEveryProperty();
            return deserialized;
        }

        //[Benchmark(Description = "Utf8Json")]
        public T Utf8Json_()
        {
            T deserialized = Utf8Json.JsonSerializer.Deserialize<T>(serialized);
            ((IVerifiable)deserialized).TouchEveryProperty();
            return deserialized;
        }

        //[Benchmark(Description = "YSharp")]
        public T YSharp()
        {
            T deserialized = new System.Text.Json.JsonParser().Parse<T>(serialized);
            ((IVerifiable)deserialized).TouchEveryProperty();
            return deserialized;
        }

        //[Benchmark(Description = "FastJson")]
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

        [Benchmark(Description = "JsonLab")]
        public T JsonLab() => System.Text.JsonLab.JsonDynamicObject.Deserialize<T>(utf8data);
    }
}
