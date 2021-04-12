using System;
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

            return Docs.Group(
                this.PrintExtraNewLines(node),
                this.PrintAttributeLists(node, node.AttributeLists),
                this.PrintModifiers(node.Modifiers),
                SyntaxTokens.Print(node.Identifier),
                this.PrintParameterListSyntax(node.ParameterList, groupId),
                node.Initializer != null
                    ? this.Print(node.Initializer)
                    : Docs.Null,
                node.Body != null
                    ? this.PrintBlockSyntaxWithConditionalSpace(
                            node.Body,
                            groupId
                        )
                    : Docs.Null,
                node.ExpressionBody != null
                    ? this.PrintArrowExpressionClauseSyntax(node.ExpressionBody)
                    : Docs.Null,
                SyntaxTokens.Print(node.SemicolonToken)
            );
        }
    }
}
