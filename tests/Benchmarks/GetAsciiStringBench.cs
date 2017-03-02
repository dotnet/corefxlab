// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Xunit;
using Microsoft.Xunit.Performance;
using System;

using System.Text;
using System.IO.Pipelines.Text.Primitives;

public class GetAsciiStringBench
{
    [Benchmark(InnerIterationCount = 1000000)]
    [InlineData("/plaintext")]
    [InlineData("text/plain,text/html;q=0.9,application/xhtml+xml;q=0.9,application/xml;q=0.8,*/*;q=0.7")]
    public static int GetAsciiString(string text)
    {
        string str;
        int len = 0;
        var bytes = (Span<byte>)Encoding.ASCII.GetBytes(text);
        foreach (var iteration in Benchmark.Iterations) {
            using (iteration.StartMeasurement()) {
                for (int i = 0; i < Benchmark.InnerIterationCount; i++) { 
                    str = bytes.GetAsciiString();
                    len += str.Length;
                }
            }
        }
        return len;
    }
}

