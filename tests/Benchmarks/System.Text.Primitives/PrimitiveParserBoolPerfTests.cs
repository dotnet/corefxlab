// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Benchmarks.System.Text.Primitives;

namespace System.Text.Primitives.Benchmarks
{
    public partial class PrimitiveParserPerfTests
    {
        [Benchmark]
        [Arguments("True")]
        [Arguments("False")]
        public void BaselineStringToBool(string text) => bool.TryParse(text, out _);

        [Benchmark]
        [ArgumentsSource(nameof(Utf8ByteArrays))]
        public void PrimitiveParserByteSpanToBool(Utf8ByteArrayArgument text) => Utf8Parser.TryParse(text.CreateSpan(), out bool value, out int bytesConsumed);

        public IEnumerable<object> Utf8ByteArrays()
        {
            yield return new Utf8ByteArrayArgument("True");
            yield return new Utf8ByteArrayArgument("False");
        }
    }
}
