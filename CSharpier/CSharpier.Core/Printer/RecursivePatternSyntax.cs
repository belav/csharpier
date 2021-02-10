using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintRecursivePatternSyntax(RecursivePatternSyntax node)
        {
            return Concat(
                node.Type != null ? this.Print(node.Type) : "",
                node.PositionalPatternClause != null
                    ? Concat(
                        "(",
                        Join(
                            ", ",
                            node.PositionalPatternClause.Subpatterns.Select(subpatternNode => Concat(
                                subpatternNode.NameColon != null
                                    ? this.PrintNameColonSyntax(subpatternNode.NameColon)
                                    : "",
                                this.Print(subpatternNode.Pattern)
                            ))
                        ),
                        ")"
                    )
                    : "",
                node.PropertyPatternClause != null
                    ? Concat(
                        " { ",
                        Join(
                            ", ",
                            node.PropertyPatternClause.Subpatterns.Select(subpatternNode => Concat(
                                    this.PrintNameColonSyntax(subpatternNode.NameColon),
                                    this.Print(subpatternNode.Pattern)
                                )
                            )
                        ),
                        " } "
                    )
                    : "",
                node.Designation != null ? this.Print(node.Designation) : ""
            );
        }
    }
}