// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;

namespace System.Text.JsonLab.Benchmarks
{
    public class ValidateStringThresholdPerf
    {
        private byte[] _dataUtf8;

        [Params(0, 1, 2, 4, 8, 16, 32, 64, 128)]
        public int StringLength;

        [GlobalSetup]
        public void Setup()
        {
            var builder = new StringBuilder();
            builder.Append("\"");
            for (int i = 0; i < StringLength; i++)
            {
                builder.Append("a");
            }
            builder.Append("\"");
            string valueString = builder.ToString();

            builder = new StringBuilder();
            builder.Append("[");
            for (int i = 0; i < 100; i++)
            {
                builder.Append(valueString).Append(",");
            }
            builder.Append(valueString).Append("]");
            string jsonString = builder.ToString();
            _dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
        }

        [Benchmark]
        public void ValidateReaderIndexOf()
        {
            var json = new JsonUtf8Reader(_dataUtf8);
            while (json.Read()) ;
        }
    }
}
