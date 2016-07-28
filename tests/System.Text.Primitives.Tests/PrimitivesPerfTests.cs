using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Runtime.InteropServices;
using Xunit;
using Microsoft.Xunit.Performance;

namespace System.Text.Primitives.Tests
{
    public class PrimitivesPerfTests
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

        #region byte
        [Benchmark]
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

        [Benchmark]
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

        [Benchmark]
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
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InvariantParser.TryParse(utf8ByteArray, 0, out value, out bytesConsumed);
                    }
                }
            }
        }

        [Benchmark]
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
                            InvariantParser.TryParse(utf8ByteStar, 0, length, out value, out bytesConsumed);
                        }
                    }
                }
            }
        }

        [Benchmark]
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
                        InvariantParser.TryParse(unmanagedBytePtr, 0, length, out value, out bytesConsumed);
                    }
                }
            }
            Marshal.FreeHGlobal((IntPtr)unmanagedBytePtr);
        }

        [Benchmark]
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

        [Benchmark]
        [InlineData("safljasldkfjsldkj2\r\n\r\n")]
        [InlineData("HTTP 1.1 / GET http://example.com/index.php?test=26,etc=blah\r\n\r\n")]
        private static void ByteArrayArbitraryLengthBufferToByte(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            int start = text.IndexOf('2');

            foreach (var iteration in Benchmark.Iterations)
            {
                byte value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InvariantParser.TryParse(utf8ByteArray, start, out value, out bytesConsumed);
                    }
                }
            }
        }
        #endregion

        #region ushort
        [Benchmark]
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

        [Benchmark]
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

        [Benchmark]
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
            foreach (var iteration in Benchmark.Iterations)
            {
                ushort value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InvariantParser.TryParse(utf8ByteArray, 0, out value, out bytesConsumed);
                    }
                }
            }
        }

        [Benchmark]
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
                            InvariantParser.TryParse(utf8ByteStar, 0, length, out value, out bytesConsumed);
                        }
                    }
                }
            }
        }

        [Benchmark]
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
                        InvariantParser.TryParse(unmanagedBytePtr, 0, length, out value, out bytesConsumed);
                }
            }
            Marshal.FreeHGlobal((IntPtr)unmanagedBytePtr);
        }

        [Benchmark]
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

        [Benchmark]
        [InlineData("safljasldkfjsldkj2\r\n\r\n")]
        [InlineData("HTTP 1.1 / GET http://example.com/index.php?test=26,etc=blah\r\n\r\n")]
        [InlineData("buffer buffer buffer buffer 26655\r\n")]
        private static void ByteArrayArbitraryLengthBufferToUShort(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            int start = text.IndexOf('2');

            foreach (var iteration in Benchmark.Iterations)
            {
                ushort value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InvariantParser.TryParse(utf8ByteArray, start, out value, out bytesConsumed);
                    }
                }
            }
        }
        #endregion

        #region uint
        [Benchmark]
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

        [Benchmark]
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

        [Benchmark]
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
            foreach (var iteration in Benchmark.Iterations)
            {
                uint value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InvariantParser.TryParse(utf8ByteArray, 0, out value, out bytesConsumed);
                    }
                }
            }
        }

        [Benchmark]
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
                            InvariantParser.TryParse(utf8ByteStar, 0, length, out value, out bytesConsumed);
                        }
                    }
                }
            }
        }

        [Benchmark]
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
                        InvariantParser.TryParse(unmanagedBytePtr, 0, length, out value, out bytesConsumed);
                    }
                }
            }
            Marshal.FreeHGlobal((IntPtr)unmanagedBytePtr);
        }

        [Benchmark]
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

        [Benchmark]
        [InlineData("safljasldkfjsldkj2\r\n\r\n")]
        [InlineData("HTTP 1.1 / GET http://example.com/index.php?test=26,etc=blah\r\n\r\n")]
        [InlineData("buffer buffer buffer buffer 26655\r\n")]
        [InlineData("this is definitely 20000021% a buffer")]
        private static void ByteArrayArbitraryLengthBufferToUint(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            int start = text.IndexOf('2');

            foreach (var iteration in Benchmark.Iterations)
            {
                uint value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InvariantParser.TryParse(utf8ByteArray, start, out value, out bytesConsumed);
                    }
                }
            }
        }
        #endregion

        #region ulong
        [Benchmark]
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

        [Benchmark]
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

        [Benchmark]
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
            ulong value;
            foreach (var iteration in Benchmark.Iterations)
            {
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InvariantParser.TryParse(utf8ByteArray, 0, out value, out bytesConsumed);
                    }
                }
            }
        }

        [Benchmark]
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
                            InvariantParser.TryParse(utf8ByteStar, 0, length, out value, out bytesConsumed);
                        }
                    }
                }
            }
        }

        [Benchmark]
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
                        InvariantParser.TryParse(unmanagedBytePtr, 0, length, out value, out bytesConsumed);
                    }
                }
            }
            Marshal.FreeHGlobal((IntPtr)unmanagedBytePtr);
        }

        [Benchmark]
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

        [Benchmark]
        [InlineData("safljasldkfjsldkj2\r\n\r\n")]
        [InlineData("HTTP 1.1 / GET http://example.com/index.php?test=26,etc=blah\r\n\r\n")]
        [InlineData("buffer buffer buffer buffer 26655\r\n")]
        [InlineData("this is definitely 20000021% a buffer")]
        [InlineData("HTTP 1.1 / POST http://example.org/form some stuff 281203218485 Four Four Eight\r\n\r\n")]
        private static void ByteArrayArbitraryLengthBufferToUlong(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            int start = text.IndexOf('2');

            foreach (var iteration in Benchmark.Iterations)
            {
                ulong value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InvariantParser.TryParse(utf8ByteArray, start, out value, out bytesConsumed);
                    }
                }
            }
        }
        #endregion

        #region sbyte
        [Benchmark]
        [InlineData("65")] // standard parse
        [InlineData("-65")] // standard negative parse
        [InlineData("+65")] // explicit positive parse
        [InlineData("128")] // +1 overflow
        [InlineData("500")] // basic overflow
        [InlineData("1281995")] // heavy overflow
        [InlineData("0124")] // leading zero
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

        [Benchmark]
        [InlineData("65")] // standard parse
        [InlineData("-65")] // standard negative parse
        [InlineData("+65")] // explicit positive parse
        [InlineData("128")] // +1 overflow
        [InlineData("500")] // basic overflow
        [InlineData("1281995")] // heavy overflow
        [InlineData("0124")] // leading zero
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

        [Benchmark]
        [InlineData("65")] // standard parse
        [InlineData("-65")] // standard negative parse
        [InlineData("+65")] // explicit positive parse
        [InlineData("128")] // +1 overflow
        [InlineData("500")] // basic overflow
        [InlineData("1281995")] // heavy overflow
        [InlineData("0124")] // leading zero
        [InlineData("00000000120")] // many leading zeroes
        [InlineData("0")] // zero
        [InlineData("127")] // max value
        [InlineData("-128")] // min value
        private static void ByteArrayToSbyte(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            foreach (var iteration in Benchmark.Iterations)
            {
                sbyte value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InvariantParser.TryParse(utf8ByteArray, 0, out value, out bytesConsumed);
                    }
                }
            }
        }

        [Benchmark]
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
                            InvariantParser.TryParse(utf8ByteStar, 0, length, out value, out bytesConsumed);
                        }
                    }
                }
            }
        }

        [Benchmark]
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
                        InvariantParser.TryParse(unmanagedBytePtr, 0, length, out value, out bytesConsumed);
                    }
                }
            }
            Marshal.FreeHGlobal((IntPtr)unmanagedBytePtr);
        }

        [Benchmark]
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

        [Benchmark]
        [InlineData("safljasldkfjsldkj2\r\n\r\n")]
        [InlineData("HTTP 1.1 / GET http://example.com/index.php?test=26,etc=blah\r\n\r\n")]
        [InlineData("HTTP 1.1 / UPDATE -29\r\n\r\n")]
        private static void ByteArrayArbitraryLengthBufferToSbyte(string text)
        {
            byte[] utf8ByteArray = UtfEncode(text);
            int start = text.IndexOf('2');
            if (text[start - 1] == '-')
                start -= 1;

            foreach (var iteration in Benchmark.Iterations)
            {
                long value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InvariantParser.TryParse(utf8ByteArray, start, out value, out bytesConsumed);
                    }
                }
            }
        }
        #endregion

        #region short
        [Benchmark]
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

        [Benchmark]
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

        [Benchmark]
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
            foreach (var iteration in Benchmark.Iterations)
            {
                short value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InvariantParser.TryParse(utf8ByteArray, 0, out value, out bytesConsumed);
                    }
                }
            }
        }

        [Benchmark]
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
                            InvariantParser.TryParse(utf8ByteStar, 0, length, out value, out bytesConsumed);
                        }
                    }
                }
            }
        }

        [Benchmark]
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
                        InvariantParser.TryParse(unmanagedBytePtr, 0, length, out value, out bytesConsumed);
                    }
                }
            }
            Marshal.FreeHGlobal((IntPtr)unmanagedBytePtr);
        }

        [Benchmark]
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

        [Benchmark]
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

            foreach (var iteration in Benchmark.Iterations)
            {
                long value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InvariantParser.TryParse(utf8ByteArray, start, out value, out bytesConsumed);
                    }
                }
            }
        }
        #endregion

        #region int
        [Benchmark]
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

        [Benchmark]
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

        [Benchmark]
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
            foreach (var iteration in Benchmark.Iterations)
            {
                int value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InvariantParser.TryParse(utf8ByteArray, 0, out value, out bytesConsumed);
                    }
                }
            }
        }

        [Benchmark]
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
                            InvariantParser.TryParse(utf8ByteStar, 0, length, out value, out bytesConsumed);
                        }
                    }
                }
            }
        }

        [Benchmark]
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
                        InvariantParser.TryParse(unmanagedBytePtr, 0, length, out value, out bytesConsumed);
                    }
                }
            }
            Marshal.FreeHGlobal((IntPtr)unmanagedBytePtr);
        }

        [Benchmark]
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

        [Benchmark]
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

            foreach (var iteration in Benchmark.Iterations)
            {
                long value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InvariantParser.TryParse(utf8ByteArray, start, out value, out bytesConsumed);
                    }
                }
            }
        }
        #endregion

        #region long
        [Benchmark]
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

        [Benchmark]
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

        [Benchmark]
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
            foreach (var iteration in Benchmark.Iterations)
            {
                int value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InvariantParser.TryParse(utf8ByteArray, 0, out value, out bytesConsumed);
                    }
                }
            }
        }

        [Benchmark]
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
                            InvariantParser.TryParse(utf8ByteStar, 0, length, out value, out bytesConsumed);
                        }
                    }
                }
            }
        }

        [Benchmark]
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
                        InvariantParser.TryParse(unmanagedBytePtr, 0, length, out value, out bytesConsumed);
                    }
                }
            }
            Marshal.FreeHGlobal((IntPtr)unmanagedBytePtr);
        }

        [Benchmark]
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

        [Benchmark]
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

            foreach (var iteration in Benchmark.Iterations)
            {
                long value;
                int bytesConsumed;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        InvariantParser.TryParse(utf8ByteArray, start, out value, out bytesConsumed);
                    }
                }
            }
        }
        #endregion
    }
}