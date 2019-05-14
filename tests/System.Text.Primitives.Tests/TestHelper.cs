// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using Xunit;
using System.Runtime.InteropServices;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Buffers.Text;

namespace System.Text.Primitives.Tests
{
    public static class TestHelper
    {
        public const int LoadIterations = 30000;

        // Converts a hex string to binary.
        public static byte[] DecodeHex(string input)
        {
            int ParseNibble(char ch)
            {
                ch -= (char)'0';
                if (ch < 10) { return ch; }

                ch -= (char)('A' - '0');
                if (ch < 6) { return (ch + 10); }

                ch -= (char)('a' - 'A');
                if (ch < 6) { return (ch + 10); }

                throw new Exception("Invalid hex character.");
            }

            if (input.Length % 2 != 0) { throw new Exception("Invalid hex data."); }

            byte[] retVal = new byte[input.Length / 2];
            for (int i = 0; i < retVal.Length; i++)
            {
                retVal[i] = (byte)((ParseNibble(input[2 * i]) << 4) | ParseNibble(input[2 * i + 1]));
            }

            return retVal;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void DoNotIgnore(uint value, int consumed)
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void DoNotIgnore(ulong value, int consumed)
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void DoNotIgnore(int value, int consumed)
        {
        }

        public static void PrintTestName(string testString, [CallerMemberName] string testName = "")
        {
            if (testString != null)
            {
                Console.WriteLine("{0} called with test string \"{1}\".", testName, testString);
            }
            else
            {
                Console.WriteLine("{0} called with no test string.", testName);
            }
        }

        public static string SpanToString(Span<byte> span, SymbolTable symbolTable = null)
        {
            if (symbolTable == null || symbolTable == SymbolTable.InvariantUtf8)
            {
                Assert.Equal(OperationStatus.Done, TextEncodings.Utf8.ToUtf16Length(span, out int needed));
                Span<byte> output = new byte[needed];
                Assert.Equal(OperationStatus.Done, TextEncodings.Utf8.ToUtf16(span, output, out int consumed, out int written));
                return new string(MemoryMarshal.Cast<byte, char>(output).ToArray());
            }
            else if (symbolTable == SymbolTable.InvariantUtf16)
            {
                return new string(MemoryMarshal.Cast<byte, char>(span).ToArray());
            }

            throw new NotSupportedException();
        }

        // Borrowed from https://github.com/dotnet/corefx/blob/master/src/System.Memory/tests/AllocationHelper.cs

        private static readonly Mutex MemoryLock = new Mutex();
        private static readonly TimeSpan WaitTimeout = TimeSpan.FromSeconds(120);

        public static bool TryAllocNative(IntPtr size, out IntPtr memory)
        {
            memory = IntPtr.Zero;

            if (!MemoryLock.WaitOne(WaitTimeout))
                return false;

            try
            {
                memory = Marshal.AllocHGlobal(size);
            }
            catch (OutOfMemoryException)
            {
                memory = IntPtr.Zero;
                MemoryLock.ReleaseMutex();
            }

            return memory != IntPtr.Zero;
        }

        public static void ReleaseNative(ref IntPtr memory)
        {
            try
            {
                Marshal.FreeHGlobal(memory);
                memory = IntPtr.Zero;
            }
            finally
            {
                MemoryLock.ReleaseMutex();
            }
        }

        public static byte[] UtfEncode(string s, bool utf16)
        {
            if (utf16)
                return Text.Encoding.Unicode.GetBytes(s);
            else
                return Text.Encoding.UTF8.GetBytes(s);
        }

        // TODO: Fix Thai + symbol and adjust tests.
        // Change from new byte[] { 43 }, i.e. '+' to new byte[] { 0xE0, 0xB8, 0x9A, 0xE0, 0xB8, 0xA7, 0xE0, 0xB8, 0x81 }, i.e. 'บวก'
        static byte[][] s_thaiUtf8DigitsAndSymbols = new byte[][]
        {
            new byte[] { 0xe0, 0xb9, 0x90 }, new byte[] { 0xe0, 0xb9, 0x91 }, new byte[] { 0xe0, 0xb9, 0x92 },
            new byte[] { 0xe0, 0xb9, 0x93 }, new byte[] { 0xe0, 0xb9, 0x94 }, new byte[] { 0xe0, 0xb9, 0x95 }, new byte[] { 0xe0, 0xb9, 0x96 },
            new byte[] { 0xe0, 0xb9, 0x97 }, new byte[] { 0xe0, 0xb9, 0x98 }, new byte[] { 0xe0, 0xb9, 0x99 }, new byte[] { 0xE0, 0xB8, 0x88, 0xE0, 0xB8, 0x94 }, null,
            new byte[] { 0xE0, 0xB8, 0xAA, 0xE0, 0xB8, 0xB4, 0xE0, 0xB9, 0x88, 0xE0, 0xB8, 0x87, 0xE0, 0xB8, 0x97, 0xE0, 0xB8, 0xB5, 0xE0, 0xB9, 0x88, 0xE0, 0xB9, 0x83,
                0xE0, 0xB8, 0xAB, 0xE0, 0xB8, 0x8D, 0xE0, 0xB9, 0x88, 0xE0, 0xB9, 0x82, 0xE0, 0xB8, 0x95, 0xE0, 0xB9, 0x80, 0xE0, 0xB8, 0xAB, 0xE0, 0xB8, 0xA5, 0xE0,
                0xB8, 0xB7, 0xE0, 0xB8, 0xAD, 0xE0, 0xB9, 0x80, 0xE0, 0xB8, 0x81, 0xE0, 0xB8, 0xB4, 0xE0, 0xB8, 0x99 },
            new byte[] { 0xE0, 0xB8, 0xA5, 0xE0, 0xB8, 0x9A }, new byte[] { 43 }, new byte[] { 0xE0, 0xB9, 0x84, 0xE0, 0xB8, 0xA1, 0xE0, 0xB9, 0x88, 0xE0, 0xB9,
                0x83, 0xE0, 0xB8, 0x8A, 0xE0, 0xB9, 0x88, 0xE0, 0xB8, 0x95, 0xE0, 0xB8, 0xB1, 0xE0, 0xB8, 0xA7, 0xE0, 0xB9, 0x80, 0xE0, 0xB8, 0xA5, 0xE0, 0xB8, 0x82 },
            new byte[] { 69 }, new byte[] { 101 },
        };

        public class ThaiSymbolTable : SymbolTable
        {
            public ThaiSymbolTable() : base(s_thaiUtf8DigitsAndSymbols) { }

            public override bool TryEncode(byte utf8, Span<byte> destination, out int bytesWritten)
                => SymbolTable.InvariantUtf8.TryEncode(utf8, destination, out bytesWritten);

            public override bool TryEncode(ReadOnlySpan<byte> utf8, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
                => SymbolTable.InvariantUtf8.TryEncode(utf8, destination, out bytesConsumed, out bytesWritten);

            public override bool TryParse(ReadOnlySpan<byte> source, out byte utf8, out int bytesConsumed)
                => SymbolTable.InvariantUtf8.TryParse(source, out utf8, out bytesConsumed);

            public override bool TryParse(ReadOnlySpan<byte> source, Span<byte> utf8, out int bytesConsumed, out int bytesWritten)
                => SymbolTable.InvariantUtf8.TryParse(source, utf8, out bytesConsumed, out bytesWritten);
        }

        public static SymbolTable ThaiTable = new ThaiSymbolTable();
    }
}
