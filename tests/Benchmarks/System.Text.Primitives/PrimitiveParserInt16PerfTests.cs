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
        private static readonly string[] s_Int16TextArray = new string[13]
        {
            "21474",
            "2",
            "-21474",
            "31484",
            "-21",
            "-2",
            "214",
            "2147",
            "-2147",
            "-9345",
            "9345",
            "1000",
            "-214"
        };

        public IEnumerable<object> PrimitiveParserByteSpanToInt16_VariableLength_Arguments()
            => s_Int16TextArray.Select(text => new Utf8ByteArrayArgument(text));
        
        public IEnumerable<object> PrimitiveParserByteSpanToInt16_Arguments()
        {
            yield return new Utf8ByteArrayArgument("10737"); // standard parse
            yield return new Utf8ByteArrayArgument("32767"); // max value
            yield return new Utf8ByteArrayArgument("0"); 
            yield return new Utf8ByteArrayArgument("-32768"); // min value
            yield return new Utf8ByteArrayArgument("-000000000000000000001235abcdfg");
            yield return new Utf8ByteArrayArgument("-2147abcdefghijklmnop");
        }

        [Benchmark]
        [ArgumentsSource(nameof(PrimitiveParserByteSpanToInt16_Arguments))]
        public bool PrimitiveParserByteSpanToInt16(Utf8ByteArrayArgument text) 
            => Utf8Parser.TryParse(text.CreateSpan(), out short value, out int consumed);
        
        [Benchmark]
        [ArgumentsSource(nameof(PrimitiveParserByteSpanToInt16_Arguments))]
        public bool PrimitiveParserByteSpanToInt16_BytesConsumed_Baseline(Utf8ByteArrayArgument text) 
            => short.TryParse(text.Text, out short value);

        [Benchmark]
        [ArgumentsSource(nameof(PrimitiveParserByteSpanToInt16_VariableLength_Arguments))]
        public bool PrimitiveParserByteSpanToInt16_VariableLength(Utf8ByteArrayArgument text) 
            => Utf8Parser.TryParse(text.CreateSpan(), out short value, out int bytesConsumed);

        [Benchmark]
        [ArgumentsSource(nameof(PrimitiveParserByteSpanToInt16_VariableLength_Arguments))]
        public bool PrimitiveParserByteSpanToInt16_VariableLength_Baseline(Utf8ByteArrayArgument text)
            => short.TryParse(text.Text, out short value);

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


