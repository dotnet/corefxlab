// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestHelper;
using SpanUsage;

namespace SpanUsage.Test
{
    [TestClass]
    public class UnitTest : CodeFixVerifier
    {

        //No diagnostics expected to show up
        [TestMethod]
        public void TestEmptyProgram()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestDetectionAndFix()
        {
            var test = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            static void Main(string[] args)
            {
                var arraySpanSlice = args.AsSpan().Slice(1);
                arraySpanSlice = args.AsSpan().Slice(1, 2);

                var stringSpanSlice = args[0].AsSpan().Slice(1);
                stringSpanSlice = args[0].AsSpan().Slice(1, 2);

                var multipleSpanSlice = args.AsSpan().Slice(1).Slice(1, 1);
                multipleSpanSlice = args.AsSpan().Slice(1).ToArray().AsSpan().Slice(1, 1);

                var stringSpanSliceInMiddle = args[0].Substring(1, 2).AsSpan().Slice(1).ToString();
            }
        }
    }";

            var defaultDiagnosticResult = new DiagnosticResult
            {
                Id = "SpanUsage",
                Message = "Can be re-written using an AsSpan() overload",
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 0, 0) }
            };

            var expected = new DiagnosticResult[8]
            {
                defaultDiagnosticResult,
                defaultDiagnosticResult,
                defaultDiagnosticResult,
                defaultDiagnosticResult,
                defaultDiagnosticResult,
                defaultDiagnosticResult,
                defaultDiagnosticResult,
                defaultDiagnosticResult
            };
            expected[0].Locations = new[] { new DiagnosticResultLocation("Test0.cs", 10, 38) };
            expected[1].Locations = new[] { new DiagnosticResultLocation("Test0.cs", 11, 34) };
            expected[2].Locations = new[] { new DiagnosticResultLocation("Test0.cs", 13, 39) };
            expected[3].Locations = new[] { new DiagnosticResultLocation("Test0.cs", 14, 35) };
            expected[4].Locations = new[] { new DiagnosticResultLocation("Test0.cs", 16, 41) };
            expected[5].Locations = new[] { new DiagnosticResultLocation("Test0.cs", 17, 37) };
            expected[6].Locations = new[] { new DiagnosticResultLocation("Test0.cs", 17, 37) };
            expected[7].Locations = new[] { new DiagnosticResultLocation("Test0.cs", 19, 47) };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            static void Main(string[] args)
            {
                var arraySpanSlice = args.AsSpan(1);
                arraySpanSlice = args.AsSpan(1, 2);

                var stringSpanSlice = args[0].AsSpan(1);
                stringSpanSlice = args[0].AsSpan(1, 2);

                var multipleSpanSlice = args.AsSpan(1).Slice(1, 1);
                multipleSpanSlice = args.AsSpan(1).ToArray().AsSpan(1, 1);

                var stringSpanSliceInMiddle = args[0].Substring(1, 2).AsSpan(1).ToString();
            }
        }
    }";
            VerifyCSharpFix(test, fixtest);
        }

        [TestMethod]
        public void TestDetectionAndFixNothingChanges()
        {
            var test = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            static void Main(string[] args)
            {
                var arraySpanSlice = args.AsSpan(1);
                arraySpanSlice = args.AsSpan(1, 2);

                var stringSpanSlice = args[0].AsSpan(1);
                stringSpanSlice = args[0].AsSpan(1, 2);

                var multipleSpanSlice = args.AsSpan(1).Slice(1, 1);
                multipleSpanSlice = args.AsSpan(1).ToArray().AsSpan(1, 1);

                var stringSpanSliceInMiddle = args[0].Substring(1, 2).AsSpan(1).ToString();
            }
        }
    }";
            var expected = new DiagnosticResult[0] {};
            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            static void Main(string[] args)
            {
                var arraySpanSlice = args.AsSpan(1);
                arraySpanSlice = args.AsSpan(1, 2);

                var stringSpanSlice = args[0].AsSpan(1);
                stringSpanSlice = args[0].AsSpan(1, 2);

                var multipleSpanSlice = args.AsSpan(1).Slice(1, 1);
                multipleSpanSlice = args.AsSpan(1).ToArray().AsSpan(1, 1);

                var stringSpanSliceInMiddle = args[0].Substring(1, 2).AsSpan(1).ToString();
            }
        }
    }";
            VerifyCSharpFix(test, fixtest);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new SpanUsageCodeFixProvider();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new SpanUsageAnalyzer();
        }
    }
}
