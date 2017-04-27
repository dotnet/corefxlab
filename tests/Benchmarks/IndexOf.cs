// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;
using Microsoft.Xunit.Performance;
using System;
using System.Buffers;
using System.Numerics;

public class IndexOfBench
{
    static int s_bufferLength = 2000;
    static byte[] s_buffer = new byte[s_bufferLength];
    static int s_loops = 1000;

    [Benchmark]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(8)]
    [InlineData(16)]
    [InlineData(30)]
    [InlineData(1000)]
    static int SpanIndexOf(int at)
    {
        s_buffer[at] = 255;
        Span<byte> buffer = s_buffer;
        int index = 0;
        foreach (var iteration in Benchmark.Iterations)
        {
            using (iteration.StartMeasurement())
            {
                for(int i=0; i<s_loops; i++) {
                    index += buffer.IndexOf(255);
                }
            }
        }
        s_buffer[at] = 0;
        return index;
    }

    [Benchmark]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(8)]
    [InlineData(16)]
    [InlineData(30)]
    [InlineData(1000)]
    static int VectorizedIndexOf(int at)
    {
        if(!Vector.IsHardwareAccelerated) return 0;
        s_buffer[at] = 255;

        Span<byte> buffer = s_buffer;
        int index = 0;
        foreach (var iteration in Benchmark.Iterations)
        {
            using (iteration.StartMeasurement())
            {
                for(int i=0; i<s_loops; i++) {
                    index += buffer.IndexOf(255);
                }
            }
        }
        s_buffer[at] = 0;
        return index;
    }
}

