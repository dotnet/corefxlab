// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text.Primitives.Tests;
using BenchmarkDotNet.Attributes;
using Benchmarks.System.Text.Primitives;

namespace System.Text.Primitives.Benchmarks
{
    public partial class PrimitiveParserPerfTests
    {
        public IEnumerable<object> ParseSbyteThai_Arguments()
        {
            yield return new Utf8ByteArrayArgument("๑๑๑");
            yield return new Utf8ByteArrayArgument("๑๒๔");
            yield return new Utf8ByteArrayArgument("๒");
            yield return new Utf8ByteArrayArgument("๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๑๒๓๕abcdfg");
            yield return new Utf8ByteArrayArgument("๒๑๔๗๔abcdefghijklmnop");
        }
        
        [Benchmark]
        [ArgumentsSource(nameof(ParseSbyteThai_Arguments))]
        public bool ParseSByteThai(Utf8ByteArrayArgument text) => CustomParser.TryParseSByte(text.CreateSpan(), out sbyte value, out int bytesConsumed, 'G', TestHelper.ThaiTable);
    }
}
