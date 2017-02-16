// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Xunit.Performance;
using System;
using System.Buffers;
using System.Numerics;
using System.Text;

public class IndexOfBench
{
    static int s_bufferLength = 1000;
    static byte[] s_buffer = new byte[s_bufferLength];
    static int s_loops = 1000;

    static IndexOfBench() 
    {
        s_buffer[s_bufferLength - 100] = 255;
    }

    [Benchmark]
    static int SpanIndexOf()
    {
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
        return index;
    }

    [Benchmark]
    static int VectorizedIndexOf()
    {
        if(!Vector.IsHardwareAccelerated) return 0;

        Span<byte> buffer = s_buffer;
        int index = 0;
        foreach (var iteration in Benchmark.Iterations)
        {
            using (iteration.StartMeasurement())
            {
                for(int i=0; i<s_loops; i++) {
                    index += buffer.IndexOfVectorized(255);
                }
            }
        }
        return index;
    }
}

