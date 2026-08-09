using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class IncompleteMember
{
    public static Doc Print(IncompleteMemberSyntax node, CSharpPrintingContext context)
    {
        return string.Empty;
    }
}
