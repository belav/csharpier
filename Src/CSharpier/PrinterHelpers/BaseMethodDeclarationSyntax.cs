using System;
using System.Collections.Generic;
using System.Linq;
using CSharpier.SyntaxPrinter;
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
            TypeSyntax? returnType = null;
            ExplicitInterfaceSpecifierSyntax? explicitInterfaceSpecifier = null;
            TypeParameterListSyntax? typeParameterList = null;
            Doc identifier = Docs.Null;
            var constraintClauses = Enumerable.Empty<TypeParameterConstraintClauseSyntax>();
            ParameterListSyntax? parameterList = null;
            BlockSyntax? body = null;
            ArrowExpressionClauseSyntax? expressionBody = null;
            SyntaxToken? semicolonToken = null;
            string? groupId = null;

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
                        methodDeclarationSyntax.Identifier
                    );
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
                identifier = SyntaxTokens.Print(
                    localFunctionStatementSyntax.Identifier
                );
                typeParameterList = localFunctionStatementSyntax.TypeParameterList;
                parameterList = localFunctionStatementSyntax.ParameterList;
                constraintClauses = localFunctionStatementSyntax.ConstraintClauses;
                body = localFunctionStatementSyntax.Body;
                expressionBody = localFunctionStatementSyntax.ExpressionBody;
                semicolonToken = localFunctionStatementSyntax.SemicolonToken;
            }

            var parts = new List<Doc>();
            parts.Add(this.PrintExtraNewLines(node));

            if (attributeLists.HasValue)
            {
                parts.Add(this.PrintAttributeLists(node, attributeLists.Value));
            }
            if (modifiers.HasValue)
            {
                parts.Add(this.PrintModifiers(modifiers.Value));
            }

            if (returnType != null)
            {
                // TODO 1 preprocessor stuff is going to be painful, because it doesn't parse some of it. Could we figure that out somehow? that may get complicated
                parts.Add(this.Print(returnType), SpaceIfNoPreviousComment);
            }

            if (explicitInterfaceSpecifier != null)
            {
                parts.Add(
                    this.Print(explicitInterfaceSpecifier.Name),
                    this.PrintSyntaxToken(explicitInterfaceSpecifier.DotToken)
                );
            }

            if (identifier != Docs.Null)
            {
                parts.Add(identifier);
            }

            if (
                node is ConversionOperatorDeclarationSyntax conversionOperatorDeclarationSyntax
            )
            {
                parts.Add(
                    this.PrintSyntaxToken(
                        conversionOperatorDeclarationSyntax.ImplicitOrExplicitKeyword,
                        " "
                    ),
                    this.PrintSyntaxToken(
                        conversionOperatorDeclarationSyntax.OperatorKeyword,
                        " "
                    ),
                    this.Print(conversionOperatorDeclarationSyntax.Type)
                );
            }
            else if (
                node is OperatorDeclarationSyntax operatorDeclarationSyntax
            )
            {
                parts.Add(
                    this.Print(operatorDeclarationSyntax.ReturnType),
                    SpaceIfNoPreviousComment,
                    this.PrintSyntaxToken(
                        operatorDeclarationSyntax.OperatorKeyword,
                        " "
                    ),
                    this.PrintSyntaxToken(
                        operatorDeclarationSyntax.OperatorToken
                    )
                );
            }

            if (typeParameterList != null)
            {
                parts.Add(this.PrintTypeParameterListSyntax(typeParameterList));
            }

            if (parameterList != null)
            {
                // if there are no parameters, but there is a super long method name, a groupId
                // will cause SpaceBrace when it isn't wanted.
                if (parameterList.Parameters.Count > 0)
                {
                    groupId = Guid.NewGuid().ToString();
                }
                parts.Add(
                    this.PrintParameterListSyntax(parameterList, groupId)
                );
            }

            parts.Add(this.PrintConstraintClauses(node, constraintClauses));
            if (body != null)
            {
                parts.Add(
                    groupId != null
                        ? this.PrintBlockSyntaxWithConditionalSpace(
                            body,
                            groupId
                        )
                        : this.PrintBlockSyntax(body)
                );
            }
            else
            {
                if (expressionBody != null)
                {
                    parts.Add(
                        this.PrintArrowExpressionClauseSyntax(expressionBody)
                    );
                }
            }

            if (semicolonToken.HasValue)
            {
                parts.Add(this.PrintSyntaxToken(semicolonToken.Value));
            }

            return Docs.Concat(parts);
        }
    }
}
