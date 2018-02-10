// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Xunit.Performance;
using System;
using System.Buffers;
using System.Buffers.Text;
using System.Text;

public class BytesReaderBench
{
    static byte[] s_data;
    static ReadOnlySequence<byte> s_buffer;

    static BytesReaderBench()
    {
        int sections = 100000;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < sections; i++)
        {
            sb.Append("1234 ");
        }
        s_data = Encoding.UTF8.GetBytes(sb.ToString());
        s_buffer = new ReadOnlySequence<byte>(s_data);
    }

    [Benchmark]
    static void ParseInt32BufferReader()
    {
        foreach (var iteration in Benchmark.Iterations) {
            var reader = BufferReader.Create(s_buffer);

            using (iteration.StartMeasurement()) {

                while(BufferReaderExtensions.TryParse(ref reader, out int value)) {
                    reader.Advance(1);
                }
            }
        }
    }

    [Benchmark]
    static void ParseInt32Utf8Parser()
    {
        var data = new ReadOnlySpan<byte>(s_data);

        foreach (var iteration in Benchmark.Iterations)
        {
            using (iteration.StartMeasurement())
            {
                int totalConsumed = 0;
                while(Utf8Parser.TryParse(data.Slice(totalConsumed), out int value, out int consumed))
                {
                    totalConsumed += consumed + 1;
                }
            }
        }
    }

    [Benchmark]
    static void ParseInt32BufferReaderRaw()
    {
        foreach (var iteration in Benchmark.Iterations)
        {
            var buffer = new ReadOnlySequence<byte>(s_data);
            var reader = BufferReader.Create(buffer);

            using (iteration.StartMeasurement())
            {
                while(Utf8Parser.TryParse(reader.CurrentSegment.Slice(reader.ConsumedBytes), out int value, out int consumed)){
                    reader.Advance(consumed + 1);
                }
            }
        }
    }
}

