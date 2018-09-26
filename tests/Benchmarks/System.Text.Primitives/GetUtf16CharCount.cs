// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;

namespace System.Text.Primitives.Benchmarks
{
    public class GetUtf16CharCount
    {
        private static readonly UTF8Encoding _nonReplacingEncoder = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
        private static readonly byte[] _utf8BOM = new byte[] { 0xEF, 0xBB, 0xBF };

        [ParamsSource(nameof(FileNames))]
        public string FileName;

        static byte[] utf8Text;

        [GlobalSetup]
        public void Setup() => utf8Text = ReadTestResource(FileName).ToArray();

        [Benchmark(Baseline = true)]
        public int UsingInboxEncoder()
        {
            return _nonReplacingEncoder.GetCharCount(utf8Text);
        }

        [Benchmark()]
        public bool UsingNewEncoder()
        {
            return Utf8Utility.TryGetUtf16CharCount(utf8Text, out _);
        }

        public IEnumerable<string> FileNames()
        {
            yield return "11.txt"; // English, ASCII
            yield return "11-0.txt"; // English, UTF-8 (has scattered non-ASCII chars such as curly quotes)
            yield return "30774-0.txt"; // Russian, UTF-8 (primarily 2-byte sequences)
            yield return "39251-0.txt"; // Greek, UTF-8 (primarily 2-byte sequences)
            yield return "25249-0.txt"; // Chinese, UTF-8 (primarily 3-byte sequences)
        }

        private static ReadOnlySpan<byte> ReadTestResource(string resourceName)
        {
            // Copy desired text to an in-memory buffer so that we can create a Span around it

            MemoryStream ms = new MemoryStream();
            typeof(GetUtf16CharCount).Assembly.GetManifestResourceStream("Benchmarks.System.Text.Primitives.SampleTexts." + resourceName).CopyTo(ms);
            ReadOnlySpan<byte> utf8Text = new ReadOnlySpan<byte>(ms.GetBuffer(), 0, checked((int)ms.Length));

            // The sample texts may begin with a BOM (since they were downloaded verbatim).
            // Strip the BOM now if present.

            return (utf8Text.StartsWith(_utf8BOM)) ? utf8Text.Slice(_utf8BOM.Length) : utf8Text;
        }
    }
}
