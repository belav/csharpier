using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using CSharpier.SyntaxPrinter.SyntaxNodePrinters;
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
                docs.Add(ExtraNewLines.Print(node));
            }

            docs.Add(SyntaxTokens.Print(node.OpenBracketToken));
            if (node.Target != null)
            {
                docs.Add(
                    SyntaxTokens.Print(node.Target.Identifier),
                    SyntaxTokens.PrintWithSuffix(node.Target.ColonToken, " ")
                );
            }

            var printSeparatedSyntaxList = SeparatedSyntaxList.Print(
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
                        SyntaxTokens.Print(
                            attributeNode.ArgumentList.OpenParenToken
                        ),
                        Docs.Indent(
                            Docs.SoftLine,
                            SeparatedSyntaxList.Print(
                                attributeNode.ArgumentList.Arguments,
                                attributeArgumentNode =>
                                    Docs.Concat(
                                        attributeArgumentNode.NameEquals != null
                                            ? NameEquals.Print(
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
                            SyntaxTokens.Print(
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
                SyntaxTokens.Print(node.CloseBracketToken)
            );

            return Docs.Group(docs);
        }
    }
}
