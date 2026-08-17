using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class ConstantExpression
{
    internal static Doc Print(ConstantExpressionAst node, PrintContext context)
    {
        if (node is StringConstantExpressionAst stringNode)
        {
            return stringNode.StringConstantType switch
            {
                StringConstantType.DoubleQuoted => "\"" + stringNode.Value + "\"",
                StringConstantType.SingleQuoted => "'" + stringNode.Value + "'",
                StringConstantType.BareWord => stringNode.Value,
                StringConstantType.DoubleQuotedHereString => Doc.Concat(
                    "@\"",
                    Doc.LiteralLine,
                    stringNode.Value,
                    Doc.LiteralLine,
                    "\"@"
                ),
                StringConstantType.SingleQuotedHereString => Doc.Concat(
                    "@'",
                    Doc.LiteralLine,
                    stringNode.Value,
                    Doc.LiteralLine,
                    "'@"
                ),
            };
        }

        return node.Value.ToString();
    }
}
