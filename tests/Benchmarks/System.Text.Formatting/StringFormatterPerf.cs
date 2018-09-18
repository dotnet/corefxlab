// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers;
using System.Buffers.Text;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using System.Text.Formatting;

namespace System.Text.Formatting.Benchmarks
{
    public class StringFormatterPerf
    {
        [Params(10, 1000)]
        public int NumbersToWrite { get; set; }
        private static ArrayPool<byte> pool = ArrayPool<byte>.Shared;
        private SymbolTable _symbolTable;

        [GlobalSetup(Target = nameof(CustomCultureFormat))]
        public void SetupCustomCultureFormat() => _symbolTable = CreateCustomCulture();

        [Benchmark]
        public string InvariantFormatIntDec()
        {
            var sb = new StringFormatter(NumbersToWrite, pool);
            for (int i = 0; i < NumbersToWrite; i++)
            {
                sb.Append(((int)(i % 10)));
            }

            return sb.ToString();
        }

        [Benchmark]
        public string InvariantFormatIntDecClr()
        {
            var sb = new StringBuilder(NumbersToWrite);
            for (int i = 0; i < NumbersToWrite; i++)
            {
                sb.Append(((int)(i % 10)));
            }

            return sb.ToString();
        }

        [Benchmark]
        public string InvariantFormatIntHex()
        {
            var sb = new StringFormatter(NumbersToWrite, pool);
            var format = new StandardFormat('X', StandardFormat.NoPrecision);

            for (int i = 0; i < NumbersToWrite; i++)
            {
                sb.Append(((int)(i % 10)), format);
            }

            return sb.ToString();
        }

        [Benchmark]
        public string InvariantFormatIntHexClr()
        {
            var sb = new StringBuilder(NumbersToWrite);
            for (int i = 0; i < NumbersToWrite; i++)
            {
                sb.Append(((int)(i % 10)).ToString("X"));
            }

            return sb.ToString();
        }

        [Benchmark]
        public string InvariantFormatStruct()
        {
            var sb = new StringFormatter(NumbersToWrite * 2, pool);
            for (int i = 0; i < NumbersToWrite; i++)
            {
                sb.Append(new Age(i % 10));
            }

            return sb.ToString();
        }

        [Benchmark]
        public string FormatGuid()
        {
            var guid = Guid.NewGuid();
            var guidsToWrite = NumbersToWrite / 10;

            var sb = new StringFormatter(guidsToWrite * 36, pool);
            for (int i = 0; i < guidsToWrite; i++)
            {
                sb.Append(guid);
            }

            return sb.ToString();
        }

        [Benchmark]
        public string InvariantFormatStructClr()
        {
            var sb = new StringBuilder(NumbersToWrite * 2);
            for (int i = 0; i < NumbersToWrite; i++)
            {
                sb.Append(new Age(i % 10));
            }

            return sb.ToString();
        }

        [Benchmark]
        public string CustomCultureFormat()
        {
            var sb = new StringFormatter(NumbersToWrite * 3, pool);
            sb.SymbolTable = _symbolTable;

            for (int i = 0; i < NumbersToWrite; i++)
            {
                var next = (i % 128) + 101;
                sb.Append(next);
            }

            return sb.ToString();
        }

        [Benchmark]
        public string CustomCultureFormatClr()
        {
            var sb = new StringBuilder(NumbersToWrite * 3);
            var culture = new CultureInfo("th");

            for (int i = 0; i < NumbersToWrite; i++)
            {
                sb.Append(((i % 128) + 100).ToString(culture));
            }

            return sb.ToString();
        }

        [Benchmark]
        public ArraySegment<byte> EncodeStringToUtf8()
        {
            const string text = "Hello World!";
            int stringsToWrite = 2000;
            int size = stringsToWrite * text.Length + stringsToWrite;
            var formatter = new ArrayFormatter(size, SymbolTable.InvariantUtf8, pool);

            for (int i = 0; i < stringsToWrite; i++)
            {
                formatter.Append(text);
                formatter.Append(1);
            }

            return formatter.Formatted;
        }

        [Benchmark]
        public byte[] EncodeStringToUtf8Clr()
        {
            const string text = "Hello World!";
            int stringsToWrite = 2000;
            int size = stringsToWrite * text.Length + stringsToWrite;
            var formatter = new StringBuilder(size);

            for (int i = 0; i < stringsToWrite; i++)
            {
                formatter.Append(text);
                formatter.Append(1);
            }

            return Encoding.UTF8.GetBytes(formatter.ToString());
        }

        public static SymbolTable CreateCustomCulture()
        {
            var utf16digitsAndSymbols = new byte[17][];
            for (ushort digit = 0; digit < 10; digit++)
            {
                char digitChar = (char)(digit + 'A');
                var digitString = new string(digitChar, 1);
                utf16digitsAndSymbols[digit] = Encoding.Unicode.GetBytes(digitString);
            }

            utf16digitsAndSymbols[(ushort)SymbolTable.Symbol.DecimalSeparator] = Encoding.Unicode.GetBytes(".");
            utf16digitsAndSymbols[(ushort)SymbolTable.Symbol.GroupSeparator] = Encoding.Unicode.GetBytes(",");
            utf16digitsAndSymbols[(ushort)SymbolTable.Symbol.MinusSign] = Encoding.Unicode.GetBytes("_?");

            return new CustomUtf16SymbolTable(utf16digitsAndSymbols);
        }

        public struct Age : IBufferFormattable
        {
            int _age;
            bool _inMonths;

            public Age(int age, bool inMonths = false)
            {
                _age = age;
                _inMonths = inMonths;
            }

            public bool TryFormat(Span<byte> buffer, out int bytesWritten, StandardFormat format, SymbolTable symbolTable)
            {
                if (!CustomFormatter.TryFormat(_age, buffer, out bytesWritten, format, symbolTable))
                    return false;

                char symbol = _inMonths ? 'm' : 'y';
                if (!symbolTable.TryEncode((byte)symbol, buffer.Slice(bytesWritten), out int written))
                    return false;

                bytesWritten += written;
                return true;
            }

            public override string ToString()
            {
                return _age.ToString() + (_inMonths ? "m" : "y");
            }
        }

        public class CustomUtf16SymbolTable : SymbolTable
        {
            public CustomUtf16SymbolTable(byte[][] symbols) : base(symbols) { }

            public override bool TryEncode(byte utf8, Span<byte> destination, out int bytesWritten)
                => SymbolTable.InvariantUtf16.TryEncode(utf8, destination, out bytesWritten);

            public override bool TryEncode(ReadOnlySpan<byte> utf8, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
                => SymbolTable.InvariantUtf16.TryEncode(utf8, destination, out bytesConsumed, out bytesWritten);

            public override bool TryParse(ReadOnlySpan<byte> source, out byte utf8, out int bytesConsumed)
                => SymbolTable.InvariantUtf16.TryParse(source, out utf8, out bytesConsumed);

            public override bool TryParse(ReadOnlySpan<byte> source, Span<byte> utf8, out int bytesConsumed, out int bytesWritten)
                => SymbolTable.InvariantUtf16.TryParse(source, utf8, out bytesConsumed, out bytesWritten);
        }
    }
}
