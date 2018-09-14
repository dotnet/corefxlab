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
        public IEnumerable<object> ParseInt16Thai_Arguments()
        {
            yield return new Utf8ByteArrayArgument("๑๐๗๓๗");
            yield return new Utf8ByteArrayArgument("๒๑๔๗๔");
            yield return new Utf8ByteArrayArgument("๐");
            yield return new Utf8ByteArrayArgument("๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๑๒๓๕abcdfg");
            yield return new Utf8ByteArrayArgument("๒๑๔๗๔abcdefghijklmnop");
        }

        [Benchmark]
        [ArgumentsSource(nameof(ParseInt16Thai_Arguments))]
        public bool ParseInt16Thai(Utf8ByteArrayArgument text) 
            => CustomParser.TryParseInt16(text.CreateSpan(), out short value, out int bytesConsumed, 'G', TestHelper.ThaiTable);
    }
}


