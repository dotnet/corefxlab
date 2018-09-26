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
        public IEnumerable<object> ByteSpanToInt32_Arguments()		
        {		
            yield return new Utf8ByteArrayArgument("107374182");	
            yield return new Utf8ByteArrayArgument("2147483647");	
            yield return new Utf8ByteArrayArgument("0"); 		
            yield return new Utf8ByteArrayArgument("000000000000000000001235abcdfg");		
            yield return new Utf8ByteArrayArgument("21474abcdefghijklmnop");		
        }

        public IEnumerable<object> ParseInt32_Thai_Arguments()
        {
            yield return new Utf8ByteArrayArgument("๑๐๗๓๗๔๑๘๒");
            yield return new Utf8ByteArrayArgument("๒๑๔๗๔๘๓๖๔๗");
            yield return new Utf8ByteArrayArgument("๐");
            yield return new Utf8ByteArrayArgument("๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๑๒๓๕abcdfg");
            yield return new Utf8ByteArrayArgument("๒๑๔๗๔abcdefghijklmnop");
        }

        [Benchmark(Baseline = true)]		
        [ArgumentsSource(nameof(ByteSpanToInt32_Arguments))]		
        public bool ByteSpanToInt32(Utf8ByteArrayArgument text) 
            => Utf8Parser.TryParse(text.CreateSpan(), out int value, out int consumed);
         
        [Benchmark]
        [ArgumentsSource(nameof(ParseInt32_Thai_Arguments))]
        public bool ParseInt32_Thai(Utf8ByteArrayArgument text) 
            => CustomParser.TryParseInt32(text.CreateSpan(), out int value, out int bytesConsumed, 'G', TestHelper.ThaiTable);
    }
}
