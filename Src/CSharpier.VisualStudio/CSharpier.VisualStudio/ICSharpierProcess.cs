namespace CSharpier.VisualStudio
{
    public interface ICSharpierProcess
    {
        string Version { get; }
        bool ProcessFailedToStart { get; }
        string FormatFile(string content, string fileName);
        void Dispose();
    }

    public interface ICSharpierProcess2 : ICSharpierProcess
    {
        FormatFileResult? formatFile(FormatFileParameter parameter);
    }

    public class FormatFileParameter
    {
        public string fileContents = string.Empty;
        public string fileName = string.Empty;
    }

    public class FormatFileResult
    {
        public string formattedFile = string.Empty;
        public Status status;
        public string errorMessage = string.Empty;
    }

    public enum Status
    {
        Formatted,
        Ignored,
        Failed,
        UnsupportedFile,
    }
}
