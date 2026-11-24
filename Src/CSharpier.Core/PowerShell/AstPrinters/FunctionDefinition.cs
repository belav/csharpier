using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class FunctionDefinition
{
    internal static Doc Print(FunctionDefinitionAst node, PrintContext context)
    {
        if (node.Parent is FunctionMemberAst)
        {
            return Verbatim.Print(node.Extent, context);
        }

        var keyword =
            node.IsFilter ? "filter"
            : node.IsWorkflow ? "workflow"
            : "function";

        // The tree has the name with its escapes resolved, which is not always valid source text.
        var tokens = context.GetTokens(node.Extent.StartOffset, node.Body.Extent.StartOffset);
        if (tokens.Count < 2)
        {
            return Doc.Concat(
                keyword,
                " ",
                node.Name,
                PrintParametersAndBody(node, null, null, context)
            );
        }

        var name = tokens[1];
        var commentsBeforeName = context.PrintCommentsInPlace(
            context.GetUnprintedCommentsIn(tokens[0].Extent.EndOffset, name.Extent.StartOffset),
            tokens[0].Extent.EndLineNumber,
            name.Extent.StartLineNumber
        );

        return Doc.Concat(
            keyword,
            commentsBeforeName == Doc.Null ? " " : commentsBeforeName,
            name.Text,
            PrintParametersAndBody(node, null, name, context)
        );
    }

    // Prints everything after the name. The parentheses are only there when the source has them,
    // because "function f()" and "function f" are different trees. Comments between the
    // parameters or base constructor call and the opening brace stay there, because PowerShell
    // reads a comment at the start of the body as the help of the function.
    internal static Doc PrintParametersAndBody(
        FunctionDefinitionAst node,
        BaseCtorInvokeMemberExpressionAst? baseConstructorCall,
        Token? name,
        PrintContext context
    )
    {
        var bodyStart = node.Body.Extent.StartOffset;
        var nameEnd = name?.Extent.EndOffset ?? node.Extent.StartOffset;
        var header = context.GetTokens(
            nameEnd,
            baseConstructorCall?.Extent.StartOffset ?? bodyStart
        );
        var colon = header is [.., { Kind: TokenKind.Colon } lastToken] ? lastToken : null;
        if (colon is not null)
        {
            header.RemoveAt(header.Count - 1);
        }

        var closingParenthesis = header is [.., { Kind: TokenKind.RParen } last] ? last : null;
        var headerEnd = closingParenthesis ?? name;
        var headerEndOffset = headerEnd?.Extent.EndOffset ?? nameEnd;
        var headerEndLine = headerEnd?.Extent.EndLineNumber ?? node.Extent.StartLineNumber;

        // The colon is printed on the line of the parameters, so a comment after the colon on its
        // line stays after it.
        Doc commentsBeforeBaseConstructorCall = Doc.Null;
        if (baseConstructorCall is not null)
        {
            var comments = context.GetUnprintedCommentsIn(
                headerEndOffset,
                baseConstructorCall.Extent.StartOffset
            );
            commentsBeforeBaseConstructorCall = context.PrintCommentsInPlace(
                comments,
                colon is not null
                && comments is [var first, ..]
                && first.StartOffset > colon.Extent.StartOffset
                    ? colon.Extent.EndLineNumber
                    : headerEndLine,
                baseConstructorCall.Extent.StartLineNumber
            );
            headerEndOffset = baseConstructorCall.Extent.EndOffset;
            headerEndLine = baseConstructorCall.Extent.EndLineNumber;
        }

        var commentsBeforeBody = context.PrintCommentsInPlace(
            context.GetUnprintedCommentsIn(headerEndOffset, bodyStart),
            headerEndLine,
            node.Body.Extent.StartLineNumber
        );

        var docs = new List<Doc>();
        if (closingParenthesis is not null)
        {
            docs.Add(
                ArgumentList.Print(
                    "(",
                    (IReadOnlyList<Ast>?)node.Parameters ?? [],
                    ")",
                    nameEnd,
                    closingParenthesis.Extent.StartOffset,
                    context
                )
            );
        }
        else if (node.Parameters is not null || node.Parent is FunctionMemberAst)
        {
            docs.Add("()");
        }

        if (baseConstructorCall is not null)
        {
            docs.Add(" :");
            docs.Add(
                commentsBeforeBaseConstructorCall == Doc.Null
                    ? " "
                    : commentsBeforeBaseConstructorCall
            );
            docs.Add(Node.Print(baseConstructorCall, context));
        }

        docs.Add(commentsBeforeBody == Doc.Null ? " " : commentsBeforeBody);

        var contentsOfBody = ScriptBlock.PrintContents(node.Body, context);
        if (contentsOfBody == Doc.Null)
        {
            docs.Add("{ }");
        }
        else
        {
            docs.Add("{");
            docs.Add(Doc.Indent(Doc.HardLine, contentsOfBody));
            docs.Add(Doc.HardLine);
            docs.Add("}");
        }

        return Doc.Concat(docs);
    }
}
