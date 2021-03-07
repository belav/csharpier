using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public class NodeOrToken
    {
        public CSharpSyntaxNode Node { get; set; }
        public SyntaxToken? Token { get; set; }
        public Doc Doc { get; set; }
    }
    
    public partial class Printer
    {
        private Doc PrintInvocationExpressionSyntax(
            InvocationExpressionSyntax node)
        {
            var nodeOrTokens = new List<NodeOrToken>();

            void PushToken(SyntaxToken syntaxToken)
            {
                nodeOrTokens.Add(new NodeOrToken { Token = syntaxToken });
            }

            void PushNode(CSharpSyntaxNode syntaxNode)
            {
                nodeOrTokens.Add(new NodeOrToken { Node = syntaxNode });
            }
            
            void PushDoc(Doc doc)
            {
                nodeOrTokens.Add(new NodeOrToken { Doc = doc });
            }
            
            void Traverse(ExpressionSyntax expression)
            {
                if (expression is InvocationExpressionSyntax invocationExpressionSyntax)
                {
                    Traverse(invocationExpressionSyntax.Expression);
                    PushNode(invocationExpressionSyntax.ArgumentList);
                }
                else if (expression is MemberAccessExpressionSyntax memberAccessExpressionSyntax)
                {
                    Traverse(memberAccessExpressionSyntax.Expression);
                    PushDoc(SoftLine);
                    PushToken(memberAccessExpressionSyntax.OperatorToken);
                    PushNode(memberAccessExpressionSyntax.Name);
                }
                else
                {
                    PushNode(expression);
                }
            }
            
            Traverse(node);

            var parts = new Parts();
            foreach (var nodeOrToken in nodeOrTokens)
            {
                if (nodeOrToken.Token != null)
                {
                    parts.Push(this.PrintSyntaxToken(nodeOrToken.Token.Value));
                }
                else if (nodeOrToken.Doc != null)
                {
                    parts.Push(nodeOrToken.Doc);
                }
                else
                {
                    parts.Push(this.Print(nodeOrToken.Node));
                }
            }
            // TODO GH-7 prettier has pretty complex logic for how to print this, which I think is what we need.
            // the logic for where to put groups to get line breaks needs to happen here, it can't happen in the nodes below this.
            return Group(parts.First(), Indent(parts.Skip(1).ToArray()));
        }
    }
}
