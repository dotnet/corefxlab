// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Text.Primitives.Tests.Encoding;

namespace System.Text.Primitives.Benchmarks
{
    public class EncoderHelper
    {
        public static IEnumerable<CodePoint> GetEncodingPerformanceTestData()
        {
            yield return new CodePoint(0x0, TextEncoderConstants.Utf8OneByteLastCodePoint);
            yield return new CodePoint(TextEncoderConstants.Utf8OneByteLastCodePoint + 1, TextEncoderConstants.Utf8TwoBytesLastCodePoint);
            yield return new CodePoint(TextEncoderConstants.Utf8TwoBytesLastCodePoint + 1, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);
            yield return new CodePoint(TextEncoderConstants.Utf16HighSurrogateFirstCodePoint, TextEncoderConstants.Utf16LowSurrogateLastCodePoint);
            yield return new CodePoint(0x0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);
            yield return new CodePoint(0, 0, SpecialTestCases.AlternatingASCIIAndNonASCII);
            yield return new CodePoint(0, 0, SpecialTestCases.MostlyASCIIAndSomeNonASCII);
        }

        public static string GenerateStringData(int length, int minCodePoint, int maxCodePoint, SpecialTestCases special = SpecialTestCases.None)
        {
            if (special != SpecialTestCases.None)
            {
                if (special == SpecialTestCases.AlternatingASCIIAndNonASCII) return TextEncoderTestHelper.GenerateStringAlternatingASCIIAndNonASCII(length);
                if (special == SpecialTestCases.MostlyASCIIAndSomeNonASCII) return TextEncoderTestHelper.GenerateStringWithMostlyASCIIAndSomeNonASCII(length);
                return "";
            }
            else
            {
                return TextEncoderTestHelper.GenerateValidString(length, minCodePoint, maxCodePoint);
            }
        }

        public enum SpecialTestCases
        {
            None,
            AlternatingASCIIAndNonASCII,
            MostlyASCIIAndSomeNonASCII
        }
    }
}
