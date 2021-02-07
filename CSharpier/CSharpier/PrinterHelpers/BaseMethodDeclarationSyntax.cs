using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintBaseMethodDeclarationSyntax(CSharpSyntaxNode node)
        {
            SyntaxList<AttributeListSyntax> attributeLists;
            SyntaxTokenList? modifiers = null;
            TypeSyntax returnType = null;
            ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier = null;
            TypeParameterListSyntax typeParameterList = null;
            string identifier = null;
            var constraintClauses = Enumerable.Empty<TypeParameterConstraintClauseSyntax>();
            ParameterListSyntax parameterList = null;
            BlockSyntax body = null;
            ArrowExpressionClauseSyntax expressionBody = null;
            if (node is BaseMethodDeclarationSyntax baseMethodDeclarationSyntax)
            {
                attributeLists = baseMethodDeclarationSyntax.AttributeLists;
                modifiers = baseMethodDeclarationSyntax.Modifiers;
                parameterList = baseMethodDeclarationSyntax.ParameterList;
                body = baseMethodDeclarationSyntax.Body;
                expressionBody = baseMethodDeclarationSyntax.ExpressionBody;
                if (node is MethodDeclarationSyntax methodDeclarationSyntax)
                {
                    returnType = methodDeclarationSyntax.ReturnType;
                    explicitInterfaceSpecifier = methodDeclarationSyntax.ExplicitInterfaceSpecifier;
                    identifier = methodDeclarationSyntax.Identifier.Text;
                    typeParameterList = methodDeclarationSyntax.TypeParameterList;
                    constraintClauses = methodDeclarationSyntax.ConstraintClauses;
                }
            }
            else if (node is LocalFunctionStatementSyntax localFunctionStatementSyntax)
            {
                attributeLists = localFunctionStatementSyntax.AttributeLists;
                modifiers = localFunctionStatementSyntax.Modifiers;
                returnType = localFunctionStatementSyntax.ReturnType;
                identifier = localFunctionStatementSyntax.Identifier.Text;
                typeParameterList = localFunctionStatementSyntax.TypeParameterList;
                parameterList = localFunctionStatementSyntax.ParameterList;
                constraintClauses = localFunctionStatementSyntax.ConstraintClauses;
                body = localFunctionStatementSyntax.Body;
                expressionBody = localFunctionStatementSyntax.ExpressionBody;
            }

            var parts = new Parts();
            var printedExtraNewLines = false;
            //this.PrintExtraNewLines(node, String("attributeLists"), String("modifiers"), [String("returnType"), String("keyword")]);
            this.PrintAttributeLists(node, attributeLists, parts);
            // printLeadingComments(node, parts, String("modifiers"), String("returnType"), String("identifier"));
            if (modifiers.HasValue)
            {
                parts.Add(this.PrintModifiers(modifiers.Value, ref printedExtraNewLines));
            }

            if (returnType != null)
            {
                // TODO 0 preprocessor stuff is going to be painful, because it doesn't parse some of it. Could we figure that out somehow?
                // TODO 0 another option for comments would be to make them a doc type, and then propagate breaks based on what we find
                // single line comments (both leading & trailing) should always have a line after
                // leading single line comments should always have a line before
                // TODO 0 look at class/method comments tests
                // TODO 0 this should probably move into something like PredefinedTypeSyntax, but then the printedExtraNewLines doesn't work. Maybe we do just need a method for printing extra new lines in the appropriate places
                PrintLeadingTrivia(returnType, parts, ref printedExtraNewLines);
                parts.Push(this.Print(returnType));
                if (!PrintTrailingTrivia(returnType, parts))
                {
                    parts.Push(" ");
                }
            }

            if (explicitInterfaceSpecifier != null)
            {
                parts.Push(this.Print(explicitInterfaceSpecifier.Name), ".");
            }

            if (identifier != null)
            {
                parts.Add(identifier);
            }

            if (node is ConversionOperatorDeclarationSyntax conversionOperatorDeclarationSyntax)
            {
                parts.Push(conversionOperatorDeclarationSyntax.ImplicitOrExplicitKeyword.Text,
                    " ",
                    conversionOperatorDeclarationSyntax.OperatorKeyword.Text,
                    " ",
                    this.Print(conversionOperatorDeclarationSyntax.Type));
            }
            else if (node is OperatorDeclarationSyntax operatorDeclarationSyntax)
            {
                parts.Push(this.Print(operatorDeclarationSyntax.ReturnType), " ", operatorDeclarationSyntax.OperatorKeyword.Text, " ", operatorDeclarationSyntax.OperatorToken.Text);
            }

            if (typeParameterList != null)
            {
                parts.Add(this.PrintTypeParameterListSyntax(typeParameterList));
            }

            if (parameterList != null)
            {
                parts.Add(this.Print(parameterList));
            }

            this.PrintConstraintClauses(node, constraintClauses, parts);
            if (body != null)
            {
                parts.Add(this.PrintBlockSyntax(body));
            }
            else
            {
                if (expressionBody != null)
                {
                    parts.Add(this.PrintArrowExpressionClauseSyntax(expressionBody));
                }

                parts.Add(String(";"));
            }

            return Concat(parts);
        }
    }
}