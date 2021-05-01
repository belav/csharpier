using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class NamespaceDeclaration
    {
        public static Doc Print(NamespaceDeclarationSyntax node)
        {
            var docs = new List<Doc>
            {
                ExtraNewLines.Print(node),
                new Printer().PrintAttributeLists(node, node.AttributeLists),
                Modifiers.Print(node.Modifiers),
                Token.Print(node.NamespaceKeyword),
                " ",
                Node.Print(node.Name)
            };

            var innerDocs = new List<Doc>();
            var hasMembers = node.Members.Count > 0;
            var hasUsing = node.Usings.Count > 0;
            var hasExterns = node.Externs.Count > 0;
            if (hasMembers || hasUsing || hasExterns)
            {
                innerDocs.Add(Doc.HardLine);
                if (hasExterns)
                {
                    innerDocs.Add(
                        Doc.Join(
                            Doc.HardLine,
                            node.Externs.Select(ExternAliasDirective.Print)
                        ),
                        Doc.HardLine
                    );
                }
                if (hasUsing)
                {
                    innerDocs.Add(
                        Doc.Join(
                            Doc.HardLine,
                            node.Usings.Select(UsingDirective.Print)
                        ),
                        Doc.HardLine
                    );
                }
                if (hasMembers)
                {
                    innerDocs.Add(
                        Doc.Join(Doc.HardLine, node.Members.Select(Node.Print)),
                        Doc.HardLine
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
                Doc.Group(
                    Doc.Line,
                    Token.Print(node.OpenBraceToken),
                    Doc.Indent(innerDocs),
                    hasMembers || hasUsing || hasExterns
                        ? Doc.HardLine
                        : Doc.Null,
                    Token.Print(node.CloseBraceToken),
                    Token.Print(node.SemicolonToken)
                )
            );
            return Doc.Concat(docs);
        }
    }
}
