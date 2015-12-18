using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Microsoft.CodeAnalysis.StackOnlyTypes.Test
{
    [TestClass]
    public class UnitTest : DiagnosticVerifier
    {

        //No diagnostics expected to show up
        [TestMethod]
        public void AllGood()
        {
            var test = @"
using System;
class StackOnlyAttribute : Attribute {}
[StackOnly]
struct StackOnlyStruct {}
[StackOnly]
struct SomeStruct {   
    StackOnlyStruct _field;
}
";

            VerifyCSharpDiagnostic(test);
        }

        //Diagnostic triggered and checked for
        [TestMethod]
        public void StackOnlyTypesCannotBeFieldsInNonStackOnlyTypes()
        {
            var test = @"
using System;
class StackOnlyAttribute : Attribute {}
[StackOnly]
struct StackOnlyStruct {}
struct SomeStruct {   
    StackOnlyStruct _field;
}
";
            var expected = new DiagnosticResult
            {
                Id = "StackOnlyField",
                Message = string.Format("Type '{0}' is a stack-only type and so it cannot be used as a field of a non-stack-only type", "StackOnlyStruct"),
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 7, 21)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        //Diagnostic triggered and checked for
        [TestMethod]
        public void ClassCannotBeMarkedAsStackOnly()
        {
            var test = @"
using System;
class StackOnlyAttribute : Attribute {}
[StackOnly]
struct StackOnlyStruct {}
[StackOnly]
class SomeClass {   
}
";
            var expected = new DiagnosticResult
            {
                Id = "StackOnlyClass",
                Message = string.Format("Reference type {0} cannot be marked as stack-only", "SomeClass"),
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 7, 7)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new StackOnlyTypeAnalyzer();
        }
    }
}