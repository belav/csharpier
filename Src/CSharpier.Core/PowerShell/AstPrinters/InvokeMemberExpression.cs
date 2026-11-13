using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class InvokeMemberExpression
{
    internal static Doc Print(InvokeMemberExpressionAst node, PrintContext context)
    {
        var arguments = node.Arguments;
        var hasArguments = arguments is { Count: > 0 };
        var afterMember = context.GetTokens(
            node.Member.Extent.EndOffset,
            hasArguments ? arguments![0].Extent.StartOffset : node.Extent.EndOffset
        );
        var openParenIndex = afterMember.FindIndex(o => o.Kind == TokenKind.LParen);
        var genericArguments = string.Concat(
            afterMember
                .Take(openParenIndex < 0 ? afterMember.Count : openParenIndex)
                .Select(o => o.Kind == TokenKind.Comma ? ", " : o.Text)
        );

        var target = Doc.Concat(
            Node.Print(node.Expression, context),
            MemberExpression.GetOperator(node),
            Node.Print(node.Member, context),
            genericArguments
        );

        if (!hasArguments)
        {
            return Doc.Concat(target, "()");
        }

        if (openParenIndex < 0)
        {
            return Doc.Concat(target, Node.Print(arguments![0], context));
        }

        if (ArgumentList.HasComments(arguments!, context))
        {
            return Doc.Concat(target, ArgumentList.Print("(", arguments!, ")", context));
        }

        var last = arguments![^1];
        if (
            (Layout.IsDelimitedBlock(last) && last is not ParenExpressionAst)
            || arguments.Any(Layout.IsMultiLineString)
        )
        {
            var docs = new List<Doc> { target, "(" };
            for (var index = 0; index < arguments.Count; index++)
            {
                if (index > 0)
                {
                    docs.Add(", ");
                }

                docs.Add(Node.Print(arguments[index], context));
            }

            docs.Add(")");
            return Doc.Concat(docs);
        }

        var contents = new List<Doc> { Doc.SoftLine };
        for (var index = 0; index < arguments.Count; index++)
        {
            if (index > 0)
            {
                contents.Add(",");
                contents.Add(Doc.Line);
            }

            contents.Add(Node.Print(arguments[index], context));
        }

        return Doc.Concat(target, Doc.Group("(", Doc.Indent(contents), Doc.SoftLine, ")"));
    }
}
