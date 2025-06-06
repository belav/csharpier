using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp;

internal class PreprocessorSymbols : CSharpSyntaxWalker
{
    private readonly List<string[]> symbolSets = [];
    private readonly HashSet<string> squashedSymbolSets = [];
    private SymbolContext CurrentContext = new()
    {
        ParentContext = new SymbolContext { ParentContext = null! },
    };

    private PreprocessorSymbols()
        : base(SyntaxWalkerDepth.Trivia) { }

    public static List<string[]> GetSets(string code)
    {
        return GetSets(CSharpSyntaxTree.ParseText(code));
    }

    public static List<string[]> GetSets(SyntaxTree syntaxTree)
    {
        return new PreprocessorSymbols().GetSymbolSets(syntaxTree);
    }

    private List<string[]> GetSymbolSets(SyntaxTree syntaxTree)
    {
        this.Visit(syntaxTree.GetRoot());

        return this.symbolSets;
    }

    public override void VisitLeadingTrivia(SyntaxToken token)
    {
        if (!token.HasLeadingTrivia)
        {
            return;
        }

        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (var syntaxTrivia in token.LeadingTrivia)
        {
            if (
                syntaxTrivia.RawSyntaxKind()
                is SyntaxKind.IfDirectiveTrivia
                    or SyntaxKind.ElifDirectiveTrivia
                    or SyntaxKind.ElseDirectiveTrivia
                    or SyntaxKind.EndIfDirectiveTrivia
            )
            {
                this.Visit((CSharpSyntaxNode)syntaxTrivia.GetStructure()!);
            }
        }
    }

    public override void VisitIfDirectiveTrivia(IfDirectiveTriviaSyntax node)
    {
        // TODO in this or the elif, if node.Condition is IdentifierNameSyntax, we know the symbol already
        // and don't need to parse it, but there isn't a good way to deal with that
        this.CurrentContext = new SymbolContext { ParentContext = this.CurrentContext };

        this.ParseExpression(node.Condition.ToFullString());
    }

    public override void VisitElifDirectiveTrivia(ElifDirectiveTriviaSyntax node)
    {
        this.ParseExpression(node.Condition.ToFullString());
    }

    public override void VisitElseDirectiveTrivia(ElseDirectiveTriviaSyntax node)
    {
        var allParameters = this
            .CurrentContext.BooleanExpressions.SelectMany(o => o.Parameters)
            .Distinct()
            .ToList();
        var combinations = GenerateCombinations(allParameters);
        var functions = this.CurrentContext.BooleanExpressions.Select(o => o.Function).ToList();

        var combination = combinations.FirstOrDefault(combination =>
            functions.All(o => !o(combination))
        );

        if (combination == null)
        {
            return;
        }

        // TODO it would be more efficient to not add a new boolean expression, because we know which
        // symbols we need
        this.CurrentContext.BooleanExpressions.Add(
            new BooleanExpression
            {
                Parameters = combination.Where(o => o.Value).Select(o => o.Key).ToList(),
                Function = o => o.All(p => p.Value),
            }
        );
    }

    public override void VisitEndIfDirectiveTrivia(EndIfDirectiveTriviaSyntax node)
    {
        var parentSymbols = Array.Empty<string>();
        if (this.CurrentContext.ParentContext.BooleanExpressions.Count != 0)
        {
            parentSymbols = GetSymbols(this.CurrentContext.ParentContext.BooleanExpressions.Last());
        }

        foreach (var booleanExpression in this.CurrentContext.BooleanExpressions)
        {
            var symbolSet = GetSymbols(booleanExpression)
                .Concat(parentSymbols)
                .Distinct()
                .ToArray();

            if (symbolSet.Length <= 0)
            {
                continue;
            }

            var squashedSymbolSet = string.Join(",", symbolSet);
            if (!this.squashedSymbolSets.Contains(squashedSymbolSet))
            {
                this.symbolSets.Add(symbolSet);
                this.squashedSymbolSets.Add(squashedSymbolSet);
            }
        }

        this.CurrentContext = this.CurrentContext.ParentContext;
    }

    private static string[] GetSymbols(BooleanExpression booleanExpression)
    {
        // TODO some type of caching on finding the symbols from the expression would speed things up
        // maybe we can solve these when constructing them, so they are all stored in the same spot
        // but the else works a bit different
        var combinations = GenerateCombinations(booleanExpression.Parameters);

        var possibleParameters = combinations.FirstOrDefault(possibleParameters =>
            booleanExpression.Function(possibleParameters)
        );

        return possibleParameters == null
            ? []
            : possibleParameters.Where(o => o.Value).Select(o => o.Key).OrderBy(o => o).ToArray();
    }

    private void ParseExpression(string expression)
    {
        var indexOf = expression.IndexOf('/');
        if (indexOf > 0)
        {
            expression = expression[..indexOf];
        }
        var booleanExpression = BooleanExpressionParser.Parse(expression);
        this.CurrentContext.BooleanExpressions.Add(booleanExpression);
    }

    private static List<Dictionary<string, bool>> GenerateCombinations(List<string> parameterNames)
    {
        if (parameterNames.Count == 0)
        {
            return
            [
                [],
            ];
        }

        var subCombinations = GenerateCombinations(parameterNames.Skip(1).ToList());
        var combinations = new List<Dictionary<string, bool>>();

        foreach (var subCombination in subCombinations)
        {
            var falseCombination = new Dictionary<string, bool>(subCombination)
            {
                { parameterNames[0], false },
            };
            var trueCombination = new Dictionary<string, bool>(subCombination)
            {
                { parameterNames[0], true },
            };

            combinations.Add(falseCombination);
            combinations.Add(trueCombination);
        }

        return combinations;
    }

    private class SymbolContext
    {
        public required SymbolContext ParentContext { get; init; }
        public List<BooleanExpression> BooleanExpressions { get; } = [];
    }
}
