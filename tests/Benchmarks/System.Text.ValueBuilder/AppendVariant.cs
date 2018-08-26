// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using System;
using System.Text;

namespace Benchmarks.System.Text.ValueBuilder
{
    //[MemoryDiagnoser]
    [DisassemblyDiagnoser(printAsm: true, recursiveDepth: 2)]
    // [MemoryDiagnoser] //, printSource: true
    [ShortRunJob]
    public class AppendVariant
    {
        [Benchmark]
        public void AppendFormatInt()
        {
            Variant v = 42;
            ValueStringBuilder sb = new ValueStringBuilder();
            sb.Append("The answer is {0}", v.ToSpan());
            sb.Dispose();
        }

        [Benchmark]
        public void AppendFormatInt_PreSize()
        {
            Variant v = 42;
            ValueStringBuilder sb = new ValueStringBuilder();
            sb.Append("The answer is {0}", v.ToSpan());
            sb.Dispose();
        }

        [Benchmark(Baseline = true)]
        public unsafe void AppendFormatInt_Stack()
        {
            Variant v = 42;
            char* c = stackalloc char[100];
            Span<char> span = new Span<char>(c, 100);
            ValueStringBuilder sb = new ValueStringBuilder(span);
            sb.Append("The answer is {0}", v.ToSpan());
        }
    }
}
