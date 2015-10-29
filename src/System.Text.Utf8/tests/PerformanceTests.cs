// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;
using System.Linq;
using System.Collections.Generic;
using System.Text.Utf8;
using System.Text.Utf16;
using Xunit.Abstractions;
using System.Diagnostics;
using System.Runtime.CompilerServices;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace System.Text.Utf8.Tests
{
    [Trait("category", "performance")]
    public class Utf8StringPerformanceTests
    {
        private readonly Stopwatch _timer = new Stopwatch();
        ITestOutputHelper _output;

        public Utf8StringPerformanceTests(ITestOutputHelper output)
        {
            _output = output;

            _timer.Start();
            _timer.Restart();
            _timer.Stop();
            _timer.Reset();
            PrintTime(new StringWithDescription("", "ignore this", -1));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PrintTime(StringWithDescription str, [CallerMemberName] string memberName = "")
        {
            _output.WriteLine(string.Format("{0} : {1} : Elapsed {2}ms ({3} iterations)", memberName, str.Description, _timer.ElapsedMilliseconds, str.Iterations));
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

        public struct StringWithDescription
        {
            string _s;
            string _description;
            int _iterations;

            public string String { get { return _s; } }
            public string Description { get { return _description;  } }
            public int Iterations { get { return _iterations;  } }
            public StringWithDescription(string s, string description, int iterations)
            {
                _s = s;
                _description = description;
                _iterations = iterations;
            }
        }

        private IEnumerable<StringWithDescription> StringsWithDescription()
        {
            int iterationsMultiplier = 10;
            yield return new StringWithDescription(GetRandomString(5, 32, 126), "Short ASCII string", 50000 * iterationsMultiplier);
            yield return new StringWithDescription(GetRandomString(5, 32, 0xD7FF), "Short string", 50000 * iterationsMultiplier);
            yield return new StringWithDescription(GetRandomString(50000, 32, 126), "Long ASCII string", 5 * iterationsMultiplier);
            yield return new StringWithDescription(GetRandomString(50000, 32, 0xD7FF), "Long string", 5 * iterationsMultiplier);
        }

        [Fact]
        public void ConstructFromString()
        {
            foreach (StringWithDescription testData in StringsWithDescription())
            {
                string s = testData.String;
                int iterations = testData.Iterations;
                _timer.Restart();
                while (iterations-- != 0)
                {
                    Utf8String utf8s = new Utf8String(s);
                }
                PrintTime(testData);
            }
        }

        [Fact]
        public void EnumerateCodeUnitsConstructFromByteArray()
        {
            foreach (StringWithDescription testData in StringsWithDescription())
            {
                string s = testData.String;
                Utf8String utf8s = new Utf8String(s);
                utf8s = new Utf8String(utf8s.CopyBytes());
                int iterations = testData.Iterations;
                _timer.Restart();
                while (iterations-- != 0)
                {
                    foreach (Utf8CodeUnit codeUnit in utf8s)
                    {
                    }
                }
                PrintTime(testData);
            }
        }

        [Fact]
        public unsafe void EnumerateCodeUnitsConstructFromSpan()
        {
            foreach (StringWithDescription testData in StringsWithDescription())
            {
                string s = testData.String;
                Utf8String utf8s = new Utf8String(s);
                fixed (byte* bytes = utf8s.CopyBytes())
                {
                    utf8s = new Utf8String(new Span<byte>(bytes, utf8s.Length));
                    int iterations = testData.Iterations;
                    _timer.Restart();
                    while (iterations-- != 0)
                    {
                        foreach (Utf8CodeUnit codeUnit in utf8s)
                        {
                        }
                    }
                    PrintTime(testData);
                }
            }
        }

        [Fact]
        public void EnumerateCodePointsConstructFromByteArray()
        {
            foreach (StringWithDescription testData in StringsWithDescription())
            {
                string s = testData.String;
                Utf8String utf8s = new Utf8String(s);
                utf8s = new Utf8String(utf8s.CopyBytes());
                int iterations = testData.Iterations;
                _timer.Restart();
                while (iterations-- != 0)
                {
                    foreach (UnicodeCodePoint codePoint in utf8s.CodePoints)
                    {
                    }
                }
                PrintTime(testData);
            }
        }

        [Fact]
        public unsafe void EnumerateCodePointsConstructFromSpan()
        {
            foreach (StringWithDescription testData in StringsWithDescription())
            {
                string s = testData.String;
                Utf8String utf8s = new Utf8String(s);
                fixed (byte* bytes = utf8s.CopyBytes())
                {
                    utf8s = new Utf8String(new Span<byte>(bytes, utf8s.Length));
                    int iterations = testData.Iterations;
                    _timer.Restart();
                    while (iterations-- != 0)
                    {
                        foreach (UnicodeCodePoint codePoint in utf8s.CodePoints)
                        {
                        }
                    }
                    PrintTime(testData);
                }
            }
        }

        [Fact]
        public void ReverseEnumerateCodePointsConstructFromByteArray()
        {
            foreach (StringWithDescription testData in StringsWithDescription())
            {
                string s = testData.String;
                Utf8String utf8s = new Utf8String(s);
                utf8s = new Utf8String(utf8s.CopyBytes());
                int iterations = testData.Iterations;
                _timer.Restart();
                while (iterations-- != 0)
                {
                    Utf8String.CodePointReverseEnumerator it = utf8s.CodePoints.GetReverseEnumerator();
                    while (it.MoveNext())
                    {
                        UnicodeCodePoint codePoint = it.Current;
                    }
                }
                PrintTime(testData);
            }
        }

        [Fact]
        public unsafe void ReverseEnumerateCodePointsConstructFromSpan()
        {
            foreach (StringWithDescription testData in StringsWithDescription())
            {
                string s = testData.String;
                Utf8String utf8s = new Utf8String(s);
                fixed (byte* bytes = utf8s.CopyBytes())
                {
                    utf8s = new Utf8String(new Span<byte>(bytes, utf8s.Length));
                    int iterations = testData.Iterations;
                    _timer.Restart();
                    while (iterations-- != 0)
                    {
                        Utf8String.CodePointReverseEnumerator it = utf8s.CodePoints.GetReverseEnumerator();
                        while (it.MoveNext())
                        {
                            UnicodeCodePoint codePoint = it.Current;
                        }
                    }
                    PrintTime(testData);
                }
            }
        }

        [Fact]
        public void SubstringTrimOneCharacterOnEachSideConstructFromByteArray()
        {
            foreach (StringWithDescription testData in StringsWithDescription())
            {
                string s = testData.String;
                Utf8String utf8s = new Utf8String(s);
                utf8s = new Utf8String(utf8s.CopyBytes());
                int iterations = testData.Iterations;
                _timer.Restart();
                while (iterations-- != 0)
                {
                    Utf8String result = utf8s.Substring(1, utf8s.Length - 2);
                }
                PrintTime(testData);
            }
        }

        [Fact]
        public unsafe void SubstringTrimOneCharacterOnEachSideConstructFromSpan()
        {
            foreach (StringWithDescription testData in StringsWithDescription())
            {
                string s = testData.String;
                Utf8String utf8s = new Utf8String(s);
                fixed (byte* bytes = utf8s.CopyBytes())
                {
                    utf8s = new Utf8String(new Span<byte>(bytes, utf8s.Length));
                    int iterations = testData.Iterations;
                    _timer.Restart();
                    while (iterations-- != 0)
                    {
                        Utf8String result = utf8s.Substring(1, utf8s.Length - 2);
                    }
                    PrintTime(testData);
                }
            }
        }

        [Fact]
        public void IndexOfNonOccuringSingleCodeUnitConstructFromByteArray()
        {
            foreach (StringWithDescription testData in StringsWithDescription())
            {
                string s = testData.String;
                Utf8String utf8s = new Utf8String(s);
                utf8s = new Utf8String(utf8s.CopyBytes());
                int iterations = testData.Iterations;
                _timer.Restart();
                while (iterations-- != 0)
                {
                    int p = utf8s.IndexOf((Utf8CodeUnit)31);
                }
                PrintTime(testData);
            }
        }

        [Fact]
        public unsafe void IndexOfNonOccuringSingleCodeUnitConstructFromSpan()
        {
            foreach (StringWithDescription testData in StringsWithDescription())
            {
                string s = testData.String;
                Utf8String utf8s = new Utf8String(s);
                fixed (byte* bytes = utf8s.CopyBytes())
                {
                    utf8s = new Utf8String(new Span<byte>(bytes, utf8s.Length));
                    int iterations = testData.Iterations;
                    _timer.Restart();
                    while (iterations-- != 0)
                    {
                        int p = utf8s.IndexOf((Utf8CodeUnit)31);
                    }
                    PrintTime(testData);
                }
            }
        }

        [Fact]
        public void IndexOfNonOccuringSingleCodePointConstructFromByteArray()
        {
            foreach (StringWithDescription testData in StringsWithDescription())
            {
                string s = testData.String;
                Utf8String utf8s = new Utf8String(s);
                utf8s = new Utf8String(utf8s.CopyBytes());
                int iterations = testData.Iterations;
                _timer.Restart();
                while (iterations-- != 0)
                {
                    int p = utf8s.IndexOf((UnicodeCodePoint)31);
                }
                PrintTime(testData);
            }
        }

        [Fact]
        public unsafe void IndexOfNonOccuringSingleCodePointConstructFromSpan()
        {
            foreach (StringWithDescription testData in StringsWithDescription())
            {
                string s = testData.String;
                Utf8String utf8s = new Utf8String(s);
                fixed (byte* bytes = utf8s.CopyBytes())
                {
                    utf8s = new Utf8String(new Span<byte>(bytes, utf8s.Length));
                    int iterations = testData.Iterations;
                    _timer.Restart();
                    while (iterations-- != 0)
                    {
                        int p = utf8s.IndexOf((UnicodeCodePoint)31);
                    }
                    PrintTime(testData);
                }
            }
        }
    }
}
