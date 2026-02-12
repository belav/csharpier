namespace CSharpier.Cli;

internal static class LoggerExtensions
{
    public static (string? Path, Region? Region, string? RuleId, string? Message) ExtractState<
        TState
    >(TState state)
    {
        string? path = null;
        Region? region = null;
        string? ruleId = null;
        string? message = null;

        if (state is IEnumerable<KeyValuePair<string, object?>> keyValuePairs)
        {
            foreach (var keyValuePair in keyValuePairs)
            {
                switch (keyValuePair.Key)
                {
                    case "Path":
                        path = keyValuePair.Value?.ToString();
                        break;
                    case "Region":
                        region = keyValuePair.Value as Region;
                        break;
                    case "RuleId":
                        ruleId = keyValuePair.Value?.ToString();
                        break;
                    case "Message":
                        message = keyValuePair.Value?.ToString();
                        break;
                }
            }
        }

        return (path, region, ruleId, message);
    }
}
