// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
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

namespace MemoryUsage
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MemoryUsageCodeFixProvider)), Shared]
    public class MemoryUsageCodeFixProvider : CodeFixProvider
    {
        private const string title = "Swap the order of Slice(..).Span to be Span.Slice(..)";

        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(MemoryUsageAnalyzer.DiagnosticId); }
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

            string methodStr = "Slice(";
            string propertyStr = ".Span.";
            string skipAlreadyFixed = propertyStr + methodStr;

            // Find the invocation expression identified by the diagnostic.
            InvocationExpressionSyntax declaration = null;
            for (int i = 0; i < declarationList.Length; i++)
            {
                InvocationExpressionSyntax decl = declarationList[i];
                string str = decl.ToFullString();
                int index = str.LastIndexOf(methodStr);
                if (index != -1 && (index < propertyStr.Length || !str.Substring(index - propertyStr.Length).StartsWith(skipAlreadyFixed)))
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
                    createChangedDocument: c => SwapMethodCalls(context.Document, declaration, c),
                    equivalenceKey: title),
                diagnostic);
        }

        private async Task<Document> SwapMethodCalls(Document document, InvocationExpressionSyntax typeDecl, CancellationToken cancellationToken)
        {
            IEnumerable<SyntaxNode> children = typeDecl.ChildNodes();
            Debug.Assert(children.Count() > 0);
            MemberAccessExpressionSyntax sliceAccess = children.ElementAt(0) as MemberAccessExpressionSyntax;

            IEnumerable<SyntaxNode> grandChildren = sliceAccess.ChildNodes();
            Debug.Assert(grandChildren.Count() > 0);
            SyntaxNode target = grandChildren.ElementAt(0);

            MemberAccessExpressionSyntax parent = typeDecl.Parent as MemberAccessExpressionSyntax;

            MemberAccessExpressionSyntax newLocal = parent.ReplaceNode(typeDecl, target);
            MemberAccessExpressionSyntax formattedLocal = newLocal.WithAdditionalAnnotations(Formatter.Annotation);

            InvocationExpressionSyntax replacement = typeDecl.ReplaceNode(target, formattedLocal).WithAdditionalAnnotations(Formatter.Annotation);

            SyntaxNode oldRoot = await document.GetSyntaxRootAsync(cancellationToken);
            SyntaxNode newRoot = oldRoot.ReplaceNode(parent, replacement);

            // Return document with transformed tree.
            return document.WithSyntaxRoot(newRoot);
        }
    }
}
