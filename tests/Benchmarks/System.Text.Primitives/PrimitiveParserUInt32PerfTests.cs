// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Benchmarks.System.Text.Primitives;

namespace System.Text.Primitives.Benchmarks
{
    public partial class PrimitiveParserPerfTests
    {
        private static readonly string[] s_UInt32TextArray = new string[10]
        {
            "42",
            "429496",
            "429496729",
            "42949",
            "4",
            "42949672",
            "4294",
            "429",
            "4294967295",
            "4294967"
        };

        private static readonly string[] s_UInt32TextArrayHex = new string[8]
        {
            "A2",
            "A29496",
            "A2949",
            "A",
            "A2949672",
            "A294",
            "A29",
            "A294967"
        };

        public IEnumerable<object> UInt32_VariableLength_Arguments()
            => s_UInt32TextArray.Select(text => new Utf8ByteArrayArgument(text));
        
        [Benchmark]
        [ArgumentsSource(nameof(UInt32_VariableLength_Arguments))]
        public bool BaselineSimpleByteStarToUInt32_VariableLength(Utf8ByteArrayArgument text) 
            => uint.TryParse(text.Text, out uint value);
        
        [Benchmark]
        [ArgumentsSource(nameof(UInt32_VariableLength_Arguments))]
        public bool BaselineByteStarToUInt32_VariableLength(Utf8ByteArrayArgument text)
            => uint.TryParse(text.Text, NumberStyles.None, CultureInfo.InvariantCulture, out uint value);
        
        [Benchmark]
        [ArgumentsSource(nameof(UInt32_VariableLength_Arguments))]
        public bool PrimitiveParserByteSpanToUInt32_BytesConsumed_VariableLength(Utf8ByteArrayArgument text) 
            => Utf8Parser.TryParse(text.CreateSpan(), out uint value, out int bytesConsumed);

        public IEnumerable<object> Int32Hex_BytesConsumed_VariableLength_Arguments()
            => s_UInt32TextArrayHex.Select(text => new Utf8ByteArrayArgument(text));
        
        [Benchmark]
        [ArgumentsSource(nameof(Int32Hex_BytesConsumed_VariableLength_Arguments))]
        public bool BaselineByteStarToUInt32Hex_VariableLength(Utf8ByteArrayArgument text)
            => uint.TryParse(text.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint value);
        
        [Benchmark]
        [ArgumentsSource(nameof(Int32Hex_BytesConsumed_VariableLength_Arguments))]
        public bool  PrimitiveParserByteSpanToUInt32Hex_BytesConsumed_VariableLength(Utf8ByteArrayArgument text) 
            => Utf8Parser.TryParse(text.CreateSpan(), out uint value, out int bytesConsumed, 'X');
        
        public IEnumerable<object> PrimitiveParser_UIn32_Arguments()
        {
            yield return new Utf8ByteArrayArgument("2134567890"); // standard parse
            yield return new Utf8ByteArrayArgument("4294967295"); // max value
            yield return new Utf8ByteArrayArgument("0"); // min value
        }
        
        [Benchmark]
        [ArgumentsSource(nameof(PrimitiveParser_UIn32_Arguments))]
        public bool BaselineSimpleByteStarToUInt32(Utf8ByteArrayArgument text) 
            => uint.TryParse(text.Text, out uint value);

        [Benchmark]
        [ArgumentsSource(nameof(PrimitiveParser_UIn32_Arguments))]
        public bool BaselineByteStarToUInt32(Utf8ByteArrayArgument text) 
            => uint.TryParse(text.Text, NumberStyles.None, CultureInfo.InvariantCulture, out uint value);

        [Benchmark]
        [ArgumentsSource(nameof(PrimitiveParser_UIn32_Arguments))]
        public bool PrimitiveParserByteSpanToUInt32_BytesConsumed(Utf8ByteArrayArgument text)
            => Utf8Parser.TryParse(text.CreateSpan(), out uint value, out int bytesConsumed);

        public IEnumerable<object> PrimitiveParser_UIn32_Hex_Arguments()
        {
            yield return new Utf8ByteArrayArgument("abcdef"); // standard parse
            yield return new Utf8ByteArrayArgument("ffffffff"); // max value
            yield return new Utf8ByteArrayArgument("0"); // min value
        }
        
        [Benchmark]
        [ArgumentsSource(nameof(PrimitiveParser_UIn32_Hex_Arguments))]
        public bool BaselineByteStarToUInt32Hex(Utf8ByteArrayArgument text)
            => uint.TryParse(text.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint value);

        [Benchmark]
        [ArgumentsSource(nameof(PrimitiveParser_UIn32_Hex_Arguments))]
        public bool PrimitiveParserByteSpanToUInt32Hex_BytesConsumed(Utf8ByteArrayArgument text)
            => Utf8Parser.TryParse(text.CreateSpan(), out uint value, out int bytesConsumed, 'X');

    }
}
