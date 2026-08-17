using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class ExpandableStringExpression
{
    public static Doc Print(ExpandableStringExpressionAst node, PrintContext context)
    {
        return node.StringConstantType switch
        {
            StringConstantType.DoubleQuoted => "\"" + node.Value + "\"",
            StringConstantType.SingleQuoted => "'" + node.Value + "'",
            StringConstantType.BareWord => node.Value,
            StringConstantType.DoubleQuotedHereString => Doc.Concat(
                "@\"",
                Doc.LiteralLine,
                node.Value,
                Doc.LiteralLine,
                "\"@"
            ),
            StringConstantType.SingleQuotedHereString => Doc.Concat(
                "@'",
                Doc.LiteralLine,
                node.Value,
                Doc.LiteralLine,
                "'@"
            ),
        };
    }
}
