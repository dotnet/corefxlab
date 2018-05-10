// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;

namespace System.Text.Utf8.Benchmarks
{
    public class Utf8StringPerf
    {
        [Params(TestCaseType.ShortAscii, TestCaseType.ShortMultiByte, TestCaseType.LongAscii, TestCaseType.LongMultiByte)]
        public TestCaseType TestCase;

        private string _string;
        private Utf8String _utf8String;

        [GlobalSetup]
        public void Setup()
        {
            int length;
            int maxCodePoint;

            switch (TestCase)
            {
                case TestCaseType.ShortAscii:
                    { length = 5; maxCodePoint = 126; break; }
                case TestCaseType.ShortMultiByte:
                    { length = 5; maxCodePoint = 0xD7FF; break; }
                case TestCaseType.LongAscii:
                    { length = 50000; maxCodePoint = 126; break; }
                case TestCaseType.LongMultiByte:
                    { length = 50000; maxCodePoint = 0xD7FF; break; }
                default:
                    throw new InvalidOperationException();
            }

            // the length of the string is same across the runs, so the content of the string can be random for this particular benchmarks
            _string = GetRandomString(length, 32, maxCodePoint);
            _utf8String = new Utf8String(_string);
        }

        [Benchmark]
        public Utf8String ConstructFromString() => new Utf8String(_string);

        [Benchmark]
        public uint EnumerateCodePoints()
        {
            uint retVal = default;
            foreach (uint codePoint in _utf8String)
            {
                retVal = codePoint;
            }
            return retVal;
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

        public enum TestCaseType
        {
            ShortAscii,
            ShortMultiByte,
            LongAscii,
            LongMultiByte
        }
    }
}
