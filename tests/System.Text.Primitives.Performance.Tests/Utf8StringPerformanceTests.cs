// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;
using Microsoft.Xunit.Performance;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace System.Text.Utf8.Tests
{
    public class Utf8StringPerformanceTests
    {
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

        [Benchmark]
        [InlineData(5, 32, 126, "Short ASCII string")]
        [InlineData(5, 32, 0xD7FF, "Short string")]
        [InlineData(50000, 32, 126, "Long ASCII string")]
        [InlineData(50000, 32, 0xD7FF, "Long string")]
        public void ConstructFromString(int length, int minCodePoint, int maxCodePoint, string description)
        {
            string s = GetRandomString(length, minCodePoint, maxCodePoint);
            Utf8String utf8s;
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    utf8s = new Utf8String(s);
            }
        }

        [Benchmark]
        [InlineData(5, 32, 126, "Short ASCII string")]
        [InlineData(5, 32, 0xD7FF, "Short string")]
        [InlineData(50000, 32, 126, "Long ASCII string")]
        [InlineData(50000, 32, 0xD7FF, "Long string")]
        public void EnumerateCodeUnitsConstructFromByteArray(int length, int minCodePoint, int maxCodePoint, string description)
        {
            string s = GetRandomString(length, minCodePoint, maxCodePoint);
            Utf8String utf8s = new Utf8String(s);
            utf8s = new Utf8String(utf8s.CopyBytes());

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    foreach (byte codeUnit in utf8s)
                    {
                    }
            }
        }

        [Benchmark]
        [InlineData(5, 32, 126, "Short ASCII string")]
        [InlineData(5, 32, 0xD7FF, "Short string")]
        [InlineData(50000, 32, 126, "Long ASCII string")]
        [InlineData(50000, 32, 0xD7FF, "Long string")]
        public unsafe void EnumerateCodeUnitsConstructFromSpan(int length, int minCodePoint, int maxCodePoint, string description)
        {
            string s = GetRandomString(length, minCodePoint, maxCodePoint);
            Utf8String utf8s = new Utf8String(s);
            fixed (byte* bytes = utf8s.CopyBytes())
            {
                utf8s = new Utf8String(new Span<byte>(bytes, utf8s.Length));

                foreach (var iteration in Benchmark.Iterations)
                {
                    using (iteration.StartMeasurement())
                        foreach (byte codeUnit in utf8s)
                        {
                        }
                }
            }
        }

        [Benchmark]
        [InlineData(5, 32, 126, "Short ASCII string")]
        [InlineData(5, 32, 0xD7FF, "Short string")]
        [InlineData(50000, 32, 126, "Long ASCII string")]
        [InlineData(50000, 32, 0xD7FF, "Long string")]
        public void EnumerateCodePointsConstructFromByteArray(int length, int minCodePoint, int maxCodePoint, string description)
        {
            string s = GetRandomString(length, minCodePoint, maxCodePoint);
            Utf8String utf8s = new Utf8String(s);
            utf8s = new Utf8String(utf8s.CopyBytes());
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    foreach (var codePoint in utf8s.CodePoints)
                    {
                    }
            }
        }

        [Benchmark]
        [InlineData(5, 32, 126, "Short ASCII string")]
        [InlineData(5, 32, 0xD7FF, "Short string")]
        [InlineData(50000, 32, 126, "Long ASCII string")]
        [InlineData(50000, 32, 0xD7FF, "Long string")]
        public unsafe void EnumerateCodePointsConstructFromSpan(int length, int minCodePoint, int maxCodePoint, string description)
        {
            string s = GetRandomString(length, minCodePoint, maxCodePoint);
            Utf8String utf8s = new Utf8String(s);
            fixed (byte* bytes = utf8s.CopyBytes())
            {
                utf8s = new Utf8String(new Span<byte>(bytes, utf8s.Length));
                foreach (var iteration in Benchmark.Iterations)
                {
                    using (iteration.StartMeasurement())
                        foreach (var codePoint in utf8s.CodePoints)
                        {
                        }
                }
            }
        }

        [Benchmark]
        [InlineData(5, 32, 126, "Short ASCII string")]
        [InlineData(5, 32, 0xD7FF, "Short string")]
        [InlineData(50000, 32, 126, "Long ASCII string")]
        [InlineData(50000, 32, 0xD7FF, "Long string")]
        public void ReverseEnumerateCodePointsConstructFromByteArray(int length, int minCodePoint, int maxCodePoint, string description)
        {
            string s = GetRandomString(length, minCodePoint, maxCodePoint);

            Utf8String utf8s = new Utf8String(s);
            utf8s = new Utf8String(utf8s.CopyBytes());

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    Utf8String.CodePointReverseEnumerator it = utf8s.CodePoints.GetReverseEnumerator();
                    while (it.MoveNext())
                    {
                        var codePoint = it.Current;
                    }
                }
            }
        }

        [Benchmark]
        [InlineData(5, 32, 126, "Short ASCII string")]
        [InlineData(5, 32, 0xD7FF, "Short string")]
        [InlineData(50000, 32, 126, "Long ASCII string")]
        [InlineData(50000, 32, 0xD7FF, "Long string")]
        public unsafe void ReverseEnumerateCodePointsConstructFromSpan(int length, int minCodePoint, int maxCodePoint, string description)
        {
            string s = GetRandomString(length, minCodePoint, maxCodePoint);
            Utf8String utf8s = new Utf8String(s);
            fixed (byte* bytes = utf8s.CopyBytes())
            {
                utf8s = new Utf8String(new Span<byte>(bytes, utf8s.Length));

                foreach (var iteration in Benchmark.Iterations)
                {
                    using (iteration.StartMeasurement())
                    {
                        Utf8String.CodePointReverseEnumerator it = utf8s.CodePoints.GetReverseEnumerator();
                        while (it.MoveNext())
                        {
                            var codePoint = it.Current;
                        }
                    }
                }
            }
        }

        [Benchmark]
        [InlineData(5, 32, 126, "Short ASCII string")]
        [InlineData(5, 32, 0xD7FF, "Short string")]
        [InlineData(50000, 32, 126, "Long ASCII string")]
        [InlineData(50000, 32, 0xD7FF, "Long string")]
        public void SubstringTrimOneCharacterOnEachSideConstructFromByteArray(int length, int minCodePoint, int maxCodePoint, string description)
        {
            string s = GetRandomString(length, minCodePoint, maxCodePoint);
            Utf8String utf8s = new Utf8String(s);
            utf8s = new Utf8String(utf8s.CopyBytes());
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    Utf8String result = utf8s.Substring(1, utf8s.Length - 2);
                }
            }
        }

        [Benchmark]
        [InlineData(5, 32, 126, "Short ASCII string")]
        [InlineData(5, 32, 0xD7FF, "Short string")]
        [InlineData(50000, 32, 126, "Long ASCII string")]
        [InlineData(50000, 32, 0xD7FF, "Long string")]
        public unsafe void SubstringTrimOneCharacterOnEachSideConstructFromSpan(int length, int minCodePoint, int maxCodePoint, string description)
        {
            string s = GetRandomString(length, minCodePoint, maxCodePoint);
            Utf8String utf8s = new Utf8String(s);
            fixed (byte* bytes = utf8s.CopyBytes())
            {
                utf8s = new Utf8String(new Span<byte>(bytes, utf8s.Length));

                foreach (var iteration in Benchmark.Iterations)
                {
                    using (iteration.StartMeasurement())
                    {
                        Utf8String result = utf8s.Substring(1, utf8s.Length - 2);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData(5, 32, 126, "Short ASCII string")]
        [InlineData(5, 32, 0xD7FF, "Short string")]
        [InlineData(50000, 32, 126, "Long ASCII string")]
        [InlineData(50000, 32, 0xD7FF, "Long string")]
        public void IndexOfNonOccuringSingleCodeUnitConstructFromByteArray(int length, int minCodePoint, int maxCodePoint, string description)
        {
            string s = GetRandomString(length, minCodePoint, maxCodePoint);
            Utf8String utf8s = new Utf8String(s);
            utf8s = new Utf8String(utf8s.CopyBytes());

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    int p = utf8s.IndexOf((byte)31);
                }
            }
        }

        [Benchmark]
        [InlineData(5, 32, 126, "Short ASCII string")]
        [InlineData(5, 32, 0xD7FF, "Short string")]
        [InlineData(50000, 32, 126, "Long ASCII string")]
        [InlineData(50000, 32, 0xD7FF, "Long string")]
        public unsafe void IndexOfNonOccuringSingleCodeUnitConstructFromSpan(int length, int minCodePoint, int maxCodePoint, string description)
        {
            string s = GetRandomString(length, minCodePoint, maxCodePoint);
            Utf8String utf8s = new Utf8String(s);
            fixed (byte* bytes = utf8s.CopyBytes())
            {
                utf8s = new Utf8String(new Span<byte>(bytes, utf8s.Length));

                foreach (var iteration in Benchmark.Iterations)
                {
                    using (iteration.StartMeasurement())
                    {
                        int p = utf8s.IndexOf((byte)31);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData(5, 32, 126, "Short ASCII string")]
        [InlineData(5, 32, 0xD7FF, "Short string")]
        [InlineData(50000, 32, 126, "Long ASCII string")]
        [InlineData(50000, 32, 0xD7FF, "Long string")]
        public void IndexOfNonOccuringSingleCodePointConstructFromByteArray(int length, int minCodePoint, int maxCodePoint, string description)
        {
            string s = GetRandomString(length, minCodePoint, maxCodePoint);
            Utf8String utf8s = new Utf8String(s);
            utf8s = new Utf8String(utf8s.CopyBytes());

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    int p = utf8s.IndexOf(31);
                }
            }
        }

        [Benchmark]
        [InlineData(5, 32, 126, "Short ASCII string")]
        [InlineData(5, 32, 0xD7FF, "Short string")]
        [InlineData(50000, 32, 126, "Long ASCII string")]
        [InlineData(50000, 32, 0xD7FF, "Long string")]
        public unsafe void IndexOfNonOccuringSingleCodePointConstructFromSpan(int length, int minCodePoint, int maxCodePoint, string description)
        {
            string s = GetRandomString(length, minCodePoint, maxCodePoint);
            Utf8String utf8s = new Utf8String(s);
            fixed (byte* bytes = utf8s.CopyBytes())
            {
                utf8s = new Utf8String(new Span<byte>(bytes, utf8s.Length));

                foreach (var iteration in Benchmark.Iterations)
                {
                    using (iteration.StartMeasurement())
                    {
                        int p = utf8s.IndexOf(31);
                    }
                }
            }
        }
    }
}
