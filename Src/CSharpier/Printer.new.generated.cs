using System;
using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using CSharpier.SyntaxPrinter.SyntaxNodePrinters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        public void Print(SyntaxNode syntaxNode, IList<Doc> docs)
        {
            if (syntaxNode == null)
            {
                return;
            }

            // TODO 0 kill? runtime repo has files that will fail on deep recursion
            if (depth > 200)
            {
                throw new InTooDeepException();
            }

            depth++;
            try
            {
                switch (syntaxNode)
                {
                    case PredefinedTypeSyntax predefinedTypeSyntax:
                        this.PrintPredefinedTypeSyntax(
                            predefinedTypeSyntax,
                            docs
                        );
                        break;
                    default:
                        throw new Exception(
                            "Can't handle " + syntaxNode.GetType().Name
                        );
                }
            }

            finally
            {
                depth--;
            }
        }
    }
}
