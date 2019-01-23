// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using System.Text;

namespace Benchmarks.System.Text.ValueBuilder
{
    // [MemoryDiagnoser]
    // [DisassemblyDiagnoser(printAsm: true, recursiveDepth: 2)]
    // [ShortRunJob]
    public class AppendBaseline
    {
        private static StringBuilder s_builder = new StringBuilder(100);

        [Benchmark(Baseline = true)]
        public void AppendFormatInt()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("The answer is {0}", 42);
        }

        [Benchmark]
        public void AppendFormatIntString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("The answer is {0}, the question is {1}", 42, "6 x 7");
        }

        [Benchmark]
        public void AppendFormatInt_Cached()
        {
            s_builder.Clear();
            s_builder.AppendFormat("The answer is {0}", 42);
        }

        [Benchmark]
        public void AppendFormatInt_PreSize()
        {
            StringBuilder sb = new StringBuilder(100);
            sb.AppendFormat("The answer is {0}", 42);
        }

        [Benchmark]
        public void AppendFormatInt_ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("The answer is {0}", 42);
            sb.ToString();
        }

        [Benchmark]
        public void AppendFormatInt_ToString_Presize()
        {
            StringBuilder sb = new StringBuilder(100);
            sb.AppendFormat("The answer is {0}", 42);
            sb.ToString();
        }
    }
}
