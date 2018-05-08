// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;

namespace System.Text.Utf8.Benchmarks
{
    public class Utf8StringPerf
    {
        [Params(5, 50000)]
        public int Length;

        [Params(126, 0xD7FF)]
        public int MaxCodePoint;

        private string randomString;
        private Utf8String utf8String;

        [GlobalSetup]
        public void Setup()
        {
            // the length of the string is same across the runs, so the content of the string can be random for this particular benchmarks
            randomString = GetRandomString(Length, 32, MaxCodePoint);
            utf8String = new Utf8String(randomString);
        }

        [Benchmark]
        public Utf8String ConstructFromString() => new Utf8String(randomString);

        [Benchmark]
        public void EnumerateCodePoints()
        {
            foreach (uint codePoint in utf8String)
            {
            }
        }

        private string GetRandomString(int length, int minCodePoint, int maxCodePoint)
        {
            Random r = new Random(42);
            StringBuilder sb = new StringBuilder(length);
            while (length-- != 0)
            {
                sb.Append((char)r.Next(minCodePoint, maxCodePoint));
            }
            return sb.ToString();
        }
    }
}
