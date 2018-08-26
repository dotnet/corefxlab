// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using System;

namespace Benchmarks.System.Text.ValueBuilder
{
    //[MemoryDiagnoser]
    [DisassemblyDiagnoser(printAsm: true)]
    [ShortRunJob]
    public class Construct
    {
        [Benchmark]
        public bool ConstructBool()
        {
            Variant v = true;
            return v.Type == VariantType.Boolean;
        }

        [Benchmark]
        public bool ConstructObject()
        {
            Variant v = new Variant(new object());
            return v.Type == VariantType.Object;
        }
    }
}
