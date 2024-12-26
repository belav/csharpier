namespace CSharpier.FakeGenerators;

public class CodeContext(string folder)
{
    public string Folder { get; set; } = folder;

    public void AddSource(string name, string source)
    {
        File.WriteAllText(Path.Combine(this.Folder, name + ".cs"), source);
    }
}
