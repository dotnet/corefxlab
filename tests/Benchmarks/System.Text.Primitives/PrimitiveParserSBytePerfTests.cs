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
        private static readonly string[] s_SByteTextArray = new string[17]
        {
           "95",
            "2",
            "112",
            "-112",
            "-21",
            "-2",
            "114",
            "-114",
            "-124",
            "117",
            "-117",
            "-14",
            "14",
            "74",
            "21",
            "83",
            "-127"
        };
        
        public IEnumerable<object> SByte_BytesConsumed_VariableLength_Arguments()
            => s_Int32TextArray.Select(text => new Utf8ByteArrayArgument(text));

        [Benchmark]
        [ArgumentsSource(nameof(SByte_BytesConsumed_VariableLength_Arguments))]
        public bool PrimitiveParserByteSpanToSByte_BytesConsumed_VariableLength(Utf8ByteArrayArgument text)
            => Utf8Parser.TryParse(text.CreateSpan(), out sbyte value, out int bytesConsumed);
        
        [Benchmark]
        [ArgumentsSource(nameof(SByte_BytesConsumed_VariableLength_Arguments))]
        public bool PrimitiveParserByteSpanToSByte_BytesConsumed_VariableLength_Baseline(Utf8ByteArrayArgument text)
            => sbyte.TryParse(text.Text, out sbyte value);
        
        public IEnumerable<object> PrimitiveParserByteSpanToSByte_Arguments()
        {
            yield return new Utf8ByteArrayArgument("107"); // standard parse
            yield return new Utf8ByteArrayArgument("127"); // max value
            yield return new Utf8ByteArrayArgument("0"); 
            yield return new Utf8ByteArrayArgument("-128"); // min value
            yield return new Utf8ByteArrayArgument("-000000000000000000001235abcdfg");
            yield return new Utf8ByteArrayArgument("-2147abcdefghijklmnop");
        }

        [Benchmark]
        [ArgumentsSource(nameof(PrimitiveParserByteSpanToSByte_Arguments))]
        public bool PrimitiveParserByteSpanToSByte_BytesConsumed(Utf8ByteArrayArgument text) => Utf8Parser.TryParse(text.CreateSpan(), out sbyte value, out int bytesConsumed);

        [Benchmark]
        [ArgumentsSource(nameof(PrimitiveParserByteSpanToSByte_Arguments))]
        public bool PrimitiveParserByteSpanToSByte_BytesConsumed_Baseline(Utf8ByteArrayArgument text) => sbyte.TryParse(text.Text, out sbyte value);
        
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
