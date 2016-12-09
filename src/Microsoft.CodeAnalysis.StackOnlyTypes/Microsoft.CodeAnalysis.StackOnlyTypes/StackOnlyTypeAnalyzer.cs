using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Microsoft.CodeAnalysis.StackOnlyTypes
{
    [DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
    public class StackOnlyTypeAnalyzer : DiagnosticAnalyzer
    {
        private const string Category = "Usage";

        public const string StackOnlyFieldId = "StackOnlyField";
        public const string StackOnlyClassId = "StackOnlyClass";

        private static readonly LocalizableString StackOnlyFieldTitle = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString StackOnlyFieldMessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString StackOnlyFieldDescription = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));

        private static DiagnosticDescriptor RuleStackOnlyField = new DiagnosticDescriptor(StackOnlyFieldId, StackOnlyFieldTitle, StackOnlyFieldMessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: StackOnlyFieldDescription);

        private static DiagnosticDescriptor RuleStackOnlyClass = new DiagnosticDescriptor(StackOnlyClassId, "Reference types cannot be stack-only", "Reference type {0} cannot be marked as stack-only", Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: "Reference types cannot have StackOnly attribute applied.");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(RuleStackOnlyField, RuleStackOnlyClass); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            if (HasStackOnlyAttribute(namedTypeSymbol)) {
                if (namedTypeSymbol.IsValueType) { return; }
                else { // reference types should not have StackOnlyAttribute applied
                    var diagnosticClass = Diagnostic.Create(RuleStackOnlyClass, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                    context.ReportDiagnostic(diagnosticClass);
                }
            }

            // This is not a stack-only type, so ...
            // check if any fields are of stack-only types 
            foreach (var member in namedTypeSymbol.GetMembers())
            {
                var field = member as IFieldSymbol;
                if (field == null) { continue; }

                var fieldType = field.Type;
                if(HasStackOnlyAttribute(fieldType)) { 
                    var diagnostic = Diagnostic.Create(RuleStackOnlyField, field.Locations[0], field.Type.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }

        static bool HasStackOnlyAttribute(ITypeSymbol type)
        {
            foreach (var attribute in type.GetAttributes())
            {
                if (attribute.AttributeClass.Name == "StackOnlyAttribute")
                {
                    return true;
                }
            }
            return false;
        }
    }
}
