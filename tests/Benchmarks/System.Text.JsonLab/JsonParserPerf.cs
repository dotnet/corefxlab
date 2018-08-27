// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BenchmarkDotNet.Attributes;
using Benchmarks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.Utf8;

namespace System.Text.JsonLab.Benchmarks
{
    // Since there are 30 tests here (2 * 15), setting low values for the warmupCount and targetCount
    //[SimpleJob(-1, 3, 5)]
    [MemoryDiagnoser]
    public class JsonParserPerf
    {
        // Keep the JsonStrings resource names in sync with TestCaseType enum values.
        public enum TestCaseType
        {
            //HelloWorld,
            //BasicJson,
            //BasicLargeNum,
            //SpecialNumForm,
            //ProjectLockJson,
            //FullSchema1,
            //FullSchema2,
            //DeepTree,
            //BroadTree,
            //LotsOfNumbers,
            //LotsOfStrings,
            //Json400B,
            //Json4KB,
            //Json40KB,
            Json400KB
        }

        private byte[] _dataUtf8;
        private MemoryStream _stream;
        private StreamReader _reader;

        [ParamsSource(nameof(TestCaseValues))]
        public TestCaseType TestCase;

        [Params(true)]
        public bool IsDataCompact;

        public static IEnumerable<TestCaseType> TestCaseValues() => (IEnumerable<TestCaseType>)Enum.GetValues(typeof(TestCaseType));

        [GlobalSetup]
        public void Setup()
        {
            string jsonString = JsonStrings.ResourceManager.GetString(TestCase.ToString());

            // Remove all formatting/indendation
            if (IsDataCompact)
            {
                using (JsonTextReader jsonReader = new JsonTextReader(new StringReader(jsonString)))
                {
                    JToken obj = JToken.ReadFrom(jsonReader);
                    var stringWriter = new StringWriter();
                    using (JsonTextWriter jsonWriter = new JsonTextWriter(stringWriter))
                    {
                        obj.WriteTo(jsonWriter);
                        jsonString = stringWriter.ToString();
                    }
                }
            }

            _dataUtf8 = Encoding.UTF8.GetBytes(jsonString);

            _stream = new MemoryStream(_dataUtf8);
            _reader = new StreamReader(_stream, Encoding.UTF8, false, 1024, true);
        }

        //[Benchmark(Baseline = true)]
        public void ParseNewtonsoft()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            using (JsonTextReader jsonReader = new JsonTextReader(_reader))
            {
                JToken obj = JToken.ReadFrom(jsonReader);

                if (TestCase == TestCaseType.Json400KB)
                {
                    var lookup = "email";

                    for (int i = 0; i < 10_000; i++)
                    {
                        string email = (string)obj[5][lookup];
                    }
                }
                /*else if (TestCase == TestCaseType.HelloWorld)
                {
                    var lookup = "message";

                    for (int i = 0; i < 10; i++)
                    {
                        string message = (string)obj[lookup];
                    }
                }*/
            }
        }

        [Benchmark]
        public void ParseSystemTextJsonLabScanSeveralProperties()
        {
            JsonObject obj = JsonObject.Parse(_dataUtf8);

            var lookup1 = new Utf8Span("about");
            var lookup4 = new Utf8Span("greeting");
            var lookup2 = new Utf8Span("friends");
            var lookup3 = new Utf8Span("name");
            var lookup5 = new Utf8Span("age");

            for (int k = 0; k < 2; k++)
            {
                for (int i = 0; i < obj.ArrayLength; i += 10)
                {
                    for (int j = 0; j < 300; j++)
                    {
                        string greeting = (string)obj[5][lookup4];
                    }
                    string about = (string)obj[i][lookup1];
                    string temp = (string)obj[i][lookup2][1][lookup3];
                    int age = (int)obj[i][lookup5];
                }
            }

            obj.Dispose();
        }


