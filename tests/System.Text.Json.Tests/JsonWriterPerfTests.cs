// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;
using Microsoft.Xunit.Performance;
using System.Text.Formatting;
using System.IO;

namespace System.Text.Json.Tests
{
    public partial class JsonPerfTests
    {
        const int ExtraArraySize = 500;

        [Benchmark(InnerIterationCount = 1000)]
        [InlineData(true, EncoderTarget.InvariantUtf8)]
        [InlineData(false, EncoderTarget.InvariantUtf8)]
        [InlineData(true, EncoderTarget.InvariantUtf16)]
        [InlineData(false, EncoderTarget.InvariantUtf16)]
        [InlineData(true, EncoderTarget.SlowUtf8)]
        [InlineData(false, EncoderTarget.SlowUtf8)]
        [InlineData(true, EncoderTarget.SlowUtf16)]
        [InlineData(false, EncoderTarget.SlowUtf16)]
        public void WriterSystemTextJson(bool formatted, EncoderTarget encoderTarget)
        {
            var encoder = GetTargetEncoder(encoderTarget);
            var f = new ArrayFormatter(BufferSize, encoder);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        f.Clear();
                        TestWriterSystemTextJson(formatted, f);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = 1000)]
        [InlineData(true, EncoderTarget.InvariantUtf8)]
        [InlineData(false, EncoderTarget.InvariantUtf8)]
        [InlineData(true, EncoderTarget.InvariantUtf16)]
        [InlineData(false, EncoderTarget.InvariantUtf16)]
        public void WriterNewtonsoft(bool formatted, EncoderTarget encoderTarget)
        {
            var enc = encoderTarget == EncoderTarget.InvariantUtf8 ? Encoding.UTF8 : Encoding.Unicode;
            var buffer = new byte[BufferSize];
            var mem = new MemoryStream(buffer);
            var sw = new StreamWriter(mem, enc, BufferSize, true);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        mem.Seek(0, SeekOrigin.Begin);
                        TestWriterNewtonsoft(formatted, sw);
                    }
                }
            }
        }

        static void TestWriterSystemTextJson(bool formatted, ArrayFormatter output)
        {
            var json = new System.Text.Json.JsonWriter(output, formatted);

            json.WriteObjectStart();
            json.WriteAttribute("age", 42);
            json.WriteAttribute("first", "John");
            json.WriteAttribute("last", "Smith");
            json.WriteArrayStart("phoneNumbers");
            json.WriteValue("425-000-1212");
            json.WriteValue("425-000-1213");
            json.WriteArrayEnd();
            json.WriteObjectStart("address");
            json.WriteAttribute("street", "1 Microsoft Way");
            json.WriteAttribute("city", "Redmond");
            json.WriteAttribute("zip", 98052);
            json.WriteObjectEnd();

            // Add a large array of values
            json.WriteArrayStart("ExtraArray");
            for (var i = 0; i < ExtraArraySize; i++)
            {
                json.WriteValue(i);
            }
            json.WriteArrayEnd();

            json.WriteObjectEnd();
        }

        static void TestWriterNewtonsoft(bool formatted, StreamWriter writer)
        {
            using (var json = new Newtonsoft.Json.JsonTextWriter(writer))
            {
                json.Formatting = formatted ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;

                json.WriteStartObject();
                json.WritePropertyName("age");
                json.WriteValue(42);
                json.WritePropertyName("first");
                json.WriteValue("John");
                json.WritePropertyName("last");
                json.WriteValue("Smith");
                json.WritePropertyName("phoneNumbers");
                json.WriteStartArray();
                json.WriteValue("425-000-1212");
                json.WriteValue("425-000-1213");
                json.WriteEnd();
                json.WritePropertyName("address");
                json.WriteStartObject();
                json.WritePropertyName("street");
                json.WriteValue("1 Microsoft Way");
                json.WritePropertyName("city");
                json.WriteValue("Redmond");
                json.WritePropertyName("zip");
                json.WriteValue(98052);
                json.WriteEnd();

                // Add a large array of values
                json.WritePropertyName("ExtraArray");
                json.WriteStartArray();
                for (var i = 0; i < ExtraArraySize; i++)
                {
                    json.WriteValue(i);
                }
                json.WriteEnd();

                json.WriteEnd();
            }
        }
    }
}
