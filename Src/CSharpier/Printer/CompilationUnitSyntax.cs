using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintCompilationUnitSyntax(CompilationUnitSyntax node)
        {
            var docs = new List<Doc>();
            if (node.Externs.Count > 0)
            {
                docs.Add(
                    Doc.Join(
                        Doc.HardLine,
                        node.Externs.Select(
                            this.PrintExternAliasDirectiveSyntax
                        )
                    ),
                    Doc.HardLine
                );
            }
            if (node.Usings.Count > 0)
            {
                docs.Add(
                    Doc.Join(
                        Doc.HardLine,
                        node.Usings.Select(this.PrintUsingDirectiveSyntax)
                    ),
                    Doc.HardLine
                );
            }
            docs.Add(this.PrintAttributeLists(node, node.AttributeLists));
            if (node.Members.Count > 0)
            {
                docs.Add(
                    Doc.Join(Doc.HardLine, node.Members.Select(this.Print))
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
