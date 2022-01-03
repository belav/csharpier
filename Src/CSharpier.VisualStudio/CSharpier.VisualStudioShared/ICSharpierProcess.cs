namespace CSharpier.VisualStudio
{
    public interface ICSharpierProcess
    {
        bool CanFormat { get; }
        string FormatFile(string content, string fileName);
    }
}
