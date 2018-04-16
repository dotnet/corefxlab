// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SpanUsage
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SpanUsageAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "SpanUsage";

        private const string Title = "Call to AsSpan() followed by Slice() can be collapsed";
        private const string MessageFormat = "Can be re-written using an AsSpan() overload";
        private const string Description = "Use an AsSpan() overload";
        private const string Category = "Usage";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeNodeInvokation, SyntaxKind.InvocationExpression);
        }

        private void AnalyzeNodeInvokation(SyntaxNodeAnalysisContext context)
        {
            var invocation = (InvocationExpressionSyntax)context.Node;

            SyntaxToken? identifier = GetMethodCallIdentifier(invocation);
            if (identifier == null)
                return;

            ISymbol methodCallSymbol = context.SemanticModel.GetSymbolInfo(identifier.Value.Parent).Symbol;
            if (methodCallSymbol == null)
                return;

            string typeContainingMethod = methodCallSymbol.ContainingType.ConstructedFrom.ToString();
            if (identifier.Value.ValueText != "AsSpan" || typeContainingMethod != "System.MemoryExtensions")
                return;

            if (invocation.ArgumentList.Arguments.Count != 0)
                return;

            InvocationExpressionSyntax grandParent = invocation.Parent.Parent as InvocationExpressionSyntax;

            SyntaxToken? secondIdentifier = GetMethodCallIdentifier(grandParent);
            if (secondIdentifier == null)
                return;

            ISymbol secondMethodCallSymbol = context.SemanticModel.GetSymbolInfo(secondIdentifier.Value.Parent).Symbol;
            if (secondMethodCallSymbol == null)
                return;

            string typeContainingSecondMethod = secondMethodCallSymbol.ContainingType.ConstructedFrom.ToString();

            if (secondIdentifier.Value.ValueText == "Slice" && (typeContainingSecondMethod == "System.Span<T>" || typeContainingSecondMethod == "System.ReadOnlySpan<T>"))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, grandParent.GetLocation()));
            }
        }

        private SyntaxToken? GetMethodCallIdentifier(InvocationExpressionSyntax invocation)
        {
            if (invocation.Expression is IdentifierNameSyntax directMethodCall)
            {
                return directMethodCall.Identifier;
            }

            if (invocation.Expression is MemberAccessExpressionSyntax memberAccessCall)
            {
                return memberAccessCall.Name.Identifier;
            }

            return null;
        }
    }
}
