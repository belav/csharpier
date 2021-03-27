using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintRecursivePatternSyntax(RecursivePatternSyntax node)
        {
            return Concat(
                node.Type != null ? this.Print(node.Type) : Doc.Null,
                node.PositionalPatternClause != null
                    ? Concat(
                        this.PrintSyntaxToken(
                            node.PositionalPatternClause.OpenParenToken
                        ),
                        this.PrintSeparatedSyntaxList(
                            node.PositionalPatternClause.Subpatterns,
                            subpatternNode => Concat(
                                subpatternNode.NameColon != null
                                    ? this.PrintNameColonSyntax(
                                        subpatternNode.NameColon
                                    )
                                    : "",
                                this.Print(subpatternNode.Pattern)
                            ),
                            " "
                        ),
                        this.PrintSyntaxToken(
                            node.PositionalPatternClause.CloseParenToken
                        )
                    )
                    : "",
                node.PropertyPatternClause != null
                    ? Concat(
                        " ",
                        this.PrintSyntaxToken(
                            node.PropertyPatternClause.OpenBraceToken,
                            " "
                        ),
                        this.PrintSeparatedSyntaxList(
                            node.PropertyPatternClause.Subpatterns,
                            subpatternNode => Concat(
                                subpatternNode.NameColon != null
                                    ? this.PrintNameColonSyntax(
                                        subpatternNode.NameColon
                                    )
                                    : Doc.Null,
                                this.Print(subpatternNode.Pattern)
                            ),
                            " "
                        ),
                        SpaceIfNoPreviousComment,
                        this.PrintSyntaxToken(
                            node.PropertyPatternClause.CloseBraceToken,
                            " "
                        )
                    )
                    : "",
                node.Designation != null ? this.Print(node.Designation) : ""
            );
        }
    }
}
