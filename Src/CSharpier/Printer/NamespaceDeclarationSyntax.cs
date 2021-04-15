using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintNamespaceDeclarationSyntax(
            NamespaceDeclarationSyntax node
        ) {
            var docs = new List<Doc>
            {
                this.PrintExtraNewLines(node),
                this.PrintAttributeLists(node, node.AttributeLists),
                this.PrintModifiers(node.Modifiers),
                SyntaxTokens.Print(node.NamespaceKeyword),
                " ",
                this.Print(node.Name)
            };

            var innerDocs = new List<Doc>();
            var hasMembers = node.Members.Count > 0;
            var hasUsing = node.Usings.Count > 0;
            var hasExterns = node.Externs.Count > 0;
            if (hasMembers || hasUsing || hasExterns)
            {
                innerDocs.Add(Docs.HardLine);
                if (hasExterns)
                {
                    innerDocs.Add(
                        Join(
                            Docs.HardLine,
                            node.Externs.Select(
                                this.PrintExternAliasDirectiveSyntax
                            )
                        ),
                        Docs.HardLine
                    );
                }
                if (hasUsing)
                {
                    innerDocs.Add(
                        Join(
                            Docs.HardLine,
                            node.Usings.Select(this.PrintUsingDirectiveSyntax)
                        ),
                        Docs.HardLine
                    );
                }
                if (hasMembers)
                {
                    innerDocs.Add(
                        Join(Docs.HardLine, node.Members.Select(this.Print)),
                        Docs.HardLine
                    );
                }

                innerDocs.RemoveAt(innerDocs.Count - 1);
            }
            else
            {
                innerDocs.Add(" ");
            }

            DocUtilities.RemoveInitialDoubleHardLine(innerDocs);

            docs.Add(
                Docs.Group(
                    Docs.Line,
                    SyntaxTokens.Print(node.OpenBraceToken),
                    Docs.Indent(innerDocs),
                    hasMembers || hasUsing || hasExterns
                        ? Docs.HardLine
                        : Docs.Null,
                    SyntaxTokens.Print(node.CloseBraceToken),
                    SyntaxTokens.Print(node.SemicolonToken)
                )
            );
            return Docs.Concat(docs);
        }
    }
}
