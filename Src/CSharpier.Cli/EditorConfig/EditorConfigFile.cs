namespace CSharpier.Cli.EditorConfig;

internal class EditorConfigFile
{
    public required IReadOnlyCollection<Section> Sections { get; init; }
    public bool IsRoot { get; init; }
}
