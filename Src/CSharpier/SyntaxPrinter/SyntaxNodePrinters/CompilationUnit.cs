using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class CompilationUnit
    {
        public static Doc Print(CompilationUnitSyntax node)
        {
            var docs = new List<Doc>();
            if (node.Externs.Count > 0)
            {
                docs.Add(
                    Doc.Join(
                        Doc.HardLine,
                        node.Externs.Select(ExternAliasDirective.Print)
                    ),
                    Doc.HardLine
                );
            }
            if (node.Usings.Count > 0)
            {
                docs.Add(
                    Doc.Join(
                        Doc.HardLine,
                        node.Usings.Select(UsingDirective.Print)
                    ),
                    Doc.HardLine
                );
            }
            docs.Add(AttributeLists.Print(node, node.AttributeLists));
            if (node.Members.Count > 0)
            {
                docs.Add(
                    Doc.Join(Doc.HardLine, node.Members.Select(Node.Print))
                );
            }

            var finalTrivia = Token.PrintLeadingTrivia(
                node.EndOfFileToken.LeadingTrivia,
                includeInitialNewLines: true
            );
            if (finalTrivia != Doc.Null)
            {
                // even though we include the initialNewLines above, a literalLine from directives trims the hardline, so add an extra one here
                docs.Add(Doc.HardLine, finalTrivia);
            }

            return Doc.Concat(docs);
        }
    }
}
