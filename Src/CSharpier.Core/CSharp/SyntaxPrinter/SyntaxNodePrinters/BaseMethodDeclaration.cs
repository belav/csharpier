using System.Text.RegularExpressions;
using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static partial class BaseMethodDeclaration
{
#if NET8_0_OR_GREATER

    [GeneratedRegex(@"\s*(\r\n?|\n)")]
    private static partial Regex RemoveWhiteSpaceLineEndingsGenerator();

    private static readonly Regex RemoveWhiteSpaceLineEndingsRegex =
        RemoveWhiteSpaceLineEndingsGenerator();
#else
    private static readonly Regex RemoveWhiteSpaceLineEndingsRegex = new(@"\s*(\r\n?|\n)");
#endif

    public static Doc Print(CSharpSyntaxNode node, PrintingContext context)
    {
        SyntaxList<AttributeListSyntax>? attributeLists = null;
        SyntaxTokenList? modifiers = null;
        TypeSyntax? returnType = null;
        ExplicitInterfaceSpecifierSyntax? explicitInterfaceSpecifier = null;
        TypeParameterListSyntax? typeParameterList = null;
        Func<CSharpSyntaxNode, PrintingContext, Doc>? identifier = null;
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
                identifier = static (node, context) =>
                {
                    var methodDeclarationSyntax = (MethodDeclarationSyntax)node;
                    return Token.Print(methodDeclarationSyntax.Identifier, context);
                };
                typeParameterList = methodDeclarationSyntax.TypeParameterList;
                constraintClauses = methodDeclarationSyntax.ConstraintClauses;
            }
            else if (node is DestructorDeclarationSyntax)
            {
                identifier = static (node, context) =>
                {
                    var destructorDeclarationSyntax = (DestructorDeclarationSyntax)node;
                    return Doc.Concat(
                        Token.Print(destructorDeclarationSyntax.TildeToken, context),
                        Token.Print(destructorDeclarationSyntax.Identifier, context)
                    );
                };
            }
            else if (node is ConstructorDeclarationSyntax constructorDeclarationSyntax)
            {
                identifier = static (node, context) =>
                {
                    var constructorDeclarationSyntax = (ConstructorDeclarationSyntax)node;
                    return Token.Print(constructorDeclarationSyntax.Identifier, context);
                };
                constructorInitializer = constructorDeclarationSyntax.Initializer;
            }

            semicolonToken = baseMethodDeclarationSyntax.SemicolonToken;
        }
        else if (node is LocalFunctionStatementSyntax localFunctionStatementSyntax)
        {
            attributeLists = localFunctionStatementSyntax.AttributeLists;
            modifiers = localFunctionStatementSyntax.Modifiers;
            returnType = localFunctionStatementSyntax.ReturnType;
            identifier = static (node, context) =>
            {
                var localFunctionStatementSyntax = (LocalFunctionStatementSyntax)node;
                return Token.Print(localFunctionStatementSyntax.Identifier, context);
            };
            typeParameterList = localFunctionStatementSyntax.TypeParameterList;
            parameterList = localFunctionStatementSyntax.ParameterList;
            constraintClauses = localFunctionStatementSyntax.ConstraintClauses;
            body = localFunctionStatementSyntax.Body;
            expressionBody = localFunctionStatementSyntax.ExpressionBody;
            semicolonToken = localFunctionStatementSyntax.SemicolonToken;
        }

        var docs = new ValueListBuilder<Doc>([null, null, null, null, null, null, null, null]);

        if (node is LocalFunctionStatementSyntax)
        {
            docs.Append(ExtraNewLines.Print(node));
        }

        if (attributeLists is { Count: > 0 })
        {
            docs.Append(AttributeLists.Print(node, attributeLists.Value, context));

            void PrintMethodUnformattedWithoutAttributes(
                SyntaxTriviaList syntaxTriviaList,
                ref ValueListBuilder<Doc> docs
            )
            {
                var attributeStart = attributeLists
                    .Value[0]
                    .GetLeadingTrivia()
                    .First()
                    .GetLocation()
                    .SourceSpan.Start;

                var methodWithoutAttributes = node.GetText()
                    .Replace(
                        0,
                        syntaxTriviaList.First().GetLocation().SourceSpan.Start - attributeStart,
                        string.Empty
                    )
                    .ToString()
                    .Trim();

                docs.Append(
                    RemoveWhiteSpaceLineEndingsRegex.Replace(
                        methodWithoutAttributes,
                        context.Options.LineEnding
                    )
                );
            }

            if (context.Information.HasCSharpierIgnore)
            {
                if (modifiers is { Count: > 0 })
                {
                    if (CSharpierIgnore.HasIgnoreComment(modifiers.Value[0]))
                    {
                        PrintMethodUnformattedWithoutAttributes(
                            modifiers.Value[0].LeadingTrivia,
                            ref docs
                        );
                        var returnDoc = Doc.Group(ref docs);
                        docs.Dispose();
                        return returnDoc;
                    }
                }
                else if (returnType is not null && CSharpierIgnore.HasIgnoreComment(returnType))
                {
                    PrintMethodUnformattedWithoutAttributes(
                        returnType.GetLeadingTrivia(),
                        ref docs
                    );
                    var returnDoc = Doc.Group(ref docs);
                    docs.Dispose();
                    return returnDoc;
                }
            }
        }

        var declarationGroup = new ValueListBuilder<Doc>(
            [null, null, null, null, null, null, null, null]
        );

        if (modifiers is { Count: > 0 })
        {
            docs.Append(Token.PrintLeadingTrivia(modifiers.Value[0], context));
            declarationGroup.Append(
                Modifiers.PrintSorterWithoutLeadingTrivia(modifiers.Value, context)
            );
        }

        if (returnType != null)
        {
            if (modifiers is not { Count: > 0 })
            {
                docs.Append(Token.PrintLeadingTrivia(returnType.GetLeadingTrivia(), context));
                context.State.SkipNextLeadingTrivia = true;
            }

            declarationGroup.Append(Node.Print(returnType, context), " ");
            context.State.SkipNextLeadingTrivia = false;
        }

        if (explicitInterfaceSpecifier != null)
        {
            declarationGroup.Append(
                Node.Print(explicitInterfaceSpecifier.Name, context),
                Token.Print(explicitInterfaceSpecifier.DotToken, context)
            );
        }

        if (identifier != null)
        {
            declarationGroup.Append(identifier(node, context));
        }

        if (node is ConversionOperatorDeclarationSyntax conversionOperatorDeclarationSyntax)
        {
            declarationGroup.Append(
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
                Token.PrintWithSuffix(
                    conversionOperatorDeclarationSyntax.CheckedKeyword,
                    " ",
                    context
                ),
                Node.Print(conversionOperatorDeclarationSyntax.Type, context)
            );
        }
        else if (node is OperatorDeclarationSyntax operatorDeclarationSyntax)
        {
            declarationGroup.Append(
                Node.Print(operatorDeclarationSyntax.ReturnType, context),
                " ",
                operatorDeclarationSyntax.ExplicitInterfaceSpecifier is not null
                    ? Node.Print(operatorDeclarationSyntax.ExplicitInterfaceSpecifier, context)
                    : Doc.Null,
                Token.PrintWithSuffix(operatorDeclarationSyntax.OperatorKeyword, " ", context),
                Token.PrintWithSuffix(operatorDeclarationSyntax.CheckedKeyword, " ", context),
                Token.Print(operatorDeclarationSyntax.OperatorToken, context)
            );
        }

        if (typeParameterList != null && typeParameterList.Parameters.Any())
        {
            declarationGroup.Append(TypeParameterList.Print(typeParameterList, context));
        }

        if (parameterList != null)
        {
            if (parameterList.Parameters.Any())
            {
                declarationGroup.Append(ParameterList.Print(parameterList, context));
            }
            else
            {
                declarationGroup.Append(
                    Token.Print(parameterList.OpenParenToken, context),
                    Token.Print(parameterList.CloseParenToken, context)
                );
            }

            declarationGroup.Append(Doc.IfBreak(Doc.Null, Doc.SoftLine));
        }

        if (constructorInitializer != null)
        {
            var colonToken = Token.PrintWithSuffix(constructorInitializer.ColonToken, " ", context);
            var argumentList = Doc.Group(
                ArgumentList.Print(constructorInitializer.ArgumentList, context)
            );

            declarationGroup.Append(
                Doc.Group(
                    Doc.Indent(Doc.HardLine),
                    Doc.Indent(colonToken),
                    Token.Print(constructorInitializer.ThisOrBaseKeyword, context),
                    Doc.Indent(argumentList)
                )
            );
        }

        docs.Append(Doc.Group(ref declarationGroup));
        declarationGroup.Dispose();

        if (constraintClauses != null)
        {
            docs.Append(ConstraintClauses.Print(constraintClauses.Value, context));
        }

        if (body != null)
        {
            docs.Append(Block.Print(body, context));
        }
        else
        {
            if (expressionBody != null)
            {
                docs.Append(ArrowExpressionClause.Print(expressionBody, context));
            }
        }

        if (semicolonToken.HasValue)
        {
            docs.Append(Token.Print(semicolonToken.Value, context));
        }

        return Doc.Group(ref docs);
    }
}
