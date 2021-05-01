using System;
using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintAnonymousMethodExpressionSyntax(
            AnonymousMethodExpressionSyntax node
        ) {
            var docs = new List<Doc>
            {
                Modifiers.Print(node.Modifiers),
                Token.Print(node.DelegateKeyword)
            };

            string? groupId = null;
            if (node.ParameterList != null)
            {
                groupId = Guid.NewGuid().ToString();
                docs.Add(
                    this.PrintParameterListSyntax(node.ParameterList, groupId)
                );
            }

            docs.Add(
                groupId == null
                    ? this.PrintBlockSyntax(node.Block)
                    : this.PrintBlockSyntaxWithConditionalSpace(
                            node.Block,
                            groupId
                        )
            );

            return Doc.Concat(docs);
        }
    }
}
