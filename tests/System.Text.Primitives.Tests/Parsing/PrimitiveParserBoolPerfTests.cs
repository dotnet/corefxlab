// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;
using Microsoft.Xunit.Performance;
using System.Buffers.Text;

namespace System.Text.Primitives.Tests
{
    public partial class PrimitiveParserPerfTests
    {
        [Benchmark]
        [InlineData("True")]
        [InlineData("False")]
        private static void BaselineStringToBool(string text)
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < TestHelper.LoadIterations; i++)
                    {
                        bool.TryParse(text, out bool value);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("True")]
        [InlineData("False")]
        private static void PrimitiveParserByteSpanToBool(string text)
        {
            byte[] utf8ByteArray = Text.Encoding.UTF8.GetBytes(text);
            ReadOnlySpan<byte> utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < TestHelper.LoadIterations; i++)
                    {
                        Utf8Parser.TryParse(utf8ByteSpan, out bool value, out int bytesConsumed);
                    }
                }
            }
        }
    }
}
