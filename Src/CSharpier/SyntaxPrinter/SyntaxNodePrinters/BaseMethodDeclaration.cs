namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class BaseMethodDeclaration
{
    public static Doc Print(CSharpSyntaxNode node, FormattingContext context)
    {
        SyntaxList<AttributeListSyntax>? attributeLists = null;
        SyntaxTokenList? modifiers = null;
        TypeSyntax? returnType = null;
        ExplicitInterfaceSpecifierSyntax? explicitInterfaceSpecifier = null;
        TypeParameterListSyntax? typeParameterList = null;
        Func<Doc>? identifier = null;
        SyntaxList<TypeParameterConstraintClauseSyntax>? constraintClauses = null;
        ParameterListSyntax? parameterList = null;
        ConstructorInitializerSyntax? constructorInitializer = null;
        BlockSyntax? body = null;
        ArrowExpressionClauseSyntax? expressionBody = null;
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
                identifier = () => Token.Print(methodDeclarationSyntax.Identifier, context);
                typeParameterList = methodDeclarationSyntax.TypeParameterList;
                constraintClauses = methodDeclarationSyntax.ConstraintClauses;
            }
            else if (node is DestructorDeclarationSyntax destructorDeclarationSyntax)
            {
                identifier = () =>
                    Doc.Concat(
                        Token.Print(destructorDeclarationSyntax.TildeToken, context),
                        Token.Print(destructorDeclarationSyntax.Identifier, context)
                    );
            }
            else if (node is ConstructorDeclarationSyntax constructorDeclarationSyntax)
            {
                identifier = () => Token.Print(constructorDeclarationSyntax.Identifier, context);
                constructorInitializer = constructorDeclarationSyntax.Initializer;
            }

            semicolonToken = baseMethodDeclarationSyntax.SemicolonToken;
        }
        else if (node is LocalFunctionStatementSyntax localFunctionStatementSyntax)
        {
            attributeLists = localFunctionStatementSyntax.AttributeLists;
            modifiers = localFunctionStatementSyntax.Modifiers;
            returnType = localFunctionStatementSyntax.ReturnType;
            identifier = () => Token.Print(localFunctionStatementSyntax.Identifier, context);
            typeParameterList = localFunctionStatementSyntax.TypeParameterList;
            parameterList = localFunctionStatementSyntax.ParameterList;
            constraintClauses = localFunctionStatementSyntax.ConstraintClauses;
            body = localFunctionStatementSyntax.Body;
            expressionBody = localFunctionStatementSyntax.ExpressionBody;
            semicolonToken = localFunctionStatementSyntax.SemicolonToken;
        }

        var docs = new List<Doc>();

        if (node is LocalFunctionStatementSyntax)
        {
            docs.Add(ExtraNewLines.Print(node));
        }

        if (attributeLists is { Count: > 0 })
        {
            docs.Add(AttributeLists.Print(node, attributeLists.Value, context));
        }

        if (modifiers is { Count: > 0 })
        {
            docs.Add(Token.PrintLeadingTrivia(modifiers.Value[0], context));
        }
        else if (returnType != null)
        {
            docs.Add(Token.PrintLeadingTrivia(returnType.GetLeadingTrivia(), context));
        }

        var declarationGroup = new List<Doc>();

        if (modifiers.HasValue && modifiers.Value.Any())
        {
            declarationGroup.Add(Modifiers.PrintWithoutLeadingTrivia(modifiers.Value, context));
        }

        if (returnType != null)
        {
            if (modifiers is not { Count: > 0 })
            {
                context.ShouldSkipNextLeadingTrivia = true;
            }

            declarationGroup.Add(Node.Print(returnType, context), " ");
            context.ShouldSkipNextLeadingTrivia = false;
        }

        if (explicitInterfaceSpecifier != null)
        {
            declarationGroup.Add(
                Node.Print(explicitInterfaceSpecifier.Name, context),
                Token.Print(explicitInterfaceSpecifier.DotToken, context)
            );
        }

        if (identifier != null)
        {
            declarationGroup.Add(identifier());
        }

        if (node is ConversionOperatorDeclarationSyntax conversionOperatorDeclarationSyntax)
        {
            declarationGroup.Add(
                Token.PrintWithSuffix(
                    conversionOperatorDeclarationSyntax.ImplicitOrExplicitKeyword,
                    " ",
                    context
                ),
                Token.PrintWithSuffix(
                    conversionOperatorDeclarationSyntax.OperatorKeyword,
                    " ",
                    context
                ),
                Node.Print(conversionOperatorDeclarationSyntax.Type, context)
            );
        }
        else if (node is OperatorDeclarationSyntax operatorDeclarationSyntax)
        {
            declarationGroup.Add(
                Node.Print(operatorDeclarationSyntax.ReturnType, context),
                " ",
                Token.PrintWithSuffix(operatorDeclarationSyntax.OperatorKeyword, " ", context),
                Token.Print(operatorDeclarationSyntax.OperatorToken, context)
            );
        }

        if (typeParameterList != null && typeParameterList.Parameters.Any())
        {
            declarationGroup.Add(TypeParameterList.Print(typeParameterList, context));
        }

        if (parameterList != null)
        {
            if (parameterList.Parameters.Any())
            {
                declarationGroup.Add(ParameterList.Print(parameterList, context));
            }
            else
            {
                declarationGroup.Add(
                    Token.Print(parameterList.OpenParenToken, context),
                    Token.Print(parameterList.CloseParenToken, context)
                );
            }

            declarationGroup.Add(Doc.IfBreak(Doc.Null, Doc.SoftLine));
        }

        if (constructorInitializer != null)
        {
            var colonToken = Token.PrintWithSuffix(constructorInitializer.ColonToken, " ", context);
            var argumentList = Doc.Group(
                ArgumentList.Print(constructorInitializer.ArgumentList, context)
            );

            declarationGroup.Add(
                Doc.Group(
                    Doc.Indent(Doc.Line),
                    Doc.Indent(colonToken),
                    Token.Print(constructorInitializer.ThisOrBaseKeyword, context),
                    Doc.Indent(argumentList)
                )
            );
        }

        docs.Add(Doc.Group(declarationGroup));

        if (constraintClauses != null)
        {
            docs.Add(ConstraintClauses.Print(constraintClauses, context));
        }

        if (body != null)
        {
            docs.Add(Block.Print(body, context));
        }
        else
        {
            if (expressionBody != null)
            {
                docs.Add(ArrowExpressionClause.Print(expressionBody, context));
            }
        }

        if (semicolonToken.HasValue)
        {
            docs.Add(Token.Print(semicolonToken.Value, context));
        }

        return Doc.Group(docs);
    }
}
