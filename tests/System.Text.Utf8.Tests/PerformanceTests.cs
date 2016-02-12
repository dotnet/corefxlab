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
    [Trait("category", "outerloop")]
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
            PrintTime(new TestCase("", "ignore this", -1));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PrintTime(TestCase str, [CallerMemberName] string memberName = "")
        {
            _timer.Stop();
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

        public struct TestCase
        {
            string _s;
            string _description;
            int _iterations;

            public string String { get { return _s; } }
            public string Description { get { return _description;  } }
            public int Iterations { get { return _iterations;  } }
            public TestCase(string s, string description, int iterations)
            {
                _s = s;
                _description = description;
                _iterations = iterations;
            }
        }

        [Fact]
        public void ConstructFromString()
        {
            TestCase[] testCases = new TestCase[] {
                new TestCase(GetRandomString(5, 32, 126), "Short ASCII string", 6000000),
                new TestCase(GetRandomString(5, 32, 0xD7FF), "Short string", 6000000),
                new TestCase(GetRandomString(50000, 32, 126), "Long ASCII string", 600),
                new TestCase(GetRandomString(50000, 32, 0xD7FF), "Long string", 600)
            };
            foreach (TestCase testData in testCases)
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
            TestCase[] testCases = new TestCase[] {
                new TestCase(GetRandomString(5, 32, 126), "Short ASCII string", 30000000),
                new TestCase(GetRandomString(5, 32, 0xD7FF), "Short string", 30000000),
                new TestCase(GetRandomString(50000, 32, 126), "Long ASCII string", 10000),
                new TestCase(GetRandomString(50000, 32, 0xD7FF), "Long string", 3000)
            };
            foreach (TestCase testData in testCases)
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
            TestCase[] testCases = new TestCase[] {
                new TestCase(GetRandomString(5, 32, 126), "Short ASCII string", 30000000),
                new TestCase(GetRandomString(5, 32, 0xD7FF), "Short string", 30000000),
                new TestCase(GetRandomString(50000, 32, 126), "Long ASCII string", 10000),
                new TestCase(GetRandomString(50000, 32, 0xD7FF), "Long string", 3000)
            };
            foreach (TestCase testData in testCases)
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
            TestCase[] testCases = new TestCase[] {
                new TestCase(GetRandomString(5, 32, 126), "Short ASCII string", 5000000),
                new TestCase(GetRandomString(5, 32, 0xD7FF), "Short string", 5000000),
                new TestCase(GetRandomString(50000, 32, 126), "Long ASCII string", 500),
                new TestCase(GetRandomString(50000, 32, 0xD7FF), "Long string", 500)
            };
            foreach (TestCase testData in testCases)
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
            TestCase[] testCases = new TestCase[] {
                new TestCase(GetRandomString(5, 32, 126), "Short ASCII string", 5000000),
                new TestCase(GetRandomString(5, 32, 0xD7FF), "Short string", 5000000),
                new TestCase(GetRandomString(50000, 32, 126), "Long ASCII string", 500),
                new TestCase(GetRandomString(50000, 32, 0xD7FF), "Long string", 500)
            };
            foreach (TestCase testData in testCases)
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
            TestCase[] testCases = new TestCase[] {
                new TestCase(GetRandomString(5, 32, 126), "Short ASCII string", 3000000),
                new TestCase(GetRandomString(5, 32, 0xD7FF), "Short string", 3000000),
                new TestCase(GetRandomString(50000, 32, 126), "Long ASCII string", 300),
                new TestCase(GetRandomString(50000, 32, 0xD7FF), "Long string", 300)
            };
            foreach (TestCase testData in testCases)
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
            TestCase[] testCases = new TestCase[] {
                new TestCase(GetRandomString(5, 32, 126), "Short ASCII string", 3000000),
                new TestCase(GetRandomString(5, 32, 0xD7FF), "Short string", 3000000),
                new TestCase(GetRandomString(50000, 32, 126), "Long ASCII string", 300),
                new TestCase(GetRandomString(50000, 32, 0xD7FF), "Long string", 300)
            };
            foreach (TestCase testData in testCases)
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
            TestCase[] testCases = new TestCase[] {
                new TestCase(GetRandomString(5, 32, 126), "Short ASCII string", 50000000),
                new TestCase(GetRandomString(5, 32, 0xD7FF), "Short string", 50000000),
                new TestCase(GetRandomString(50000, 32, 126), "Long ASCII string", 50000000),
                new TestCase(GetRandomString(50000, 32, 0xD7FF), "Long string", 50000000)
            };
            foreach (TestCase testData in testCases)
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
            TestCase[] testCases = new TestCase[] {
                new TestCase(GetRandomString(5, 32, 126), "Short ASCII string", 50000000),
                new TestCase(GetRandomString(5, 32, 0xD7FF), "Short string", 50000000),
                new TestCase(GetRandomString(50000, 32, 126), "Long ASCII string", 50000000),
                new TestCase(GetRandomString(50000, 32, 0xD7FF), "Long string", 50000000)
            };
            foreach (TestCase testData in testCases)
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
            TestCase[] testCases = new TestCase[] {
                new TestCase(GetRandomString(5, 32, 126), "Short ASCII string", 30000000),
                new TestCase(GetRandomString(5, 32, 0xD7FF), "Short string", 30000000),
                new TestCase(GetRandomString(50000, 32, 126), "Long ASCII string", 3000),
                new TestCase(GetRandomString(50000, 32, 0xD7FF), "Long string", 3000)
            };
            foreach (TestCase testData in testCases)
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
            TestCase[] testCases = new TestCase[] {
                new TestCase(GetRandomString(5, 32, 126), "Short ASCII string", 30000000),
                new TestCase(GetRandomString(5, 32, 0xD7FF), "Short string", 30000000),
                new TestCase(GetRandomString(50000, 32, 126), "Long ASCII string", 3000),
                new TestCase(GetRandomString(50000, 32, 0xD7FF), "Long string", 3000)
            };
            foreach (TestCase testData in testCases)
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
            TestCase[] testCases = new TestCase[] {
                new TestCase(GetRandomString(5, 32, 126), "Short ASCII string", 5000000),
                new TestCase(GetRandomString(5, 32, 0xD7FF), "Short string", 5000000),
                new TestCase(GetRandomString(50000, 32, 126), "Long ASCII string", 500),
                new TestCase(GetRandomString(50000, 32, 0xD7FF), "Long string", 500)
            };
            foreach (TestCase testData in testCases)
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
            TestCase[] testCases = new TestCase[] {
                new TestCase(GetRandomString(5, 32, 126), "Short ASCII string", 5000000),
                new TestCase(GetRandomString(5, 32, 0xD7FF), "Short string", 5000000),
                new TestCase(GetRandomString(50000, 32, 126), "Long ASCII string", 500),
                new TestCase(GetRandomString(50000, 32, 0xD7FF), "Long string", 500)
            };
            foreach (TestCase testData in testCases)
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
