using System;
using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class AnonymousMethodExpression
    {
        public static Doc Print(AnonymousMethodExpressionSyntax node)
        {
            var docs = new List<Doc>
            {
                Modifiers.Print(node.Modifiers),
                Token.Print(node.DelegateKeyword)
            };

            string? groupId = null;
            if (node.ParameterList != null)
            {
                groupId = Guid.NewGuid().ToString();
                docs.Add(ParameterList.Print(node.ParameterList, groupId));
            }

            docs.Add(
                groupId == null
                    ? Block.Print(node.Block)
                    : Block.PrintWithConditionalSpace(node.Block, groupId)
            );

            return Doc.Concat(docs);
        }
    }
}
