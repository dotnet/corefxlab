// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MemoryUsage
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MemoryUsageAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MemoryUsage";

        private const string Title = "Call to Slice() followed by Span property access should be reversed";
        private const string MessageFormat = "Can be re-ordered and written as Span.Slice()";
        private const string Description = "Slicing a Span is faster than slicing Memory";
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
            if (identifier.Value.ValueText != "Slice" || (typeContainingMethod != "System.Memory<T>" && typeContainingMethod != "System.ReadOnlyMemory<T>"))
                return;

            if (invocation.ArgumentList.Arguments.Count != 1 && invocation.ArgumentList.Arguments.Count != 2)
                return;

            MemberAccessExpressionSyntax parent = invocation.Parent as MemberAccessExpressionSyntax;

            SyntaxToken? secondIdentifier = GetMethodCallIdentifier(parent);
            if (secondIdentifier == null)
                return;

            ISymbol memberAccessSymbol = context.SemanticModel.GetSymbolInfo(secondIdentifier.Value.Parent).Symbol;
            if (memberAccessSymbol == null)
                return;

            string typeContainingMemberAccess = memberAccessSymbol.ContainingType.ConstructedFrom.ToString();

            if (secondIdentifier.Value.ValueText == "Span" && (typeContainingMemberAccess == "System.Memory<T>" || typeContainingMemberAccess == "System.ReadOnlyMemory<T>"))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, parent.GetLocation()));
            }
        }

        private SyntaxToken? GetMethodCallIdentifier(MemberAccessExpressionSyntax memberAccess)
        {
            if (memberAccess != null && memberAccess.Name is IdentifierNameSyntax memberName)
            {
                return memberName.Identifier;
            }

            return null;
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
