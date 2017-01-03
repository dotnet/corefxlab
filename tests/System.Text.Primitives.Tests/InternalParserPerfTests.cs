using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Runtime.InteropServices;
using Xunit;
using Microsoft.Xunit.Performance;
using System.Text.Internal;

namespace System.Text.Primitives.Tests
{
    public class InternalParserPerfTests
    {
        private static byte[] UtfEncode(string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }
        private static string UtfDecode(byte[] b)
        {
            return Encoding.UTF8.GetString(b);
        }

        private static int LOAD_ITERATIONS = 1000;

        #region utf-8 basic culture specific

        //[Benchmark] // Legacy benchmark
        [InlineData("1147483647")] // standard parse
        [InlineData("-1147483647")] // standard negative parse
        [InlineData("+1147483647")] // explicit positive parse
        [InlineData("2147483648")] // +1 overflow
        [InlineData("5000000000")] // basic overflow
        [InlineData("128199519951281995")] // heavy overflow
        [InlineData("0127")] // leading zero
        [InlineData("00000000120")] // many leading zeroes
        [InlineData("0")] // zero
        [InlineData("2147483647")] // max value
        [InlineData("-2147483648")] // min value
        private static void Utf8CultureBaselineIntParse(string text)
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                int value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        Int32.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("1147483647")] // standard parse
        [InlineData("-1147483647")] // standard negative parse
        [InlineData("+1147483647")] // explicit positive parse
        [InlineData("2147483648")] // +1 overflow
        [InlineData("5000000000")] // basic overflow
        [InlineData("128199519951281995")] // heavy overflow
        [InlineData("0127")] // leading zero
        [InlineData("00000000120")] // many leading zeroes
        [InlineData("0")] // zero
        [InlineData("2147483647")] // max value
        [InlineData("-2147483648")] // min value
        private static void Utf8CultureBaselineArrayIntParse(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            foreach (var iteration in Benchmark.Iterations)
            {
                int value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        string decodedText = UtfDecode(utf8ByteArray);
                        Int32.TryParse(decodedText, NumberStyles.None, CultureInfo.InvariantCulture, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("1147483647")] // standard parse
        [InlineData("-1147483647")] // standard negative parse
        [InlineData("+1147483647")] // explicit positive parse
        [InlineData("2147483648")] // +1 overflow
        [InlineData("5000000000")] // basic overflow
        [InlineData("128199519951281995")] // heavy overflow
        [InlineData("0127")] // leading zero
        [InlineData("00000000120")] // many leading zeroes
        [InlineData("0")] // zero
        [InlineData("2147483647")] // max value
        [InlineData("-2147483648")] // min value
        private static void Utf8CultureByteArrayToInt(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            foreach (var iteration in Benchmark.Iterations)
            {
                int value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InternalParser.TryParseInt32(utf8ByteArray, 0, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("1147483647")] // standard parse
        [InlineData("-1147483647")] // standard negative parse
        [InlineData("+1147483647")] // explicit positive parse
        [InlineData("2147483648")] // +1 overflow
        [InlineData("5000000000")] // basic overflow
        [InlineData("128199519951281995")] // heavy overflow
        [InlineData("0127")] // leading zero
        [InlineData("00000000120")] // many leading zeroes
        [InlineData("0")] // zero
        [InlineData("2147483647")] // max value
        [InlineData("-2147483648")] // min value
        private unsafe static void Utf8CultureByteStarToInt(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            foreach (var iteration in Benchmark.Iterations)
            {
                int value;
                int bytesConsumed;
                fixed (byte* utf8ByteStar = utf8ByteArray)
                {
                    using (iteration.StartMeasurement())
                    {
                        for (int i = 0; i < LOAD_ITERATIONS; i++)
                        {
                            InternalParser.TryParseInt32(utf8ByteStar, 0, length, nf, fd, out value, out bytesConsumed);
                        }
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("1147483647")] // standard parse
        [InlineData("-1147483647")] // standard negative parse
        [InlineData("+1147483647")] // explicit positive parse
        [InlineData("2147483648")] // +1 overflow
        [InlineData("5000000000")] // basic overflow
        [InlineData("128199519951281995")] // heavy overflow
        [InlineData("0127")] // leading zero
        [InlineData("00000000120")] // many leading zeroes
        [InlineData("0")] // zero
        [InlineData("2147483647")] // max value
        [InlineData("-2147483648")] // min value
        private unsafe static void Utf8CultureByteStarUnmanagedToInt(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = UtfEncode(text);
            byte* unmanagedBytePtr;
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            unmanagedBytePtr = (byte*)Marshal.AllocHGlobal(utf8ByteArray.Length);
            Marshal.Copy(utf8ByteArray, 0, (IntPtr)unmanagedBytePtr, utf8ByteArray.Length);
            foreach (var iteration in Benchmark.Iterations)
            {
                int value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InternalParser.TryParseInt32(unmanagedBytePtr, 0, length, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
            Marshal.FreeHGlobal((IntPtr)unmanagedBytePtr);
        }

        #endregion

        #region byte
        //[Benchmark] // Legacy benchmark
        [InlineData("128")] // standard parse
        [InlineData("300")] // basic overflow
        [InlineData("1281995")] // heavy overflow
        [InlineData("0128")] // leading zero
        [InlineData("00000000128")] // many leading zeroes
        [InlineData("-128")] // negative value handling for unsigned types
        [InlineData("255")] // Max value
        [InlineData("0")] // Min value
        private static void BaselineByteParse(string text)
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                byte value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        Byte.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("128")] // standard parse
        [InlineData("300")] // basic overflow
        [InlineData("1281995")] // heavy overflow
        [InlineData("0128")] // leading zero
        [InlineData("00000000128")] // many leading zeroes
        [InlineData("-128")] // negative value handling for unsigned types
        [InlineData("255")] // Max value
        [InlineData("0")] // Min value
        private static void BaselineArrayByteParse(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            foreach (var iteration in Benchmark.Iterations)
            {
                byte value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        string decodedText = UtfDecode(utf8ByteArray);
                        Byte.TryParse(decodedText, NumberStyles.None, CultureInfo.InvariantCulture, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("128")] // standard parse
        [InlineData("300")] // basic overflow
        [InlineData("1281995")] // heavy overflow
        [InlineData("0128")] // leading zero
        [InlineData("00000000128")] // many leading zeroes
        [InlineData("-128")] // negative value handling for unsigned types
        [InlineData("255")] // Max value
        [InlineData("0")] // Min value
        private static void ByteArrayToByte(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            foreach (var iteration in Benchmark.Iterations)
            {
                byte value;
                int bytesConsumed;
                EncodingData fd = EncodingData.InvariantUtf8;
                TextFormat nf = new TextFormat('N');
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InternalParser.TryParseByte(utf8ByteArray, 0, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("128")] // standard parse
        [InlineData("300")] // basic overflow
        [InlineData("1281995")] // heavy overflow
        [InlineData("0128")] // leading zero
        [InlineData("00000000128")] // many leading zeroes
        [InlineData("-128")] // negative value handling for unsigned types
        [InlineData("255")] // Max value
        [InlineData("0")] // Min value
        private unsafe static void ByteStarToByte(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            foreach (var iteration in Benchmark.Iterations)
            {
                byte value;
                int bytesConsumed;
                fixed (byte* utf8ByteStar = utf8ByteArray)
                {
                    using (iteration.StartMeasurement())
                    {
                        for (int i = 0; i < LOAD_ITERATIONS; i++)
                        {
                            InternalParser.TryParseByte(utf8ByteStar, 0, length, nf, fd, out value, out bytesConsumed);
                        }
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("128")] // standard parse
        [InlineData("300")] // basic overflow
        [InlineData("1281995")] // heavy overflow
        [InlineData("0128")] // leading zero
        [InlineData("00000000128")] // many leading zeroes
        [InlineData("-128")] // negative value handling for unsigned types
        [InlineData("255")] // Max value
        [InlineData("0")] // Min value
        private unsafe static void ByteStarUnmanagedToByte(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            byte* unmanagedBytePtr;
            unmanagedBytePtr = (byte*)Marshal.AllocHGlobal(utf8ByteArray.Length);
            Marshal.Copy(utf8ByteArray, 0, (IntPtr)unmanagedBytePtr, utf8ByteArray.Length);
            foreach (var iteration in Benchmark.Iterations)
            {
                byte value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InternalParser.TryParseByte(unmanagedBytePtr, 0, length, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
            Marshal.FreeHGlobal((IntPtr)unmanagedBytePtr);
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("safljasldkfjsldkj2\r\n\r\n")]
        [InlineData("HTTP 1.1 / GET http://example.com/index.php?test=26,etc=blah\r\n\r\n")]
        private static void BaselineArbitraryLengthBufferToByte(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            int start = text.IndexOf('2');

            foreach (var iteration in Benchmark.Iterations)
            {
                byte value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        string decodedText = UtfDecode(utf8ByteArray);
                        int currentIndex = start;
                        char currentChar = text[currentIndex];
                        while (currentChar >= '0' && currentChar <= '9')
                        {
                            currentChar = text[currentIndex];
                            currentIndex++;
                        }
                        Byte.TryParse(decodedText.Substring(start, currentIndex - start), NumberStyles.None, CultureInfo.InvariantCulture, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("safljasldkfjsldkj2\r\n\r\n")]
        [InlineData("HTTP 1.1 / GET http://example.com/index.php?test=26,etc=blah\r\n\r\n")]
        private static void ByteArrayArbitraryLengthBufferToByte(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            int start = text.IndexOf('2');
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');

            foreach (var iteration in Benchmark.Iterations)
            {
                byte value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InternalParser.TryParseByte(utf8ByteArray, start, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
        }
        #endregion

        #region ushort
        //[Benchmark] // Legacy benchmark
        [InlineData("20500")] // standard parse
        [InlineData("80000")] // basic overflow
        [InlineData("1281995000")] // heavy overflow
        [InlineData("023128")] // leading zero
        [InlineData("00000000128")] // many leading zeroes
        [InlineData("-128")] // negative value handling for unsigned types
        [InlineData("65535")] // max value
        [InlineData("0")] // min value
        private static void BaselineUshortParse(string text)
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                ushort value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        UInt16.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("20500")] // standard parse
        [InlineData("80000")] // basic overflow
        [InlineData("1281995000")] // heavy overflow
        [InlineData("023128")] // leading zero
        [InlineData("00000000128")] // many leading zeroes
        [InlineData("-128")] // negative value handling for unsigned types
        [InlineData("65535")] // max value
        [InlineData("0")] // min value
        private static void BaselineArrayUshortParse(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            foreach (var iteration in Benchmark.Iterations)
            {
                ushort value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        string decodedText = UtfDecode(utf8ByteArray);
                        UInt16.TryParse(decodedText, NumberStyles.None, CultureInfo.InvariantCulture, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("20500")] // standard parse
        [InlineData("80000")] // basic overflow
        [InlineData("1281995000")] // heavy overflow
        [InlineData("023128")] // leading zero
        [InlineData("00000000128")] // many leading zeroes
        [InlineData("-128")] // negative value handling for unsigned types
        [InlineData("65535")] // max value
        [InlineData("0")] // min value
        private static void ByteArrayToUshort(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            foreach (var iteration in Benchmark.Iterations)
            {
                ushort value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InternalParser.TryParseUInt16(utf8ByteArray, 0, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("20500")] // standard parse
        [InlineData("80000")] // basic overflow
        [InlineData("1281995000")] // heavy overflow
        [InlineData("023128")] // leading zero
        [InlineData("00000000128")] // many leading zeroes
        [InlineData("-128")] // negative value handling for unsigned types
        [InlineData("65535")] // max value
        [InlineData("0")] // min value
        private unsafe static void ByteStarToUshort(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            foreach (var iteration in Benchmark.Iterations)
            {
                ushort value;
                int bytesConsumed;
                fixed (byte* utf8ByteStar = utf8ByteArray)
                {
                    using (iteration.StartMeasurement())
                    {
                        for (int i = 0; i < LOAD_ITERATIONS; i++)
                        {
                            InternalParser.TryParseUInt16(utf8ByteStar, 0, length, nf, fd, out value, out bytesConsumed);
                        }
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("20500")] // standard parse
        [InlineData("80000")] // basic overflow
        [InlineData("1281995000")] // heavy overflow
        [InlineData("023128")] // leading zero
        [InlineData("00000000128")] // many leading zeroes
        [InlineData("-128")] // negative value handling for unsigned types
        [InlineData("65535")] // max value
        [InlineData("0")] // min value
        private unsafe static void ByteStarUnmanagedToUshort(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            byte* unmanagedBytePtr;
            unmanagedBytePtr = (byte*)Marshal.AllocHGlobal(utf8ByteArray.Length);
            Marshal.Copy(utf8ByteArray, 0, (IntPtr)unmanagedBytePtr, utf8ByteArray.Length);
            foreach (var iteration in Benchmark.Iterations)
            {
                ushort value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InternalParser.TryParseUInt16(unmanagedBytePtr, 0, length, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
            Marshal.FreeHGlobal((IntPtr)unmanagedBytePtr);
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("safljasldkfjsldkj2\r\n\r\n")]
        [InlineData("HTTP 1.1 / GET http://example.com/index.php?test=26,etc=blah\r\n\r\n")]
        [InlineData("buffer buffer buffer buffer 26655\r\n")]
        private static void BaselineArbitraryLengthBufferToUshort(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            int start = text.IndexOf('2');

            foreach (var iteration in Benchmark.Iterations)
            {
                ushort value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        string decodedText = UtfDecode(utf8ByteArray);
                        int currentIndex = start;
                        char currentChar = text[currentIndex];
                        while (currentChar >= '0' && currentChar <= '9')
                        {
                            currentChar = text[currentIndex];
                            currentIndex++;
                        }
                        UInt16.TryParse(decodedText.Substring(start, currentIndex - start), NumberStyles.None, CultureInfo.InvariantCulture, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("safljasldkfjsldkj2\r\n\r\n")]
        [InlineData("HTTP 1.1 / GET http://example.com/index.php?test=26,etc=blah\r\n\r\n")]
        [InlineData("buffer buffer buffer buffer 26655\r\n")]
        private static void ByteArrayArbitraryLengthBufferToUShort(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            int start = text.IndexOf('2');
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');

            foreach (var iteration in Benchmark.Iterations)
            {
                ushort value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InternalParser.TryParseUInt16(utf8ByteArray, start, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
        }
        #endregion

        #region uint
        //[Benchmark] // Legacy benchmark
        [InlineData("2134567890")] // standard parse
        [InlineData("5000000000")] // basic overflow
        [InlineData("12819950000000")] // heavy overflow
        [InlineData("0231281995")] // leading zero
        [InlineData("00000000128")] // many leading zeroes
        [InlineData("-128")] // negative value handling for unsigned types
        [InlineData("4294967295")] // max value
        [InlineData("0")] // min value
        private static void BaselineUintParse(string text)
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                uint value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        UInt32.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("2134567890")] // standard parse
        [InlineData("5000000000")] // basic overflow
        [InlineData("12819950000000")] // heavy overflow
        [InlineData("0231281995")] // leading zero
        [InlineData("00000000128")] // many leading zeroes
        [InlineData("-128")] // negative value handling for unsigned types
        [InlineData("4294967295")] // max value
        [InlineData("0")] // min value
        private static void BaselineArrayUintParse(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            foreach (var iteration in Benchmark.Iterations)
            {
                uint value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        string decodedText = UtfDecode(utf8ByteArray);
                        UInt32.TryParse(decodedText, NumberStyles.None, CultureInfo.InvariantCulture, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("2134567890")] // standard parse
        [InlineData("5000000000")] // basic overflow
        [InlineData("12819950000000")] // heavy overflow
        [InlineData("0231281995")] // leading zero
        [InlineData("00000000128")] // many leading zeroes
        [InlineData("-128")] // negative value handling for unsigned types
        [InlineData("4294967295")] // max value
        [InlineData("0")] // min value
        private static void ByteArrayToUint(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            foreach (var iteration in Benchmark.Iterations)
            {
                uint value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InternalParser.TryParseUInt32(utf8ByteArray, 0, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("2134567890")] // standard parse
        [InlineData("5000000000")] // basic overflow
        [InlineData("12819950000000")] // heavy overflow
        [InlineData("0231281995")] // leading zero
        [InlineData("00000000128")] // many leading zeroes
        [InlineData("-128")] // negative value handling for unsigned types
        [InlineData("4294967295")] // max value
        [InlineData("0")] // min value
        private unsafe static void ByteStarToUint(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            foreach (var iteration in Benchmark.Iterations)
            {
                uint value;
                int bytesConsumed;
                fixed (byte* utf8ByteStar = utf8ByteArray)
                {
                    using (iteration.StartMeasurement())
                    {
                        for (int i = 0; i < LOAD_ITERATIONS; i++)
                        {
                            InternalParser.TryParseUInt32(utf8ByteStar, 0, length, nf, fd, out value, out bytesConsumed);
                        }
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("2134567890")] // standard parse
        [InlineData("5000000000")] // basic overflow
        [InlineData("12819950000000")] // heavy overflow
        [InlineData("0231281995")] // leading zero
        [InlineData("00000000128")] // many leading zeroes
        [InlineData("-128")] // negative value handling for unsigned types
        [InlineData("4294967295")] // max value
        [InlineData("0")] // min value
        private unsafe static void ByteStarUnmanagedToUint(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            byte* unmanagedBytePtr;
            unmanagedBytePtr = (byte*)Marshal.AllocHGlobal(utf8ByteArray.Length);
            Marshal.Copy(utf8ByteArray, 0, (IntPtr)unmanagedBytePtr, utf8ByteArray.Length);
            foreach (var iteration in Benchmark.Iterations)
            {
                uint value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InternalParser.TryParseUInt32(unmanagedBytePtr, 0, length, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
            Marshal.FreeHGlobal((IntPtr)unmanagedBytePtr);
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("safljasldkfjsldkj2\r\n\r\n")]
        [InlineData("HTTP 1.1 / GET http://example.com/index.php?test=26,etc=blah\r\n\r\n")]
        [InlineData("buffer buffer buffer buffer 26655\r\n")]
        [InlineData("this is definitely 20000021% a buffer")]
        private static void BaselineArbitraryLengthBufferToUint(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            int start = text.IndexOf('2');

            foreach (var iteration in Benchmark.Iterations)
            {
                uint value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        string decodedText = UtfDecode(utf8ByteArray);
                        int currentIndex = start;
                        char currentChar = text[currentIndex];
                        while (currentChar >= '0' && currentChar <= '9')
                        {
                            currentChar = text[currentIndex];
                            currentIndex++;
                        }
                        UInt32.TryParse(decodedText.Substring(start, currentIndex - start), NumberStyles.None, CultureInfo.InvariantCulture, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("safljasldkfjsldkj2\r\n\r\n")]
        [InlineData("HTTP 1.1 / GET http://example.com/index.php?test=26,etc=blah\r\n\r\n")]
        [InlineData("buffer buffer buffer buffer 26655\r\n")]
        [InlineData("this is definitely 20000021% a buffer")]
        private static void ByteArrayArbitraryLengthBufferToUint(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            int start = text.IndexOf('2');
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');

            foreach (var iteration in Benchmark.Iterations)
            {
                uint value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InternalParser.TryParseUInt32(utf8ByteArray, start, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
        }
        #endregion

        #region ulong
        //[Benchmark] // Legacy benchmark
        [InlineData("9446744073709551615")] // standard parse
        [InlineData("50000000000000000000")] // basic overflow
        [InlineData("1281995128199512819950000000")] // heavy overflow
        [InlineData("09446744073709551615")] // leading zero
        [InlineData("00000000128")] // many leading zeroes
        [InlineData("-128")] // negative value handling for unsigned types
        [InlineData("18446744073709551615")] // max value
        [InlineData("0")] // min value
        private static void BaselineUlongParse(string text)
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                ulong value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        UInt64.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("9446744073709551615")] // standard parse
        [InlineData("50000000000000000000")] // basic overflow
        [InlineData("1281995128199512819950000000")] // heavy overflow
        [InlineData("09446744073709551615")] // leading zero
        [InlineData("00000000128")] // many leading zeroes
        [InlineData("-128")] // negative value handling for unsigned types
        [InlineData("18446744073709551615")] // max value
        [InlineData("0")] // min value
        private static void BaselineArrayUlongParse(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            foreach (var iteration in Benchmark.Iterations)
            {
                ulong value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        string decodedText = UtfDecode(utf8ByteArray);
                        UInt64.TryParse(decodedText, NumberStyles.None, CultureInfo.InvariantCulture, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("9446744073709551615")] // standard parse
        [InlineData("50000000000000000000")] // basic overflow
        [InlineData("1281995128199512819950000000")] // heavy overflow
        [InlineData("09446744073709551615")] // leading zero
        [InlineData("00000000128")] // many leading zeroes
        [InlineData("-128")] // negative value handling for unsigned types
        [InlineData("18446744073709551615")] // max value
        [InlineData("0")] // min value
        private static void ByteArrayToUlong(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            ulong value;
            foreach (var iteration in Benchmark.Iterations)
            {
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InternalParser.TryParseUInt64(utf8ByteArray, 0, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("9446744073709551615")] // standard parse
        [InlineData("50000000000000000000")] // basic overflow
        [InlineData("1281995128199512819950000000")] // heavy overflow
        [InlineData("09446744073709551615")] // leading zero
        [InlineData("00000000128")] // many leading zeroes
        [InlineData("-128")] // negative value handling for unsigned types
        [InlineData("18446744073709551615")] // max value
        [InlineData("0")] // min value
        private unsafe static void ByteStarToUlong(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            foreach (var iteration in Benchmark.Iterations)
            {
                ulong value;
                int bytesConsumed;
                fixed (byte* utf8ByteStar = utf8ByteArray)
                {
                    using (iteration.StartMeasurement())
                    {
                        for (int i = 0; i < LOAD_ITERATIONS; i++)
                        {
                            InternalParser.TryParseUInt64(utf8ByteStar, 0, length, nf, fd, out value, out bytesConsumed);
                        }
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("9446744073709551615")] // standard parse
        [InlineData("50000000000000000000")] // basic overflow
        [InlineData("1281995128199512819950000000")] // heavy overflow
        [InlineData("09446744073709551615")] // leading zero
        [InlineData("00000000128")] // many leading zeroes
        [InlineData("-128")] // negative value handling for unsigned types
        [InlineData("18446744073709551615")] // max value
        [InlineData("0")] // min value
        private unsafe static void ByteStarUnmanagedToUlong(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            byte* unmanagedBytePtr;
            unmanagedBytePtr = (byte*)Marshal.AllocHGlobal(utf8ByteArray.Length);
            Marshal.Copy(utf8ByteArray, 0, (IntPtr)unmanagedBytePtr, utf8ByteArray.Length);
            foreach (var iteration in Benchmark.Iterations)
            {
                ulong value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InternalParser.TryParseUInt64(unmanagedBytePtr, 0, length, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
            Marshal.FreeHGlobal((IntPtr)unmanagedBytePtr);
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("safljasldkfjsldkj2\r\n\r\n")]
        [InlineData("HTTP 1.1 / GET http://example.com/index.php?test=26,etc=blah\r\n\r\n")]
        [InlineData("buffer buffer buffer buffer 26655\r\n")]
        [InlineData("this is definitely 20000021% a buffer")]
        [InlineData("HTTP 1.1 / POST http://example.org/form some stuff 281203218485 Four Four Eight\r\n\r\n")]
        private static void BaselineArbitraryLengthBufferToUlong(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            int start = text.IndexOf('2');

            foreach (var iteration in Benchmark.Iterations)
            {
                ulong value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        string decodedText = UtfDecode(utf8ByteArray);
                        int currentIndex = start;
                        char currentChar = text[currentIndex];
                        while (currentChar >= '0' && currentChar <= '9')
                        {
                            currentChar = text[currentIndex];
                            currentIndex++;
                        }
                        UInt64.TryParse(decodedText.Substring(start, currentIndex - start), NumberStyles.None, CultureInfo.InvariantCulture, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("safljasldkfjsldkj2\r\n\r\n")]
        [InlineData("HTTP 1.1 / GET http://example.com/index.php?test=26,etc=blah\r\n\r\n")]
        [InlineData("buffer buffer buffer buffer 26655\r\n")]
        [InlineData("this is definitely 20000021% a buffer")]
        [InlineData("HTTP 1.1 / POST http://example.org/form some stuff 281203218485 Four Four Eight\r\n\r\n")]
        private static void ByteArrayArbitraryLengthBufferToUlong(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            int start = text.IndexOf('2');

            foreach (var iteration in Benchmark.Iterations)
            {
                ulong value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InternalParser.TryParseUInt64(utf8ByteArray, start, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
        }
        #endregion

        #region sbyte
        //[Benchmark] // Legacy benchmark
        [InlineData("65")] // standard parse
        [InlineData("-65")] // standard negative parse
        [InlineData("+65")] // explicit positive parse
        [InlineData("128")] // +1 overflow
        [InlineData("500")] // basic overflow
        [InlineData("1281995")] // heavy overflow
        [InlineData("0127")] // leading zero
        [InlineData("00000000120")] // many leading zeroes
        [InlineData("0")] // zero
        [InlineData("127")] // max value
        [InlineData("-128")] // min value
        private static void BaselineSbyteParse(string text)
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                sbyte value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        SByte.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("65")] // standard parse
        [InlineData("-65")] // standard negative parse
        [InlineData("+65")] // explicit positive parse
        [InlineData("128")] // +1 overflow
        [InlineData("500")] // basic overflow
        [InlineData("1281995")] // heavy overflow
        [InlineData("0127")] // leading zero
        [InlineData("00000000120")] // many leading zeroes
        [InlineData("0")] // zero
        [InlineData("127")] // max value
        [InlineData("-128")] // min value
        private static void BaselineArraySbyteParse(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            foreach (var iteration in Benchmark.Iterations)
            {
                sbyte value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        string decodedText = UtfDecode(utf8ByteArray);
                        SByte.TryParse(decodedText, NumberStyles.None, CultureInfo.InvariantCulture, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("65")] // standard parse
        [InlineData("-65")] // standard negative parse
        [InlineData("+65")] // explicit positive parse
        [InlineData("128")] // +1 overflow
        [InlineData("500")] // basic overflow
        [InlineData("1281995")] // heavy overflow
        [InlineData("0127")] // leading zero
        [InlineData("00000000120")] // many leading zeroes
        [InlineData("0")] // zero
        [InlineData("127")] // max value
        [InlineData("-128")] // min value
        private static void ByteArrayToSbyte(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            foreach (var iteration in Benchmark.Iterations)
            {
                sbyte value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InternalParser.TryParseSByte(utf8ByteArray, 0, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("65")] // standard parse
        [InlineData("-65")] // standard negative parse
        [InlineData("+65")] // explicit positive parse
        [InlineData("128")] // +1 overflow
        [InlineData("500")] // basic overflow
        [InlineData("1281995")] // heavy overflow
        [InlineData("0127")] // leading zero
        [InlineData("00000000120")] // many leading zeroes
        [InlineData("0")] // zero
        [InlineData("127")] // max value
        [InlineData("-128")] // min value
        private unsafe static void ByteStarToSbyte(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            foreach (var iteration in Benchmark.Iterations)
            {
                sbyte value;
                int bytesConsumed;
                fixed (byte* utf8ByteStar = utf8ByteArray)
                {
                    using (iteration.StartMeasurement())
                    {
                        for (int i = 0; i < LOAD_ITERATIONS; i++)
                        {
                            InternalParser.TryParseSByte(utf8ByteStar, 0, length, nf, fd, out value, out bytesConsumed);
                        }
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("65")] // standard parse
        [InlineData("-65")] // standard negative parse
        [InlineData("+65")] // explicit positive parse
        [InlineData("128")] // +1 overflow
        [InlineData("500")] // basic overflow
        [InlineData("1281995")] // heavy overflow
        [InlineData("0127")] // leading zero
        [InlineData("00000000120")] // many leading zeroes
        [InlineData("0")] // zero
        [InlineData("127")] // max value
        [InlineData("-128")] // min value
        private unsafe static void ByteStarUnmanagedToSbyte(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            byte* unmanagedBytePtr;
            unmanagedBytePtr = (byte*)Marshal.AllocHGlobal(utf8ByteArray.Length);
            Marshal.Copy(utf8ByteArray, 0, (IntPtr)unmanagedBytePtr, utf8ByteArray.Length);
            foreach (var iteration in Benchmark.Iterations)
            {
                sbyte value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InternalParser.TryParseSByte(unmanagedBytePtr, 0, length, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
            Marshal.FreeHGlobal((IntPtr)unmanagedBytePtr);
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("safljasldkfjsldkj2\r\n\r\n")]
        [InlineData("HTTP 1.1 / GET http://example.com/index.php?test=26,etc=blah\r\n\r\n")]
        [InlineData("HTTP 1.1 / UPDATE -29\r\n\r\n")]
        private static void BaselineArbitraryLengthBufferToSbyte(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            int start = text.IndexOf('2');
            if (text[start - 1] == '-')
                start -= 1;

            foreach (var iteration in Benchmark.Iterations)
            {
                long value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        string decodedText = UtfDecode(utf8ByteArray);
                        int currentIndex = start;
                        char currentChar = text[currentIndex];
                        while (currentChar >= '0' && currentChar <= '9')
                        {
                            currentChar = text[currentIndex];
                            currentIndex++;
                        }
                        Int64.TryParse(decodedText.Substring(start, currentIndex - start), NumberStyles.None, CultureInfo.InvariantCulture, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("safljasldkfjsldkj2\r\n\r\n")]
        [InlineData("HTTP 1.1 / GET http://example.com/index.php?test=26,etc=blah\r\n\r\n")]
        [InlineData("HTTP 1.1 / UPDATE -29\r\n\r\n")]
        private static void ByteArrayArbitraryLengthBufferToSbyte(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            int start = text.IndexOf('2');
            if (text[start - 1] == '-')
                start -= 1;
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');

            foreach (var iteration in Benchmark.Iterations)
            {
                sbyte value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InternalParser.TryParseSByte(utf8ByteArray, start, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
        }
        #endregion

        #region short
        //[Benchmark] // Legacy benchmark
        [InlineData("16000")] // standard parse
        [InlineData("-16000")] // standard negative parse
        [InlineData("+16000")] // explicit positive parse
        [InlineData("32768")] // +1 overflow
        [InlineData("50000")] // basic overflow
        [InlineData("12819951995")] // heavy overflow
        [InlineData("0127")] // leading zero
        [InlineData("00000000120")] // many leading zeroes
        [InlineData("0")] // zero
        [InlineData("32767")] // min value
        [InlineData("-32768")] // min value
        private static void BaselineShortParse(string text)
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                short value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        Int16.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("16000")] // standard parse
        [InlineData("-16000")] // standard negative parse
        [InlineData("+16000")] // explicit positive parse
        [InlineData("32768")] // +1 overflow
        [InlineData("50000")] // basic overflow
        [InlineData("12819951995")] // heavy overflow
        [InlineData("0127")] // leading zero
        [InlineData("00000000120")] // many leading zeroes
        [InlineData("0")] // zero
        [InlineData("32767")] // min value
        [InlineData("-32768")] // min value
        private static void BaselineArrayShortParse(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            foreach (var iteration in Benchmark.Iterations)
            {
                short value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        string decodedText = UtfDecode(utf8ByteArray);
                        Int16.TryParse(decodedText, NumberStyles.None, CultureInfo.InvariantCulture, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("16000")] // standard parse
        [InlineData("-16000")] // standard negative parse
        [InlineData("+16000")] // explicit positive parse
        [InlineData("32768")] // +1 overflow
        [InlineData("50000")] // basic overflow
        [InlineData("12819951995")] // heavy overflow
        [InlineData("0127")] // leading zero
        [InlineData("00000000120")] // many leading zeroes
        [InlineData("0")] // zero
        [InlineData("32767")] // min value
        [InlineData("-32768")] // min value
        private static void ByteArrayToShort(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            foreach (var iteration in Benchmark.Iterations)
            {
                short value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InternalParser.TryParseInt16(utf8ByteArray, 0, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("16000")] // standard parse
        [InlineData("-16000")] // standard negative parse
        [InlineData("+16000")] // explicit positive parse
        [InlineData("32768")] // +1 overflow
        [InlineData("50000")] // basic overflow
        [InlineData("12819951995")] // heavy overflow
        [InlineData("0127")] // leading zero
        [InlineData("00000000120")] // many leading zeroes
        [InlineData("0")] // zero
        [InlineData("32767")] // min value
        [InlineData("-32768")] // min value
        private unsafe static void ByteStarToShort(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            foreach (var iteration in Benchmark.Iterations)
            {
                short value;
                int bytesConsumed;
                fixed (byte* utf8ByteStar = utf8ByteArray)
                {
                    using (iteration.StartMeasurement())
                    {
                        for (int i = 0; i < LOAD_ITERATIONS; i++)
                        {
                            InternalParser.TryParseInt16(utf8ByteStar, 0, length, nf, fd, out value, out bytesConsumed);
                        }
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("16000")] // standard parse
        [InlineData("-16000")] // standard negative parse
        [InlineData("+16000")] // explicit positive parse
        [InlineData("32768")] // +1 overflow
        [InlineData("50000")] // basic overflow
        [InlineData("12819951995")] // heavy overflow
        [InlineData("0127")] // leading zero
        [InlineData("00000000120")] // many leading zeroes
        [InlineData("0")] // zero
        [InlineData("32767")] // min value
        [InlineData("-32768")] // min value
        private unsafe static void ByteStarUnmanagedToShort(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            byte* unmanagedBytePtr;
            unmanagedBytePtr = (byte*)Marshal.AllocHGlobal(utf8ByteArray.Length);
            Marshal.Copy(utf8ByteArray, 0, (IntPtr)unmanagedBytePtr, utf8ByteArray.Length);
            foreach (var iteration in Benchmark.Iterations)
            {
                short value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InternalParser.TryParseInt16(unmanagedBytePtr, 0, length, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
            Marshal.FreeHGlobal((IntPtr)unmanagedBytePtr);
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("safljasldkfjsldkj2\r\n\r\n")]
        [InlineData("HTTP 1.1 / GET http://example.com/index.php?test=26,etc=blah\r\n\r\n")]
        [InlineData("buffer buffer buffer buffer 26655\r\n")]
        [InlineData("HTTP 1.1 / UPDATE -29\r\n\r\n")]
        [InlineData("HTTP 1.1 / UPDATE -20654\r\n\r\n")]
        private static void BaselineArbitraryLengthBufferToShort(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            int start = text.IndexOf('2');
            if (text[start - 1] == '-')
                start -= 1;

            foreach (var iteration in Benchmark.Iterations)
            {
                long value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        string decodedText = UtfDecode(utf8ByteArray);
                        int currentIndex = start;
                        char currentChar = text[currentIndex];
                        while (currentChar >= '0' && currentChar <= '9')
                        {
                            currentChar = text[currentIndex];
                            currentIndex++;
                        }
                        Int64.TryParse(decodedText.Substring(start, currentIndex - start), NumberStyles.None, CultureInfo.InvariantCulture, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("safljasldkfjsldkj2\r\n\r\n")]
        [InlineData("HTTP 1.1 / GET http://example.com/index.php?test=26,etc=blah\r\n\r\n")]
        [InlineData("buffer buffer buffer buffer 26655\r\n")]
        [InlineData("HTTP 1.1 / UPDATE -29\r\n\r\n")]
        [InlineData("HTTP 1.1 / UPDATE -20654\r\n\r\n")]
        private static void ByteArrayArbitraryLengthBufferToShort(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            int start = text.IndexOf('2');
            if (text[start - 1] == '-')
                start -= 1;
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');

            foreach (var iteration in Benchmark.Iterations)
            {
                short value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InternalParser.TryParseInt16(utf8ByteArray, start, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
        }
        #endregion

        #region int
        //[Benchmark] // Legacy benchmark
        [InlineData("1147483647")] // standard parse
        [InlineData("-1147483647")] // standard negative parse
        [InlineData("+1147483647")] // explicit positive parse
        [InlineData("2147483648")] // +1 overflow
        [InlineData("5000000000")] // basic overflow
        [InlineData("128199519951281995")] // heavy overflow
        [InlineData("0127")] // leading zero
        [InlineData("00000000120")] // many leading zeroes
        [InlineData("0")] // zero
        [InlineData("2147483647")] // max value
        [InlineData("-2147483648")] // min value
        private static void BaselineIntParse(string text)
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                int value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        Int32.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("1147483647")] // standard parse
        [InlineData("-1147483647")] // standard negative parse
        [InlineData("+1147483647")] // explicit positive parse
        [InlineData("2147483648")] // +1 overflow
        [InlineData("5000000000")] // basic overflow
        [InlineData("128199519951281995")] // heavy overflow
        [InlineData("0127")] // leading zero
        [InlineData("00000000120")] // many leading zeroes
        [InlineData("0")] // zero
        [InlineData("2147483647")] // max value
        [InlineData("-2147483648")] // min value
        private static void BaselineArrayIntParse(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            foreach (var iteration in Benchmark.Iterations)
            {
                int value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        string decodedText = UtfDecode(utf8ByteArray);
                        Int32.TryParse(decodedText, NumberStyles.None, CultureInfo.InvariantCulture, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("1147483647")] // standard parse
        [InlineData("-1147483647")] // standard negative parse
        [InlineData("+1147483647")] // explicit positive parse
        [InlineData("2147483648")] // +1 overflow
        [InlineData("5000000000")] // basic overflow
        [InlineData("128199519951281995")] // heavy overflow
        [InlineData("0127")] // leading zero
        [InlineData("00000000120")] // many leading zeroes
        [InlineData("0")] // zero
        [InlineData("2147483647")] // max value
        [InlineData("-2147483648")] // min value
        private static void ByteArrayToInt(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            foreach (var iteration in Benchmark.Iterations)
            {
                int value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InternalParser.TryParseInt32(utf8ByteArray, 0, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("1147483647")] // standard parse
        [InlineData("-1147483647")] // standard negative parse
        [InlineData("+1147483647")] // explicit positive parse
        [InlineData("2147483648")] // +1 overflow
        [InlineData("5000000000")] // basic overflow
        [InlineData("128199519951281995")] // heavy overflow
        [InlineData("0127")] // leading zero
        [InlineData("00000000120")] // many leading zeroes
        [InlineData("0")] // zero
        [InlineData("2147483647")] // max value
        [InlineData("-2147483648")] // min value
        private unsafe static void ByteStarToInt(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            foreach (var iteration in Benchmark.Iterations)
            {
                int value;
                int bytesConsumed;
                fixed (byte* utf8ByteStar = utf8ByteArray)
                {
                    using (iteration.StartMeasurement())
                    {
                        for (int i = 0; i < LOAD_ITERATIONS; i++)
                        {
                            InternalParser.TryParseInt32(utf8ByteStar, 0, length, nf, fd, out value, out bytesConsumed);
                        }
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("1147483647")] // standard parse
        [InlineData("-1147483647")] // standard negative parse
        [InlineData("+1147483647")] // explicit positive parse
        [InlineData("2147483648")] // +1 overflow
        [InlineData("5000000000")] // basic overflow
        [InlineData("128199519951281995")] // heavy overflow
        [InlineData("0127")] // leading zero
        [InlineData("00000000120")] // many leading zeroes
        [InlineData("0")] // zero
        [InlineData("2147483647")] // max value
        [InlineData("-2147483648")] // min value
        private unsafe static void ByteStarUnmanagedToInt(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = UtfEncode(text);
            byte* unmanagedBytePtr;
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            unmanagedBytePtr = (byte*)Marshal.AllocHGlobal(utf8ByteArray.Length);
            Marshal.Copy(utf8ByteArray, 0, (IntPtr)unmanagedBytePtr, utf8ByteArray.Length);
            foreach (var iteration in Benchmark.Iterations)
            {
                int value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InternalParser.TryParseInt32(unmanagedBytePtr, 0, length, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
            Marshal.FreeHGlobal((IntPtr)unmanagedBytePtr);
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("safljasldkfjsldkj2\r\n\r\n")]
        [InlineData("HTTP 1.1 / GET http://example.com/index.php?test=26,etc=blah\r\n\r\n")]
        [InlineData("buffer buffer buffer buffer 26655\r\n")]
        [InlineData("this is definitely 20000021% a buffer")]
        [InlineData("HTTP 1.1 / UPDATE -29\r\n\r\n")]
        [InlineData("HTTP 1.1 / UPDATE -20654\r\n\r\n")]
        private static void BaselineArbitraryLengthBufferToInt(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            int start = text.IndexOf('2');
            if (text[start - 1] == '-')
                start -= 1;

            foreach (var iteration in Benchmark.Iterations)
            {
                long value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        string decodedText = UtfDecode(utf8ByteArray);
                        int currentIndex = start;
                        char currentChar = text[currentIndex];
                        while (currentChar >= '0' && currentChar <= '9')
                        {
                            currentChar = text[currentIndex];
                            currentIndex++;
                        }
                        Int64.TryParse(decodedText.Substring(start, currentIndex - start), NumberStyles.None, CultureInfo.InvariantCulture, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("safljasldkfjsldkj2\r\n\r\n")]
        [InlineData("HTTP 1.1 / GET http://example.com/index.php?test=26,etc=blah\r\n\r\n")]
        [InlineData("buffer buffer buffer buffer 26655\r\n")]
        [InlineData("this is definitely 20000021% a buffer")]
        [InlineData("HTTP 1.1 / UPDATE -29\r\n\r\n")]
        [InlineData("HTTP 1.1 / UPDATE -20654\r\n\r\n")]
        private static void ByteArrayArbitraryLengthBufferToInt(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            int start = text.IndexOf('2');
            if (text[start - 1] == '-')
                start -= 1;
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');

            foreach (var iteration in Benchmark.Iterations)
            {
                int value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InternalParser.TryParseInt32(utf8ByteArray, start, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
        }
        #endregion

        #region long
        //[Benchmark] // Legacy benchmark
        [InlineData("4223372036854775807")] // standard parse
        [InlineData("-4223372036854775807")] // standard negative parse
        [InlineData("+4223372036854775807")] // explicit positive parse
        [InlineData("9223372036854775808")] // +1 overflow
        [InlineData("50000000000000000000")] // basic overflow
        [InlineData("12819951995128199512819951995128")] // heavy overflow
        [InlineData("0127")] // leading zero
        [InlineData("00000000120")] // many leading zeroes
        [InlineData("0")] // zero
        [InlineData("9223372036854775807")] // max value
        [InlineData("-9223372036854775808")] // min value
        private static void BaselineLongParse(string text)
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                long value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        Int64.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("4223372036854775807")] // standard parse
        [InlineData("-4223372036854775807")] // standard negative parse
        [InlineData("+4223372036854775807")] // explicit positive parse
        [InlineData("9223372036854775808")] // +1 overflow
        [InlineData("50000000000000000000")] // basic overflow
        [InlineData("12819951995128199512819951995128")] // heavy overflow
        [InlineData("0127")] // leading zero
        [InlineData("00000000120")] // many leading zeroes
        [InlineData("0")] // zero
        [InlineData("9223372036854775807")] // max value
        [InlineData("-9223372036854775808")] // min value
        private static void BaselineArrayLongParse(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            foreach (var iteration in Benchmark.Iterations)
            {
                long value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        string decodedText = UtfDecode(utf8ByteArray);
                        Int64.TryParse(decodedText, NumberStyles.None, CultureInfo.InvariantCulture, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("4223372036854775807")] // standard parse
        [InlineData("-4223372036854775807")] // standard negative parse
        [InlineData("+4223372036854775807")] // explicit positive parse
        [InlineData("9223372036854775808")] // +1 overflow
        [InlineData("50000000000000000000")] // basic overflow
        [InlineData("12819951995128199512819951995128")] // heavy overflow
        [InlineData("0127")] // leading zero
        [InlineData("00000000120")] // many leading zeroes
        [InlineData("0")] // zero
        [InlineData("9223372036854775807")] // max value
        [InlineData("-9223372036854775808")] // min value
        private static void ByteArrayToLong(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            foreach (var iteration in Benchmark.Iterations)
            {
                long value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InternalParser.TryParseInt64(utf8ByteArray, 0, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("4223372036854775807")] // standard parse
        [InlineData("-4223372036854775807")] // standard negative parse
        [InlineData("+4223372036854775807")] // explicit positive parse
        [InlineData("9223372036854775808")] // +1 overflow
        [InlineData("50000000000000000000")] // basic overflow
        [InlineData("12819951995128199512819951995128")] // heavy overflow
        [InlineData("0127")] // leading zero
        [InlineData("00000000120")] // many leading zeroes
        [InlineData("0")] // zero
        [InlineData("9223372036854775807")] // max value
        [InlineData("-9223372036854775808")] // min value
        private unsafe static void ByteStarToLong(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            foreach (var iteration in Benchmark.Iterations)
            {
                long value;
                int bytesConsumed;
                fixed (byte* utf8ByteStar = utf8ByteArray)
                {
                    using (iteration.StartMeasurement())
                    {
                        for (int i = 0; i < LOAD_ITERATIONS; i++)
                        {
                            InternalParser.TryParseInt64(utf8ByteStar, 0, length, nf, fd, out value, out bytesConsumed);
                        }
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("4223372036854775807")] // standard parse
        [InlineData("-4223372036854775807")] // standard negative parse
        [InlineData("+4223372036854775807")] // explicit positive parse
        [InlineData("9223372036854775808")] // +1 overflow
        [InlineData("50000000000000000000")] // basic overflow
        [InlineData("12819951995128199512819951995128")] // heavy overflow
        [InlineData("0127")] // leading zero
        [InlineData("00000000120")] // many leading zeroes
        [InlineData("0")] // zero
        [InlineData("9223372036854775807")] // max value
        [InlineData("-9223372036854775808")] // min value
        private unsafe static void ByteStarUnmanagedToLong(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            byte* unmanagedBytePtr;
            unmanagedBytePtr = (byte*)Marshal.AllocHGlobal(utf8ByteArray.Length);
            Marshal.Copy(utf8ByteArray, 0, (IntPtr)unmanagedBytePtr, utf8ByteArray.Length);
            foreach (var iteration in Benchmark.Iterations)
            {
                long value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InternalParser.TryParseInt64(unmanagedBytePtr, 0, length, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
            Marshal.FreeHGlobal((IntPtr)unmanagedBytePtr);
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("safljasldkfjsldkj2\r\n\r\n")]
        [InlineData("HTTP 1.1 / GET http://example.com/index.php?test=26,etc=blah\r\n\r\n")]
        [InlineData("buffer buffer buffer buffer 26655\r\n")]
        [InlineData("this is definitely 20000021% a buffer")]
        [InlineData("HTTP 1.1 / POST http://example.org/form some stuff 281203218485 Four Four Eight\r\n\r\n")]
        [InlineData("HTTP 1.1 / UPDATE -29\r\n\r\n")]
        [InlineData("HTTP 1.1 / UPDATE -20654\r\n\r\n")]
        private static void BaselineArbitraryLengthBufferToLong(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            int start = text.IndexOf('2');
            if (text[start - 1] == '-')
                start -= 1;

            foreach (var iteration in Benchmark.Iterations)
            {
                long value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        string decodedText = UtfDecode(utf8ByteArray);
                        int currentIndex = start;
                        char currentChar = text[currentIndex];
                        while (currentChar >= '0' && currentChar <= '9')
                        {
                            currentChar = text[currentIndex];
                            currentIndex++;
                        }
                        Int64.TryParse(decodedText.Substring(start, currentIndex - start), NumberStyles.None, CultureInfo.InvariantCulture, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("safljasldkfjsldkj2\r\n\r\n")]
        [InlineData("HTTP 1.1 / GET http://example.com/index.php?test=26,etc=blah\r\n\r\n")]
        [InlineData("buffer buffer buffer buffer 26655\r\n")]
        [InlineData("this is definitely 20000021% a buffer")]
        [InlineData("HTTP 1.1 / POST http://example.org/form some stuff 281203218485 Four Four Eight\r\n\r\n")]
        [InlineData("HTTP 1.1 / UPDATE -29\r\n\r\n")]
        [InlineData("HTTP 1.1 / UPDATE -20654\r\n\r\n")]
        private static void ByteArrayArbitraryLengthToLong(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            int start = text.IndexOf('2');
            if (text[start - 1] == '-')
                start -= 1;
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');

            foreach (var iteration in Benchmark.Iterations)
            {
                long value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InternalParser.TryParseInt64(utf8ByteArray, start, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
        }
        #endregion

        #region bool
        //[Benchmark] // Legacy benchmark
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("true")]
        [InlineData("TRUE")]
        [InlineData("True")]
        [InlineData("false")]
        [InlineData("FALSE")]
        [InlineData("False")]
        [InlineData("SuperpositionofTrueandFalse")]
        private static void BaselineBoolParse(string text)
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                bool value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        Boolean.TryParse(text, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("true")]
        [InlineData("TRUE")]
        [InlineData("True")]
        [InlineData("false")]
        [InlineData("FALSE")]
        [InlineData("False")]
        [InlineData("SuperpositionofTrueandFalse")]
        private static void BaselineArrayBoolParse(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            foreach (var iteration in Benchmark.Iterations)
            {
                bool value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        string decodedText = UtfDecode(utf8ByteArray);
                        Boolean.TryParse(decodedText, out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("true")]
        [InlineData("TRUE")]
        [InlineData("True")]
        [InlineData("false")]
        [InlineData("FALSE")]
        [InlineData("False")]
        [InlineData("SuperpositionofTrueandFalse")]
        private static void ByteArrayToBool(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            foreach (var iteration in Benchmark.Iterations)
            {
                bool value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        Internal.InternalParser.TryParseBoolean(utf8ByteArray, 0, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("true")]
        [InlineData("TRUE")]
        [InlineData("True")]
        [InlineData("false")]
        [InlineData("FALSE")]
        [InlineData("False")]
        [InlineData("SuperpositionofTrueandFalse")]
        private unsafe static void ByteStarToBool(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            foreach (var iteration in Benchmark.Iterations)
            {
                bool value;
                int bytesConsumed;
                fixed (byte* utf8ByteStar = utf8ByteArray)
                {
                    using (iteration.StartMeasurement())
                    {
                        for (int i = 0; i < LOAD_ITERATIONS; i++)
                        {
                            Internal.InternalParser.TryParseBoolean(utf8ByteStar, 0, length, fd, nf, out value, out bytesConsumed);
                        }
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("true")]
        [InlineData("TRUE")]
        [InlineData("True")]
        [InlineData("false")]
        [InlineData("FALSE")]
        [InlineData("False")]
        [InlineData("SuperpositionofTrueandFalse")]
        private unsafe static void ByteStarUnmanagedToBool(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = UtfEncode(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            byte* unmanagedBytePtr;
            unmanagedBytePtr = (byte*)Marshal.AllocHGlobal(utf8ByteArray.Length);
            Marshal.Copy(utf8ByteArray, 0, (IntPtr)unmanagedBytePtr, utf8ByteArray.Length);
            foreach (var iteration in Benchmark.Iterations)
            {
                bool value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        Internal.InternalParser.TryParseBoolean(unmanagedBytePtr, 0, length, fd, nf, out value, out bytesConsumed);
                    }
                }
            }
            Marshal.FreeHGlobal((IntPtr)unmanagedBytePtr);
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("safljasldkfjsldkj=true\r\n\r\n")]
        [InlineData("HTTP 1.1 / GET http://example.com/index.php?test=FALSE,etc=blah\r\n\r\n")]
        [InlineData("buffer buffer buffer buffer =false\r\n")]
        [InlineData("this is definitely =true a buffer")]
        [InlineData("HTTP 1.1 / POST http://example.org/form some stuff this=FALSE Four Four Eight\r\n\r\n")]
        [InlineData("HTTP 1.1 / UPDATE fact=0\r\n\r\n")]
        [InlineData("HTTP 1.1 / UPDATE fact=1\r\n\r\n")]
        private static void BaselineArbitraryLengthBufferToBool(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            int start = text.IndexOf('=') + 1;

            foreach (var iteration in Benchmark.Iterations)
            {
                bool value;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        string decodedText = UtfDecode(utf8ByteArray);
                        int currentIndex = start;
                        char currentChar = text[currentIndex];
                        while (currentChar >= '0' && currentChar <= '9')
                        {
                            currentChar = text[currentIndex];
                            currentIndex++;
                        }
                        Boolean.TryParse(decodedText.Substring(start, currentIndex - start), out value);
                    }
                }
            }
        }

        //[Benchmark] // Legacy benchmark
        [InlineData("safljasldkfjsldkj=true\r\n\r\n")]
        [InlineData("HTTP 1.1 / GET http://example.com/index.php?test=FALSE,etc=blah\r\n\r\n")]
        [InlineData("buffer buffer buffer buffer =false\r\n")]
        [InlineData("this is definitely =true a buffer")]
        [InlineData("HTTP 1.1 / POST http://example.org/form some stuff this=FALSE Four Four Eight\r\n\r\n")]
        [InlineData("HTTP 1.1 / UPDATE fact=0\r\n\r\n")]
        [InlineData("HTTP 1.1 / UPDATE fact=1\r\n\r\n")]
        private static void ByteArrayArbitraryLengthToBool(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            int start = text.IndexOf('=') + 1;
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');

            foreach (var iteration in Benchmark.Iterations)
            {
                bool value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InternalParser.TryParseBoolean(utf8ByteArray, start, nf, fd, out value, out bytesConsumed);
                    }
                }
            }
        }
        #endregion
    }
}