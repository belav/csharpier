namespace CSharpier.VisualStudio
{
    public class NullCSharpierProcess : ICSharpierProcess
    {
        public bool CanFormat => false;

        public string FormatFile(string content, string fileName)
        {
            return null;
        }
    }
}