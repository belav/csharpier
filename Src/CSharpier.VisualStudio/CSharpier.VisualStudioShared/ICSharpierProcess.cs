namespace CSharpier.VisualStudio
{
    public interface ICSharpierProcess
    {
        string FormatFile(string content, string fileName);
        void Dispose();
    }
}
