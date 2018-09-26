// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;

namespace System.Diagnostics
{
    internal static class Precondition
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Require(bool condition)
        {
            if (!condition)
            {
                Fail();
            }
        }

        private static void Fail()
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
            throw new Failure();
        }

        public sealed class Failure : Exception
        {
            static string s_message = "precondition failed";
            internal Failure() : base(s_message) { }
        } 
    }
}
