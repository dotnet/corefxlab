// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace System.Text.Formatting.Tests
{
    public class PerfSmokeTests
    {
        static ArrayPool<byte> pool = ArrayPool<byte>.Shared;
        const int numbersToWrite = 10000;
        static Stopwatch timer = new Stopwatch();

        const int itterationsInvariant = 300;
        const int itterationsCulture = 200;

        public void PrintTime([CallerMemberName] string memberName = "")
        {
            //Trace.WriteLine(string.Format("{0} : Elapsed {1}ms", memberName, timer.ElapsedMilliseconds));
        }

        [Fact]
        private void InvariantFormatIntDec()
        {
            timer.Restart();
            for (int itteration = 0; itteration < itterationsInvariant; itteration++)
            {
                StringFormatter sb = new StringFormatter(numbersToWrite, pool);
                for (int i = 0; i < numbersToWrite; i++)
                {
                    sb.Append(((int)(i % 10)));
                }
                var text = sb.ToString();
                if (text.Length != numbersToWrite)
                {
                    throw new Exception("test failed");
                }
            }
            PrintTime();
        }

        [Fact]
        private void InvariantFormatIntDecClr()
        {
            timer.Restart();
            for (int itteration = 0; itteration < itterationsInvariant; itteration++)
            {
                StringBuilder sb = new StringBuilder(numbersToWrite);
                for (int i = 0; i < numbersToWrite; i++)
                {
                    sb.Append(((int)(i % 10)));
                }
                var text = sb.ToString();
                if (text.Length != numbersToWrite)
                {
                    throw new Exception("test failed");
                }
            }
            PrintTime();
        }

        [Fact]
        private void InvariantFormatIntHex()
        {
            timer.Restart();
            for (int itteration = 0; itteration < itterationsInvariant; itteration++)
            {
                StringFormatter sb = new StringFormatter(numbersToWrite, pool);
                for (int i = 0; i < numbersToWrite; i++)
                {
                    sb.Append(((int)(i % 10)), Format.Parsed.HexUppercase);
                }
                var text = sb.ToString();
                if (text.Length != numbersToWrite)
                {
                    throw new Exception("test failed");
                }
            }
            PrintTime();
        }

        [Fact]
        private void InvariantFormatIntHexClr()
        {
            timer.Restart();
            for (int itteration = 0; itteration < itterationsInvariant; itteration++)
            {
                StringBuilder sb = new StringBuilder(numbersToWrite);
                for (int i = 0; i < numbersToWrite; i++)
                {
                    sb.Append(((int)(i % 10)).ToString("X"));
                }
                var text = sb.ToString();
                if (text.Length != numbersToWrite)
                {
                    throw new Exception("test failed");
                }
            }
            PrintTime();
        }

        [Fact]
        private void InvariantFormatStruct()
        {
            timer.Restart();
            for (int itteration = 0; itteration < itterationsInvariant; itteration++)
            {
                StringFormatter sb = new StringFormatter(numbersToWrite * 2, pool);
                for (int i = 0; i < numbersToWrite; i++)
                {
                    sb.Append(new Age(i % 10));
                }
                var text = sb.ToString();
                if (text.Length != numbersToWrite * 2)
                {
                    throw new Exception("test failed");
                }
            }
            PrintTime();
        }

        [Fact]
        private void FormatGuid()
        {
            var guid = Guid.NewGuid();
            var guidsToWrite = numbersToWrite / 10;
            timer.Restart();
            for (int itteration = 0; itteration < itterationsInvariant; itteration++)
            {
                StringFormatter sb = new StringFormatter(guidsToWrite * 36, pool);
                for (int i = 0; i < guidsToWrite; i++)
                {
                    sb.Append(guid);
                }
                var text = sb.ToString();
                if (text.Length != guidsToWrite * 36)
                {
                    throw new Exception("test failed");
                }
            }
            PrintTime();
        }

        [Fact]
        private void InvariantFormatStructClr()
        {
            timer.Restart();
            for (int itteration = 0; itteration < itterationsInvariant; itteration++)
            {
                StringBuilder sb = new StringBuilder(numbersToWrite * 2);
                for (int i = 0; i < numbersToWrite; i++)
                {
                    sb.Append(new Age(i % 10));
                }
                var text = sb.ToString();
                if (text.Length != numbersToWrite * 2)
                {
                    throw new Exception("test failed");
                }
            }
            PrintTime();
        }

        [Fact]
        private void CustomCultureFormat()
        {
            StringFormatter sb = new StringFormatter(numbersToWrite * 3, pool);
            sb.FormattingData = CreateCustomCulture();

            timer.Restart();
            for (int itteration = 0; itteration < itterationsCulture; itteration++)
            {
                sb.Clear();
                for (int i = 0; i < numbersToWrite; i++)
                {
                    var next = (i % 128) + 101;
                    sb.Append(next);
                }
                var text = sb.ToString();
                if (text.Length != numbersToWrite * 3)
                {
                    throw new Exception("test failed");
                }
            }
            PrintTime();
        }

        [Fact]
        private void CustomCultureFormatClr()
        {
            StringBuilder sb = new StringBuilder(numbersToWrite * 3);
            var culture = new CultureInfo("th");

            timer.Restart();
            for (int itteration = 0; itteration < itterationsCulture; itteration++)
            {
                sb.Clear();
                for (int i = 0; i < numbersToWrite; i++)
                {
                    sb.Append(((i % 128) + 100).ToString(culture));
                }
                var text = sb.ToString();
                if (text.Length != numbersToWrite * 3)
                {
                    throw new Exception("test failed");
                }
            }
            PrintTime();
        }

        [Fact]
        private void EncodeStringToUtf8()
        {
            string text = "Hello World!";
            int stringsToWrite = 2000;
            int size = stringsToWrite * text.Length + stringsToWrite;
            BufferFormatter formatter = new BufferFormatter(size, FormattingData.InvariantUtf8, pool);

            timer.Restart();
            for (int itteration = 0; itteration < itterationsInvariant; itteration++)
            {
                formatter.Clear();
                for (int i = 0; i < stringsToWrite; i++)
                {
                    formatter.Append(text);
                    formatter.Append(1);
                }
                Assert.Equal(size, formatter.CommitedByteCount);
            }
            PrintTime();
        }

        [Fact]
        private void EncodeStringToUtf8Clr()
        {
            string text = "Hello World!";
            int stringsToWrite = 2000;
            int size = stringsToWrite * text.Length + stringsToWrite;
            StringBuilder formatter = new StringBuilder(size);

            timer.Restart();
            for (int itteration = 0; itteration < itterationsInvariant; itteration++)
            {
                formatter.Clear();
                for (int i = 0; i < stringsToWrite; i++)
                {
                    formatter.Append(text);
                    formatter.Append(1);
                }
                var bytes = Encoding.UTF8.GetBytes(formatter.ToString());
                Assert.Equal(size, bytes.Length);
            }
            PrintTime();
        }

        static FormattingData CreateCustomCulture()
        {
            var utf16digitsAndSymbols = new byte[17][];
            for (ushort digit = 0; digit < 10; digit++)
            {
                char digitChar = (char)(digit + 'A');
                var digitString = new string(digitChar, 1);
                utf16digitsAndSymbols[digit] = GetBytesUtf16(digitString);
            }
            utf16digitsAndSymbols[(ushort)FormattingData.Symbol.DecimalSeparator] = GetBytesUtf16(".");
            utf16digitsAndSymbols[(ushort)FormattingData.Symbol.GroupSeparator] = GetBytesUtf16(",");
            utf16digitsAndSymbols[(ushort)FormattingData.Symbol.MinusSign] = GetBytesUtf16("_?");
            return new FormattingData(utf16digitsAndSymbols, FormattingData.Encoding.Utf16);
        }
        static byte[] GetBytesUtf16(string text)
        {
            return System.Text.Encoding.Unicode.GetBytes(text);
        }
    }
}

