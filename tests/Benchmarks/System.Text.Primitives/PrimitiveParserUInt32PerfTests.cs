// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text.Primitives.Tests;
using BenchmarkDotNet.Attributes;

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
        
        [Benchmark]
        [Arguments("2134567890")] // standard parse
        [Arguments("4294967295")] // max value
        [Arguments("0")] // min value
        public bool BaselineSimpleByteStarToUInt32(string text) => uint.TryParse(text, out uint value);

        [Benchmark]
        public bool BaselineSimpleByteStarToUInt32_VariableLength()
        {
            bool result = false;
            for (int i = 0; i < TestHelper.LoadIterations; i++)
            {
                result = uint.TryParse(s_UInt32TextArray[i % 10], out uint value);
            }
            return result;
        }

        [Benchmark]
        [Arguments("2134567890")] // standard parse
        [Arguments("4294967295")] // max value
        [Arguments("0")] // min value
        public bool BaselineByteStarToUInt32(string text) => uint.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out uint value);

        [Benchmark]
        public bool BaselineByteStarToUInt32_VariableLength()
        {
            bool result = false;
            for (int i = 0; i < TestHelper.LoadIterations; i++)
            {
                result = uint.TryParse(s_UInt32TextArray[i % 10], NumberStyles.None, CultureInfo.InvariantCulture, out uint value);
            }
            return result;
        }

        [Benchmark]
        [Arguments("abcdef")] // standard parse
        [Arguments("ffffffff")] // max value
        [Arguments("0")] // min value
        public bool BaselineByteStarToUInt32Hex(string text)
        {
            bool result = false;
            for (int i = 0; i < TestHelper.LoadIterations; i++)
            {
                result =uint.TryParse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint value);
            }
            return result;
        }

        [Benchmark]
        public bool BaselineByteStarToUInt32Hex_VariableLength()
        {
            bool result = false;
            for (int i = 0; i < TestHelper.LoadIterations; i++)
            {
                uint.TryParse(s_UInt32TextArrayHex[i % 8], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint value);
            }
            return result;
        }

        [Benchmark]
        [Arguments("2134567890")] // standard parse
        [Arguments("4294967295")] // max value
        [Arguments("0")] // min value
        public bool PrimitiveParserByteSpanToUInt32_BytesConsumed(string text)
        {
            byte[] utf8ByteArray = Text.Encoding.UTF8.GetBytes(text);
            var utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);
            bool result = false;
            for (int i = 0; i < TestHelper.LoadIterations; i++)
            {
                result = Utf8Parser.TryParse(utf8ByteSpan, out uint value, out int bytesConsumed);
            }
            return result;
        }

        public IEnumerable<object> PrimitiveParserByteSpanToUInt32_BytesConsumed_VariableLength_Arguments()
            => s_UInt32TextArray.Select(text => Encoding.UTF8.GetBytes(text));

        [Benchmark]
        [ArgumentsSource(nameof(PrimitiveParserByteSpanToUInt32_BytesConsumed_VariableLength_Arguments))]
        public bool PrimitiveParserByteSpanToUInt32_BytesConsumed_VariableLength(byte[] utf8ByteArray) 
            => Utf8Parser.TryParse(new ReadOnlySpan<byte>(utf8ByteArray), out uint value, out int bytesConsumed);

        [Benchmark]
        [Arguments("abcdef")] // standard parse
        [Arguments("ffffffff")] // max value
        [Arguments("0")] // min value
        public bool PrimitiveParserByteSpanToUInt32Hex_BytesConsumed(string text)
        {
            byte[] utf8ByteArray = Text.Encoding.UTF8.GetBytes(text);
            var utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);

            bool result = false;
            for (int i = 0; i < TestHelper.LoadIterations; i++)
            {
                Utf8Parser.TryParse(utf8ByteSpan, out uint value, out int bytesConsumed, 'X');
            }

            return result;
        }
        
        public IEnumerable<object> PrimitiveParserByteSpanToUInt32Hex_BytesConsumed_VariableLength_Arguments()
            => s_UInt32TextArrayHex.Select(text => Encoding.UTF8.GetBytes(text));

        [Benchmark]
        [ArgumentsSource(nameof(PrimitiveParserByteSpanToUInt32Hex_BytesConsumed_VariableLength_Arguments))]
        public bool  PrimitiveParserByteSpanToUInt32Hex_BytesConsumed_VariableLength(byte[] utf8ByteArray) 
            => Utf8Parser.TryParse(new ReadOnlySpan<byte>(utf8ByteArray), out uint value, out int bytesConsumed, 'X');
    }
}
