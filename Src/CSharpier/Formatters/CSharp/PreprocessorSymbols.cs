namespace CSharpier.Formatters.CSharp;

// TODO the current method pulls in #endif from verbatim and literal strings, ugh
// we can always catch and warn on things, and deal with it later?
// we could also handle the #ifndef thing
// and then do the try catch in a smaller area, to still handle some of the symbols in the relevant files

// using an actual syntax walker doesn't handle the case of nested #ifs
// because the inner #if may not be parsed as trivia unless the proper
// preprocessor symbols are passed when parsing the syntax tree
internal class PreprocessorSymbols
{
    private readonly List<string[]> symbolSets = new();
    private readonly HashSet<string> squashedSymbolSets = new();
    private SymbolContext CurrentContext =
        new() { ParentContext = new SymbolContext { ParentContext = null! } };

    private PreprocessorSymbols() { }

    public static List<string[]> GetSets(string code)
    {
        return new PreprocessorSymbols().GetSymbolSets(code);
    }

    private List<string[]> GetSymbolSets(string code)
    {
        using var reader = new StringReader(code);
        while (reader.ReadLine()?.Trim() is { } line)
        {
            if (line.StartsWith("#if "))
            {
                this.CurrentContext = new SymbolContext { ParentContext = this.CurrentContext };

                this.ParseExpression(line[4..]);
            }
            else if (line.StartsWith("#elif "))
            {
                this.ParseExpression(line[6..]);
            }
            else if (line == "#else")
            {
                this.Else();
            }
            else if (line.StartsWith("#endif"))
            {
                this.EndIf();
            }
        }

        return this.symbolSets;
    }

    private void Else()
    {
        var allParameters = this.CurrentContext.booleanExpressions
            .SelectMany(o => o.Parameters)
            .Distinct()
            .ToList();
        var combinations = GenerateCombinations(allParameters);
        var functions = this.CurrentContext.booleanExpressions.Select(o => o.Function).ToList();

        var combination = combinations.FirstOrDefault(
            combination => functions.All(o => !o(combination))
        );

        if (combination == null)
        {
            return;
        }

        // TODO it would be more efficient to not add a new boolean expression, because we know which
        // symbols we need
        this.CurrentContext.booleanExpressions.Add(
            new BooleanExpression
            {
                Parameters = combination.Where(o => o.Value).Select(o => o.Key).ToList(),
                Function = o => o.All(p => p.Value)
            }
        );
    }

    private void EndIf()
    {
        var parentSymbols = Array.Empty<string>();
        // TODO this only works one level deep
        // TODO this also assumes the last expression added is the one we want
        if (this.CurrentContext.ParentContext.booleanExpressions.Any())
        {
            parentSymbols = GetSymbols(this.CurrentContext.ParentContext.booleanExpressions.Last());
        }

        foreach (var booleanExpression in this.CurrentContext.booleanExpressions)
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

    private string[] GetSymbols(BooleanExpression booleanExpression)
    {
        var combinations = GenerateCombinations(booleanExpression.Parameters);

        var possibleParameters = combinations.FirstOrDefault(
            possibleParameters => booleanExpression.Function(possibleParameters)
        );

        return possibleParameters == null
            ? Array.Empty<string>()
            : possibleParameters.Where(o => o.Value).Select(o => o.Key).OrderBy(o => o).ToArray();
    }

    private void ParseExpression(string expression)
    {
        if (expression.IndexOf("/") > 0)
        {
            expression = expression[..expression.IndexOf("/")];
        }
        // TODO we can do some form of caching here, possibly with the solution instead of just the data
        var booleanExpression = BooleanExpressionParser.Parse(expression);
        this.CurrentContext.booleanExpressions.Add(booleanExpression);
    }

    private static List<Dictionary<string, bool>> GenerateCombinations(List<string> parameterNames)
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

    private class SymbolContext
    {
        public required SymbolContext ParentContext { get; init; }
        public List<BooleanExpression> booleanExpressions { get; } = new();
    }
}
