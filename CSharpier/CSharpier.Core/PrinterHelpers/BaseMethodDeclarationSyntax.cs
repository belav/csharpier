using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintBaseMethodDeclarationSyntax(CSharpSyntaxNode node)
        {
            // TODO this.printNewLinesInLeadingTrivia.Push(true);
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
            parts.Push(this.PrintExtraNewLines(node));

            if (attributeLists.HasValue)
            {
                parts.Push(this.PrintAttributeLists(node, attributeLists.Value));   
            }
            if (modifiers.HasValue)
            {
                parts.Push(this.PrintModifiers(modifiers.Value));
            }

            if (returnType != null)
            {
                // TODO 0 build a validation tool, it can format a file, and then squash whitespace/newlines down to a single space and compare the results
                // TODO 0 can we write out unhandled nodes in the generic print? ToFullString() ??
                // TODO 0 preprocessor stuff is going to be painful, because it doesn't parse some of it. Could we figure that out somehow? that may get complicated
                parts.Push(this.Print(returnType));
                parts.Push(SpaceIfNoPreviousComment);
            }

            // TODO this.printNewLinesInLeadingTrivia.Pop();

            if (explicitInterfaceSpecifier != null)
            {
                parts.Push(this.Print(explicitInterfaceSpecifier.Name), ".");
            }

            if (identifier != null)
            {
                parts.Push(identifier);
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
                parts.Push(this.PrintTypeParameterListSyntax(typeParameterList));
            }

            if (parameterList != null)
            {
                parts.Push(this.PrintParameterListSyntax(parameterList));
            }

            this.PrintConstraintClauses(node, constraintClauses, parts);
            if (body != null)
            {
                parts.Push(this.PrintBlockSyntax(body));
            }
            else
            {
                if (expressionBody != null)
                {
                    parts.Push(this.PrintArrowExpressionClauseSyntax(expressionBody));
                }

                parts.Push(";");
            }

            return Concat(parts);
        }
    }
}