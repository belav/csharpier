using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintNamespaceDeclarationSyntax(
            NamespaceDeclarationSyntax node)
        {
            var parts = new Parts();
            parts.Push(this.PrintExtraNewLines(node));
            parts.Push(this.PrintAttributeLists(node, node.AttributeLists));
            parts.Push(this.PrintModifiers(node.Modifiers));
            parts.Push(this.PrintSyntaxToken(node.NamespaceKeyword), " ");
            parts.Push(this.Print(node.Name));

            var innerParts = new Parts();
            var hasMembers = node.Members.Count > 0;
            var hasUsing = node.Usings.Count > 0;
            var hasExterns = node.Externs.Count > 0;
            if (hasMembers || hasUsing || hasExterns)
            {
                innerParts.Push(HardLine);
                if (hasExterns)
                {
                    innerParts.Push(
                        Join(
                            HardLine,
                            node.Externs.Select(
                                this.PrintExternAliasDirectiveSyntax
                            )
                        ),
                        HardLine
                    );
                }
                if (hasUsing)
                {
                    innerParts.Push(
                        Join(
                            HardLine,
                            node.Usings.Select(this.PrintUsingDirectiveSyntax)
                        ),
                        HardLine
                    );
                }
                if (hasMembers)
                {
                    innerParts.Push(
                        Join(HardLine, node.Members.Select(this.Print)),
                        HardLine
                    );
                }

                innerParts.RemoveAt(innerParts.Count - 1);
            }
            else
            {
                innerParts.Push(" ");
            }


            parts.Push(
                Group(
                    Line,
                    this.PrintSyntaxToken(node.OpenBraceToken),
                    Indent(Concat(innerParts)),
                    hasMembers || hasUsing || hasExterns ? HardLine : null,
                    this.PrintSyntaxToken(node.CloseBraceToken),
                    this.PrintSyntaxToken(node.SemicolonToken)
                )
            );
            return Concat(parts);
        }
    }
}
