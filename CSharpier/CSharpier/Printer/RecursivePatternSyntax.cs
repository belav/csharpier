using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintRecursivePatternSyntax(RecursivePatternSyntax node)
        {
            return Concat(
                node.Type != null ? this.Print(node.Type) : "",
                node.PositionalPatternClause != null
                    ? Concat(
                        String("("),
                        Join(
                            String(", "),
                            node.PositionalPatternClause.Subpatterns.Select(subpatternNode => Concat(
                                subpatternNode.NameColon != null
                                    ? this.PrintNameColonSyntax(subpatternNode.NameColon)
                                    : "",
                                this.Print(subpatternNode.Pattern)
                            ))
                        ),
                        String(")")
                    )
                    : "",
                node.PropertyPatternClause != null
                    ? Concat(
                        String(" { "),
                        Join(
                            String(", "),
                            node.PropertyPatternClause.Subpatterns.Select(subpatternNode => Concat(
                                    this.PrintNameColonSyntax(subpatternNode.NameColon),
                                    this.Print(subpatternNode.Pattern)
                                )
                            )
                        ),
                        String(" } ")
                    )
                    : "",
                node.Designation != null ? this.Print(node.Designation) : ""
            );
        }
    }
}