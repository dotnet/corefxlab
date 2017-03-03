// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;
using Microsoft.Xunit.Performance;
using System;
using System.Text;

public class AsciiDecodingBench
{
    [Benchmark(InnerIterationCount = 1000000)]
    [InlineData("/plaintext")]
    [InlineData("text/plain,text/html;q=0.9,application/xhtml+xml;q=0.9,application/xml;q=0.8,*/*;q=0.7")]
    public static int AsciiToStringPrimitives(string text)
    {
        string str;
        int len = 0;
        var bytes = (ReadOnlySpan<byte>)Encoding.ASCII.GetBytes(text);
        foreach (var iteration in Benchmark.Iterations) {
            using (iteration.StartMeasurement()) {
                for (int i = 0; i < Benchmark.InnerIterationCount; i++) {
                    str = bytes.DecodeAscii();
                    len += str.Length;
                }
            }
        }
        return len;
    }

    [Benchmark(InnerIterationCount = 1000000)]
    [InlineData("/plaintext")]
    [InlineData("text/plain,text/html;q=0.9,application/xhtml+xml;q=0.9,application/xml;q=0.8,*/*;q=0.7")]
    public static int AsciiToStringClr(string text)
    {
        string str;
        int len = 0;
        var bytes = Encoding.ASCII.GetBytes(text);
        foreach (var iteration in Benchmark.Iterations) {
            using (iteration.StartMeasurement()) {
                for (int i = 0; i < Benchmark.InnerIterationCount; i++) {
                    str = Encoding.ASCII.GetString(bytes);
                    len += str.Length;
                }
            }
        }
        return len;
    }

    [Benchmark(InnerIterationCount = 1000000)]
    [InlineData("/plaintext")]
    [InlineData("text/plain,text/html;q=0.9,application/xhtml+xml;q=0.9,application/xml;q=0.8,*/*;q=0.7")]
    public static int Utf8ToStringTextEncoder(string text)
    {
        string str;
        int len = 0;
        var bytes = (ReadOnlySpan<byte>)Encoding.ASCII.GetBytes(text);
        foreach (var iteration in Benchmark.Iterations) {
            using (iteration.StartMeasurement()) {
                for (int i = 0; i < Benchmark.InnerIterationCount; i++) {
                    if (!TextEncoder.Utf8.TryDecode(bytes, out str, out var consumed)) {
                        throw new Exception();
                    }
                    len += str.Length;
                }
            }
        }
        return len;
    }
}

