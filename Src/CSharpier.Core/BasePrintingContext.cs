using CSharpier.Core.Utilities;

namespace CSharpier.Core;

internal abstract class BasePrintingContext
{
    public required string LineEnding { get; init; }

    private readonly Dictionary<string, int> groupNumberByValue = [];

    public string GroupFor(string value)
    {
        var number = this.groupNumberByValue.GetValueOrDefault(value, 0) + 1;
        this.groupNumberByValue[value] = number;

        return $"{value} #{number}";
    }
}
