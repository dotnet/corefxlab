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
    // Since there are 120 tests here (8 * 15), setting low values for the warmupCount and targetCount
    //[SimpleJob(-1, 3, 5)]
    //[MemoryDiagnoser]
    public class JsonParserPerf
    {
        // Keep the JsonStrings resource names in sync with TestCaseType enum values.
        public enum TestCaseType
        {
            HelloWorld,
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

        [Params(true/*, false*/)]
        public bool IsDataCompact;

        [Params(/*true,*/ false)]
        public bool OnlyParse;

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

                if (!OnlyParse)
                {
                    if (TestCase == TestCaseType.HelloWorld)
                    {
                        ReadHelloWorld(obj);
                    }
                    /*else if (TestCase == TestCaseType.Json400B)
                    {
                        ReadJson400B(obj);
                    }
                    else if (TestCase == TestCaseType.BasicJson)
                    {
                        ReadJsonBasic(obj);
                    }*/
                    else if (TestCase == TestCaseType.Json400KB/* || TestCase == TestCaseType.Json40KB || TestCase == TestCaseType.Json4KB*/)
                    {
                        ReadJson400KB(obj);
                    }
                }
            }
        }

        [Benchmark(Baseline = true)]
        public void ParseSystemTextJsonLab()
        {
            JsonObject obj = JsonObject.Parse(_dataUtf8);
            if (!OnlyParse)
            {
                if (TestCase == TestCaseType.HelloWorld)
                {
                    ReadHelloWorld(obj);
                }
                /*else if (TestCase == TestCaseType.Json400B)
                {
                    ReadJson400B(obj);
                }
                else if (TestCase == TestCaseType.BasicJson)
                {
                    ReadJsonBasic(obj);
                }*/
                else if (TestCase == TestCaseType.Json400KB/* || TestCase == TestCaseType.Json40KB || TestCase == TestCaseType.Json4KB*/)
                {
                    ReadJson400KB(obj);
                }
            }
            obj.Dispose();
        }

        private static string ReadHelloWorld(JToken obj)
        {
            string message = (string)obj["message"];
            return message;
        }

        private static string ReadJson400KB(JToken obj)
        {
            var sb = new StringBuilder();
            foreach (JToken token in obj)
            {
                sb.Append((string)token["_id"]);
                sb.Append((int)token["index"]);
                sb.Append((string)token["guid"]);
                sb.Append((bool)token["isActive"]);
                sb.Append((string)token["balance"]);
                sb.Append((string)token["picture"]);
                sb.Append((int)token["age"]);
                sb.Append((string)token["eyeColor"]);
                sb.Append((string)token["name"]);
                sb.Append((string)token["gender"]);
                sb.Append((string)token["company"]);
                sb.Append((string)token["email"]);
                sb.Append((string)token["phone"]);
                sb.Append((string)token["address"]);
                sb.Append((string)token["about"]);
                sb.Append((string)token["registered"]);
                sb.Append((double)token["latitude"]);
                sb.Append((double)token["longitude"]);

                JToken tags = token["tags"];
                foreach (JToken tag in tags)
                {
                    sb.Append((string)tag);
                }
                JToken friends = token["friends"];
                foreach (JToken friend in friends)
                {
                    sb.Append((int)friend["id"]);
                    sb.Append((string)friend["name"]);
                }
                sb.Append((string)token["greeting"]);
                sb.Append((string)token["favoriteFruit"]);

            }
            return sb.ToString();
        }

        private static string ReadJson400B(JToken obj)
        {
            var sb = new StringBuilder();
            foreach (JToken token in obj)
            {
                sb.Append((string)token["_id"]);
                sb.Append((int)token["index"]);
                sb.Append((bool)token["isActive"]);
                sb.Append((string)token["balance"]);
                sb.Append((string)token["picture"]);
                sb.Append((int)token["age"]);
                sb.Append((string)token["email"]);
                sb.Append((string)token["phone"]);
                sb.Append((string)token["address"]);
                sb.Append((string)token["registered"]);
                sb.Append((double)token["latitude"]);
                sb.Append((double)token["longitude"]);
            }
            return sb.ToString();
        }

        private static string ReadJsonBasic(JToken obj)
        {
            var sb = new StringBuilder();
            sb.Append((int)obj["age"]);
            sb.Append((string)obj["first"]);
            sb.Append((string)obj["last"]);
            JToken phoneNumbers = obj["phoneNumbers"];
            foreach (JToken phoneNumber in phoneNumbers)
            {
                sb.Append((string)phoneNumber);
            }
            JToken address = obj["address"];
            sb.Append((string)address["street"]);
            sb.Append((string)address["city"]);
            sb.Append((string)address["zip"]);
            return sb.ToString();
        }

        private static string ReadHelloWorld(JsonObject obj)
        {
            string message = (string)obj["message"];
            return message;
        }

        private static string ReadJson400KB(JsonObject obj)
        {
            Utf8Span _id = (Utf8Span)"_id";
            Utf8Span index = (Utf8Span)"index";
            Utf8Span guid = (Utf8Span)"guid";
            Utf8Span isActive = (Utf8Span)"isActive";
            Utf8Span balance = (Utf8Span)"balance";
            Utf8Span picture = (Utf8Span)"picture";
            Utf8Span age = (Utf8Span)"age";
            Utf8Span eyeColor = (Utf8Span)"eyeColor";
            Utf8Span name = (Utf8Span)"name";
            Utf8Span gender = (Utf8Span)"gender";
            Utf8Span company = (Utf8Span)"company";
            Utf8Span email = (Utf8Span)"email";
            Utf8Span phone = (Utf8Span)"phone";
            Utf8Span address = (Utf8Span)"address";
            Utf8Span about = (Utf8Span)"about";
            Utf8Span registered = (Utf8Span)"registered";
            Utf8Span latitude = (Utf8Span)"latitude";
            Utf8Span longitude = (Utf8Span)"longitude";
            Utf8Span tags = (Utf8Span)"tags";
            Utf8Span friends = (Utf8Span)"friends";
            Utf8Span id = (Utf8Span)"id";
            Utf8Span greeting = (Utf8Span)"greeting";
            Utf8Span favoriteFruit = (Utf8Span)"favoriteFruit";

            var sb = new StringBuilder();
            for (int i = 0; i < obj.ArrayLength; i++)
            {
                /*sb.Append((string)obj[i]["_id"]);
                sb.Append((int)obj[i]["index"]);
                sb.Append((string)obj[i]["guid"]);
                sb.Append((bool)obj[i]["isActive"]);
                sb.Append((string)obj[i]["balance"]);
                sb.Append((string)obj[i]["picture"]);
                sb.Append((int)obj[i]["age"]);
                sb.Append((string)obj[i]["eyeColor"]);
                sb.Append((string)obj[i]["name"]);
                sb.Append((string)obj[i]["gender"]);
                sb.Append((string)obj[i]["company"]);
                sb.Append((string)obj[i]["email"]);
                sb.Append((string)obj[i]["phone"]);
                sb.Append((string)obj[i]["address"]);
                sb.Append((string)obj[i]["about"]);
                sb.Append((string)obj[i]["registered"]);
                sb.Append((double)obj[i]["latitude"]);
                sb.Append((double)obj[i]["longitude"]);

                JsonObject tags = obj[i]["tags"];
                for (int j = 0; j < tags.ArrayLength; j++)
                {
                    sb.Append((string)tags[j]);
                }
                JsonObject friends = obj[i]["friends"];
                for (int j = 0; j < friends.ArrayLength; j++)
                {
                    sb.Append((int)friends[j]["id"]);
                    sb.Append((string)friends[j]["name"]);
                }
                sb.Append((string)obj[i]["greeting"]);
                sb.Append((string)obj[i]["favoriteFruit"]);*/

                sb.Append((string)obj[i][_id]);
                sb.Append((int)obj[i][index]);
                sb.Append((string)obj[i][guid]);
                sb.Append((bool)obj[i][isActive]);
                sb.Append((string)obj[i][balance]);
                sb.Append((string)obj[i][picture]);
                sb.Append((int)obj[i][age]);
                sb.Append((string)obj[i][eyeColor]);
                sb.Append((string)obj[i][name]);
                sb.Append((string)obj[i][gender]);
                sb.Append((string)obj[i][company]);
                sb.Append((string)obj[i][email]);
                sb.Append((string)obj[i][phone]);
                sb.Append((string)obj[i][address]);
                sb.Append((string)obj[i][about]);
                sb.Append((string)obj[i][registered]);
                sb.Append((double)obj[i][latitude]);
                sb.Append((double)obj[i][longitude]);

                JsonObject tagsObject = obj[i][tags];
                for (int j = 0; j < tagsObject.ArrayLength; j++)
                {
                    sb.Append((string)tagsObject[j]);
                }
                JsonObject friendsObject = obj[i][friends];
                for (int j = 0; j < friendsObject.ArrayLength; j++)
                {
                    sb.Append((int)friendsObject[j][id]);
                    sb.Append((string)friendsObject[j][name]);
                }
                sb.Append((string)obj[i][greeting]);
                sb.Append((string)obj[i][favoriteFruit]);
            }
            return sb.ToString();
        }

        private static string ReadJson400B(JsonObject obj)
        {
            Utf8Span _id = (Utf8Span)"_id";
            Utf8Span index = (Utf8Span)"index";
            Utf8Span isActive = (Utf8Span)"isActive";
            Utf8Span balance = (Utf8Span)"balance";
            Utf8Span picture = (Utf8Span)"picture";
            Utf8Span age = (Utf8Span)"age";
            Utf8Span email = (Utf8Span)"email";
            Utf8Span phone = (Utf8Span)"phone";
            Utf8Span address = (Utf8Span)"address";
            Utf8Span registered = (Utf8Span)"registered";
            Utf8Span latitude = (Utf8Span)"latitude";
            Utf8Span longitude = (Utf8Span)"longitude";

            var sb = new StringBuilder();
            for (int i = 0; i < obj.ArrayLength; i++)
            {
                sb.Append((string)obj[i][_id]);
                sb.Append((int)obj[i][index]);
                sb.Append((bool)obj[i][isActive]);
                sb.Append((string)obj[i][balance]);
                sb.Append((string)obj[i][picture]);
                sb.Append((int)obj[i][age]);
                sb.Append((string)obj[i][email]);
                sb.Append((string)obj[i][phone]);
                sb.Append((string)obj[i][address]);
                sb.Append((string)obj[i][registered]);
                sb.Append((double)obj[i][latitude]);
                sb.Append((double)obj[i][longitude]);
            }
            return sb.ToString();
        }

        private static string ReadJsonBasic(JsonObject obj)
        {
            var sb = new StringBuilder();
            sb.Append((int)obj["age"]);
            sb.Append((string)obj["first"]);
            sb.Append((string)obj["last"]);
            JsonObject phoneNumbers = obj["phoneNumbers"];
            for (int i = 0; i < phoneNumbers.ArrayLength; i++)
            {
                sb.Append((string)phoneNumbers[i]);
            }
            JsonObject address = obj["address"];
            sb.Append((string)address["street"]);
            sb.Append((string)address["city"]);
            sb.Append((string)address["zip"]);
            return sb.ToString();
        }

        //[Benchmark]
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
        public void ParseSystemTextJsonLabConstantAccess()
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
            else if (TestCase == TestCaseType.HelloWorld)
            {
                var lookup = new Utf8Span("message");

                for (int i = 0; i < 10; i++)
                {
                    Utf8Span message = (Utf8Span)obj[lookup];
                }
            }
        }
    }
}
