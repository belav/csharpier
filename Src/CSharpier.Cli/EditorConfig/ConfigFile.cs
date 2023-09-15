namespace CSharpier.Cli.EditorConfig;

internal class ConfigFile
{
    public required IReadOnlyCollection<Section> Sections { get; init; }
    public bool IsRoot { get; init; }
}
