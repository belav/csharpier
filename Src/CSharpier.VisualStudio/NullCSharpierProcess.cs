namespace CSharpier.VisualStudio
{
    public class NullCSharpierProcess : ICSharpierProcess
    {
        public string FormatFile(string content, string fileName)
        {
            return null;
        }
    }
}