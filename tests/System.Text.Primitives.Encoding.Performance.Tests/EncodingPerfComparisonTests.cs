// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Utf8;
using Microsoft.Xunit.Performance;


namespace System.Text.Primitives.Encoding.Performance.Tests
{
    public class EncodingPerfComparisonTests
    {
        [Benchmark]
        public void EncodingPerformanceTestUsingCoreCLR()
        {
            string unicodeString = GenerateString(1000);
            ReadOnlySpan<char> characters = unicodeString.Slice();

            Text.Encoding utf8 = Text.Encoding.UTF8;

            int utf8Length = utf8.GetByteCount(unicodeString);
            byte[] utf8Buffer = new byte[utf8Length];
            Span<byte> span;

            char[] charArray = characters.ToArray();

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    utf8.GetBytes(charArray, 0, characters.Length, utf8Buffer, 0);
                span = new Span<byte>(utf8Buffer);
            }
        }

        [Benchmark]
        public void EncodingPerformanceTestUsingCorefxlab()
        {
            string unicodeString = GenerateString(1000);
            ReadOnlySpan<char> characters = unicodeString.Slice();

            int encodedBytes;

            int utf8Length = Utf8Encoder.ComputeEncodedBytes(characters);
            byte[] utf8Buffer = new byte[utf8Length];
            Span<byte> span = new Span<byte>(utf8Buffer);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    if (!Utf8Encoder.TryEncode(characters, span, out encodedBytes))
                    {
                        throw new Exception(); // this should not happen
                    }
            }
        }

        public string GenerateString(int charLength)
        {
            Random rand = new Random();
            var plainText = new StringBuilder();
            for (int j = 0; j < charLength; j++)
            {
                plainText.Append((char)rand.Next(char.MinValue, char.MaxValue));
            }
            byte[] byteText = Text.Encoding.Unicode.GetBytes(plainText.ToString());
            return Text.Encoding.Unicode.GetString(byteText);
        }
    }
}
