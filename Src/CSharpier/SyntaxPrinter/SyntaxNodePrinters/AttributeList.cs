using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using CSharpier.SyntaxPrinter.SyntaxNodePrinters;
using CSharpier.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class AttributeList
    {
        public static Doc Print(AttributeListSyntax node)
        {
            var docs = new List<Doc>();
            if (node.Parent is CompilationUnitSyntax)
            {
                docs.Add(ExtraNewLines.Print(node));
            }

            docs.Add(Token.Print(node.OpenBracketToken));
            if (node.Target != null)
            {
                docs.Add(
                    Token.Print(node.Target.Identifier),
                    Token.PrintWithSuffix(node.Target.ColonToken, " ")
                );
            }

            var printSeparatedSyntaxList = SeparatedSyntaxList.Print(
                node.Attributes,
                attributeNode =>
                {
                    var name = Node.Print(attributeNode.Name);
                    if (attributeNode.ArgumentList == null)
                    {
                        return name;
                    }

                    return Doc.Group(
                        name,
                        Token.Print(attributeNode.ArgumentList.OpenParenToken),
                        Doc.Indent(
                            Doc.SoftLine,
                            SeparatedSyntaxList.Print(
                                attributeNode.ArgumentList.Arguments,
                                attributeArgumentNode =>
                                    Doc.Concat(
                                        attributeArgumentNode.NameEquals != null
                                          ? NameEquals.Print(attributeArgumentNode.NameEquals)
                                          : Doc.Null,
                                        attributeArgumentNode.NameColon != null
                                            ? BaseExpressionColon.Print(attributeArgumentNode.NameColon)
                                            : Doc.Null,
                                        Node.Print(attributeArgumentNode.Expression)
                                    ),
                                Doc.Line
                            )
                        ),
                        Doc.SoftLine,
                        Token.Print(attributeNode.ArgumentList.CloseParenToken)
                    );
                },
                Doc.Line
            );

            docs.Add(
                node.Attributes.Count > 1
                  ? Doc.Indent(Doc.SoftLine, printSeparatedSyntaxList)
                  : printSeparatedSyntaxList
            );

            docs.Add(
                node.Attributes.Count > 1 ? Doc.SoftLine : Doc.Null,
                Token.Print(node.CloseBracketToken)
            );

            return Doc.Group(docs);
        }
    }
}
