// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Buffers.Text;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Benchmarks.System.Text.Primitives;

namespace System.Text.Primitives.Benchmarks
{
    public partial class PrimitiveParserPerfTests
    {
        [Benchmark]
        [Arguments("2134567890")] // standard parse
        [Arguments("18446744073709551615")] // max value
        [Arguments("0")] // min value
        public bool BaselineSimpleByteStarToUInt64(string text) => ulong.TryParse(text, out ulong value);

        [Benchmark]
        [Arguments("2134567890")] // standard parse
        [Arguments("18446744073709551615")] // max value
        [Arguments("0")] // min value
        public bool BaselineByteStarToUInt64(string text) => ulong.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out ulong value);

        [Benchmark]
        [Arguments("abcdef")] // standard parse
        [Arguments("ffffffffffffffff")] // max value
        [Arguments("0")] // min value
        public bool BaselineByteStarToUInt64Hex(string text) => ulong.TryParse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ulong value);
        
        public IEnumerable<object> PrimitiveParserByteSpanToUInt64_BytesConsumed_Arguments()
        {
            yield return new Utf8ByteArrayArgument("2134567890"); // standard parse
            yield return new Utf8ByteArrayArgument("18446744073709551615"); // max value
            yield return new Utf8ByteArrayArgument("0"); // min value
        }
        
        [Benchmark]
        [ArgumentsSource(nameof(PrimitiveParserByteSpanToUInt64_BytesConsumed_Arguments))] 
        public bool PrimitiveParserByteSpanToUInt64_BytesConsumed(Utf8ByteArrayArgument text) => Utf8Parser.TryParse(text.CreateSpan(), out ulong value, out int bytesConsumed);

        public IEnumerable<object> PrimitiveParserByteSpanToUInt64Hex_BytesConsumed_Arguments()
        {
            yield return new Utf8ByteArrayArgument("abcdef"); // standard parse
            yield return new Utf8ByteArrayArgument("ffffffffffffffff"); // max value
            yield return new Utf8ByteArrayArgument("0"); // min value
        }
        
        [Benchmark]
        [ArgumentsSource(nameof(PrimitiveParserByteSpanToUInt64Hex_BytesConsumed_Arguments))]
        public bool PrimitiveParserByteSpanToUInt64Hex_BytesConsumed(Utf8ByteArrayArgument text) => Utf8Parser.TryParse(text.CreateSpan(), out ulong value, out int bytesConsumed, 'X');
    }
}
