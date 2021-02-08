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
            this.printNewLinesInLeadingTrivia.Push(true);
            SyntaxList<AttributeListSyntax>? attributeLists = null;
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
            if (attributeLists.HasValue)
            {
                this.PrintAttributeLists(node, attributeLists.Value, parts);   
            }
            if (modifiers.HasValue)
            {
                parts.Add(this.PrintModifiers(modifiers.Value));
            }

            if (returnType != null)
            {
                // TODO 0 try out method parameters with leading/trailing comments to see how this approach works
                // TODO 0 preprocessor stuff is going to be painful, because it doesn't parse some of it. Could we figure that out somehow?
                // TODO 0 look at class/method comments tests
                // TODO 0 this should probably move into something like PredefinedTypeSyntax, but then the printedExtraNewLines doesn't work. Maybe we do just need a method for printing extra new lines in the appropriate places
                parts.Push(this.Print(returnType));
                parts.Push(SpaceIfNoPreviousComment);
            }

            this.printNewLinesInLeadingTrivia.Pop();

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