// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.Formatting;
using System.Diagnostics;

namespace SpanUsage
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SpanUsageCodeFixProvider)), Shared]
    public class SpanUsageCodeFixProvider : CodeFixProvider
    {
        private const string title = "Collapse AsSpan().Slice(..) into AsSpan(..)";

        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(SpanUsageAnalyzer.DiagnosticId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            SyntaxNode root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            Diagnostic diagnostic = context.Diagnostics.First();
            TextSpan diagnosticSpan = diagnostic.Location.SourceSpan;

            // Order of iterating the list matters, which is why we use arrays
            InvocationExpressionSyntax[] declarationList = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<InvocationExpressionSyntax>().ToArray();

            if (declarationList.Length == 0) return;

            // Find the invocation expression identified by the diagnostic.
            InvocationExpressionSyntax declaration = null;
            for (int i = 0; i < declarationList.Length; i++)
            {
                InvocationExpressionSyntax decl = declarationList[i];
                if (decl.ToFullString().Contains("AsSpan()"))
                {
                    declaration = decl;
                    break;
                }
            }

            Debug.Assert(declaration != null);

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: title,
                    createChangedDocument: c => CollapseMethodCalls(context.Document, declaration, c),
                    equivalenceKey: title),
                diagnostic);
        }

        private async Task<Document> CollapseMethodCalls(Document document, InvocationExpressionSyntax typeDecl, CancellationToken cancellationToken)
        {
            InvocationExpressionSyntax grandParent = typeDecl.Parent.Parent as InvocationExpressionSyntax;
            InvocationExpressionSyntax newLocal = typeDecl.AddArgumentListArguments(grandParent.ArgumentList.Arguments.ToArray());
            InvocationExpressionSyntax formattedLocal = newLocal.WithAdditionalAnnotations(Formatter.Annotation);

            SyntaxNode oldRoot = await document.GetSyntaxRootAsync(cancellationToken);
            SyntaxNode newRoot = oldRoot.ReplaceNode(grandParent, formattedLocal);

            // Return document with transformed tree.
            return document.WithSyntaxRoot(newRoot);
        }
    }
}
