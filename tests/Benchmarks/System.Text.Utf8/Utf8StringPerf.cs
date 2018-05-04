using System;
using System.Text;
using System.Text.Utf8;
using BenchmarkDotNet.Attributes;

namespace Benchmarks.System.Text.Utf8
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
            foreach (var codePoint in utf8String)
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
