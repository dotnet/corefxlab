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
        private const int InnerCount = 10000;

        private static readonly string[] s_Int32TextArray = new string[20]
        {
            "214748364",
            "2",
            "21474836",
            "-21474",
            "21474",
            "-21",
            "-2",
            "214",
            "-21474836",
            "-214748364",
            "2147",
            "-2147",
            "-214748",
            "-2147483",
            "214748",
            "-2147483648",
            "2147483647",
            "21",
            "2147483",
            "-214"
        };
        
        public IEnumerable<object> Int32_BytesConsumed_VariableLength_Arguments()
            => s_Int32TextArray.Select(text => new Utf8ByteArrayArgument(text));

        [Benchmark]
        [ArgumentsSource(nameof(Int32_BytesConsumed_VariableLength_Arguments))]
        public bool PrimitiveParserByteSpanToInt32_BytesConsumed_VariableLength(Utf8ByteArrayArgument text)
            => Utf8Parser.TryParse(text.CreateSpan(), out int value, out int bytesConsumed);
        
        [Benchmark]
        [ArgumentsSource(nameof(Int32_BytesConsumed_VariableLength_Arguments))]
        public bool PrimitiveParserByteSpanToInt32_BytesConsumed_VariableLength_Baseline(Utf8ByteArrayArgument text)
            => int.TryParse(text.Text, out int value);
        
        public IEnumerable<object> PrimitiveParserByteSpanToInt32Arguments()
        {
            yield return new Utf8ByteArrayArgument("107374182"); // standard parse
            yield return new Utf8ByteArrayArgument("2147483647"); // max value
            yield return new Utf8ByteArrayArgument("0"); 
            yield return new Utf8ByteArrayArgument("-2147483648"); // min value
            yield return new Utf8ByteArrayArgument("-000000000000000000001235abcdfg");
            yield return new Utf8ByteArrayArgument("-2147abcdefghijklmnop");
        }

        [Benchmark]
        [ArgumentsSource(nameof(PrimitiveParserByteSpanToInt32Arguments))]
        public bool PrimitiveParserByteSpanToInt32(Utf8ByteArrayArgument text) 
            => Utf8Parser.TryParse(text.CreateSpan(), out int value, out int consumed);
        
        [Benchmark]
        [ArgumentsSource(nameof(PrimitiveParserByteSpanToInt16_Arguments))]
        public bool PrimitiveParserByteSpanToInt32_Baseline(Utf8ByteArrayArgument text) 
            => int.TryParse(text.Text, out int value);

        public IEnumerable<object> ParseInt32Thai_Arguments()
        {
            yield return new Utf8ByteArrayArgument("๑๐๗๓๗๔๑๘๒");
            yield return new Utf8ByteArrayArgument("๒๑๔๗๔๘๓๖๔๗");
            yield return new Utf8ByteArrayArgument("๐");
            yield return new Utf8ByteArrayArgument("๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๑๒๓๕abcdfg");
            yield return new Utf8ByteArrayArgument("๒๑๔๗๔abcdefghijklmnop");
        }
        
        [Benchmark]
        [ArgumentsSource(nameof(ParseInt32Thai_Arguments))]
        public bool ParseInt32Thai(Utf8ByteArrayArgument text) 
            => CustomParser.TryParseInt32(text.CreateSpan(), out int value, out int bytesConsumed, 'G', TestHelper.ThaiTable);
    }
}
