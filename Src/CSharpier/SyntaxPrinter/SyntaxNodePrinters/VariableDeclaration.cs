using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class VariableDeclaration
    {
        public static Doc Print(VariableDeclarationSyntax node)
        {
            if (node.Variables.Count > 1)
            {
                var docs = Doc.Concat(
                    SeparatedSyntaxList.Print(
                        node.Variables,
                        VariableDeclarator.Print,
                        node.Parent is ForStatementSyntax ? Doc.Line : Doc.HardLine
                    )
                );

                return Doc.Concat(Node.Print(node.Type), " ", Doc.Indent(docs));
            }

            var variable = node.Variables[0];

            var leftDoc = Doc.Concat(
                Node.Print(node.Type),
                " ",
                Token.Print(variable.Identifier),
                variable.ArgumentList != null
                    ? BracketedArgumentList.Print(variable.ArgumentList)
                    : Doc.Null
            );

            var initializer = variable.Initializer;

            return initializer == null
                ? Doc.Group(leftDoc)
                : RightHandSide.Print(
                      Doc.Concat(leftDoc, " "),
                      Token.Print(initializer.EqualsToken),
                      initializer.Value
                  );
        }
    }
}
