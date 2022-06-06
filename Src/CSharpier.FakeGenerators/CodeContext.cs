namespace CSharpier.FakeGenerators;

public class CodeContext
{
    public string Folder { get; set; }

    public CodeContext(string folder)
    {
        this.Folder = folder;
    }

    public void AddSource(string name, string source)
    {
        File.WriteAllText(Path.Combine(this.Folder, name + ".cs"), source);
    }
}
