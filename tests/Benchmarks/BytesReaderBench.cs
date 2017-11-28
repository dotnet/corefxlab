// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Xunit.Performance;
using System;
using System.Buffers;
using System.Buffers.Text;
using System.Collections.Sequences;
using System.Text;

public class BytesReaderBench
{
    static byte[] s_eol = new byte[] { (byte)'\r', (byte)'\n' };
    static byte[] s_data;
    static ReadOnlyBytes s_bytes;

    static BytesReaderBench()
    {
        int sections = 100000;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < sections; i++)
        {
            sb.Append("123456789012345678\r\n");
        }
        s_data = Encoding.UTF8.GetBytes(sb.ToString());
        s_bytes = new ReadOnlyBytes(s_data);
    }

    [Benchmark]
    static void BytesReaderBasic()
    {
        var eol = new ReadOnlySpan<byte>(s_eol);

        foreach (var iteration in Benchmark.Iterations)
        {
            var reader = BytesReader.Create(s_bytes);

            using (iteration.StartMeasurement())
            {
                while (true)
                {
                    var range = reader.ReadRange(eol);
                    if (range.To == Position.End) break;
                }
            }
        }
    }

    [Benchmark]
    static void BytesReaderBaseline()
    {
        var eol = new ReadOnlySpan<byte>(s_eol);

        foreach (var iteration in Benchmark.Iterations)
        {
            using (iteration.StartMeasurement())
            {
                int read = 0;
                while (true)
                {
                    var slice = s_bytes.Memory.Slice(read);
                    var index = slice.Span.IndexOf(eol);
                    if (index == -1) break;
                    read += index;
                    read += eol.Length;
                }
            }
        }
    }
}

