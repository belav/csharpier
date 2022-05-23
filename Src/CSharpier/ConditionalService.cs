namespace CSharpier;

public class ConditionalService
{
    // TODO this can probably take a SyntaxTree to make it more efficient
    public static List<string[]> GetSymbolSets(string code)
    {
        var tree = CSharpSyntaxTree.ParseText(code);

        var customWalker = new CustomWalker();
        customWalker.Visit(tree.GetRoot());

        return customWalker.GetSymbolSets();
    }

    private class CustomWalker : CSharpSyntaxWalker
    {
        public CustomWalker() : base(SyntaxWalkerDepth.Trivia) { }

        private List<string[]> symbolSets = new();

        public List<string[]> GetSymbolSets()
        {
            return this.symbolSets;
        }

        // TODO test with real code!!!
        // TODO could we optimize even more? do we need to visit trailing trivia?
        public override void VisitLeadingTrivia(SyntaxToken token)
        {
            if (token.HasLeadingTrivia)
            {
                foreach (var tr in token.LeadingTrivia)
                {
                    if (
                        tr.RawSyntaxKind()
                        is SyntaxKind.IfDirectiveTrivia
                            or SyntaxKind.ElifDirectiveTrivia
                            or SyntaxKind.ElseDirectiveTrivia
                            or SyntaxKind.EndIfDirectiveTrivia
                    )
                    {
                        this.Visit((CSharpSyntaxNode)tr.GetStructure()!);
                    }
                }
            }
        }

        public override void VisitIfDirectiveTrivia(IfDirectiveTriviaSyntax node)
        {
            DoThing(node.Condition);
        }

        public override void VisitElifDirectiveTrivia(ElifDirectiveTriviaSyntax node)
        {
            DoThing(node.Condition);
        }

        // TODO make use of the walker itself for this?
        // TODO ( )
        // TODO ! !
        // TODO mix of && and ||
        // TODO nested ifs
        // TODO ideally we narrow down to the best set of symbols to limit the formatting passes
        private void DoThing(ExpressionSyntax expression)
        {
            if (expression is IdentifierNameSyntax identifierNameSyntax)
            {
                this.symbolSets.Add(new[] { identifierNameSyntax.Identifier.ToString() });
            }
            else if (expression is PrefixUnaryExpressionSyntax prefixUnaryExpressionSyntax)
            {
                if (
                    prefixUnaryExpressionSyntax.OperatorToken.RawSyntaxKind()
                    is SyntaxKind.ExclamationToken
                )
                {
                    if (
                        prefixUnaryExpressionSyntax.Operand
                        is IdentifierNameSyntax identifierNameSyntax2
                    )
                    {
                        this.symbolSets.Add(Array.Empty<string>());
                    }
                }
                else
                {
                    Console.WriteLine("Can't handle " + prefixUnaryExpressionSyntax.OperatorToken);
                }
            }
            else if (expression is BinaryExpressionSyntax binaryExpressionSyntax)
            {
                throw new Exception("BINARY EXPRESSIONS!");
            }
            else
            {
                Console.WriteLine("Can't handle " + expression.GetType());
            }
        }

        public override void VisitElseDirectiveTrivia(ElseDirectiveTriviaSyntax node)
        {
            // TODO
        }

        public override void VisitEndIfDirectiveTrivia(EndIfDirectiveTriviaSyntax node)
        {
            // TODO
        }
    }
}
