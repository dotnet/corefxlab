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

        public IEnumerable<object> ByteSpanToSByte_Thai_Arguments()
        {
            yield return new Utf8ByteArrayArgument("๑๑๑");
            yield return new Utf8ByteArrayArgument("๑๒๔");
            yield return new Utf8ByteArrayArgument("๒");
            yield return new Utf8ByteArrayArgument("๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๑๒๓๕abcdfg");
            yield return new Utf8ByteArrayArgument("๒๑๔๗๔abcdefghijklmnop");
        }

        public IEnumerable<object> ByteSpanToSByte_Arguments()		
        {		
            yield return new Utf8ByteArrayArgument("111");	
            yield return new Utf8ByteArrayArgument("124");		
            yield return new Utf8ByteArrayArgument("2"); 		
            yield return new Utf8ByteArrayArgument("000000000000000000003535abcdfg");		
            yield return new Utf8ByteArrayArgument("21474abcdefghijklmnop");		
        }

        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(ByteSpanToSByte_Arguments))]
        public bool ByteSpanToSByte(Utf8ByteArrayArgument text) 
            => Utf8Parser.TryParse(text.CreateSpan(), out sbyte value, out int bytesConsumed);

        
        [Benchmark]
        [ArgumentsSource(nameof(ByteSpanToSByte_Thai_Arguments))]
        public bool ByteSpanToSByte_Thai(Utf8ByteArrayArgument text) 
            => CustomParser.TryParseSByte(text.CreateSpan(), out sbyte value, out int bytesConsumed, 'G', TestHelper.ThaiTable);
    }
}
