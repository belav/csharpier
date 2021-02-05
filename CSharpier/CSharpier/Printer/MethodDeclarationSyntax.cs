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
            this.PrintAttributeLists(node, node.AttributeLists, parts);
            //printLeadingComments(node, parts, String("modifiers"), String("returnType"), String("identifier"));
            parts.Add(this.PrintModifiers(node.Modifiers));
            if (node.ReturnType != null)
            {
                parts.Push(this.Print(node.ReturnType), String(" "));
            }

            if (node.ExplicitInterfaceSpecifier != null)
            {
                // TODO this doesn't appear valid ? parts.Add(printIdentifier(node.ExplicitInterfaceSpecifier.name), ".");
            }

            // TODO this is only true if this isn't a method, I think
            if (node.Identifier.RawKind != 0)
            {
                parts.Add(node.Identifier.Text);
            }

            // TODO non method stuff
            // if (node.ImplicitOrExplicitKeyword != null) {
            //     parts.Add(printSyntaxToken(node.ImplicitOrExplicitKeyword), String(" "));
            // }
            // if (node.OperatorKeyword != null) {
            //     parts.Add(String("operator "));
            // }
            // if (node.OperatorToken != null) {
            //     parts.Add(printSyntaxToken(node.OperatorToken));
            // }
            // if (node.Type != null) {
            //     parts.Add(this.Print(node.Type));
            // }
            if (node.TypeParameterList != null)
            {
                parts.Add(this.PrintTypeParameterListSyntax(node.TypeParameterList));
            }

            parts.Add(this.Print(node.ParameterList));
            this.PrintConstraintClauses(node, node.ConstraintClauses, parts);
            if (node.Body != null)
            {
                parts.Add(this.PrintBlockSyntax(node.Body));
            }
            else
            {
                if (node.ExpressionBody != null)
                {
                    parts.Add(this.PrintArrowExpressionClauseSyntax(node.ExpressionBody));
                }

                parts.Add(String(";"));
            }

            return Concat(parts);
        }
    }
}