// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using System.Linq;

namespace System.Text.Encodings.Web.Utf8.Benchmarks
{
    public class UrlEncoderPerf
    {
        private byte[] _encodedBytes;

        [GlobalSetup]
        public void Setup()
        {
            var random = new Random(42);
            var builder = new StringBuilder();
            for (int i = 0; i < 10_000; i++)
            {
                if (random.NextDouble() < 0.15)
                {
                    builder.Append('%');
                    builder.Append((char) random.Next('0', '9' + 1));
                    builder.Append((char) random.Next('a', 'f' + 1));
                }
                else
                {
                    builder.Append((char) random.Next('a', 'z' + 1));
                }
            }

            _encodedBytes = Encoding.UTF8.GetBytes(builder.ToString()).ToArray();
        }

        [Benchmark]
        public void DecodeInPlace() => UrlDecoder.Utf8.DecodeInPlace(_encodedBytes, _encodedBytes.Length, out _);
    }
}
