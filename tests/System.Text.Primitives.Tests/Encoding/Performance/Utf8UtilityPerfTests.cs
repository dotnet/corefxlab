// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers.Text;
using System.IO;
using Microsoft.Xunit.Performance;
using Xunit;

namespace System.Text.Primitives.Tests
{
    public class Utf8UtilityPerfTests
    {
        private const int InnerCount = 10000;

        private static readonly UTF8Encoding _nonReplacingEncoder = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
        private static readonly byte[] _utf8BOM = new byte[] { 0xEF, 0xBB, 0xBF };

        [Benchmark(InnerIterationCount = InnerCount)]
        [InlineData("11.txt")] // English, ASCII
        [InlineData("11-0.txt")] // English, UTF-8 (has scattered non-ASCII chars such as curly quotes)
        [InlineData("30774-0.txt")] // Russian, UTF-8 (primarily 2-byte sequences)
        [InlineData("39251-0.txt")] // Greek, UTF-8 (primarily 2-byte sequences)
        [InlineData("25249-0.txt")] // Chinese, UTF-8 (primarily 3-byte sequences)
        public void GetUtf16CharCount_UsingInboxEncoder(string resourceName)
        {
            ReadOnlySpan<byte> utf8Text = ReadTestResource(resourceName);

            // Call UTF8Encoding.GetCharCount once to ensure it's JITted

            _nonReplacingEncoder.GetCharCount(utf8Text);

            // Perform perf test

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        _nonReplacingEncoder.GetCharCount(utf8Text);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        [InlineData("11.txt")] // English, ASCII
        [InlineData("11-0.txt")] // English, UTF-8 (has scattered non-ASCII chars such as curly quotes)
        [InlineData("30774-0.txt")] // Russian, UTF-8 (primarily 2-byte sequences)
        [InlineData("39251-0.txt")] // Greek, UTF-8 (primarily 2-byte sequences)
        [InlineData("25249-0.txt")] // Chinese, UTF-8 (primarily 3-byte sequences)
        public void GetUtf16CharCount_UsingNewEncoder(string resourceName)
        {
            ReadOnlySpan<byte> utf8Text = ReadTestResource(resourceName);

            // Call UTF8Encoding.GetCharCount once to ensure it's JITted

            bool succeeded = Utf8Utility.TryGetUtf16CharCount(utf8Text, out _);
            Assert.True(succeeded);

            // Perform perf test

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        Utf8Utility.TryGetUtf16CharCount(utf8Text, out _);
                    }
                }
            }
        }

        private static ReadOnlySpan<byte> ReadTestResource(string resourceName)
        {
            // Copy desired text to an in-memory buffer so that we can create a Span around it

            MemoryStream ms = new MemoryStream();
            typeof(Utf8UtilityPerfTests).Assembly.GetManifestResourceStream("System.Text.Primitives.Tests.SampleTexts." + resourceName).CopyTo(ms);
            ReadOnlySpan<byte> utf8Text = new ReadOnlySpan<byte>(ms.GetBuffer(), 0, checked((int)ms.Length));

            // The sample texts may begin with a BOM (since they were downloaded verbatim).
            // Strip the BOM now if present.

            return (utf8Text.StartsWith(_utf8BOM)) ? utf8Text.Slice(_utf8BOM.Length) : utf8Text;
        }
    }
}
