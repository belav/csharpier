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
            
            // TODO 0 I think I can ditch that stack, and just use this everywhere. Maybe leave the stack for now until I have comments/new lines fully done
            foreach (var leadingTrivia in node.GetLeadingTrivia())
            {
                if (leadingTrivia.Kind() == SyntaxKind.EndOfLineTrivia)
                {
                    parts.Push(HardLine);
                }
                else if (leadingTrivia.Kind() != SyntaxKind.WhitespaceTrivia)
                {
                    break;
                }
            }
            
            if (attributeLists.HasValue)
            {
                parts.Push(this.PrintAttributeLists(node, attributeLists.Value));   
            }
            if (modifiers.HasValue)
            {
                parts.Add(this.PrintModifiers(modifiers.Value));
            }

            if (returnType != null)
            {
                // TODO 0 build a validation tool, it can format a file, and then squash whitespace/newlines down to a single space and compare the results
                // TODO 0 can we write out unhandled nodes in the generic print? ToFullString() ??
                // TODO 0 review each node for leading/trailing stuff
                // TODO 0 multiline comments need a doc type
                // TODO 0 try out method parameters with leading/trailing comments to see how this approach works
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

                parts.Add(";");
            }

            return Concat(parts);
        }
    }
}