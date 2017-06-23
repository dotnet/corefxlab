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
    }
}
