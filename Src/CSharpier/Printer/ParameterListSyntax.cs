using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintParameterListSyntax(
            ParameterListSyntax node,
            string? groupId = null
        ) {
            var docs = new List<Doc>();
            SyntaxTokens.Print(node.OpenParenToken, docs);
            if (node.Parameters.Count > 0)
            {
                var innerDocs = new List<Doc> { Docs.SoftLine };
                this.PrintSeparatedSyntaxList(
                    node.Parameters,
                    this.PrintParameterSyntax,
                    Docs.Line,
                    innerDocs
                );

                docs.Add(Docs.Indent(innerDocs));
                docs.Add(Docs.SoftLine);
            }
            SyntaxTokens.Print(node.CloseParenToken, docs);

            return Docs.GroupWithId(groupId ?? string.Empty, docs);
        }
    }
}
