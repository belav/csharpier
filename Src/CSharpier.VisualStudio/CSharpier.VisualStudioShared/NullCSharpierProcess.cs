namespace CSharpier.VisualStudio
{
    public class NullCSharpierProcess : ICSharpierProcess
    {
        public static NullCSharpierProcess Instance = new();

        private NullCSharpierProcess() { }

        public string Version => "NULL";
        public bool ProcessFailedToStart => false;

        public string FormatFile(string content, string fileName)
        {
            return string.Empty;
        }

        public void Dispose() { }
    }
}
