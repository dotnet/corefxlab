// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text.CaseFolding;
using System.Text.CaseFolding.Tests;
using Xunit;

namespace System.Text.CaseFolding.Tests
{
    public class FoldCharTests
    {
        [Fact]
        public static void Fold_Char()
        {
            for (int i = 0; i <= 0xffff; i++)
            {
                var expected = i;
                if (CharUnicodeInfoTestData.CaseFoldingPairs.TryGetValue((char)i, out int foldedCharOut))
                {
                    expected = foldedCharOut;
                }

                var foldedChar = (int)SimpleCaseFolding.SimpleCaseFold((char)i);
                Assert.Equal(expected, foldedChar);
            }
        }

        [Fact]
        public static void Fold_Char_Surrogate()
        {
            for (int i = 0x10000; i <= 0x1ffff; i++)
            {
                var expected = i;
                if (CharUnicodeInfoTestData.CaseFoldingPairs.TryGetValue(i, out int foldedOut))
                {
                    expected = foldedOut;
                }

                var expectedString = Char.ConvertFromUtf32(expected);
                var value = Char.ConvertFromUtf32(i);
                var foldedString = SimpleCaseFolding.SimpleCaseFold(value);
                Assert.Equal(expectedString, foldedString);
            }
        }
    }
}
