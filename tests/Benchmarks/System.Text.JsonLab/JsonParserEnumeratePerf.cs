// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

        private const int ArrayLength = 300; // We know Json400KB has a 300 length array within the payload.

        private const int IterationCount = 1000;

        [GlobalSetup]
        public void Setup()
        {
            string jsonString = JsonStrings.Json400KB;

            // Remove all formatting/indendation
            using (var jsonReader = new JsonTextReader(new StringReader(jsonString)))
            {
                JToken obj = JToken.ReadFrom(jsonReader);
                var stringWriter = new StringWriter();
                using (var jsonWriter = new JsonTextWriter(stringWriter))
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
        public void ParseNewtonsoftEnumerateArray()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            using (var jsonReader = new JsonTextReader(_reader))
            {
                JToken obj = JToken.ReadFrom(jsonReader);

                for (int j = 0; j < IterationCount; j++)
                {
                    foreach (JToken token in obj)
                    {
                    }
                }
            }
        }

        [Benchmark]
        public void ParseNewtonsoftEnumerateArrayReverse()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            using (var jsonReader = new JsonTextReader(_reader))
            {
                JToken obj = JToken.ReadFrom(jsonReader);

                for (int j = 0; j < IterationCount; j++)
                {
                    for (int i = ArrayLength - 1; i >= 0; i--)
                    {
                        JToken token = obj[i];
                    }
                }
            }
        }

        [Benchmark]
        public void ParseNewtonsoftArrayFirst()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            using (var jsonReader = new JsonTextReader(_reader))
            {
                JToken obj = JToken.ReadFrom(jsonReader);

                for (int j = 0; j < IterationCount; j++)
                {
                    for (int i = 0; i < ArrayLength; i++)
                    {
                        JToken token = obj[0];
                    }
                }
            }
        }

        [Benchmark]
        public void ParseNewtonsoftArrayMiddle()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            using (var jsonReader = new JsonTextReader(_reader))
            {
                JToken obj = JToken.ReadFrom(jsonReader);

                for (int j = 0; j < IterationCount; j++)
                {
                    for (int i = 0; i < ArrayLength; i++)
                    {
                        JToken token = obj[ArrayLength / 2];
                    }
                }
            }
        }

        [Benchmark]
        public void ParseNewtonsoftEnumerate()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            using (var jsonReader = new JsonTextReader(_reader))
            {
                JToken obj = JToken.ReadFrom(jsonReader);
                JToken withinArray = obj[0];
                for (int j = 0; j < IterationCount; j++)
                {
                    foreach (JToken childObject in withinArray)
                    {
                    }
                }
            }
        }

        [Benchmark]
        public void ParseSystemTextJsonLabEnumerateArray()
        {
            JsonObject obj = JsonObject.Parse(_dataUtf8);

            for (int j = 0; j < IterationCount; j++)
            {
                for (int i = 0; i < obj.ArrayLength; i++)
                {
                    JsonObject withinArray = obj[i];
                }
            }

            obj.Dispose();
        }

        [Benchmark]
        public void ParseSystemTextJsonLabEnumerateForeachArray()
        {
            JsonObject obj = JsonObject.Parse(_dataUtf8);

            for (int j = 0; j < IterationCount; j++)
            {
                foreach(JsonObject withinArray in obj)
                {
                }
            }

            obj.Dispose();
        }

        [Benchmark]
        public void ParseSystemTextJsonLabEnumerateArrayReverse()
        {
            JsonObject obj = JsonObject.Parse(_dataUtf8);

            for (int j = 0; j < IterationCount; j++)
            {
                for (int i = obj.ArrayLength - 1; i >= 0; i--)
                {
                    JsonObject withinArray = obj[i];
                }
            }

            obj.Dispose();
        }

        [Benchmark]
        public void ParseSystemTextJsonLabArrayFirst()
        {
            JsonObject obj = JsonObject.Parse(_dataUtf8);

            for (int j = 0; j < IterationCount; j++)
            {
                for (int i = 0; i < obj.ArrayLength; i++)
                {
                    JsonObject withinArray = obj[0];
                }
            }

            obj.Dispose();
        }

        [Benchmark]
        public void ParseSystemTextJsonLabArrayMiddle()
        {
            JsonObject obj = JsonObject.Parse(_dataUtf8);

            for (int j = 0; j < IterationCount; j++)
            {
                for (int i = 0; i < obj.ArrayLength; i++)
                {
                    JsonObject withinArray = obj[obj.ArrayLength / 2];
                }
            }

            obj.Dispose();
        }

        [Benchmark]
        public void ParseSystemTextJsonLabEnumerate()
        {
            JsonObject obj = JsonObject.Parse(_dataUtf8);
            JsonObject withinArray = obj[0];

            for (int j = 0; j < IterationCount; j++)
            {
                foreach (JsonObject childObject in withinArray)
                {
                }
            }

            obj.Dispose();
        }
    }
}
