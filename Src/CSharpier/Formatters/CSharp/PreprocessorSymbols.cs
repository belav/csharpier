namespace CSharpier.Formatters.CSharp;

// using an actual syntax walker doesn't handle the case of nested #ifs
// because the inner #if may not be parsed as trivia unless the proper
// preprocessor symbols are passed when parsing the syntax tree
internal class PreprocessorSymbols
{
    private readonly List<string[]> symbolSets = new();
    private List<BooleanExpression> booleanExpressions;

    public static List<string[]> GetSets(string code)
    {
        return new PreprocessorSymbols().GetSymbolSets(code);
    }

    private PreprocessorSymbols() { }

    public List<string[]> GetSymbolSets(string code)
    {
        using var reader = new StringReader(code);
        while (reader.ReadLine()?.Trim() is { } line)
        {
            // TODO we need to keep track of the current parent expression for if/elseif/else
            // that can probably be a stack? as long as we keep track of
            // all the grandparents too
            if (line.StartsWith("#if "))
            {
                this.booleanExpressions = new List<BooleanExpression>();
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
        var allParameters = this.booleanExpressions
            .SelectMany(o => o.Parameters)
            .Distinct()
            .ToList();
        var combinations = GenerateCombinations(allParameters);
        var functions = this.booleanExpressions.Select(o => o.Function).ToList();

        var combination = combinations.FirstOrDefault(
            combination => functions.All(o => !o(combination))
        );

        if (combination == null)
        {
            return;
        }

        // TODO it would be more efficient to not add a new one of these, because we know the values we ant.
        this.booleanExpressions.Add(
            new BooleanExpression
            {
                Parameters = combination.Where(o => o.Value).Select(o => o.Key).ToList(),
                Function = (o => o.All(p => p.Value))
            }
        );
    }

    private void EndIf()
    {
        foreach (var booleanExpression in this.booleanExpressions)
        {
            var combinations = GenerateCombinations(booleanExpression.Parameters);

            var possibleParameters = combinations.FirstOrDefault(
                possibleParameters => booleanExpression.Function(possibleParameters)
            );

            if (possibleParameters == null)
            {
                return;
            }

            var symbolSet = possibleParameters
                .Where(o => o.Value)
                .Select(o => o.Key)
                .OrderBy(o => o)
                .ToArray();

            if (symbolSet.Length > 0)
            {
                // TODO we can make this way more efficient
                if (this.symbolSets.All(o => string.Join(",", o) != string.Join(",", symbolSet)))
                {
                    this.symbolSets.Add(symbolSet);
                }
            }
        }
    }

    private void ParseExpression(string expression)
    {
        // TODO we can do some form of caching here, possibly with the solution instead of just the data
        var booleanExpression = BooleanExpressionParser.Parse(expression);
        this.booleanExpressions.Add(booleanExpression);
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
}
