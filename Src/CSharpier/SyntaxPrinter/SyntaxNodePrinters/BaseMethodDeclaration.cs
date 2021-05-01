using System;
using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class BaseMethodDeclaration
    {
        // TODO partial - The three BaseX files in here can probably go in SyntaxNodePrinters. They are all abstract types
        // so I believe if we are generating the code correctly they will work fine.
        // The only weird one is this one, because it also needs to accept a LocalFunctionStatementSyntax
        // I think we can just add a LocalFunctionStatement.Print class/method that pass the node into BaseMethodDeclaration.Print
        // and this will continue to accept CSharpSyntaxNode
        public static Doc Print(CSharpSyntaxNode node)
        {
            SyntaxList<AttributeListSyntax>? attributeLists = null;
            SyntaxTokenList? modifiers = null;
            TypeSyntax? returnType = null;
            ExplicitInterfaceSpecifierSyntax? explicitInterfaceSpecifier = null;
            TypeParameterListSyntax? typeParameterList = null;
            Doc identifier = Doc.Null;
            var constraintClauses = Enumerable.Empty<TypeParameterConstraintClauseSyntax>();
            ParameterListSyntax? parameterList = null;
            BlockSyntax? body = null;
            ArrowExpressionClauseSyntax? expressionBody = null;
            SyntaxToken? semicolonToken = null;
            string? groupId = null;

            if (
                node is BaseMethodDeclarationSyntax baseMethodDeclarationSyntax
            ) {
                attributeLists = baseMethodDeclarationSyntax.AttributeLists;
                modifiers = baseMethodDeclarationSyntax.Modifiers;
                parameterList = baseMethodDeclarationSyntax.ParameterList;
                body = baseMethodDeclarationSyntax.Body;
                expressionBody = baseMethodDeclarationSyntax.ExpressionBody;
                if (node is MethodDeclarationSyntax methodDeclarationSyntax)
                {
                    returnType = methodDeclarationSyntax.ReturnType;
                    explicitInterfaceSpecifier = methodDeclarationSyntax.ExplicitInterfaceSpecifier;
                    identifier = Token.Print(
                        methodDeclarationSyntax.Identifier
                    );
                    typeParameterList = methodDeclarationSyntax.TypeParameterList;
                    constraintClauses = methodDeclarationSyntax.ConstraintClauses;
                }
                else if (
                    node is DestructorDeclarationSyntax destructorDeclarationSyntax
                ) {
                    identifier = Doc.Concat(
                        Token.Print(destructorDeclarationSyntax.TildeToken),
                        Token.Print(destructorDeclarationSyntax.Identifier)
                    );
                }
                else if (
                    node is ConstructorDeclarationSyntax constructorDeclarationSyntax
                ) {
                    identifier = Token.Print(
                        constructorDeclarationSyntax.Identifier
                    );
                }

                semicolonToken = baseMethodDeclarationSyntax.SemicolonToken;
            }
            else if (
                node is LocalFunctionStatementSyntax localFunctionStatementSyntax
            ) {
                attributeLists = localFunctionStatementSyntax.AttributeLists;
                modifiers = localFunctionStatementSyntax.Modifiers;
                returnType = localFunctionStatementSyntax.ReturnType;
                identifier = Token.Print(
                    localFunctionStatementSyntax.Identifier
                );
                typeParameterList = localFunctionStatementSyntax.TypeParameterList;
                parameterList = localFunctionStatementSyntax.ParameterList;
                constraintClauses = localFunctionStatementSyntax.ConstraintClauses;
                body = localFunctionStatementSyntax.Body;
                expressionBody = localFunctionStatementSyntax.ExpressionBody;
                semicolonToken = localFunctionStatementSyntax.SemicolonToken;
            }

            var docs = new List<Doc> { ExtraNewLines.Print(node) };

            if (attributeLists.HasValue)
            {
                docs.Add(AttributeLists.Print(node, attributeLists.Value));
            }

            var declarationGroup = new List<Doc>();

            if (modifiers.HasValue)
            {
                declarationGroup.Add(Modifiers.Print(modifiers.Value));
            }

            if (returnType != null)
            {
                // TODO 1 preprocessor stuff is going to be painful, because it doesn't parse some of it. Could we figure that out somehow? that may get complicated
                declarationGroup.Add(Node.Print(returnType), " ");
            }

            if (explicitInterfaceSpecifier != null)
            {
                declarationGroup.Add(
                    Node.Print(explicitInterfaceSpecifier.Name),
                    Token.Print(explicitInterfaceSpecifier.DotToken)
                );
            }

            if (identifier != Doc.Null)
            {
                declarationGroup.Add(identifier);
            }

            if (
                node is ConversionOperatorDeclarationSyntax conversionOperatorDeclarationSyntax
            ) {
                declarationGroup.Add(
                    Token.PrintWithSuffix(
                        conversionOperatorDeclarationSyntax.ImplicitOrExplicitKeyword,
                        " "
                    ),
                    Token.Print(
                        conversionOperatorDeclarationSyntax.OperatorKeyword,
                        " "
                    ),
                    Node.Print(conversionOperatorDeclarationSyntax.Type)
                );
            }
            else if (
                node is OperatorDeclarationSyntax operatorDeclarationSyntax
            ) {
                declarationGroup.Add(
                    Node.Print(operatorDeclarationSyntax.ReturnType),
                    " ",
                    Token.Print(operatorDeclarationSyntax.OperatorKeyword, " "),
                    Token.Print(operatorDeclarationSyntax.OperatorToken)
                );
            }

            if (typeParameterList != null)
            {
                declarationGroup.Add(
                    TypeParameterList.Print(typeParameterList)
                );
            }

            if (parameterList != null)
            {
                // if there are no parameters, but there is a super long method name, a groupId
                // will cause SpaceBrace when it isn't wanted.
                if (parameterList.Parameters.Count > 0)
                {
                    groupId = Guid.NewGuid().ToString();
                }
                declarationGroup.Add(
                    ParameterList.Print(parameterList, groupId)
                );
                declarationGroup.Add(Doc.IfBreak(Doc.Null, Doc.SoftLine));
            }

            docs.Add(Doc.Group(declarationGroup));

            docs.Add(ConstraintClauses.Print(constraintClauses));
            if (body != null)
            {
                docs.Add(
                    groupId != null
                        ? Block.PrintWithConditionalSpace(body, groupId)
                        : Block.Print(body)
                );
            }
            else
            {
                if (expressionBody != null)
                {
                    docs.Add(ArrowExpressionClause.Print(expressionBody));
                }
            }

            if (semicolonToken.HasValue)
            {
                docs.Add(Token.Print(semicolonToken.Value));
            }

            return Doc.Group(docs);
        }
    }
}
