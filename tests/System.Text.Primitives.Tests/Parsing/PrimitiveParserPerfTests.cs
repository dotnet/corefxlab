// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;

namespace System.Text.Primitives.Tests
{
    public partial class PrimitiveParserPerfTests
    {
        private const int LoadIterations = 30000;

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void DoNotIgnore(uint value, int consumed)
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void DoNotIgnore(ulong value, int consumed)
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void DoNotIgnore(int value, int consumed)
        {
        }

        private static void PrintTestName(string testString, [CallerMemberName] string testName = "")
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

        static TextEncoder s_thaiEncoder = TextEncoder.CreateUtf8Encoder(s_thaiUtf8DigitsAndSymbols);

        private byte[] UtfEncode(string s, bool utf16)
        {
            if (utf16)
                return Text.Encoding.Unicode.GetBytes(s);
            else
                return Text.Encoding.UTF8.GetBytes(s);
        }
    }
}
