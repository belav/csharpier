namespace CSharpier.Cli.EditorConfig;

internal class ConfigFile
{
    public required List<Section> Sections { get; init; }
    public bool IsRoot { get; init; }
}
