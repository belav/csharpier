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
            SyntaxList<AttributeListSyntax>? attributeLists = null;
            SyntaxTokenList? modifiers = null;
            TypeSyntax returnType = null;
            ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier = null;
            TypeParameterListSyntax typeParameterList = null;
            Doc identifier = null;
            var constraintClauses = Enumerable.Empty<TypeParameterConstraintClauseSyntax>();
            ParameterListSyntax parameterList = null;
            BlockSyntax body = null;
            ArrowExpressionClauseSyntax expressionBody = null;
            SyntaxToken? semicolonToken = null;
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
                    identifier = this.PrintSyntaxToken(
                        methodDeclarationSyntax.Identifier);
                    typeParameterList = methodDeclarationSyntax.TypeParameterList;
                    constraintClauses = methodDeclarationSyntax.ConstraintClauses;
                }

                semicolonToken = baseMethodDeclarationSyntax.SemicolonToken;
            }
            else if (
                node is LocalFunctionStatementSyntax localFunctionStatementSyntax
            )
            {
                attributeLists = localFunctionStatementSyntax.AttributeLists;
                modifiers = localFunctionStatementSyntax.Modifiers;
                returnType = localFunctionStatementSyntax.ReturnType;
                identifier = this.PrintSyntaxToken(
                    localFunctionStatementSyntax.Identifier);
                typeParameterList = localFunctionStatementSyntax.TypeParameterList;
                parameterList = localFunctionStatementSyntax.ParameterList;
                constraintClauses = localFunctionStatementSyntax.ConstraintClauses;
                body = localFunctionStatementSyntax.Body;
                expressionBody = localFunctionStatementSyntax.ExpressionBody;
                semicolonToken = localFunctionStatementSyntax.SemicolonToken;
            }

            var parts = new Parts();
            parts.Push(this.PrintExtraNewLines(node));

            if (attributeLists.HasValue)
            {
                parts.Push(
                    this.PrintAttributeLists(node, attributeLists.Value));
            }
            if (modifiers.HasValue)
            {
                parts.Push(this.PrintModifiers(modifiers.Value));
            }

            if (returnType != null)
            {
                // TODO 1 preprocessor stuff is going to be painful, because it doesn't parse some of it. Could we figure that out somehow? that may get complicated
                parts.Push(this.Print(returnType));
                parts.Push(SpaceIfNoPreviousComment);
            }

            if (explicitInterfaceSpecifier != null)
            {
                parts.Push(
                    this.Print(explicitInterfaceSpecifier.Name),
                    this.PrintSyntaxToken(explicitInterfaceSpecifier.DotToken));
            }

            if (identifier != null)
            {
                parts.Push(identifier);
            }

            if (
                node is ConversionOperatorDeclarationSyntax conversionOperatorDeclarationSyntax
            )
            {
                parts.Push(
                    this.PrintSyntaxToken(
                        conversionOperatorDeclarationSyntax.ImplicitOrExplicitKeyword,
                        " "),
                    this.PrintSyntaxToken(
                        conversionOperatorDeclarationSyntax.OperatorKeyword,
                        " "),
                    this.Print(conversionOperatorDeclarationSyntax.Type));
            }
            else if (node is OperatorDeclarationSyntax operatorDeclarationSyntax)
            {
                parts.Push(
                    this.Print(operatorDeclarationSyntax.ReturnType),
                    SpaceIfNoPreviousComment,
                    this.PrintSyntaxToken(
                        operatorDeclarationSyntax.OperatorKeyword,
                        " "),
                    this.PrintSyntaxToken(
                        operatorDeclarationSyntax.OperatorToken));
            }

            if (typeParameterList != null)
            {
                parts.Push(
                    this.PrintTypeParameterListSyntax(typeParameterList));
            }

            if (parameterList != null)
            {
                parts.Push(this.PrintParameterListSyntax(parameterList));
            }

            parts.Push(this.PrintConstraintClauses(node, constraintClauses));
            if (body != null)
            {
                parts.Push(this.PrintBlockSyntax(body));
            }
            else
            {
                if (expressionBody != null)
                {
                    parts.Push(
                        this.PrintArrowExpressionClauseSyntax(expressionBody));
                }
            }

            if (semicolonToken.HasValue)
            {
                parts.Push(this.PrintSyntaxToken(semicolonToken.Value));
            }

            return Concat(parts);
        }
    }
}
