namespace CSharpier.Formatters.CSharp;

internal class ConditionalService
{
    public static List<string[]> GetSymbolSets(string code)
    {
        return GetSymbolSets(CSharpSyntaxTree.ParseText(code));
    }

    public static List<string[]> GetSymbolSets(SyntaxTree syntaxTree)
    {
        var customWalker = new CustomWalker();
        customWalker.Visit(syntaxTree.GetRoot());

        return customWalker.GetSymbolSets();
    }

    private class CustomWalker : CSharpSyntaxWalker
    {
        public CustomWalker()
            : base(SyntaxWalkerDepth.Trivia) { }

        private readonly List<string[]> symbolSets = new();

        public List<string[]> GetSymbolSets()
        {
            return this.symbolSets;
        }

        // TODO test with real code!!!
        // TODO do we need to visit trailing trivia?
        public override void VisitLeadingTrivia(SyntaxToken token)
        {
            if (!token.HasLeadingTrivia)
            {
                return;
            }

            foreach (
                var syntaxTrivia in token.LeadingTrivia.Where(
                    syntaxTrivia =>
                        syntaxTrivia.RawSyntaxKind()
                            is SyntaxKind.IfDirectiveTrivia
                                or SyntaxKind.ElifDirectiveTrivia
                                or SyntaxKind.ElseDirectiveTrivia
                                or SyntaxKind.EndIfDirectiveTrivia
                )
            )
            {
                this.Visit((CSharpSyntaxNode)syntaxTrivia.GetStructure()!);
            }
        }

        public override void VisitIfDirectiveTrivia(IfDirectiveTriviaSyntax node)
        {
            this.DoThing(node.Condition);
        }

        public override void VisitElifDirectiveTrivia(ElifDirectiveTriviaSyntax node)
        {
            this.DoThing(node.Condition);
        }

        private void DoThing(ExpressionSyntax expression)
        {
            if (expression is IdentifierNameSyntax identifierNameSyntax)
            {
                this.symbolSets.Add(new[] { identifierNameSyntax.Identifier.ToString() });
            }
            else
            {
                var (function, parameters) = BooleanExpressionParser.Parse(
                    expression.ToFullString()
                );

                var combinations = GenerateCombinations(parameters);

                var possibleParameters = combinations.FirstOrDefault(
                    possibleParameters => function(possibleParameters)
                );

                if (possibleParameters != null)
                {
                    this.symbolSets.Add(
                        possibleParameters.Where(o => o.Value).Select(o => o.Key).ToArray()
                    );
                }
            }
        }

        private static List<Dictionary<string, bool>> GenerateCombinations(
            List<string> parameterNames
        )
        {
            if (!parameterNames.Any())
            {
                return new List<Dictionary<string, bool>> { new() };
            }

            var subCombinations = GenerateCombinations(parameterNames.Skip(1).ToList());
            var combinations = new List<Dictionary<string, bool>>();

            foreach (var subCombination in subCombinations)
            {
                var falseCombination = new Dictionary<string, bool>(subCombination)
                {
                    { parameterNames[0], false }
                };
                var trueCombination = new Dictionary<string, bool>(subCombination)
                {
                    { parameterNames[0], true }
                };

                combinations.Add(falseCombination);
                combinations.Add(trueCombination);
            }

            return combinations;
        }

        public override void VisitElseDirectiveTrivia(ElseDirectiveTriviaSyntax node)
        {
            // TODO do we need to do this? there are probably some edge cases
            // IF the only way to get in the else is with a specific symbol set
        }
    }
}
