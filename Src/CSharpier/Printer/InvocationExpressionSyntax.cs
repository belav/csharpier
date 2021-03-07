using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintInvocationExpressionSyntax(
            InvocationExpressionSyntax node)
        {
            var nodes = new List<Doc>();

            void PushNode(ExpressionSyntax expression)
            {
                if (expression is InvocationExpressionSyntax invocationExpressionSyntax)
                {
                    PushNode(invocationExpressionSyntax.Expression);
                    nodes.Add(this.PrintArgumentListSyntax(invocationExpressionSyntax.ArgumentList));
                }
                else if (expression is MemberAccessExpressionSyntax memberAccessExpressionSyntax)
                {
                    PushNode(memberAccessExpressionSyntax.Expression);
                    nodes.Add(SoftLine);
                    nodes.Add(this.PrintSyntaxToken(memberAccessExpressionSyntax.OperatorToken));
                    nodes.Add(this.Print(memberAccessExpressionSyntax.Name));
                }
                else if (expression is ThisExpressionSyntax thisExpressionSyntax)
                {
                    nodes.Add(this.PrintThisExpressionSyntax(thisExpressionSyntax));
                }
                else if (expression is BaseExpressionSyntax baseExpressionSyntax)
                {
                    nodes.Add(this.PrintBaseExpressionSyntax(baseExpressionSyntax));
                }
                else if (expression is IdentifierNameSyntax identifierNameSyntax)
                {
                    nodes.Add(this.PrintIdentifierNameSyntax(identifierNameSyntax));
                }
                else if (expression is ObjectCreationExpressionSyntax objectCreationExpressionSyntax)
                {
                    nodes.Add(this.PrintObjectCreationExpressionSyntax(objectCreationExpressionSyntax));
                }
                else
                {
                    throw new Exception(expression.GetType().ToString());
                }
            }
            
            PushNode(node);

            return Group(nodes.First(), Indent(nodes.Skip(1).ToArray()));
        }
    }
}
