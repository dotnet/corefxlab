// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Benchmarks.System.Text.ValueBuilder
{
    // [DisassemblyDiagnoser(printAsm: true)]
    // [MemoryDiagnoser] //, printSource: true
    // [ShortRunJob]
    public class GetValue
    {
        private Variant _int = 42;
        private readonly Variant _readonlyInt = 101;
        private object _boxedInt = 42;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _boxedInt = 1945;
        }

        [Benchmark]
        public int TryGetInt()
        {
            _int.TryGetValue(out int value);
            value++;
            return value;
        }

        [Benchmark]
        public void TryGetInt_Readonly()
        {
            _readonlyInt.TryGetValue(out int value);
        }

        [Benchmark]
        public void TryGetInt_Fail()
        {
            _int.TryGetValue(out float value);
        }

        [Benchmark]
        public int UnboxInt()
        {
            int i = Unbox(_boxedInt);
            i++;
            return i;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int Unbox(object o)
        {
            return (int)o;
        }
    }
}
