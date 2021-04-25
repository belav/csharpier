using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintRecursivePatternSyntax(RecursivePatternSyntax node)
        {
            return Docs.Concat(
                node.Type != null ? this.Print(node.Type) : Doc.Null,
                node.PositionalPatternClause != null
                    ? Docs.Concat(
                            this.PrintSyntaxToken(
                                node.PositionalPatternClause.OpenParenToken
                            ),
                            SeparatedSyntaxList.Print(
                                node.PositionalPatternClause.Subpatterns,
                                subpatternNode =>
                                    Docs.Concat(
                                        subpatternNode.NameColon != null
                                            ? this.PrintNameColonSyntax(
                                                    subpatternNode.NameColon
                                                )
                                            : string.Empty,
                                        this.Print(subpatternNode.Pattern)
                                    ),
                                " "
                            ),
                            this.PrintSyntaxToken(
                                node.PositionalPatternClause.CloseParenToken
                            )
                        )
                    : string.Empty,
                node.PropertyPatternClause != null
                    ? Docs.Concat(
                            " ",
                            this.PrintSyntaxToken(
                                node.PropertyPatternClause.OpenBraceToken,
                                " "
                            ),
                            SeparatedSyntaxList.Print(
                                node.PropertyPatternClause.Subpatterns,
                                subpatternNode =>
                                    Docs.Concat(
                                        subpatternNode.NameColon != null
                                            ? this.PrintNameColonSyntax(
                                                    subpatternNode.NameColon
                                                )
                                            : Doc.Null,
                                        this.Print(subpatternNode.Pattern)
                                    ),
                                " "
                            ),
                            " ",
                            this.PrintSyntaxToken(
                                node.PropertyPatternClause.CloseBraceToken,
                                " "
                            )
                        )
                    : string.Empty,
                node.Designation != null
                    ? this.Print(node.Designation)
                    : string.Empty
            );
        }
    }
}
