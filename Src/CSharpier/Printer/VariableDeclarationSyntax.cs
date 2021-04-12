using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintVariableDeclarationSyntax(
            VariableDeclarationSyntax node
        ) {
            var docs = Docs.Concat(
                this.PrintSeparatedSyntaxList(
                    node.Variables,
                    this.PrintVariableDeclaratorSyntax,
                    node.Parent is ForStatementSyntax
                        ? Docs.Line
                        : Docs.HardLine
                )
            );

            return Docs.Concat(
                this.Print(node.Type),
                Docs.SpaceIfNoPreviousComment,
                node.Variables.Count > 1 ? Docs.Indent(docs) : docs
            );
        }
    }
}
