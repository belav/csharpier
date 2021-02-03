using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        // TODO MethodDeclarationSyntax has a base class, would that work? make a method that requires the base class, do is on the passed in object
        private Doc PrintMethodDeclarationSyntax(MethodDeclarationSyntax node)
        {
            var parts = new Parts();
            //this.PrintExtraNewLines(node, String("attributeLists"), String("modifiers"), [String("returnType"), String("keyword")]);
            this.PrintAttributeLists(node.AttributeLists, parts);
            //printLeadingComments(node, parts, String("modifiers"), String("returnType"), String("identifier"));
            parts.Push(this.PrintModifiers(node.Modifiers));
            if (NotNull(node.ReturnType)) {
                parts.Push(this.Print(node.ReturnType), String(" "));
            }
            if (node.ExplicitInterfaceSpecifier != null) {
                // TODO this doesn't appear valid ? parts.Push(printIdentifier(node.ExplicitInterfaceSpecifier.name), ".");
            }
            // TODO this is only true if this isn't a method, I think
            if (node.Identifier.RawKind != 0) {
                parts.Push(node.Identifier.Text);
            }
            // TODO non method stuff
            // if (NotNull(node.ImplicitOrExplicitKeyword)) {
            //     parts.Push(printSyntaxToken(node.ImplicitOrExplicitKeyword), String(" "));
            // }
            // if (NotNull(node.OperatorKeyword)) {
            //     parts.Push(String("operator "));
            // }
            // if (NotNull(node.OperatorToken)) {
            //     parts.Push(printSyntaxToken(node.OperatorToken));
            // }
            // if (NotNull(node.Type)) {
            //     parts.Push(this.Print(node.Type));
            // }
            if (NotNull(node.TypeParameterList)) {
                parts.Push(this.PrintTypeParameterListSyntax(node.TypeParameterList));
            }
            parts.Push(this.Print(node.ParameterList));
            this.PrintConstraintClauses(node, parts);
            if (NotNull(node.Body)) {
                parts.Push(this.PrintBlockSyntax(node.Body));
            } else {
                if (NotNull(node.ExpressionBody)) {
                    parts.Push(this.PrintArrowExpressionClauseSyntax(node.ExpressionBody));
                }
                parts.Push(String(";"));
            }
            return Concat(parts);
        }
    }
}
