using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintAttributeListSyntax(AttributeListSyntax node)
        {
            var docs = new List<Doc>();
            if (node.Parent is CompilationUnitSyntax)
            {
                docs.Add(this.PrintExtraNewLines(node));
            }

            docs.Add(this.PrintSyntaxToken(node.OpenBracketToken));
            if (node.Target != null)
            {
                docs.Add(
                    this.PrintSyntaxToken(node.Target.Identifier),
                    this.PrintSyntaxToken(
                        node.Target.ColonToken,
                        afterTokenIfNoTrailing: " "
                    )
                );
            }

            var printSeparatedSyntaxList = this.PrintSeparatedSyntaxList(
                node.Attributes,
                attributeNode =>
                {
                    var name = this.Print(attributeNode.Name);
                    if (attributeNode.ArgumentList == null)
                    {
                        return name;
                    }

                    return Docs.Group(
                        name,
                        this.PrintSyntaxToken(
                            attributeNode.ArgumentList.OpenParenToken
                        ),
                        Docs.Indent(
                            Docs.SoftLine,
                            this.PrintSeparatedSyntaxList(
                                attributeNode.ArgumentList.Arguments,
                                attributeArgumentNode =>
                                    Docs.Concat(
                                        attributeArgumentNode.NameEquals != null
                                            ? this.PrintNameEqualsSyntax(
                                                    attributeArgumentNode.NameEquals
                                                )
                                            : Doc.Null,
                                        attributeArgumentNode.NameColon != null
                                            ? this.PrintNameColonSyntax(
                                                    attributeArgumentNode.NameColon
                                                )
                                            : Doc.Null,
                                        this.Print(
                                            attributeArgumentNode.Expression
                                        )
                                    ),
                                Docs.Line
                            ),
                            this.PrintSyntaxToken(
                                attributeNode.ArgumentList.CloseParenToken
                            )
                        )
                    );
                },
                Docs.Line
            );

            docs.Add(
                node.Attributes.Count > 1
                    ? Docs.Indent(Docs.SoftLine, printSeparatedSyntaxList)
                    : printSeparatedSyntaxList
            );

            docs.Add(
                node.Attributes.Count > 1 ? Docs.SoftLine : Docs.Null,
                this.PrintSyntaxToken(node.CloseBracketToken)
            );

            return Docs.Group(docs);
        }
    }
}
