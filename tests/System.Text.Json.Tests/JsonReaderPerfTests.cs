// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;
using Microsoft.Xunit.Performance;
using System.Text.Formatting;
using System.IO;
using System.Text.Json.Tests.Resources;

namespace System.Text.Json.Tests
{
    partial class JsonPerfTests
    {
        [Benchmark(InnerIterationCount = 1000)]
        [InlineData(EncoderTarget.InvariantUtf8)]
        [InlineData(EncoderTarget.InvariantUtf16)]
        public void ReaderSystemTextJson(EncoderTarget encoderTarget)
        {
            var encoder = GetTargetEncoder(encoderTarget);
            var data = LoadTestData(encoderTarget);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        TestReaderSystemTextJson(data, encoder);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = 1000)]
        [InlineData(EncoderTarget.InvariantUtf8)]
        [InlineData(EncoderTarget.InvariantUtf16)]
        public void ReaderNewtonsoft(EncoderTarget encoderTarget)
        {
            var enc = encoderTarget == EncoderTarget.InvariantUtf8 ? Encoding.UTF8 : Encoding.Unicode;
            var data = LoadTestData(encoderTarget);
            var mem = new MemoryStream(data);
            var reader = new StreamReader(mem, enc, false, 1024, true);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        mem.Seek(0, SeekOrigin.Begin);
                        TestReaderNewtonsoft(reader);
                    }
                }
            }
        }

        static void TestReaderSystemTextJson(ReadOnlySpan<byte> data, TextEncoder encoder)
        {
            var json = new System.Text.Json.JsonReader(data, encoder);
            while (json.Read());
        }

        static void TestReaderNewtonsoft(StreamReader reader)
        {
            using (var json = new Newtonsoft.Json.JsonTextReader(reader))
                while (json.Read());
        }

        static byte[] LoadTestData(EncoderTarget encoderTarget)
        {
            if (encoderTarget == EncoderTarget.InvariantUtf16 || encoderTarget == EncoderTarget.SlowUtf16)
                return Encoding.Unicode.GetBytes(TestJson.HeavyNestedJson);
            else
                return Encoding.UTF8.GetBytes(TestJson.HeavyNestedJson);
        }
    }
}
