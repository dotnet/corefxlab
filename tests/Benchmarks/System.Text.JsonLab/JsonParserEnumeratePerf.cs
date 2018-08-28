// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BenchmarkDotNet.Attributes;
using Benchmarks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace System.Text.JsonLab.Benchmarks
{
    [MemoryDiagnoser]
    public class JsonParserEnumeratePerf
    {
        private byte[] _dataUtf8;
        private MemoryStream _stream;
        private StreamReader _reader;

        [GlobalSetup]
        public void Setup()
        {
            string jsonString = JsonStrings.Json400KB;

            // Remove all formatting/indendation
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

            _dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            _stream = new MemoryStream(_dataUtf8);
            _reader = new StreamReader(_stream, Encoding.UTF8, false, 1024, true);
        }

        [Benchmark(Baseline = true)]
        public void ParseNewtonsoftEnumerate()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            using (JsonTextReader jsonReader = new JsonTextReader(_reader))
            {
                JToken obj = JToken.ReadFrom(jsonReader);

                for (int j = 0; j < 100; j++)
                {
                    foreach (JToken token in obj)
                    {
                    }
                }
            }
        }

        [Benchmark]
        public void ParseNewtonsoftEnumerateReverse()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            using (JsonTextReader jsonReader = new JsonTextReader(_reader))
            {
                JToken obj = JToken.ReadFrom(jsonReader);

                for (int j = 0; j < 100; j++)
                {
                    for (int i = 299; i >= 0; i--)
                    {
                        JToken token = obj[i];
                    }
                }
            }
        }

        [Benchmark]
        public void ParseNewtonsoftFirst()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            using (JsonTextReader jsonReader = new JsonTextReader(_reader))
            {
                JToken obj = JToken.ReadFrom(jsonReader);

                for (int j = 0; j < 100; j++)
                {
                    for (int i = 0; i < 300; i++)
                    {
                        JToken token = obj[0];
                    }
                }
            }
        }

        [Benchmark]
        public void ParseNewtonsoftMiddle()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            using (JsonTextReader jsonReader = new JsonTextReader(_reader))
            {
                JToken obj = JToken.ReadFrom(jsonReader);

                for (int j = 0; j < 100; j++)
                {
                    for (int i = 0; i < 300; i++)
                    {
                        JToken token = obj[150];
                    }
                }
            }
        }

        [Benchmark]
        public void ParseSystemTextJsonLabEnumerate()
        {
            JsonObject obj = JsonObject.Parse(_dataUtf8);

            for (int j = 0; j < 100; j++)
            {
                for (int i = 0; i < obj.ArrayLength; i++)
                {
                    JsonObject withinArray = obj[i];
                }
            }

            obj.Dispose();
        }

        [Benchmark]
        public void ParseSystemTextJsonLabEnumerateReverse()
        {
            JsonObject obj = JsonObject.Parse(_dataUtf8);

            for (int j = 0; j < 100; j++)
            {
                for (int i = obj.ArrayLength - 1; i >= 0; i--)
                {
                    JsonObject withinArray = obj[i];
                }
            }

            obj.Dispose();
        }

        [Benchmark]
        public void ParseSystemTextJsonLabFirst()
        {
            JsonObject obj = JsonObject.Parse(_dataUtf8);

            for (int j = 0; j < 100; j++)
            {
                for (int i = 0; i < obj.ArrayLength; i++)
                {
                    JsonObject withinArray = obj[0];
                }
            }

            obj.Dispose();
        }

        [Benchmark]
        public void ParseSystemTextJsonLabMiddle()
        {
            JsonObject obj = JsonObject.Parse(_dataUtf8);

            for (int j = 0; j < 100; j++)
            {
                for (int i = 0; i < obj.ArrayLength; i++)
                {
                    JsonObject withinArray = obj[150];
                }
            }

            obj.Dispose();
        }
    }
}
