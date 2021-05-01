using System;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintConstructorDeclarationSyntax(
            ConstructorDeclarationSyntax node
        ) {
            var groupId = Guid.NewGuid().ToString();

            return Doc.Group(
                ExtraNewLines.Print(node),
                this.PrintAttributeLists(node, node.AttributeLists),
                Modifiers.Print(node.Modifiers),
                Token.Print(node.Identifier),
                this.PrintParameterListSyntax(node.ParameterList, groupId),
                node.Initializer != null
                    ? this.Print(node.Initializer)
                    : Doc.Null,
                node.Body != null
                    ? this.PrintBlockSyntaxWithConditionalSpace(
                            node.Body,
                            groupId
                        )
                    : Doc.Null,
                node.ExpressionBody != null
                    ? this.PrintArrowExpressionClauseSyntax(node.ExpressionBody)
                    : Doc.Null,
                Token.Print(node.SemicolonToken)
            );
        }
    }
}
