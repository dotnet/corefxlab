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
        public IEnumerable<object> ByteSpanToInt16_Arguments()		
        {		
            yield return new Utf8ByteArrayArgument("10737");		
            yield return new Utf8ByteArrayArgument("21474");	
            yield return new Utf8ByteArrayArgument("0"); 		
            yield return new Utf8ByteArrayArgument("000000000000000000001235abcdfg");		
            yield return new Utf8ByteArrayArgument("21474abcdefghijklmnop");		
        }

        public IEnumerable<object> ByteSpanToInt16_Thai_Arguments()
        {
            yield return new Utf8ByteArrayArgument("๑๐๗๓๗");
            yield return new Utf8ByteArrayArgument("๒๑๔๗๔");
            yield return new Utf8ByteArrayArgument("๐");
            yield return new Utf8ByteArrayArgument("๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๑๒๓๕abcdfg");
            yield return new Utf8ByteArrayArgument("๒๑๔๗๔abcdefghijklmnop");
        }

        [Benchmark(Baseline = true)]		
        [ArgumentsSource(nameof(ByteSpanToInt16_Arguments))]		
        public bool ByteSpanToInt16(Utf8ByteArrayArgument text) 
            => Utf8Parser.TryParse(text.CreateSpan(), out short value, out int consumed);
        
        [Benchmark]
        [ArgumentsSource(nameof(ByteSpanToInt16_Thai_Arguments))]
        public bool ByteSpanToInt16_Thai(Utf8ByteArrayArgument text) 
            => CustomParser.TryParseInt16(text.CreateSpan(), out short value, out int bytesConsumed, 'G', TestHelper.ThaiTable);
    }
}