        //[Benchmark]
        public void ParseSystemTextJsonLabScanProperties()
        {
            JsonObject obj = JsonObject.Parse(_dataUtf8);

            var lookup4 = new Utf8Span("greeting");
            for (int i = 0; i < 20_000; i++)
            {
                string id = (string)obj[10][lookup4];
            }

            obj.Dispose();
        }

        //[Benchmark]
        public void ParseSystemTextJsonLabEnumerate()
        {
            JsonObject obj = JsonObject.Parse(_dataUtf8);

            for (int i = 0; i < obj.ArrayLength; i++)
            {
                JsonObject withinArray = obj[i];
            }

            obj.Dispose();
        }

        //[Benchmark]
        public void ParseSystemTextJsonLabEnumerateReverse()
        {
            JsonObject obj = JsonObject.Parse(_dataUtf8);

            for (int i = obj.ArrayLength - 1; i >= 0; i--)
            {
                JsonObject withinArray = obj[i];
            }

            obj.Dispose();
        }

        //[Benchmark]
        public void ParseSystemTextJsonLabFirst()
        {
            JsonObject obj = JsonObject.Parse(_dataUtf8);

            for (int i = 0; i < obj.ArrayLength; i++)
            {
                JsonObject withinArray = obj[0];
            }

            obj.Dispose();
        }

        //[Benchmark]
        public void ParseSystemTextJsonLab1()
        {
            //var parser = new JsonParser(_dataUtf8);
            JsonObject obj = JsonObject.Parse(_dataUtf8);

            if (TestCase == TestCaseType.Json400KB)
            {
                var lookup = new Utf8Span("email");

                for (int i = 0; i < obj.ArrayLength; i++)
                {
                    JsonObject withinArray = obj[i];
                }

                /*for (int i = 0; i < 10_000; i++)
                {
                    Utf8Span email = (Utf8Span)obj[5][lookup];
                }*/
            }
            /*else if (TestCase == TestCaseType.HelloWorld)
            {
                var lookup = new Utf8Span("message");

                for (int i = 0; i < 10; i++)
                {
                    Utf8Span message = (Utf8Span)obj[lookup];
                }
            }*/
            obj.Dispose();
        }

        //[Benchmark]
        public void ParseSystemTextJsonLab3()
        {
            JsonObject obj = JsonObject.Parse(_dataUtf8);

            if (TestCase == TestCaseType.Json400KB)
            {
                var lookup = new Utf8Span("email");

                for (int i = 0; i < 10_000; i++)
                {
                    JsonObject temp = obj[5];
                }
            }
            /*else if (TestCase == TestCaseType.HelloWorld)
            {
                var lookup = new Utf8Span("message");

                for (int i = 0; i < 10; i++)
                {
                    Utf8Span message = (Utf8Span)obj[lookup];
                }
            }*/
        }

        //[Benchmark]
        public void ParseSystemTextJsonLab4()
        {
            JsonObject obj = JsonObject.Parse(_dataUtf8);

            if (TestCase == TestCaseType.Json400KB)
            {
                var lookup = new Utf8Span("email");

                for (int i = 0; i < 10_000; i++)
                {
                    JsonObject temp = obj[5][lookup];
                }
            }
            /*else if (TestCase == TestCaseType.HelloWorld)
            {
                var lookup = new Utf8Span("message");

                for (int i = 0; i < 10; i++)
                {
                    Utf8Span message = (Utf8Span)obj[lookup];
                }
            }*/
        }

        //[Benchmark]
        public void ParseSystemTextJsonLab5()
        {
            JsonObject obj = JsonObject.Parse(_dataUtf8);

            if (TestCase == TestCaseType.Json400KB)
            {
                var lookup = new Utf8Span("email");

                for (int i = 0; i < 10_000; i++)
                {
                    Utf8Span message = (Utf8Span)obj[5][lookup];
                }
            }
            /*else if (TestCase == TestCaseType.HelloWorld)
            {
                var lookup = new Utf8Span("message");

                for (int i = 0; i < 10; i++)
                {
                    Utf8Span message = (Utf8Span)obj[lookup];
                }
            }*/
        }
    }
}
